using HWA.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace HWA.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}