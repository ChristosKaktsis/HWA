using Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HWA.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(HWA.iOS.Service.WebViewService))]
namespace HWA.iOS.Service
{
    public class WebViewService : IWebViewService
    {
        public string GetContent()
        {
            return Path.Combine(NSBundle.MainBundle.BundlePath, "Resources/call.html");
        }
    }
}