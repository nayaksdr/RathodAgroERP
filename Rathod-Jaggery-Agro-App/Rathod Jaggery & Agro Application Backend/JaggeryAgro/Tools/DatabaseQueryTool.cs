using JaggeryAgro.Data; // your DbContext namespace
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgro.Tools
{
    public class DatabaseQueryTool
    {
        private readonly ApplicationDbContext _db;

        public string Name => "DatabaseQueryTool";
        public string Description => "Query Labor, Attendance, Advance, WeeklySalary tables";

        public DatabaseQueryTool(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<string> QueryAsync(string input)
        {
            if (input.Contains("weekly salary", StringComparison.OrdinalIgnoreCase))
            {
                var result = await _db.WeeklySalaries
                    .Include(w => w.Labor)
                    .Select(w => new
                    {
                        w.Labor.Name,
                        w.NetSalary,
                        w.WeekStart,
                        w.WeekEnd
                    })
                    .ToListAsync();

                return string.Join("\n", result.Select(r =>
                    $"{r.Name}: {r.NetSalary} (Week: {r.WeekStart:dd/MM/yyyy} - {r.WeekEnd:dd/MM/yyyy})"));
            }

            if (input.Contains("attendance", StringComparison.OrdinalIgnoreCase))
            {
                var result = await _db.Attendances
                    .Include(a => a.Labor)
                    .OrderByDescending(a => a.Date)
                    .Take(10)
                    .Select(a => new
                    {
                        a.Labor.Name,
                        a.Date,
                        a.IsPresent
                    })
                    .ToListAsync();

                return string.Join("\n", result.Select(r =>
                    $"{r.Date:dd/MM/yyyy} - {r.Name}: {(r.IsPresent ? "Present" : "Absent")}"));
            }

            return "Sorry, no relevant data found.";
        }
    }
}
