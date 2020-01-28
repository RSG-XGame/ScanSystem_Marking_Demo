using ScanSystems.Logic.Models;
using ScanSystems.Marking.DAL;
using ScanSystems.Marking.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ScanSystems.Logic.Controllers
{
    public class Registrator
    {
        private List<RegistratorModel> models;
        private RegistratorModel baseRegistratorModel;
        private RegistratorModel child;
        private Product product;

        public RegistratorModel CurrentRegistratorModel { get; set; }
        public Product Product => product;

        public Registrator()
        {
            models = new List<RegistratorModel>();
        }

        public void Initialize(CodeType baseCodeType, Product product)
        {
            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                models.AddRange(InitializeModels(db.CodeTypes.ToList(), baseCodeType.Id));
                models.ForEach(x => x.FullPackage += FullPackage);
                child = models.First(x => x.Child == null);
                this.product = product;
                child.ProductId = product.Id;
                CurrentRegistratorModel = child;
            }
        }

        private void FullPackage(RegistratorModel registratorModel)
        {
            if (CurrentRegistratorModel == registratorModel)
            {
                if (registratorModel.Parent == null)
                {
                    registratorModel.Codes.Clear();
                    CurrentRegistratorModel = child;
                }
                else
                {
                    if (registratorModel.Codes.Count == registratorModel.Parent.BaseCodeType.MaxCountChildrens)
                    {
                        CurrentRegistratorModel = registratorModel.Parent;
                    }
                    else
                    {
                        if (registratorModel.Child != null)
                        {
                            CurrentRegistratorModel = registratorModel.Child;
                        }
                        else
                        {
                            CurrentRegistratorModel = child;
                        }
                    }
                }
            }
            //if (registratorModel.Parent == null)
            //{
            //    registratorModel.Codes.Clear();
            //    CurrentRegistratorModel = child;
            //}
            //else
            //{
            //    if (registratorModel.Parent != null)
            //    {
            //        CurrentRegistratorModel = registratorModel.Parent;
            //    }
            //}
        }

        private IEnumerable<RegistratorModel> InitializeModels(IEnumerable<CodeType> codeTypes, int id)
        {
            int cId = id;
            while (cId != -1)
            {
                var codeType = codeTypes.FirstOrDefault(x => x.Id == cId);

                if (codeType != null)
                {
                    if (baseRegistratorModel == null)
                    {
                        baseRegistratorModel = new RegistratorModel();
                    }
                    cId = codeType.ChildrenCodeTypeId.HasValue ? codeType.ChildrenCodeTypeId.Value : -1;
                }
                yield return baseRegistratorModel.Initizlie(codeType);
            }
        }

        public void Registration(string dataMatrix)
        {
            CurrentRegistratorModel.Registration(dataMatrix);
        }
    }
}
