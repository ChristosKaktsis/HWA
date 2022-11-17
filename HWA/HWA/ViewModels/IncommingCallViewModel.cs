using HWA.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class IncommingCallViewModel
    {
        public IncommingCallViewModel()
        {
        }
        public async Task AcceptCall(string id)
        {
            await App.Service.AcceptCall(id);
            //await App.Service.DisconnectToHub();
            await Shell.Current.Navigation.PopAsync();
            await Shell.Current.Navigation.PushAsync(new VideoCallPage(App.CurrentConnectedUser.ID, id));
        }
        public async Task RejectCall(string id)
        {
            await App.Service.RejectCall(id);
            //await App.Service.DisconnectToHub();
            await Shell.Current.Navigation.PopAsync();
        }
    }
}
