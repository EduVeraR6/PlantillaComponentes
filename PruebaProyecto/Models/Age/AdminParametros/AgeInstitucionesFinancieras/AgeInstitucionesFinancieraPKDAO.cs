﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeInstitucionesFinancieras
{
    public class AgeInstitucionesFinancieraPKDAO
    {
        [Required(ErrorMessage = "El código de licenciatario no puede ser nulo.")]
        [RegularExpression("^[1-9]\\d*$", ErrorMessage = "El código de licenciatario debe ser un número entero mayor a cero.")]
        [FromRoute(Name = "codigoLicenciatario")]
        public int AgeLicencCodigo { get; set; }


        [Required(ErrorMessage = "El código de institución financiera no puede ser nulo.")]
        [RegularExpression("^[1-9]\\d*$", ErrorMessage = "El código de institución financiera debe ser un número entero mayor a cero.")]
        [FromRoute(Name = "id")]
        public int Codigo { get; set; }
    }
}
