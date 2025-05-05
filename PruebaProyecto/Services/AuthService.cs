using Blazored.Toast.Services;
using PruebaProyecto.Utils;
using System.Net.Http.Json;

namespace PruebaProyecto.Services
{
    public class AuthService : BaseHttpService
    {
        public AuthService(HttpClient client, IToastService toast) : base(client, toast) { }

        public async Task<string?> Login()
        {
            // el primer object es el dto que se envía a guardar, el segundo es la respuesta.
            // string 'kevin' simula un login para retornar token.
            return await PostAsync<string, string>(data: "kevin",
                                                    url: "login",
                                                    successMessage: "Login exitoso.");
        }
    }
}
