using PruebaProyecto.Models.AGE.AgeCamposGenerales;
using Newtonsoft.Json;

namespace PruebaProyecto.Models.CLI.CliTiposCliente
{
    public class CliTiposClienteSaveDAO : AgeCamposGeneralesDAO
    {
        [JsonProperty("id")]
        public CliTiposClientePK Id { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; } = null!;
    }
}
