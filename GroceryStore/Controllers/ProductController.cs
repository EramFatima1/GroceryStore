using GroceryStore.Models;
using GroceryStore.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult AddProduct(Guid id)
        {
            if (id != null && id != Guid.Empty)
            {
                tblProducts _tblProducts = null;
                try
                {
                    _tblProducts = _productService.GetProduct(id);
                }
                catch
                {
                    Notify("Something Wrong!", notificationType: NotificationType.error);
                }
                return View(_tblProducts);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddProduct(tblProducts tblProducts, Guid id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(tblProducts.ProductImage!=null)
                    {
                        if (Path.GetExtension(tblProducts.ProductImage.FileName) != ".png" || Path.GetExtension(tblProducts.ProductImage.FileName) != ".jpg" || Path.GetExtension(tblProducts.ProductImage.FileName) != ".jpeg" || Path.GetExtension(tblProducts.ProductImage.FileName) != ".gif")
                        {
                            Notify("Upload Image file only!", notificationType: NotificationType.error);
                            return View();
                        }
                    }
                    else
                    {
                        int flag = _productService.AddProduct(tblProducts, id);
                        if (flag == 1)
                            Notify("Congrats! Product updated");
                        else if (flag == 2)
                            Notify("Congrats! New Product added");
                        else if (flag == 0)
                            Notify("This product name already exists!", notificationType: NotificationType.warning);
                    }
                }
                catch (Exception e)
                {
                    Notify("Something Wrong!", notificationType: NotificationType.error);
                }
            }
            return RedirectToAction("ProductsList", "Product");
        }

        [HttpPost]
        public IActionResult DeleteProduct(Guid id)
        {
            try
            {
                _productService.DeleteProduct(id);
                Notify("Congrats! Product deleted");
            }
            catch
            {
                Notify("Something Wrong!", notificationType: NotificationType.error);
            }

            return RedirectToAction("ProductsList");
        }
        public IActionResult ProductsList()
        {
            List<tblProducts> model = null;
            try
            {
                model = _productService.ProductsList();
            }
            catch
            {
                Notify("Something Wrong!", notificationType: NotificationType.error);
            }
            return View(model);
        }
    }
}
