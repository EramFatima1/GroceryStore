using GroceryStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.ViewModels
{
    public class tblTrackOrderViewModel
    {
        public int? OrderNumber { get; set; }
        public string OrderStatus { get; set; } 
        public tblUsers UserDetails { get; set; }
    }
}
