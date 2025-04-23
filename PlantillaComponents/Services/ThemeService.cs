using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace PlantillaComponents.Services
{
    public class ThemeService
    {
        private readonly IJSRuntime _js;
        private const string ThemeKey = "theme";
        public string CurrentTheme { get; private set; } = "light";
        public event Action? OnThemeChanged;

        public ThemeService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task InitializeAsync()
        {
            var savedTheme = await _js.InvokeAsync<string>("localStorage.getItem", ThemeKey);
            if (!string.IsNullOrWhiteSpace(savedTheme))
            {
                CurrentTheme = savedTheme;
            }

            await ApplyThemeAsync();
        }

        public async Task ToggleThemeAsync()
        {
            CurrentTheme = CurrentTheme == "light" ? "dark" : "light";
            await _js.InvokeVoidAsync("localStorage.setItem", ThemeKey, CurrentTheme);
            await ApplyThemeAsync();
            OnThemeChanged?.Invoke();
        }

        private async Task ApplyThemeAsync()
        {
            await _js.InvokeVoidAsync("setThemeClass", CurrentTheme);
        }
    }
}
