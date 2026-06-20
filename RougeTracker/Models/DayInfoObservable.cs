using CommunityToolkit.Mvvm.ComponentModel;
using Infrastructure.Services;

namespace RougeTracker.Models
{
    public partial class DayInfoObservable : ObservableObject
    {
        private bool _isInitializing;

        public int? Id { get; init; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        [ObservableProperty]
        public partial int Gross { get; set; }

        [ObservableProperty]
        public partial bool IsDirty { get; set; }

        public static DayInfoObservable FromModel(DayInfo model)
        {
            var item = new DayInfoObservable
            {
                _isInitializing = true,

                Id = model.Id,
                Day = model.Day,
                Month = model.Month,
                Year = model.Year,
                Gross = model.Gross
            };

            item._isInitializing = false;
            return item;
        }

        public DayInfo ToModel()
        {
            return new DayInfo()
            {
                Id = this.Id,
                Day = this.Day,
                Month = this.Month,
                Year = this.Year,
                Gross = this.Gross
            };
        }

        partial void OnGrossChanged(int oldValue, int newValue)
        {
            if (_isInitializing)
                return;

            if (oldValue == newValue)
                return;

            IsDirty = true;
        }
    }
}