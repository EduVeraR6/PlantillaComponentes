using PruebaProyecto.Models.Age.AdminParametros.AgeFormasPagos;
using PruebaProyecto.Models.Age.AdminParametros.AgeFranquicias;
using PruebaProyecto.Models.Age.AdminParametros.AgeInstitucionesFinancieras;
using PruebaProyecto.Models.Age.AdminParametros.AgeLicenciatario;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeFormasPagoInstFinan
{
    public class AgeFormasPagosInstFinaDAO
    {
        public AgeFormasPagosInstFinaPKDAO Id { get; set; }

        public string Descripcion { get; set; } = null!;

        public int AgeForPaAgeLicencCodigo { get; set; }

        public int AgeForPaCodigo { get; set; }

        public int? AgeFranquCodigo { get; set; }

        public int? AgeInsFiAgeLicencCodigo { get; set; }

        public int? AgeInsFiCodigo { get; set; }

        public string Estado { get; set; } = null!;

        public AgeLicenciatarioDAO? Licenciatario { get; set; }

        public AgeFormasPagosDAO? FormasPagos { get; set; }

        public AgeFranquiciasDAO? Franquicia { get; set; }

        public AgeInstitucionesFinancieraDAO? InstitucionFinanciera { get; set; }
    }
}
