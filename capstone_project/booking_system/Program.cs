using BookingSystem.Contexts;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Interfaces;
using BookingSystem.Repositories;
using BookingSystem.Models;
using BookingSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookingSystem.Misc;
using Serilog;
using System.Threading.RateLimiting;


Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();


// builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    }); 


//db context
builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//services 
builder.Services.AddTransient<IRepository<string, User>, UserRepository>();
builder.Services.AddTransient<IRepository<string, Admin>, AdminRepository>();
builder.Services.AddTransient<IRepository<string, Customer>, CustomerRepository>();
builder.Services.AddTransient<IRepository<string, Event>, EventRepository>();
builder.Services.AddTransient<IRepository<string, Category>, CategoryRepository>();
builder.Services.AddTransient<IRepository<int, Ticket>, TicketRepository>();
builder.Services.AddTransient<IRepository<string, Wallet>, WalletRepository>();

builder.Services.AddTransient<IEncryptionService, EncryptionService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<ITicketService, TicketService>();
builder.Services.AddTransient<IEventService, EventService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddSingleton<ITokenCacheService, InMemoryTokenCacheService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IWalletService, WalletService>();


builder.Services.AddHttpContextAccessor();

//rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
});



//signalr
builder.Services.AddSignalR();

#region CORS
builder.Services.AddCors(options=>{
    options.AddDefaultPolicy(policy=>{
        policy.WithOrigins("http://localhost:8080", "http://localhost:4200","http://localhost:54999","http://127.0.0.1:4200","http://127.0.0.1:4201")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
#endregion

//jwt auth
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Your API",
        Version = "v1"
    });

    // Add JWT Auth to Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token.\nExample: `Bearer abcdef12345`"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtTokenKey"]))
                    };
                });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
    db.Database.Migrate(); // This applies any pending migrations
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

//middleware
app.UseMiddleware<TokenValidationMiddleware>();
app.UseSerilogRequestLogging();
app.MapControllers();


app.MapHub<EventHub>("/eventhub");
app.Run();

//Host=74.235.137.12;Port=5432;Username=postgres;Password=mac151203;Database=bookingdb_az


