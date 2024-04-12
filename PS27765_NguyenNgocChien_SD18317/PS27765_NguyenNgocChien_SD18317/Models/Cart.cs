using System.ComponentModel.DataAnnotations;

namespace PS27765_NguyenNgocChien_SD18317.Models
{
    public class Cart
    {
        [Key]
        public string CartId {get; set; }
        public string EUserId { get; set; }
        CartDetails? CartDetails { get; set; }
    }
}
