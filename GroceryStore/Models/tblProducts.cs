using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.Models
{
    public class tblProducts
    {
        [Key]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Required")]
        public double ProductPrice { get; set; }
        public double? DiscountedPrice { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0, 50)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(200)]
        public string Description { get; set; }

        [Range(0, 50)]
        public int? Discount { get; set; }

        [NotMapped]
        public IFormFile? ProductImage { get; set; }

        public string? ProductPicture { get; set; }
        public bool ExtraDiscount { get; set; }
    }
}
