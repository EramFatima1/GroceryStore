using GroceryStore.Models;
using GroceryStore.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GroceryStore.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }
        public IActionResult Index()
        {
            List<tblProducts> model = new List<tblProducts>();
            try
            {
                model = _homeService.GetProductsList();
            }
            catch
            {
                Notify("Something Wrong!", notificationType: NotificationType.error);
            }
            return View(model);
        }
        public IActionResult Products()
        {
            List<tblProducts> model = new List<tblProducts>();
            try
            {
                model = _homeService.GetProductsList();
            }
            catch
            {
                Notify("Something Wrong!", notificationType: NotificationType.error);
            }
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AddToCart(int num, Guid id, bool isPrime)
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            if (id != null)
            {
                try
                {
                    _homeService.AddToCart(num, id, UserId, isPrime);
                }
                catch
                {
                    Notify("Something Wrong!", notificationType: NotificationType.error);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
