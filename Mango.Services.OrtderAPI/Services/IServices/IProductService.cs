using Mango.Services.OrtderAPI.Models.DTO;

namespace Mango.Services.OrtderAPI.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
    }
}
