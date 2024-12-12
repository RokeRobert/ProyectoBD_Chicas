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
    public partial class frmInicio : Form
    {
        public frmInicio()
        {
            InitializeComponent();
            // Configurar el autocompletado del TextBox txt_Bus
            txt_Bus.AutoCompleteMode = AutoCompleteMode.Suggest;
            txt_Bus.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_Bus.AutoCompleteCustomSource.AddRange(new string[] { "Bases", "Correctores", "Iluminadores", "Labiales", "Mascaras", "Primers", "Rubores", "Sombras", "Sombras de ojos", "Sombras de cejas" });
        
        }

        // Al hacer clic en el elemento "Bases" del menú, se crea una nueva instancia del formulario "Bases",
        // se muestra ese formulario y se oculta el formulario actual.
        private void labialesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bases bases = new Bases();
            bases.Show();
            this.Hide();
        }

        private void masToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mascaras mascaras = new Mascaras();
            mascaras.Show();
            this.Hide();
        }

        private void frmInicio_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private void CargarProductos()
        { // Limpia el FlowLayoutPanel antes de agregar productos
            flp_TP.Controls.Clear();

            // Conexión a la base de datos
            string ConexionString = "Data Source= DESKTOP-0TP6D1B\\SQLEXPRESS ;Initial Catalog= TiendaMa;Integrated Security=True";
            using (SqlConnection Conexion = new SqlConnection(ConexionString))
            {
                // Consulta SQL para obtener productos aleatorios(ORDER BY NEWID()) de diferentes categorías.
                // Cada subconsulta selecciona los primeros 3 productos aleatorios (TOP 3) de una categoría específica (ID_Categoria),
                // y se combinan los resultados usando UNION ALL.

                string consulta = @"
                SELECT ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria
                FROM (
                    SELECT TOP 3 ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria 
                    FROM Productos WHERE ID_Categoria = 1 ORDER BY NEWID()
                ) AS Categoria1
                UNION ALL
                SELECT ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria
                FROM (
                    SELECT TOP 3 ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria 
                    FROM Productos WHERE ID_Categoria = 5 ORDER BY NEWID()
                ) AS Categoria2
                UNION ALL
                SELECT ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria
                FROM (
                    SELECT TOP 3 ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria 
                    FROM Productos WHERE ID_Categoria = 4 ORDER BY NEWID()
                ) AS Categoria3
                UNION ALL
                SELECT ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria
                FROM (
                    SELECT TOP 3 ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria 
                    FROM Productos WHERE ID_Categoria = 2 ORDER BY NEWID()
                ) AS Categoria4
                UNION ALL
                SELECT ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria
                FROM (
                    SELECT TOP 3 ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria 
                    FROM Productos WHERE ID_Categoria = 8 ORDER BY NEWID()
                ) AS Categoria5
                UNION ALL
                SELECT ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria
                FROM (
                    SELECT TOP 3 ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria 
                    FROM Productos WHERE ID_Categoria = 3 ORDER BY NEWID()
                ) AS Categoria6
                UNION ALL
                SELECT ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria
                FROM (
                    SELECT TOP 3 ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria 
                    FROM Productos WHERE ID_Categoria = 9 ORDER BY NEWID()
                ) AS Categoria7
                UNION ALL
                SELECT ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria
                FROM (
                    SELECT TOP 3 ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria 
                    FROM Productos WHERE ID_Categoria = 7 ORDER BY NEWID()
                ) AS Categoria8
                UNION ALL
                SELECT ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria
                FROM (
                    SELECT TOP 3 ID_Producto, Imagen, Nombre, Marca, Stock, Precio, ID_Categoria 
                    FROM Productos WHERE ID_Categoria = 6 ORDER BY NEWID()
                ) AS Categoria9;";

                // Se crea un comando SQL con la consulta y la conexión establecida.
                SqlCommand comando = new SqlCommand(consulta, Conexion);
                // Se abre la conexión a la base de datos.
                Conexion.Open();
                // Se ejecuta la consulta y se obtiene un objeto SqlDataReader para leer los resultados.
                SqlDataReader lector = comando.ExecuteReader();

                while (lector.Read())// Iterar sobre los resultados de la consulta.
                {
                    // Crea un panel para cada producto
                    Panel panelProducto = new Panel
                    {
                        Size = new Size(200, 325),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.White
                    };

                    // Muestra la imagen
                    PictureBox pictureBox = new PictureBox
                    {
                        Size = new Size(180, 180),
                        Location = new Point(10, 10),
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    // Convierte los bytes de la imagen a una imagen válida
                    byte[] imagenBytes = (byte[])lector["Imagen"];
                    using (MemoryStream ms = new MemoryStream(imagenBytes))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                    panelProducto.Controls.Add(pictureBox);

                    // Etiquetas con información del producto
                    // Muestra el nombre
                    Label labelNombre = new Label
                    {
                        Text = lector["Nombre"].ToString(),
                        Location = new Point(10, 200),
                        Size = new Size(180, 20),
                        ForeColor = Color.DarkBlue,   // Texto en azul oscuro
                        Font = new Font("Verdana", 12, FontStyle.Regular)
                    };
                    panelProducto.Controls.Add(labelNombre);

                    // Muestra la marca
                    Label labelMarca = new Label
                    {
                        Text = $"Marca: {lector["Marca"]}",
                        Location = new Point(10, 240),
                        Size = new Size(180, 20),
                        ForeColor = Color.DarkBlue,   // Texto en azul oscuro
                        Font = new Font("Verdana", 12, FontStyle.Regular)
                    };
                    panelProducto.Controls.Add(labelMarca);


                    // Muestra el precio
                    Label labelPrecio = new Label
                    {
                        Text = $"Precio: ${lector["Precio"]}",
                        Location = new Point(10, 220),
                        Size = new Size(180, 20),
                        ForeColor = Color.DarkBlue,   // Texto en azul oscuro
                        Font = new Font("Verdana", 12, FontStyle.Regular)
                    };
                    panelProducto.Controls.Add(labelPrecio);

                    // Muestra el Stock
                    Label labelStock = new Label
                    {
                        Text = $"Stock: ${lector["Stock"]}",
                        Location = new Point(10, 220),
                        Size = new Size(180, 20),
                        ForeColor = Color.DarkBlue,   // Texto en azul oscuro
                        Font = new Font("Verdana", 12, FontStyle.Regular)
                    };
                    panelProducto.Controls.Add(labelStock);

                    // Botón para agregar al carrito
                    Button btnAgregar = new Button
                    {
                        Text = "Agregar al carrito",
                        Location = new Point(10, 270),
                        Size = new Size(180, 35),
                        ForeColor = Color.White,      // Texto en blanco
                        BackColor = Color.DarkRed,      // Fondo verde
                        Font = new Font("Arial", 10, FontStyle.Bold),
                        Tag = lector["ID_Producto"] // Guardar el ID del producto
                    };
                    btnAgregar.Click += Btn_Agregar_Click;// Vincula evento al botón
                    panelProducto.Controls.Add(btnAgregar);

                    // Agregar el panel al FlowLayoutPanel
                    flp_TP.Controls.Add(panelProducto);
                    flp_TP.Padding = new Padding(30);  // Un poco de espacio alrededor del pane
                }
            }
        }

        private void Btn_Agregar_Click(object sender, EventArgs e)
        {
            // Obtener el botón que disparó el evento
            Button btn = sender as Button;

            // Verificar que el objeto convertido a botón no sea nulo 
            // y que la propiedad Tag del botón contenga un valor válido.
            if (btn != null && btn.Tag != null)
            {
                // Recupera el ID del producto desde la propiedad Tag del botón
                int productoID = Convert.ToInt32(btn.Tag);

                // Llama al método para agregar el producto al carrito en la base de datos
                AgregarAlCarrito(productoID);

                // Muestra mensaje de confirmación
                MessageBox.Show($"Producto agregado al carrito.", "Información");
            }
        }

        // Recibe el ID del producto como parámetro, utiliza el ID del usuario logueado
        // y agrega el producto con una cantidad inicial de 1.
        private void AgregarAlCarrito(int productoID)
        {
            // Consulta para incluir el ID del usuario
            string query = "INSERT INTO Carrito (ID_Usuario, ID_Producto, Cantidad) VALUES (@ID_Usuario, @ProductoID, @Cantidad)";

            using (SqlCommand cmd = new SqlCommand(query, ConexionBD.Conexion))
            {
                // Parámetros a la consulta
                cmd.Parameters.AddWithValue("@ID_Usuario", UsuarioLogueado.ID_Usuario);  // ID del usuario logueado
                cmd.Parameters.AddWithValue("@ProductoID", productoID);  // ID del producto
                cmd.Parameters.AddWithValue("@Cantidad", 1);             // Cantidad inicial es 1

                // Ejecutar la consulta para insertar el producto en el carrito
                ConexionBD.Conexion.Open();
                cmd.ExecuteNonQuery();// Ejecuta la consulta INSERT para agregar el producto al carrito
                ConexionBD.Conexion.Close();
            }
        }
        private void inicioToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void carritoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Crea una nueva instancia del formulario "Carrito"
            Carrito carrito = new Carrito();
            // Muestra el formulario del carrito
            carrito.Show();
            // Ocultar el formulario actual
            this.Hide();
        }

        private void cuentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cuenta cuenta = new Cuenta();
            cuenta.Show();
            this.Hide();
        }

        private void labialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Labiales labiales = new Labiales();
            labiales.Show();
            this.Hide();
        }

        private void correcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Corrector corrector = new Corrector();
            corrector.Show();
            this.Hide();
        }

        private void iluminadoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Iluminadores iluminadores = new Iluminadores();
            iluminadores.Show();
            this.Hide();
        }

        private void primerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Primers primers = new Primers();
            primers.Show();
            this.Hide();
        }

        private void ruborToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rubores rubores = new Rubores();
            rubores.Show();
            this.Hide();
        }

        private void ojosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sombras_Ojos sombras_Ojos = new Sombras_Ojos();
            sombras_Ojos.Show();
            this.Hide();
        }

        private void cejasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sombras_Cejas sombras_Cejas = new Sombras_Cejas();
            sombras_Cejas.Show();
            this.Hide();
        }

        private void sombrasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SombrasT sombrasT = new SombrasT();
            sombrasT.Show();
            this.Hide();
        }

        private void btn_Bus_Click(object sender, EventArgs e)
        {
            string busqueda = txt_Bus.Text.Trim().ToLower(); // Obtiene la búsqueda ingresada y la convierte a minúsculas

            // Conexión a la base de datos
            string ConexionString = "Data Source= DESKTOP-0TP6D1B\\SQLEXPRESS ;Initial Catalog= TiendaMa;Integrated Security=True";
            using (SqlConnection Conexion = new SqlConnection(ConexionString))
            {
                string consultaCategoria = "SELECT COUNT(*) FROM Categoria WHERE LOWER(LTRIM(RTRIM(Nombre))) = @Busqueda";
                string consultaMarca = "SELECT COUNT(*) FROM Productos WHERE LOWER(LTRIM(RTRIM(Marca))) = @Busqueda";
                string consultaProducto = "SELECT COUNT(*) FROM Productos WHERE LOWER(LTRIM(RTRIM(Nombre))) = @Busqueda";

                Conexion.Open();

                // Verificar si es una categoría
                SqlCommand cmdCategoria = new SqlCommand(consultaCategoria, Conexion);
                cmdCategoria.Parameters.AddWithValue("@Busqueda", busqueda);
                int esCategoria = (int)cmdCategoria.ExecuteScalar();

                // Verificar si es una marca
                SqlCommand cmdMarca = new SqlCommand(consultaMarca, Conexion);
                cmdMarca.Parameters.AddWithValue("@Busqueda", busqueda);
                int esMarca = (int)cmdMarca.ExecuteScalar();

                // Verificar si es un producto específico
                SqlCommand cmdProducto = new SqlCommand(consultaProducto, Conexion);
                cmdProducto.Parameters.AddWithValue("@Busqueda", busqueda);
                int esProducto = (int)cmdProducto.ExecuteScalar();

                // Si coincide con una categoría
                if (esCategoria > 0)
                {
                    MostrarFormularioCategoria(busqueda);
                }
                // Si coincide con una marca
                else if (esMarca > 0)
                {
                    MostrarFormularioMarca(busqueda);
                }
                // Si coincide con un producto específico
                else if (esProducto > 0)
                {
                    MostrarFormularioProducto(busqueda);
                }
                else
                {
                    // Muestra mensaje de error si no se encuentra la búsqueda
                    MessageBox.Show("No se encontraron resultados para la búsqueda ingresada. Por favor, intente nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MostrarFormularioCategoria(string categoria)
        {
            switch (categoria)
            {
                case "labiales":
                    Labiales labialesForm = new Labiales();
                    labialesForm.Show();
                    break;

                case "bases":
                    Bases basesForm = new Bases();
                    basesForm.Show();
                    break;

                case "iluminadores":
                    Iluminadores iluminadoresForm = new Iluminadores();
                    iluminadoresForm.Show();
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
                    MessageBox.Show("No se encontró un formulario asociado para esta categoría.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            this.Close(); // Cierra el formulario actual
        }

        private void MostrarFormularioMarca(string marca)
        {
            Marcas formMarca = new Marcas(marca); // Pasa la marca como parámetro
            formMarca.Show();
            this.Close(); // Cierra el formulario actual
        }

        private void MostrarFormularioProducto(string producto)
        {
            ProductosEspe formProducto = new ProductosEspe(producto); // Pasa el producto como parámetro
            formProducto.Show();
            this.Close(); // Cierra el formulario actual
        }


        private void Productos_Click(object sender, EventArgs e)
        {

        }
    }
}
