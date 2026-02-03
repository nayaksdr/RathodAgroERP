using AutoMapper;
using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Core.Services;

namespace JaggeryAgro.Infrastructure.Services
{
    public class LaborTypeRateService : ILaborTypeRateService
    {
        private readonly ILaborTypeRateRepository _repository;
        private readonly IMapper _mapper;

        public LaborTypeRateService(
            ILaborTypeRateRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<LaborTypeRateDto>> GetAllRatesAsync()
        {
            var entities = await _repository.GetAllRatesAsync();
            return _mapper.Map<List<LaborTypeRateDto>>(entities);
        }

        public async Task<LaborTypeRateDto?> GetCurrentRateAsync(int laborTypeId)
        {
            var entity = await _repository.GetCurrentRateByLaborTypeIdAsync(laborTypeId);
            return _mapper.Map<LaborTypeRateDto>(entity);
        }

        public async Task<LaborTypeRateDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<LaborTypeRateDto>(entity);
        }

        public async Task AddRateAsync(LaborTypeRateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<LaborTypeRate>(dto);
            await _repository.AddRateAsync(entity);
        }

        public async Task UpdateRateAsync(LaborTypeRateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<LaborTypeRate>(dto);
            await _repository.UpdateRateAsync(entity);
        }

        public async Task DeleteRateAsync(int id)
        {
            await _repository.DeleteRateAsync(id);
        }
    }
}
