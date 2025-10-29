using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get all with Labor included
        public async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await _context.Attendances
                                 .Include(a => a.Labor)
                                 .ToListAsync();
        }

        // ✅ Get by Id
        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await _context.Attendances.FindAsync(id);
        }

        // ✅ Add
        public async Task AddAsync(Attendance attendance)
        {
            await _context.Attendances.AddAsync(attendance);
            await SaveAsync();
        }

        // ✅ Update
        public async Task UpdateAsync(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
            await SaveAsync();
        }

        // ✅ Delete
        public async Task DeleteAsync(int id)
        {
            var att = await GetByIdAsync(id);
            if (att != null)
            {
                _context.Attendances.Remove(att);
                await SaveAsync();
            }
        }

        // ✅ Save changes
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        // ✅ Get by Labor and Date Range
        public async Task<IEnumerable<Attendance>> GetByLaborIdAndDateRangeAsync(int laborId, DateTime start, DateTime end)
        {
            return await _context.Attendances
                                 .Where(a => a.LaborId == laborId &&
                                             a.Date >= start &&
                                             a.Date <= end &&
                                             a.IsPresent)
                                 .ToListAsync();
        }

        // ✅ Get Attendance between dates
        public async Task<IEnumerable<Attendance>> GetAttendanceBetweenDatesAsync(DateTime start, DateTime end)
        {
            return await _context.Attendances
                                 .Where(a => a.Date >= start &&
                                             a.Date <= end &&
                                             a.IsPresent)
                                 .ToListAsync();
        }

        // ✅ Get by Labor
        public async Task<IEnumerable<Attendance>> GetByLaborAsync(int laborId)
        {
            return await _context.Attendances
                                 .Where(a => a.LaborId == laborId)
                                 .ToListAsync();
        }

        // ✅ Get by Labor in Range
        public async Task<IEnumerable<Attendance>> GetByLaborInRangeAsync(int laborId, DateTime from, DateTime to)
        {
            return await _context.Attendances
                                 .Where(a => a.LaborId == laborId && a.Date >= from && a.Date <= to)
                                 .ToListAsync();
        }
        public async Task<List<Attendance>> GetByLaborIdAndDateRangeAsync(int laborId, DateTime? from, DateTime? to)
        {
            var query = _context.Attendances.Where(a => a.LaborId == laborId);

            if (from.HasValue)
                query = query.Where(a => a.Date >= from.Value);

            if (to.HasValue)
                query = query.Where(a => a.Date <= to.Value);

            return await query.ToListAsync();
        }
        // ✅ Implementation for GetDaysPresentAsync
        public async Task<int> GetDaysPresentAsync(int laborId, DateTime start, DateTime end)
        {
            return await _context.Attendances
                                 .Where(a => a.LaborId == laborId &&
                                             a.Date >= start &&
                                             a.Date <= end &&
                                             a.IsPresent)
                                 .CountAsync();
        }

    }
}
