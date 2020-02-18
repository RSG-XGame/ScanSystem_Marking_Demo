using Microsoft.EntityFrameworkCore;
using ScanSystems.Marking.DAL;
using ScanSystems.Marking.DAL.Models;
using ScanSystems.Marking.Demo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ScanSystems.Marking.Demo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly bool initialized = false;
        private string login;
        private string password;
        private Guid? userId;

        public string AssemblyVersion
        {
            get
            {
                string version = global::Xamarin.Essentials.AppInfo.VersionString;
                return $"Версия: {version}";
            }
        }
        public string AssemblyCopyright
        {
            get
            {
                return "Copyright © 2020";
            }
        }

        public string Login { get { return login; } set { SetProperty(ref login, value); } }
        public string Password { get { return password; } set { SetProperty(ref password, value); } }

        public ICommand ShownCommand { get; private set; }
        public ICommand SignInCommand { get; private set; }
        public ICommand SignOutCommand { get; private set; }
        public ICommand ScanModeCommand { get; private set; }
        public ICommand DataCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }

        protected override void Initialization()
        {
            ShownCommand = new Command(Shown);
            SignInCommand = new Command(SignIn);
            SignOutCommand = new Command(SignOut);
            ScanModeCommand = new Command(ScanMode);
            DataCommand = new Command(Data);
            SettingsCommand = new Command(Settings);

            if (!initialized)
            {
                using (ScanSystemsContext db = new ScanSystemsContext())
                {
                    db.Database.EnsureCreated();
                    db.SaveChanges();
                }
            }

            base.Initialization();
        }

        private void Shown()
        {
            bool signedIn = false;

            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                var user = db.Users.FirstOrDefault(x => x.DeviceSerialNumber == GetDeviceId());
                if (user != null)
                {
                    if (user.LastSignIn.AddDays(1) < DateTime.Now)
                    {
                        user.DeviceSerialNumber = null;
                    }
                    else
                    {
                        user.LastSignIn = DateTime.Now;
                        signedIn = true;
                    }
                    db.Entry(user).State = EntityState.Modified;
                }
                db.SaveChanges();
            }

            if (signedIn)
            {
                Navigation.PopToRootAsync();
                Navigation.PushAsync(new MenuView { BindingContext = this }, animated);
            }
            else
            {
                Navigation.PopToRootAsync();
                Navigation.PushAsync(new LoginView { BindingContext = this }, animated);
            }
        }

        private void ScanMode()
        {
            Navigation.PopToRootAsync();
            Navigation.PushAsync(new ScanModeView { BindingContext = new ScanModeViewModel { Navigation = Navigation } }, animated);
        }
        private void Data()
        {
            Navigation.PopToRootAsync();
            Navigation.PushAsync(new DataView { BindingContext = new DataViewModel { Navigation = Navigation } }, animated);
        }
        private void Settings()
        {
            bool openLogin = false;
            bool isAdmin = false;
            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                var user = userId.HasValue ? db.Users.FirstOrDefault(x => x.Id == userId.Value) : null;
                if (user == null)
                {
                    openLogin = true;
                    userId = null;
                }
                else
                {
                    isAdmin = user.IsAdmin;
                }
            }
            if (isAdmin)
            {
                Navigation.PushAsync(new SettingsView { BindingContext = new SettingsViewModel { Navigation = Navigation } }, animated);
            }
            else if (openLogin)
            {
                Navigation.PopToRootAsync(animated);
            }
            else
            {
                ShowMessage("Доступ запрещен!", $"У данного пользователя нет доступа{Environment.NewLine}к редактированию настроек!");
            }
        }
        private void SignIn()
        {
            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
                if (user == null)
                {
                    ShowMessage("Ошибка аутоидентификации!", "Некорректный логин или пароль!");
                }
                else
                {
                    user.DeviceSerialNumber = GetDeviceId();
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    userId = user.Id;
                    Login = "";
                    Password = "";
                    Navigation.PopAsync(animated);
                    //Navigation.RemovePage(Navigation.PopAsync().Result);
                    Navigation.PushAsync(new MenuView { BindingContext = this }, animated);
                }
            }
        }
        private void SignOut()
        {
            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                if (userId.HasValue) 
                {
                    var user = db.Users.FirstOrDefault(x => x.Id == userId.Value);
                    if (user != null)
                    {
                        user.DeviceSerialNumber = string.Empty;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    userId = null;
                }
            }
            userId = null;
            Navigation.PopToRootAsync();
            Navigation.PushAsync(new LoginView { BindingContext = this }, animated);
        }

        private string GetDeviceId()
        {
            string deviceId;
            string key = "UniqueDeviceId";

            if (!App.Current.Properties.ContainsKey(key))
            {
                deviceId = Guid.NewGuid().ToString();
                App.Current.Properties.Add(key, deviceId.ToString());
                App.Current.SavePropertiesAsync();
            }
            else
            {
                deviceId = App.Current.Properties[key].ToString();
            }

            return deviceId;
        }
    }
}
