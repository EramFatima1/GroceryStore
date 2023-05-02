using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.Models
{
    public class tblAddress
    {
        [Key]
        public Guid AddressId { get; set; }
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(90)]
        public string Address { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int StateId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string PinCode { get; set; }
        public bool IsDefault { get; set; }
    }
}
