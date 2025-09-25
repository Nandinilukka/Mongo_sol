using Mango.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace Mango.services.EmailAPI.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        { }

        public DbSet<EmailLogger> EmailLoggers { get; set; }

    }
}

