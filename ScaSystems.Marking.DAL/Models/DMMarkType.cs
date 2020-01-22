using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ScaSystems.Marking.DAL.Models
{
    [Table(name: "DMMarkTypes", Schema = "scansys")]
    public class DMMarkType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
