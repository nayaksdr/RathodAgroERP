using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JaggeryAgro.Core.Entities
{
    [Table("JaggerySalePayments")]
    public class JaggerySalePayment
    {
        [Key]
        public int Id { get; set; }

        // -------------------- Relationships --------------------
        [ForeignKey(nameof(JaggerySale))]
        public int JaggerySaleId { get; set; }
        public JaggerySale JaggerySale { get; set; } = null!;

        [ForeignKey(nameof(FromMember))]
        public int FromMemberId { get; set; }      // Who paid
        public Member FromMember { get; set; } = null!;

        [ForeignKey(nameof(ToMember))]
        public int ToMemberId { get; set; }        // Who receives / owed
        public Member ToMember { get; set; } = null!;

        // If you truly need a *generic* Member reference, keep it,
        // otherwise remove it to avoid confusion.
        //[ForeignKey(nameof(Member))]
        //public int MemberId { get; set; }
        //public Member Member { get; set; } = null!;

        // -------------------- Payment Details --------------------
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }        // Total amount of the payment

        public DateTime PaymentDate { get; set; }  // Date when payment was made

        [Required, MaxLength(50)]
        public string PaymentMode { get; set; } = string.Empty; // e.g. Cash, Bank, UPI
    }
}
