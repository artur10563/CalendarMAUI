using CommunityToolkit.Mvvm.ComponentModel;
using RougeTracker.ViewModels;
using System.Collections.ObjectModel;

namespace RougeTracker.Views;

public partial class MonthGrid : ContentPage
{
    public MonthGrid(MonthGridVM viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}