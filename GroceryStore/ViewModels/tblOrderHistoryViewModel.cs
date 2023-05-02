using GroceryStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.ViewModels
{
    public class tblOrderHistoryViewModel
    {
        [Key]
        public Guid OrderId { get; set; }
        public string OrderStatus { get; set; }
        public int? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public double GrandTotal { get; set; }
        public tblUsers UserDetails { get; set; }
        public List<tblOrderViewModel> OrderHistory { get; set; }
    }

    public class tblOrderViewModel
    {
        public string ProductName { get; set; }
        public string ProductPicture { get; set; }
        public int NoOfProducts { get; set; }
     
        public double ProductPrice { get; set; }
    }
}
