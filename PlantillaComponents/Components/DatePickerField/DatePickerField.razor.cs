using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaComponents.Components.DatePickerField
{
    public partial class DatePickerField : ComponentBase
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public string Name { get; set; }
        [Parameter] public string LabelClassName { get; set; }
        [Parameter] public string InputClassName { get; set; }
        [Parameter] public string ErrorClassName { get; set; }
        [Parameter] public bool IsRequired { get; set; }
        [Parameter] public DateTime? MinDate { get; set; }
        [Parameter] public DateTime? MaxDate { get; set; }
        [Parameter] public string PlaceholderText { get; set; }
        [Parameter] public string RequiredMsg { get; set; }

        [Parameter]
        public DateTime? Value { get; set; }

        [Parameter]
        public EventCallback<DateTime?> ValueChanged { get; set; }

        private string ErrorMessage { get; set; }

        private string GetFormattedValue()
        {
            return Value.HasValue ? Value.Value.ToString("yyyy-MM-dd") : string.Empty;
        }

        private async Task HandleChange(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                Value = date;
            }
            else
            {
                Value = null;
            }

            Validate();

            await ValueChanged.InvokeAsync(Value);
        }

        private void Validate()
        {
            ErrorMessage = null;

            if (IsRequired && Value == null)
            {
                ErrorMessage = RequiredMsg ?? "Este campo es obligatorio.";
            }
            else if (Value.HasValue)
            {
                if (MinDate.HasValue && Value < MinDate)
                {
                    ErrorMessage = "La fecha debe ser posterior a la mínima.";
                }
                else if (MaxDate.HasValue && Value > MaxDate)
                {
                    ErrorMessage = "La fecha debe ser anterior a la máxima.";
                }
            }
        }
    }
}
