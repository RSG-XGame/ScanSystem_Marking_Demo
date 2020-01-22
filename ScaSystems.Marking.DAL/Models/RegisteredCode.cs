using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ScaSystems.Marking.DAL.Models
{
    [Table(name: "RegisteredCodes", Schema = "scansys")]
    public class RegisteredCode
    {
        public Guid Id { get; set; }
        public Guid ChildrenId { get; set; }
        public Guid ParentId { get; set; }
        public int DMMarkTypeId { get; set; }

        [ForeignKey(nameof(DMMarkTypeId))]
        public DMMarkType DMMarkType { get; set; }
    }
}
