using appInventario.DAOS;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appInventario.Forms
{
    public partial class confirmacioncompra : Form
    {
        private int id;
        private int idp;
        private resultadoscompracostales compra;
        private Compras compras;
        private daos Daos;
        private daosadministrador DaosA;
        private double kgOriginal; // Variable para guardar el valor original de kg

        public confirmacioncompra(string kg, int id, int idp)
        {
            InitializeComponent();
            compra = new resultadoscompracostales();
            Daos = new daos();
            DaosA = new daosadministrador();
            compras = new Compras();

            txtkg.Text = kg;
            this.id = id;
            this.idp = idp;
            this.kgOriginal = double.Parse(kg); // Guardar el valor original de kg

            txtprecio.TextChanged += new EventHandler(precioFinal_TextChanged);
            txtporcentaje.TextChanged += new EventHandler(precioFinal_TextChanged);
            txtkg.Enabled = false;
            txtprecio.Enabled = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void precioFinal_TextChanged(object sender, EventArgs e)
        {
            string porcentaje = txtporcentaje.Text.Trim();
            string precio = txtprecio.Text.Trim();

            if (!string.IsNullOrEmpty(porcentaje) && double.TryParse(porcentaje, out double dporcentaje))
            {
                if (dporcentaje >= 0 && dporcentaje <= 100)
                {
                    // Calcular el nuevo valor de kilos basado en el valor original
                    double calculo = kgOriginal * (dporcentaje / 100);

                    // Actualizar el TextBox de kg con el nuevo valor calculado
                    txtkg.Text = calculo.ToString("F2");

                    if (double.TryParse(precio, out double dprecio))
                    {
                        // Calcular el importe basado en el nuevo valor de kilos
                        double importef = dprecio * calculo;
                        txtimporte.Text = importef.ToString("F2");
                    }
                }
                else
                {
                    MessageBox.Show("El porcentaje debe estar entre 0 y 100.", "Valor de Porcentaje Incorrecto");
                    txtporcentaje.Focus();
                }
            }
            else
            {
                txtimporte.Text = "";
            }
        }

        private void confirmacioncompra_Load(object sender, EventArgs e)
        {

        }

        private async void txtpagar_Click(object sender, EventArgs e)
        {
            string precio = txtprecio.Text.Trim();
            string importe = txtimporte.Text.Trim();

            using (var loadingForm = new carga())
            {
                loadingForm.Show();
                await Task.Run(() =>
                {
                    try
                    {
                        // Registrar la compra en la base de datos
                        loadingForm.UpdateStatus("Guardando datos...", 30);
                        // Actualiza el estado de pago
                        compra.pagoFinal(precio, importe, id, "Pagar");

                        loadingForm.UpdateStatus("Recuperando informacion...", 45);

                        loadingForm.UpdateStatus("Actualizado", 100);

                        MessageBox.Show("Informacion actualizada.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al registrar la compra: " + ex.Message, "Error");
                    }
                });
                loadingForm.Close();
                // Configurar el DialogResult para OK cuando se confirme la acción
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void confirmacioncompra_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                txtprecio.Text = DaosA.ObtenerPrecioCostal();
            }
        }
    }
}
