using HWA.Data;
using HWA.Resources;
using HWA.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }
        public Command PanicCommand { get; }
        public Command StopTimer { get; }
        public MainMenuViewModel()
        {
            //MenuItems = new ObservableCollection<MenuItemViewModel>()
            //{
            //    new MenuItemViewModel{ Image="doc.png", ItemName = AppResources.docappointment, PageName=nameof(DoctorAppointmentPage)},
            //    new MenuItemViewModel{ Image="schedule.png", ItemName = AppResources.centerappointment, PageName=nameof(CenterAppointmentPage)},
            //    new MenuItemViewModel{ Image="healthgraph.png", ItemName = AppResources.checkup, PageName=nameof(CheckupPage)},
            //    new MenuItemViewModel{ Image="hospital.png", ItemName = AppResources.hospitalization, PageName = nameof(HospitalizationPage)},
            //    new MenuItemViewModel{ Image="search.png", ItemName = AppResources.netlist, PageName = nameof(NetListPage)},
            //};
            MenuItems = new ObservableCollection<MenuItemViewModel>();
            PanicCommand = new Command(async () =>await CountdownToPanic());
            StopTimer = new Command(() => Panic_Warning = false);
            //PanicCommand = new Command(async () => await PanicPressed());
        }
        public void OnAppearing() 
        {
            CheckUserValidOperations();
        }
        private void CheckUserValidOperations()
        {
            if (App.Customer == null)
                return;
            string[] operations = App.Customer.InsuranceProgramValidOperations.Split(',');
            MenuItems.Clear();
            foreach (string operation in operations)
                ActivateProgram(operation);
        }
        private void ActivateProgram(string operation)
        {
            switch (operation)
            {
                case "0":
                    PanicIsVisible = true;
                    Console.WriteLine("Panic button");
                    break;
                case "1":
                    MenuItems.Add(new MenuItemViewModel 
                    { 
                        Image = "doc.png", 
                        ItemName = AppResources.docappointment,
                        PageName = nameof(DoctorAppointmentPage)
                    });
                    Console.WriteLine("doc");
                    break;
                case "2":
                    MenuItems.Add(new MenuItemViewModel 
                    { Image = "schedule.png", 
                        ItemName = AppResources.centerappointment,
                        PageName = nameof(CenterAppointmentPage)
                    });
                    Console.WriteLine("center");
                    break;
                case "3":
                    MenuItems.Add(new MenuItemViewModel 
                    { 
                        Image = "healthgraph.png",
                        ItemName = AppResources.checkup,
                        PageName = nameof(CheckupPage)
                    });
                    Console.WriteLine("check");
                    break;
                case "4":
                    MenuItems.Add(new MenuItemViewModel 
                    { 
                        Image = "hospital.png", 
                        ItemName = AppResources.hospitalization,
                        PageName = nameof(HospitalizationPage)
                    });
                    Console.WriteLine("hosp");
                    break;
                case "5":
                    MenuItems.Add(new MenuItemViewModel 
                    { 
                        Image = "search.png", 
                        ItemName = AppResources.netlist,
                        PageName = nameof(NetListPage)
                    });
                    Console.WriteLine("claims");
                    break;
            }
        }
        private bool panicIsVisible ;
        public bool PanicIsVisible
        {
            get { return panicIsVisible; }
            set { SetProperty(ref panicIsVisible, value); }
        }
        private bool panic_Warning;

        public bool Panic_Warning
        {
            get { return panic_Warning; }
            set { SetProperty(ref panic_Warning, value); }
        }
        private string timerInfo;

        public string TimerInfo
        {
            get { return timerInfo; }
            set { SetProperty(ref timerInfo,value); }
        }

        private async Task PanicPressed()
        {
            PanicButton panicButton = new PanicButton();
            try
            {
                IsBusy = true;
                var location = await GetCurrentLocation();
                var answer = await panicButton.SendPanic(App.Customer.ID.ToString(),
                    location.Latitude.ToString(),location.Longitude.ToString());
                await ToastUser(answer.IsSuccessful, answer.Status);
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
        CancellationTokenSource cts;
        private async Task<Location> GetCurrentLocation()
        {
            string coordinates = string.Empty;
            Location location = new Location();

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    coordinates = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            return location;
        }
        private async Task CountdownToPanic()
        {
            Stopwatch timer = new Stopwatch();
            // open pop up 
            Panic_Warning = true;
            //start counting
            timer.Start();
            await Task.Run(() => 
            {
                while (timer.Elapsed.TotalSeconds <= 10)
                {
                    TimerInfo =(10 - timer.Elapsed.TotalSeconds).ToString("0");
                    //stop timer
                    if (!Panic_Warning)
                        return;
                }
            });
            //cancel operation
            if (!Panic_Warning)
                return;
            timer.Stop();
            Panic_Warning = false;
            //start operation
            Console.WriteLine("PANIC!!!");
            await PanicPressed();
        }
    }
}
