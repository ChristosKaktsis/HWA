using HWA.Models;
using HWA.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    internal class ProductDetailViewModel
    {
        public StripeProduct Product { get; set; }
        public Command CheckoutCommand { get; set; }
        public Command CancelCommand { get; set; }
        public ProductDetailViewModel(StripeProduct product)
        {
            Product = product;
            CheckoutCommand = new Command(GoToCheckout);
            CancelCommand = new Command(Cancel);
        }

        private async void Cancel(object obj)
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async void GoToCheckout()
        {
            if (Product == null) return;
            await Shell.Current.Navigation.PushAsync(new CheckoutPage(Product.StripePaymentLink));
        }
    }
}
