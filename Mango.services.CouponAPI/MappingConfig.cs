using AutoMapper;
using Mango.services.CouponAPI.DTO;
using Mango.services.CouponAPI.Models;

namespace Mango.services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDTO, Coupon>();
                config.CreateMap<Coupon, CouponDTO>();
            });
            return mappingConfig;
        }
    }
}
