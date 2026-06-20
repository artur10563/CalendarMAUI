using CommunityToolkit.Mvvm.ComponentModel;
using Infrastructure.Services;
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
        public partial int Income { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Gross))]
        public partial int Loss { get; set; }

        public int Gross => Income - Loss;

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
