using System.Reflection.PortableExecutable;
using AutoMapper;
using Mango.services.ShoppingCartAPI.Data;
using Mango.services.ShoppingCartAPI.DTO;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    //[Route("api/[controller]")]

    [Route("api/Cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private ResponseDTO _response;
        private ApplicationDBContext _dbContext;
        private IProductService _productService;
        private ICouponService _couponService;
        private IMapper _mapper;
        public CartAPIController(ApplicationDBContext dbContext, IMapper mapper, IProductService productService, ICouponService couponService)
        {
            _dbContext = dbContext;
            this._response = new ResponseDTO();
            _productService = productService;
            _mapper = mapper;
            _couponService = couponService;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDTO> GetCart(string userId)
        {
            try
            {
                CartDTO cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDTO>(
                        await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId))
                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDTO>>(_dbContext.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));

                IEnumerable<ProductDTO> productDTos = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDTos.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += item.Count * item.Product.Price;
                }

                //apply coupon if any
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDTO coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if (coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }
                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDTO> ApplyCoupon([FromBody] CartDTO cartdto)
        {
            try
            {
                var cartFromDb = await _dbContext.CartHeaders.FirstAsync(u => u.UserId == cartdto.CartHeader.UserId);
                cartFromDb.CouponCode = cartdto.CartHeader.CouponCode;
                _dbContext.CartHeaders.Update(cartFromDb);
                await _dbContext.SaveChangesAsync();
                _response.Result = true;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }
            return _response;
        }

        //[HttpPost("RemoveCoupon")]
        //public async Task<ResponseDTO> RemoveCoupon([FromBody] CartDTO cartdto)
        //{
        //    try
        //    {
        //        var cartFromDb = await _dbContext.CartHeaders.FirstAsync(u => u.UserId == cartdto.CartHeader.UserId);
        //        cartFromDb.CouponCode = "";
        //        _dbContext.CartHeaders.Update(cartFromDb);
        //        await _dbContext.SaveChangesAsync();
        //        _response.Result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = ex.ToString();
        //    }
        //    return _response;
        //}




        [HttpPost("CartUpsert")]
        public async Task<ResponseDTO> CartUpsert(CartDTO cartDTO)
        {
            try
            {
                var cartHeaderFromDb = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDTO.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDTO.CartHeader);
                    _dbContext.CartHeaders.Add(cartHeader);
                    await _dbContext.SaveChangesAsync();

                    cartDTO.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _dbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                    await _dbContext.SaveChangesAsync();


                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await _dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDTO.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if(cartDetailsFromDb == null)
                    {
                        //create cart details
                        cartDTO.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _dbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                        await _dbContext.SaveChangesAsync();

                    }
                    else
                    {
                        //update count in coartdetails
                        cartDTO.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDTO.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDTO.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _dbContext.CartDetails.Update(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                        await _dbContext.SaveChangesAsync();
                    }
                }
               _response.Result = cartDTO;
              
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDTO> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartdetails = _dbContext.CartDetails.First(u => u.CartDetailsId == cartDetailsId);
                int TotalCountofCartItem = _dbContext.CartDetails.Where(u => u.CartHeaderId == cartdetails.CartHeaderId).Count();
                _dbContext.CartDetails.Remove(cartdetails);
                if (TotalCountofCartItem == 1)
                {
                    var cartHeaderToRemove = await _dbContext.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartdetails.CartHeaderId);
                    _dbContext.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _dbContext.SaveChangesAsync();
                _response.Result = true;
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





















































//user adds first item to the cart
//--create cart header
//--create cart details

//user adds a new item in shopping cart(user has already ew items in the cart)
//--find cart header
//--add cart details under same cart headre ID

//user updates quatity of an existing item in cart
//--find details
//--update count in cat details
