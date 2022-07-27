using HWA.ViewModels;
using HWA.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HWA
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(DoctorAppointmentPage), typeof(DoctorAppointmentPage));
            Routing.RegisterRoute(nameof(CenterAppointmentPage), typeof(CenterAppointmentPage));
            Routing.RegisterRoute(nameof(CheckupPage), typeof(CheckupPage));
            Routing.RegisterRoute(nameof(HospitalizationPage), typeof(HospitalizationPage));
            Routing.RegisterRoute(nameof(NetListPage), typeof(NetListPage));
            Routing.RegisterRoute(nameof(ClaimsSubPage), typeof(ClaimsSubPage));
            Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
        }

    }
}
