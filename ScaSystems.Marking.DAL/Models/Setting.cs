using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ScaSystems.Marking.DAL.Models
{
    [Table(name: "Settings", Schema = "scansys")]
    public class Setting
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
