using System.Collections.Generic;
using Mango.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Mongo.Web.Service.IService;
using Newtonsoft.Json;

namespace Mongo.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService ProductService;
        public ProductController(IProductService ProductService)
        {
            this.ProductService = ProductService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO>? list = new();
            ResponseDTO? response = await ProductService.GetAllProductAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"]=response?.Message;
            }
            return View(list);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? response = await ProductService.CreateProductAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product Created Successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }
        public async Task<IActionResult> ProductDelete(int ProductId)
        {
            ResponseDTO? response = await ProductService.GetProductByIdAsync(ProductId);

            if (response != null && response.IsSuccess)
            {
                ProductDTO? model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDTO ProductDTO)
        {
            ResponseDTO? response = await ProductService.DeleteProductAsync(ProductDTO.ProductId) ;

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product Deleted Successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(ProductDTO);
        }


        public async Task<IActionResult> ProductEdit(int ProductId)
        {
            ResponseDTO? response = await ProductService.GetProductByIdAsync(ProductId);

            if (response != null && response.IsSuccess)
            {
                ProductDTO? model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDTO ProductDTO)
        {
            ResponseDTO? response = await ProductService.UpdateProductAsync(ProductDTO);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product Updated Successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(ProductDTO);
        }

    }
}
