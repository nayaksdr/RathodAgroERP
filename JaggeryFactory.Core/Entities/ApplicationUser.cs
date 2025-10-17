using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System;

namespace JaggeryAgro.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime? LastDayAsRoleUse { get; set; }
    }

}
