using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backend.database;

public sealed class PrintforgeDbContext : DbContext
{
    public PrintforgeDbContext(DbContextOptions<PrintforgeDbContext> options) : base(options) {}

    public DbSet<ModelRecord> Models => Set<ModelRecord>();
    public DbSet<ModelWebpPlaceholder> ModelWebpPlaceholders => Set<ModelWebpPlaceholder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ModelRecord>(b =>
        {
            b.ToTable("Models");
            b.HasKey(m => m.Id);
            b.Property(m => m.Name).IsRequired().HasMaxLength(256);
            b.Property(m => m.Description).HasMaxLength(4096);
            b.Property(m => m.Image).HasMaxLength(1024);
            b.Property(m => m.Category).HasMaxLength(128);
            b.Property(m => m.DateAdded).HasDefaultValueSql("GETUTCDATE()");
            b.Property(m => m.Likes).HasDefaultValue(0);
        });

        modelBuilder.Entity<ModelWebpPlaceholder>(b =>
        {
            b.ToTable("ModelWebpPlaceholders");
            b.HasKey(s => s.Id);
            b.Property(s => s.FileName).HasMaxLength(512);
            b.Property(s => s.ContentType).HasMaxLength(128);

            b.HasOne(s => s.Model)
             .WithMany(m => m.WebpPlaceholders)
             .HasForeignKey(s => s.ModelId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

public sealed class ModelRecord
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Likes { get; set; }
    public string Image { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }

    public List<ModelWebpPlaceholder> WebpPlaceholders { get; set; } = new();
}

public sealed class ModelWebpPlaceholder
{
    public int Id { get; set; }
    public int ModelId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "image/webp"; // WebP MIME
    public byte[] Data { get; set; } = Array.Empty<byte>();

    public ModelRecord? Model { get; set; }
}
