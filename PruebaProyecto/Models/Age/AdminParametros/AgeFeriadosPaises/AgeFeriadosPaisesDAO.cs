﻿using PruebaProyecto.Models.Age.AdminParametros.AgeDiasFeriados;
using PruebaProyecto.Models.Age.AdminParametros.AgePaises;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeFeriadoPais
{
    public class AgeFeriadosPaisesDAO
    {
        public AgeFeriadosPaisesPKDAO Id { get; set; }

        public AgeDiasFeriadosDAO? DiasFeriados { get; set; }

        public AgePaisesDAO? Paises { get; set; }

        public DateTime? FechaEjecucionDesde { get; set; }


        public DateTime? FechaEjecucionHasta { get; set; }


        public DateTime? HoraEjecucionDesde { get; set; }


        public DateTime? HoraEjecucionHasta { get; set; }

        public string? Estado { get; set; }
    }
}
