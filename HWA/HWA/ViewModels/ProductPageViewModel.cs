using HWA.Data;
using HWA.Models;
using HWA.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class ProductPageViewModel : BaseViewModel
    {
        public ProductPageViewModel()
        {
            GoToDetailCommand = new Command<StripeProduct>(GoToDetail);
        }
        public async void OnAppearing()
        {
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            IsBusy = true;
            try
            {
                ProductCollection.Clear();
                ProductManager manager = new ProductManager();
                var items = await manager.GetStripeProducts();
                foreach (var item in items)
                    ProductCollection.Add(item);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void GoToDetail(StripeProduct product)
        {
            if(product == null) return;
            await Shell.Current.Navigation.PushAsync(new ProductDetailPage(product));
        }

        public Command<StripeProduct> GoToDetailCommand { get; }
        public ObservableCollection<StripeProduct> ProductCollection
        { get; set; } = new ObservableCollection<StripeProduct>();

    }
}
