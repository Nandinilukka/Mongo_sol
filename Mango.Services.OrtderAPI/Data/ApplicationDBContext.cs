using Mango.Services.OrderAPI.Models;
using Mango.Services.OrtderAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace Mango.services.OrtderAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        { }

        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

    }
}

