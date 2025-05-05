using PruebaProyecto.Models.Age.AdminParametros.AgeParametrosGenerales;
using PruebaProyecto.Models.Age.AdminRoles.AgeUsuarios;
using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeUsuarioParamVigencias
{
    public class AgeUsuarioParamVigenciaDAO
    {

        [JsonProperty("id")]
        public AgeUsuarioParamVigenciaPKDAO id { get; set; }

        [JsonProperty("observacion")]
        public string? observacion { get; set; }

        [JsonProperty("fechaDesde")]
        public DateTime fechaDesde { get; set; }

        [JsonProperty("fechaHasta")]
        public DateTime? fechaHasta { get; set; }

        [JsonProperty("valorParametro")]
        public string valorParametro { get; set; } = null!;

        [JsonProperty("estado")]
        public string estado { get; set; } = null!;


        [JsonProperty("usuario")]
        public AgeUsuariosDAO Usuario { get; set; }

        [JsonProperty("parametrosGenerales")]
        public AgeParametrosGeneralesDAO ParametrosGenerales { get; set; }

    }
}
