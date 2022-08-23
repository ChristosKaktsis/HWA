using HWA.Data;
using HWA.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class CheckUpViewModel : CenterAppointmentViewModel
    {
        public ObservableCollection<Package> Packages { get; set; }
        public Command OpenInfoCommand { get; set; }

        public CheckUpViewModel()
        {
            Packages = new ObservableCollection<Package>();
            OpenInfoCommand = new Command(ShowPackageInfo, HasInfo);
            this.PropertyChanged +=
                (_, __) => OpenInfoCommand.ChangeCanExecute();
            CurrentDate = DateTime.Now.AddDays(1);
        }

        private bool HasInfo(object arg)
        {
            return !string.IsNullOrEmpty(PackageInfo);
        }

        private void ShowPackageInfo(object obj)
        {
            ShowPackage = !ShowPackage;
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
                    Packages.Add(item);
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
        private Package selectedPackage;

        public Package SelectedPackage
        {
            get { return selectedPackage; }
            set 
            { 
                SetProperty(ref selectedPackage, value);
                ProcessInfo(value);
            }
        }

        private void ProcessInfo(Package value)
        {
            PackageInfo = string.Empty;
            if (value == null)
                return;
            PackageInfo = value.Comments.Replace("newline", "\n");
            
        }

        private bool showPackage;
        public bool ShowPackage
        {
            get { return showPackage; }
            set { SetProperty(ref showPackage,value); }
        }
        private string packageInfo;
        public string PackageInfo
        {
            get { return packageInfo; }
            set { SetProperty(ref packageInfo, value); }
        }
        private bool hasInfo = false;
       
        protected override async Task Sunmit()
        {
            if (SelectedPackage == null)
                return;
            bool isOk = false;
            try
            {
                IsBusy = true;
                //submit
                var answer = await centerManager.SubmitCheckUp(App.Customer.ID.ToString(), SelectedCenter.Oid.ToString(),
                     SelectedTime, Date1.ToString("MM/dd/yyyy"), Date2.ToString("MM/dd/yyyy"), SelectedPackage.Description);
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
