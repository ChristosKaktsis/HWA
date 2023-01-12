using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public Command LogoutCommand { get; }
        public ProfileViewModel()
        {
            LogoutCommand = new Command(async () => await Logout());
        }
        public string ContractNo
        {
            get => Preferences.Get(nameof(ContractNo), "");
            set => Preferences.Set(nameof(ContractNo), value);
        }
        public string CustomerCode
        {
            get => Preferences.Get(nameof(CustomerCode), "");
            set => Preferences.Set(nameof(CustomerCode), value);
        }
        public string Phone
        {
            get => Preferences.Get(nameof(Phone), "");
            set => Preferences.Set(nameof(Phone), value);
        }
        public string Email
        {
            get => Preferences.Get(nameof(Email), "");
            set => Preferences.Set(nameof(Email), value);
        }
        private async Task Logout()
        {
            Preferences.Remove("customer_id");
            //disconnect from hub
            if (App.Service.IsConnected)
                await App.Service.DisconnectToHub();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
