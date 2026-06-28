using CommunityToolkit.Mvvm.ComponentModel;
using INFR.DTO;
using System.Globalization;

namespace RougeTracker.Models
{
    public partial class MonthInfoObservable : ObservableObject
    {
        public int MonthId { get; set; }
        public string MonthName => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(MonthId);
        public string ShortMonthName => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(MonthId);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Gross))]
        public partial decimal Income { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Gross))]
        public partial decimal Loss { get; set; }

        public decimal Gross => Income + Loss; // + cuz Loss is negative number

        public static MonthInfoObservable FromMonthInfo(MonthInfo model)
        {
            return new MonthInfoObservable()
            {
                MonthId = model.MonthId,
                Income = model.Income,
                Loss = model.Loss
            };
        }
    }
}
