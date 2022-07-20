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
    public class NetListViewModel : BaseViewModel
    {
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> Areas { get; set; }
        public ObservableCollection<Doctor> Doctors { get; set; }
        private NetManager netManager;
        public Command LoadCommand { get; }
        public Command CancelCommand { get; }
        public NetListViewModel()
        {
            Cities = new ObservableCollection<string>();
            Areas = new ObservableCollection<string>();
            Doctors = new ObservableCollection<Doctor>();
            netManager = new NetManager();
            LoadCommand = new Command(async () => await LoadAll());
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
                var items = await netManager.GetAllCities();
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
                var items = await netManager.GetAllAreas(SelectedCity);
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
            }
        }
        private async Task LoadAll()
        {
            try
            {
                IsBusy = true;
                Doctors.Clear();
                var items = await netManager.GetAll(SelectedCity,SelectedArea);
                foreach (var item in items)
                    Doctors.Add(item);
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
    }
}
