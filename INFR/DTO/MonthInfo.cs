using System.Globalization;

namespace INFR.DTO
{
    public class MonthInfo()
    {
        public int MonthId { get; set; }
        public string MonthName => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(MonthId);
        public string ShortMonthName => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(MonthId);
        public decimal Income { get; set; }
        public decimal Loss { get; set; }
        public decimal Gross => Income - Loss;
    }
}
