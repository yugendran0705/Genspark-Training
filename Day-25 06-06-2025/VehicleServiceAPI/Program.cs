using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Repositories;
using VehicleServiceAPI.Services;

var builder = WebApplication.CreateBuilder(args);

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
// (If you are following best practice, your controllers should depend on services,
// and services should depend on repositories. However, you can register and use both if needed.)
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
// Register any additional services as needed...

// Add Swagger/OpenAPI support.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Optionally apply pending migrations on startup.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<VehicleServiceDbContext>();
    dbContext.Database.Migrate();
    // Optionally seed the database if necessary.
}

// Configure the middleware pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
