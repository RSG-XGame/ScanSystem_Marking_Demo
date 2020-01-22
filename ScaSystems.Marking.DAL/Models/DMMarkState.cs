using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ScaSystems.Marking.DAL.Models
{
    [Table(name: "DMMarkStates", Schema = "scansys")]
    public class DMMarkState
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<DMMark> DMMarks { get; set; }
    }
}
