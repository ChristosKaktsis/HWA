using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWA.ViewModels
{
    public class ChangeRegionsViewModel : BaseViewModel
    {
        private string _SelectedCity;
        private object LoadList;
        private object Loadcenter;
        private object Loadclinic;

        public ObservableCollection<string> region { get; set; }
        public ObservableCollection<string> centers { get; set; }
        public ObservableCollection<string> clinic { get; set; }
        public string SelectedCity
        {
            get { return _SelectedCity; }
            set 
            { 
                SetProperty(ref _SelectedCity, value);
                ComboBoxEdit_SelectionChanged();
            }
        }
        
        public ChangeRegionsViewModel()
        {
            region = new ObservableCollection<string>();
            centers = new ObservableCollection<string>();
            clinic = new ObservableCollection<string>();
        }
        private async void ComboBoxEdit_SelectionChanged()
        {
            
            switch (SelectedCity)
            {
                case "Θεσσαλονίκη":
                    App.Current.Resources.TryGetValue("thess_region", out LoadList);
                    App.Current.Resources.TryGetValue("thess_center", out Loadcenter);
                    App.Current.Resources.TryGetValue("thess_clin", out Loadclinic);
                    break;
                case "Αθήνα":
                    App.Current.Resources.TryGetValue("athens_region", out LoadList);
                    App.Current.Resources.TryGetValue("athens_center", out Loadcenter);
                    App.Current.Resources.TryGetValue("athens_clin", out Loadclinic);
                    break;
            }
            await LoadRegionStrings(((IEnumerable<string>)LoadList).Cast<string>().ToList());
            await LoadCenterStrings(((IEnumerable<string>)Loadcenter).Cast<string>().ToList());
            await LoadClinicStrings(((IEnumerable<string>)Loadclinic).Cast<string>().ToList());
        }

        private async Task LoadClinicStrings(List<string> list)
        {
            try
            {
                clinic.Clear();
                await Task.Run(() =>
                {
                    foreach (var item in list)
                        clinic.Add(item);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task LoadCenterStrings(List<string> list)
        {
            try
            {
                centers.Clear();
                await Task.Run(() =>
                {
                    foreach (var item in list)
                        centers.Add(item);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task LoadRegionStrings(List<string> loadList)
        {
            try
            {
                region.Clear();
                await Task.Run(() =>
                {
                    foreach (var item in loadList)
                        region.Add(item);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
