using System;
using System.Collections.Generic;
using System.Text;

namespace ScanSystems.Marking.DAL.Models
{
    public abstract class ModelBase<T>
    {
        public T Id { get; set; }
    }
}
