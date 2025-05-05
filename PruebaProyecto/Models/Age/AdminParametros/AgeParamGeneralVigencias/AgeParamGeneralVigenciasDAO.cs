﻿using PruebaProyecto.Models.Age.AdminParametros.AgeParametrosGenerales;
using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeParamGeneralVigencias
{
    public class AgeParamGeneralVigenciasDAO
    {

        [JsonProperty("id")]
        public AgeParamGeneralVigenciasPKDAO Id { get; set; }

        [JsonProperty("observacion")]
        public string? Observacion { get; set; }
        [JsonProperty("fechaDesde")]
        public DateTime FechaDesde { get; set; }
        
        [JsonProperty("fechaHasta")]
        public DateTime? FechaHasta { get; set; }
        
        [JsonProperty("valorParametro")]
        public string ValorParametro { get; set; } = null!;
        
        [JsonProperty("estado")]
        public string Estado { get; set; } = null!;

        [JsonProperty("parametrosGenerales")]
        public AgeParametrosGeneralesDAO ParametrosGenerales { get; set; }
    }
}
