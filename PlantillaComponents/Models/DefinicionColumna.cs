using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaComponents.Models
{
    public class DefinicionColumna<TItem>
    {
        public string Encabezado { get; set; } = "";

        public string? Accesor { get; set; }

        public Func<TItem, object>? Plantilla { get; set; }

        public string? ClaseCss { get; set; }

        public bool? PermitirOrdenamiento { get; set; }

        public bool? Registrar {  get; set; }

        public bool Editable { get; set; } = true;

        public bool Requerido { get; set; } = false;

        public string? PatronRegex { get; set; }

        public string? MensajeError { get; set; }

        public string? TipoInput { get; set; } = "text";

        public List<SelectOption>? OpcionesSelect { get; set; }

        public string? Min { get; set; }

        public string? Max { get; set; }

        public string? Step { get; set; }

        public string? FormatoFecha { get; set; } = "dd/MM/yyyy";

        public string? FormatoNumero { get; set; }

        public DefinicionColumna() { }

        public DefinicionColumna(string encabezado, string? accesor, string? claseCss = null)
        {
            Encabezado = encabezado;
            Accesor = accesor;
            ClaseCss = claseCss;
        }
    }

}
