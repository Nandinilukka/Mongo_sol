using Mango.Web.Models;

namespace Mongo.Web.Service.IService
{
    public interface IOrderService
    {
        Task <ResponseDTO?> CreateOrder(CartDTO cartDTO);
    }
}
