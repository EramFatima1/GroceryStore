using GroceryStore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GroceryStore.Repositories
{
    public interface IProductService
    {
        tblProducts GetProduct(Guid id);
        List<tblProducts> ProductsList();
        int AddProduct(tblProducts product, Guid id);
        void DeleteProduct(Guid id);
    }
}
