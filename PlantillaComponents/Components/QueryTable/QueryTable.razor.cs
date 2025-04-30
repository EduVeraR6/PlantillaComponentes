using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using PlantillaComponents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PlantillaComponents.Components.QueryTable
{
    public partial class QueryTable<TItem> : ComponentBase
    {
        public class DefinicionColumna
        {
            public string Encabezado { get; set; }
            public string Accesor { get; set; }
            public string ClaseCss { get; set; }
            public bool? PermitirOrdenamiento { get; set; }
            public Func<TItem, object> Celda { get; set; }
        }

        [Parameter] public List<DefinicionColumna> Columnas { get; set; } = new();
        [Parameter] public List<TItem> Elementos { get; set; } = new();
        [Parameter] public string Titulo { get; set; }
        [Parameter] public Dictionary<string, object> ParametrosConsulta { get; set; } = new();
        [Parameter] public string LlaveFiltro { get; set; } = "filtro";
        [Parameter] public string LlavePagina { get; set; } = "pagina";
        [Parameter] public string LlaveTamano { get; set; } = "tamano";
        [Parameter] public string LlaveOrden { get; set; } = "orden";
        [Parameter] public string LlaveDatosRespuesta { get; set; } = "contenido";
        [Parameter] public string LlaveTotalRespuesta { get; set; } = "elementosTotales";
        [Parameter] public int RetardoBusqueda { get; set; } = 300;
        [Parameter] public bool MostrarOpciones { get; set; } = true;
        [Parameter] public bool BusquedaHabilitada { get; set; } = true;
        [Parameter] public EventCallback<TItem> AlSeleccionar { get; set; }
        [Parameter] public EventCallback<TItem> AlEliminar { get; set; }
        [Parameter] public string AccesorEstado { get; set; }
        [Parameter] public EventCallback<(TItem, string)> AlCambiarEstado { get; set; }
        [Parameter] public EventCallback AlNuevo { get; set; }
        [Parameter] public EventCallback<List<TItem>> AlEliminarMasivo { get; set; }
        [Parameter] public int PaginaPorDefecto { get; set; } = 0;
        [Parameter] public int TamanoPorDefecto { get; set; } = 5;
        [Parameter] public string OrdenPorDefecto { get; set; } = "";
        [Parameter] public bool Ordenable { get; set; } = true;
        [Parameter] public int PaginasAMostrar { get; set; } = 5;
        [Parameter] public string ClaseTabla { get; set; }
        [Parameter] public RenderFragment<TItem> FilaExpandida { get; set; }
        [Parameter] public Func<TItem, bool> DeshabilitarFilaExpandida { get; set; }
        [Parameter] public string EtiquetaNoEncontrado { get; set; } = "No se encontraron resultados";
        [Parameter] public string EventoRefrescar { get; set; }
        [Parameter] public string PlaceholderBusqueda { get; set; } = "Buscar...";

        [Parameter] public List<TItem> Datos { get; set; } = new();
        [Parameter] public bool Cargando { get; set; } = false;
        [Parameter] public int TamanoPagina { get; set; } = 5;

        private int TotalElementos { get; set; } = 0;
        private bool HayError { get; set; } = false;
        private string FiltroGlobal { get; set; } = "";
        private int IndicePagina { get; set; }

        private string OrdenConsulta { get; set; } = "";
        private Dictionary<string, bool> FilasExpandidas { get; set; } = new();
        private List<TItem> FilasSeleccionadas { get; set; } = new();
        private bool MostrarConfirmacionEliminarMasivo { get; set; } = false;
        private bool EsMovil { get; set; } = false;
        private TItem DatosSuperpuestos { get; set; }
        private Posicion PosicionSuperpuesta { get; set; }
        private System.Threading.Timer _temporizadorBusqueda;

        private List<TItem> ElementosFiltrados { get; set; } = new();
        private List<TItem> ElementosPaginados { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            IndicePagina = PaginaPorDefecto;
            TamanoPagina = TamanoPagina <= 0 ? TamanoPorDefecto : TamanoPagina;
            OrdenConsulta = OrdenPorDefecto;

            ElementosFiltrados = Elementos.ToList();
            TotalElementos = Elementos.Count;

            AplicarPaginado();
            await Task.CompletedTask;
        }

        protected override async Task OnParametersSetAsync()
        {
            ElementosFiltrados = Elementos.Where(e =>
                string.IsNullOrEmpty(FiltroGlobal) ||
                ContieneFiltro(e, FiltroGlobal)
            ).ToList();

            TotalElementos = ElementosFiltrados.Count;
            AplicarPaginado();

            await base.OnParametersSetAsync();
        }

        private bool ContieneFiltro(TItem elemento, string filtro)
        {
            if (elemento == null || string.IsNullOrEmpty(filtro))
                return true;

            foreach (var columna in Columnas)
            {
                var valor = ObtenerValorPropiedad(elemento, columna.Accesor)?.ToString();
                if (valor != null && valor.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private void CambiarFiltroGlobal(ChangeEventArgs e)
        {
            FiltroGlobal = e.Value?.ToString() ?? "";
            IndicePagina = 0;

            _temporizadorBusqueda?.Dispose();
            _temporizadorBusqueda = new System.Threading.Timer(async _ =>
            {
                await InvokeAsync(() =>
                {
                    FiltrarDatos();
                    StateHasChanged();
                });
            }, null, RetardoBusqueda, Timeout.Infinite);
        }

        private void LimpiarFiltroGlobal()
        {
            FiltroGlobal = "";
            IndicePagina = 0;
            FiltrarDatos();
        }

        private void FiltrarDatos()
        {
            ElementosFiltrados = Elementos.Where(e => string.IsNullOrEmpty(FiltroGlobal) || ContieneFiltro(e, FiltroGlobal)).ToList();
            TotalElementos = ElementosFiltrados.Count;
            AplicarPaginado();
        }

        private void AplicarPaginado()
        {
            int inicio = IndicePagina * TamanoPagina;
            int fin = Math.Min(inicio + TamanoPagina, ElementosFiltrados.Count);

            if (inicio >= ElementosFiltrados.Count && ElementosFiltrados.Count > 0)
            {
                IndicePagina = 0;
                inicio = 0;
                fin = Math.Min(TamanoPagina, ElementosFiltrados.Count);
            }

            ElementosPaginados = ElementosFiltrados.Skip(inicio).Take(TamanoPagina).ToList();
        }


        private void AlternarExpansionFila(string idFila, TItem item)
        {
            if (DeshabilitarFilaExpandida != null && DeshabilitarFilaExpandida(item)) return;

            if (FilasExpandidas.ContainsKey(idFila))
                FilasExpandidas[idFila] = !FilasExpandidas[idFila];
            else
                FilasExpandidas[idFila] = true;
        }

        private void SeleccionarFila(TItem item)
        {
            if (FilasSeleccionadas.Contains(item))
                FilasSeleccionadas.Remove(item);
            else
                FilasSeleccionadas.Add(item);
        }

        private async Task EliminarElemento(TItem item)
        {
            await InvokeAsync(StateHasChanged); // Asegura renderizado si es necesario
            PosicionSuperpuesta = await JS.InvokeAsync<Posicion>("obtenerPosicionElemento", "btnEliminarCliente");
            DatosSuperpuestos = item;
        }


        private async Task ConfirmarEliminar()
        {
            if (DatosSuperpuestos != null)
            {
                await AlEliminar.InvokeAsync(DatosSuperpuestos);
            }
            DatosSuperpuestos = default;
        }

        private void CancelarEliminar()
        {
            DatosSuperpuestos = default;
        }

        private async Task CambiarEstado(TItem item)
        {
            if (string.IsNullOrEmpty(AccesorEstado) || !AlCambiarEstado.HasDelegate)
                return;

            var estadoActual = ObtenerValorPropiedad(item, AccesorEstado)?.ToString();
            var nuevoEstado = estadoActual == "A" ? "I" : "A";

            await AlCambiarEstado.InvokeAsync((item, nuevoEstado));
        }


        private async Task ConfirmarEliminarMasivo()
        {
            if (AlEliminarMasivo.HasDelegate && FilasSeleccionadas.Count > 0)
            {
                await AlEliminarMasivo.InvokeAsync(FilasSeleccionadas);
            }
            FilasSeleccionadas.Clear();
            MostrarConfirmacionEliminarMasivo = false;
        }

        private void IrPrimeraPagina()
        {
            if (IndicePagina > 0)
            {
                IndicePagina = 0;
                AplicarPaginado();
            }
        }

        private void IrPaginaAnterior()
        {
            if (IndicePagina > 0)
            {
                IndicePagina--;
                AplicarPaginado();
            }
        }

        private void IrPaginaSiguiente()
        {
            int totalPaginas = (int)Math.Ceiling((double)TotalElementos / TamanoPagina);
            if (IndicePagina < totalPaginas - 1)
            {
                IndicePagina++;
                AplicarPaginado();
            }
        }

        private void IrUltimaPagina()
        {
            int totalPaginas = (int)Math.Ceiling((double)TotalElementos / TamanoPagina);
            if (IndicePagina < totalPaginas - 1)
            {
                IndicePagina = totalPaginas - 1;
                AplicarPaginado();
            }
        }

        private void IrPagina(int pagina)
        {
            IndicePagina = pagina;
            AplicarPaginado();
        }

        private List<int> ObtenerPaginasParaMostrar()
        {
            int totalPaginas = (int)Math.Ceiling((double)TotalElementos / TamanoPagina);
            int maxPaginas = EsMovil ? 3 : PaginasAMostrar;
            int inicio = Math.Max(0, IndicePagina - maxPaginas / 2);
            int fin = Math.Min(totalPaginas - 1, inicio + maxPaginas - 1);

            if (fin - inicio < maxPaginas - 1)
            {
                inicio = Math.Max(0, fin - maxPaginas + 1);
            }

            return Enumerable.Range(inicio, fin - inicio + 1).ToList();
        }

        private async Task CambiarTamanoPagina(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int nuevoTamano))
            {
                TamanoPagina = nuevoTamano;
                IndicePagina = 0;
                AplicarPaginado();
            }
        }

        private int ObtenerCantidadColumnas()
        {
            int cantidad = Columnas.Count;
            if (FilaExpandida != null) cantidad++;
            if (AlSeleccionar.HasDelegate || AlEliminar.HasDelegate) cantidad++;
            return cantidad;
        }

        private object ObtenerValorPropiedad(object objeto, string propiedad)
        {
            return objeto?.GetType().GetProperty(propiedad)?.GetValue(objeto);
        }


        private object ObtenerValorCelda(TItem item, DefinicionColumna column)
        {
            if (column.Celda != null)
            {
                return column.Celda(item);
            }

            return ObtenerValorPropiedad(item, column.Accesor);
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