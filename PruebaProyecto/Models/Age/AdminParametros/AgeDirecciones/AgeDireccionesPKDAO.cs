using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeDirecciones
{
    public class AgeDireccionesPKDAO
    {
        [JsonProperty("agePersonCodigo")]
        [JsonRequired]
        public long AgePersonCodigo { get; set; }

        [JsonProperty("agePersonAgeLicencCodigo")]
        [JsonRequired]
        public int AgePersonAgeLicencCodigo { get; set; }

        [JsonProperty("codigo")]
        [JsonRequired]
        public int Codigo { get; set; }
    }
}
