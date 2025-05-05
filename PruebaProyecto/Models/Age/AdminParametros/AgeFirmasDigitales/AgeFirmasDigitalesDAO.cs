﻿

using PruebaProyecto.Models.Age.AdminParametros.AgeFirmasDigitales;
using PruebaProyecto.Models.Age.AdminParametros.AgeSucursales;
using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AgeFirmasDigitales
{
    public class AgeFirmasDigitalesDAO
    {

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("id")]
        public AgeFirmasDigitalesPKDAO Id { get; set; }

        [JsonProperty("nombres")]
        public string Nombres { get; set; } = null!;

        [JsonProperty("apellidos")]
        public string Apellidos { get; set; } = null!;

        [JsonProperty("fechaDesde")]
        public DateTime FechaDesde { get; set; }

        [JsonProperty("fechaHasta")]
        public DateTime? FechaHasta { get; set; }

        [JsonProperty("tipoFirmaDigital")]
        public string TipoFirmaDigital { get; set; } = null!;

        [JsonProperty("clave")]
        public string Clave { get; set; } = null!;

        [JsonProperty("nombreArchivo")]
        public string? NombreArchivo { get; set; }

        [JsonProperty("url")]
        public string? Url { get; set; }

        [JsonProperty("fechaUltimaNotificacion")]
        public DateTime? FechaUltimaNotificacion { get; set; }

        [JsonProperty("sucursales")]
        public AgeSucursalesDAO Sucursales { get; set; }

    }
}