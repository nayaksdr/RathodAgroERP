using System.ComponentModel.DataAnnotations.Schema;

namespace JaggeryAgro.Core.Entities
{
    public class JaggerySaleShare
    {
        public int Id { get; set; }

        // कोणत्या विक्रीसाठी
        public int JaggerySaleId { get; set; }
       
        // व्यक्ती नाव
        public string PersonName { get; set; }

        // व्यक्तीने भरलेली रक्कम
        public decimal PaidAmount { get; set; }

        // व्यक्तीची वाटप रक्कम
        public decimal PercentageShare { get; set; } // Example: 50 for 50% share

        public decimal ShareAmount { get; set; }

        // नेट रक्कम = PaidAmount - ShareAmount
        public decimal NetAmount { get; set; }

        public int MemberId { get; set; }   // Foreign Key       

        public JaggerySale JaggerySale { get; set; } = null!;

        public Member Member { get; set; } = null!;  

        [NotMapped]                            // Not stored in DB, just for display
        public string MemberName => Member.Name;

        [NotMapped]
        public string DealerName => JaggerySale.Dealer.Name;
       

    }

}
