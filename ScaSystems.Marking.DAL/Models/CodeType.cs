using System;
using System.Collections.Generic;
using System.Text;

namespace ScanSystems.Marking.DAL.Models
{
    public class CodeType : ModelBase<int>
    {
        public string Name { get; set; }
        public int? ChildrenCodeTypeId { get; set; }
        public int MaxCountChildrens { get; set; }
        public int DMCodeStateId { get; set; }
        public string MapCode { get; set; }
        public string SelectableFor { get; set; }

        public DMCodeState DMCodeState { get; set; }
        public CodeType ChildrenCodeType { get; set; }

        public List<RegisterCode> RegisterCodes { get; set; }
    }
}
