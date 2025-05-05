using PruebaProyecto.Models.AGE.AgeCamposGenerales;
using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeSucursales
{
    public class AgeSucursalesSaveDAO : AgeCamposGeneralesDAO
    {
        [JsonProperty("id")]
        public AgeSucursalesPKDAO Id { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; } = null!;

        [JsonProperty("ageAgeTipLoAgePaisCodigo")]
        public int AgeAgeTipLoAgePaisCodigo { get; set; }

        [JsonProperty("ageLocaliAgeTipLoCodigo")]
        public int AgeLocaliAgeTipLoCodigo { get; set; }

        [JsonProperty("ageLocaliCodigo")]
        public int AgeLocaliCodigo { get; set; }

        [JsonProperty("ageTipSuAgeLicencCodigo")]
        public int AgeTipSuAgeLicencCodigo { get; set; }

        [JsonProperty("ageTipSuCodigo")]
        public int AgeTipSuCodigo { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; } = null!;

        [JsonProperty("telefono1")]
        public string Telefono1 { get; set; } = null!;

        [JsonProperty("eMail1")]
        public string? EMail1 { get; set; }

        [JsonProperty("eMail2")]
        public string? EMail2 { get; set; }

        [JsonProperty("ageSucursAgeLicencCodigo")]
        public int? AgeSucursAgeLicencCodigo { get; set; }

        [JsonProperty("ageSucursCodigo")]
        public int? AgeSucursCodigo { get; set; }

        [JsonProperty("codigoEstablecimiento")]
        public short? CodigoEstablecimiento { get; set; }

        [JsonProperty("telefono2")]
        public string? Telefono2 { get; set; }

        [JsonProperty("observacion")]
        public string? Observacion { get; set; }

        [JsonProperty("latitud")]
        public string? Latitud { get; set; }

        [JsonProperty("longitud")]
        public string? Longitud { get; set; }

        [JsonProperty("centroAcopio")]
        public string? CentroAcopio { get; set; }
    }
}