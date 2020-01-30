using ScanSystems.Marking.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScanSystems.Marking.Demo.ViewModels
{
    public class ScanViewModel : ViewModelBase
    {
        private CodeType[] codeTypes;
        
        protected override void Initialization()
        {
            base.Initialization();
        }
        public void InitializeRegistrator(CodeType[] codeTypes)
        {
        }
    }
}
