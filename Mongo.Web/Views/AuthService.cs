using Mango.Web.Models;
using Mango.Web.Models.DTO;
using Mongo.Web.Models;
using Mongo.Web.Service.IService;
using Mongo.Web.Utility;

namespace Mongo.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public  async Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDTO,
                Url = SD.AuthAPIBase + "/api/auth/AssignRole"
            });
        }

        public async Task<ResponseDTO?> LoginAsync(LoginRequestDTO loginRequest)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequest,
                Url = SD.AuthAPIBase + "/api/auth/login"
            },withBearer: false);
        }

        public async Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDTO,
                Url = SD.AuthAPIBase + "/api/auth/register"
            },withBearer: false);
        }
    }
}
