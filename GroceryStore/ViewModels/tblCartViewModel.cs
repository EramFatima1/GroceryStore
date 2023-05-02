using GroceryStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.ViewModels
{
    public class tblCartViewModel
    {
        [Key]
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int NoOfProducts { get; set; }
        public double TotalPrice { get; set; }
        public tblProducts productDetails { get; set; }
    }
}
