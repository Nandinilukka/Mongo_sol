using AutoMapper;
using Mango.services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.services.ProductAPI.Controllers
{
    //[Route("api/[controller]")]

    [Route("api/Product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private ResponseDTO _response;
        private IMapper _mapper;

        public ProductController(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDTO();
            _mapper = mapper;
        }
        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Product> objList = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDTO>>(objList);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDTO Get(int id)
        {
            try
            {
                Product obj = _db.Products.First(u => u.ProductId == id);
                _response.Result = _mapper.Map<ProductDTO>(obj);

                // ProductDTO ProductDTO = new ProductDTO()
                // {
                //     ProductId =obj.ProductId,
                //     ProductCode = obj.ProductCode,
                //     DiscountAmount = obj.DiscountAmount,
                //     MinAmount = obj.MinAmount,
                // };
                //_response.Result = ProductDTO;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

       
        [HttpPost]
        [Authorize(Roles ="ADMIN")]
        public ResponseDTO Post([FromBody] ProductDTO ProductDto)
        {

            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid model state.";
                return _response;
            }

            try
            {
                Product obj = _mapper.Map<Product>(ProductDto);
                _db.Products.Add(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDTO>(obj);
            }
            //catch (Exception ex)
            //{
            //    _response.IsSuccess = false;
            //    _response.Message = ex.Message;
            //}
            //return _response;

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return _response;

        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Put([FromBody] ProductDTO ProductDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(ProductDto);
                _db.Products.Update(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDTO>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDTO Delete(int id)
        {
            try
            {
                Product obj = _db.Products.First(u => u.ProductId == id);
                _db.Products.Remove(obj);
                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
