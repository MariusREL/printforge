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

// Database: SQL Server (default to localhost\\SQLEXPRESS)
var connectionString = builder.Configuration.GetConnectionString("Default")
                       ?? "Server=localhost\\SQLEXPRESS;Database=PrintforgeDb;Trusted_Connection=True;TrustServerCertificate=True;";

builder.Services.AddDbContext<PrintforgeDbContext>(options =>
    options.UseSqlServer(connectionString));

// Optional: keep file catalog registration disabled; DB is source of truth now
// var dataPath = Path.Combine(builder.Environment.ContentRootPath, "data", "models.json");
// builder.Services.AddSingleton<IModelCatalog>(_ => new FileModelCatalog(dataPath));

var app = builder.Build();

// Ensure database exists and seed initial data from data/models.json if empty
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PrintforgeDbContext>();
    await db.Database.EnsureCreatedAsync();

    if (!await db.Models.AnyAsync())
    {
        var dataPath = Path.Combine(app.Environment.ContentRootPath, "data", "models.json");
        if (File.Exists(dataPath))
        {
            var json = await File.ReadAllTextAsync(dataPath);
            var items = System.Text.Json.JsonSerializer.Deserialize<List<ModelItem>>(json, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<ModelItem>();

            var records = items.Select(m => new ModelRecord
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Likes = m.Likes,
                Image = m.Image,
                Category = m.Category,
                DateAdded = m.DateAdded
            }).ToList();

            db.Models.AddRange(records);
            await db.SaveChangesAsync();
        }
    }
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
