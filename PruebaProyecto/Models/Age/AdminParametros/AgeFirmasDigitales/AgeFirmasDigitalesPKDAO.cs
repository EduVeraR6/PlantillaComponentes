using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AgeFirmasDigitales
{
    public class AgeFirmasDigitalesPKDAO
    {
        [JsonProperty("ageSucursAgeLicencCodigo")]
        public int AgeSucursAgeLicencCodigo { get; set; }

        [JsonProperty("ageSucursCodigo")]
        public int AgeSucursCodigo { get; set; }

        [JsonProperty("codigo")]
        public int Codigo { get; set; }
    }
}