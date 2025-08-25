using Mango.services.ShoppingCartAPI.DTO;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Services.IServices;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class ProductService : IProductService
    {
        private IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/product");
            var apiCount = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDTO>(apiCount);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(Convert.ToString(resp.Result));
            }
            return new List<ProductDTO>();
        }
    }
}
