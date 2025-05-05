using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeMensaje
{
    public class AgeMensajeDAO
    {
        [JsonProperty("estado")]
        public string Estado { get; set; } = null!;
        [JsonProperty("codigo")]
        public long Codigo { get; set; }
        [JsonProperty("accionMsg")]
        public string AccionMsg { get; set; } = null!;
    }
}
