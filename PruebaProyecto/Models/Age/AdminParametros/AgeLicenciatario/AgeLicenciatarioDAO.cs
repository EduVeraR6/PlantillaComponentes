﻿using PruebaProyecto.Models.Age.AdminParametros.AgeArchivosMultimedia;
using Newtonsoft.Json;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeLicenciatario
{
    public class AgeLicenciatarioDAO
    {
        [JsonProperty("codigo")]
        public int Codigo { get; set; }

        [JsonProperty("ageClaCoCodigo")]
        public long AgeClaCoCodigo { get; set; }

        [JsonProperty("ageTipIdCodigo")]
        public int AgeTipIdCodigo { get; set; }

        [JsonProperty("nombreCorto")]
        public string NombreCorto { get; set; } = null!;

        [JsonProperty("representanteLegal")]
        public string RepresentanteLegal { get; set; } = null!;

        [JsonProperty("direccionPrincipal")]
        public string DireccionPrincipal { get; set; } = null!;

        [JsonProperty("telefono1")]
        public string Telefono1 { get; set; } = null!;

        [JsonProperty("eMail2")]
        public string? EMail2 { get; set; }

        [JsonProperty("esCorporacion")]
        public string? EsCorporacion { get; set; }

        [JsonProperty("observacion")]
        public string? Observacion { get; set; }

        [JsonProperty("paginaWeb")]
        public string? PaginaWeb { get; set; }

        [JsonProperty("contribuyenteEspecialResoluc")]
        public string? ContribuyenteEspecialResoluc { get; set; }

        [JsonProperty("rutaLicenciatario")]
        public string RutaLicenciatario { get; set; } = null!;

        [JsonProperty("ageLicencCodigo")]
        public int? AgeLicencCodigo { get; set; }

        [JsonProperty("razonSocial")]
        public string? RazonSocial { get; set; }

        [JsonProperty("nombreComercial")]
        public string NombreComercial { get; set; } = null!;

        [JsonProperty("numeroIdentificacion")]
        public string NumeroIdentificacion { get; set; } = null!;

        [JsonProperty("telefono2")]
        public string? Telefono2 { get; set; }

        [JsonProperty("eMail1")]
        public string? EMail1 { get; set; }

        [JsonProperty("obligadoLlevarContabilidad")]
        public string ObligadoLlevarContabilidad { get; set; } = null!;

        [JsonProperty("descripcion")]
        public string? Descripcion { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; } = null!;

        [JsonProperty("ageArchivosMultimediaList")]
        public List<AgeArchivosMultimediaDAO> AgeArchivosMultimediaList { get; set; }
    }
}
