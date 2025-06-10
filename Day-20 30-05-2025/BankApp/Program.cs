using BankApp.Interfaces;
using BankApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure HttpClient using the BaseUrl from configuration.
builder.Services.AddHttpClient<IFaqService, FaqService>(client =>
{
    var baseUrl = builder.Configuration["GeminiApi:BaseUrl"];
    if (string.IsNullOrEmpty(baseUrl))
        throw new Exception("GeminiApi:BaseUrl must be configured in appsettings.json.");
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
