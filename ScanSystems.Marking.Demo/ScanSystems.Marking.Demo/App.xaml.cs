﻿using ScanSystems.Marking.DAL;
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

            MainPage = new MainPage() { BindingContext =  };
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
