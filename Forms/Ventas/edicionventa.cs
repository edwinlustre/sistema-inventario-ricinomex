using System;
using System.IO;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class edicionventa : Form
    {
        public string descargafactura = "";
        public string descargapago = "";
        public string descargaenvio = "";

        public edicionventa()
        {
            InitializeComponent();
            txtid.ReadOnly = true;
            txtid.Enabled = false;

            // Establecer el borde de la ventana para evitar redimensionamiento
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Mostrar los valores iniciales
            //MessageBox.Show("Valores iniciales en el constructor: " + descargafactura + ", " + descargapago + ", " + descargaenvio);
        }

        public void SetTextBoxValues(string id_cotizacion, string formaEnvio, string estatusFactura, string estatusPago, string estatusEnvio)
        {
            txtid.Text = "ARM-" + id_cotizacion;
            comboBoxFormaEnvio.Text = formaEnvio;
            descargafactura = estatusFactura;
            descargapago = estatusPago;
            descargaenvio = estatusEnvio;
        }

        private void btnactualizar_Click(object sender, EventArgs e)
        {
            // Si todo está bien, cerrar la ventana y devolver DialogResult.OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private string ConvertFileToBase64(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(fileBytes);
        }

        private void edicionventa_Load(object sender, EventArgs e)
        {
            // Mostrar los valores recibidos
            //MessageBox.Show("Valores recibidos en load: " + descargafactura + ", " + descargapago + ", " + descargaenvio);
        }


        private void btnañadirf_Click(object sender, EventArgs e)
        {
            // Crear una instancia de OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Configurar las propiedades del OpenFileDialog
            openFileDialog.InitialDirectory = "C:\\";
            openFileDialog.Filter = "Archivo PDF (*.pdf)|*.pdf|Todos los archivos (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            // Mostrar el diálogo y verificar si el usuario seleccionó un archivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtener la ruta del archivo seleccionado
                string selectedFilePath = openFileDialog.FileName;

                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    // Convertir archivo a Base64
                    string nuevoArchivoBase64 = ConvertFileToBase64(selectedFilePath);

                    if (!string.IsNullOrEmpty(nuevoArchivoBase64))
                    {
                        // Si ya hay archivos previamente, agregar una coma antes de concatenar el nuevo archivo
                        if (!string.IsNullOrEmpty(descargafactura))
                        {
                            descargafactura += ",";  // Añadir separador si ya existe contenido
                        }

                        // Concatenar el nuevo archivo convertido
                        descargafactura += nuevoArchivoBase64;
                    }
                    else
                    {
                        MessageBox.Show("Error al convertir el archivo a Base64.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Error al subir el archivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnañadirp_Click(object sender, EventArgs e)
        {
            // Crear una instancia de OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Configurar las propiedades del OpenFileDialog
            openFileDialog.InitialDirectory = "C:\\";
            openFileDialog.Filter = "Archivo PDF (*.pdf)|*.pdf|Todos los archivos (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            // Mostrar el diálogo y verificar si el usuario seleccionó un archivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtener la ruta del archivo seleccionado
                string selectedFilePath = openFileDialog.FileName;

                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    // Convertir archivo a Base64
                    string nuevoArchivoBase64 = ConvertFileToBase64(selectedFilePath);

                    if (!string.IsNullOrEmpty(nuevoArchivoBase64))
                    {
                        // Si ya hay archivos previamente, agregar una coma antes de concatenar el nuevo archivo
                        if (!string.IsNullOrEmpty(descargapago))
                        {
                            descargapago += ",";  // Añadir separador si ya existe contenido
                        }

                        // Concatenar el nuevo archivo convertido
                        descargapago += nuevoArchivoBase64;
                    }
                    else
                    {
                        MessageBox.Show("Error al convertir el archivo a Base64.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Error al subir el archivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnañadire_Click(object sender, EventArgs e)
        {
            // Crear una instancia de OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Configurar las propiedades del OpenFileDialog
            openFileDialog.InitialDirectory = "C:\\";
            openFileDialog.Filter = "Archivo PDF (*.pdf)|*.pdf|Todos los archivos (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            // Mostrar el diálogo y verificar si el usuario seleccionó un archivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtener la ruta del archivo seleccionado
                string selectedFilePath = openFileDialog.FileName;

                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    // Convertir archivo a Base64
                    string nuevoArchivoBase64 = ConvertFileToBase64(selectedFilePath);

                    if (!string.IsNullOrEmpty(nuevoArchivoBase64))
                    {
                        // Si ya hay archivos previamente, agregar una coma antes de concatenar el nuevo archivo
                        if (!string.IsNullOrEmpty(descargaenvio))
                        {
                            descargaenvio += ",";  // Añadir separador si ya existe contenido
                        }

                        // Concatenar el nuevo archivo convertido
                        descargaenvio += nuevoArchivoBase64;
                    }
                    else
                    {
                        MessageBox.Show("Error al convertir el archivo a Base64.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Error al subir el archivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btndescargarf_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(descargafactura))
            {
                MessageBox.Show("No hay archivos subidos.", "Mensaje");
            }
            else
            {
                // Abrir el diálogo para que el usuario seleccione el directorio donde quiere guardar los archivos
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Selecciona el directorio donde deseas guardar los archivos PDF";

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedPath = folderDialog.SelectedPath; // Ruta del directorio seleccionado

                        // Dividir la cadena descargafactura por las comas (en caso de que haya varios archivos concatenados)
                        string[] archivosBase64Array = descargafactura.Split(',');

                        // Iterar sobre cada archivo Base64 en el array
                        for (int i = 0; i < archivosBase64Array.Length; i++)
                        {
                            string archivoBase64 = archivosBase64Array[i];

                            if (!string.IsNullOrEmpty(archivoBase64))
                            {
                                try
                                {
                                    // Convertir Base64 a bytes
                                    byte[] pdfBytes = Convert.FromBase64String(archivoBase64);

                                    // Especificar la ruta completa del archivo PDF reconstruido
                                    string filePath = Path.Combine(selectedPath, $"archivo_factura{i + 1}.pdf");

                                    // Guardar el archivo reconstruido en la ruta especificada
                                    File.WriteAllBytes(filePath, pdfBytes);

                                    MessageBox.Show($"Archivo PDF {i + 1} reconstruido y guardado en: {filePath}", "Mensaje");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error al reconstruir el archivo PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se seleccionó ningún directorio.", "Mensaje");
                    }
                }
            }
        }

        private void btndescargarp_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(descargapago))
            {
                MessageBox.Show("No hay archivos subidos.", "Mensaje");
            }
            else
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Selecciona el directorio donde deseas guardar los archivos PDF de pagos";

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedPath = folderDialog.SelectedPath;

                        // Dividir la cadena descargapago por las comas
                        string[] archivosBase64Array = descargapago.Split(',');

                        // Iterar sobre cada archivo Base64 en el array
                        for (int i = 0; i < archivosBase64Array.Length; i++)
                        {
                            string archivoBase64 = archivosBase64Array[i];

                            if (!string.IsNullOrEmpty(archivoBase64))
                            {
                                try
                                {
                                    // Convertir Base64 a bytes
                                    byte[] pdfBytes = Convert.FromBase64String(archivoBase64);

                                    // Especificar la ruta completa del archivo PDF reconstruido
                                    string filePath = Path.Combine(selectedPath, $"archivo_pago{i + 1}.pdf");

                                    // Guardar el archivo reconstruido en la ruta especificada
                                    File.WriteAllBytes(filePath, pdfBytes);

                                    MessageBox.Show($"Archivo PDF {i + 1} reconstruido y guardado en: {filePath}", "Mensaje");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error al reconstruir el archivo PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se seleccionó ningún directorio.", "Mensaje");
                    }
                }
            }
        }

        private void btndescargare_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(descargaenvio))
            {
                MessageBox.Show("No hay archivos subidos.", "Mensaje");
            }
            else
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Selecciona el directorio donde deseas guardar los archivos PDF de envíos";

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedPath = folderDialog.SelectedPath;

                        // Dividir la cadena descargaenvio por las comas
                        string[] archivosBase64Array = descargaenvio.Split(',');

                        // Iterar sobre cada archivo Base64 en el array
                        for (int i = 0; i < archivosBase64Array.Length; i++)
                        {
                            string archivoBase64 = archivosBase64Array[i];

                            if (!string.IsNullOrEmpty(archivoBase64))
                            {
                                try
                                {
                                    // Convertir Base64 a bytes
                                    byte[] pdfBytes = Convert.FromBase64String(archivoBase64);

                                    // Especificar la ruta completa del archivo PDF reconstruido
                                    string filePath = Path.Combine(selectedPath, $"archivo_envio{i + 1}.pdf");

                                    // Guardar el archivo reconstruido en la ruta especificada
                                    File.WriteAllBytes(filePath, pdfBytes);

                                    MessageBox.Show($"Archivo PDF {i + 1} reconstruido y guardado en: {filePath}", "Mensaje");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error al reconstruir el archivo PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se seleccionó ningún directorio.", "Mensaje");
                    }
                }
            }
        }
    }
}
