using PruebaProyecto.Models.Age.AdminParametros.AgeFormasPagos;
using PruebaProyecto.Models.Age.AdminParametros.AgeFranquicias;
using PruebaProyecto.Models.Age.AdminParametros.AgeInstitucionesFinancieras;
using PruebaProyecto.Models.Age.AdminParametros.AgeLicenciatario;
using PruebaProyecto.Models.AGE.AgeCamposGenerales;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeFormasPagoInstFinan
{
    public class AgeFormasPagosInstFinaSaveDAO : AgeCamposGeneralesDAO
    {
        public AgeFormasPagosInstFinaPKDAO Id { get; set; }

        public string Descripcion { get; set; } = null!;

        public int AgeForPaAgeLicencCodigo { get; set; }

        public int AgeForPaCodigo { get; set; }
        public int? AgeFranquCodigo { get; set; }

        public int? AgeInsFiAgeLicencCodigo { get; set; }

        public int? AgeInsFiCodigo { get; set; }
       

    }
}
