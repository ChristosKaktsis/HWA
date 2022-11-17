using HWA.Data;
using HWA.Services;
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
        public static Models.User CurrentConnectedUser { get; set; } = new Models.User { Name = "John the hard coded" };
        public App()
        {
            DevExpress.XamarinForms.Editors.Initializer.Init();
            DevExpress.XamarinForms.CollectionView.Initializer.Init();
            DevExpress.XamarinForms.Popup.Initializer.Init();
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InstalledUICulture;
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
