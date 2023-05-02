using GroceryStore.Models;
using GroceryStore.Repositories;
using GroceryStore.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace GroceryStore.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly ApplicationDbContext _dbContext;
        public AccountController(IAccountService accountService, ApplicationDbContext dbContext)
        {
            this._accountService = accountService;
            this._dbContext = dbContext;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(tblUsers tblUsers)
        {
            if (!string.IsNullOrEmpty(tblUsers.UserName) && string.IsNullOrEmpty(tblUsers.Password))
            {
                return RedirectToAction("Login");
            }
            //Check the user name and password
            else
            {
                var userList = _accountService.GetUsersListOnUserName(tblUsers);

                if (userList.Count > 0)
                {
                    //Create the identity for the user  
                    var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, tblUsers.UserName),
                    new Claim(ClaimTypes.Role, userList[0].Role)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    Response.Cookies.Append("ROLE", userList[0].Role);
                    Response.Cookies.Append("UserId", userList[0].UserId.ToString());
                    Response.Cookies.Append("FirstName", userList[0].FirstName.ToString());
                    Response.Cookies.Append("LastName", userList[0].LastName.ToString());
                    bool IsPrimeMember = _accountService.IsPrimeMember(new Guid(userList[0].UserId.ToString()));
                    Response.Cookies.Append("IsPrimeMember", IsPrimeMember.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(tblUsers tblUsers)
        {
            ModelState.Remove("UserName");
            if (ModelState.IsValid)
            {
                try
                {
                    var model = _accountService.SignUp(tblUsers);
                    if (model.Role == null)
                        Notify("This Email or Phone already exists! Try again", notificationType: NotificationType.error);
                    else
                        Notify("Congrats! SignUp Successful.." + model.UserName + " is your UserName");
                }
                catch
                {
                    Notify("Something Wrong!", notificationType: NotificationType.error);
                }
            }
            return RedirectToAction("Login", "Account");
        }
        public IActionResult Logout()
        {
            var signOut = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult MyAccount()
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            var model = _accountService.GetMyAccountInfo(UserId);
            return View(model);
        }
        public IActionResult MyProfile()
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            var model = _accountService.GetMyAccountInfo(UserId);
            return View(model);
        }
        public IActionResult EditMyProfile()
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            tblUsers _tblUsers = _accountService.GetUserInfoOnUserId(UserId);
            return View(_tblUsers);
        }
        [HttpPost]
        public IActionResult EditMyProfile(tblUsers users)
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            tblUsers _tblUsers = new tblUsers();
            try
            {
                _tblUsers = _accountService.SaveProfileDetails(UserId, users);
                Notify("Congrats! Profile changed");
            }
            catch
            {
                Notify("Something Wrong!", notificationType: NotificationType.error);
            }
            return View(_tblUsers);

        }
        public IActionResult AddressBook()
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            var model = _accountService.GetListAddressBook(UserId);
            return View(model);

        }
        public IActionResult AddAddressBook(Guid id)
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            var model = _accountService.AddAddressBook(UserId, id);
            return View(model);
        }
        [HttpPost]
        public IActionResult DeleteAddress(Guid id)
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            try
            {
               var model1 = _accountService.DeleteAddress(id, UserId);
                Notify("Address deleted");
            }
            catch
            {
                Notify("Something Wrong!", notificationType: NotificationType.error);
            }

            return RedirectToAction("AddressBook", "Account");
        }
        [HttpPost]
        public IActionResult AddAddressBook(tblAddress model, Guid id)
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            tblAddressViewModel model1 = new tblAddressViewModel();
            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.SaveAddressBook(UserId, id, model);
                    if (id != Guid.Empty)
                        Notify("Congrats! Address changed");
                    else
                        Notify("Congrats! Address Saved");
                }
                catch
                {
                    Notify("Something Wrong!", notificationType: NotificationType.error);
                }
            }

            return RedirectToAction("AddressBook", "Account");
        }
        public IActionResult TrackOrder()
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            var model = _accountService.GetTrackOrder(UserId);
            return View(model);
        }
        [HttpPost]
        public IActionResult TrackOrder(tblTrackOrderViewModel model)
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            var model1 = _accountService.PostTrackOrder(UserId, model);
            return View(model1);
        }
        public IActionResult MyOrder()
        {
            Guid UserId = new Guid(Request.Cookies["UserId"]);
            var model = _accountService.GetMyOrder(UserId);
            return View(model);
        }
        public JsonResult GetCitiesName()
        {
            var model = Json(_accountService.GetCitiesName());
            return model;
        }
    }
}

