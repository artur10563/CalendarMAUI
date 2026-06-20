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
        public int? Id { get; init; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
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
                    Id = day?.Id,
                    Day = d,
                    Month = month,
                    Year = year,
                    Gross = day?.Gross ?? 0,
                };
            }).ToList();

            return allDays;
        }

        public async Task<int> UpdateDaysAsync(List<DayInfo> days)
        {
            if (days == null || days.Count == 0)
                return 0;

            var year = days[0].Year;
            var month = days[0].Month;

            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var existing = await _db.DailyRecords
                .Where(x => x.Date >= startDate && x.Date < endDate)
                .ToListAsync();

            var existingMap = existing
                .ToDictionary(x => x.Date.Day);

            foreach (var dto in days)
            {
                var date = new DateTime(dto.Year, dto.Month, dto.Day);

                if (existingMap.TryGetValue(dto.Day, out var entity))
                {
                    entity.Gross = dto.Gross;
                    _db.DailyRecords.Update(entity);
                }
                else
                {
                    _db.DailyRecords.Add(new DailyRecord
                    {
                        Date = date,
                        Gross = dto.Gross
                    });
                }
            }

            return await _db.SaveChangesAsync();
        }
    }


}
