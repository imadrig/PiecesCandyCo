using Microsoft.EntityFrameworkCore;
using PiecesCandyCo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace PiecesCandyCo.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ApplicationUser>ApplicationUsers {get; set;}
        public DbSet<CustomerOrderDetail> CustomerOrderDetails { get; set; }
        public DbSet<CartOrderDetail> CartOrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 101, Name = "Chocolate", DisplayOrder = 1 },
                new Category { Id = 102, Name = "Hard Candy", DisplayOrder = 3 },
                new Category { Id = 103, Name = "Gummies", DisplayOrder = 2 },
                new Category { Id = 107, Name = "Special Edition", DisplayOrder = 4 }
                );

            modelBuilder.Entity<Product>().HasData(
                    new Product
                    {
                        Id = 301,
                        Name = "Champagne Gummy Bears (12 PK)",
                        Description = "Perfect for any occasion, these gummy bears come infused with flavors of champagne.",
                        Price = 10.95,
                        Price10 = 8.95,
                        CategoryId = 103,
                        ImageURL=""

                    },
                    new Product
                    {
                        Id = 302,
                        Name = "Rosé Gummy Bears (12 PK)",
                        Description = "Perfect for any occasion, these gummy bears come infused with flavors of rosé wine.",
                        Price = 10.95,
                        Price10 = 8.95,
                        CategoryId = 103,
                        ImageURL = ""
                    },
                    new Product
                    {
                        Id = 303,
                        Name = "Winter Wonderland White Chocolate Bar",
                        Description = "Perfect for any occasion, this chocolate bar is topped with peppermint and marshmallows.",
                        Price = 14.95,
                        Price10 = 12.95,
                        CategoryId = 101,
                        ImageURL = ""

                    },
                    new Product
                    {
                        Id = 304,
                        Name = "Cappuccino Chocolate Bar",
                        Description = "Perfect for any occasion, this chocolate bar is filled with liquid cappuccino.",
                        Price = 14.95,
                        Price10 = 12.95,
                        CategoryId = 101,
                        ImageURL = ""

                    },
                    new Product
                    {
                        Id = 305,
                        Name = "Whiskey Sour Hard Candies (12 PK)",
                        Description = "Perfect for any occasion, these candies come infused with flavors of whiskey.",
                        Price = 10.95,
                        Price10 = 8.95,
                        CategoryId = 102,
                        ImageURL = ""

                    },
                    new Product
                    {
                        Id = 306,
                        Name = "Pineapple Candy (12 PK)",
                        Description = "Perfect for any occasion, these candies come in pineapple flavor.",
                        Price = 10.95,
                        Price10 = 8.95,
                        CategoryId = 102,
                        ImageURL = ""

                    }
                ) ;
        }
    }
}
