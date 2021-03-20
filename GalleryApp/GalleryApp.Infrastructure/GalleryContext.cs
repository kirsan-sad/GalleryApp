using GalleryApp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace GalleryApp.Infrastructure
{
    public class GalleryContext : DbContext
    {
        public DbSet<PhotoEntity> Photos { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=galleryDb;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhotoEntity>()
                    .HasMany(g => g.Genres)
                    .WithMany(p => p.Photos);
        }
    }
}
