using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace GalleryApp.Infrastructure
{
    class SampleContextFactory : IDesignTimeDbContextFactory<GalleryContext>
    {
        public GalleryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GalleryContext>();

            // получаем конфигурацию из файла appsettings.json
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();

            // получаем строку подключения из файла appsettings.json
            string connectionString = config.GetConnectionString("TestConnection");
            optionsBuilder.UseSqlServer(connectionString, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new GalleryContext(optionsBuilder.Options);
        }
    }
}
