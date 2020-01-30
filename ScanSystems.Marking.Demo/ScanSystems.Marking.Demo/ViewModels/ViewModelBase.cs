using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;

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
        protected void ShowMessage(string title, string message)
        {
            string accept = "ОК";
            string cancel = string.Empty;

            Navigation.NavigationStack[Navigation.NavigationStack.Count - 1].DisplayAlert(title, message, accept, cancel);
        }
        protected async Task ShowMessageAsync(string title, string message)
        {
            string accept = "ОК";
            string cancel = string.Empty;

            await Navigation.NavigationStack[Navigation.NavigationStack.Count - 1].DisplayAlert(title, message, accept, cancel);
        }
        protected void CloseModal()
        {
            Navigation.PopModalAsync(animated);
        }

        protected bool SetProperty<T>(ref T source, T value, [CallerMemberName] string propertyName = "", Action<T> postAction = null)
        {
            bool result = false;
            if (!EqualityComparer<T>.Default.Equals(source, value))
            {
                source = value;
                result = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                postAction?.Invoke(source);
            }
            return result;
        }
        protected void PropertyUpdated([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected async Task<Result> Scanning(BarcodeFormat barcodeFormat)
        {
            var options = new MobileBarcodeScanningOptions();
            options.PossibleFormats = new List<ZXing.BarcodeFormat>() { barcodeFormat };

            var scanner = new MobileBarcodeScanner();
            return await scanner.Scan(options);
        }

        protected virtual void Initialization()
        {

        }
    }
}
