using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Infrastructure.Services
{
    // DTOs
    public class MonthInfo()
    {
        public int MonthId { get; set; }
        public string MonthName => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(MonthId);
        public string ShortMonthName => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(MonthId);
        public int Income { get; set; }
        public int Loss { get; set; }
        public int Gross => Income - Loss;
    }

    public class DayInfo()
    {
        public int Day { get; set; }
        public int Gross { get; set; }
    }

    //DTOs end

    public class DbService(AppDbContext _db)
    {
        public async Task<List<MonthInfo>> GetAllMonthSummaryByYearAsync(int year)
        {
            var dbData = await
                _db.DailyRecords
                .Where(x => x.Date.Year == year)
                .GroupBy(x => x.Date.Month)
                .Select(g => new MonthInfo
                {
                    MonthId = g.Key,
                    Income = g.Sum(x => x.Gross > 0 ? x.Gross : 0),
                    Loss = g.Sum(x => x.Gross < 0 ? x.Gross : 0)
                })
                .ToListAsync();

            var allMonthData = Enumerable.Range(1, 12).Select(m =>
            {
                var data = dbData.FirstOrDefault(x => x.MonthId == m);

                return new MonthInfo
                {
                    MonthId = m,
                    Income = data?.Income ?? 0,
                    Loss = data?.Loss ?? 0
                };
            }).ToList();

            return allMonthData;
        }


        public async Task<List<DayInfo>> GetMonthDaysAsync(int year, int month)
        {
            var daysCount = CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(year, month);

            var dbDays = await _db.DailyRecords
                .Where(x => x.Date.Year == year && x.Date.Month == month)
                .ToListAsync();

            var allDays = Enumerable.Range(1, daysCount).Select(d =>
            {
                var day = dbDays.FirstOrDefault(x => x.Date.Day == d);

                return new DayInfo
                {
                    Day = d,
                    Gross = day?.Gross ?? 0,
                };
            }).ToList();

            return allDays;
        }
    }
}
