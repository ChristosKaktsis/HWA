using HWA.Models;
using HWA.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HWA.Services
{
    public class Connection
    {
        public static string Url
        {
            get
            {
                string ip = "192.168.3.20";
                //string ip = "79.129.5.42";
#if DEBUG
                ip = "10.0.2.2";
                //ip = "192.168.3.99";
                //ip = "79.129.5.42";
#endif
                var device = DeviceInfo.Platform;
                if (device == DevicePlatform.Android) return ip;
                if (device == DevicePlatform.iOS) return "192.168.3.99";

                else return "localhost";

            }
        }
    }
}
