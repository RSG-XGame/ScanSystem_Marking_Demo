using System;
using System.Collections.Generic;
using System.Text;

namespace ScanSystems.Marking.DAL.Models
{
    public class User : ModelBase<Guid>
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string DeviceSerialNumber { get; set; }
        public DateTime LastSignIn { get; set; }
        public bool IsAdmin { get; set; }
    }
}
