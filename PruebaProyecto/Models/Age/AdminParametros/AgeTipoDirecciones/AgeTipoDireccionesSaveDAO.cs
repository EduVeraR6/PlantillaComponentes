using PruebaProyecto.Models.AGE.AgeCamposGenerales;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeTipoDirecciones
{
    public class AgeTipoDireccionesSaveDAO: AgeCamposGeneralesDAO
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; } = null!;

    }
}
