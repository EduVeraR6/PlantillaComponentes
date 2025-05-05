using PruebaProyecto.Models.Age.AdminParametros.AgeIdiomas;
using PruebaProyecto.Models.Age.AdminParametros.AgeMensaje;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeMensajesIdiomas
{
    public class AgeMensajesIdiomaDAO
    {
        public AgeMensajesIdiomaPKDAO Id { get; set; }

        public string DescripcionMsg { get; set; } = null!;

        public string SolucionMsg { get; set; }

        public string Estado { get; set; } = null!;

        public AgeIdiomasDAO Idiomas { get; set; }

        public AgeMensajeDAO Mensaje { get; set; }
    }
}
