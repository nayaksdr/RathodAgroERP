﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Core.ViewModel
{
    public class PaymentGenerateVm
    {
        public int FarmerId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
