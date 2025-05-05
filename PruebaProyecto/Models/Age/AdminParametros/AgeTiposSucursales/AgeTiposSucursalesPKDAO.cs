using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeTiposSucursales
{
    public class AgeTiposSucursalesPKDAO
    {

        [JsonProperty("ageLicencCodigo")]
        public int AgeLicencCodigo { get; set; }

        [JsonProperty("codigo")]
        public int Codigo { get; set; }

    }
}
