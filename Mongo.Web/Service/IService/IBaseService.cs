using Mango.Web.Models;
using Mongo.Web.Models;

namespace Mongo.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDTO> SendAsync(RequestDTO requestDTO, bool withBearer = true);
    }
}
