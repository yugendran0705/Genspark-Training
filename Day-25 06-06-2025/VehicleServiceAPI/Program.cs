using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.Context;
using VehicleServiceAPI.Repositories;
using VehicleServiceAPI.Services;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

// Add configuration (from appsettings.json)
var configuration = builder.Configuration;

// Register controllers
builder.Services.AddControllers();

// Configure EF Core to use PostgreSQL
builder.Services.AddDbContext<VehicleServiceDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// Register repository implementations (individual ones, not using IRepository)
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<BookingRepository>();
builder.Services.AddScoped<ServiceSlotRepository>();
builder.Services.AddScoped<VehicleRepository>();
builder.Services.AddScoped<InvoiceRepository>();
builder.Services.AddScoped<ImageRepository>();

// Register service implementations
builder.Services.AddScoped<AuthService>();
// Add other service registrations as needed
// builder.Services.AddScoped<UserService>(); etc.

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Optionally, apply pending migrations on startup and/or seed the database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<VehicleServiceDbContext>();
    dbContext.Database.Migrate();
    // Optionally call a database initializer here, e.g.,
    // VehicleServiceDbInitializer.Initialize(dbContext);
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
