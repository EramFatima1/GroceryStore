using GroceryStore.Models;
using System;
using System.Collections.Generic;

namespace GroceryStore.Repositories
{
    public interface IHomeService
    {
        List<tblProducts> GetProductsList();
        void AddToCart(int num, Guid id, Guid UserId, bool isPrime);
    }
}
