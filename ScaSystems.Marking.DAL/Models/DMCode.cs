using System;
using System.Collections.Generic;
using System.Text;

namespace ScanSystems.Marking.DAL.Models
{
    public class DMCode : ModelBase<Guid>
    {
        public string DataMatrix { get; set; }
        public int DMCodeStateId { get; set; }
        public Guid? ProductId { get; set; }

        public Product Product { get; set; }
        public DMCodeState DMCodeState { get; set; }
    }
}
