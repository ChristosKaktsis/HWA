using HWA.Data;
using HWA.Resources;
using HWA.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command GoToDocLoginCommand { get; }
        CustomerManager customerManager = new CustomerManager();
        private bool _ContracrHasError, _CodeHasError, _PhoneHasError, _EmailHasError, _Error;
        private string _ErrorText;

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            GoToDocLoginCommand = new Command(GoToDocLogin);
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
        public bool ContracrHasError 
        { 
            get => _ContracrHasError; 
            set => SetProperty(ref _ContracrHasError, value); 
        }
        public bool CodeHasError 
        { 
            get => _CodeHasError; 
            set => SetProperty(ref _CodeHasError, value); 
        }
        public bool PhoneHasError 
        { 
            get => _PhoneHasError; 
            set => SetProperty(ref _PhoneHasError, value); 
        }
        public bool EmailHasError 
        { 
            get => _EmailHasError; 
            set => SetProperty(ref _EmailHasError, value); 
        }
        public bool HasError
        {
            get => _Error;
            set => SetProperty(ref _Error, value);
        }
        public string ErrorText
        {
            get => _ErrorText;
            set => SetProperty(ref _ErrorText, value);
        }

        //keep user logedin after success
        public async void OnAppearing()
        {
            if (!Preferences.ContainsKey("customer_id"))
                return;
            var cid = Preferences.Get("customer_id", "");
            try
            {
                IsBusy = true;
                if (await CustomerHasError(cid)) return;

                App.CurrentConnectedUser = new Models.User { Name = App.Customer.Name };
                await Shell.Current.GoToAsync($"//{nameof(MainMenu)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Shell.Current.DisplayAlert($"{AppResources.error}", $"{AppResources.smt_wrong}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
        /// <summary>
        /// Sets The App Customer And Checks if there is an error
        /// </summary>
        /// <param name="cid"></param>
        /// <returns>true if App Customer Has ErrorMessage</returns>
        private async Task<bool> CustomerHasError(string cid)
        {
            App.Customer = await customerManager.GetCustomerInfo(cid);
            App.Customer.ID = Guid.Parse(cid);
            if (HasError = !string.IsNullOrEmpty(App.Customer.ErrorMessage))
                ErrorText = App.Customer.ErrorMessage;
            return HasError;
        }
        private async void OnLoginClicked()
        {
            try
            {
                IsBusy = true;
                HasError = false;
                if (CheckForErrors())
                    return;
                //"11512517", "12343562", "6978894988", "test@test.gr"
                var result = await customerManager.CheckCustomer(ContractNo, CustomerCode, Phone, Email);
                //Customer does not exists or is not Active
                if (HasError = !result.IsSuccessful)
                {
                    ErrorText = result.ErrorMessage;
                    return;
                }
                if (await CustomerHasError(result.Info))
                    return;
                Preferences.Set("customer_id", result.Info);
                App.CurrentConnectedUser = new Models.User { Name = App.Customer.Name };
                await Shell.Current.GoToAsync($"//{nameof(MainMenu)}");
            }
            catch(System.Net.Http.HttpRequestException httpex)
            {
                Console.WriteLine(httpex);
                await Shell.Current.DisplayAlert($"{AppResources.error}", $"{AppResources.smt_wrong}", "Ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Shell.Current.DisplayAlert($"{AppResources.error}", $"{AppResources.smt_wrong}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private bool CheckForErrors()
        {
            ContracrHasError = CodeHasError = PhoneHasError = EmailHasError = false;
            ContracrHasError = string.IsNullOrWhiteSpace(ContractNo);
            CodeHasError = string.IsNullOrWhiteSpace(CustomerCode);
            PhoneHasError = string.IsNullOrWhiteSpace(Phone);
            //EmailHasError = string.IsNullOrWhiteSpace(Email);
            return ContracrHasError || CodeHasError || PhoneHasError || EmailHasError;
        }
        private async void GoToDocLogin(object obj)
        {
            await Shell.Current.GoToAsync("//LoginDoctor");
        }
    }
}
