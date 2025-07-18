using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Repositories;
using VehicleServiceAPI.Services;
using VehicleServiceAPI.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Threading.RateLimiting;
using VehicleServiceAPI.Misc;
using System.Security.Claims;


Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Get configuration from appsettings.json.
var configuration = builder.Configuration;

// Register controllers.
builder.Services.AddControllers();

// Configure EF Core to use PostgreSQL.
builder.Services.AddDbContext<VehicleServiceDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// Register repository implementations.
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<BookingRepository>();
builder.Services.AddScoped<ServiceSlotRepository>();
builder.Services.AddScoped<VehicleRepository>();
builder.Services.AddScoped<InvoiceRepository>();
builder.Services.AddScoped<ImageRepository>();
builder.Services.AddScoped<RoleRepository>();

// Register service implementations.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IServiceSlotService, ServiceSlotService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var userIdentifier = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                             ?? httpContext.Request.Headers.Host.ToString();
        return RateLimitPartition.GetFixedWindowLimiter(userIdentifier, partition => new FixedWindowRateLimiterOptions
        {
            AutoReplenishment = true,
            PermitLimit = 200,
            QueueLimit = 0,
            Window = TimeSpan.FromMinutes(5)
        });
    });
});




//signalr
builder.Services.AddSignalR();

// cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5192", "http://127.0.0.1:5500", "http://127.0.0.1:4200", "http://localhost:4200", "http://localhost:51613")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configure JWT Authentication.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"])),
            ValidateLifetime = true
        };
    });

// Configure authorization policies.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
         policy.RequireRole("Admin"));
    options.AddPolicy("MechanicAccess", policy =>
         policy.RequireRole("Admin", "Mechanic"));
    options.AddPolicy("UserAccess", policy =>
         policy.RequireRole("Admin", "Mechanic", "User"));
});

// Add Swagger/OpenAPI support BEFORE building the app.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VehicleServiceAPI", Version = "v1" });
    
    // Define the Bearer auth scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                      "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer abcdefgh12345\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Optionally apply pending migrations on startup.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<VehicleServiceDbContext>();
    dbContext.Database.Migrate();
}

// Configure the middleware pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();  
app.UseSerilogRequestLogging();

app.MapControllers();

app.MapHub<EventHub>("/eventhub")
   .DisableRateLimiting();

app.Run();