using Microsoft.EntityFrameworkCore;
using RickMortyDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RickMortyInfraestructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Character> Characters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Character>(entity =>
            {
                entity.ToTable("Characters");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.Species).HasMaxLength(100);
                entity.Property(e => e.Type).HasMaxLength(100);
                entity.Property(e => e.Gender).HasMaxLength(50);
                entity.Property(e => e.OriginName).HasMaxLength(255);
                entity.Property(e => e.OriginUrl).HasMaxLength(500);
                entity.Property(e => e.LocationName).HasMaxLength(255);
                entity.Property(e => e.LocationUrl).HasMaxLength(500);
                entity.Property(e => e.Image).HasMaxLength(500);
                entity.Property(e => e.Url).HasMaxLength(500);

                entity.Property(e => e.Episodes)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                    .HasColumnType("json");

                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.Species);
            });
        }
    }
}
