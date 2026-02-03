using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class FarmerRepository : IFarmerRepository
    {
        private readonly ApplicationDbContext _db;
        public FarmerRepository(ApplicationDbContext db) => _db = db;

        public async Task AddAsync(Farmer farmer) { _db.Farmers.Add(farmer); await _db.SaveChangesAsync(); }
        public async Task DeleteAsync(int id)
        {
            var ent = await _db.Farmers.FindAsync(id);
            if (ent != null) { _db.Farmers.Remove(ent); await _db.SaveChangesAsync(); }
        }
        public async Task<IEnumerable<Farmer>> GetAllAsync() => await _db.Farmers.OrderBy(f => f.Name).ToListAsync();
        public async Task<Farmer> GetByIdAsync(int id) => await _db.Farmers.FindAsync(id);
        public async Task UpdateAsync(Farmer farmer) { _db.Farmers.Update(farmer); await _db.SaveChangesAsync(); }
        public async Task<Farmer?> GetAsync(int id) => await _db.Farmers.FirstOrDefaultAsync(x => x.Id == id);
      
    }
}
