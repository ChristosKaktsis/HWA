using System;
using System.Collections.Generic;
using System.Text;

namespace HWA.Models
{
    public class StripeProduct
    {
        public string Description { get; set; }
        public double Price { get; set; }
        public string Comments { get; set; }
        public string StripePaymentLink { get; set; }
    }
}
