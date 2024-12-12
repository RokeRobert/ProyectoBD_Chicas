using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prueba_ProyectoBD
{
    public partial class InicioSesion : Form
    {
        public InicioSesion()
        {
            InitializeComponent();
            txtCE.Text = "Correo Electrónico";
            txtCE.ForeColor = Color.Silver;

            txtPW.Text = "Contraseña";
            txtPW.ForeColor = Color.Silver;
            txtPW.UseSystemPasswordChar = false; // Mostrar texto en lugar de puntos
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(txtCE.Text) || string.IsNullOrWhiteSpace(txtPW.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Consulta SQL para verificar las credenciales del usuario en la base de datos.
                // Se busca si el correo y la contraseña coinciden con algún registro en la tabla "Usuarios"
                string query = "SELECT ID_Usuario, NombreUsuario FROM Usuarios WHERE Email = @Email AND Contraseña = @Password";

                using (SqlCommand cmd = new SqlCommand(query, ConexionBD.Conexion))
                {
                    // Agregar los parámetros de correo y contraseña a la consulta
                    cmd.Parameters.AddWithValue("@Email", txtCE.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPW.Text);

                    // Abrir la conexión a la base de datos.
                    ConexionBD.Conexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Si se encuentra un usuario con las credenciales correctas, se obtiene su ID y nombre
                    if (reader.Read())
                    {
                        int idUsuario = reader.GetInt32(0);// Recupera el ID del usuario
                        string nombreUsuario = reader.GetString(1); // Recupera el nombre del usuario.

                        // Guardar el ID y el nombre del usuario en la clase estática UsuarioLogueado
                        UsuarioLogueado.ID_Usuario = idUsuario;
                        UsuarioLogueado.Nombre = nombreUsuario; // Guardar el nombre en la clase estática

                        // Inicio de sesión exitoso
                        MessageBox.Show($"Inicio de sesión exitoso. ¡Bienvenido, {nombreUsuario}!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Redirigir a la pantalla principal 
                        this.Hide();
                        frmInicio principal = new frmInicio();
                        principal.Show();
                    }
                    else
                    {
                        // Si las credenciales son incorrectas, muestra un mensaje de error
                        MessageBox.Show("Correo o contraseña incorrectos. Intente nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // Cierra la conexión a la base de datos después de realizar la consulta.
                ConexionBD.Conexion.Close();
            }
            catch (Exception ex)
            {
                // Si ocurre un error durante el proceso de inicio de sesión, muestra un mensaje de error
                MessageBox.Show($"Error al intentar iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            RegistroCuenta registro = new RegistroCuenta();
            registro.Show();
            this.Hide();
        }

        private void txtCE_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPW_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCE_Enter(object sender, EventArgs e)
        {
            if (txtCE.Text == "Correo Electrónico")
            {
                txtCE.Text = string.Empty;
                txtCE.ForeColor = Color.Black; 
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

        private void txtPW_Enter(object sender, EventArgs e)
        {
            if (txtPW.Text == "Contraseña")
            {
                txtPW.Text = string.Empty;
                txtPW.ForeColor = Color.Black; // Color normal del texto
            }
        }

        private void txtPW_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPW.Text))
            {
                txtPW.Text = "Contraseña";
                txtPW.ForeColor = Color.Silver; // Color del marcador de posición
            }
        }
    }
}

