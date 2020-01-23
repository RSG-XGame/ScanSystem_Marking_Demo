using System;
using System.Collections.Generic;
using System.Text;

namespace ScanSystems.Marking.DAL.Models
{
    public class DMCodeState : ModelBase<int>
    {
        public string Name { get; set; }

        public List<DMCode> Codes { get; set; }
    }
}
