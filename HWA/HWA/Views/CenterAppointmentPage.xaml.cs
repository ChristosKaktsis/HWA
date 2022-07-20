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
    public partial class CenterAppointmentPage : ContentPage
    {
        private CenterAppointmentViewModel _viewModel;

        public CenterAppointmentPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CenterAppointmentViewModel();
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
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private async void photo_button_Clicked(object sender, EventArgs e)
        {
            await TakePhotoAsync();
        }
        async Task TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo == null)
                    return;
                LabelInfo.Text = photo.FileName;
                _viewModel.File = photo;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
                Console.WriteLine($"CapturePhotoAsync Feature is not supported on the device");

            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                Console.WriteLine($"CapturePhotoAsync Permissions not granted");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
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