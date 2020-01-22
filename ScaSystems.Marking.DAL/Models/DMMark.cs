using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ScaSystems.Marking.DAL.Models
{
    [Table(name: "DMMarks", Schema = "scansys")]
    public class DMMark
    {
        public Guid Id { get; set; }
        public string DataMatrix { get; set; }
        public int DMMarkStateId { get; set; }

        [ForeignKey(nameof(DMMarkStateId))]
        public DMMarkState DMMarkState { get; set; }
    }
}
