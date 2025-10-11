using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Data
{
    public class ApplicationRole : IdentityRole
    {
        public DateTime? LastDayAsRoleUse { get; set; }
    }

}
