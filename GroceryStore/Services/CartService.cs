using GroceryStore.Models;
using GroceryStore.Repositories;
using GroceryStore.ViewModels;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using System.IO;
using Microsoft.Extensions.Options;
using MailKit.Security;
using System.Collections;
using Microsoft.Extensions.Configuration;

namespace GroceryStore.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MailSettings _mailSettings;
        private readonly IConfiguration _configuration;
        public CartService(ApplicationDbContext dbContext, IOptions<MailSettings> mailSettings, IConfiguration configuration)
        {
            this._dbContext = dbContext;
            _mailSettings = mailSettings.Value;
            this._configuration = configuration;
        }
        public List<tblCartViewModel> GetAllItemsOfCart(Guid UserId)
        {
            List<tblCartViewModel> model = new List<tblCartViewModel>();
            List<tblCart> _tblCart = _dbContext.tblCart.Where(c => c.UserId == UserId).ToList();
            if (_tblCart.Count > 0)
            {
                _tblCart = _dbContext.tblCart.Where(c => c.UserId == UserId).ToList<tblCart>();
                if (_tblCart.Count > 0)
                {
                    var consolidatedChildren =
                                           from c in _tblCart
                                           group c by new
                                           {
                                               c.ProductId
                                           } into gcs
                                           select new tblCart()
                                           {
                                               ProductId = gcs.Key.ProductId,
                                           };

                    foreach (var item in consolidatedChildren)
                    {
                        List<tblCart> _tblCart1 = _tblCart.Where(c => c.ProductId == item.ProductId).ToList<tblCart>();
                        int numberofProduct = 0;
                        Double totalPrice = 0;
                        foreach (tblCart item1 in _tblCart1)
                        {
                            numberofProduct = numberofProduct + item1.NoOfProducts;
                            totalPrice = Math.Round((totalPrice + item1.TotalPrice), 2);
                        }
                        var data = _tblCart1.Where(c => c.ProductId == item.ProductId).FirstOrDefault<tblCart>();
                        model.Add(new tblCartViewModel()
                        {
                            CartId = data.CartId,
                            UserId = data.UserId,
                            ProductId = data.ProductId,
                            NoOfProducts = numberofProduct,
                            TotalPrice = totalPrice,
                            productDetails = _dbContext.tblProducts.Where(c => c.ProductId == item.ProductId).FirstOrDefault(),
                        });

                    }
                }
            }
            return model;
        }
        public tblProducts GetProductOnProductId(Guid id)
        {
            return _dbContext.tblProducts.Where(c => c.ProductId == id).FirstOrDefault();
        }
        public List<tblCartViewModel> AddToCart(int num, Guid id, Guid UserId, bool isPrime)
        {
            tblCart model = new tblCart();
            model.CartId = Guid.NewGuid();
            model.UserId = UserId;
            model.ProductId = id;
            model.NoOfProducts = num;
            tblProducts _tblProducts = GetProductOnProductId(id);
            if (_tblProducts.Discount != 0)
                model.TotalPrice = (double)(_tblProducts.DiscountedPrice * num);
            else
                model.TotalPrice = Math.Round(_tblProducts.ProductPrice * num, 2);
            if (isPrime)
                model.TotalPrice = Math.Round(0.9 * model.TotalPrice, 2);
            _dbContext.tblCart.Add(model);
            _dbContext.SaveChanges();
            List<tblCartViewModel> model1 = GetAllItemsOfCart(UserId);
            return model1;
        }
        public bool SavePlaceOrder(Guid AddressId, double ShippingCharges, double Tax, double Subtotal, Guid UserId)
        {
            tblOrder _tblOrder = new tblOrder();
            _tblOrder.OrderId = Guid.NewGuid();
            _tblOrder.UserId = UserId;
            _tblOrder.AddressId = AddressId;
            _tblOrder.OrderStatus = 'O';
            _tblOrder.ShippingCharges = ShippingCharges;
            _tblOrder.Tax = Tax;
            _tblOrder.Subtotal = Subtotal;
            _tblOrder.OrderDate = DateTime.Now;
            if (_dbContext.tblOrder.ToList().Count > 0)
                _tblOrder.OrderNumber = Convert.ToInt32((from c in _dbContext.tblOrder
                                                         select c).Max(c => c.OrderNumber)) + 1;
            else
                _tblOrder.OrderNumber = 1000;
            _dbContext.tblOrder.Add(_tblOrder);
            _dbContext.SaveChanges();
            tblOrderDetails _tblOrderDetails = new tblOrderDetails();
            List<tblCart> _tblCart = _dbContext.tblCart.Where(c => c.UserId == UserId).ToList();
            IEnumerable<tblCart> model = null;
            if (_tblCart.Count > 0)
            {
                model =
                                       from c in _tblCart
                                       group c by new
                                       {
                                           c.ProductId
                                       } into gcs
                                       select new tblCart()
                                       {
                                           ProductId = gcs.Key.ProductId,
                                           NoOfProducts = gcs.Count()
                                       };

            }
            foreach (var item in model)
            {
                _tblOrderDetails.OrderId = _tblOrder.OrderId;
                _tblOrderDetails.UserId = UserId;
                _tblOrderDetails.ProductId = item.ProductId;
                _tblOrderDetails.NoOfproducts = item.NoOfProducts;
                _dbContext.tblOrderDetails.Add(_tblOrderDetails);
                _dbContext.SaveChanges();
            }

            EmptyCart(UserId);
            bool flag = SendEmail(UserId, _tblOrder.OrderId);
            return flag;
        }
        public void EmptyCart(Guid UserId)
        {
            List<tblCart> tblCarts = _dbContext.tblCart.Where(c => c.UserId == UserId).ToList();
            foreach (tblCart item in tblCarts)
            {
                _dbContext.tblCart.Remove(item);
            }
            _dbContext.SaveChanges();
        }
        public tblCheckOutViewModel CheckOut(Guid UserId)
        {
            tblCheckOutViewModel model = new tblCheckOutViewModel();
            tblUsers _tblUsers = _dbContext.tblUsers.Where(c => c.UserId == UserId).FirstOrDefault();
            List<tblCartViewModel> _tblCartViewModel = GetAllItemsOfCart(UserId);
            double ShippingCharges = 80;
            double Tax = 40;
            double SubTotal = 0;
            foreach (var item in _tblCartViewModel)
            {
                SubTotal = Math.Round(SubTotal + Convert.ToDouble(item.TotalPrice), 2);
            }
            tblAddress _tblAddress = _dbContext.tblAddress.Where(c => c.UserId == UserId && c.IsDefault).FirstOrDefault();
            if (_tblAddress != null)
            {
                tblCountry _tblCountry = _dbContext.tblCountry.Where(c => c.CountryId == _tblAddress.CountryId).FirstOrDefault();
                tblCity _tblCity = _dbContext.tblCity.Where(c => c.CityId == _tblAddress.CityId).FirstOrDefault();
                tblState _tblState = _dbContext.tblState.Where(c => c.StateId == _tblAddress.StateId).FirstOrDefault();
                model = new tblCheckOutViewModel()
                {
                    UserId = UserId,
                    UserName = _tblUsers.UserName,
                    FirstName = _tblUsers.FirstName,
                    LastName = _tblUsers.LastName,
                    Email = _tblUsers.Email,
                    Phone = _tblUsers.Phone,
                    tblAddress = _tblAddress,
                    Country = _tblCountry.CountryName,
                    State = _tblState.StateName,
                    City = _tblCity.CityName,
                    tblCartViewModel = _tblCartViewModel,
                    ShippingCharges = ShippingCharges,
                    Tax = Tax,
                    Subtotal = SubTotal
                };
            }
            else
            {
                model = new tblCheckOutViewModel()
                {
                    UserId = UserId,
                    UserName = _tblUsers.UserName,
                    FirstName = _tblUsers.FirstName,
                    LastName = _tblUsers.LastName,
                    Email = _tblUsers.Email,
                    Phone = _tblUsers.Phone,
                    tblCartViewModel = _tblCartViewModel,
                    ShippingCharges = ShippingCharges,
                    Tax = Tax,
                    Subtotal = SubTotal,
                };
            }
            return model;
        }
        public List<tblCartViewModel> DeleteCart(Guid id, Guid UserId)
        {
            List<tblCartViewModel> model1 = new List<tblCartViewModel>();
            var item = _dbContext.tblCart.Where(c => c.CartId == id).FirstOrDefault();
            if (item != null)
            {
                _dbContext.tblCart.Remove(item);
                _dbContext.SaveChanges();
                model1 = GetAllItemsOfCart(UserId);
            }
            return model1;

        }
        public List<tblCartViewModel> DeleteProductFromCart(Guid id, Guid UserId)
        {
            List<tblCart> _tblCart = _dbContext.tblCart.Where(c => c.ProductId == id && c.UserId == UserId).ToList();
            if (_tblCart.Count > 0)
            {
                foreach (tblCart item in _tblCart)
                {
                    _dbContext.tblCart.Remove(item);
                }
            }
            _dbContext.SaveChanges();
            List<tblCartViewModel> model1 = GetAllItemsOfCart(UserId);
            return model1;
        }
        public List<tblCartViewModel> MiniCart(Guid UserId)
        {
            List<tblCartViewModel> model = GetAllItemsOfCart(UserId);
            return model;
        }
        public bool SendEmail(Guid UserId, Guid OrderId)
        {
            MailRequest mailRequest = new MailRequest();
            tblUsers EmailModel = _dbContext.tblUsers.Where(a => a.UserId == UserId).FirstOrDefault();
            tblOrder tblOrder = _dbContext.tblOrder.Where(a => a.OrderId == OrderId).FirstOrDefault();
            mailRequest.Attachments = null;
            string emailBody = _configuration.GetValue<string>("Template:HtmlPart").
                Replace("xxxx", tblOrder.OrderNumber.ToString()).
                Replace("####", EmailModel.FirstName + " " + EmailModel.LastName);
            mailRequest.Body = emailBody;
            mailRequest.ToEmail = EmailModel.Email;
            mailRequest.Subject = _configuration.GetValue<string>("Template:SubjectPart");
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            //if (mailRequest.Attachments != null)
            //{
            //    byte[] fileBytes;
            //    foreach (var file in mailRequest.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                file.CopyTo(ms);
            //                fileBytes = ms.ToArray();
            //            }
            //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            //        }
            //    }
            //}
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
