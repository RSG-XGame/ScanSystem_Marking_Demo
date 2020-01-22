using System;
using System.Collections.Generic;
using System.Text;

namespace ScaSystems.Marking.DAL.Models
{
    public class CodeType : ModelBase<int>
    {
        public string Name { get; set; }
        public int ChildrenCodeTypeId { get; set; }
        public int MaxCountChildrens { get; set; }

        public CodeType ChildrenCodeType { get; set; }

        public List<RegisterCode> RegisterCodes { get; set; }
    }
}
