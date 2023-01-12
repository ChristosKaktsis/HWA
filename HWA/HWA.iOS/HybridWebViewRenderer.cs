
using WebKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using HWA.Views;
using HWA.iOS;

[assembly: ExportRenderer(typeof(GenericWebView), typeof(HybridWebViewRenderer))]
namespace HWA.iOS
{
    public class HybridWebViewRenderer : ViewRenderer<WebView, WKWebView>
    {
        WKWebView wkWebView;

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
            if (Control != null) return;
            var config = new WKWebViewConfiguration();
            wkWebView = new WKWebView(Frame, config) { NavigationDelegate = new MyNavigationDelegate() };
            SetNativeControl(wkWebView);
        }
    }

    public class MyNavigationDelegate : WKNavigationDelegate
    {
        public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {

            //get url here
            var url = webView.Url;

            // webView.LoadFileUrl(url, url);

        }
    }
}