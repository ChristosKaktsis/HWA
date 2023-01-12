using HWA.Models;
using HWA.Network;
using HWA.Resources;
using HWA.Services;
using HWA.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class CommunicationViewModel : BaseViewModel
    {
        public ObservableCollection<string> messages { get; set; }
        public ObservableCollection<User> users { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
        public Command SendMessageCommand { get; set; }
        public Command CallCommand { get; set; }
        public CommunicationViewModel()
        {
            SendMessageCommand = new Command(SendMessage);
            CallCommand = new Command(CallUser);
            messages = new ObservableCollection<string>();
            users = new ObservableCollection<User>();
            InitializeConnection();
        }
        public async void OnAppearing()
        {
            //await App.Service.DisconnectToHub();
            if (App.CurrentConnectedUser.IsDoctor) return;
            var result = await TokenApi.NotifyDoctor();
            ShowNotifyOnScreen(result);
        }

        private async void ShowNotifyOnScreen(bool result)
        {
            if (result)
                await Shell.Current.DisplayAlert(
                    AppResources.docinfo,
                    AppResources.notification_send, 
                    "OK");
            else
                await Shell.Current.DisplayAlert(
                    AppResources.notification_notsend,
                    AppResources.notification_error,
                    "OK");
        }

        private async void SendMessage()
        {
            if (!App.Service.IsConnected)
                return;
            try
            {
                await App.Service.SendMessage(User, Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private async void CallUser()
        {
            if (!App.Service.IsConnected)
                return;
            if (User is null)
                return;
            try
            {
                await App.Service.CallUser(User.ID);
                messages.Add("Calling ...");
                await Shell.Current.Navigation.PushAsync(new CallingPage(User));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private async void InitializeConnection()
        {
            //disconnect and connect again
            //otherwise user wil be 2 times connected
            //also need to connect again to set the methods 
            if (App.Service.IsConnected)
                await App.Service.DisconnectToHub();
            //set only user methods
            //all the other methods acceptcall recievecall... are set in mainmenu view model
            //and they work unless the main menu dies
            try
            {
                App.Service.ReceiveConnectedUsers(GetUsers);
                App.Service.ReceiveDisconnectedUser(RemoveUser);
                App.Service.ReceiveMessage(GetMessage);
                await App.Service.ConnectToHub();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private void GetMessage(string message)
        {
            messages.Add(message);
        }
        private void GetUsers(User user)
        {
            if (users.Where(u => u.ID == user.ID).Any()) return;
            if (user.ID == App.CurrentConnectedUser.ID) return;
            users.Add(new User { ID = user.ID, Name = user.Name });
        }
        private void RemoveUser(string id)
        {
            if (!users.Where(u => u.ID == id).Any())
                return;
            users.Remove(users.Where(u => u.ID == id).FirstOrDefault());
        }
    }
}
