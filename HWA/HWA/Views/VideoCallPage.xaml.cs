using HWA.Services;
using HWA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HWA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoCallPage : ContentPage
    {
        private string current_id;
        private string other_id;
        private bool cameraOn;
        private VideoCallViewModel _viewModel;

        public VideoCallPage(string current_id, string other_id,bool cameraOn = true)
        {
            InitializeComponent();
            BindingContext = _viewModel = new VideoCallViewModel();
            var urlSource = new UrlWebViewSource();
            string baseUrl = DependencyService.Get<IWebViewService>().GetContent();
            urlSource.Url = baseUrl;
            CallWebView.Source = urlSource;
            this.current_id = current_id;
            this.other_id = other_id;
            this.cameraOn = cameraOn;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(2000);
            await Connect();
            //if (cameraOn) return;
            //await Task.Delay(2000);
            //_viewModel.IsCameraOn = false;
            //await CallWebView.EvaluateJavaScriptAsync($"toggleVideo('{false}');");
        }
        private async Task Connect()
        {
            try
            {
                var x = await CallWebView.EvaluateJavaScriptAsync($"init('{current_id}');");
                await Task.Delay(2000);
                var y = await CallWebView.EvaluateJavaScriptAsync($"startCall('{other_id}');");
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
        private void EndCall()
        {
            CallWebView.Source = "about:blank";
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            EndCall();
        }
        private async void reject_btn_Clicked(object sender, EventArgs e)
        {
            await _viewModel.EndCall(other_id);
            await Shell.Current.Navigation.PopAsync();
        }
        private async void togglecamera_btn_Clicked(object sender, EventArgs e)
        {
            _viewModel.IsCameraOn = !_viewModel.IsCameraOn;
            await CallWebView.EvaluateJavaScriptAsync($"toggleVideo('{_viewModel.IsCameraOn.ToString().ToLower()}');");
        }
        private async void togglemic_btn_Clicked(object sender, EventArgs e)
        {
            _viewModel.IsMicOn = !_viewModel.IsMicOn;
            await CallWebView.EvaluateJavaScriptAsync($"toggleAudio('{_viewModel.IsMicOn.ToString().ToLower()}');");
        }
    }
}