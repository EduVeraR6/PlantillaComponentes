using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaComponents.Components.CheckboxField
{
    public partial class CheckboxField : ComponentBase
    {
        [Parameter] public string Name { get; set; } = string.Empty;
        [Parameter] public string Label { get; set; } = string.Empty;
        [Parameter] public bool IsDisabled { get; set; }
        [Parameter] public bool Value { get; set; }
        [Parameter] public EventCallback<bool> ValueChanged { get; set; }
        [Parameter] public string? ErrorMessage { get; set; }

        [Parameter] public string? CheckClassName { get; set; }
        [Parameter] public string? LabelClassName { get; set; }
        [Parameter] public string? ErrorClassName { get; set; }

   
    }
}
