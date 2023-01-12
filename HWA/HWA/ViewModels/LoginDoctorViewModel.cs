using HWA.Data;
using HWA.Network;
using HWA.Resources;
using HWA.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class LoginDoctorViewModel : BaseViewModel
    {
        public LoginDoctorViewModel()
        {
            LoginCommand = new Command(OnLogin);
            GoToLoginCommand = new Command(GoToLogin);
        }

        private async void GoToLogin(object obj)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnLogin()
        {
            if (IsSomethingEmpty()) return;
            try
            {
                IsBusy= true;
                var manager = new CustomerManager();
                var result = await manager.GetDoctorAccount(UserName, Password);
                if(HasError = result.StatusCode !="200")
                {
                    ErrorText = result.Status;
                    return;
                }
                App.CurrentConnectedUser = new Models.User { Name = result.Info , IsDoctor = true};
                SendToken(App.Token);
                await Shell.Current.GoToAsync($"//{nameof(MainMenu)}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await Shell.Current.DisplayAlert($"{AppResources.error}", $"{AppResources.smt_wrong}", "Ok");
            }
            finally { IsBusy = false; }
        }
        private async void SendToken(string token)
        {
            if(string.IsNullOrEmpty(token)) return;
            await TokenApi.SendToken(token);
        }
        private bool IsSomethingEmpty()
        {
            return IsEmpty(UserName) || IsEmpty(Password);
        }
        private bool IsEmpty(string text)
        {
            return string.IsNullOrEmpty(text);
        }
        public string UserName
        {
            get => Preferences.Get(nameof(UserName), "");
            set => Preferences.Set(nameof(UserName), value);
        }
        public string Password
        {
            get => Preferences.Get(nameof(Password), "");
            set => Preferences.Set(nameof(Password), value);
        }
        private string _ErrorMessage;
        public string ErrorText
        {
            get { return _ErrorMessage; }
            set { SetProperty(ref _ErrorMessage, value); }
        }
        private bool _HasError;

        public bool HasError
        {
            get { return _HasError; }
            set => SetProperty(ref _HasError, value);
        }
        public Command LoginCommand { get; }
        public Command GoToLoginCommand { get; }
    }
}
