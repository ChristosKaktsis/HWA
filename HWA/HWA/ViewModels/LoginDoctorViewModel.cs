using HWA.Data;
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
                var manager = new CustomerManager();
                var result = await manager.GetDoctorAccount(UserName, Password);
                if(HasError = result.StatusCode !="200")
                {
                    ErrorText = result.Status;
                    return;
                }
                App.CurrentConnectedUser = new Models.User { Name = result.Info , IsDoctor = true};
                await Shell.Current.GoToAsync($"//{nameof(MainMenu)}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await Shell.Current.DisplayAlert($"{AppResources.error}", $"{AppResources.smt_wrong}", "Ok");
            }
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
