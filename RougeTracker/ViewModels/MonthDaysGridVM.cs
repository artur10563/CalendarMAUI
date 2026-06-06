using CommunityToolkit.Mvvm.ComponentModel;
using Infrastructure.Services;
using System.Collections.ObjectModel;

namespace RougeTracker.ViewModels
{
    [QueryProperty(nameof(Year), "year")]
    [QueryProperty(nameof(Month), "month")]
    public partial class MonthDaysGridVM : ObservableObject
    {
        private readonly DbService _db;

        public MonthDaysGridVM(DbService db)
        {
            _db = db;
        }

        [ObservableProperty]
        public partial int Year { get; set; }

        [ObservableProperty]
        public partial int Month { get; set; }
        
        [ObservableProperty]
        public partial ObservableCollection<DayInfo> Days { get; set; }

        partial void OnMonthChanged(int value)
        {
            _ = Load();
        }

        partial void OnYearChanged(int value)
        {
            _ = Load();
        }

        private async Task Load()
        {
            var data = await _db.GetMonthDaysAsync(Year, Month);
            Days = new ObservableCollection<DayInfo>(data);
        }
    }
}
