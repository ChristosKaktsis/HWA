using HWA.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace HWA.ViewModels
{
    public class CheckUpViewModel : CenterAppointmentViewModel
    {
        public ObservableCollection<string> Packages { get; set; }

        public CheckUpViewModel()
        {
            Packages = new ObservableCollection<string>();
        }
        public override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadPackages();
        }

        private async Task LoadPackages()
        {
            try
            {
                IsBusy = true;
                Packages.Clear();
                var items = await centerManager.GetAvailablePackages();
                foreach (var item in items)
                    Packages.Add(item.Description);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public string SelectedPackage { get; set; }

        protected override async Task Sunmit()
        {
            bool isOk = false;
            try
            {
                IsBusy = true;
                //submit
                var answer = await centerManager.SubmitCheckUp(App.Customer.ID.ToString(), SelectedCenter.Oid.ToString(),
                     SelectedTime, Date1.ToString("MM/dd/yyyy"), Date2.ToString("MM/dd/yyyy"), SelectedPackage);
                //go to main menu
                Cancel();
                //check if error
                isOk = answer.IsSuccessful;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
                //toast result to user
                await ToastUser(isOk, AppResources.appoinment_result, AppResources.appoinment_result_error);
            }
        }
    }
}
