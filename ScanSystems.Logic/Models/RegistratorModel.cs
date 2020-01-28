using Microsoft.EntityFrameworkCore;
using ScanSystems.Logic.Enums;
using ScanSystems.Marking.DAL;
using ScanSystems.Marking.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ScanSystems.Logic.Models
{
    public class RegistratorModel
    {
        public delegate void FullPackageHandler(RegistratorModel registratorModel);

        public event FullPackageHandler FullPackage;

        private bool initialized = false;

        public RegistratorModel Parent { get; set; }
        public RegistratorModel Child { get; set; }
        public CodeType BaseCodeType { get; set; }
        public Guid ProductId { get; set; }

        public ObservableCollection<RegisterCode> Codes { get; }

        public RegistratorModel()
        {
            Codes = new ObservableCollection<RegisterCode>();
            Codes.CollectionChanged += Codes_CollectionChanged;
        }

        public RegistratorModel Initizlie(CodeType baseCodeType)
        {
            RegistratorModel result = null;
            if (baseCodeType != null)
            {
                if (initialized)
                {
                    result = InitializeInternal(baseCodeType);
                }
                else
                {
                    BaseCodeType = baseCodeType;
                    result = this;
                    initialized = true;
                }
            }
            return result;
        }

        private RegistratorModel InitializeInternal(CodeType baseCodeType)
        {
            RegistratorModel result = null;

            if (BaseCodeType.ChildrenCodeTypeId == baseCodeType.Id)
            {
                if (Child == null)
                {
                    Child = new RegistratorModel();
                    Child.Initizlie(baseCodeType);
                    Child.Parent = this;
                }
                result = Child;
            }
            else if (BaseCodeType.Id == baseCodeType.ChildrenCodeTypeId)
            {
                if (Parent == null)
                {
                    Parent = new RegistratorModel();
                    Parent.Initizlie(baseCodeType);
                    Parent.Child = this;
                }
                result = Parent;
            }
            else
            {
                if (Child != null)
                {
                    result = Child.InitializeInternal(baseCodeType);
                }
                if (result == null && Parent != null)
                {
                    result = Parent.InitializeInternal(baseCodeType);
                }
            }

            return result;
        }

        private void Codes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Parent != null)
            {
                if (Parent.BaseCodeType.MaxCountChildrens == Codes.Count || (Parent.BaseCodeType.MaxCountChildrens == 0))
                {
                    FullPackage?.Invoke(this);
                }
                else if (Child != null)
                {
                    FullPackage?.Invoke(this);
                }
            }
            else
            {
                if (Codes.Count > 0)
                    FullPackage?.Invoke(this);
            }
        }

        public RegistrationResult Registration(string dataMatrix)
        {
            RegistrationResult result = RegistrationResult.Success;
            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                DMCode dm = db.DMCodes.FirstOrDefault(x => x.DataMatrix == dataMatrix && x.DMCodeStateId == 1);
                if (dm != null)
                {
                    RegisterCode code = new RegisterCode
                    {
                        Id = Guid.NewGuid(),
                        CodeTypeId = BaseCodeType.Id,
                        CurrentCode = dm.Id
                    };
                    dm.DMCodeStateId = BaseCodeType.DMCodeStateId;
                    db.Entry(dm).State = EntityState.Modified;
                    if (Child == null)
                    {
                        dm.ProductId = ProductId;
                        db.RegisterCodes.Add(code);
                        db.SaveChanges();
                        Codes.Add(code);
                    }
                    else
                    {
                        if (Child.Codes.Count > 0)
                        {
                            foreach (var rcode in Child.Codes)
                            {
                                rcode.ParentCode = dm.Id;
                                db.Entry(rcode).State = EntityState.Modified;
                            }
                            db.RegisterCodes.Add(code);
                            db.SaveChanges();
                            Child.Codes.Clear();
                            Codes.Add(code);
                        }
                        else
                        {
                            result = RegistrationResult.NoChildren;
                        }
                    }
                }
                else
                {
                    result = RegistrationResult.CodeRegitered;
                }
            }
            return result;
        }
    }
}
