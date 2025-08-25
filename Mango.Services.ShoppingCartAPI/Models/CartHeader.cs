using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        [Key]
        public int CartHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        [NotMapped] // This property will not be mapped to the database
        public double Discount { get; set; }
        [NotMapped]
        public double CartTotal { get; set; }

    }
}
