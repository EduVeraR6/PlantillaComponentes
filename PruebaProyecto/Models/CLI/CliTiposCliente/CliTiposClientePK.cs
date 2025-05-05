using Newtonsoft.Json;

namespace PruebaProyecto.Models.CLI.CliTiposCliente
{
    public class CliTiposClientePK
    {

        [JsonProperty("ageLicencCodigo")]
        public int ageLicencCodigo { get; set; }

        [JsonProperty("codigo")]
        public long codigo { get; set; }

    }
}
