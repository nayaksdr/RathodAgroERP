using AutoMapper;
using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Repositories
{
    public class LaborTypeRateProfile : Profile
    {
        public LaborTypeRateProfile()
        {
            CreateMap<LaborTypeRate, LaborTypeRateDto>();
            CreateMap<LaborTypeRateDto, LaborTypeRate>();
        }
    }

}
