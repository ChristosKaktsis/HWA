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
    public partial class LoginDoctor : ContentPage
    {
        private LoginDoctorViewModel _viewModel;

        public LoginDoctor()
        {
            InitializeComponent();
            BindingContext = _viewModel = new LoginDoctorViewModel();
        }
    }
}