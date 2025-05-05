using PruebaProyecto.Models.AGE.AgeCamposGenerales;

namespace PruebaProyecto.Models.Age.AdminParametros.AgePaises

{
    public class AgePaisesSaveDAO : AgeCamposGeneralesDAO
    {
        public int Codigo { get; set; }

        public string Descripcion { get; set; } = null!;

    }
}
