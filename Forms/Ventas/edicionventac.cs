using appInventario.DAOS;
using System;
using System.IO;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class edicionventac : Form
    {
        private string descargarfactura;
        private string descargarcomprobante;
        private string descargarorden;
        private daosventas Daos;

        public edicionventac()
        {
            InitializeComponent();
            txtid.Enabled = false;
            Daos = new daosventas();

            // Establecer el borde de la ventana para evitar redimensionamiento
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        // Método para recibir y mostrar los datos en los TextBox
        public void SetTextBoxValues(string id_cotizacion, string comprobantePago, string ordenCompra, string factura)
        {
            txtid.Text = "ARM-" + id_cotizacion;
            descargarcomprobante = comprobantePago;
            descargarorden = ordenCompra;
            descargarfactura = factura;
        }

        private string ConvertFileToBase64(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(fileBytes);
        }

        // Métodos para obtener los valores modificados
        public string GetFactura()
        {
            return descargarfactura;
        }

        public string GetComprobantePago()
        {
            return descargarcomprobante;
        }

        public string GetOrdenCompra()
        {
            return descargarorden;
        }

        private void btnactualizar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK; // Confirmar la edición
            this.Close();
        }

        // Métodos para subir archivos
        private void btnañadirf_Click_1(object sender, EventArgs e)
        {
            // Crear una instancia de OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Configurar las propiedades del OpenFileDialog
            openFileDialog.InitialDirectory = "C:\\"; // Usa barras invertidas escapadas
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
                    descargarfactura = ConvertFileToBase64(selectedFilePath);

                    if (!string.IsNullOrEmpty(descargarfactura))
                    {
                        // Si ya hay archivos previamente, agregar una coma antes de concatenar el nuevo archivo
                        if (!string.IsNullOrEmpty(descargarfactura))
                        {
                            descargarfactura += ",";  // Añadir separador si ya existe contenido
                        }

                        // Concatenar el archivo convertido
                        descargarfactura += descargarfactura;
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

        private void btnañadircomprobante_Click(object sender, EventArgs e)
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
                        if (!string.IsNullOrEmpty(descargarcomprobante))
                        {
                            descargarcomprobante += ",";  // Añadir separador si ya existe contenido
                        }

                        // Concatenar el nuevo archivo convertido
                        descargarcomprobante += nuevoArchivoBase64;
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

        private void btnañadirc_Click(object sender, EventArgs e)
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
                        if (!string.IsNullOrEmpty(descargarorden))
                        {
                            descargarorden += ",";  // Añadir separador si ya existe contenido
                        }

                        // Concatenar el nuevo archivo convertido
                        descargarorden += nuevoArchivoBase64;
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

        private void btndescargarcomprobante_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(descargarcomprobante))
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
                        string[] archivosBase64Array = descargarcomprobante.Split(',');

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
                                    string filePath = Path.Combine(selectedPath, $"archivo_{i + 1}.pdf");

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

        private void btndescargarcompra_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(descargarorden))
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
                        string[] archivosBase64Array = descargarorden.Split(',');

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
                                    string filePath = Path.Combine(selectedPath, $"archivo_{i + 1}.pdf");

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

        private void btndescargarfactura_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(descargarfactura))
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
                        string[] archivosBase64Array = descargarfactura.Split(',');

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
                                    string filePath = Path.Combine(selectedPath, $"archivo_{i + 1}.pdf");

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
