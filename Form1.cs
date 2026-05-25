using appInventario.DAOS;
using appInventario.Form_Administrador;
using appInventario.Form_Ventas;
using appInventario.Forms;
using appInventario.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appInventario
{
    public partial class Form1 : Form, IFormToShow
    {
        //Instancias
        private Compras compras;
        private produccion Produccion;
        private ventas Ventas;
        private administrador Administrador;
        private recuperarusuario ru;
        private daos Daos;
        //private daosadministrador DaosA;
        private EncriptadoContraseñas encriptado;

        public Form1()
        {
            InitializeComponent();
            Daos = new daos();
            encriptado = new EncriptadoContraseñas();

            /* --- No deja redimensionar --- */
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

        }

        //Maneja los accesos al administrador y usuarios
        public void ShowForm()
        {
            this.Show();
        }

        //Revision de inicio de sesion
        private async void btniniciarsesion_Click(object sender, EventArgs e)
        {
            string usuario = txtusuario.Text.Trim();
            string password = txtpassword.Text.Trim();

            if (string.IsNullOrEmpty(usuario))
            {
                MessageBox.Show("Te falta ingresar el usuario", "Mensaje");
            }
            else if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Te falta ingresar la contraseña", "Mensaje");
            }
            else
            {
                // Crear y mostrar el formulario de carga
                carga cargaForm = new carga();
                cargaForm.StartPosition = FormStartPosition.CenterScreen;
                //cargaForm.TopMost = true;
                cargaForm.Show();

                try
                {
                    // Actualizar el formulario de carga con el mensaje y progreso inicial
                    cargaForm.UpdateStatus("Verificando datos...", 20);

                    // Ejecutar la tarea en un hilo de fondo
                    var datosUsuario = await Task.Run(() => Daos.obtenerDatosUsuario(usuario, password));

                    // Actualizar el progreso al 50% (opcional)
                    cargaForm.UpdateStatus("Procesando...", 50);

                    if (datosUsuario.area == null)
                    {
                        // Si el área es null, significa que el usuario o contraseña son incorrectos
                        MessageBox.Show("Usuario o contraseña incorrectos.", "Advertencia");
                        // Cerrar el formulario de carga
                        cargaForm.Close();
                    }
                    else
                    {
                        string saludo = $"Hola, {datosUsuario.nombre} {datosUsuario.apellidoPat}";
                        string area = $"{datosUsuario.area}";

                        // Dependiendo del área, se muestra la ventana correspondiente con el saludo en el Label
                        this.Hide();
                        if (datosUsuario.area == "Produccion")
                        {
                            Produccion = new produccion(this);
                            Produccion.labelusuario.Text = saludo;
                            Produccion.Show();
                        }
                        else if (datosUsuario.area == "Compras")
                        {
                            compras = new Compras(this);
                            compras.labelusuario.Text = saludo;
                            compras.InicializarAutenticacion(area);
                            compras.Show();
                        }
                        else if (datosUsuario.area == "Ventas")
                        {
                            Ventas = new ventas(this);
                            Ventas.usuarioventa = saludo;
                            Ventas.Show();
                        }
                        else if (datosUsuario.area == "Administrador")
                        {
                            Administrador = new administrador(this);
                            Administrador.InicializarAutenticacion(area);
                            Administrador.Show();
                        }

                        daosadministrador DaosA = new daosadministrador();
                        DaosA.VerificarOCrearRegistros();

                        // Limpiar los campos de texto después del inicio de sesión
                        txtusuario.Text = "";
                        txtpassword.Text = "";
                    }

                    // Actualizar el progreso al 100% (opcional)
                    cargaForm.UpdateStatus("Carga completada.", 100);
                }
                catch (Exception ex)
                {
                    // Manejar cualquier error aquí
                    MessageBox.Show("Error al procesar el inicio de sesión: " + ex.Message, "Error");
                }
                finally
                {
                    // Cerrar el formulario de carga
                    cargaForm.Close();
                }
            }
        }

        private void txtpassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verificar si la tecla presionada es Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Suprimir el sonido "ding"
                e.Handled = true;
                btniniciarsesion.PerformClick();
            }
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            ru = new recuperarusuario(this);
            this.Hide();
            ru.Show();
        }

    }
}
