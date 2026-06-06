using RougeTracker.ViewModels;

namespace RougeTracker.Views;

public partial class MonthDaysGrid : ContentPage
{
    public MonthDaysGrid(MonthDaysGridVM viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}