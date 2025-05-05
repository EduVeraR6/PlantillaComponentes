using System.Reflection;
using System.Text.Json.Serialization;

namespace PlantillaComponents.Utils.Paginacion
{
    public class Sort<TModel>
    {
        [JsonPropertyName("fieldName")]
        public string FieldName { get; set; }

        [JsonPropertyName("direction")]
        public string Direction { get; set; }

    }
}
