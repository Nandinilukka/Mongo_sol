using Mango.Web.Models;
using Mongo.Web.Models;
using Mongo.Web.Service.IService;
using Mongo.Web.Utility;

namespace Mongo.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> CreateCouponsAsync(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = couponDTO,
                Url = SD.CouponAPIBase + "/api/Coupon/" 
            });
        }

        public async Task<ResponseDTO?> DeleteCouponsAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponAPIBase + "/api/Coupon/" + id
            });
        }

        public async Task<ResponseDTO?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/Coupon",
            });
        }

        public async Task<ResponseDTO?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/Coupon/GetByCode" + couponCode
            });
        }

        public async Task<ResponseDTO?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/Coupon/" + id
            });
        }

        public async Task<ResponseDTO?> UpdateCouponsAsync(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = couponDTO,
                Url = SD.CouponAPIBase + "/api/Coupon/"
            });
        }
    }
}
