using HWA.Data;
using HWA.Services;
using Plugin.FirebasePushNotification;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HWA
{
    public partial class App : Application
    {
        public static Customer Customer { get; set; }
        CustomerManager customerManager = new CustomerManager();
        public static SignalRService Service { get; set; }
        public static Models.User CurrentConnectedUser { get; set; }
        public static string Token
        {
            get => Preferences.Get(nameof(Token), null);
            set => Preferences.Set(nameof(Token), value);
        }

        public App(bool hasNotification = false, IDictionary<string, object> notificationData = null)
        {
            DevExpress.XamarinForms.Editors.Initializer.Init();
            DevExpress.XamarinForms.CollectionView.Initializer.Init();
            DevExpress.XamarinForms.Popup.Initializer.Init();
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InstalledUICulture;
            InitializeComponent();
            MainPage = new AppShell();
            // Token event
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
                Token = p.Token;
            };
            // Push message received event
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {

                System.Diagnostics.Debug.WriteLine("Received");

            };
            //Push message received event
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

            };
        }

        protected override void OnStart()
        {
            //await Shell.Current.GoToAsync("//LoginPage");
            //try
            //{
            //    if (Service.IsConnected) await Service.DisconnectToHub();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
            //DependencyService.Resolve<IForegroundService>().StopMyService();
            //DependencyService.Resolve<IForegroundService>().StartiMyService();
        }

        protected override void OnSleep()
        {
        }

        protected override  void OnResume()
        {
        }

    }
}
