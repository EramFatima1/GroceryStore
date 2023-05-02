using GroceryStore.Models;
using GroceryStore.Repositories;
using GroceryStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            this._cartService = cartService;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MyCart()
        {
            List<tblCartViewModel> model = new List<tblCartViewModel>();
            if (Request.Cookies["UserId"] != null)
            {
                Guid UserId = new Guid(Request.Cookies["UserId"]);
                model = _cartService.GetAllItemsOfCart(UserId);
            }
            return View(model);
        }
        public IActionResult AddToCart(int num, Guid id, bool isPrime)
        {
            if (id != null)
            {
                if (Request.Cookies["UserId"] != null)
                {
                    Guid UserId = new Guid(Request.Cookies["UserId"]);
                    List<tblCartViewModel> model1 = new List<tblCartViewModel>();
                    try
                    {
                        model1 = _cartService.AddToCart(num, id, UserId, isPrime);
                    }
                    catch
                    {
                        Notify("Something Wrong!", notificationType: NotificationType.error);
                    }
                    return RedirectToAction("MyCart", model1);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult CheckOut(Guid? AddressId, double? ShippingCharges, double? Tax, double? Subtotal)
        {
            if (AddressId != Guid.Empty)
            {
                Guid UserId = new Guid(Request.Cookies["UserId"]);
                try
                {
                    var flag = _cartService.SavePlaceOrder((Guid)AddressId, (double)ShippingCharges, (double)Tax, (double)Subtotal, UserId);
                    if (flag)
                        Notify("Congrats! Order placed. An E-mail has been sent.");
                    else
                        Notify("Order placed but E-mail is not sent!", notificationType: NotificationType.error);
                }
                catch
                {
                    Notify("Something Wrong!", notificationType: NotificationType.error);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("AddAddressBook", "Account");
            }
        }
        public IActionResult CheckOut()
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            tblCheckOutViewModel model = new tblCheckOutViewModel();
            try
            {
                model = _cartService.CheckOut(UserId);
            }
            catch
            {
                Notify("Something Wrong!", notificationType: NotificationType.error);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteCart(Guid id)
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            List<tblCartViewModel> model1 = new List<tblCartViewModel>();
            try
            {
                model1 = _cartService.DeleteCart(id, UserId);
            }
            catch
            {
                Notify("Something Wrong!", notificationType: NotificationType.error);
            }

            return RedirectToAction("MyCart", model1);
        }

        [HttpPost]
        public IActionResult DeleteProductFromCart(Guid id, string name)
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            List<tblCartViewModel> model1 = new List<tblCartViewModel>();
            try
            {
                model1 = _cartService.DeleteProductFromCart(id, UserId);
                Notify("Product deleted");
            }
            catch
            {
                Notify("Something Wrong!", notificationType: NotificationType.error);
            }
            if (name == "myCart")
            {
                return RedirectToAction("MyCart", model1);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult MiniCart()
        {
            List<tblCartViewModel> model = new List<tblCartViewModel>();
            if (Request.Cookies["UserId"] != null)
            {
                Guid UserId = new Guid(Request.Cookies["UserId"]);
                if (UserId != Guid.Empty)
                {
                    try
                    {
                        model = _cartService.MiniCart(UserId);
                    }
                    catch
                    {
                        Notify("Something Wrong!", notificationType: NotificationType.error);
                    }
                    return PartialView(model);
                }
            }
            return PartialView(model);
        }
    }
}
