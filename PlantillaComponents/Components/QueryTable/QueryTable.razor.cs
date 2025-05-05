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
        [Parameter] public EventCallback<TItem> AlNuevo { get; set; }
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

        //PROPIEDADES CONTROL ACCIONES

        private bool AccionEnProceso { get; set; } = false;
        private string AccionActual { get; set; } = string.Empty;

        //PROPIEDADES INSERCION REGISTRO

        private bool MostrarNuevoRegistro = false;

        private TItem NuevoItem;


        //PROPIEDADES PAGINACION
        private int TotalElementos { get; set; } = 0;
        private int IndicePagina { get; set; }
        private bool EsMovil { get; set; } = false;

        // PROPIEDADES GENERALES
        private bool HayError { get; set; } = false;
        private string FiltroGlobal { get; set; } = "";
        private string OrdenConsulta { get; set; } = "";
        private Dictionary<string, bool> FilasExpandidas { get; set; } = new();
        private List<TItem> FilasSeleccionadas { get; set; } = new();
        private bool MostrarConfirmacionEliminarMasivo { get; set; } = false;
        private TItem DatosSuperpuestos { get; set; }
        private Posicion PosicionSuperpuesta { get; set; }
        private Timer _temporizadorBusqueda;
        private List<TItem> ElementosFiltrados { get; set; } = new();
        private List<TItem> ElementosPaginados { get; set; } = new();
        private Dictionary<string, bool> FilaEnEdicion { get; set; } = new();

        private string columnaOrdenada;

        private bool ordenAscendente = true;

        private Dictionary<string, Dictionary<string, string>> ValoresTemporales = new Dictionary<string, Dictionary<string, string>>();

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
            IniciarAccion("actualizando-parametros");

            ElementosFiltrados = Elementos.Where(e =>
                string.IsNullOrEmpty(FiltroGlobal) ||
                ContieneFiltro(e, FiltroGlobal)
            ).ToList();

            TotalElementos = ElementosFiltrados.Count;
            AplicarPaginado();

            await base.OnParametersSetAsync();

            FinalizarAccion();
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
            IniciarAccion("configurando-filtro");

            FiltroGlobal = e.Value?.ToString() ?? "";
            IndicePagina = 0;

            _temporizadorBusqueda?.Dispose();
            _temporizadorBusqueda = new Timer(async _ =>
            {
                await InvokeAsync(() =>
                {
                    IniciarAccion("aplicando-filtro");
                    FiltrarDatos();
                    StateHasChanged();
                    FinalizarAccion();
                });
            }, null, RetardoBusqueda, Timeout.Infinite);

            FinalizarAccion();
        }

        private void LimpiarFiltroGlobal()
        {
            FiltroGlobal = "";
            IndicePagina = 0;
            FiltrarDatos();
        }

        private void FiltrarDatos()
        {
            IniciarAccion("filtrado");

            ElementosFiltrados = Elementos.Where(e =>
                string.IsNullOrEmpty(FiltroGlobal) || ContieneFiltro(e, FiltroGlobal)
            ).ToList();

            TotalElementos = ElementosFiltrados.Count;
            AplicarPaginado();

            FinalizarAccion();
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
            IniciarAccion("eliminar");

            await InvokeAsync(StateHasChanged);
            PosicionSuperpuesta = await JS.InvokeAsync<Posicion>("obtenerPosicionElemento", "btnEliminarCliente");
            DatosSuperpuestos = item;

            FinalizarAccion();
        }

        private async Task ConfirmarEliminar()
        {
            IniciarAccion("confirmando-eliminacion");

            if (DatosSuperpuestos != null)
            {
                await AlEliminar.InvokeAsync(DatosSuperpuestos);
            }
            DatosSuperpuestos = default;

            FinalizarAccion();
        }

        private void CancelarEliminar()
        {
            DatosSuperpuestos = default;
        }

        private async Task CambiarEstado(TItem item)
        {
            if (string.IsNullOrEmpty(AccesorEstado) || !AlCambiarEstado.HasDelegate)
                return;

            IniciarAccion("cambio-estado");

            var estadoActual = ObtenerValorPropiedad(item, AccesorEstado)?.ToString();
            var nuevoEstado = estadoActual == "A" ? "I" : "A";

            await AlCambiarEstado.InvokeAsync((item, nuevoEstado));

            FinalizarAccion();
        }

        private async Task ConfirmarEliminarMasivo()
        {
            IniciarAccion("eliminar-masivo");

            if (AlEliminarMasivo.HasDelegate && FilasSeleccionadas.Count > 0)
            {
                await AlEliminarMasivo.InvokeAsync(FilasSeleccionadas);
            }
            FilasSeleccionadas.Clear();
            MostrarConfirmacionEliminarMasivo = false;

            FinalizarAccion();
        }

        //PAGINACION

        private void IrPagina(int pagina)
        {
            IniciarAccion("cambio-pagina");

            IndicePagina = pagina;
            AplicarPaginado();

            FinalizarAccion();
        }

        private async Task CambiarTamanoPagina(int nuevoTamano)
        {
            IniciarAccion("cambio-tamano");

            TamanoPagina = nuevoTamano;
            IndicePagina = 0;
            AplicarPaginado();

            FinalizarAccion();
        }


        //METODOS OBTENCION PROPIEDADES
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


        // LOGICA EDICION FILA 
        private void AlternarEdicionFila(string idFila, TItem item)
        {
            IniciarAccion("editar-registro");


            if (FilaEnEdicion.ContainsKey(idFila))
            {
                FilaEnEdicion[idFila] = !FilaEnEdicion[idFila];
            }
            else
            {
                FilaEnEdicion[idFila] = true;
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
                        Console.WriteLine(ErroresValidacion[rowId][propertyName]);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void RegistrarCambioInput(TItem item, string accesor, string valor)
        {

            var rowId = item.GetHashCode().ToString();

            if (!ValoresTemporales.ContainsKey(rowId))
            {
                ValoresTemporales[rowId] = new Dictionary<string, string>();
            }

            ValoresTemporales[rowId][accesor] = valor;
        }

        private void ValidarYGuardarCambios(TItem item)
        {

            var rowId = item.GetHashCode().ToString();

            if (!ValoresTemporales.ContainsKey(rowId) || !ValoresTemporales[rowId].Any())
            {
                FilaEnEdicion[rowId] = false;
                return;
            }

            if (ErroresValidacion.ContainsKey(rowId))
            {
                ErroresValidacion[rowId].Clear();
            }

            bool esValido = true;

            foreach (var entrada in ValoresTemporales[rowId])
            {
                var accesor = entrada.Key;
                var valor = entrada.Value;
                var column = Columnas.FirstOrDefault(c => c.Accesor == accesor);

                if (column?.Requerido == true && string.IsNullOrWhiteSpace(valor))
                {
                    if (!ErroresValidacion.ContainsKey(rowId))
                        ErroresValidacion[rowId] = new Dictionary<string, string>();

                    ErroresValidacion[rowId][accesor] = "Este campo es requerido";
                    Console.WriteLine("Este campo es requerido");
                    esValido = false;
                    continue;
                }

                // Validar con Regex si está configurado
                if (!string.IsNullOrEmpty(column?.PatronRegex) && !string.IsNullOrEmpty(valor))
                {
                    var regex = new Regex(column.PatronRegex);
                    if (!regex.IsMatch(valor))
                    {
                        if (!ErroresValidacion.ContainsKey(rowId))
                            ErroresValidacion[rowId] = new Dictionary<string, string>();

                        ErroresValidacion[rowId][accesor] = column.MensajeError ?? "Formato inválido";
                        Console.WriteLine("Formato inválido");
                        esValido = false;
                        continue;
                    }
                }
            }

            if (esValido)
            {
                foreach (var entrada in ValoresTemporales[rowId])
                {
                    AsignarValorPropiedad(item, entrada.Key, entrada.Value);
                }

                ValoresTemporales.Remove(rowId);
                GuardarCambios(item, rowId);
            }

            StateHasChanged();
        }

        private void GuardarCambios(TItem item, string idFila)
        {
            AlSeleccionar.InvokeAsync(item);
            FinalizarAccion();
            FilaEnEdicion[idFila] = false;
        }

        private void CancelarEdicion(string idFila)
        {
            FinalizarAccion();
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

                Console.WriteLine(mensaje);

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


        //ORDERNAR COLUMNAS ASC DESC
        private void OrdenarColumna(string accesor)
        {
            if (columnaOrdenada == accesor)
            {
                ordenAscendente = !ordenAscendente;
            }
            else
            {
                columnaOrdenada = accesor;
                ordenAscendente = true;
            }

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

        private void IniciarNuevoRegistro()
        {
            IniciarAccion("registrar");

            // Crear una nueva instancia del tipo genérico
            NuevoItem = Activator.CreateInstance<TItem>();
            MostrarNuevoRegistro = true;

        }

        //NUEVO REGISTRO METODOS
        private async Task GuardarNuevoRegistro()
        {

            var rowId = NuevoItem?.GetHashCode().ToString();

            if (string.IsNullOrEmpty(rowId))
            {
                FinalizarAccion();
                return;
            }

            // Limpiar errores anteriores
            if (ErroresValidacion.ContainsKey(rowId))
            {
                ErroresValidacion[rowId].Clear();
            }

            bool esValido = true;

            if (ValoresTemporales.ContainsKey(rowId))
            {
                var valoresTemp = ValoresTemporales[rowId];

                foreach (var column in Columnas)
                {
                    if (valoresTemp.ContainsKey(column.Accesor))
                    {
                        var valor = valoresTemp[column.Accesor];

                        // Usar reflexión para asignar el valor al NuevoItem
                        var propiedad = typeof(TItem).GetProperty(column.Accesor);
                        if (propiedad != null)
                        {
                            try
                            {
                                // Convertir el valor al tipo adecuado de la propiedad
                                var valorConvertido = Convert.ChangeType(valor, propiedad.PropertyType);
                                propiedad.SetValue(NuevoItem, valorConvertido);
                                StateHasChanged();
                            }
                            catch (Exception ex)
                            {
                                // En caso de error al convertir, puedes manejarlo aquí
                                esValido = false;
                                if (!ErroresValidacion.ContainsKey(rowId))
                                {
                                    ErroresValidacion[rowId] = new Dictionary<string, string>();
                                }

                                ErroresValidacion[rowId][column.Accesor] = $"Error al convertir el valor: {ex.Message}";
                            }
                        }
                    }
                }
            }

            foreach (var column in Columnas.Where(c => c.Registrar != false))
            {
                var valor = ObtenerValorPropiedad(NuevoItem, column.Accesor)?.ToString() ?? string.Empty;

                if (column.Requerido && string.IsNullOrWhiteSpace(valor))
                {
                    if (!ErroresValidacion.ContainsKey(rowId))
                        ErroresValidacion[rowId] = new Dictionary<string, string>();

                    ErroresValidacion[rowId][column.Accesor] = "Este campo es requerido";
                    Console.WriteLine($"Este campo es requerido {column.Accesor}");
                    esValido = false;
                    continue;
                }

                // VALIDAR REGEX
                if (!string.IsNullOrEmpty(column.PatronRegex) && !string.IsNullOrEmpty(valor))
                {
                    var regex = new Regex(column.PatronRegex);
                    if (!regex.IsMatch(valor))
                    {
                        if (!ErroresValidacion.ContainsKey(rowId))
                            ErroresValidacion[rowId] = new Dictionary<string, string>();

                        ErroresValidacion[rowId][column.Accesor] = column.MensajeError ?? "Formato inválido";
                        Console.WriteLine($"Formato inválido {column.Accesor}");
                        esValido = false;
                        continue;
                    }
                }
            }

            if (!esValido)
            {
                StateHasChanged(); // Actualizar interfaz para mostrar errores
                return;
            }

            if (AlNuevo.HasDelegate)
            {
                await AlNuevo.InvokeAsync(NuevoItem);
                FinalizarAccion();
            }

            MostrarNuevoRegistro = false;

            if (ErroresValidacion.ContainsKey(rowId))
                ErroresValidacion.Remove(rowId);
            if (ValoresTemporales.ContainsKey(rowId))
                ValoresTemporales.Remove(rowId);

            NuevoItem = default;
           
            StateHasChanged();
        }

        private void CancelarNuevoRegistro()
        {
            MostrarNuevoRegistro = false;

            if (NuevoItem != null)
            {
                var rowId = NuevoItem.GetHashCode().ToString();

                if (ErroresValidacion.ContainsKey(rowId))
                {
                    ErroresValidacion.Remove(rowId);
                }


            }
            FinalizarAccion();
            NuevoItem = default;
            StateHasChanged();
        }

        //CONTROL ACCIONES METODOS

        private void IniciarAccion(string nombreAccion)
        {
            AccionEnProceso = true;
            AccionActual = nombreAccion;
            StateHasChanged();
        }

        private void FinalizarAccion()
        {
            AccionEnProceso = false;
            AccionActual = string.Empty;
            StateHasChanged();
        }



    }
}