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
    public partial class Primers : Form
    {
        public Primers()
        {
            InitializeComponent();
            // Configurar el autocompletado del txt_Bus
            txt_Bus.AutoCompleteMode = AutoCompleteMode.Suggest;
            txt_Bus.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_Bus.AutoCompleteCustomSource.AddRange(new string[] { "Bases", "Correctores", "Iluminadores", "Labiales", "Mascaras", "Primers", "Rubores", "Sombras", "Sombras de ojos", "Sombras de cejas" });

        }
        private void Primers_Load(object sender, EventArgs e)
        {
            // Cargar los productos al iniciar el formulario
            CargarProductos();
        }

        private void CargarProductos()
        {
            // Limpia el FlowLayoutPanel antes de agregar productos
            flp_Primers.Controls.Clear();

            // Conexión a la base de datos
            string ConexionString = "Data Source= DESKTOP-0TP6D1B\\SQLEXPRESS ;Initial Catalog= TiendaMa ;Integrated Security=True";
            using (SqlConnection Conexion = new SqlConnection(ConexionString))
            {
                // Consulta SQL para obtener productos de la categoría 1
                string consulta = "SELECT ID_Producto, Imagen, Nombre, Marca, Stock, Precio FROM Productos WHERE ID_Categoria = 6";
                // Se crea un comando SQL con la consulta y la conexión establecida
                SqlCommand comando = new SqlCommand(consulta, Conexion);
                // Se abre la conexión a la base de datos
                Conexion.Open();
                // Se ejecuta la consulta y se obtiene un objeto SqlDataReader para leer los resultados
                SqlDataReader lector = comando.ExecuteReader();

                while (lector.Read())// Iterar sobre los resultados de la consulta
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
                    flp_Primers.Controls.Add(panelProducto);
                    flp_Primers.Padding = new Padding(30);  // Un poco de espacio alrededor del pane
                }
            }
        }

        private void Btn_Agregar_Click(object sender, EventArgs e)
        {
            // Obtener el botón que disparó el evento
            Button btn = sender as Button;
            // Verifica que el objeto convertido a botón no sea nulo 
            // y que la propiedad Tag del botón contenga un valor válido
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

        private void AgregarAlCarrito(int productoID)
        {

            // Modifica la consulta para incluir el ID del usuario
            string query = "INSERT INTO Carrito (ID_Usuario, ID_Producto, Cantidad) VALUES (@ID_Usuario, @ProductoID, @Cantidad)";

            using (SqlCommand cmd = new SqlCommand(query, ConexionBD.Conexion))
            {
                // Pasa los parámetros a la consulta
                cmd.Parameters.AddWithValue("@ID_Usuario", UsuarioLogueado.ID_Usuario);  // ID del usuario logueado
                cmd.Parameters.AddWithValue("@ProductoID", productoID);  // ID del producto
                cmd.Parameters.AddWithValue("@Cantidad", 1);             // Cantidad inicial es 1


                ConexionBD.Conexion.Open();
                cmd.ExecuteNonQuery();// Ejecuta la consulta INSERT para agregar el producto al carrito
                ConexionBD.Conexion.Close();
            }
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

        private void btn_Bus_Click_1(object sender, EventArgs e)
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
