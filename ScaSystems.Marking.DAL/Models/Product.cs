using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ScaSystems.Marking.DAL.Models
{
    [Table(name: "Products", Schema = "scansys")]
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
    }
}
