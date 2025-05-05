using PruebaProyecto.Models.AGE.AgeCamposGenerales;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeLogErrores
{
    public class AgeLogErroresSaveDAO : AgeCamposGeneralesDAO
    {
        public AgeLogErroresPKDAO id { get; set; }
        public string?  mensaje { get; set; }
        public DateTime fecha { get; set; }
    }
}
