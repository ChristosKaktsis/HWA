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
    public partial class CheckupPage : ContentPage
    {
        private CheckUpViewModel _viewModel;

        public CheckupPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CheckUpViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        private void Submit_Clicked(object sender, EventArgs e)
        {
            if (!CheckForEmpty())
                _viewModel.SubmitCommand.Execute(null);
        }
        private bool CheckForEmpty()
        {
            centers_edit.HasError = centers_edit.SelectedIndex == -1;
            city_edit.HasError = city_edit.SelectedIndex == -1;
            area_edit.HasError = area_edit.SelectedIndex == -1;
            date1_edit.HasError = !date1_edit.Date.HasValue;
            date2_edit.HasError = !date2_edit.Date.HasValue;
            time_edit.HasError = time_edit.SelectedIndex == -1;

            return centers_edit.HasError || city_edit.HasError ||
                area_edit.HasError || date1_edit.HasError || date2_edit.HasError || time_edit.HasError;
        }
    }
}