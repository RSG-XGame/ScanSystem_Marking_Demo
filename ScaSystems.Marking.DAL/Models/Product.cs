﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ScaSystems.Marking.DAL.Models
{
    public class Product : ModelBase<Guid>
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
    }
}
