using GroceryStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.ViewModels
{
    public class tblCheckOutViewModel
    {
        [Key]
        public Guid UserId { get; set; }       
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public double ShippingCharges { get; set; }
        public double Tax { get; set; }
        public double Subtotal { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public tblAddress tblAddress { get; set; }
        public List<tblCartViewModel> tblCartViewModel { get; set; }
    }
}
