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
    public class HospitalizationViewModel : BaseViewModel
    {
        public ObservableCollection<Clinic> Clinics { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> Areas { get; set; }
        private ClinicManager clinicManager;
        public Command SubmitCommand { get; }
        public Command CancelCommand { get; }
        public HospitalizationViewModel()
        {
            Clinics = new ObservableCollection<Clinic>();
            Cities = new ObservableCollection<string>();
            Areas = new ObservableCollection<string>();

            var programID = App.Customer.InsuranceProgramID;
            clinicManager = new ClinicManager(programID);
            SubmitCommand = new Command(async () => await Sunmit());
            CancelCommand = new Command(Cancel);
        }
        public async void OnAppearing()
        {
            await LoadCities();
        }
        private async Task LoadCities()
        {
            try
            {
                IsBusy = true;
                Cities.Clear();
                var items = await clinicManager.GetClinicCities();
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
                var items = await clinicManager.GetClinicAreas(SelectedCity);
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
            await LoadClinics();
        }
        private async Task LoadClinics()
        {
            try
            {
                IsBusy = true;
                Clinics.Clear();
                var items = await clinicManager.GetClinics(SelectedCity, SelectedArea);
                foreach (var item in items)
                    Clinics.Add(item);
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
        public DateTime Date1 { get; set; }
        public FileResult File { get; set; }
        public Clinic SelectedClinic { get; set; }
        protected async Task Sunmit()
        {
            bool isOk = false;
            try
            {
                IsBusy = true;
                //submit
                var answer = await clinicManager.SubmitHospitalization(App.Customer.ID.ToString(),
                    SelectedClinic.Oid, Date1.ToString("MM/dd/yyyy"), File);
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
