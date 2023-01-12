using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class VideoCallViewModel : BaseViewModel
    {
        private bool _IsCameraOn = true;
        private bool _IsMicOn = true;

        public async Task EndCall(string cid)
        {
            await App.Service.EndCall(cid);
        }
        public bool IsCameraOn 
        {
            get => _IsCameraOn;
            set => SetProperty(ref _IsCameraOn, value);
        }
        public bool IsMicOn
        {
            get => _IsMicOn;
            set => SetProperty( ref _IsMicOn, value);
        }
    }
}
