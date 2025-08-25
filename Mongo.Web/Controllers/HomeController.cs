using System.Diagnostics;
using IdentityModel;
using Mango.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Mongo.Web.Models;
using Mongo.Web.Service;
using Mongo.Web.Service.IService;
using Newtonsoft.Json;

namespace Mongo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService ProductService;
        private readonly ICartService _cartService;
        public HomeController(IProductService ProductService , ICartService cartService)
        {
            this.ProductService = ProductService;
            _cartService = cartService;
        }

        public async  Task<IActionResult> Index()
        {
            List<ProductDTO>? list = new();
            ResponseDTO? response = await ProductService.GetAllProductAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDTO model = new();
            ResponseDTO? response = await ProductService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDTO productDTO)
        {
            CartDTO cartDTO = new CartDTO()
            {
                CartHeader = new CartHeaderDTO
                {
                    UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value,
                },
            };

            CartDetailsDTO cartDetails = new CartDetailsDTO()
            {
                Count = productDTO.Count,
                ProductId = productDTO.ProductId,
            };

            List<CartDetailsDTO> cartDetailsDTOs = new() { cartDetails };
            cartDTO.CartDetails = cartDetailsDTOs;

            ResponseDTO? response = await _cartService.UpsertCartAsync(cartDTO);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item has been added to shoppingcart successfully";
                return RedirectToAction(nameof(Index));
            
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDTO);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
