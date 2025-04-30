using PlantillaComponents.Models;

public class Validador<TItem> 
{
    private readonly Func<TItem, string> obtenerClaveItem;

    public Validador(Func<TItem, string> obtenerClaveItem)
    {
        this.obtenerClaveItem = obtenerClaveItem;
    }

    private readonly Dictionary<string, Dictionary<string, string>> ErroresValidacion = new();

    public string ObtenerClaveItem(TItem item)
    {
        return obtenerClaveItem(item);
    }

    public void AgregarError(TItem item, string propiedad, string mensaje)
    {
        var clave = ObtenerClaveItem(item);

        if (!ErroresValidacion.ContainsKey(clave))
        {
            ErroresValidacion[clave] = new Dictionary<string, string>();
        }

        ErroresValidacion[clave][propiedad] = mensaje;
    }

    public bool TieneErrorValidacion(TItem item, string propiedad)
    {
        var claveItem = ObtenerClaveItem(item);
        return ErroresValidacion.ContainsKey(claveItem) &&
               ErroresValidacion[claveItem].ContainsKey(propiedad);
    }

    public string ObtenerMensajeError(TItem item, string propiedad)
    {
        var claveItem = ObtenerClaveItem(item);
        if (ErroresValidacion.TryGetValue(claveItem, out var erroresPropiedad) &&
            erroresPropiedad.TryGetValue(propiedad, out var mensaje))
        {
            return mensaje;
        }
        return string.Empty;
    }
}
