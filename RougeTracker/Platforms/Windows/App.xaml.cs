using Microsoft.UI.Xaml;
using XAML = Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RougeTracker.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            var resources = XAML.Application.Current.Resources;

            // Default hover of list items
            resources["ListViewItemBackgroundPointerOver"] = new XAML.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
            resources["ListViewItemBackgroundPressed"] = new XAML.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
            resources["ListViewItemBackgroundSelected"] = new XAML.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
            resources["ListViewItemBackgroundSelectedPointerOver"] = new XAML.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
            resources["ListViewItemBackgroundSelectedPressed"] = new XAML.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);


            // Entry remove default underline
            resources["TextControlBorderThemeThickness"] = new XAML.Thickness(1);
            resources["TextControlBorderThemeThicknessFocused"] = new XAML.Thickness(1);
            resources["TextControlBorderBrush"] = new XAML.Media.SolidColorBrush(Microsoft.UI.Colors.Gray);
            resources["TextControlBorderBrushFocused"] = new XAML.Media.SolidColorBrush(Microsoft.UI.Colors.Gray);
        }
    }
}
