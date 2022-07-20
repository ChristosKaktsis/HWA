using System;
using System.Collections.Generic;
using System.Text;

namespace HWA.Data
{
    public class Response
    {
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Info { get; set; }
        public bool IsSuccessful 
        {
            get 
            {
                return Status == "SUCCESS";
            } 
        }
    }
}
