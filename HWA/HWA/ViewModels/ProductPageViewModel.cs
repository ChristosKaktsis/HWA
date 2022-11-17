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
    public class ProductPageViewModel
    {
        public ProductPageViewModel()
        {
            ProductCollection = new ObservableCollection<StripeProduct>()
            {
                new StripeProduct{
                    Name = "Product 1" ,
                    Description = "Product 1 Description",
                    Link ="https://buy.stripe.com/test_00gfZUaJH9Rq3de6oo"
                },
                 new StripeProduct{
                    Name = "Product 2" ,
                    Description = "Product 2 Description",
                    Link ="https://buy.stripe.com/test_7sIaFA2dbbZyg00145"
                }
            };
            GoToCheckoutCommand = new Command<StripeProduct>(GoToCheckout);

        }
        public async void OnAppearing()
        {
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            try
            {
                ProductManager manager = new ProductManager();
                var items = await manager.GetStripeProducts();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async void GoToCheckout(StripeProduct product)
        {
            if(product == null) return;
            await Shell.Current.Navigation.PushAsync(new CheckoutPage(product.Link));
        }

        public Command<StripeProduct> GoToCheckoutCommand { get; }
        public ObservableCollection<StripeProduct> ProductCollection 
        { get; set; } 

    }
}
