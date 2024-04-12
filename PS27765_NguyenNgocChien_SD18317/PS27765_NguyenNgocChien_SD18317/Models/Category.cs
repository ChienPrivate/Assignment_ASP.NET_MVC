using System.ComponentModel.DataAnnotations;

namespace PS27765_NguyenNgocChien_SD18317.Models
{
    public class Category
    {
        [Key]
        public string CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public int DisplayOrder { get; set; }
        ICollection<Product>? Products { get; set; }
    }
}
