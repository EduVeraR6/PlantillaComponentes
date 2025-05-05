using PruebaProyecto.Models.Age.AdminParametros.AgeTransacciones;
using PruebaProyecto.Models.Age.AdminRoles.AgePerfiles;
using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminRoles.AgePerfilesTransacciones
{
    public class AgePerfilTransaccionDAO
    {
        [JsonProperty("agePerfiles")]
        public AgePerfilesDAO AgePerfiles { get; set; }

        [JsonProperty("ageTransaccionesList")]
        public List<AgeTransaccionesDAO> AgeTransaccionesList { get; set; }
    }
}
