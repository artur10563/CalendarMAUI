using INFR.Migrations;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Persistence
{
    //dotnet ef migrations add InitialCreate--project INFR\INFR.csproj --startup-project RougeTracker\RougeTracker.csproj --framework net10.0 - windows10.0.19041.0
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<DailyRecord> DailyRecords { get; set; }
    }

    // For multity platform DI
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "app.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new AppDbContext(optionsBuilder.Options);
        }
    }

    // Domain object
    public class DailyRecord
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Gross { get; set; }
    }
}
