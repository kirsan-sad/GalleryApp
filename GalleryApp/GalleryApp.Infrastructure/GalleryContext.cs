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
    }
}
