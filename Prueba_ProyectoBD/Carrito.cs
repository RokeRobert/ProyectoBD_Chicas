using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prueba_ProyectoBD
{
    public partial class Carrito : Form
    {

        private decimal totalCalculado = 0;
        public Carrito()
        {
            InitializeComponent();
            // Configurar el autocompletado del TextBox txt_Bus
            txt_Bus.AutoCompleteMode = AutoCompleteMode.Suggest;
            txt_Bus.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_Bus.AutoCompleteCustomSource.AddRange(new string[] { "Bases", "Correctores", "Iluminadores", "Labiales", "Mascaras", "Primers", "Rubores", "Sombras", "Sombras de ojos", "Sombras de cejas" });

        }

        // Método que carga los productos en el carrito
        private void CargarCarrito()
        {

            // Limpiar el FlowLayoutPanel antes de cargar productos
            flp_Carrito.Controls.Clear();

            // Consulta SQL para obtener los productos en el carrito del usuario logueado
            string query = "SELECT P.Imagen, P.Nombre, P.Marca, P.Stock, P.Precio, C.Cantidad, C.ID_Producto " +
                           "FROM Carrito C " +
                           "INNER JOIN Productos P ON C.ID_Producto = P.ID_Producto " +
                           "WHERE C.ID_Usuario = @ID_Usuario";

            using (SqlCommand cmd = new SqlCommand(query, ConexionBD.Conexion))
            {
                // Asigna el ID del usuario logueado como parámetro para la consulta
                cmd.Parameters.AddWithValue("@ID_Usuario", UsuarioLogueado.ID_Usuario);

                // Abre la conexión a la base de datos
                ConexionBD.Conexion.Open();
                SqlDataReader lector = cmd.ExecuteReader();

                while (lector.Read())// Itera sobre cada registro devuelto por la consulta
                {
                    byte[] imagen = (byte[])lector["Imagen"];               // Obtiene la imagen del producto o null si no existe
                    string nombre = lector["Nombre"].ToString();            // Obtiene el nombre del producto
                    string marca = lector["Marca"].ToString();              // Obtiene la marca del producto
                    int stock = Convert.ToInt32(lector["Stock"]);           // Obtiene el stock del producto
                    decimal precio = Convert.ToDecimal(lector["Precio"]);   // Obtiene el precio del producto
                    int cantidad = Convert.ToInt32(lector["Cantidad"]);     // Obtiene la cantidad del producto en el carrito
                    int idProducto = Convert.ToInt32(lector["ID_Producto"]);// Obtiene el ID del producto

                    // Agrega el producto al carrito 
                    AgregarProductoAlCarrito(imagen, nombre, marca, stock, precio, cantidad, idProducto);
                }
                ConexionBD.Conexion.Close();
            }
        }

        // Método para agregar un producto visual al carrito
        public void AgregarProductoAlCarrito(byte[] imagen, string nombre, string marca, int stock, decimal precio, int cantidad, int idProducto)
        {
            // Crear un panel para cada producto en el carrito
            Panel panelProducto = new Panel
            {
                Size = new Size(350, 250),
                BorderStyle = BorderStyle.None,
                BackColor = Color.LavenderBlush
                
            };

            // Imagen del producto
            PictureBox pictureBox = new PictureBox
            {
                Size = new Size(100, 220),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            using (MemoryStream ms = new MemoryStream(imagen))
            {
                pictureBox.Image = Image.FromStream(ms);// Carga la imagen desde el array de bytes
            }
            panelProducto.Controls.Add(pictureBox);

            // Nombre del producto
            Label labelNombre = new Label
            {
                Text = nombre,
                Location = new Point(120, 10),
                Size = new Size(200, 20),
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            panelProducto.Controls.Add(labelNombre);

            // Marca del producto
            Label labelMarca = new Label
            {
                Text = $"Marca: {marca}",
                Location = new Point(120, 40),
                Size = new Size(200, 20),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            panelProducto.Controls.Add(labelMarca);

            // Stock del producto
            Label labelStock = new Label
            {
                Text = $"Stock: {stock}",
                Location = new Point(120, 70),
                Size = new Size(200, 20),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            panelProducto.Controls.Add(labelStock);

            // Precio del producto
            Label labelPrecio = new Label
            {
                Text = $"Precio: ${precio:F2}",
                Location = new Point(120, 100),
                Size = new Size(200, 20),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.DarkGreen
            };
            panelProducto.Controls.Add(labelPrecio);

            // Cantidad del producto
            Label labelCantidad = new Label
            {
                Text = $"Cantidad: {cantidad}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(120, 130),
                Size = new Size(250, 20),
                Tag = cantidad // Guardar la cantidad en el Tag
            };
            panelProducto.Controls.Add(labelCantidad);

            // Botón para aumentar la cantidad
            Button btnAumentar = new Button
            {
                Text = " + ",
                FlatStyle = FlatStyle.Flat,
                Font = new Font(SystemFonts.DefaultFont.FontFamily, 10, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(35, 30),
                Location = new Point(120, 160),
                BackColor = Color.Brown,
                Tag = (idProducto, labelCantidad)// Almacena el ID del producto y la etiqueta de cantidad
            };
            btnAumentar.Click += (s, e) => BtnAumentar_Click(s, e);// Asigna evento para aumentar cantidad
            panelProducto.Controls.Add(btnAumentar);

            // Botón para disminuir la cantidad
            Button btnDisminuir = new Button
            {
                Text = " - ",
                ForeColor = Color.White,
                Font = new Font(SystemFonts.DefaultFont.FontFamily, 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(35,30),
                Location = new Point(160, 160),
                BackColor = Color.Brown,
                Tag = (idProducto, labelCantidad)// Almacena el ID del producto y la etiqueta de cantidad
            };
            btnDisminuir.Click += (s, e) => BtnDisminuir_Click(s, e);// Asigna evento para disminuir cantidad
            panelProducto.Controls.Add(btnDisminuir);

            // Botón para eliminar el producto
            Button btnEliminar = new Button
            {
                Text = "Eliminar",
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(SystemFonts.DefaultFont.FontFamily,12, FontStyle.Bold),
                Size = new Size(100, 30),
                Location = new Point(220, 160),
                BackColor = Color.Red,
                Tag = idProducto// Almacena el ID del producto
            };
            btnEliminar.Click += (s, e) => EliminarProducto(idProducto, panelProducto);// Asigna evento para eliminar producto
            panelProducto.Controls.Add(btnEliminar);

            // CheckBox para seleccionar el producto
            CheckBox checkBoxSeleccionar = new CheckBox
            {
                Location = new Point(120, 200),
                Text = "Seleccionar",
                Size = new Size(200, 50),
                Font = new Font(SystemFonts.DefaultFont.FontFamily, 11, FontStyle.Bold),
                Tag = precio // Almacena el precio del producto
            };
            checkBoxSeleccionar.CheckedChanged += (s, e) => CalcularTotal();// Evento para calcular el total al seleccionar
            panelProducto.Controls.Add(checkBoxSeleccionar);

            // Agregar el panel al FlowLayoutPanel del carrito
            flp_Carrito.Controls.Add(panelProducto);
            /*flp_Carrito.FlowDirection = FlowDirection.TopDown; // Asegura que los paneles se coloquen hacia abajo
            flp_Carrito.AutoSize = true;
            flp_Carrito.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flp_Carrito.WrapContents = false;*/
        }

        // Evento para aumentar la cantidad
        private void BtnAumentar_Click(object sender, EventArgs e)
        {
            // Convertir el objeto sender en un botón
            Button btn = sender as Button;
            // Obtiene los valores almacenados en el atributo Tag del botón
            // Tag contiene una tupla con el ID del producto y la etiqueta de cantidad
            var (idProducto, labelCantidad) = ((int, Label))btn.Tag;
            // Llama al método para actualizar la cantidad en el carrito con un incremento de 1
            ActualizarCantidad(idProducto, 1, labelCantidad);
        }

        // Evento para disminuir la cantidad
        private void BtnDisminuir_Click(object sender, EventArgs e)
        {
            // Convertir el objeto sender en un botón
            Button btn = sender as Button;

            // Obtener los valores almacenados en el atributo Tag del botón
            // Tag contiene una tupla con el ID del producto y la etiqueta de cantidad
            var (idProducto, labelCantidad) = ((int, Label))btn.Tag;
            // Llama al método para actualizar la cantidad en el carrito con un decremento de 1
            ActualizarCantidad(idProducto, -1, labelCantidad);
        }

        // Método para actualizar la cantidad del producto en la base de datos
        private void ActualizarCantidad(int idProducto, int cambio, Label labelCantidad)
        {
            // Obtener la cantidad actual desde el atributo Tag del Label
            int cantidadActual = Convert.ToInt32(labelCantidad.Tag);
            // Calcular la nueva cantidad con el cambio proporcionado
            int nuevaCantidad = cantidadActual + cambio;

            // Validar que la nueva cantidad no sea menor a 1
            if (nuevaCantidad < 1)
            {
                // Mostrar un mensaje de advertencia si la cantidad es inválida
                MessageBox.Show("La cantidad mínima es 1.", "Cantidad inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Query para actualizar la cantidad del producto en la tabla 'Carrito'
            string query = "UPDATE Carrito SET Cantidad = Cantidad + @Cambio WHERE ID_Usuario = @ID_Usuario AND ID_Producto = @ID_Producto";

            // Crear un comando SQL para ejecutar la consulta
            using (SqlCommand cmd = new SqlCommand(query, ConexionBD.Conexion))
            {
                // Asigna valores a los parámetros de la consulta
                cmd.Parameters.AddWithValue("@Cambio", cambio); // Incremento o decremento en la cantidad
                cmd.Parameters.AddWithValue("@ID_Usuario", UsuarioLogueado.ID_Usuario); // ID del usuario actual
                cmd.Parameters.AddWithValue("@ID_Producto", idProducto); // ID del producto
                // Abrir la conexión a la base de datos
                ConexionBD.Conexion.Open();
                // Ejecutar la consulta
                cmd.ExecuteNonQuery();
                // Cerrar la conexión
                ConexionBD.Conexion.Close();
            }

            // Actualizar visualmente la cantidad
            labelCantidad.Tag = nuevaCantidad;
            labelCantidad.Text = $"Cantidad: {nuevaCantidad}";

            // Recalcular el total
            CalcularTotal();
        }

        // Método para eliminar un producto del carrito
        private void EliminarProducto(int idProducto, Panel panelProducto)
        {
            // Query para eliminar el producto del carrito en la base de datos
            string query = "DELETE FROM Carrito WHERE ID_Usuario = @ID_Usuario AND ID_Producto = @ID_Producto";
            // Crear un comando SQL para ejecutar la consulta
            using (SqlCommand cmd = new SqlCommand(query, ConexionBD.Conexion))
            {
                // Asigna los valores de los parámetros a la consulta
                cmd.Parameters.AddWithValue("@ID_Usuario", UsuarioLogueado.ID_Usuario);// ID del usuario actual
                cmd.Parameters.AddWithValue("@ID_Producto", idProducto);// ID del producto a eliminar

                ConexionBD.Conexion.Open();// Abrir la conexión a la base de datos
                cmd.ExecuteNonQuery();// Ejecutar la consulta para eliminar el producto
                ConexionBD.Conexion.Close();// Cerrar la conexión
            }

            // Eliminar el panel visualmente
            flp_Carrito.Controls.Remove(panelProducto);

            // Liberar los recursos del panel eliminado
            panelProducto.Dispose();

            // Recalcular el total
            CalcularTotal();
        }

        // Método para calcular el total de los productos seleccionados
        private void CalcularTotal()
        {
            decimal total = 0;

            // Recorremos todos los controles dentro del FlowLayoutPanel
            foreach (Control control in flp_Carrito.Controls)
            {
                if (control is Panel panel) // Verificamos si el control es un Panel
                {
                    // Obtener el CheckBox para saber si el producto está seleccionado
                    var checkBox = panel.Controls.OfType<CheckBox>().FirstOrDefault();
                    if (checkBox != null && checkBox.Checked)// Si el producto está seleccionado
                    {
                        // Obtener el Label de Precio
                        var precioLabel = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Text.StartsWith("Precio:"));
                        if (precioLabel != null)// Si se encuentra el Label de precio
                        {
                            // Extraemos el precio
                            decimal precio = Convert.ToDecimal(precioLabel.Text.Replace("Precio: $", "").Trim());

                            // Obtener el Label de Cantidad
                            var cantidadLabel = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Text.StartsWith("Cantidad:"));
                            if (cantidadLabel != null)// Si se encuentra el Label de cantidad
                            {
                                // Extraemos la cantidad
                                int cantidad = Convert.ToInt32(cantidadLabel.Text.Replace("Cantidad: ", "").Trim());

                                // Calcular el total para este producto y agregarlo al total general
                                total += precio * cantidad;
                            }
                        }
                    }
                }
            }
            // Actualizar la variable global con el total calculado
            totalCalculado = total;

            // Actualizar el Label del total con el valor calculado
            lbl_Total.Text = $"Total: ${total:F2}";
        }

        private void Carrito_Load(object sender, EventArgs e)
        {
            // Cargar los productos al abrir el formulario
            CargarCarrito();
        }

        private void btn_Pago_Click(object sender, EventArgs e)
        {
            // Variable para verificar si hay algún producto seleccionado
            bool productoSeleccionado = false;

            // Recorrer los controles dentro del FlowLayoutPanel
            foreach (Control panel in flp_Carrito.Controls)
            {
                // Asegurarse de que sea un Panel 
                if (panel is Panel panelProducto)
                {
                    // Buscar el CheckBox dentro del Panel
                    foreach (Control control in panelProducto.Controls)
                    {
                        if (control is CheckBox checkBoxSeleccionar && checkBoxSeleccionar.Checked)
                        {
                            productoSeleccionado = true;// Marcar como seleccionado si el CheckBox está marcado
                            break; // Salimos del bucle si encontramos un producto seleccionado
                        }
                    }

                    // Salimos del bucle principal si ya hay un producto seleccionado
                    if (productoSeleccionado) break;
                }
            }

            // Validar que el total sea mayor a 0
            if (totalCalculado <= 0)
            {
                // Mostrar un mensaje de advertencia si el total es 0 o menor
                MessageBox.Show("El total no puede ser $0. Verifica tu carrito y selecciona un producto.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Salir del método sin continuar
            }

            // Si hay productos seleccionados, proceder con el flujo de pago
            btn_GNuevaDireccion pedido = new btn_GNuevaDireccion(totalCalculado); // Pasar el total calculado al formulario Pedido
            pedido.Show();// Mostrar el formulario de pago
            this.Hide();// Ocultar el formulario actual (carrito de compras)
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip2_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {

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

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
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

        private void Menu_Carrito_Click(object sender, EventArgs e)
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

        private void Menu_Inicio_Click(object sender, EventArgs e)
        {
            frmInicio inicio = new frmInicio();
            inicio.Show();
            this.Hide();
        }

        private void btn_Bus_Click(object sender, EventArgs e)
        {
            string categoria = txt_Bus.Text.Trim();  // Obtener la categoría ingresada

            // Verificar la categoría y abrir el formulario correspondiente
            switch (categoria.ToLower())
            {
                case "labiales":

                    Labiales labialesForm = new Labiales();
                    labialesForm.Show();  // Mostrar el formulario
                    break;

                case "bases":

                    Bases bases = new Bases();
                    bases.Show();  // Mostrar el formulario
                    break;

                case "corrector":

                    Corrector corrector = new Corrector();
                    corrector.Show();  // Mostrar el formulario
                    break;

                case "iluminadores":
                    // Crear una instancia del formulario Maquillaje
                    Iluminadores iluminadores = new Iluminadores();
                    iluminadores.Show();  // Mostrar el formulario
                    break;

                case "mascaras":
                    // Crear una instancia del formulario Maquillaje
                    Mascaras masc = new Mascaras();
                    masc.Show();  // Mostrar el formulario
                    break;

                case "primers":
                    // Crear una instancia del formulario Maquillaje
                    Primers primers = new Primers();
                    primers.Show();  // Mostrar el formulario
                    break;

                case "rubores":
                    // Crear una instancia del formulario Maquillaje
                    Rubores rubores = new Rubores();
                    rubores.Show();  // Mostrar el formulario
                    break;

                case "sombras de cejas":
                    // Crear una instancia del formulario Maquillaje
                    Sombras_Cejas Sc = new Sombras_Cejas();
                    Sc.Show();  // Mostrar el formulario
                    break;

                case "sombras de ojos":
                    // Crear una instancia del formulario Maquillaje
                    Sombras_Ojos So = new Sombras_Ojos();
                    So.Show();  // Mostrar el formulario
                    break;

                case "sombras":
                    // Crear una instancia del formulario Maquillaje
                    SombrasT ST = new SombrasT();
                    ST.Show();  // Mostrar el formulario
                    break;

                default:
                    MessageBox.Show("Categoría no encontrada. Por favor, intente nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            // Cerrar el formulario de inicio
            this.Close();
        }
    }
}
