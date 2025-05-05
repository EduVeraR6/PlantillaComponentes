﻿using PruebaProyecto.Models.AGE.AgeCamposGenerales;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeArchivosMultimedia
{
    public class AgeArchivosMultimediaSaveDAO : AgeCamposGeneralesDAO
    {
        public AgeArchivosMultimediaPKDAO Id { get; set; }
        public string? Descripcion { get; set; } = null!;
        public string Ruta { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public string? Promocion { get; set; }
    }
}
