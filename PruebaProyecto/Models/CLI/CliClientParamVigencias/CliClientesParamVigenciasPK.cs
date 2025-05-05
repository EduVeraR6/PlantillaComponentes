using Newtonsoft.Json;

namespace PruebaProyecto.Models.CLI.CliClientParamVigencias
{
    public class CliClientesParamVigenciasPK
    {
        [JsonProperty("codigo")]
        public long Codigo { get; set; }

        [JsonProperty("cliClientAgeLicencCodigo")]
        public int CliClientAgeLicencCodigo { get; set; }

        [JsonProperty("cliClientCodigo")]
        public int CliClientCodigo { get; set; }

        [JsonProperty("cliParGeCodigo")]
        public int CLIParGeCodigo { get; set; }

    }
}
