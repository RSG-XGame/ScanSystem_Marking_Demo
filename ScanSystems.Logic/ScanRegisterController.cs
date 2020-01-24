using ScanSystems.Logic.Models;
using ScanSystems.Marking.DAL;
using ScanSystems.Marking.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScanSystems.Logic
{


    public class ScanRegisterController
    {
        private CodeType baseCodeType;
        private RegistrationModel currentRegisterCode;

        public ScanRegisterController()
        {
            currentRegisterCode = new RegistrationModel();
        }

        public void Initialize(CodeType baseCodeType)
        {
            this.baseCodeType = baseCodeType;
            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                int? id = baseCodeType.Id;
                db.CodeTypes.OrderBy(x => x.Id).ToList().ForEach((x) =>
                {
                    if (id.HasValue && x.Id == id.Value)
                    {
                        currentRegisterCode.Initialize(x, RegisteredCodes_CollectionChanged);
                        id = x.ChildrenCodeTypeId;
                    }
                });
            }
            currentRegisterCode = currentRegisterCode.GetLastChild();
        }

        private void RegisteredCodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }

        public void RegisterScan(string dataMatrix)
        {
            currentRegisterCode.Registration(dataMatrix);
        }
    }
}
