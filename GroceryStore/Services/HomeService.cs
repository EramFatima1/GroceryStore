using GroceryStore.Models;
using GroceryStore.Repositories;
using GroceryStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GroceryStore.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext _dbContext;
        public HomeService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public List<tblProducts> GetProductsList()
        {
            return _dbContext.tblProducts.ToList();
        }
        public void AddToCart(int num, Guid id, Guid UserId,bool isPrime)
        {
            tblCart model = new tblCart();
            model.CartId = Guid.NewGuid();
            model.UserId = UserId;
            model.ProductId = id;
            model.NoOfProducts = num;
            tblProducts _tblProducts = _dbContext.tblProducts.Where(c => c.ProductId == id).FirstOrDefault();
            if (_tblProducts.Discount != 0)
                model.TotalPrice = (double)(_tblProducts.DiscountedPrice * num);
            else
                model.TotalPrice = _tblProducts.ProductPrice * num;
            if (isPrime)
                model.TotalPrice = 0.9 * model.TotalPrice;
            _dbContext.tblCart.Add(model);
            _dbContext.SaveChanges();
        }
    }
}
