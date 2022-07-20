using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class MenuItemViewModel
    {
        public MenuItemViewModel()
        {
            OpenServiceCommand = new Command(async () => await Shell.Current.GoToAsync(PageName));
        }
        public ImageSource Image { get; set; }
        public string ItemName { get; set; }
        //the instanse of the page is created in main menu so the view model will live until main page is off
        //public Page Page { get; set; }
        //use string to navigate insted of page 
        public string PageName { get; set; }
        private bool isVisible = true;
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        public Command OpenServiceCommand { get; set; }
    }
}
