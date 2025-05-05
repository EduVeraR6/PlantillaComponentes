using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminRoles.AgeUsuarios
{
    public class AgeUsuariosLoginDAO
    {
        [JsonProperty("codigoExterno")]
        public string CodigoExterno { get; set; } = null!;

        [JsonProperty("clave")]
        public string Clave { get; set; } = null!;
    }
}
