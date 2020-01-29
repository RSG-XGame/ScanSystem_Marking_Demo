using ScanSystems.Marking.DAL;
using ScanSystems.Marking.DAL.Models;
using ScanSystems.Marking.Demo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ScanSystems.Marking.Demo.ViewModels
{
    public class ScanModeViewModel : ViewModelBase
    {
        private CodeType selectedMainCodeType;

        public ObservableCollection<CodeType> MainCodeTypes { get; private set; }
        public ObservableCollection<ScanModeModel> OptionalCodeTypes { get; private set; }

        public CodeType SelectedMainCodeType { get => selectedMainCodeType; set => SetProperty(ref selectedMainCodeType, value, postAction: SelectedMainCodeType_Changed); }

        public ICommand ScanCommand { get; private set; }

        protected override void Initialization()
        {
            ScanCommand = new Command(Scan);

            MainCodeTypes = new ObservableCollection<CodeType>();
            OptionalCodeTypes = new ObservableCollection<ScanModeModel>();

            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                db.CodeTypes.Where(x => x.Selectable).OrderBy(x => x.Id).ToList().ForEach((x) =>
                    {
                        MainCodeTypes.Add(x);
                    });
            }

            if (MainCodeTypes.Count > 0)
            {
                SelectedMainCodeType = MainCodeTypes.First();
            }

            base.Initialization();
        }
        private void Scan()
        {
            string map = $"[{SelectedMainCodeType.Id}]";
            foreach (var m in OptionalCodeTypes.Where(x => x.Selected).OrderBy(x => x.CodeType.Id))
            {
                map += $"[{m.CodeType.Id}]";
            }
            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                db.CodeTypes.FirstOrDefault(x => x.MapCode.Contains(map));
            }
        }
        private void SelectedMainCodeType_Changed(CodeType codeType)
        {
            OptionalCodeTypes.Clear();
            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                db.CodeTypes.Where(x => x.ChildrenCodeTypeId == codeType.Id).OrderBy(x => x.Id).ToList()
                    .ForEach((x) =>
                    {
                        OptionalCodeTypes.Add(new ScanModeModel { CodeType = x });
                    });
            }
        }
    }
}
