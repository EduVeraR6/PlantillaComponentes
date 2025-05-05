using PruebaProyecto.Models.AGE.AgeCamposGenerales;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeParametrosGenerales
{
    public class AgeParametrosGeneralesSaveDAO : AgeCamposGeneralesDAO
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; } = null!;
        public string TipoDatoParametro { get; set; } = null!;
    }
}
