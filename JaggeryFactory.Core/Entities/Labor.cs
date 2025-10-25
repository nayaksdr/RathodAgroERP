using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JaggeryAgro.Core.Entities
{
    public class Labor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;       

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime JoinDate { get; set; }

        // 👉 Foreign Key
        [Required]
        public int LaborTypeId { get; set; }

        [ForeignKey(nameof(LaborTypeId))]   // ✅ Explicitly point to FK
        public LaborType LaborType { get; set; } = null!;  // Navigation Property       
        public string Mobile { get; set; } // can use as username
        public string PasswordHash { get; set; } // store hashed password
        public string Role { get; set; } = "Labor"; // default role
        public bool IsActive { get; set; } = true;
    }
}
