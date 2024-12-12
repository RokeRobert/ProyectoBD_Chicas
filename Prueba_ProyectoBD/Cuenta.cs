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
    public partial class Cuenta : Form
    {
        public Cuenta()
        {
            InitializeComponent();
            
        }

        private void btn_CambioCorreo_Click(object sender, EventArgs e)
        {
            MostrarFormularioCambioCorreo();
        }

        private void btn_CambioContraseña_Click(object sender, EventArgs e)
        {
            MostrarFormularioCambioContrasena();
        }

        private void btn_CamDireccion_Click(object sender, EventArgs e)
        {

            MostrarFormularioCambioDireccion();
            
        }

        // Métodos para cargar formularios en el FlowLayoutPanel
        private void MostrarFormularioCambioCorreo()
        {
            // Limpiar los controles existente
            flp_CambioDatos.Controls.Clear();
            // Crear una etiqueta para mostrar "Correo actual"
            Label lblCorreoActual = new Label { Text = "Correo actual:" };
            lblCorreoActual.Font = new Font("Arial", 14, FontStyle.Bold);
            lblCorreoActual.Location = new Point(10, 10); // Ajustar posición
            lblCorreoActual.Size = new Size(200, 30);

            // Crear un TextBox para ingresar el correo actual
            TextBox txtCorreoActual = new TextBox();
            txtCorreoActual.Font = new Font("Arial", 14, FontStyle.Bold);
            txtCorreoActual.Size = new Size(200, 30); // Ajustar tamaño
            txtCorreoActual.Location = new Point(10, 40); // Ajustar posición
            // Crear una etiqueta para mostrar "Nuevo correo"
            Label lblNuevoCorreo = new Label { Text = "Nuevo correo:" };
            lblNuevoCorreo.Font = new Font("Arial", 14, FontStyle.Bold);
            lblNuevoCorreo.Location = new Point(15, 15); // Ajustar posición
            lblNuevoCorreo.Size = new Size(200, 30);
            // Crear un TextBox para ingresar el nuevo correo
            TextBox txtNuevoCorreo = new TextBox();
            txtNuevoCorreo.Font = new Font("Arial", 14, FontStyle.Bold);
            txtNuevoCorreo.Size = new Size(200, 30); // Ajustar tamaño
            txtNuevoCorreo.Location = new Point(10, 110); // Ajustar posición
            // Crear un botón para guardar el cambio de correo
            Button btnGuardarCorreo = new Button { Text = "Guardar" };
            btnGuardarCorreo.BackColor = Color.DarkRed; // Color de fondo
            btnGuardarCorreo.ForeColor = Color.White; // Color del texto

            btnGuardarCorreo.FlatStyle = FlatStyle.Flat;
            btnGuardarCorreo.FlatAppearance.BorderColor = Color.DarkRed; // Color del borde
            btnGuardarCorreo.FlatAppearance.BorderSize = 2; // Grosor del borde

            btnGuardarCorreo.Font = new Font("Arial", 16, FontStyle.Bold);
            btnGuardarCorreo.Size = new Size(200, 40); // Ajustar tamaño
            btnGuardarCorreo.Location = new Point(10, 200); // Ajustar posición

            // Evento que se ejecuta cuando se hace clic en el botón "Guardar"
            btnGuardarCorreo.Click += (s, e) => GuardarCambioCorreo(txtCorreoActual.Text, txtNuevoCorreo.Text);

            // Agregar los controles al FlowLayoutPanel 
            flp_CambioDatos.Controls.Add(lblCorreoActual);
            flp_CambioDatos.Controls.Add(txtCorreoActual);
            flp_CambioDatos.Controls.Add(lblNuevoCorreo);
            flp_CambioDatos.Controls.Add(txtNuevoCorreo);
            flp_CambioDatos.Controls.Add(btnGuardarCorreo);
        }

        private void MostrarFormularioCambioContrasena()
        {
            // Limpiar los controles existentes
            flp_CambioDatos.Controls.Clear();

            // Crear una etiqueta para mostrar "Contraseña actual"
            Label lblContrasenaActual = new Label { Text = "Contraseña actual:" };
            lblContrasenaActual.Font = new Font("Arial", 14, FontStyle.Bold);
            lblContrasenaActual.Location = new Point(10, 10); // Ajustar posición
            lblContrasenaActual.Size = new Size(200, 30);

            // Crear un TextBox para ingresar la contraseña actual
            TextBox txtContrasenaActual = new TextBox { PasswordChar = '\0' };
            txtContrasenaActual.Font = new Font("Arial", 14, FontStyle.Bold);
            txtContrasenaActual.Size = new Size(200, 30); // Ajustar tamaño
            txtContrasenaActual.Location = new Point(10, 40); // Ajustar posición

            // Crear una etiqueta para mostrar "Nueva contraseña"
            Label lblNuevaContrasena = new Label { Text = "Nueva contraseña:" };
            lblNuevaContrasena.Font = new Font("Arial", 14, FontStyle.Bold);
            lblNuevaContrasena.Location = new Point(15, 15); // Ajustar posición
            lblNuevaContrasena.Size = new Size(200, 30);

            // Crear un TextBox para ingresar la nueva contraseña
            TextBox txtNuevaContrasena = new TextBox { PasswordChar = '\0' };
            txtNuevaContrasena.Font = new Font("Arial", 14, FontStyle.Bold);
            txtNuevaContrasena.Size = new Size(200, 30); // Ajustar tamaño
            txtNuevaContrasena.Location = new Point(10, 110); // Ajustar posición

            // Crear un botón para guardar el cambio de contraseña
            Button btnGuardarContrasena = new Button { Text = "Guardar" };
            btnGuardarContrasena.BackColor = Color.DarkRed; // Color de fondo
            btnGuardarContrasena.ForeColor = Color.White; // Color del texto

            btnGuardarContrasena.FlatStyle = FlatStyle.Flat;
            btnGuardarContrasena.FlatAppearance.BorderColor = Color.DarkRed; // Color del borde
            btnGuardarContrasena.FlatAppearance.BorderSize = 2; // Grosor del borde

            btnGuardarContrasena.Font = new Font("Arial", 16, FontStyle.Bold);
            btnGuardarContrasena.Size = new Size(200, 40); // Ajustar tamaño
            btnGuardarContrasena.Location = new Point(10, 200); // Ajustar posición

            // Evento que se ejecuta cuando se hace clic en el botón "Guardar"
            btnGuardarContrasena.Click += (s, e) => GuardarCambioContrasena(txtContrasenaActual.Text, txtNuevaContrasena.Text);

            // Agregar los controles al FlowLayoutPanel
            flp_CambioDatos.Controls.Add(lblContrasenaActual);
            flp_CambioDatos.Controls.Add(txtContrasenaActual);
            flp_CambioDatos.Controls.Add(lblNuevaContrasena);
            flp_CambioDatos.Controls.Add(txtNuevaContrasena);
            flp_CambioDatos.Controls.Add(btnGuardarContrasena);
        }


        private void EliminarDireccion(int idDireccion)
        {
            // Cadena de conexión a la base de datos
            string conexionString = "Data Source= DESKTOP-0TP6D1B\\SQLEXPRESS ;Initial Catalog= TiendaMa;Integrated Security=True";

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                string query = "DELETE FROM DireccionesEnvio WHERE ID_DireccionEnvio = @ID_Direccion";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@ID_Direccion", idDireccion);

                    try
                    {
                        conexion.Open();
                        cmd.ExecuteNonQuery();
                        conexion.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar la dirección: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }



        private void MostrarFormularioCambioDireccion()
        {

            // Verificar si una dirección fue seleccionada
            if (lstDirecciones.SelectedItem != null)
            {
                // Limpiar cualquier control anterior en el panel
                flp_CambioDatos.Controls.Clear();

                // Obtener la dirección seleccionada
                DireccionEnvio direccionSeleccionada = (DireccionEnvio)lstDirecciones.SelectedItem;

                // Crear los controles para mostrar y modificar la dirección
                Label lblPais = new Label { Text = "País:", Font = new Font("Arial", 14, FontStyle.Bold) };
                ComboBox cmbPais = new ComboBox
                {
                    Font = new Font("Arial", 12),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Width = 200
                };
                cmbPais.Items.AddRange(new string[] { "Argentina", "Bolivia", "Brasil", "Canadá", "Chile", "Colombia", "Costa Rica", "Cuba", "Ecuador",
            "El Salvador", "Estados Unidos", "Guatemala", "Guyana", "Haití", "Honduras", "México", "Nicaragua", "Panamá",
            "Paraguay", "Perú", "República Dominicana", "Surinam", "Uruguay", "Venezuela" });

                // Seleccionar el país de la dirección seleccionada
                cmbPais.SelectedItem = direccionSeleccionada.Pais;

                Label lblDireccion = new Label { Text = "Dirección (Calle y Número):", Font = new Font("Arial", 14, FontStyle.Bold) };
                TextBox txtDireccion = new TextBox { Font = new Font("Arial", 12), Width = 200, Text = direccionSeleccionada.Direccion };

                Label lblTelefono = new Label { Text = "Teléfono:", Font = new Font("Arial", 14, FontStyle.Bold) };
                TextBox txtTelefono = new TextBox { Font = new Font("Arial", 12), Width = 200, Text = direccionSeleccionada.Telefono };

                Label lblCodigoPostal = new Label { Text = "Código Postal:", Font = new Font("Arial", 14, FontStyle.Bold) };
                TextBox txtCodigoPostal = new TextBox { Font = new Font("Arial", 12), Width = 200, Text = direccionSeleccionada.CodigoPostal };

                Button btnGuardarDireccion = new Button
                {
                    Text = "Guardar cambios",
                    BackColor = Color.DarkRed,
                    ForeColor = Color.White,
                    Font = new Font("Arial", 16, FontStyle.Bold),
                    Size = new Size(200, 50)
                };

                // Guardar los cambios realizados
                btnGuardarDireccion.Click += (s, e) => GuardarCambioDireccion(txtDireccion.Text, txtCodigoPostal.Text, cmbPais.Text, txtTelefono.Text);

                // Añadir los controles al panel
                flp_CambioDatos.Controls.Add(lblPais);
                flp_CambioDatos.Controls.Add(cmbPais);
                flp_CambioDatos.Controls.Add(lblDireccion);
                flp_CambioDatos.Controls.Add(txtDireccion);
                flp_CambioDatos.Controls.Add(lblTelefono);
                flp_CambioDatos.Controls.Add(txtTelefono);
                flp_CambioDatos.Controls.Add(lblCodigoPostal);
                flp_CambioDatos.Controls.Add(txtCodigoPostal);
                flp_CambioDatos.Controls.Add(btnGuardarDireccion);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una dirección antes de modificarla.");
            }

        }

        // Métodos para guardar los cambios en la base de datos
        private void GuardarCambioCorreo(string correoActual, string nuevoCorreo)
        {
            // Consulta SQL para actualizar el correo en la tabla de Usuarios, donde el correo actual coincide con el que se pasa como parámetro
            string query = "UPDATE Usuarios SET Email = @NuevoCorreo WHERE ID_Usuario = @IDUsuario AND Email = @CorreoActual";
            // Crear el objeto SqlCommand con la consulta SQL y la conexión a la base de datos
            using (SqlCommand cmd = new SqlCommand(query, ConexionBD.Conexion))
            {
                // Agregar los parámetros
                cmd.Parameters.AddWithValue("@NuevoCorreo", nuevoCorreo);// Nuevo correo del usuario
                cmd.Parameters.AddWithValue("@CorreoActual", correoActual);// Correo actual del usuario
                cmd.Parameters.AddWithValue("@IDUsuario", UsuarioLogueado.ID_Usuario);// ID del usuario logueado

                // Abrir la conexión a la base de datos
                ConexionBD.Conexion.Open();
                // Ejecutar la consulta y obtener el número de filas afectadas (si la consulta tuvo éxito)
                int filasAfectadas = cmd.ExecuteNonQuery();
                // Cerrar la conexión a la base de datos
                ConexionBD.Conexion.Close();

                // Verificar si se actualizó al menos una fila (lo que indica que se actualizó el correo)
                if (filasAfectadas > 0)
                    MessageBox.Show("Correo actualizado correctamente.");
                else
                    MessageBox.Show("No se pudo actualizar el correo. Verifique los datos.");
            }
        }

        private void GuardarCambioContrasena(string contrasenaActual, string nuevaContrasena)
        {
            // Consulta SQL para actualizar la contraseña en la tabla de Usuarios, donde la contraseña actual coincide con la que se pasa como parámetro
            string query = "UPDATE Usuarios SET Contraseña = @NuevaContrasena WHERE ID_Usuario = @IDUsuario AND Contraseña = @ContrasenaActual";
            using (SqlCommand cmd = new SqlCommand(query, ConexionBD.Conexion))
            {
                // Agregar los parámetros
                cmd.Parameters.AddWithValue("@NuevaContrasena", nuevaContrasena);
                cmd.Parameters.AddWithValue("@ContrasenaActual", contrasenaActual);
                cmd.Parameters.AddWithValue("@IDUsuario", UsuarioLogueado.ID_Usuario);
                // Abrir la conexión a la base de datos
                ConexionBD.Conexion.Open();
                // Ejecutar la consulta y obtener el número de filas afectadas (si la consulta tuvo éxito)
                int filasAfectadas = cmd.ExecuteNonQuery();
                // Cerrar la conexión a la base de datos
                ConexionBD.Conexion.Close();

                // Verificar si se actualizó al menos una fila (lo que indica que se actualizó la contraseña)
                if (filasAfectadas > 0)
                    MessageBox.Show("Contraseña actualizada correctamente.");
                else
                    MessageBox.Show("No se pudo actualizar la contraseña. Verifique los datos.");
            }
        }

        private void GuardarCambioDireccion(string direccion, string codigoPostal, string pais, string telefono)
        {
            string query = "UPDATE DireccionesEnvio " +
                   "SET Dirección = @Direccion, CodigoPostal = @CodigoPostal, País = @Pais, Telefono = @Telefono " +
                   "WHERE ID_Usuario = @IDUsuario AND EsPredeterminada = 1";

            using (SqlCommand cmd = new SqlCommand(query, ConexionBD.Conexion))
            {
                cmd.Parameters.AddWithValue("@IDUsuario", UsuarioLogueado.ID_Usuario);
                cmd.Parameters.AddWithValue("@Direccion", direccion);
                cmd.Parameters.AddWithValue("@CodigoPostal", codigoPostal);
                cmd.Parameters.AddWithValue("@Pais", pais);
                cmd.Parameters.AddWithValue("@Telefono", telefono);

                ConexionBD.Conexion.Open();
                int filasAfectadas = cmd.ExecuteNonQuery();//error
                ConexionBD.Conexion.Close();

                if (filasAfectadas > 0)
                    MessageBox.Show("Dirección guardada correctamente.");
                else
                    MessageBox.Show("No se pudo guardar la dirección.");
            }
        }

        private void CargarDirecciones()
        {
            string query = "SELECT ID_DireccionEnvio, Dirección, CodigoPostal, País, Telefono FROM DireccionesEnvio " +
                           "WHERE ID_Usuario = @IDUsuario";

            using (SqlCommand cmd = new SqlCommand(query, ConexionBD.Conexion))
            {
                cmd.Parameters.AddWithValue("@IDUsuario", UsuarioLogueado.ID_Usuario);

                try
                {
                    ConexionBD.Conexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    lstDirecciones.Items.Clear();
                    while (reader.Read())
                    {
                        // Crear un objeto DireccionEnvio
                        DireccionEnvio direccion = new DireccionEnvio
                        {
                            ID_Direccion = (int)reader["ID_DireccionEnvio"],
                            Direccion = (string)reader["Dirección"],
                            CodigoPostal = (string)reader["CodigoPostal"],
                            Pais = (string)reader["País"],
                            Telefono = (string)reader["Telefono"]
                        };

                        // Agregar el objeto DireccionEnvio al ListBox
                        lstDirecciones.Items.Add(direccion);
                    }
                    ConexionBD.Conexion.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar las direcciones: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lstDirecciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDirecciones.SelectedItem != null)
            {
                // Obtener el objeto DireccionEnvio seleccionado
                DireccionEnvio direccionSeleccionada = (DireccionEnvio)lstDirecciones.SelectedItem;

                // Acceder a la propiedad Direccion
                string direccion = direccionSeleccionada.Direccion;
            }
        }

        private void Cuenta_Load(object sender, EventArgs e)
        {
            lstDirecciones.Visible = true;
            CargarDirecciones();
            // Mostrar el nombre del usuario en un Label al cargar el formulario
            lbl_Saludo.Text = $"Hola, {UsuarioLogueado.Nombre}";// El nombre del usuario se obtiene de la variable 'UsuarioLogueado'
        }
        private void Menu_Inicio_Click_1(object sender, EventArgs e)
        {
            frmInicio inicio = new frmInicio();
            inicio.Show();
            this.Hide();
        }

        private void toolStripMenuItem3_Click_1(object sender, EventArgs e)
        {
            Bases bases = new Bases();
            bases.Show();
            this.Hide();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Corrector corrector = new Corrector();
            corrector.Show();
            this.Hide();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Iluminadores iluminadores = new Iluminadores();
            iluminadores.Show();
            this.Hide();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Labiales labiales = new Labiales();
            labiales.Show();
            this.Hide();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            Mascaras mascaras = new Mascaras();
            mascaras.Show();
            this.Hide();
        }

        private void toolStripMenuItem8_Click_1(object sender, EventArgs e)
        {
            Primers primers = new Primers();
            primers.Show();
            this.Hide();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            Rubores raubores = new Rubores();
            raubores.Show();
            this.Hide();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            SombrasT sombrasT = new SombrasT();
            sombrasT.Show();
            this.Hide();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            Sombras_Ojos sombras_Ojos = new Sombras_Ojos();
            sombras_Ojos.Show();
            this.Hide();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            Sombras_Cejas sombras_Cejas = new Sombras_Cejas();
            sombras_Cejas.Show();
            this.Hide();
        }

        private void Menu_Carrito_Click_1(object sender, EventArgs e)
        {
            Carrito carrito = new Carrito();
            carrito.Show();
            this.Hide();
        }

        private void Menu_Cuenta_Click(object sender, EventArgs e)
        {
            Cuenta cuenta = new Cuenta();
            cuenta.Show();
            this.Hide();
        }

        private void btn_EliminarDireccion_Click(object sender, EventArgs e)
        {
            // Verifica si hay una dirección seleccionada
            if (lstDirecciones.SelectedItem != null)
            {
                var direccionSeleccionada = (DireccionEnvio)lstDirecciones.SelectedItem;

                // Muestra un mensaje de confirmación
                DialogResult resultado = MessageBox.Show(
                    $"¿Estás seguro de que deseas eliminar la dirección '{direccionSeleccionada.Direccion}'?",
                    "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    // Eliminar la dirección de la base de datos
                    EliminarDireccion(direccionSeleccionada.ID_Direccion);

                    // Eliminar la dirección del ListBox
                    lstDirecciones.Items.Remove(direccionSeleccionada);

                    // Mostrar mensaje de éxito
                    MessageBox.Show("Dirección eliminada correctamente.", "Eliminación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Por favor selecciona una dirección para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
