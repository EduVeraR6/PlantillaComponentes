using PruebaProyecto.Models.AGE.AgeCamposGenerales;
using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminRoles.AgePerfilesTransacciones
{
    public class AgePerfilesTransaccioneSaveDAO : AgeCamposGeneralesDAO
    {
        [JsonProperty("id")]
        public AgePerfilesTransaccionesPKDAO Id { get; set; }

        [JsonProperty("ageTransaAgeAplicaCodigo")]
        public int AgeTransaAgeAplicaCodigo { get; set; }

        [JsonProperty("ageTransaCodigo")]
        public int AgeTransaCodigo { get; set; }



    }
}
