using PruebaProyecto.Models.AGE.AgeCamposGenerales;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeFormasPagos
{
    public class AgeFormasPagosSaveDAO : AgeCamposGeneralesDAO
    {
        public AgeFormasPagosPKDAO Id { get; set; }


        public int CodigoInstitucionControl { get; set; }


        public string Descripcion { get; set; } = null!;


        public string Retiene { get; set; } = null!;


        public string? PresentaCajaBancos { get; set; }
    }
}
