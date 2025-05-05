using Newtonsoft.Json;

namespace PruebaProyecto.Models.AGE.Authenticate
{
    public class UsuariosLogin
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

    }
}
