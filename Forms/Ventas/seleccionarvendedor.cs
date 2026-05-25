using appInventario.DAOS;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class seleccionarvendedor : Form
    {

        private daosventas Daos;
        public string NombreVendedor { get; private set; }
        public string TelefonoVendedor { get; private set; }
        public string CorreoVendedor { get; private set; }

        public seleccionarvendedor()
        {
            Daos = new daosventas();
            InitializeComponent();

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Mostrar el formulario en primer plano
            this.TopMost = true;
            this.BringToFront();
        }

        private void seleccionarvendedor_Load(object sender, EventArgs e)
        {
            MostrarVendedores();

            // Suscribirse al evento CellDoubleClick del DataGridView
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;

            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 9);
        }


        public void MostrarVendedores()
        {
            // Obtener los datos de los vendedores
            DataTable dtVendedores = Daos.ObtenerVendedores();

            if (dtVendedores.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron vendedores en la base de datos.");
                return;
            }

            // Asignar los datos al DataGridView
            dataGridView1.DataSource = dtVendedores;

            // Establecer los encabezados de las columnas del DataGridView
            dataGridView1.Columns["id"].HeaderText = "ID";
            dataGridView1.Columns["nombre"].HeaderText = "Nombre";
            dataGridView1.Columns["apellidopat"].HeaderText = "Apellido Paterno";
            dataGridView1.Columns["apellidomat"].HeaderText = "Apellido Materno";
            dataGridView1.Columns["numero"].HeaderText = "Número de Telefono";
            dataGridView1.Columns["correo"].HeaderText = "Correo";
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow filaSeleccionada = dataGridView1.Rows[e.RowIndex];

                if (filaSeleccionada != null &&
                    filaSeleccionada.Cells["nombre"].Value != null &&
                    filaSeleccionada.Cells["apellidopat"].Value != null)
                {
                    NombreVendedor = $"{filaSeleccionada.Cells["nombre"].Value.ToString()} " +
                                     $"{filaSeleccionada.Cells["apellidopat"].Value.ToString()} " +
                                     $"{filaSeleccionada.Cells["apellidomat"].Value.ToString()}";
                    TelefonoVendedor = filaSeleccionada.Cells["numero"].Value.ToString();
                    CorreoVendedor = filaSeleccionada.Cells["correo"].Value.ToString();

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudieron obtener los detalles del vendedor. Verifique los datos.");
                }
            }
        }


    }
}
