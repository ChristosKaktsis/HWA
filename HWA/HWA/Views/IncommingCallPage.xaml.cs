using HWA.Models;
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
    public partial class IncommingCallPage : ContentPage
    {
        private IncommingCallViewModel _viewModel;
        private string id_calling;
        private string name;
        public IncommingCallPage(User user_calling)
        {
            InitializeComponent();
            BindingContext = _viewModel = new IncommingCallViewModel();
            name = user_calling?.Name;
            id_calling = user_calling?.ID;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            user_label.Text = $"From : {name}";
        }
        private async void accept_btn_Clicked(object sender, EventArgs e)
        {
            await _viewModel.AcceptCall(id_calling);
        }

        private async void reject_btn_Clicked(object sender, EventArgs e)
        {
            await _viewModel.RejectCall(id_calling);
        }
    }
}