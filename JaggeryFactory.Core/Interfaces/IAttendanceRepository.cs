using JaggeryAgro.Core.Entities;

namespace JaggeryAgro.Core.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<Attendance?> GetByIdAsync(int id);
        Task AddAsync(Attendance attendance);
        Task UpdateAsync(Attendance attendance);
        Task DeleteAsync(int id);
        Task SaveAsync();
        Task<List<Attendance>> GetByLaborIdAndDateRangeAsync(int laborId, DateTime? from, DateTime? to);
        Task<IEnumerable<Attendance>> GetByLaborAsync(int laborId);
        Task<IEnumerable<Attendance>> GetByLaborInRangeAsync(int laborId, DateTime from, DateTime to);
        Task<IEnumerable<Attendance>> GetByLaborIdAndDateRangeAsync(int laborId, DateTime start, DateTime end);
        Task<IEnumerable<Attendance>> GetAttendanceBetweenDatesAsync(DateTime start, DateTime end);
    }
}
