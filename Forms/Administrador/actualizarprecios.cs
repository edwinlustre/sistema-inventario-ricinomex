using appInventario.DAOS;
using System;
using System.Windows.Forms;

namespace appInventario.Form_Administrador
{
    public partial class actualizarprecios : Form
    {

        private daosadministrador Daos;
        private administrador form;

        public actualizarprecios(administrador form)
        {
            InitializeComponent();
            Daos = new daosadministrador();
            this.form = form;
            this.FormClosing += actualizarprecios_FormClosing;

            /* --- No deja redimensionar --- */
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void actualizarprecios_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            form?.Show(); // Asegúrate de que el formulario principal se muestre nuevamente
        }

        private void btnactualizar_Click(object sender, EventArgs e)
        {
            // Obtener y limpiar los valores de los TextBoxes
            string grano = txtgrano.Text.Trim();
            string semilla = txtsemilla.Text.Trim();
            string costal = txtcostal.Text.Trim();

            // Verificar si los campos están vacíos
            if (string.IsNullOrEmpty(grano) || string.IsNullOrEmpty(semilla) || string.IsNullOrEmpty(costal))
            {
                MessageBox.Show("Por favor, asegúrese de que todos los campos estén llenos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Actualizar o insertar precios en la tabla registro_precios
            Daos.ActualizarOInsertarPrecio("Grano", grano);
            Daos.ActualizarOInsertarPrecio("Semilla", semilla);
            Daos.ActualizarOInsertarPrecio("Costal", costal);

            MessageBox.Show("Los precios se han actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void actualizarprecios_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                txtgrano.Text = Daos.ObtenerPrecioGrano();
                txtsemilla.Text = Daos.ObtenerPrecioSemilla();
                txtcostal.Text = Daos.ObtenerPrecioCostal();
            }
        }
    }
}
