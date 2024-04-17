using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PS27765_NguyenNgocChien_SD18317.Models
{
    public class CartDetails
    {
        [Required]
        public string CartId { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public int Quantity { get; set; } = 1;
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        [ForeignKey(nameof(CartId))]
        public Cart? Cart { get; set; }
    }
}
