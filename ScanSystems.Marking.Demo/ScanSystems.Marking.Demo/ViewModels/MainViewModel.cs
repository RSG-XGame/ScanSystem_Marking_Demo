using Microsoft.EntityFrameworkCore;
using ScanSystems.Marking.DAL;
using ScanSystems.Marking.DAL.Models;
using ScanSystems.Marking.Demo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ScanSystems.Marking.Demo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool initialized = false;

        public ICommand ShownCommand { get; private set; }
        public ICommand SignInCommand { get; private set; }
        public ICommand SignOutCommand { get; private set; }

        protected override void Initialization()
        {
            ShownCommand = new Command(Shown);

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
                if (user!= null && user.LastSignIn.AddDays(1) >= DateTime.Now)
                {
                    user.DeviceSerialNumber = null;
                    db.Entry(user).State = EntityState.Modified;
                }
                else
                {
                    signedIn = true;
                }
                db.SaveChanges();
            }

            if (signedIn)
            {
                Navigation.PushAsync(new MenuView { BindingContext = this }, animated);
            }
            else
            {
                Navigation.PushAsync(new LoginView { BindingContext = this }, animated);
            }
        }

        private void SignIn()
        {
        }
        private void SignOut()
        {
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
