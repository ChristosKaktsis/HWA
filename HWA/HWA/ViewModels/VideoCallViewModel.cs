using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class VideoCallViewModel
    {
        public async Task EndCall(string cid)
        {
            await App.Service.EndCall(cid);
        }
    }
}
