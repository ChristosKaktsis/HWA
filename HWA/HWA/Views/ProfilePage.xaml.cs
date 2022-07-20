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
    public partial class ProfilePage : ContentPage
    {
        private ProfileViewModel _viewModel;

        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ProfileViewModel();
        }
    }
}