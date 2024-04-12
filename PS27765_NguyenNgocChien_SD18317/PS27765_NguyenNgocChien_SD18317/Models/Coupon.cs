using System.ComponentModel.DataAnnotations;

namespace PS27765_NguyenNgocChien_SD18317.Models
{
    public class Coupon
    {
        [Key]
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        ICollection<Cart>? Carts { get; set; }

    }
}
