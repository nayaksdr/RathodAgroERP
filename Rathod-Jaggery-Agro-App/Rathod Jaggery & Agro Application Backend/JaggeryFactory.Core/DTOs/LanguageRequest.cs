using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.DTOs
{
    public class LanguageRequest
    {
        public string Culture { get; set; } = "en";
        public string? UiCulture { get; set; }
    }
}
