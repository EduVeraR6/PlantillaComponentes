﻿using Newtonsoft.Json;

namespace PruebaProyecto.Models.CLI.CliContacto
{
    public class CliContactosPK
    {
        [JsonProperty("codigo")]
        public long Codigo { get; set; }

        [JsonProperty("cliClientAgeLicencCodigo")]
        public int CliClientAgeLicencCodigo { get; set; }

        [JsonProperty("cliClientCodigo")]
        public int CliClientCodigo { get; set; }

    }
}
