using HWA.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HWA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuItemView : Frame
    {
        //private MenuItemViewModel _viewModel;

        public MenuItemView()
        {
            InitializeComponent();
            //BindingContext = _viewModel = new MenuItemViewModel();
        }
    }
}