using GroceryStore.Models;
using GroceryStore.Repositories;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GroceryStore.Services
{
    public class ProductService : IProductService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _dbContext;
        public ProductService(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this._dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public tblProducts GetProduct(Guid id)
        {
            return _dbContext.tblProducts.Find(id);
        }
        public List<tblProducts> ProductsList()
        {
            return _dbContext.tblProducts.OrderBy(c => c.ProductName).ToList();
        }
        public int AddProduct(tblProducts product, Guid id)
        {
            if (id != null && id != Guid.Empty) //Existing 
            {
                product.ProductId = id;
                tblProducts _tblProducts = _dbContext.tblProducts.Find(id);
                if (product.ProductImage != null)
                {
                    string uniqueFileName = saveImage(_tblProducts);
                    _tblProducts.ProductPicture = uniqueFileName.ToUpper();
                }
                else
                    product.ProductPicture = "";

                _tblProducts.ProductName = product.ProductName;
                _tblProducts.ProductPrice = product.ProductPrice;
                _tblProducts.Quantity = product.Quantity;
                if (product.Discount == null)
                {
                    _tblProducts.Discount = 0;
                    _tblProducts.DiscountedPrice = 0;
                }
                else
                {
                    _tblProducts.Discount = product.Discount;
                    _tblProducts.DiscountedPrice = Math.Round((double)(product.ProductPrice - (product.ProductPrice * product.Discount) / 100), 2);
                }
                _tblProducts.Description = product.Description;
                _tblProducts.ExtraDiscount = product.ExtraDiscount;
                _dbContext.SaveChanges();
                return 1;
            }
            else   //New
            {
                tblProducts tbl = _dbContext.tblProducts.Where(c => c.ProductName == product.ProductName).FirstOrDefault();
                if (tbl != null)    //if exists
                {
                    return 0;
                }
                else
                {
                    product.ProductId = Guid.NewGuid();
                    if (product.Discount == null)
                        product.Discount = 0;
                    product.DiscountedPrice = Math.Round((double)(product.ProductPrice - (product.ProductPrice * product.Discount) / 100), 2);
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "product");
                    if (product.ProductImage != null)
                    {
                        string uniqueFileName = saveImage(product);
                        product.ProductPicture = uniqueFileName.ToUpper();
                    }
                    else
                        product.ProductPicture = "";

                    _dbContext.tblProducts.Add(product);
                    _dbContext.SaveChanges();
                    return 2;
                }
            }
        }
        string saveImage(tblProducts product)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "product");
            string uniqueFileName = product.ProductId.ToString() + "_" + product.ProductImage.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                product.ProductImage.CopyTo(fileStream);
                fileStream.Flush();
            }
            return uniqueFileName;
        }
        public void DeleteProduct(Guid id)
        {
            var item = _dbContext.tblProducts.Find(id);
            if (item != null)
            {
                _dbContext.tblProducts.Remove(item);
                _dbContext.SaveChanges();
            }

        }
    }
}
