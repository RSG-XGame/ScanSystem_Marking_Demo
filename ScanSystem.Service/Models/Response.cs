using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanSystem.Service.Models
{
    public class Response
    {
        public Guid ResponseId { get; set; }
        public string Data { get; set; }
        public string Result { get; set; }
    }
}
