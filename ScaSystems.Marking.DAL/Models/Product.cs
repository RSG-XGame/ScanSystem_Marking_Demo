using System;
using System.Collections.Generic;
using System.Text;

namespace ScanSystems.Marking.DAL.Models
{
    public class Product : ModelBase<Guid>
    {
        public string Name { get; set; }
        public string Barcode { get; set; }

        public List<DMCode> DMCodes { get; set; }
    }
}
