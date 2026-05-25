using appInventario.DAOS;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class agregarfacturación : Form
    {

        private daosventas Daos;
        public int id = 0;

        public string Paqueteria => comboBoxPaqueteria.Text;
        public string Razon => txtrazon.Text.Trim();
        public string RFC => txtrfc.Text.Trim();
        public string CodigoPostal => txtcodigopostal.Text.Trim();
        public string Regimen => comboBoxRegimen.Text;
        public string CFDI => comboBoxCFDI.Text;
        public string MetodoPago => comboBoxMetodo.Text;
        public string FormaPago => comboBoxForma.Text;
        public agregarfacturación(int id)
        {
            this.id = id;
            InitializeComponent();
            Daos = new daosventas();

            comboBoxRegimen.SelectedIndex = 0;
            comboBoxCFDI.SelectedIndex = 0;
            comboBoxMetodo.SelectedIndex = 0;
            comboBoxForma.SelectedIndex = 0;
            comboBoxSolicitud.SelectedIndex = 0;
            comboBoxPaqueteria.SelectedIndex = 0;

            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Evitar redimensionamiento
        }

        private void AjustarAnchoComboBox(ComboBox comboBox)
        {
            int anchoMaximo = comboBox.DropDownWidth;
            using (Graphics g = comboBox.CreateGraphics())
            {
                foreach (string s in comboBox.Items)
                {
                    int anchoItem = (int)g.MeasureString(s, comboBox.Font).Width;
                    if (anchoItem > anchoMaximo)
                    {
                        anchoMaximo = anchoItem;
                    }
                }
            }
            comboBox.DropDownWidth = anchoMaximo;
        }

        private void btnañadir_Click(object sender, EventArgs e)
        {
            // Validar que ninguno de los campos esté vacío
            if (string.IsNullOrEmpty(Razon) || string.IsNullOrEmpty(RFC) || string.IsNullOrEmpty(CodigoPostal) ||
                comboBoxRegimen.SelectedIndex == 0 || comboBoxCFDI.SelectedIndex == 0 || comboBoxMetodo.SelectedIndex == 0 ||
                comboBoxForma.SelectedIndex == 0 || comboBoxPaqueteria.SelectedIndex == 0)
            {
                MessageBox.Show("Todos los campos de facturación son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Asigna la información y cierra el formulario
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void agregarfacturación_Load(object sender, EventArgs e)
        {
            AjustarAnchoComboBox(comboBoxCFDI);
            AjustarAnchoComboBox(comboBoxRegimen);
            AjustarAnchoComboBox(comboBoxMetodo);
            AjustarAnchoComboBox(comboBoxForma);
        }

        private void comboBoxSolicitud_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedIndex == 0)
            {
                // Llamar a la función para cargar los datos del cliente y asignarlos directamente a los TextBox
                Daos.CargarDatosClientePorId(id, ref txtrazon, ref txtrfc, ref txtcodigopostal);
                comboBoxRegimen.SelectedIndex = 0;
                comboBoxCFDI.SelectedIndex = 0;
            }
            else if (comboBox.SelectedIndex == 1)
            {
                txtrazon.Text = "PUBLICO EN GENERAL";
                txtrfc.Text = "XAXX010101000";
                txtcodigopostal.Text = "71502";
                comboBoxRegimen.SelectedIndex = 12;
                comboBoxCFDI.SelectedIndex = 24;
            }
        }
    }
}
