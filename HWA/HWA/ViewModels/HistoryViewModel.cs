using HWA.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HWA.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        public ObservableCollection<History> HistoryCollection { get; set; }
        CustomerManager manager ;
        public HistoryViewModel()
        {
            HistoryCollection = new ObservableCollection<History>();
            manager = new CustomerManager();
        }
        public async void OnAppearing()
        {
            await CustomerHistory();
        }
        private async Task CustomerHistory()
        {
            try
            {
                var cust = App.Customer;
                var result = await manager.GetHistory(
                    cust.InsuranceProgramID, cust.ID.ToString(), 
                    Preferences.Get("CustomerCode", ""), Preferences.Get("ContractNo", ""));
                var items = result.OrderByDescending(x => x.ClaimDate);
                foreach (var item in items)
                    HistoryCollection.Add(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
