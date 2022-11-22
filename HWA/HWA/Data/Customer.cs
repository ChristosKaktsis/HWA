using System;
using System.Collections.Generic;
using System.Text;

namespace HWA.Data
{
    public class Customer
    {
        public Guid ID { get; set; }
        public string LoginID { get; set; }
        public string InsuranceProgramID { get; set; }
        public string InsuranceProgramValidOperations { get; set; }
        public string ErrorMessage { get; set; }
        public string Name { get; set; }
    }
}
