using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.Models
{
    public class tblUsers
    {
        [Key]
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        public string LastName { get; set; }
        public string Gender { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone must be of 10 digits")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        public string Password { get; set; }
        public string Role { get; set; }

    }
}
