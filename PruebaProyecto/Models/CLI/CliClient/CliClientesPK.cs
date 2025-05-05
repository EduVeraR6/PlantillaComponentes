using Newtonsoft.Json;

namespace PruebaProyecto.Models.CLI.CliClient
{
    public class CliClientesPK
    {
        [JsonProperty("codigo")]
        public long Codigo { get; set; }

        [JsonProperty("ageLicencCodigo")]
        public int AgeLicencCodigo { get; set; }
    }
}
