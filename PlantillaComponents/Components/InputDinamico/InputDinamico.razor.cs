using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using PlantillaComponents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaComponents.Components.InputDinamico
{
    public partial class InputDinamico<TItem> : ComponentBase
    {
        [Parameter] public TItem Item { get; set; }
        [Parameter] public string Accesor { get; set; }
        [Parameter] public string TipoInput { get; set; }
        [Parameter] public List<SelectOption> OpcionesSelect { get; set; }
        [Parameter] public string Min { get; set; }
        [Parameter] public string Max { get; set; }
        [Parameter] public string Step { get; set; }
        [Parameter] public bool TieneError { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public string MensajeError { get; set; }
        [Parameter] public string ValorInicial { get; set; }
        [Parameter] public EventCallback<(string Accesor, string Valor)> OnInputChange { get; set; }

        private bool _valorInicialRegistrado = false;

        private string _valorLocal;
        private string ValorActual => _valorLocal ?? ObtenerValorPropiedad(Item, Accesor)?.ToString();
        private string ValorFechaFormateado => FormatearFechaParaInput(ValorActual);

        protected override void OnInitialized()
        {
            _valorLocal = ObtenerValorPropiedad(Item, Accesor)?.ToString();
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !_valorInicialRegistrado && !string.IsNullOrWhiteSpace(ValorActual))
            {
                _valorInicialRegistrado = true;
                await OnInputChange.InvokeAsync((Accesor, ValorActual));
            }
        }

        private void ActualizarValorLocal(string valor)
        {
            _valorLocal = valor;
            OnInputChange.InvokeAsync((Accesor, valor));
        }

        private object ObtenerValorPropiedad(TItem item, string nombrePropiedad)
        {
            if (string.IsNullOrEmpty(nombrePropiedad))
                return null;

            // Manejar propiedades anidadas 
            var partes = nombrePropiedad.Split('.');
            object valor = item;

            foreach (var parte in partes)
            {
                if (valor == null)
                    return null;

                var propiedad = valor.GetType().GetProperty(parte);
                if (propiedad == null)
                    return null;

                valor = propiedad.GetValue(valor);
            }

            return valor;
        }

        private string FormatearFechaParaInput(string valorFecha)
        {
            if (string.IsNullOrEmpty(valorFecha))
                return string.Empty;

            if (DateTime.TryParse(valorFecha, out DateTime fecha))
            {
                return fecha.ToString("yyyy-MM-dd");
            }

            return valorFecha;
        }

    }
}
