using Microsoft.EntityFrameworkCore;
using filestorage.Contexts;
using filestorage.models;
using filestorage.models.DTOs;
using filestorage.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


builder.Services.AddDbContext<TrainingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<VideoService>();


builder.Services.AddCors();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();



app.MapControllers();
app.Run();


