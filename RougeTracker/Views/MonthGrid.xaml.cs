using RougeTracker.ViewModels;

namespace RougeTracker.Views;

public partial class MonthGrid : ContentPage
{
    private static int SAFETY_PADDING = 50;
    private static readonly int COLUMNS = 4;

    public MonthGrid(MonthGridVM viewModel)
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
        if (BindingContext is not MonthGridVM vm)
            return;

        var items = vm.Months ?.Count ?? 0;
        if (items == 0)
            return;


        var rows = Math.Ceiling(items / (double)COLUMNS);

        var availableHeight = this.Height;

        if (availableHeight <= 0)
            return;

        var cellHeight = (availableHeight - SAFETY_PADDING) / (rows + 1);

        vm.CellHeight = cellHeight;
    }
}