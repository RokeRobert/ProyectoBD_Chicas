using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_ProyectoBD
{
    public class DireccionEnvio
    {
        public int ID_Direccion { get; set; }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Pais { get; set; }
        public string Telefono { get; set; }

        public override string ToString()
        {
            return $"{Direccion}, {CodigoPostal}, {Pais}, {Telefono}";  // Lo que se muestra en el ListBox
        }
    }
}
