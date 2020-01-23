using System;
using System.Collections.Generic;
using System.Text;

namespace ScanSystems.Marking.DAL.Models
{
    public class RegisterCode : ModelBase<Guid>
    {
        public Guid ChildrenCode { get; set; }
        public Guid ParentCode { get; set; }
        public int CodeTypeId { get; set; }

        public CodeType CodeType { get; set; }
    }
}
