using Mango.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace Mango.services.ShoppingCartAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        { }

        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }

    }
}

