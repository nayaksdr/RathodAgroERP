using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class LaborTypeRepository : ILaborTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public LaborTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get all (async)
        public async Task<IEnumerable<LaborType>> GetAllAsync()
        {
            return await _context.LaborTypes.ToListAsync();
        }

        // ✅ Get all (list, async)
        public async Task<List<LaborType>> GetAllLaborTypesAsync()
        {
            return await _context.LaborTypes.ToListAsync();
        }

        // ✅ Get by Id (async)
        public async Task<LaborType?> GetByIdAsync(int id)
        {
            return await _context.LaborTypes.FindAsync(id);
        }

        // ✅ Add (async)
        public async Task AddAsync(LaborType type)
        {
            await _context.LaborTypes.AddAsync(type);
            await _context.SaveChangesAsync();
        }

        // ✅ Update (async)
        public async Task UpdateAsync(LaborType type)
        {
            _context.LaborTypes.Update(type);
            await _context.SaveChangesAsync();
        }

        // ✅ Delete (async)
        public async Task DeleteAsync(int id)
        {
            var type = await _context.LaborTypes.FindAsync(id);
            if (type != null)
            {
                _context.LaborTypes.Remove(type);
                await _context.SaveChangesAsync();
            }
        }       

    }
}
