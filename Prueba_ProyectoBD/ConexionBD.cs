using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Prueba_ProyectoBD
{
    public class ConexionBD
    {
        // Cadena de conexión a la base de datos
        static string ConexionString = @"server= DESKTOP-0TP6D1B\SQLEXPRESS ; database= TiendaMa; integrated security= true";

        // Objeto de conexión estático para la base de datos
        // Permite el acceso a la base de datos desde cualquier parte de la aplicación
        public static SqlConnection Conexion = new SqlConnection(ConexionString);

    }
}
