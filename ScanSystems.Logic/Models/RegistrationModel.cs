using Microsoft.EntityFrameworkCore;
using ScanSystems.Marking.DAL;
using ScanSystems.Marking.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ScanSystems.Logic.Models
{
    public class RegistrationModel : IDisposable
    {
        private CodeType baseCodeType;
        private RegistrationModel child;
        private RegistrationModel parent;
        private bool initialized;

        private RegistrationModel lastChild;
        private RegistrationModel firstParent;

        public ObservableCollection<RegisterCode> RegisteredCodes { get; private set; }
        public RegistrationModel LastChild => lastChild;
        public RegistrationModel FirstParent => firstParent;

        public RegistrationModel()
        {
            RegisteredCodes = new ObservableCollection<RegisterCode>();
            initialized = false;
        }

        public void Initialize(CodeType baseCodeType, NotifyCollectionChangedEventHandler collectionChanged)
        {
            if (initialized)
            {
                if (baseCodeType.Id == this.baseCodeType.ChildrenCodeTypeId)
                {
                    if (parent == null)
                    {
                        parent = new RegistrationModel();
                        parent.child = this;

                    }
                    parent.Initialize(baseCodeType, collectionChanged);
                }
                else if (this.baseCodeType.Id == baseCodeType.ChildrenCodeTypeId)
                {
                    if (child == null)
                    {
                        child = new RegistrationModel();
                        child.parent = this;
                    }
                    child.Initialize(baseCodeType, collectionChanged);
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                this.baseCodeType = baseCodeType;
                RegisteredCodes.CollectionChanged += collectionChanged;
                initialized = true;
            }
        }

        public RegistrationModel GetLastChild()
        {
            if (lastChild == null)
                lastChild = child == null ? this : child.GetLastChild();
            return lastChild;
        }
        public RegistrationModel GetFirstParent()
        {
            if (firstParent == null)
                firstParent = parent == null ? this : parent.GetFirstParent();
            return firstParent;
        }

        public bool Registration(string value)
        {
            bool result = false;
            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                var dm = db.DMCodes.FirstOrDefault(x => x.DataMatrix == value && x.DMCodeStateId == 1);
                if (dm != null)
                {
                    RegisterCode newCode = new RegisterCode
                    {
                        Id = Guid.NewGuid(),
                        CurrentCode = dm.Id,
                        CodeTypeId = baseCodeType.Id
                    };
                    dm.DMCodeStateId = baseCodeType.DMCodeStateId;
                    if (child != null && child.RegisteredCodes.Count == baseCodeType.MaxCountChildrens)
                    {
                        foreach (var code in child.RegisteredCodes)
                        {
                            code.ParentCode = newCode.Id;
                            db.Entry(code).State = EntityState.Modified;
                        }
                    }
                    db.Entry(dm).State = EntityState.Modified;
                    db.RegisterCodes.Add(newCode);
                    db.SaveChanges();
                    RegisteredCodes.Add(newCode);
                    result = true;
                }
            }
            return result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                    if (child != null && !child.disposedValue)
                    {
                        child.RegisteredCodes.CollectionChanged -= RegisteredCodes_CollectionChanged;
                    }
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~RegisteredCodeTypeModel()
        // {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
