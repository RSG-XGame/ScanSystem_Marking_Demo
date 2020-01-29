using ScanSystems.Marking.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ScanSystems.Marking.Demo.Models
{
    public class ScanModeModel : INotifyPropertyChanged
    {
        private CodeType codeType;
        private bool selected;

        public CodeType CodeType { get => codeType; set => SetProperty(ref codeType, value); }
        public bool Selected { get => selected; set => SetProperty(ref selected, value); }
        public string Name => codeType.Name;

        public event PropertyChangedEventHandler PropertyChanged;

        private void SetProperty<T>(ref T source, T value, [CallerMemberName] string propertyName = "")
        {
            source = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
