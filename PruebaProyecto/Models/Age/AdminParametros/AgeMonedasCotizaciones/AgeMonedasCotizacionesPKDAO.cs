﻿using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeMonedasCotizaciones
{
    public class AgeMonedasCotizacionesPKDAO
    {
        [JsonProperty("ageLicencCodigo")]
        public int AgeLicencCodigo { get; set; }

        [JsonProperty("codigo")]
        public long Codigo { get; set; }
    }
}