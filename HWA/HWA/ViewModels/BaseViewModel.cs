using HWA.Models;
using HWA.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        bool isBusy = false;
        bool lockButton = true;
        public bool IsBusy
        {
            get { return isBusy; }
            set 
            { 
                SetProperty(ref isBusy, value);
                LockButton = !value;
            }
        }
        public bool LockButton
        {
            get { return lockButton; }
            set { SetProperty(ref lockButton, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        private DateTime currentDate = DateTime.Now;
        public DateTime CurrentDate 
        { 
            get => currentDate; 
            set => currentDate = value;
        }
        protected async void Cancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        protected async static Task ToastUser(bool isOk, string text)
        {
            await Shell.Current.DisplayToastAsync(new Xamarin.CommunityToolkit.UI.Views.Options.ToastOptions
            {
                MessageOptions =
                {
                    Message = text
                },
                BackgroundColor = isOk ? Color.Green : Color.Red
            });
        }
        protected async static Task ToastUser(bool isOk,string positive, string negative)
        {
            await Shell.Current.DisplayToastAsync(new Xamarin.CommunityToolkit.UI.Views.Options.ToastOptions
            {
                MessageOptions =
                {
                    Message = isOk? positive : negative
                },
                BackgroundColor = isOk ? Color.Green : Color.Red
            });
        }
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
