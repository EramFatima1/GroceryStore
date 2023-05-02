using GroceryStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.ViewModels
{
    public class tblProductsViewModel
    {
        public List<tblProducts> tblProducts { get; set; }
        public List<tblProducts> DiscountedProducts { get; set; }
    }
}
