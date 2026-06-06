using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Services;
using RougeTracker.Views;
using System.Collections.ObjectModel;

namespace RougeTracker.ViewModels
{


    public partial class MonthGridVM : ObservableObject
    {
        private readonly DbService _dbService;

        private static int YEARS_RANGE = 5;
        private static int CURRENT_YEAR = DateTime.Now.Year;
        private static int START_YEAR = CURRENT_YEAR - YEARS_RANGE;
        private static int END_YEAR = CURRENT_YEAR + YEARS_RANGE;

        public MonthGridVM(DbService dbService)
        {
            _dbService = dbService;

            SelectedYear = DateTime.Now.Year;
            Years = new ObservableCollection<int>(Enumerable.Range(START_YEAR, END_YEAR - START_YEAR + 1));
        }

        [ObservableProperty]
        public partial ObservableCollection<int> Years { get; set; }

        [ObservableProperty]
        public partial object SelectedYear { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<MonthInfo> Months { get; set; }

        [ObservableProperty]
        public partial object SelectedMonth { get; set; }



        partial void OnSelectedYearChanged(object oldValue, object newValue)
        {
            _ = LoadMonthsAsync((int)newValue);
        }

        private async Task LoadMonthsAsync(int year)
        {
            Months = new ObservableCollection<MonthInfo>(
                await _dbService.GetAllMonthSummaryByYearAsync(year));
        }


        [RelayCommand]
        private async Task OpenMonth(MonthInfo month)
        {
            if (month is null)
                return;

            await Shell.Current.GoToAsync(
                $"{nameof(MonthDaysGrid)}?year={SelectedYear}&month={month.MonthId}");
        }

    }
}
