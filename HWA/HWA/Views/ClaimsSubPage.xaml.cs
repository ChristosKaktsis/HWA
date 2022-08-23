using DevExpress.XamarinForms.Editors;
using HWA.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HWA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClaimsSubPage : ContentPage
    {
        private ClaimsSubViewModel _viewModel;

        public ClaimsSubPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ClaimsSubViewModel();
        }

        private async void pick_button_Clicked(object sender, EventArgs e)
        {
            await PickFile();
        }
        async Task PickFile()
        {
            try
            {
                var file = await FilePicker.PickAsync();
                if (file == null)
                    return;
                //add to list
                _viewModel.ChoosenFiles.Add(file);
                //show file as chip 
                Chip_Collection.Chips.Add(new Chip { IsRemoveIconVisible = true,Text= file.FileName });
            }
            catch (Exception ex)
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
                //add to list
                _viewModel.ChoosenFiles.Add(photo);
                //show file as chip 
                Chip_Collection.Chips.Add(new Chip { IsRemoveIconVisible = true, Text = photo.FileName });
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

        private void Chip_Collection_ChipRemoveIconClicked(object sender, ChipEventArgs e)
        {
            _viewModel.Delete(e.Chip.Text);
        }
    }
}