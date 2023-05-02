using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.Models
{
    public class tblOrder
    {
        [Key]
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public Guid AddressId { get; set; }
        [NotMapped]
        public char OrderStatus { get; set; }
        public double Tax { get; set; }
        public double Subtotal { get; set; }
        public double ShippingCharges { get; set; }
        public int? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
