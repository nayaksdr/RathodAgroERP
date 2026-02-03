using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JaggeryAgro.Core.Entities
{
    public class AdvancePayment
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key to Labor
        [Required]
        public int LaborId { get; set; }

        // Navigation property to Labor
        [ForeignKey(nameof(LaborId))]
        public virtual Labor Labor { get; set; } = null!;

        // Date of the payment
        [Required]
        public DateTime DateGiven { get; set; } = DateTime.Now;

        // Amount of the advance
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
