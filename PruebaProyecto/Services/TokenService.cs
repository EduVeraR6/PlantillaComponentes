using Blazored.LocalStorage;

namespace PruebaProyecto.Services
{
    public class TokenService
    {
        private const string TokenKey = "authToken";
        private readonly ILocalStorageService _localStorage;

        public TokenService(ILocalStorageService storage)
        {
            _localStorage = storage;
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>(TokenKey);
        }

        public async Task SetTokenAsync(string token)
        {
            await _localStorage.SetItemAsync(TokenKey, token);
        }

        public async Task ClearTokenAsync()
        {
            await _localStorage.RemoveItemAsync(TokenKey);
        }
    }
}
