using System.Text.Json.Serialization;

namespace PruebaProyecto.Models.AGE.Authenticate
{
    public class UsuariosLoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
