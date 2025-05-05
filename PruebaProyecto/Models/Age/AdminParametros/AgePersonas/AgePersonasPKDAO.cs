using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminParametros.AgePersonas
{
    public class AgePersonasPKDAO
    {
        [JsonProperty("ageLicencCodigo")]
        public int ageLicencCodigo { get; set; }

        [JsonProperty("codigo")]
        public int codigo { get; set; }
    }
}
