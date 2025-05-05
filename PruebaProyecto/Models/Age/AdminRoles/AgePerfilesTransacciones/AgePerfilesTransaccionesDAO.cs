﻿using PruebaProyecto.Models.Age.AdminParametros.AgeTransacciones;
using PruebaProyecto.Models.Age.AdminRoles.AgePerfiles;
using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminRoles.AgePerfilesTransacciones
{
    public class AgePerfilesTransaccionesDAO
    {
        [JsonProperty("id")]
        public AgePerfilesTransaccionesPKDAO Id { get; set; }

        [JsonProperty("ageTransaAgeAplicaCodigo")]
        public int AgeTransaAgeAplicaCodigo { get; set; }

        [JsonProperty("ageTransaCodigo")]
        public int AgeTransaCodigo { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("agePerfiles")]
        public AgePerfilesDAO AgePerfiles { get; set; } = null!;

        [JsonProperty("ageTransacciones")]
        public AgeTransaccionesDAO AgeTransacciones { get; set; } = null!;


    }
}
