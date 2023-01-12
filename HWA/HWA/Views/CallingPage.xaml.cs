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
    public partial class CallingPage : ContentPage
    {
        private string _name;
        private string _uid;
        private CallingPageViewModel _viewModel;

        public CallingPage(User user)
        {
            InitializeComponent();
            BindingContext = _viewModel = new CallingPageViewModel();
            _name= user?.Name;
            _uid = user?.ID;
            user_label.Text = $"Calling {_name}";
        }

        private async void reject_btn_Clicked(object sender, EventArgs e)
        {
            await _viewModel.RejectCall(_uid);
        }
    }
}