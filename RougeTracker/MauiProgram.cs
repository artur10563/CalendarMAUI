using CommunityToolkit.Maui;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RougeTracker.ViewModels;
using RougeTracker.Views;

namespace RougeTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            #region service DI
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
                options.UseSqlite($"Data Source={dbPath}");
            });

            builder.Services.AddScoped<DbService>();
            #endregion

            #region DI for VMS
            builder.Services.AddTransient<MonthGridVM>();
            builder.Services.AddTransient<MonthGrid>();

            builder.Services.AddTransient<MonthDaysGridVM>();
            builder.Services.AddTransient<MonthDaysGrid>();
            #endregion

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();


            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            return app;
        }
    }
}
