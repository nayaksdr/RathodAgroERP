using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{ 

    public class LaborRepository : ILaborRepository
    {
        private readonly ApplicationDbContext _context;

        public LaborRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Labor>> GetAllLaborsAsync(int? laborTypeId = null)
        {
            var query = _context.Labors
                .Include(l => l.LaborType)
                .AsQueryable();

            if (laborTypeId.HasValue)
                query = query.Where(l => l.LaborTypeId == laborTypeId.Value);

            // Use EF Core async directly
            return await query.ToListAsync();
        }
        public IEnumerable<Labor> GetAll()
        {
            // Use AsNoTracking() for better performance on GET requests
            return _context.Labors
                           .Include(a => a.LaborType)
                           .AsNoTracking()
                           .ToList();
        }
        public async Task<IEnumerable<Labor>> GetAllAsync()
        {
            return await _context.Labors
                .Include(l => l.LaborType) // if you need labor type also
                .ToListAsync();
        }
        //public IEnumerable<Labor> GetAll() => _context.Labors.ToList();

        public async Task<Labor?> GetByIdAsync(int id)
        {
            return await _context.Labors.FindAsync(id);
        }        
        public Labor GetById(int id) => _context.Labors.Find(id);

        public void Add(Labor labor)
        {
            _context.Labors.Add(labor);
            Save();
        }
        public async Task AddAsync(Labor labor)
        {
            await _context.Labors.AddAsync(labor);   // Asynchronous add
            await _context.SaveChangesAsync();       // Asynchronous save
        }

        public void Update(Labor labor)
        {
            _context.Labors.Update(labor);
            Save();
        }     


        public async Task<List<Labor>> GetLaborsByTypeAsync(int laborTypeId)
        {
            return await _context.Labors
                .Include(l => l.LaborType)
                .Where(l => l.LaborTypeId == laborTypeId)
                .ToListAsync();
        }
        public IEnumerable<Labor> GetAllWithLaborType()
        {
            return _context.Labors
                           .Include(l => l.LaborType) // so you get DailyWage & LaborTypeName
                           .ToList();
        }
        public async Task UpdateAsync(Labor labor)
        {
            // Attach entity if not tracked
            var trackedEntity = await _context.Labors.FindAsync(labor.Id);
            if (trackedEntity != null)
            {
                // Update the tracked entity's values
                _context.Entry(trackedEntity).CurrentValues.SetValues(labor);

                 // Save changes asynchronously
                await _context.SaveChangesAsync();
            }
        }

        public void Save() => _context.SaveChangesAsync();

        public async Task DeleteAsync(int id)
        {
            // Get the labor record by Id
            var labor = await _context.Labors.FindAsync(id);

            if (labor != null)
            {
                // Remove the record
                _context.Labors.Remove(labor);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }

        public void Delete(int id)
        {
            // Find the labor by Id
            var labor = _context.Labors.Find(id);  // synchronous

            if (labor != null)
            {
                // Remove the record
                _context.Labors.Remove(labor);

                // Save changes to database
                _context.SaveChanges();  // synchronous
            }
        }
        public async Task<Labor> GetByMobileAsync(string mobile)
        {
            return await _context.Labors.FirstOrDefaultAsync(l => l.Mobile == mobile && l.IsActive);
        }

        public async Task<List<Attendance>> GetAttendanceAsync(int laborId)
        {
            return await _context.Attendances
                .Where(a => a.LaborId == laborId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }
        public async Task<List<WeeklySalary>> GetWeeklySalariesAsync(int laborId)
        {
            return await _context.WeeklySalaries
                .Where(s => s.LaborId == laborId)
                .OrderByDescending(s => s.WeekEnd)
                .ToListAsync();
        }
       
        public async Task<Labor> GetLaborsByTypeIdAsync(int laborTypeId)
        {
            return await _context.Labors
                .Include(l => l.LaborType)
                .FirstOrDefaultAsync(l => l.Id == laborTypeId);
        }

        // ✅ Corrected methods to include LaborType to prevent NullReferenceException
        public async Task<Labor> GetByIdWithLaborTypeAsync(int id)
        {
            return await _context.Labors
                                 .Include(l => l.LaborType) // Include LaborType
                                 .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<Labor>> GetAllLaborsWithLaborTypeAsync()
        {
            return await _context.Labors
                                 .Include(l => l.LaborType) // Include LaborType
                                 .ToListAsync();
        }

        // Optional: get labors by LaborTypeId correctly
        public async Task<List<Labor>> GetLaborsByLaborTypeIdAsync(int laborTypeId)
        {
            return await _context.Labors
                                 .Include(l => l.LaborType)
                                 .Where(l => l.LaborTypeId == laborTypeId)
                                 .ToListAsync();
        }

    }
}
