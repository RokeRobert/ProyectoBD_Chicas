using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prueba_ProyectoBD
{
    public partial class btn_GNuevaDireccion : Form
    {
        // Variable para almacenar el total del pedido
        private decimal totalPedido;

        // Lista de países de América que se usarán en el ComboBox de países
        private List<string> paisesAmerica = new List<string>
        {
        "Argentina", "Bolivia", "Brasil", "Canadá", "Chile", "Colombia", "Costa Rica", "Cuba", "Ecuador",
        "El Salvador", "Estados Unidos", "Guatemala", "Guyana", "Haití", "Honduras", "México", "Nicaragua", "Panamá",
        "Paraguay", "Perú", "República Dominicana", "Surinam", "Uruguay", "Venezuela"
        };
        public btn_GNuevaDireccion(decimal total)
        {
            InitializeComponent();
            totalPedido = total;    // Asigna el total del pedido al campo totalPedido

            // Carga la lista de países en el ComboBox
            CargarPaises();
            // Carga los métodos de pago en el ComboBox
            CargarMetodosPago();


        }

        // Cargar los países en el ComboBox
        private void CargarPaises()
        {
            cmb_Paises.DataSource = paisesAmerica;
        }

        // Cargar métodos de pago en el ComboBox de Método de Pago
        private void CargarMetodosPago()
        {
            List<MetodoPago> metodosPago = new List<MetodoPago>
            {
                new MetodoPago { ID = 1, Nombre = "Tarjeta de Crédito" },
                new MetodoPago { ID = 2, Nombre = "Tarjeta de Débito" },
                new MetodoPago { ID = 3, Nombre = "Tarjeta de Regalo" },
                new MetodoPago { ID = 4, Nombre = "Transferencia Bancaria" }
            };

            // Asignar la lista de métodos de pago al ComboBox
            cmb_MetodosPago.DataSource = metodosPago;
            cmb_MetodosPago.DisplayMember = "Nombre"; // Mostrar el nombre en el ComboBox
            cmb_MetodosPago.ValueMember = "ID"; // Almacenar el ID en el ComboBox (en el fondo)
        }

        // Evento cuando se selecciona un método de pago
        private void cmb_MetodosPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Limpia el FlowLayoutPanel antes de agregar nuevos botones
            flp_Metodos.Controls.Clear();

        }

        // Guardar la dirección de envío en la base de datos
        private int GuardarDireccionEnvio(bool esPredeterminada)
        {
            // Cadena de conexión a la base de datos
            string conexionString = "Data Source= DESKTOP-0TP6D1B\\SQLEXPRESS ;Initial Catalog= TiendaMa ;Integrated Security=True";
            int idDireccion = 0;// Variable para almacenar el ID de la nueva dirección

            // Crear una conexión a la base de datos usando la cadena de conexión
            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                // Abrir la conexión a la base de datos
                conexion.Open();

                // Consulta para insertar una nueva dirección en la base de datos
                string consulta = @"
                INSERT INTO DireccionesEnvio (ID_Usuario, Telefono, Dirección, CodigoPostal, País)
                VALUES (@ID_Usuario, @Telefono, @Direccion, @CodigoPostal, @Pais);
                SELECT SCOPE_IDENTITY(); "; // Obtiene el ID generado para la nueva dirección

                // Crear un comando para ejecutar la consulta de inserción
                SqlCommand comando = new SqlCommand(consulta, conexion);

                // Parámetros
                comando.Parameters.AddWithValue("@ID_Usuario", UsuarioLogueado.ID_Usuario); // ID del usuario actual
                comando.Parameters.AddWithValue("@Telefono", txt_Telefono.Text); // Teléfono de la dirección
                comando.Parameters.AddWithValue("@Direccion", txt_Direccion.Text); // Dirección de envío
                comando.Parameters.AddWithValue("@CodigoPostal", txt_CP.Text); // Código postal de la dirección
                comando.Parameters.AddWithValue("@Pais", cmb_Paises.SelectedItem?.ToString()); // País seleccionado
                
                // Ejecuta la consulta y obtener el ID de la nueva dirección generada
                idDireccion = Convert.ToInt32(comando.ExecuteScalar()); // Obtiene el ID generado para la dirección insertada
                Console.WriteLine($"Dirección guardada");
            }
            return idDireccion;// Retorna el ID de la nueva dirección
        }


        private void btn_FinalizarPago_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener la dirección seleccionada desde el ComboBox
                int idDireccionEnvio = (int)cmb_Direcciones.SelectedValue;

                // Guarda el pedido en la base de datos y obtiene el ID del pedido
                int idPedido = GuardarPedido(idDireccionEnvio);

                // Obtiene el ID del método de pago seleccionado
                int idMetodoPago = (int)cmb_MetodosPago.SelectedValue;

                // Estado de pago es "Pendiente" inicialmente
                int idEstadoPago = 1;
                // Guarda el pago asociado al pedido
                GuardarPago(idPedido, idMetodoPago, idEstadoPago, totalPedido);

                // Muestra un mensaje indicando que el pedido fue finalizado correctamente
                MessageBox.Show("Pedido finalizado correctamente. ¡Gracias por tu compra!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Si ocurre algún error durante el proceso, muestra un mensaje con el detalle del error
                MessageBox.Show($"Ocurrió un error al finalizar el pedido: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Método para guardar el pedido en la base de datos
        private int GuardarPedido(int idDireccionEnvio)
        {
            // Cadena de conexión a la base de datos
            string conexionString = "Data Source= DESKTOP-0TP6D1B\\SQLEXPRESS ;Initial Catalog= TiendaMa ;Integrated Security=True";

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                // Consulta SQL para insertar un nuevo pedido
                string consulta = @"
                INSERT INTO Pedidos (ID_Usuario, ID_DireccionEnvio, Total, ID_Estado)
                VALUES (@ID_Usuario, @DireccionEnvio, @TotalCompra, @ID_Estado);
                SELECT SCOPE_IDENTITY();"; // Devuelve el ID del pedido recién insertado

                SqlCommand comando = new SqlCommand(consulta, conexion);

                // Parámetros
                comando.Parameters.AddWithValue("@ID_Usuario", UsuarioLogueado.ID_Usuario); // ID del Usuario actual
                comando.Parameters.AddWithValue("@DireccionEnvio", idDireccionEnvio); // ID de la Dirección de envío
                comando.Parameters.AddWithValue("@TotalCompra", totalPedido);//Total de la compra
                comando.Parameters.AddWithValue("@ID_Estado", 1); // Estado del pedido "Pendiente"

                conexion.Open();
                return Convert.ToInt32(comando.ExecuteScalar()); // Devuelve el ID del nuevo pedido insertado
            }
        }

        private void GuardarPago(int idPedido, int idMetodoPago, int idEstadoPago, decimal montoPagado)
        {
            // Cadena de conexión a la base de datos
            string conexionString = "Data Source= DESKTOP-0TP6D1B\\SQLEXPRESS ;Initial Catalog= TiendaMa ;Integrated Security=True";

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                // Consulta SQL para insertar un nuevo registro en la tabla de pagos
                string consulta = @"
                INSERT INTO Pagos (ID_Pedido, ID_MetodoPago, ID_EstadoPago, MontoPagado)
                VALUES (@ID_Pedido, @ID_MetodoPago, @ID_EstadoPago, @MontoPagado)";

                SqlCommand comando = new SqlCommand(consulta, conexion);

                // Parámetros
                comando.Parameters.AddWithValue("@ID_Pedido", idPedido);// ID del pedido relacionado con el pago
                comando.Parameters.AddWithValue("@ID_MetodoPago", idMetodoPago);// Método de pago utilizado
                comando.Parameters.AddWithValue("@ID_EstadoPago", idEstadoPago);// Estado actual del pago
                comando.Parameters.AddWithValue("@MontoPagado", montoPagado);// Monto total pagado
                //Abre conexión y ejecuta la consulta
                conexion.Open();
                comando.ExecuteNonQuery();// Inserta el registro
            }
        }

        private bool esPrimeraCompra = true;
        private void btn_GNuevaDirec_Click(object sender, EventArgs e)
        {
            try
            {
                // Validación básica de campos (puedes agregar más validaciones si es necesario)
                if (string.IsNullOrWhiteSpace(txt_Telefono.Text) ||
                    string.IsNullOrWhiteSpace(txt_Direccion.Text) ||
                    string.IsNullOrWhiteSpace(txt_CP.Text) ||
                    cmb_Paises.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, completa todos los campos de la dirección.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Guarda la nueva dirección como no predeterminada si ya existe otra
                int idDireccionEnvio = GuardarDireccionEnvio(false);

                if (idDireccionEnvio > 0)
                {
                    MessageBox.Show("Dirección guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Si es la primera compra, ocultar los campos para nueva dirección y mostrar el ComboBox con la nueva dirección
                    if (esPrimeraCompra)
                    {
                        esPrimeraCompra = false;
                        panel2.Visible = false; // Ocultar los campos de nueva dirección
                        panel1.Visible = true;  // Mostrar el ComboBox para seleccionar una dirección
                        CargarDireccionesExistentes(); // Cargar las direcciones existentes en el ComboBox
                    }

                }
                else
                {
                    MessageBox.Show("Hubo un error al guardar la dirección.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al guardar la dirección: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para cargar las direcciones existentes del usuario en el ComboBox
        private void CargarDireccionesExistentes()
        {
            string conexionString = "Data Source= DESKTOP-0TP6D1B\\SQLEXPRESS ;Initial Catalog= TiendaMa ;Integrated Security=True";
            List<DireccionEnvio> direcciones = new List<DireccionEnvio>();

            // Crear una conexión a la base de datos usando la cadena de conexión
            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                // Consulta para obtener las direcciones de envío del usuario
                string consulta = "SELECT ID_DireccionEnvio, Dirección, CodigoPostal, País FROM DireccionesEnvio WHERE ID_Usuario = @ID_Usuario";

                SqlCommand comando = new SqlCommand(consulta, conexion);
                comando.Parameters.AddWithValue("@ID_Usuario", UsuarioLogueado.ID_Usuario); // ID del usuario actual

                conexion.Open();
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    // Agregar cada dirección a la lista
                    DireccionEnvio direccion = new DireccionEnvio
                    {
                        ID_Direccion = reader.GetInt32(0),
                        Direccion = reader.GetString(1),
                        CodigoPostal = reader.GetString(2),
                        Pais = reader.GetString(3)
                    };
                    direcciones.Add(direccion);
                }
            }

            // Asignar las direcciones al ComboBox
            cmb_Direcciones.DataSource = direcciones;
            cmb_Direcciones.DisplayMember = "Direccion";  // Mostrar la dirección en el ComboBox
            cmb_Direcciones.ValueMember = "ID_Direccion";           // El ID de la dirección estará en el ValueMember
        }

        private void cmb_Direcciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cuando el usuario selecciona una dirección, puedes hacer algo con la dirección seleccionada, por ejemplo:
            string direccionSeleccionada = cmb_Direcciones.SelectedItem?.ToString();
            Console.WriteLine($"Dirección seleccionada: {direccionSeleccionada}");
        }

        private void Pedido_Load(object sender, EventArgs e)
        {
            // Muestra el total del pedido formateado con dos decimales
            lbl_Total.Text = $"Total: ${totalPedido:F2}";
            panel2.Visible = false;
            CargarDireccionesExistentes();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            panel2.Visible = true;   // Mostrar el panel para agregar una nueva dirección
            // Limpiar los campos
            txt_Telefono.Text = string.Empty;
            txt_Direccion.Text = string.Empty;
            txt_CP.Text = string.Empty;
            cmb_Paises.SelectedIndex = -1;
        }

        private void btn_MetodoPago_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

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

        private void toolStripMenuItem4_Click_1(object sender, EventArgs e)
        {
            Corrector corrector = new Corrector();
            corrector.Show();
            this.Hide();
        }

        private void toolStripMenuItem5_Click_1(object sender, EventArgs e)
        {
            Iluminadores iluminadores = new Iluminadores();
            iluminadores.Show();
            this.Hide();
        }

        private void toolStripMenuItem6_Click_1(object sender, EventArgs e)
        {
            Labiales labiales = new Labiales();
            labiales.Show();
            this.Hide();
        }

        private void toolStripMenuItem7_Click_1(object sender, EventArgs e)
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

        private void toolStripMenuItem9_Click_1(object sender, EventArgs e)
        {
            Rubores raubores = new Rubores();
            raubores.Show();
            this.Hide();
        }

        private void toolStripMenuItem10_Click_1(object sender, EventArgs e)
        {
            SombrasT sombrasT = new SombrasT();
            sombrasT.Show();
            this.Hide();
        }

        private void toolStripMenuItem11_Click_1(object sender, EventArgs e)
        {
            Sombras_Ojos sombras_Ojos = new Sombras_Ojos();
            sombras_Ojos.Show();
            this.Hide();
            this.Hide();
        }

        private void toolStripMenuItem12_Click_1(object sender, EventArgs e)
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

        private void Menu_Cuenta_Click_1(object sender, EventArgs e)
        {
            Cuenta cuenta = new Cuenta();
            cuenta.Show();
            this.Hide();
        }

        private void btn_Bus_Click(object sender, EventArgs e)
        {
            string categoria = txt_Bus.Text.Trim();  // Obtiene la categoría ingresada

            // Verifica la categoría y abre el formulario correspondiente
            switch (categoria.ToLower())
            {
                case "labiales":

                    Labiales labialesForm = new Labiales();
                    labialesForm.Show();  // Muestra el formulario
                    break;

                case "bases":

                    Bases bases = new Bases();
                    bases.Show();  // Muestra el formulario
                    break;

                case "corrector":

                    Corrector corrector = new Corrector();
                    corrector.Show();  // Muestra el formulario
                    break;

                case "iluminadores":

                    Iluminadores iluminadores = new Iluminadores();
                    iluminadores.Show();  // Muestra el formulario
                    break;

                case "mascaras":

                    Mascaras masc = new Mascaras();
                    masc.Show();  // Muestra el formulario
                    break;

                case "primers":

                    Primers primers = new Primers();
                    primers.Show();  // Muestra el formulario
                    break;

                case "rubores":

                    Rubores rubores = new Rubores();
                    rubores.Show();  // Muestra el formulario
                    break;

                case "sombras de cejas":

                    Sombras_Cejas Sc = new Sombras_Cejas();
                    Sc.Show();  // Muestra el formulario
                    break;

                case "sombras de ojos":

                    Sombras_Ojos So = new Sombras_Ojos();
                    So.Show();  // Muestra el formulario
                    break;

                case "sombras":

                    SombrasT ST = new SombrasT();
                    ST.Show();  // Muestra el formulario
                    break;

                default:
                    MessageBox.Show("Categoría no encontrada. Por favor, intente nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            // Cierra el formulario de inicio
            this.Close();
        }
    }
}

