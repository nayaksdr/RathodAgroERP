using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class CreateJaggeryShareDto
    {
        [Required]
        public int JaggerySaleId { get; set; }

        [Required]
        public int PayingMemberId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one member is required")]
        public List<int> SplitMemberIds { get; set; } = new();

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Paid amount must be valid")]
        public decimal PaidAmount { get; set; }
    }
}
