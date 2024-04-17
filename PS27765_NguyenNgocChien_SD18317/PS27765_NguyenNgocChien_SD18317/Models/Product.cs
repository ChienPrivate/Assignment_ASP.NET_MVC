using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PS27765_NguyenNgocChien_SD18317.Models
{
    public class Product
    {

        [Key]
        public string ProductId { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không thể trống")]
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Giá sản phẩm không thể trống")]
        public double Price { get; set; }
        [Required(ErrorMessage ="Số Lượng sản phẩm không thể rỗng")]
        public int ProductQuantity { get; set; }
        public string DescriptionText { get; set; }
        public string CategoryId { get; set; }
        [Range(1,50,ErrorMessage = "Giảm giả chỉ có thể từ 1 - 50%")]
        public double? Discount { get; set; }
        public string? Info { get; set;}
        [Required(ErrorMessage = "Không thể để trống ngày ra mắt")]
        public DateTime RealeaseDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedOn { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        [NotMapped]
        public int ShoppingQuantity { get; set; } = 1;

    }
}
