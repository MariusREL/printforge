using backend.services;
using backend.Extensions;
using backend.database;
using Microsoft.EntityFrameworkCore;
using backend.models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS from configuration
builder.Services.AddConfiguredCors(builder.Configuration);

// Database: SQL Server via appsettings.json only (no hardcoded fallback)
var connectionString = builder.Configuration.GetConnectionString("Default")
                       ?? throw new InvalidOperationException("ConnectionStrings:Default is missing in configuration.");

builder.Services.AddDbContext<PrintforgeDbContext>(options => options.UseSqlServer(connectionString));

// Catalog seeded from new path backend/data/database_seeding/models.json
var seedJsonPath = Path.Combine(builder.Environment.ContentRootPath, "data", "database_seeding", "models.json");
builder.Services.AddSingleton<IModelCatalog>(_ => new FileModelCatalog(seedJsonPath));

// Database initializer service
builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
// Likes & rate limiting services
builder.Services.AddScoped<backend.services.ILikesService, backend.services.LikesService>();
builder.Services.AddSingleton<backend.services.ILikeRateLimiter, backend.services.LikeRateLimiter>();
// Models query service
builder.Services.AddScoped<backend.services.IModelsQueryService, backend.services.ModelsQueryService>();

var app = builder.Build();

// Initialize database (drop all tables, recreate, seed from catalog & webp placeholders)
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    await initializer.InitializeAsync();
}

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
