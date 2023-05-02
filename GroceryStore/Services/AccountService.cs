using GroceryStore.Models;
using GroceryStore.Repositories;
using GroceryStore.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GroceryStore.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public List<tblUsers> GetUsersListOnUserName(tblUsers tblUsers)
        {
            return _dbContext.tblUsers.Where(x => x.UserName == tblUsers.UserName).ToList<tblUsers>();
        }
        public tblUsers GetUserInfoOnUserId(Guid UserId)
        {
            return _dbContext.tblUsers.Find(UserId);
        }
        public tblAddress GetAddressOnUserIdIsDefault(Guid UserId)
        {
            return _dbContext.tblAddress.Where(c => c.UserId == UserId && c.IsDefault).FirstOrDefault();
        }
        public tblUsers SignUp(tblUsers tblUsers)
        {
            tblUsers isEmail = _dbContext.tblUsers.Where(a => a.Email == tblUsers.Email).FirstOrDefault();
            tblUsers isPhone = _dbContext.tblUsers.Where(a => a.Phone == tblUsers.Phone).FirstOrDefault();
            if (isEmail == null) //If Email do not exists
            {
                if (isPhone == null) //If Phone do not exists
                {
                    tblUsers.UserName = tblUsers.FirstName + tblUsers.LastName.Substring(0, 1);
                    tblUsers model1 = _dbContext.tblUsers.Where(a => a.UserName == tblUsers.UserName).FirstOrDefault();
                    if (model1 != null) //If UserName already exists
                    {
                        int suffix = 1;
                        string lastIndex = model1.UserName.Substring(model1.UserName.Length - 1, 1);
                        bool isNumber = int.TryParse(lastIndex, out int numericValue);
                        for (int i = 1; i < 1000; i++)
                        {
                            tblUsers model2 = _dbContext.tblUsers.Where(a => a.UserName == tblUsers.FirstName + tblUsers.LastName.Substring(0, 1) + i.ToString()).FirstOrDefault();
                            if (model2 == null)
                            {
                                suffix = i;
                                break;
                            }
                        }
                        tblUsers.UserName = tblUsers.FirstName + tblUsers.LastName.Substring(0, 1) + suffix.ToString();
                    }
                    else
                    {
                        tblUsers.UserName = tblUsers.FirstName + tblUsers.LastName.Substring(0, 1);
                    }
                    tblUsers.UserId = new Guid();
                    tblUsers.Role = "USER";
                    _dbContext.tblUsers.Add(tblUsers);
                    _dbContext.SaveChanges();
                    return tblUsers;
                }
                else
                {
                    return tblUsers;
                }
            }
            else
            {
                return tblUsers;
            }
        }
        public tblUsers SaveProfileDetails(Guid UserId, tblUsers users)
        {
            tblUsers _tblUsers = GetUserInfoOnUserId(UserId);
            _tblUsers.FirstName = users.FirstName;
            _tblUsers.LastName = users.LastName;
            _tblUsers.Gender = users.Gender;
            _tblUsers.Phone = users.Phone;
            _dbContext.SaveChanges();
            return _tblUsers;
        }
        public tblAddressViewModel GetMyAccountInfo(Guid UserId)
        {
            tblAddress _tblAddress = GetAddressOnUserIdIsDefault(UserId);
            return GetAddressViewModel(_tblAddress, UserId);
        }
        private tblAddressViewModel GetAddressViewModel(tblAddress _tblAddress, Guid UserId)
        {
            tblUsers _tblUsers = GetUserInfoOnUserId(UserId);
            if (_tblAddress != null)
            {
                tblAddressViewModel model = new tblAddressViewModel();
                tblCountry _tblCountry = _dbContext.tblCountry.Where(c => c.CountryId == _tblAddress.CountryId).FirstOrDefault();
                tblCity _tblCity = _dbContext.tblCity.Where(c => c.CityId == _tblAddress.CityId).FirstOrDefault();
                tblState _tblState = _dbContext.tblState.Where(c => c.StateId == _tblAddress.StateId).FirstOrDefault();

                return model = new tblAddressViewModel()
                {
                    UserDetails = _tblUsers,
                    AddressId = _tblAddress.AddressId,
                    UserId = _tblAddress.UserId,
                    Address = _tblAddress.Address,
                    City = _tblCity.CityName,
                    State = _tblState.StateName,
                    Country = _tblCountry.CountryName,
                    CityId = _tblCity.CityId,
                    StateId = _tblState.StateId,
                    CountryId = _tblCountry.CountryId,
                    PinCode = _tblAddress.PinCode,
                    IsDefault = _tblAddress.IsDefault,
                    CountryList = new SelectList(_dbContext.tblCountry, "CountryId", "CountryName"),
                    StateList = new SelectList(_dbContext.tblState, "StateId", "StateName"),
                    CityList = new SelectList(_dbContext.tblCity, "CityId", "CityName")
                };
            }
            else
            {
                return AddressBookDefault(_tblUsers);
            }
        }
        public tblAddressViewModel AddAddressBook(Guid UserId, Guid id)
        {
            tblUsers _tblUsers = GetUserInfoOnUserId(UserId);
            if (id != Guid.Empty)
            {
                tblAddress _tblAddress = _dbContext.tblAddress.Where(c => c.AddressId == id).FirstOrDefault();
                return GetAddressViewModel(_tblAddress, UserId);
            }
            else    //new form
            {
                return AddressBookDefault(_tblUsers);
            }
        }
        public List<tblAddressViewModel> GetListAddressBook(Guid UserId)
        {
            tblUsers _tblUsers = GetUserInfoOnUserId(UserId);
            List<tblAddressViewModel> model = new List<tblAddressViewModel>();

            IEnumerable<tblAddress> _tblAddress = _dbContext.tblAddress.Where(c => c.UserId == UserId).ToList<tblAddress>();
            if (_tblAddress.Count() != 0)
            {
                foreach (tblAddress item in _tblAddress)
                {
                    tblCity City = _dbContext.tblCity.Where(c => c.CityId == item.CityId).FirstOrDefault();
                    tblState State = _dbContext.tblState.Where(c => c.StateId == item.StateId).FirstOrDefault();
                    tblCountry Country = _dbContext.tblCountry.Where(c => c.CountryId == item.CountryId).FirstOrDefault();

                    model.Add(new tblAddressViewModel()
                    {
                        UserDetails = _tblUsers,
                        AddressId = item.AddressId,
                        UserId = item.UserId,
                        Address = item.Address,
                        CityId = item.CityId,
                        StateId = item.StateId,
                        CountryId = item.CountryId,
                        City = City.CityName,
                        State = State.StateName,
                        Country = Country.CountryName,
                        PinCode = item.PinCode,
                        IsDefault = item.IsDefault
                    });
                }
            }
            else
            {
                model.Add(AddressBookDefault(_tblUsers));
            }
            return model;
        }
        private tblAddressViewModel AddressBookDefault(tblUsers _tblUsers)
        {
            tblAddressViewModel model = new tblAddressViewModel()
            {
                UserDetails = _tblUsers,
                CountryList = new SelectList(_dbContext.tblCountry, "CountryId", "CountryName"),
                StateList = new SelectList(_dbContext.tblState, "StateId", "StateName"),
                CityList = new SelectList(_dbContext.tblCity.Where(a => a.StateId == 1), "CityId", "CityName")
            };
            return model;
        }
        public void SaveAddressBook(Guid UserId, Guid id, tblAddress model)
        {
            tblAddress _tblAddress = new tblAddress();
            if (id != Guid.Empty) //Edit
            {
                //  model.AddressId = id;
                _tblAddress = _dbContext.tblAddress.Find(id);
                if (model.IsDefault)
                {
                    List<tblAddress> tbl = _dbContext.tblAddress.Where(c => c.UserId == UserId).ToList<tblAddress>();
                    foreach (tblAddress item in tbl)
                    {
                        item.IsDefault = false;
                    }
                }
                _tblAddress.IsDefault = model.IsDefault;
            }
            else //New
            {
                _tblAddress.AddressId = Guid.NewGuid();
                _tblAddress.UserId = UserId;
                List<tblAddress> tbl = _dbContext.tblAddress.Where(c => c.UserId == UserId).ToList<tblAddress>();
                foreach (tblAddress item in tbl)
                {
                    item.IsDefault = false;
                }
                _tblAddress.IsDefault = true;
            }
            _tblAddress.Address = model.Address;
            _tblAddress.CountryId = model.CountryId;
            _tblAddress.StateId = model.StateId;
            _tblAddress.CityId = model.CityId;
            _tblAddress.PinCode = model.PinCode;
            if (id == Guid.Empty)
                _dbContext.tblAddress.Add(_tblAddress);
            _dbContext.SaveChanges();
        }

        public List<tblAddress> DeleteAddress(Guid id, Guid UserId)
        {
            List<tblAddress> model1 = new List<tblAddress>();
            var item = _dbContext.tblAddress.Where(c => c.AddressId == id && c.UserId == UserId).FirstOrDefault();
            if (item != null)
            {
                _dbContext.tblAddress.Remove(item);
                _dbContext.SaveChanges();
            }
            return model1;
        }
        public tblTrackOrderViewModel GetTrackOrder(Guid UserId)
        {
            tblUsers userDetails = GetUserInfoOnUserId(UserId);
            tblTrackOrderViewModel model = new tblTrackOrderViewModel()
            {
                UserDetails = userDetails
            };
            return model;
        }
        public tblTrackOrderViewModel PostTrackOrder(Guid UserId, tblTrackOrderViewModel model)
        {
            tblUsers userDetails = GetUserInfoOnUserId(UserId);
            List<tblOrder> tblOrder = _dbContext.tblOrder.Where(c => c.UserId == UserId && c.OrderNumber == model.OrderNumber).ToList();
            tblTrackOrderViewModel model1 = null;
            if (tblOrder.Count > 0)
            {
                var orderIdList = (from c in tblOrder
                                   select new
                                   {
                                       c.OrderNumber,
                                       OrderStatus = (DateTime.Now - c.OrderDate).TotalDays > 2 ? "Delivered" : (DateTime.Now - c.OrderDate).TotalDays > 1 ? "Shipped" : (DateTime.Now - c.OrderDate).TotalHours > 6 ? "Processing" : (DateTime.Now - c.OrderDate).TotalSeconds > 30 ? "Order Confirmed" : "Order Confirmed"
                                   }).ToList();

                model1 = new tblTrackOrderViewModel()
                {
                    UserDetails = model.UserDetails,
                    OrderStatus = orderIdList[0].OrderStatus,
                    OrderNumber = model.OrderNumber
                };
            }
            else
            {
                model1 = new tblTrackOrderViewModel()
                {
                    UserDetails = model.UserDetails,
                    OrderStatus = "Not Found! Please enter correct Order Number",
                    OrderNumber = model.OrderNumber
                };
            }
            return model1;
        }
        public List<tblOrderHistoryViewModel> GetMyOrder(Guid UserId)
        {
            tblUsers userDetails = _dbContext.tblUsers.Find(UserId);
            List<tblOrderHistoryViewModel> model = new List<tblOrderHistoryViewModel>();

            List<tblOrder> _tblOrder = _dbContext.tblOrder.Where(c => c.UserId == UserId).ToList();
            if (_tblOrder.Count > 0)
            {
                var orderIdList = (from c in _tblOrder
                                   select new
                                   {
                                       c.OrderId,
                                       c.OrderNumber,
                                       c.ShippingCharges,
                                       c.Subtotal,
                                       c.Tax,
                                       c.UserId,
                                       c.AddressId,
                                       c.OrderDate,
                                       OrderStatus = (DateTime.Now - c.OrderDate).TotalDays > 2 ? "Delivered" : (DateTime.Now - c.OrderDate).TotalDays > 1 ? "Shipped" : (DateTime.Now - c.OrderDate).TotalHours > 6 ? "Processing" : (DateTime.Now - c.OrderDate).TotalSeconds > 30 ? "Order Confirmed" : "Order Confirmed"
                                   }).ToList();

                foreach (var item in orderIdList)
                {
                    double GrandTotal = Math.Round(item.ShippingCharges + item.Tax + item.Subtotal, 2);
                    var x = _dbContext.tblOrderDetails.Where(c => c.OrderId == item.OrderId).ToList();
                    List<tblOrderViewModel> OrderHistory = new List<tblOrderViewModel>();
                    if (x.Count > 0)
                    {
                        foreach (var item1 in x)
                        {
                            var data = _dbContext.tblProducts.Where(c => c.ProductId == item1.ProductId).FirstOrDefault();
                            OrderHistory.Add(new tblOrderViewModel()
                            {
                                NoOfProducts = item1.NoOfproducts,
                                ProductName = data.ProductName,
                                ProductPicture = data.ProductPicture,
                                ProductPrice = data.ProductPrice,
                            });
                        }
                    }

                    model.Add(new tblOrderHistoryViewModel()
                    {
                        UserDetails = userDetails,
                        OrderId = item.OrderId,
                        OrderNumber = item.OrderNumber,
                        OrderStatus = item.OrderStatus,
                        OrderDate = item.OrderDate,
                        GrandTotal = GrandTotal,
                        OrderHistory = OrderHistory
                    });
                }
            }
            else
            {
                model.Add(new tblOrderHistoryViewModel()
                {
                    UserDetails = userDetails,
                });
            }
            return model;
        }
        public List<tblCity> GetCitiesName()
        {
            return _dbContext.tblCity.ToList();
        }
        public bool IsPrimeMember(Guid UserId)
        {
            List<tblOrder> tblOrders = _dbContext.tblOrder.Where(a => a.UserId == UserId).ToList();
            if (tblOrders.Count > 5)
                return true;
            else
                return false;
        }
    }
}
