using HWA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HWA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HospitalizationPage : ContentPage
    {
        private HospitalizationViewModel _viewModel;

        public HospitalizationPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HospitalizationViewModel();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var file = await FilePicker.PickAsync();
                if (file == null)
                    return;
                LabelInfo.Text = file.FileName;
                _viewModel.File = file;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private void Submit_Clicked(object sender, EventArgs e)
        {
            if (!CheckForEmpty())
                _viewModel.SubmitCommand.Execute(null);
        }
        private bool CheckForEmpty()
        {
            clinic_edit.HasError = clinic_edit.SelectedIndex == -1;
            city_edit.HasError = city_edit.SelectedIndex == -1;
            area_edit.HasError = area_edit.SelectedIndex == -1;
            date1_edit.HasError = !date1_edit.Date.HasValue;

            return clinic_edit.HasError || city_edit.HasError ||
                area_edit.HasError || date1_edit.HasError ;
        }
    }
}