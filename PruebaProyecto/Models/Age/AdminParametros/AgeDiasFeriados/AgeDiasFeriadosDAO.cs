﻿using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeDiasFeriados
{
    public class AgeDiasFeriadosDAO
    {
        [JsonProperty("codigo")]
        public int Codigo { get; set; }

        [JsonProperty("descripcion")]
        public string? Descripcion { get; set; }
        
        [JsonProperty("fechaDesde")]
        public DateTime? FechaDesde { get; set; }
        
        [JsonProperty("fechaHasta")]
        public DateTime? FechaHasta { get; set;}
        
        [JsonProperty("horaDesde")]
        public DateTime? HoraDesde { get; set;}
        
        [JsonProperty("horaHasta")]
        public DateTime? HoraHasta { get; set;}

        [JsonProperty("estado")]
        public string? Estado { get; set; }
    }
}
