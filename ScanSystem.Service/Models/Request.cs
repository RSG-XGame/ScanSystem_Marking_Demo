using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanSystem.Service.Models
{
    public class Request
    {
        public Guid RequestId { get; set; }
        public string Method { get; set; }
        public string Params { get; set; }
    }
}
