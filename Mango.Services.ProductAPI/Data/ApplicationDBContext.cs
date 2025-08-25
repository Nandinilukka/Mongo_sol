using Mango.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace Mango.services.ProductAPI.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //for the identity purpose

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Samosa",
                Price = 15,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://th.bing.com/th/id/R.9c8ad13951dfff89a5dddae6d98ed0d1?rik=vtjq95gG5RhBoQ&riu=http%3a%2f%2fgiglee.in%2fwp-content%2fuploads%2f2017%2f10%2fmaxresdefault-1.jpg&ehk=1EL9HjTLRdHppTI0ll%2fC%2blJ%2fX7sBCtT8iTGcIoemfPc%3d&risl=&pid=ImgRaw&r=0",
                CategoryName = "Appetizer"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Paneer Tikka",
                Price = 13.99,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://ruchirasoi.weebly.com/uploads/9/8/9/1/98916480/published/tandoori-paneer-tikka.jpg?1617990687",
                CategoryName = "Appetizer"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Sweet Pie",
                Price = 10.99,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://tse4.mm.bing.net/th/id/OIP.R8GUF4HkkYyf3WRJPpYfqgAAAA?rs=1&pid=ImgDetMain&o=7&rm=3",
                CategoryName = "Dessert"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                Name = "Pav Bhaji",
                Price = 15,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://tse3.mm.bing.net/th/id/OIP.luR7lSoY3H2tE49HVb0GdAHaGL?rs=1&pid=ImgDetMain&o=7&rm=3",
                CategoryName = "Entree"
            });


        }

    }
}

