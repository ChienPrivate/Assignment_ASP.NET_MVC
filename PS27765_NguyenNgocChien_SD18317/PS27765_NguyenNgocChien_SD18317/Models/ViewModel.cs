namespace PS27765_NguyenNgocChien_SD18317.Models
{
    public class ViewModel
    {
        public IQueryable<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public List<EUser> Users { get; set; }
        public List<Cart> Carts { get; set; }
        public Product Product { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        
    }
}
