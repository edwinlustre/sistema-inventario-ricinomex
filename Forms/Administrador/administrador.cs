using appInventario.Form_Ventas;
using appInventario.Forms;
using appInventario.Models;
using System;
using System.Windows.Forms;

namespace appInventario.Form_Administrador
{
    public partial class administrador : Form, IFormToShow
    {
        private Form1 form;
        private Compras compras;
        private produccion Produccion;
        private ventas Ventas;
        private string autenticacion;

        public administrador(Form1 form)
        {
            InitializeComponent();
            compras = new Compras(this);
            Produccion = new produccion(this);
            Ventas = new ventas(this);

            this.form = form;
            this.FormClosing += new FormClosingEventHandler(administrador_FormClosing);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Poblar el ListBox con palabras o ítems
            listBox1.Items.Add("Actualizar precios");
            listBox1.Items.Add("Cerrar sesion");
        }

        // Clase para representar ítems con imagen
        public class ListBoxItem
        {
            public string Text { get; }
            public int ImageIndex { get; }

            public ListBoxItem(string text, int imageIndex)
            {
                Text = text;
                ImageIndex = imageIndex;
            }

            public override string ToString() => Text;
        }

        public void ShowForm()
        {
            this.Show();
        }

        private void administrador_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            form?.Show(); // Utiliza el operador null-conditional para asegurarte de que form no es null
        }

        public void InicializarAutenticacion(string area)
        {
            this.autenticacion = area;
        }

        private void administrador_Load(object sender, EventArgs e)
        {

        }

        private void panelcompras_MouseClick(object sender, MouseEventArgs e)
        {
            this.Hide();
            compras.InicializarAutenticacion(autenticacion);
            compras.Show();
        }

        private void panelproduccion_MouseClick(object sender, MouseEventArgs e)
        {
            this.Hide();
            Produccion.Show();
        }

        private void panelventas_MouseClick(object sender, MouseEventArgs e)
        {
            this.Hide();
            Ventas.Show();
        }

        private void pictureBoxCompras_MouseClick(object sender, MouseEventArgs e)
        {
            panelcompras_MouseClick(sender, e);
        }

        private void pictureBoxProduccion_MouseClick(object sender, MouseEventArgs e)
        {
            panelproduccion_MouseClick(sender, e);
        }

        private void pictureBoxVentas_MouseClick(object sender, MouseEventArgs e)
        {
            panelventas_MouseClick(sender, e);
        }

        private void labelcompras_MouseClick(object sender, MouseEventArgs e)
        {
            panelcompras_MouseClick(sender, e);
        }

        private void labelproduccion_MouseClick(object sender, MouseEventArgs e)
        {
            panelproduccion_MouseClick(sender, e);
        }

        private void labelventas_MouseClick(object sender, MouseEventArgs e)
        {
            panelventas_MouseClick(sender, e);
        }

        private void panelcompras_MouseEnter(object sender, EventArgs e)
        {
            panelcompras.Cursor = Cursors.Hand;
        }

        private void panelproduccion_MouseEnter(object sender, EventArgs e)
        {
            panelproduccion.Cursor = Cursors.Hand;
        }

        private void panelventas_MouseEnter(object sender, EventArgs e)
        {
            panelventas.Cursor = Cursors.Hand;
        }

        private void pictureBoxCompras_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxCompras.Cursor = Cursors.Hand;
        }

        private void pictureBoxProduccion_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxProduccion.Cursor = Cursors.Hand;
        }

        private void pictureBoxVentas_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxVentas.Cursor = Cursors.Hand;
        }

        private void labelcompras_MouseEnter(object sender, EventArgs e)
        {
            labelcompras.Cursor = Cursors.Hand;
        }

        private void labelproduccion_MouseEnter(object sender, EventArgs e)
        {
            labelproduccion.Cursor = Cursors.Hand;
        }

        private void labelventas_MouseEnter(object sender, EventArgs e)
        {
            labelventas.Cursor = Cursors.Hand;
        }

        private void panelcompras_MouseLeave(object sender, EventArgs e)
        {
            panelcompras.Cursor = Cursors.Default;
        }

        private void panelproduccion_MouseLeave(object sender, EventArgs e)
        {
            panelproduccion.Cursor = Cursors.Default;
        }

        private void panelventas_MouseLeave(object sender, EventArgs e)
        {
            panelventas.Cursor = Cursors.Default;
        }

        private void pictureBoxCompras_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxCompras.Cursor = Cursors.Default;
        }

        private void pictureBoxProduccion_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxProduccion.Cursor = Cursors.Default;
        }

        private void pictureBoxVentas_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxVentas.Cursor = Cursors.Default;
        }

        private void labelcompras_MouseLeave(object sender, EventArgs e)
        {
            labelcompras.Cursor = Cursors.Default;
        }

        private void labelproduccion_MouseLeave(object sender, EventArgs e)
        {
            labelproduccion.Cursor = Cursors.Default;
        }

        private void labelventas_MouseLeave(object sender, EventArgs e)
        {
            labelventas.Cursor = Cursors.Default;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            // Alternar la visibilidad del ListBox
            listBox1.Visible = !listBox1.Visible;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Hand;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Default;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtener el elemento seleccionado
            string selectedItem = listBox1.SelectedItem.ToString();

            // Dependiendo de la palabra seleccionada, realiza las acciones correspondientes
            switch (selectedItem)
            {
                case "Actualizar precios":
                    actualizarprecios actualizar = new actualizarprecios(this);
                    actualizar.Show();
                    this.Hide();
                    listBox1.Visible = false;
                    break;

                case "Cerrar sesion":
                    GC.Collect();
                    // Cerrar la ventana actual
                    this.Close();
                    listBox1.Visible = false;
                    break;

                default:
                    MessageBox.Show("Ninguna acción asociada");
                    break;
            }
        }

        private void listBox1_MouseEnter(object sender, EventArgs e)
        {
            listBox1.Cursor = Cursors.Hand;
        }

        private void listBox1_MouseLeave(object sender, EventArgs e)
        {
            listBox1.Cursor = Cursors.Default;
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
