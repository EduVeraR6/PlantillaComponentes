﻿using PruebaProyecto.Models.AGE.AgeCamposGenerales;
using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeLicenciatarioAplicaciones
{
    public class AgeLicenciatariosAplicacionesSaveDAO : AgeCamposGeneralesDAO
    {
        [JsonProperty("id")]
        public AgeLicenciatariosAplicacionesPKDAO Id { get; set; }

        [JsonProperty("ageRutaAgeLicencCodigo")]
        public int? AgeRutaAgeLicencCodigo { get; set; }

        [JsonProperty("ageRutaCodigo")]
        public int? AgeRutaCodigo { get; set; }
    }
}
