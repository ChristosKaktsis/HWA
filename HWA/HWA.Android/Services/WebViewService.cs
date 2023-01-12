using HWA.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(HWA.Droid.Services.WebViewService))]

namespace HWA.Droid.Services
{
    public class WebViewService : IWebViewService
    {
        public string GetContent()
        {
            return "file:///android_asset/call.html";
        }
    }
}