using HWA.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(HWA.Droid.Service.WebViewService))]

namespace HWA.Droid.Service
{
    public class WebViewService : IWebViewService
    {
        public string GetContent()
        {
            return "file:///android_asset/call.html";
        }
    }
}