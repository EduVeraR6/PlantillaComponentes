using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaComponents.Components.DropDownField
{
    public partial class DropDownField : ComponentBase
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public string Name { get; set; }
        [Parameter] public string PlaceholderText { get; set; }
        [Parameter] public string Value { get; set; }
        [Parameter] public EventCallback<string> ValueChanged { get; set; }
        [Parameter] public string ErrorMessage { get; set; }
        [Parameter] public bool IsRequired { get; set; }
        [Parameter] public bool IsDisabled { get; set; }
        [Parameter] public string LabelClassName { get; set; }
        [Parameter] public string InputClassName { get; set; }
        [Parameter] public string ErrorClassName { get; set; }

        [Parameter] public List<Option> Options { get; set; }

        private void HandleChange(ChangeEventArgs e)
        {
            Value = e.Value.ToString();
            ValueChanged.InvokeAsync(Value);
        }

    }

    public class Option
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }

}
