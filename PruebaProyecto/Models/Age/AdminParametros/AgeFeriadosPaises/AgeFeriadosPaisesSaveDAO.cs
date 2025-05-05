﻿using PruebaProyecto.Models.AGE.AgeCamposGenerales;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeFeriadoPais
{
    public class AgeFeriadosPaisesSaveDAO : AgeCamposGeneralesDAO
    {
        public AgeFeriadosPaisesPKDAO Id { get; set; }


        public DateTime FechaEjecucionDesde { get; set; }


        public DateTime FechaEjecucionHasta { get; set; }

        public DateTime HoraEjecucionDesde { get; set; }


        public DateTime HoraEjecucionHasta { get; set; }
    }
}
