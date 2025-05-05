using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaComponents.Components.Paginacion
{
    public partial class Paginacion : ComponentBase
    {
        [Parameter] public int IndicePagina { get; set; }
        [Parameter] public EventCallback<int> IndicePaginaChanged { get; set; }
        [Parameter] public int TamanoPagina { get; set; } = 5;
        [Parameter] public EventCallback<int> TamanoPaginaChanged { get; set; }
        [Parameter] public int TotalElementos { get; set; }
        [Parameter] public int PaginasAMostrar { get; set; } = 5;
        [Parameter] public bool EsMovil { get; set; } = false;
        [Parameter] public EventCallback<bool> OnAccionIniciada { get; set; }
        [Parameter] public EventCallback<bool> OnAccionFinalizada { get; set; }
        [Parameter] public bool EnProcesoAccion{ get; set; }

        private async Task IniciarAccion()
        {
            if (OnAccionIniciada.HasDelegate)
                await OnAccionIniciada.InvokeAsync(true);
        }

        private async Task FinalizarAccion()
        {
            if (OnAccionFinalizada.HasDelegate)
                await OnAccionFinalizada.InvokeAsync(true);
        }

        private async Task IrPrimeraPagina()
        {
            if (IndicePagina > 0)
            {
                await IniciarAccion();
                await CambiarIndicePagina(0);
                await FinalizarAccion();
            }
        }

        private async Task IrPaginaAnterior()
        {
            if (IndicePagina > 0)
            {
                await IniciarAccion();
                await CambiarIndicePagina(IndicePagina - 1);
                await FinalizarAccion();
            }
        }

        private async Task IrPaginaSiguiente()
        {
            int totalPaginas = GetTotalPages();
            if (IndicePagina < totalPaginas - 1)
            {
                await IniciarAccion();
                await CambiarIndicePagina(IndicePagina + 1);
                await FinalizarAccion();
            }
        }

        private async Task IrUltimaPagina()
        {
            int totalPaginas = GetTotalPages();
            if (IndicePagina < totalPaginas - 1)
            {
                await IniciarAccion();
                await CambiarIndicePagina(totalPaginas - 1);
                await FinalizarAccion();
            }
        }

        private async Task IrPagina(int pagina)
        {
            await IniciarAccion();
            await CambiarIndicePagina(pagina);
            await FinalizarAccion();
        }

        private async Task CambiarIndicePagina(int nuevaPagina)
        {
            if (IndicePagina != nuevaPagina)
            {
                IndicePagina = nuevaPagina;
                await IndicePaginaChanged.InvokeAsync(IndicePagina);
            }
        }

        private async Task CambiarTamanoPagina(ChangeEventArgs e)
        {
            await IniciarAccion();

            if (int.TryParse(e.Value?.ToString(), out int nuevoTamano))
            {
                TamanoPagina = nuevoTamano;
                await TamanoPaginaChanged.InvokeAsync(TamanoPagina);
                await CambiarIndicePagina(0);
            }

            await FinalizarAccion();
        }

        private List<int> ObtenerPaginasParaMostrar()
        {
            int totalPaginas = GetTotalPages();
            int maxPaginas = EsMovil ? 3 : PaginasAMostrar;
            int inicio = Math.Max(0, IndicePagina - maxPaginas / 2);
            int fin = Math.Min(totalPaginas - 1, inicio + maxPaginas - 1);

            if (fin - inicio < maxPaginas - 1 && inicio > 0)
            {
                inicio = Math.Max(0, fin - maxPaginas + 1);
            }

            return Enumerable.Range(inicio, fin - inicio + 1).ToList();
        }

        private int GetTotalPages()
        {
            return (int)Math.Ceiling((double)TotalElementos / TamanoPagina);
        }

        private int GetFirstItemIndex()
        {
            return TotalElementos == 0 ? 0 : IndicePagina * TamanoPagina + 1;
        }

        private int GetLastItemIndex()
        {
            return Math.Min((IndicePagina + 1) * TamanoPagina, TotalElementos);
        }



    }
}
