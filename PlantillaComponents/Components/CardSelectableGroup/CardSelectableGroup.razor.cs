using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaComponents.Components.CardSelectableGroup
{
    public partial class CardSelectableGroup : ComponentBase
    {
        [Parameter] public string Name { get; set; } = Guid.NewGuid().ToString();
        [Parameter] public string? BoundValue { get; set; }
        [Parameter] public EventCallback<string?> BoundValueChanged { get; set; }

        [Parameter] public List<CardOption> Options { get; set; } = new();
        [Parameter] public string? GroupClassName { get; set; }
        [Parameter] public string? CardClassName { get; set; }
        [Parameter] public string? ErrorClassName { get; set; }
        [Parameter] public string? OptionClassName { get; set; }
        [Parameter] public string? Error { get; set; }
        [Parameter] public int CardSizeX { get; set; } = 300;
        [Parameter] public int CardSizeY { get; set; } = 200;

        private string CardSizeXpx => $"{CardSizeX}px";
        private string CardSizeYpx => $"{CardSizeY}px";

        private Task OnSelect(string value)
        {
            BoundValue = value;
            return BoundValueChanged.InvokeAsync(value);
        }

    }

    public class CardOption
    {
        public string Value { get; set; } = default!;
        public string? Label { get; set; }
        public bool IsLoadingContent { get; set; } = false;
        public RenderFragment? Content { get; set; }
    }
}
