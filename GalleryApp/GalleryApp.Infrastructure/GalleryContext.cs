using GalleryApp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace GalleryApp.Infrastructure
{
    public class GalleryContext : DbContext
    {

        public GalleryContext(DbContextOptions<GalleryContext> options)
            : base(options)
        {
        }
        public DbSet<PhotoEntity> Photos { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<PhotoEntity>()
        //            .HasMany(g => g.Genres)
        //            .WithMany(p => p.Photos);

        //    modelBuilder.Entity<GenreEntity>()
        //            .HasMany(p => p.Photos)
        //            .WithMany(g => g.Genres);
        //}
    }
}
