using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Infrastructure.Services;
using RougeTracker.Messages;
using RougeTracker.Models;
using RougeTracker.Views;
using System.Collections.ObjectModel;

namespace RougeTracker.ViewModels
{


    public partial class MonthGridVM : ObservableObject
    {
        #region UI hack
        [ObservableProperty]
        public partial double CellHeight { get; set; }
        #endregion

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


            WeakReferenceMessenger.Default.Register<MonthGrossChangedMessage>(this, (r, msg) =>
            {
                var month = Months.FirstOrDefault(x => x.MonthId == msg.Month);
                if (month == null) return;

                month.Income = msg.Income;
                month.Loss = msg.Loss;
            });

        }

        [ObservableProperty]
        public partial ObservableCollection<int> Years { get; set; }

        [ObservableProperty]
        public partial object SelectedYear { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<MonthInfoObservable> Months { get; set; }

        [ObservableProperty]
        public partial object SelectedMonth { get; set; }



        partial void OnSelectedYearChanged(object oldValue, object newValue)
        {
            _ = LoadMonthsAsync((int)newValue);
        }

        private async Task LoadMonthsAsync(int year)
        {
            Months = new ObservableCollection<MonthInfoObservable>(
                (await _dbService.GetAllMonthSummaryByYearAsync(year))
                .Select(MonthInfoObservable.FromMonthInfo)
                );
        }


        [RelayCommand]
        private async Task OpenMonth(MonthInfoObservable month)
        {
            if (month is null)
                return;

            await Shell.Current.GoToAsync(
                $"{nameof(MonthDaysGrid)}?year={SelectedYear}&month={month.MonthId}");

            SelectedMonth = null; // reset to make clickable again
        }

    }
}
