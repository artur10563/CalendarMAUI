using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Infrastructure.Services;
using RougeTracker.Messages;
using RougeTracker.Models;
using System.Collections.ObjectModel;

namespace RougeTracker.ViewModels
{
    [QueryProperty(nameof(Year), "year")]
    [QueryProperty(nameof(Month), "month")]


    public partial class MonthDaysGridVM : ObservableObject
    {
        #region UI hack
        [ObservableProperty]
        public partial double CellHeight { get; set; }
        #endregion

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
        public partial ObservableCollection<DayInfoObservable> Days { get; set; }


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
            var data = (await _db.GetMonthDaysAsync(Year, Month)).Select(DayInfoObservable.FromModel);
            Days = new ObservableCollection<DayInfoObservable>(data);
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            try
            {
                if (Days == null || Days.Count == 0)
                    return;

                var changedDays = Days.Where(x => x.IsDirty).Select(x => x.ToModel()).ToList();
                var recordsUpdated = await _db.UpdateDaysAsync(changedDays);

                if (recordsUpdated > 0)
                {
                    var loss = Days.Sum(x => x.Gross < 0 ? x.Gross : 0);
                    var income = Days.Sum(x => x.Gross > 0 ? x.Gross : 0);

                    WeakReferenceMessenger.Default.Send(new MonthGrossChangedMessage()
                    {
                        Month = Month,
                        Year = Year,
                        Income = income,
                        Loss = loss
                    });

                    await Shell.Current.DisplayAlertAsync("Success", "Data saved successfully", "Ok");
                    return;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Send this to 10563: {ex.Message}", "Ok");
            }
        }
    }
}
