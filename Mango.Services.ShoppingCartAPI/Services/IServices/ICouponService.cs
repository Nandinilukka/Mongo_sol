using Mango.services.ShoppingCartAPI.DTO;

namespace Mango.Services.ShoppingCartAPI.Services.IServices
{
    public interface ICouponService
    {
        Task<CouponDTO> GetCoupon(string couponCode);
    }
}
