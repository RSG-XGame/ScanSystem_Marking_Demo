using ScanSystems.Marking.DAL;
using ScanSystems.Marking.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScanSystems.Logic
{


    public class ScanRegister
    {
        private CodeType baseCodeType;

        public ScanRegister()
        {

        }

        public void Initialize(CodeType baseCodeType)
        {
            this.baseCodeType = baseCodeType;
            using (ScanSystemsContext db = new ScanSystemsContext())
            {
            }
        }
    }
}
