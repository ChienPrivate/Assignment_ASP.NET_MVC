using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PS27765_NguyenNgocChien_SD18317.Models;

namespace PS27765_NguyenNgocChien_SD18317.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<EUser> EUser { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartDetails>()
            .HasKey(cd => new { cd.CartId, cd.ProductId });

            modelBuilder.Entity<CartDetails>()
                .HasOne(cd => cd.Cart)
                .WithMany()
                .HasForeignKey(cd => cd.CartId)
                .HasPrincipalKey(c => c.CartId);

            modelBuilder.Entity<CartDetails>()
                .HasOne(cd => cd.Product)
                .WithMany()
                .HasForeignKey(cd => cd.ProductId)
                .HasPrincipalKey(p => p.ProductId);
        }
    }
}
