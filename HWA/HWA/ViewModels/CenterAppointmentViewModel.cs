using HWA.Data;
using HWA.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class CenterAppointmentViewModel : BaseViewModel
    {
        public ObservableCollection<Center> Centers { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> Areas { get; set; }
        public ObservableCollection<string> PrefTimes { get; set; }
        protected CenterManager centerManager;
        public Command SubmitCommand { get; }
        public Command CancelCommand { get; }
        public CenterAppointmentViewModel()
        {
            var programID = App.Customer.InsuranceProgramID;
            centerManager = new CenterManager(programID);
            Centers = new ObservableCollection<Center>();
            Cities = new ObservableCollection<string>();
            Areas = new ObservableCollection<string>();
            PrefTimes = new ObservableCollection<string>();
            SubmitCommand = new Command(async () => await Sunmit());
            CancelCommand = new Command(Cancel);
        }

        public virtual async void OnAppearing()
        {
            await LoadCities();
            await LoadPrefTime();
        }
        private async Task LoadPrefTime()
        {
            try
            {
                IsBusy = true;
                PrefTimes.Clear();
                var items = await centerManager.GetCenterTime();
                foreach (var item in items)
                    PrefTimes.Add(item);
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
        private async Task LoadCities()
        {
            try
            {
                IsBusy = true;
                Cities.Clear();
                var items = await centerManager.GetCenterCities();
                foreach (var item in items)
                    Cities.Add(item.Name);
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
        private string selectedCity;
        public string SelectedCity
        {
            get { return selectedCity; }
            set 
            { 
                selectedCity = value;
                OnSelectedCity(value);
            }
        }
        private async void OnSelectedCity(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            await LoadAreas();
        }
        private async Task LoadAreas()
        {
            try
            {
                IsBusy = true;
                Areas.Clear();
                var items = await centerManager.GetCenterAreas(SelectedCity);
                foreach (var item in items)
                    Areas.Add(item.Name);
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
        private string selectedArea;
        public string SelectedArea
        {
            get { return selectedArea; }
            set 
            { 
                selectedArea = value;
                OnSelectedArea(value);
            }
        }
        private async void OnSelectedArea(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            await LoadCenters();
        }
        private async Task LoadCenters()
        {
            try
            {
                IsBusy = true;
                Centers.Clear();
                var items = await centerManager.GetCenters(SelectedCity, SelectedArea);
                foreach (var item in items)
                    Centers.Add(item);
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
        public Center SelectedCenter { get; set; }
        public string SelectedTime { get; set; }
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public FileResult File { get; set; }
        protected virtual async Task Sunmit()
        {
            bool isOk = false;
            try
            {
                IsBusy = true;
                //submit
                var answer = await centerManager.SubmitAppointment(App.Customer.ID.ToString(), SelectedCenter.Oid.ToString(),
                     SelectedTime, Date1.ToString("MM/dd/yyyy"), Date2.ToString("MM/dd/yyyy"),File);
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
                await ToastUser(isOk, AppResources.appoinment_result,AppResources.appoinment_result_error);
            }
        }
    }
}
