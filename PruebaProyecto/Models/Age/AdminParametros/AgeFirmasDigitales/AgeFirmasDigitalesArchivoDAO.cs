﻿
using System.ComponentModel.DataAnnotations;

namespace PruebaProyecto.Models.Age.AdminParametros.AgeFirmasDigitales
{
    public class AgeFirmasDigitalesArchivoDAO
    {   
        [Required(ErrorMessage = "El Nombre del archivo de la firma digital no puede ser nulo.")]
        public string? NombreArchivo { get; set; }
    }
}
