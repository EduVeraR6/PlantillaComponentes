using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlantillaComponents.Models
{
    public class Posicion
    {
        public double Top { get; set; }
        public double Left { get; set; }
    }

  

    public class SelectOption
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }


    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string TipoCliente { get; set; }
        public string Estado { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Nombre: {Nombre}, Email: {Email}, Tipo Cliente: {TipoCliente} , Estado: {Estado} ";
        }

    }


}
