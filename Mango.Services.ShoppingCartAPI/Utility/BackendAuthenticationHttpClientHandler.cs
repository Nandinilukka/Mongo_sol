using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace Mango.Services.ShoppingCartAPI.Utility
{
    public class BackendAuthenticationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor; //used to access the bearer token
        public BackendAuthenticationHttpClientHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Get the access token from the HttpContext
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            // Add the access token to the request headers
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Call the base class implementation to continue processing the request
            return await base.SendAsync(request, cancellationToken);
        }
    }
}





//DelegatingHandler // is used to add custom logic to the HTTP request pipeline, such as authentication or logging.
//they are kind of similar to .net core middleware
//but main diff is DelegatingHandler are nainly from client side
