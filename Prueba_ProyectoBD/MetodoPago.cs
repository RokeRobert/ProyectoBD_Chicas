using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_ProyectoBD
{
    public class MetodoPago
    {
        public int ID { get; set; }      // Identificador único del método de pago.
        public string Nombre { get; set; } // Nombre del método de pago (por ejemplo, "Tarjeta de crédito").

        // Sobrescribe el método ToString() para devolver solo el nombre del método de pago,
        // lo que se mostrará en un ComboBox cuando se utilice esta clase.
        public override string ToString()
        {
            return Nombre; // Esto es lo que se mostrará en el ComboBox
        }
    }
}
