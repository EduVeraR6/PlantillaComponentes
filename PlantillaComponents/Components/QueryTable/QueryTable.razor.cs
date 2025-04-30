using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using PlantillaComponents.Models;
using PlantillaComponents.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlantillaComponents.Components.QueryTable
{
    public partial class QueryTable<TItem> : ComponentBase where TItem : class
    {
      


        public class Posicion
        {
            public int Top { get; set; }
            public int Left { get; set; }
        }


        [Parameter] public List<DefinicionColumna<TItem>> Columnas { get; set; } = new();
        [Parameter] public List<TItem> Elementos { get; set; } = new();
        [Parameter] public string Titulo { get; set; }
        [Parameter] public Dictionary<string, object> ParametrosConsulta { get; set; } = new();
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

        // Propiedades del componente
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
        private Dictionary<string, bool> FilaEnEdicion { get; set; } = new();
        private string columnaOrdenada;
        private bool ordenAscendente = true;

        private Dictionary<string, Dictionary<string, string>> ErroresValidacion = new Dictionary<string, Dictionary<string, string>>();

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
            ElementosFiltrados = Elementos.Where(e =>
                string.IsNullOrEmpty(FiltroGlobal) || ContieneFiltro(e, FiltroGlobal)
            ).ToList();

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

            ElementosPaginados = ElementosFiltrados
                .Skip(inicio)
                .Take(TamanoPagina)
                .ToList();
        }

        private void AlternarExpansionFila(string idFila, TItem item)
        {
            if (DeshabilitarFilaExpandida != null && DeshabilitarFilaExpandida(item))
                return;

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
            await InvokeAsync(StateHasChanged);
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
            int totalPaginas = GetTotalPages();
            if (IndicePagina < totalPaginas - 1)
            {
                IndicePagina++;
                AplicarPaginado();
            }
        }

        private void IrUltimaPagina()
        {
            int totalPaginas = GetTotalPages();
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
            int totalPaginas = GetTotalPages();
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
            if (AlEliminarMasivo.HasDelegate) cantidad++;
            if (AlSeleccionar.HasDelegate || AlEliminar.HasDelegate) cantidad++;
            return cantidad;
        }

        private object ObtenerValorPropiedad(object objeto, string propiedad)
        {
            return objeto?.GetType().GetProperty(propiedad)?.GetValue(objeto);
        }

        private object ObtenerValorCelda(TItem item, DefinicionColumna<TItem> column)
        {
            if (column.Plantilla != null)
            {
                return column.Plantilla(item);
            }

            return ObtenerValorPropiedad(item, column.Accesor);
        }

        private string ObtenerValorFechaFormateado(TItem item, string propiedad)
        {
            var valor = ObtenerValorPropiedad(item, propiedad);
            if (valor is DateTime fecha)
            {
                return fecha.ToString("yyyy-MM-dd");
            }
            return "";
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

        // Lógica de edición en fila de la tabla
        private void AlternarEdicionFila(string idFila, TItem item)
        {
            if (FilaEnEdicion.ContainsKey(idFila))
            {
                FilaEnEdicion[idFila] = !FilaEnEdicion[idFila];
            }
            else
            {
                FilaEnEdicion[idFila] = true;
            }

            if (FilaEnEdicion[idFila])
            {
                AlSeleccionar.InvokeAsync(item);
            }
        }

        private void AsignarValorPropiedad(TItem item, string? propertyName, string? value)
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            var property = item.GetType().GetProperty(propertyName);
            if (property == null)
                return;

            try
            {
                var rowId = item.GetHashCode().ToString();
                var column = Columnas.FirstOrDefault(c => c.Accesor == propertyName);

                // Limpiar errores previos para esta propiedad
                if (ErroresValidacion.ContainsKey(rowId) && ErroresValidacion[rowId].ContainsKey(propertyName))
                {
                    ErroresValidacion[rowId].Remove(propertyName);
                }

                // Validar que no esté vacío si es requerido
                if (column?.Requerido == true && string.IsNullOrWhiteSpace(value))
                {
                    if (!ErroresValidacion.ContainsKey(rowId))
                        ErroresValidacion[rowId] = new Dictionary<string, string>();

                    ErroresValidacion[rowId][propertyName] = "Este campo es requerido";
                    return;
                }

                // Validar con Regex si está configurado
                if (!string.IsNullOrEmpty(column?.PatronRegex) && !string.IsNullOrEmpty(value))
                {
                    var regex = new Regex(column.PatronRegex);
                    if (!regex.IsMatch(value))
                    {
                        if (!ErroresValidacion.ContainsKey(rowId))
                            ErroresValidacion[rowId] = new Dictionary<string, string>();

                        ErroresValidacion[rowId][propertyName] = column.MensajeError ?? "Formato inválido";
                        return;
                    }
                }

                // Convertir el valor al tipo de propiedad y asignarlo
                Type propertyType = property.PropertyType;
                object? convertedValue = null;

                try
                {
                    if (propertyType == typeof(string))
                    {
                        convertedValue = value;
                    }
                    else if (propertyType == typeof(int) || propertyType == typeof(int?))
                    {
                        if (int.TryParse(value, out int intValue))
                            convertedValue = intValue;
                        else
                            throw new FormatException("El valor debe ser un número entero");
                    }
                    else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                    {
                        if (decimal.TryParse(value, out decimal decimalValue))
                            convertedValue = decimalValue;
                        else
                            throw new FormatException("El valor debe ser un número decimal");
                    }
                    else if (propertyType == typeof(double) || propertyType == typeof(double?))
                    {
                        if (double.TryParse(value, out double doubleValue))
                            convertedValue = doubleValue;
                        else
                            throw new FormatException("El valor debe ser un número");
                    }
                    else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                    {
                        if (DateTime.TryParse(value, out DateTime dateValue))
                            convertedValue = dateValue;
                        else
                            throw new FormatException("Formato de fecha inválido");
                    }
                    else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                    {
                        if (bool.TryParse(value, out bool boolValue))
                            convertedValue = boolValue;
                        else
                            convertedValue = value?.ToLower() == "si" || value?.ToLower() == "sí" || value?.ToLower() == "true";
                    }
                    else
                    {
                        // Para otros tipos, intentar usar un convertidor
                        convertedValue = Convert.ChangeType(value, propertyType);
                    }

                    property.SetValue(item, convertedValue);
                }
                catch (Exception ex)
                {
                    if (!ErroresValidacion.ContainsKey(rowId))
                        ErroresValidacion[rowId] = new Dictionary<string, string>();

                    ErroresValidacion[rowId][propertyName] = ex.Message;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void GuardarCambios(TItem item, string idFila)
        {
            FilaEnEdicion[idFila] = false;
        }

        private void CancelarEdicion(string idFila)
        {
            FilaEnEdicion[idFila] = false;
        }

        private bool TieneErrorValidacion(TItem item, string propiedad)
        {
            var claveItem = ObtenerClaveItem(item);
            return ErroresValidacion.ContainsKey(claveItem) &&
                   ErroresValidacion[claveItem].ContainsKey(propiedad);
        }

        private string ObtenerMensajeError(TItem item, string propiedad)
        {
            var claveItem = ObtenerClaveItem(item);
            if (ErroresValidacion.TryGetValue(claveItem, out var erroresPropiedad) &&
                erroresPropiedad.TryGetValue(propiedad, out var mensaje))
            {
                return mensaje;
            }
            return string.Empty;
        }

        private string ObtenerClaveItem<TItem>(TItem item)
        {
            var prop = typeof(TItem).GetProperty("Id");
            if (prop != null)
            {
                return prop.GetValue(item)?.ToString();
            }

            throw new InvalidOperationException("El tipo no contiene una propiedad 'Id'.");
        }

        // Ordenar columnas de la tabla
        private void OrdenarColumna(string accesor)
        {
            if (columnaOrdenada == accesor)
            {
                // Si ya está ordenado por esta columna, cambia la dirección
                ordenAscendente = !ordenAscendente;
            }
            else
            {
                // Si es una nueva columna, por defecto ordenamos ascendentemente
                columnaOrdenada = accesor;
                ordenAscendente = true;
            }

            // Ordenar los datos
            OrdenarDatos();
        }

        private void OrdenarDatos()
        {
            var propiedad = typeof(TItem).GetProperty(columnaOrdenada);

            if (propiedad != null)
            {
                // Ordenar según la dirección
                if (ordenAscendente)
                {
                    ElementosFiltrados = ElementosFiltrados.OrderBy(x => propiedad.GetValue(x, null)).ToList();
                }
                else
                {
                    ElementosFiltrados = ElementosFiltrados.OrderByDescending(x => propiedad.GetValue(x, null)).ToList();
                }
            }

            AplicarPaginado();
        }
    }
}