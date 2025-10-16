using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.ViewModel
{
    public class LaborLoginViewModel
    {
        [Required(ErrorMessage = "मोबाईल नंबर आवश्यक आहे")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "मोबाईल नंबर 10 अंकांचा असावा")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "पासवर्ड आवश्यक आहे")]
        public string Password { get; set; }
    }


}
