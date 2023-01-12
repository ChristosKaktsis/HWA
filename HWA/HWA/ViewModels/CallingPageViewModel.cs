using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class CallingPageViewModel
    {
        public async Task RejectCall(string id)
        {
            await App.Service.RejectCall(id);
            //await App.Service.DisconnectToHub();
            await Shell.Current.Navigation.PopAsync();
        }
    }
}
