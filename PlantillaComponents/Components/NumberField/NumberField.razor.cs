using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaComponents.Components.NumberField
{
    public partial class NumberField : ComponentBase
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public string Name { get; set; }
        [Parameter] public double? Min { get; set; }
        [Parameter] public double? Max { get; set; }
        [Parameter] public string LabelClassName { get; set; }
        [Parameter] public string ErrorClassName { get; set; }
        [Parameter] public string InputClassName { get; set; }
        [Parameter] public bool IsRequired { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public double? DefaultValue { get; set; }
        [Parameter] public bool ShowTooltip { get; set; } = true;
        [Parameter] public EventCallback<double> OnChange { get; set; }
        [Parameter] public string RequiredMsg { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public string MinValueMsg { get; set; }
        [Parameter] public string MaxValueMsg { get; set; }
        [Parameter] public string NumberType { get; set; } = "float";

        private string TooltipMessage { get; set; }
        private double? Value { get; set; }
        private string ErrorMessage { get; set; }

        private void HandleInput(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            if (string.IsNullOrEmpty(value)) return;

            // Define regex based on number type
            var regex = NumberType == "integer" ? @"^\d*$" : @"^[\d.,]*$";

            if (!System.Text.RegularExpressions.Regex.IsMatch(value, regex))
            {
                TooltipMessage = NumberType == "integer" ? "Solo se permiten números enteros" : "Solo se permiten números decimales";
                return;
            }

            // Normalize comma to dot for float type
            if (NumberType == "float")
            {
                value = value.Replace(",", ".");
            }

            // Validate if it is a valid number
            if (!double.TryParse(value, out var parsedValue))
            {
                TooltipMessage = "Por favor ingrese un número válido";
                return;
            }

            TooltipMessage = null;
            Value = parsedValue;

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(parsedValue);
            }
        }

        private void HandleBlur()
        {
            if (Min.HasValue && Value < Min)
            {
                ErrorMessage = MinValueMsg ?? $"El valor mínimo es {Min.Value}";
            }
            else if (Max.HasValue && Value > Max)
            {
                ErrorMessage = MaxValueMsg ?? $"El valor máximo es {Max.Value}";
            }
            else
            {
                ErrorMessage = null;
            }
        }



    }
}
