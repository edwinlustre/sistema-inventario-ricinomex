using System;
using System.Drawing;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class agregarcondiciones : Form
    {
        public string Condiciones { get; private set; }

        public agregarcondiciones()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            comboBoxCondiciones.SelectedIndex = 0;
            AjustarAnchoComboBox(comboBoxCondiciones);

        }

        private void btnañadir_Click(object sender, EventArgs e)
        {
            Condiciones = txtcondicionesextra.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void comboBoxCondiciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificar si el índice seleccionado no es 0
            if (comboBoxCondiciones.SelectedIndex != 0)
            {
                // Obtener el texto seleccionado del ComboBox
                string selectedText = comboBoxCondiciones.SelectedItem.ToString();

                // Agregar la información seleccionada al TextBox con un salto de línea
                txtcondicionesextra.AppendText(selectedText + "\r\n");

                // Regresar el ComboBox a la opción default (índice 0)
                comboBoxCondiciones.SelectedIndex = 0;
            }
        }

        private void agregarcondiciones_Load(object sender, EventArgs e)
        {

        }

        private void AjustarAnchoComboBox(System.Windows.Forms.ComboBox comboBox)
        {
            int maxWidth = comboBox.DropDownWidth;
            using (Graphics g = comboBox.CreateGraphics())
            {
                foreach (var item in comboBox.Items)
                {
                    int itemWidth = (int)g.MeasureString(item.ToString(), comboBox.Font).Width;
                    if (itemWidth > maxWidth)
                    {
                        maxWidth = itemWidth;
                    }
                }
            }
            comboBox.DropDownWidth = maxWidth;
        }
    }
}
