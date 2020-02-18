using ScanSystems.Marking.DAL;
using ScanSystems.Marking.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ScanSystems.Marking.Demo.ViewModels
{
    public class DataViewModel : ViewModelBase
    {
        public ObservableCollection<RegisterCode> RegisteredCodes { get; private set; }

        public ICommand DetailsCommand { get; private set; }

        protected override void Initialization()
        {
            RegisteredCodes = new ObservableCollection<RegisterCode>();
        
            DetailsCommand = new Command(Details);

            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                db.RegisterCodes.ToList().ForEach((x) =>
                {
                    RegisteredCodes.Add(x);
                });
            }
        }

        private void Details(object obj)
        {
            RegisterCode registeredCode = obj as RegisterCode;
        }
    }
}
