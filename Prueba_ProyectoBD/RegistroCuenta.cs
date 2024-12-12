using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Prueba_ProyectoBD
{
    public partial class RegistroCuenta : Form
    {
        public RegistroCuenta()
        {
            InitializeComponent();
            // Configuración inicial de los TextBox con los marcadores de posición
            txt_NC.Text = "Nombre Completo";
            txt_NC.ForeColor = Color.Silver;

            txt_Nombre.Text = "Nombre de Usuario";
            txt_Nombre.ForeColor = Color.Silver;

            txtCE.Text = "Correo Electrónico";
            txtCE.ForeColor = Color.Silver;

            txtPW.Text = "Contraseña";
            txtPW.ForeColor = Color.Silver;
            txtPW.UseSystemPasswordChar = false; // Mostrar texto en lugar de puntos

            txt_ConfiContrasena.Text = "Confirmacion de Contraseña";
            txt_ConfiContrasena.ForeColor = Color.Silver;
            txt_ConfiContrasena.UseSystemPasswordChar = false; // Mostrar texto en lugar de puntos
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(txt_NC.Text) ||
                string.IsNullOrWhiteSpace(txt_Nombre.Text) ||
                string.IsNullOrWhiteSpace(txtCE.Text) ||
                string.IsNullOrWhiteSpace(txtPW.Text) ||
                string.IsNullOrWhiteSpace(txt_ConfiContrasena.Text))
            {
                // Si alguno está vacío, se muestra un mensaje de error y el proceso se detiene
                MessageBox.Show("Todos los campos son obligatorios. Por favor, complételos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar que la contraseña tenga al menos 3 caracteres
            if (txtPW.Text.Length < 3)
            {
                MessageBox.Show("La contraseña debe tener al menos 3 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar que las contraseñas coincidan
            if (txtPW.Text != txt_ConfiContrasena.Text)
            {
                // Si no coinciden, se muestra un mensaje de error, se limpian los campos de la contraseña
                MessageBox.Show("Las contraseñas no coinciden. Por favor, inténtelo de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPW.Clear();
                txt_ConfiContrasena.Clear();
                txtPW.Focus();
                return;
            }

            try
            {
                // Validar si el nombre de usuario ya existe
                if (!ValidarNombreUsuarioUnico(txt_Nombre.Text))
                {
                    // Si el nombre ya existe, se muestra un mensaje de error
                    MessageBox.Show("El nombre de usuario ya está registrado. Por favor, elige otro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt_Nombre.Focus();
                    return;
                }

                // Verificar si el correo ya existe
                if (CorreoYaRegistrado(txtCE.Text))
                {
                    // Si el correo ya está registrado, se muestra un mensaje de error
                    MessageBox.Show("El correo ya está registrado. Por favor, use otro correo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCE.Focus();
                    return;
                }

                // Generar ID único para el nuevo usuario
                int nuevoID = GenerarIDUnico();

                // Insertar los datos del nuevo usuario en la base de datos
                RegistrarUsuario(nuevoID, txt_Nombre.Text, txtCE.Text, txtPW.Text);
                // Muestra un mensaje de éxito si el registro se realizó correctamente
                MessageBox.Show("Cuenta registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Redirigir al formulario de inicio de sesión
                this.Hide();
                InicioSesion loginForm = new InicioSesion();
                loginForm.Show();
            }
            catch (Exception ex)
            {
                // Si ocurre un error durante el proceso de registro, se captura la excepción
                // y se muestra un mensaje de error con el detalle del problema
                MessageBox.Show($"Ocurrió un error durante el registro: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CorreoYaRegistrado(string correo)
        {
            // Consulta SQL para contar cuántos registros tienen el correo especificado.
            string query = "SELECT COUNT(*) FROM Usuarios WHERE Email = @Email";
            // Crear un comando SQL con la consulta y la conexión a la base de datos
            using (SqlCommand cmd = new SqlCommand(query, ConexionBD.Conexion))
            {
                // Agregar el parámetro @Email al comando con el valor proporcionado por el usuario
                cmd.Parameters.AddWithValue("@Email", correo);
                // Abre la conexión a la base de datos.
                ConexionBD.Conexion.Open();
                // Ejecuta la consulta y obtiene el número de registros que coinciden con el correo dado
                int count = (int)cmd.ExecuteScalar(); // Devuelve el número de registros encontrados
                // Cierra la conexión a la base de datos después de ejecutar la consulta
                ConexionBD.Conexion.Close();

                return count > 0; // Si count es mayor que 0, el correo ya está registrado
            }
        }

        private int GenerarIDUnico()
        {
            int nuevoID = 1; // Si no existen registros, el ID inicial será 1
            // Consulta SQL para obtener el valor máximo de ID_Usuario en la tabla Usuarios
            string query = "SELECT MAX(ID_Usuario) FROM Usuarios";
            // Crear un comando SQL con la consulta y la conexión a la base de datos.
            using (SqlCommand cmd = new SqlCommand(query, ConexionBD.Conexion))
            {
                // Abrir la conexión a la base de datos
                ConexionBD.Conexion.Open();
                // Ejecuta la consulta y obtener el valor máximo del ID de usuario
                object resultado = cmd.ExecuteScalar();// ExecuteScalar devuelve el valor de la primera columna de la primera fila

                // Si se encontró un valor (no es DBNull), incrementar el valor máximo para generar un nuevo ID
                if (resultado != DBNull.Value)
                {
                    nuevoID = Convert.ToInt32(resultado) + 1;
                }
                // Cerrar la conexión a la base de datos
                ConexionBD.Conexion.Close();
            }
            // Devolver el nuevo ID único generado
            return nuevoID;
        }

        private void RegistrarUsuario(int id, string nombre, string email, string contraseña)
        {
            // Consulta SQL para insertar un nuevo usuario en la tabla Usuarios
            string InsertarUsuario = "INSERT INTO Usuarios (NombreUsuario, NombreCompleto, Email, Contraseña) " +
                         "VALUES (@Nombre, @NombreCompleto, @Email, @Contraseña)";

            // Crear un objeto SqlCommand con la consulta SQL y la conexión a la base de datos
            using (SqlCommand cmd = new SqlCommand(InsertarUsuario, ConexionBD.Conexion))
            {
                // Asignar los valores de los parámetros de la consulta SQL 
                cmd.Parameters.AddWithValue("@Nombre", txt_Nombre.Text);// Nombre de usuario
                cmd.Parameters.AddWithValue("@NombreCompleto", txt_NC.Text);// Nombre completo del usuario
                cmd.Parameters.AddWithValue("@Email", txtCE.Text);// Correo electrónico
                cmd.Parameters.AddWithValue("@Contraseña", txtPW.Text); // Contraseña del usuario
                // Abrir la conexión a la base de datos.
                ConexionBD.Conexion.Open();
                // Ejecuta la consulta para insertar los datos del usuario en la base de datos
                cmd.ExecuteNonQuery();// Ejecuta la consulta sin esperar un valor de retorno
                                      // Cerrar la conexión a la base de datos
                ConexionBD.Conexion.Close();
            }
        }

        private bool ValidarNombreUsuarioUnico(string nombreUsuario)
        {
            // Consulta SQL que cuenta el número de registros con el mismo NombreUsuario en la tabla Usuarios
            string consulta = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @NombreUsuario";
            // Crear un objeto SqlCommand con la consulta SQL y la conexión a la base de datos.
            using (SqlCommand cmd = new SqlCommand(consulta, ConexionBD.Conexion))
            {
                // Asigna el valor del nombre de usuario como parámetro de la consulta SQL
                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                // Abrir la conexión a la base de datos
                ConexionBD.Conexion.Open();

                // Ejecuta la consulta y obtiene el conteo de registros con ese nombre de usuario
                int count = (int)cmd.ExecuteScalar();// ExecuteScalar devuelve el primer valor de la primera columna
                // Cerrar la conexión a la base de datos
                ConexionBD.Conexion.Close();

                return count == 0; // Si el conteo es 0, el nombre de usuario es único
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            InicioSesion inicioSesion = new InicioSesion();
            inicioSesion.Show();
            this.Hide();
        }

        // Para el TextBox Nombre Completo
        private void txt_NC_Enter(object sender, EventArgs e)
        {
            if (txt_NC.Text == "Nombre Completo")
            {
                txt_NC.Text = string.Empty;
                txt_NC.ForeColor = Color.Black; // Color normal del texto
            }
        }

        private void txt_NC_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_NC.Text))
            {
                txt_NC.Text = "Nombre Completo";
                txt_NC.ForeColor = Color.Silver; // Color del marcador de posición
            }
        }

        // Para el TextBox Nombre de Usuario
        private void txt_Nombre_Enter(object sender, EventArgs e)
        {
            if (txt_Nombre.Text == "Nombre de Usuario")
            {
                txt_Nombre.Text = string.Empty;
                txt_Nombre.ForeColor = Color.Black; // Color normal del texto
            }
        }

        private void txt_Nombre_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Nombre.Text))
            {
                txt_Nombre.Text = "Nombre de Usuario";
                txt_Nombre.ForeColor = Color.Silver; // Color del marcador de posición
            }
        }

        // Para el TextBox Correo Electrónico
        private void txtCE_Enter(object sender, EventArgs e)
        {
            if (txtCE.Text == "Correo Electrónico")
            {
                txtCE.Text = string.Empty;
                txtCE.ForeColor = Color.Black; // Color normal del texto
            }
        }

        private void txtCE_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCE.Text))
            {
                txtCE.Text = "Correo Electrónico";
                txtCE.ForeColor = Color.Silver; // Color del marcador de posición
            }
        }

        // Para el TextBox Contraseña
        private void txtPW_Enter(object sender, EventArgs e)
        {
            if (txtPW.Text == "Contraseña")
            {
                txtPW.Text = string.Empty;
                txtPW.ForeColor = Color.Black; // Color normal del texto
                txtPW.UseSystemPasswordChar = false; // Ocultar texto como contraseña
            }
        }

        private void txtPW_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPW.Text))
            {
                txtPW.UseSystemPasswordChar = false; // Mostrar texto como normal
                txtPW.Text = "Contraseña";
                txtPW.ForeColor = Color.Silver; // Color del marcador de posición
            }
        }

        // Para el TextBox Confirmación de Contraseña
        private void txt_ConfiContrasena_Enter(object sender, EventArgs e)
        {
            if (txt_ConfiContrasena.Text == "Confirmacion de Contraseña")
            {
                txt_ConfiContrasena.Text = string.Empty;
                txt_ConfiContrasena.ForeColor = Color.Black; // Color normal del texto
                txt_ConfiContrasena.UseSystemPasswordChar = false; // Ocultar texto como contraseña
            }
        }

        private void txt_ConfiContrasena_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_ConfiContrasena.Text))
            {
                txt_ConfiContrasena.UseSystemPasswordChar = false; // Mostrar texto como normal
                txt_ConfiContrasena.Text = "Confirmacion de Contraseña";
                txt_ConfiContrasena.ForeColor = Color.Silver; // Color del marcador de posición
            }
        }

    }
}
