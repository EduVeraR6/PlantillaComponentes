using Microsoft.AspNetCore.Components;
using PruebaProyecto.Services;
using System.Net;

namespace PruebaProyecto.Handlers
{
    public class AuthRedirectHandler : DelegatingHandler
    {
        private readonly NavigationManager _navigation;
        private readonly TokenService _tokenService;

        public AuthRedirectHandler(NavigationManager navigation, TokenService tokenService)
        {
            _navigation = navigation;
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                await _tokenService.ClearTokenAsync();

                _navigation.NavigateTo("/login", forceLoad: true);
            }

            return response;
        }
    }
}
