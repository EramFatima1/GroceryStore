using GroceryStore.Models;
using GroceryStore.ViewModels;
using System;
using System.Collections.Generic;

namespace GroceryStore.Repositories
{
    public interface IAccountService
    {
        tblUsers GetUserInfoOnUserId(Guid UserId);
        List<tblUsers> GetUsersListOnUserName(tblUsers tblUsers);
        tblAddress GetAddressOnUserIdIsDefault(Guid UserId);
        tblUsers SignUp(tblUsers tblUsers);
        tblAddressViewModel GetMyAccountInfo(Guid UserId);
        tblUsers SaveProfileDetails(Guid UserId, tblUsers users);
        tblAddressViewModel AddAddressBook(Guid UserId, Guid id);
        List<tblAddressViewModel> GetListAddressBook(Guid UserId);
        void SaveAddressBook(Guid UserId, Guid id, tblAddress model);
        List<tblAddress> DeleteAddress(Guid id, Guid UserId);
        tblTrackOrderViewModel GetTrackOrder(Guid UserId);
        tblTrackOrderViewModel PostTrackOrder(Guid UserId, tblTrackOrderViewModel model);
        List<tblOrderHistoryViewModel> GetMyOrder(Guid UserId);
        List<tblCity> GetCitiesName();
        bool IsPrimeMember(Guid UserId);
    }
}
