using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.Interfaces
{
    public class PdfService : IPdfService
    {

        public byte[] GenerateLaborSlip(LaborPayment payment,string name, DateTime from, DateTime to)
        {
            // Your PDF generation logic here
            return new byte[0]; // placeholder
        }
    }
}