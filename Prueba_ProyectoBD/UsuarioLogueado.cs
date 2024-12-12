using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_ProyectoBD
{
    public static class UsuarioLogueado
    {
        public static int ID_Usuario { get; set; }// Alamacena el ID de usuario
        public static string Nombre { get; set; } // Almacena el nombre del usuario
        public static int ID_DireccionPredeterminada { get; set; } //Almace el ID de la direccion
    }
}
