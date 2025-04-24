using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaComponents.Components.Button
{
    public partial class Button : ComponentBase
    {
        [Parameter] public string Variant { get; set; } = "primary";
        [Parameter] public string? ClassName { get; set; }
        [Parameter] public string Type { get; set; } = "button";
        [Parameter] public string? Href { get; set; }
        [Parameter] public EventCallback OnClick { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }

        private void HandleClick()
        {
            if (!string.IsNullOrEmpty(Href))
            {
                NavigationManager.NavigateTo(Href);
            }
            else
            {
                OnClick.InvokeAsync(null);
            }
        }
    }
}
