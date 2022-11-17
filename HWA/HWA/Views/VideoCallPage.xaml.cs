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
        private VideoCallViewModel _viewModel;

        public VideoCallPage(string current_id, string other_id)
        {
            InitializeComponent();
            BindingContext = _viewModel = new VideoCallViewModel();
            var urlSource = new UrlWebViewSource();
            string baseUrl = DependencyService.Get<IWebViewService>().GetContent();
            urlSource.Url = baseUrl;
            CallWebView.Source = urlSource;
            this.current_id = current_id;
            this.other_id = other_id;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(2000);
            await Connect();
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
    }
}