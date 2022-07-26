using HWA.Data;
using HWA.Resources;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class DoctorAppointmentViewModel : BaseViewModel
    {
        DoctorManager doctorManager;
        private string selectedDocSpecialty, selectedCity, selectedArea;

        public ObservableCollection<string> DoctorSpecialties { get; }
        public ObservableCollection<string> DoctorCities { get; }
        public ObservableCollection<string> DoctorAreas { get; }
        public ObservableCollection<string> DoctorPrefTimes { get; }
        public Command SubmitCommand { get;  }
        public Command CancelCommand { get;  }
        public DoctorAppointmentViewModel()
        {
            DoctorSpecialties = new ObservableCollection<string>();
            DoctorCities = new ObservableCollection<string>();
            DoctorAreas = new ObservableCollection<string>();
            DoctorPrefTimes = new ObservableCollection<string>();
            var programID = App.Customer.InsuranceProgramID;
            doctorManager = new DoctorManager(programID);
            SubmitCommand = new Command(async ()=> await Sunmit());
            CancelCommand = new Command(Cancel);
        }
        public async void OnAppearing()
        {
            await LoadDoctorSpecialties();
            await LoadDoctorPrefTime();
        }
        private async Task LoadDoctorSpecialties()
        {
            try
            {
                IsBusy = true;
                DoctorSpecialties.Clear();
                var items = await doctorManager.GetDoctorSpecialties();
                foreach (var item in items)
                    DoctorSpecialties.Add(item.Specialty);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task LoadDoctorPrefTime()
        {
            try
            {
                IsBusy = true;
                DoctorPrefTimes.Clear();
                var items = await doctorManager.GetDoctorTime();
                foreach (var item in items)
                    DoctorPrefTimes.Add(item);
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
        public string SelectedDocSpecialty
        {
            get { return selectedDocSpecialty; }
            set 
            { 
                selectedDocSpecialty = value;
                OnSelectedDocSpecialty(value);
            }
        }
        private async void OnSelectedDocSpecialty(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            await LoadDoctorCities();
        }
        private async Task LoadDoctorCities()
        {
            try
            {
                IsBusy = true;
                SelectedCity = string.Empty;
                DoctorCities.Clear();
                var items = await doctorManager.GetDoctorCities(SelectedDocSpecialty);
                foreach (var item in items)
                    DoctorCities.Add(item.Name);
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
        public string SelectedCity
        {
            get { return selectedCity; }
            set 
            { 
                SetProperty(ref selectedCity, value);
                OnSelectedDocCity(value);
            }
        }
        private async void OnSelectedDocCity(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            await LoadDoctorAreas();
        }
        private async Task LoadDoctorAreas()
        {
            try
            {
                IsBusy = true;
                DoctorAreas.Clear();
                var items = await doctorManager.GetDoctorAreas(SelectedDocSpecialty,SelectedCity);
                foreach (var item in items)
                    DoctorAreas.Add(item.Name);
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
        public string SelectedArea
        {
            get { return selectedArea; }
            set { selectedArea = value; }
        }
        public string SelectedTime { get; set; }
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        private async Task Sunmit()
        {
            bool isOk = false;
            try
            {
                IsBusy = true;
                //submit appointment
                var answer = await doctorManager.SubmitAppointment(App.Customer.ID.ToString(), SelectedDocSpecialty,
                    SelectedCity, SelectedArea, SelectedTime, Date1.ToString("MM/dd/yyyy"), Date2.ToString("MM/dd/yyyy"));
                //return to main menu
                Cancel();
                //check if success
                isOk = answer.IsSuccessful;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
                //Notify user
                await ToastUser(isOk, AppResources.appoinment_result, AppResources.appoinment_result_error);
            }
        }

        
    }
}