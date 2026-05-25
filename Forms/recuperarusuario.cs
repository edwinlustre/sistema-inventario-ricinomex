using appInventario.DAOS;
using System;
using System.Windows.Forms;

namespace appInventario.Forms
{
    public partial class recuperarusuario : Form
    {
        private Form1 form;
        private daos Daos;

        public recuperarusuario(Form1 form)
        {
            InitializeComponent();
            this.form = form;
            this.FormClosing += new FormClosingEventHandler(recuperarusuario_FormClosing);
            Daos = new daos();

            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void txtrepassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verificar si la tecla presionada es Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Suprimir el sonido "ding"
                e.Handled = true;
                btncambiar.PerformClick();
            }
        }

        private void recuperarusuario_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            form?.Show(); // Utiliza el operador null-conditional para asegurarte de que form no es null   
        }

        private void btncambiar_Click_1(object sender, EventArgs e)
        {
            string usuario = txtusuario.Text.Trim();
            string password = txtpassword.Text;
            string repassword = txtrepassword.Text;

            if (String.IsNullOrEmpty(usuario) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(repassword))
            {
                MessageBox.Show("Rellena todos los datos.", "Advertencia");
                return;
            }

            if (password != repassword)
            {
                MessageBox.Show("Las contraseñas no coinciden, intentalo de nuevo.", "Advertencia");
                return;
            }

            bool exito = Daos.CambiarContraseña(usuario, password);
            if (exito)
            {
                MessageBox.Show("Contraseña cambiada con éxito.", "Éxito");
                // Limpiar campos
                txtusuario.Clear();
                txtpassword.Clear();
                txtrepassword.Clear();
            }
            else
            {
                MessageBox.Show("No se pudo cambiar la contraseña. Verifique que el usuario exista.", "Error");
            }
        }

        private void recuperarusuario_Load(object sender, EventArgs e)
        {

        }
    }
}
