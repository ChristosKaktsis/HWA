using Android.Annotation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Android.Renderscripts.Sampler;
using Xamarin.Forms.Platform.Android;
using HWA.Droid;
using HWA.Views;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(GenericWebView), typeof(HybridWebViewRenderer))]
namespace HWA.Droid
{
    public class HybridWebViewRenderer : WebViewRenderer
    {
        Activity mContext;
        public HybridWebViewRenderer(Context context) : base(context)
        {
            this.mContext = context as Activity;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            Control.Settings.JavaScriptEnabled = true;
            Control.ClearCache(true);
            Control.Settings.MediaPlaybackRequiresUserGesture = false;
            Control.SetWebChromeClient(new MyWebClient(mContext));
        }
        public class MyWebClient : WebChromeClient
        {
            Activity mContext;
            public MyWebClient(Activity context)
            {
                this.mContext = context;
            }
            [TargetApi(Value = 21)]
            public override void OnPermissionRequest(PermissionRequest request)
            {
                mContext.RunOnUiThread(() =>
                {
                    request.Grant(request.GetResources());
                });
            }
        }
    }
}