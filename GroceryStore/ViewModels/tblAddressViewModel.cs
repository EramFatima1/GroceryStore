using GroceryStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GroceryStore.ViewModels
{
    public class tblAddressViewModel
    {
        [Key]
        public Guid AddressId { get; set; }
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(90)]
        public string Address { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int StateId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Required")]
        public string State { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Required")]
        public string PinCode { get; set; }
        public bool IsDefault { get; set; }
        public tblUsers UserDetails { get; set; }
        public SelectList CountryList { get; set; }
        public SelectList StateList {get;set;}
        public SelectList CityList { get; set; }
       
    }
}
