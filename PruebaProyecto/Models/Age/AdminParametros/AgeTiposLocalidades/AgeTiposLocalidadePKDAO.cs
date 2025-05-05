

using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeTiposLocalidades
{
    public class AgeTiposLocalidadePKDAO
    {

        [JsonProperty("agePaisCodigo")]
        public int agePaisCodigo { get; set; }

        [JsonProperty("codigo")]
        public int codigo { get; set; }
    }
}
