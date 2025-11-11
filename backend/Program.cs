using backend; // for AddConfiguredCors extension
using backend.services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using backend.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS from configuration
builder.Services.AddConfiguredCors(builder.Configuration);

// Register services
var dataPath = Path.Combine(builder.Environment.ContentRootPath, "data", "models.json");
builder.Services.AddSingleton<IModelCatalog>(_ => new FileModelCatalog(dataPath));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS before mapping controllers
app.UseCors(builder.Configuration["Cors:PolicyName"] ?? "AllowFrontend");

app.MapControllers();

app.Run();
