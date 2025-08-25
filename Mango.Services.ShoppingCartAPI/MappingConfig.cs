using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTO;

namespace Mango.services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeaderDTO, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailsDTO, CartDetails>().ReverseMap();

            });
            return mappingConfig;
        }
    }
}
