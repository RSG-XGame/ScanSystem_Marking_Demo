using ScanSystems.Marking.DAL;
using ScanSystems.Marking.Demo.ViewModels;
using ScanSystems.Marking.Demo.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScanSystems.Marking.Demo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            BannerView mainView = new BannerView();
            MainViewModel viewModel = new MainViewModel { Navigation = mainView.Navigation };
            mainView.BindingContext = viewModel;

            MainPage = new NavigationPage(mainView);
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
