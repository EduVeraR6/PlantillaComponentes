using PruebaProyecto.Models.Age.AdminParametros.AgeTiposLocalidades;
using PruebaProyecto.Models.AGE.AgeCamposGenerales;
using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeTiposSucursales
{
    public class AgeTiposSucursalesSaveDAO: AgeCamposGeneralesDAO
    {

        [JsonProperty("id")]
        public AgeTiposSucursalesPKDAO Id { get; set; }


        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

    }
}
