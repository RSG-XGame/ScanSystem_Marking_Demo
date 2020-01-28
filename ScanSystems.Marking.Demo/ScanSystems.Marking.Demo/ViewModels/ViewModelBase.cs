using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ScanSystems.Marking.Demo.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected const bool animated = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand BackCommand { get; private set; }
        public ICommand ReturnToMainMenuCommand { get; private set; }
        public ICommand CloseModalCommand { get; private set; }

        public INavigation Navigation { get; set; }

        public ViewModelBase()
        {
            InitializeCommands();
            Initialization();
        }

        private void InitializeCommands()
        {
            BackCommand = new Command(Back);
            ReturnToMainMenuCommand = new Command(ReturnToMainMenu);
            CloseModalCommand = new Command(CloseModal);
        }

        protected void Back()
        {
            Navigation.PopAsync(animated);
        }
        protected void ReturnToMainMenu()
        {
            Navigation.PopToRootAsync(animated);
        }
        protected void CloseModal()
        {
            Navigation.PopModalAsync(animated);
        }

        protected bool SetProperty<T>(ref T source, T value, [CallerMemberName] string propertyName = "")
        {
            bool result = false;
            if (!EqualityComparer<T>.Default.Equals(source, value))
            {
                source = value;
                result = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            return result;
        }

        protected virtual void Initialization()
        {

        }
    }
}
