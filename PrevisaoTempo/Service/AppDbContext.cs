using Microsoft.EntityFrameworkCore;
using PrevisaoDoTempoApp.Models;

namespace MauiAppPrevisaodoTempo.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<PrevisaoTempo> PrevisaoTempos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={AppDbPath}");
        }

        private string AppDbPath
        {
            get
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return System.IO.Path.Combine(path, "PrevisaoTempo.db");
            }
        }
    }
}
