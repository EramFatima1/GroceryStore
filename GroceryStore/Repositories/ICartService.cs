using GroceryStore.Models;
using GroceryStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.Repositories
{
    public interface ICartService
    {
        List<tblCartViewModel> GetAllItemsOfCart(Guid UserId);
        List<tblCartViewModel> AddToCart(int num, Guid id, Guid UserId, bool isPrime);
        bool SavePlaceOrder(Guid AddressId, double ShippingCharges, double Tax, double Subtotal, Guid UserId);
        tblCheckOutViewModel CheckOut(Guid UserId);
        List<tblCartViewModel> DeleteCart(Guid id, Guid UserId);
        List<tblCartViewModel> DeleteProductFromCart(Guid id, Guid UserId);
        List<tblCartViewModel> MiniCart(Guid UserId);
    }
}
