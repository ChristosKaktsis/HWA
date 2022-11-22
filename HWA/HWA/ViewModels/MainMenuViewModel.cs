using HWA.Data;
using HWA.Models;
using HWA.Resources;
using HWA.Services;
using HWA.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
        public List<User> Users { get; set; } = new List<User>();
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
            App.Service = new SignalRService(Connection.Url);
            AskPermissions();
        }

        public  void OnAppearing() 
        {
            CheckUserValidOperations();
            InitializeConnection();
        }
        private void CheckUserValidOperations()
        {
            var operations = GetOperations(App.Customer);
            MenuItems.Clear();
            SetOperations(operations);
            ActivateProgram("communication");
        }

        private void SetOperations(string[] operations)
        {
            //string[] operations =new string[] { "0", "1", "2", "3", "4", "5", "6", "7" };
            if (operations == null || operations.Length == 0) return;
            foreach (string operation in operations)
                ActivateProgram(operation);
        }

        private string[] GetOperations(Customer customer)
        {
            if (customer == null)
                return null;
            return App.Customer.InsuranceProgramValidOperations.Split(',');
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
                //case "5":
                //    MenuItems.Add(new MenuItemViewModel 
                //    { 
                //        Image = "search.png", 
                //        ItemName = AppResources.netlist,
                //        PageName = nameof(NetListPage)
                //    });
                //    Console.WriteLine("search");
                //    break;
                case "6":
                    MenuItems.Add(new MenuItemViewModel
                    {
                        Image = "submit.png",
                        ItemName = AppResources.claimsSub,
                        PageName = nameof(ClaimsSubPage)
                    });
                    Console.WriteLine("claims");
                    break;
                case "7":
                    MenuItems.Add(new MenuItemViewModel
                    {
                        Image = "history.png",
                        ItemName = AppResources.history,
                        PageName = nameof(HistoryPage)
                    });
                    Console.WriteLine("history");
                    break;
                case "communication":
                    MenuItems.Add(new MenuItemViewModel
                    {
                        Image = "communication.png",
                        ItemName = AppResources.communication,
                        PageName = nameof(CommunicationPage)
                    });
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
        private async void InitializeConnection()
        {
            if (App.CurrentConnectedUser == null) return;
            if (App.Service.IsConnected)
                return;
            try
            {
                App.Service.ReceiveId(GetCurrentUserID);
                App.Service.ReceiveConnectedUsers(GetUsers);
                App.Service.ReceiveDisconnectedUser(RemoveUser);
                App.Service.ReceiveCall(GetCall);
                App.Service.OnAcceptCall(Accepted);
                App.Service.OnRejectCall(Rejected);
                App.Service.OnEndCall(CallEnded);

                await App.Service.ConnectToHub();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private void GetCurrentUserID(string id)
        {
            App.CurrentConnectedUser.ID = id;
        }
        private void GetUsers(User user)
        {
            if (Users.Where(u => u.ID == user.ID).Any()) return;
            if (user.ID == App.CurrentConnectedUser.ID) return;
            Users.Add(new User { ID = user.ID, Name = user.Name });
        }
        private void RemoveUser(string id)
        {
            if (!Users.Where(u => u.ID == id).Any())
                return;
            Users.Remove(Users.Where(u => u.ID == id).FirstOrDefault());
        }
        private void GetCall(string call)
        {
            var finduser = Users.Where(u => u.ID == call).FirstOrDefault();
            if (finduser is null) return;
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.Navigation.PushAsync(new IncommingCallPage(finduser));
            });
        }
        private void Accepted(string id)
        {
            var finduser = Users.Where(u => u.ID == id).FirstOrDefault();
            if (finduser is null) return;
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.Navigation.PushAsync(new VideoCallPage(App.CurrentConnectedUser.ID, finduser.ID));
            });
        }
        private void Rejected()
        {
            try 
            {
                Console.WriteLine("Call rejected");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            //messages.Add("Call rejected");
        }
        private void CallEnded()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.Navigation.PopAsync();
            });
        }
        private async void AskPermissions()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    var response = await Permissions.RequestAsync<Permissions.Camera>();
                    if (response != PermissionStatus.Granted)
                    {
                    }
                }

                var statusMic = await Permissions.CheckStatusAsync<Permissions.Microphone>();
                if (statusMic != PermissionStatus.Granted)
                {
                    var response = await Permissions.RequestAsync<Permissions.Microphone>();
                    if (response != PermissionStatus.Granted)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
