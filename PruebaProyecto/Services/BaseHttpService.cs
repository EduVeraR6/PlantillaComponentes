using System.Net.Http.Json;
using Blazored.Toast.Services;
using PruebaProyecto.Utils;

namespace PruebaProyecto.Services
{
    public class BaseHttpService
    {
        protected readonly HttpClient _http;
        protected readonly IToastService _toast;

        protected BaseHttpService(HttpClient http, IToastService toast)
        {
            _http = http;
            _toast = toast;
        }

        protected async Task<T?> GetAsync<T>(string? url = "", string? successMessage = null)
        {
            try
            {
                CustomResponse<T>? response = await _http.GetFromJsonAsync<CustomResponse<T>>(url);

                if (response?.StatusCode == 200)
                {
                    if (!string.IsNullOrEmpty(successMessage))
                        _toast.ShowSuccess(successMessage);
                    return response.Content;
                }

                _toast.ShowError(response?.Message ?? "Error en la solicitud.");
                return default;
            }
            catch (Exception ex)
            {
                _toast.ShowError($"Excepción: {ex.Message}");
                return default;
            }
        }


        protected async Task<TResponse?> PostAsync<TRequest, TResponse>(TRequest data, string? url = "", string? successMessage = null)
        {
            try
            {
                HttpResponseMessage httpResponse = await _http.PostAsJsonAsync(url, data);
                CustomResponse<TResponse> response = await httpResponse.Content.ReadFromJsonAsync<CustomResponse<TResponse>>();

                if (response?.StatusCode == 201 || httpResponse.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(successMessage))
                        _toast.ShowSuccess(successMessage);
                    return response.Content;
                }

                _toast.ShowError(response?.Message ?? "Error al guardar.");
                return default;
            }
            catch (Exception ex)
            {
                _toast.ShowError($"Excepción: {ex.Message}");
                return default;
            }
        }



        protected async Task<TResponse?> PutAsync<TRequest, TResponse>(TRequest data, string? url = null, string? successMessage = null)
        {
            try
            {
                HttpResponseMessage httpResponse = await _http.PutAsJsonAsync(url, data);
                CustomResponse<TResponse>? response = await httpResponse.Content.ReadFromJsonAsync<CustomResponse<TResponse>>();

                if (response?.StatusCode == 200 || httpResponse.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(successMessage))
                        _toast.ShowSuccess(successMessage);
                    return response.Content;
                }

                _toast.ShowError(response?.Message ?? "Error al actualizar.");
                return default;
            }
            catch (Exception ex)
            {
                _toast.ShowError($"Excepción: {ex.Message}");
                return default;
            }
        }



        protected async Task<bool> DeleteAsync(string? url = "", string? successMessage = null)
        {
            try
            {
                HttpResponseMessage httpResponse = await _http.DeleteAsync(url);
                CustomResponse<object>? response = await httpResponse.Content.ReadFromJsonAsync<CustomResponse<object>>();

                if (response?.StatusCode == 200 || httpResponse.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(successMessage))
                        _toast.ShowSuccess(successMessage);
                    return true;
                }

                _toast.ShowError(response?.Message ?? "Error al eliminar.");
                return false;
            }
            catch (Exception ex)
            {
                _toast.ShowError($"Excepción: {ex.Message}");
                return false;
            }
        }
    }
}
