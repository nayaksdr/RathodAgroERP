using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JaggeryAgro.Core.Entities
{
    public class Labor
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

        // Foreign Key
        [Required]
        public int LaborTypeId { get; set; }

        [ForeignKey(nameof(LaborTypeId))]
        public virtual LaborType LaborType { get; set; } = null!;

        [MaxLength(15)]
        public string Mobile { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Role { get; set; } = "Labor";

        public bool IsActive { get; set; } = true;

        // Add this if using AdvancePayments
       // public virtual ICollection<AdvancePayment> AdvancePayments { get; set; } = new List<AdvancePayment>();
    }
}
