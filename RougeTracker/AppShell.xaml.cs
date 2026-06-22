using RougeTracker.Views;

namespace RougeTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Application.Current?.UserAppTheme = AppTheme.Dark;

            Routing.RegisterRoute(nameof(MonthGrid), typeof(MonthGrid));
            Routing.RegisterRoute(nameof(MonthDaysGrid), typeof(MonthDaysGrid));
        }
    }
}
