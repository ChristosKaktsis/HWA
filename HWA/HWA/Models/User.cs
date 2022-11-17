using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HWA.Models
{
    public class User
    {
        private Random random = new Random();
        public string ID { get; set; }
        public string Name { get; set; }
        public bool IsDoctor { get; set; }
        public Color Color { 
            get 
            {
                return Color.FromRgb(random.Next(256), random.Next(256), random.Next(256));
            } 
        }
    }
}
