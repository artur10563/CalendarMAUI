using RougeTracker.ViewModels;

namespace RougeTracker.Views;

public partial class MonthDaysGrid : ContentPage
{
    private const int SAFETY_PADDING = 50;
    public MonthDaysGrid(MonthDaysGridVM viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        ApplyChipSizing();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ApplyChipSizing();
    }

    private void ApplyChipSizing()
    {
        if (BindingContext is not MonthDaysGridVM vm)
            return;

        var items = vm.Days?.Count ?? 0;
        if (items == 0)
            return;

        const int columns = 5;

        var rows = Math.Ceiling(items / (double)columns);

        var availableHeight = this.Height;

        if (availableHeight <= 0)
            return;

        var cellHeight = (availableHeight - SAFETY_PADDING) / ( rows + 1 );

        vm.CellHeight = cellHeight;
    }
}