using Blazored.Toast.Services;
using static PruebaProyecto.Pages.AuthTest;

namespace PruebaProyecto.Services
{
    public class UbicacionesService : BaseHttpService
    {
        public UbicacionesService(HttpClient client, IToastService toast) : base(client, toast) { }

        public async Task<List<Ubicacione>> ObtenerUbicaciones()
        {
            return await GetAsync<List<Ubicacione>>(successMessage: "Ubicaciones cargadas correctamente.") ?? new();
        }

        public async Task<List<object>> ObtenerTodosAsync()
        {
            // método GetAsync sirve para obtener 1 (por id) entidad o varias,
            // dependerá de la url y el tipo de dato de respuesta.
            return await GetAsync<List<object>>(successMessage: "Entidades cargadas correctamente.") ?? new();
        }

        public async Task<object?> CrearAsync(object saveDto)
        {
            // el primer object es el dto que se envía a guardar, el segundo es la respuesta.
            return await PostAsync<object, object>(saveDto, successMessage: "Entidad creada correctamente.");
        }

        public async Task<object?> ActualizarAsync(int id, object saveDto)
        {
            // el primer object es el dto que se envía a guardar, el segundo es la respuesta.
            return await PutAsync<object, object>(saveDto, $"{id}", "Entidad actualizada correctamente.");
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await DeleteAsync($"{id}", "Entidad eliminada correctamente.");
        }
    }
}
