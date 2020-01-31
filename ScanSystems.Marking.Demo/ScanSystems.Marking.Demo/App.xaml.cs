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
            MainViewModel viewModel = new MainViewModel();
            mainView.BindingContext = viewModel;

            NavigationPage page = new NavigationPage(mainView);
            viewModel.Navigation = page.Navigation;

            MainPage = page;
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
