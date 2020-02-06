using ScanSystems.Marking.DAL;
using ScanSystems.Marking.DAL.Models;
using ScanSystems.Marking.Demo.Models;
using ScanSystems.Marking.Demo.Views;
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
        private Product product;

        public ObservableCollection<CodeType> MainCodeTypes { get; private set; }
        public ObservableCollection<ScanModeModel> OptionalCodeTypes { get; private set; }

        public CodeType SelectedMainCodeType { get => selectedMainCodeType; set => SetProperty(ref selectedMainCodeType, value, postAction: SelectedMainCodeType_Changed); }
        public string ProductName { get => product?.Name ?? string.Empty; }
        public bool NeedScanProduct => !SelectedMainCodeType?.ChildrenCodeTypeId.HasValue ?? false;

        public ICommand ScanCommand { get; private set; }
        public ICommand ScanProductCommand { get; private set; }

        protected override void Initialization()
        {
            ScanCommand = new Command(Scan);
            ScanProductCommand = new Command(ScanProduct);

            MainCodeTypes = new ObservableCollection<CodeType>();
            OptionalCodeTypes = new ObservableCollection<ScanModeModel>();

            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                db.CodeTypes.OrderBy(x => x.Id).ToList().ForEach((x) =>
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
        private async void ScanProduct()
        {
            var result = await Scanning(ZXing.BarcodeFormat.All_1D);
            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                using (ScanSystemsContext db = new ScanSystemsContext())
                {
                    var product = db.Products.FirstOrDefault(x => x.Barcode == result.Text);
                    if (product != null)
                    {
                        this.product = product;
                        PropertyUpdated(nameof(ProductName));
                    }
                    else 
                    {
                        await ShowMessageAsync("Ошибка!", "Продукт не найден!");
                    }
                }
            }
        }
        private void Scan()
        {
            CodeType[] codeTypes = null;
            int requestCodeTypes = 1 + OptionalCodeTypes.Count(x => x.Selected);
            string map = $"[{SelectedMainCodeType.Id}]";
            if ((product != null && !SelectedMainCodeType.ChildrenCodeTypeId.HasValue) || SelectedMainCodeType.ChildrenCodeTypeId.HasValue)
            {
                foreach (var m in OptionalCodeTypes.Where(x => x.Selected).OrderBy(x => x.CodeType.Id))
                {
                    map += $"[{m.CodeType.Id}]";
                }

                using (ScanSystemsContext db = new ScanSystemsContext())
                {
                    codeTypes = db.CodeTypes.Where(x => x.MapCode.Contains(map)).ToArray();
                }

                if (codeTypes != null && codeTypes.Length == requestCodeTypes)
                {
                    ScanViewModel model = new ScanViewModel { Navigation = Navigation };
                    model.InitializeRegistrator(codeTypes);
                    Navigation.PopAsync();
                    Navigation.PushAsync(new ScanView { BindingContext = model }, animated);
                }
                else
                {
                    ShowMessage("Ошибка!", "Не удалось установить тип сканирования.");
                }
            }
            else
            {
                ShowMessage("Ошибка!", "Не отсканирован продукт!");
            }
        }
        private void SelectedMainCodeType_Changed(CodeType codeType)
        {
            OptionalCodeTypes.Clear();
            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                string search = $"[{codeType.Id}]";
                db.CodeTypes.Where(x => x.SelectableFor.Contains(search)).OrderBy(x => x.Id).ToList()
                    .ForEach((x) =>
                    {
                        OptionalCodeTypes.Add(new ScanModeModel { CodeType = x });
                    });
            }
            PropertyUpdated(nameof(NeedScanProduct));
        }
    }
}
