using System;
using System.Windows.Forms;

namespace appInventario.Forms
{
    public partial class carga : Form
    {
        public carga()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }


        public void UpdateStatus(string message, int progress)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, int>(UpdateStatus), message, progress);
            }
            else
            {
                labeltitulo.Text = message;
                progressBar1.Value = progress;
            }
        }
        private void carga_Load(object sender, EventArgs e)
        {

        }
    }
}
