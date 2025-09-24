using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Services
{
    public class FarmerService : IFarmerService
    {
        private readonly IFarmerRepository _repo;
        public FarmerService(IFarmerRepository repo) => _repo = repo;
        public Task CreateAsync(Farmer f) => _repo.AddAsync(f);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task<Farmer> GetAsync(int id) => _repo.GetByIdAsync(id);
        public Task<IEnumerable<Farmer>> ListAsync() => _repo.GetAllAsync();
        public Task UpdateAsync(Farmer f) => _repo.UpdateAsync(f);
    }
}
