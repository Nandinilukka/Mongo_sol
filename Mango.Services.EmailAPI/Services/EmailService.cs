using System.Text;
using Mango.services.EmailAPI.Data;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<ApplicationDBContext> _dboptions;

        public EmailService(DbContextOptions<ApplicationDBContext> dboptions)
        {
            this._dboptions = dboptions;
        }

        public async Task EmailCartAndLog(CartDTO cartDTO)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("<br> cart Email requested");
            message.Append("<br> cart Total" + cartDTO.CartHeader.CartTotal);
            message.Append("<br>");
            message.Append("<ul>");
            foreach(var item in cartDTO.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDTO.CartHeader.Email);
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
               EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.UtcNow,
                    Message = message
                };
                await using var db = new ApplicationDBContext(_dboptions);
                await db.EmailLoggers.AddAsync(emailLog);
                await db.SaveChangesAsync();
                return true;
    
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
