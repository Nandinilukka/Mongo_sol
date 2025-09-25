using Mango.ServiceBus;
using Mango.services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Mango.Services.AuthAPI.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDTO _response;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        public AuthAPIController(IAuthService authService, IConfiguration configuration,IMessageBus messageBus)
        {
            _authService = authService;
            _response = new();
            _configuration = configuration;
            _messageBus = messageBus;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            var errormessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errormessage))
            {
                _response.IsSuccess = false;
                _response.Message = errormessage;
                return BadRequest(_response);
            }
            //await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));
            //return Ok(_response);

            try
            {
                //await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue"));

                await _messageBus.PublishMessage(
                    model.Email,
                    "RegisterUser", // connection key
                    _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue") // topic/queue name
                );

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"Error publishing message: {ex.Message}";
                return StatusCode(500, _response); // Return Internal Server Error with details
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encouncountered";
                return BadRequest(_response);
            }
         
            return Ok(_response);
        }
    }
}
