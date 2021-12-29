using System;
using UnosquareTest.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UnosquareTest
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new EmployeesView());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
