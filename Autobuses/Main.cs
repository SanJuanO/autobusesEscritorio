using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Autobuses.Planeacion;
using Autobuses.Administracion;
using Autobuses.Reportes;
using System.Drawing.Printing;
using System.Configuration;
using System.IO;
using ConnectDB;
using System.Data.SqlClient;
using Autobuses.Utilerias;

namespace Autobuses
{
    public partial class Main : Form
    {
        Form form = null;
        private string _clase = "main";
        private bool mostrar = false;
        private string impresoraname;
        public database db;
        private string actualizacion;
        private string ligaactualizacion;


        ResultSet res = null;

        public void checkInternetAvaible() {
            if (!Utilerias.Utilerias.CheckForInternetConnection())
            {
                MessageBox.Show("Error al conectarse a internet");
                return;
              
            }
        }

        public Main()
        {
            try
            {

          

                //Si el usuario ya inicio Sesion
                InitializeComponent();

                if (LoginInfo.isLoggedIn)
                {

                }
                else
                {
                    db = new database();

                    // form.BringToFront();
                    timer1.Enabled = true;
                    timer2.Start();
                    versiontext.Text = ProductVersion;
                    PopulateInstalledPrintersCombo();
                    comboimpresora.DropDownStyle = ComboBoxStyle.DropDownList;
                    panelplaneacion.Visible = false;
                    panelreportes.Visible = false;
                    panelconfiguracion.Visible = false;
                    labelsucursal.Text = LoginInfo.Sucursal;

                    labelnombre.Text = LoginInfo.NombreID;
                    labelapellido.Text = LoginInfo.ApellidoID;
                    MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
                    Bitmap bmp = new Bitmap(ms);
                    pictureBoxfoto.Image = bmp;
                    labelcargo.Text = LoginInfo.rol;
                    permiso();

                    pagarguia.Visible = false;
                    timer3.Start();
                    if (Settings1.Default.impresora == "Ninguna")
                    {
                        Form mensaje = new Mensaje("Configura la impresora por favor", true);

                        mensaje.ShowDialog();
                    }

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "PERMISOS";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        private void mostraractualizacion()
        {

            if (int.Parse(ProductVersion.Replace(".",""))< int.Parse(actualizacion.Replace(".","")))
            {
                pictureBoxactualizar.Visible = true;
                buttonactualizar.Visible = true;
                
                Form mensaje = new Mensaje("Existe una nueva versión, actualiza el sistema", true);
                versionactualtext.Visible = true;
                versionactualtext.Text = "Versión disponible:   " + actualizacion;

                mensaje.ShowDialog();
            }
            else
            {
                versionactualtext.Visible = false;

                pictureBoxactualizar.Visible = false;
                buttonactualizar.Visible = false;
            }

        }
        void OBTENERCHOFERESSUPRA() 
        {
            try
            {
                LoginInfo.supra.Clear();
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("Error al conectarse a internet");
                    return;
                }
                else
                {
                    string sql = "SELECT USUARIO FROM CHOFERES WHERE ACTIVO=1 AND BORRADO=0 AND LINEA='SUPRA'";


                    db.PreparedSQL(sql);



                    res = db.getTable();
                    while (res.Next())
                    {
                        LoginInfo.supra.Add(res.Get("USUARIO"));
                    }
                    for (int i = 0; i < LoginInfo.supra.Count(); i++)
                    {


                        string cadenaDeConexion = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                        using (SqlConnection cn = new SqlConnection(cadenaDeConexion))
                        {
                            cn.Open();
                            SqlCommand cmd = new SqlCommand("SELECT HUELLA FROM CHOFERES WHERE USUARIO='" + LoginInfo.supra[i] + "'", cn);
                            LoginInfo.fingerPrintchoferessupra.Add ( (byte[])cmd.ExecuteScalar());
                        }

                    }
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "validaruser";
                Utilerias.LOG.write(_clase, funcion, error);

            }

        }
        void OBTENERCHOFERESEJECUTIVO()
        {
            try
            {
                LoginInfo.ejecutivo.Clear();

                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("Error al conectarse a internet");
                    return;
                }
                else
                {
                    string sql = "SELECT USUARIO FROM CHOFERES WHERE ACTIVO=1 AND BORRADO=0 AND LINEA='Ejecutivo Centro'";


                    db.PreparedSQL(sql);



                    res = db.getTable();
                    while (res.Next())
                    {
                      LoginInfo.ejecutivo.Add(res.Get("USUARIO"));
                    }
                    for (int i = 0; i < LoginInfo.ejecutivo.Count(); i++)
                    {


                        string cadenaDeConexion = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                        using (SqlConnection cn = new SqlConnection(cadenaDeConexion))
                        {
                            cn.Open();
                            SqlCommand cmd = new SqlCommand("SELECT HUELLA FROM CHOFERES WHERE USUARIO='" + LoginInfo.ejecutivo[i] + "'", cn);
                            LoginInfo.fingerPrintchoferesejecutivo[i] = (byte[])cmd.ExecuteScalar();
                        }

                    }
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "validaruser";
                Utilerias.LOG.write(_clase, funcion, error);

            }

        }


        private void verificaractualizacion()
        {
            try
            {
                string sql = "SELECT VALOR,DESCRIPCION FROM VARIABLES";

                sql += " WHERE  NOMBRE='ACTUALIZACION' ";
                db.PreparedSQL(sql);


                res = db.getTable();
                if (res.Next())
                {
                    actualizacion = res.Get("VALOR");
                    ligaactualizacion = res.Get("DESCRIPCION");
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "verificaracutalizacion";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void permiso()
        {
            try
            {
                usuarios.Visible = false;
                socios.Visible = false;
                sucursales.Visible = false;
                conductores.Visible = false;
                autobuses.Visible = false;
                diseño.Visible = false;
                quejas.Visible = false;
                infracciones.Visible = false;
                roles.Visible = false;
                permisos.Visible = false;
                notificar.Visible = false;
                corridas.Visible = false;
                rutas.Visible = false;
                rolado.Visible = false;
                lineas.Visible = false;
                destinos.Visible = false;
                tarjetas.Visible = false;
                pasaje.Visible = false;
                zonas.Visible = false;
                panel5.Visible = false;
                rprecios.Visible = false;
                rguias.Visible = false;
                rcortedecaja.Visible = false;
                rtarjetas.Visible = false;
                rprecios.Visible = false;

                taquilla.Visible = false;
                cortecaja.Visible = false;
                cambioautobus.Visible = false;

                rpagos.Visible = false;
                pagarguia.Visible = false;

                reporterutas.Visible = false;
                reportehorarios.Visible = false;
                if (LoginInfo.privilegios.Any(x => x == "Pagar Guias"))
                {
                    mostrar = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Boletos Cancelados"))
                {
                    panel5.Visible = true;

                }

                if (LoginInfo.privilegios.Any(x => x == "Socios"))
                {
                    socios.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Reporte horarios"))
                {
                    reportehorarios.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Reporte Rutas"))
                {
                    reporterutas.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Usuario"))
                {
                    usuarios.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Permisos"))
                {
                    permisos.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Choferes"))
                {
                    conductores.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Notificaciones"))
                {
                    notificar.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Quejas"))
                {
                    quejas.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Infracciones"))
                {
                    infracciones.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Sucursales"))
                {
                    sucursales.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Autobuses"))
                {
                    autobuses.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Roles"))
                {
                    roles.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Autobuses Diseño"))
                {
                    diseño.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Lineas"))
                {
                    lineas.Visible = true;

                }

                if (LoginInfo.privilegios.Any(x => x == "Destinos"))
                {
                    destinos.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Rolado"))
                {
                    rolado.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Corridas"))
                {
                    corridas.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Pasajes"))
                {
                    pasaje.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Tarjetas"))
                {
                    tarjetas.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Zonas"))
                {
                    zonas.Visible = true;

                }

                if (LoginInfo.privilegios.Any(x => x == "Rutas"))
                {
                    rutas.Visible = true;

                }



                if (LoginInfo.privilegios.Any(x => x == "Reporte de precios"))
                {
                    rprecios.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Reporte de Pagos"))
                {
                    rpagos.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Reporte de tarjetas"))
                {
                    rtarjetas.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Reporte de Guias"))
                {
                    rguias.Visible = true;

                }
                if (LoginInfo.privilegios.Any(x => x == "Reporte de Caja"))
                {
                    rcortedecaja.Visible = true;

                }


                if (LoginInfo.privilegios.Any(x => x == "Corte de Caja"))
                {
                    cortecaja.Visible = true;

                }

                if (LoginInfo.privilegios.Any(x => x == "Cambio de Autobus"))
                {
                    cambioautobus.Visible = true;

                }

                if (LoginInfo.privilegios.Any(x => x == "Taquilla"))
                {
                   taquilla.Visible = true;

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "PERMISOS";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        public void ActivarMenu()
        {
            try
            {
            
                //  menuStrip1.Visible = true;
                // permisos();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "activarmenu";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        public void DesactivarMenu()
        {

          //  menuStrip1.Visible = false;

        }

      /*  public void SplashStart()
        {
            
                Application.Run(new Splash());
            try
            {
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "splashstart";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

    */
        private bool CheckOpened(string name)
        {

            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    form = frm;
                    return true;
                }
            }
            return false;
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                checkInternetAvaible();
                if (CheckOpened("Usuarios"))
                {
                    form.WindowState = FormWindowState.Maximized;
                    // form.Dock = DockStyle.Fill;
                    form.Show();
                    form.Focus();

                }
                else
                {

                    form = new Usuarios();
                    //form.MdiParent = this;
                    //form.WindowState = FormWindowState.Maximized;
                    //form.Dock = DockStyle.Fill;

                    //form.Show();
                    Utilerias.LOG.acciones("ingresar a usuarios");
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "usuariostool";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Roles"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Roles();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a roles");

            }

        }


        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Esta accion terminara la aplicación",
                                    "Salir del programa",
                                    MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {

                System.Windows.Forms.Application.Exit();
                Utilerias.LOG.acciones("salir de la aplicación");

            }
            else
            {
                // If 'No', do something here.
            }

        }


        private void LineasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Lineas"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Lineas();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a lineas");

            }
        }


        private void ChoferesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Conductor"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Choferes();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a corte de choferes");

            }
        }

        private void SociosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Socios"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Socios();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a socios");

            }
        }

        private void PermisosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Permisos"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Permisos();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a permisos");

            }
        }

        private void SucursalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Sucursales"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Sucursales();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a sucursales");

            }
        }

        private void DestinosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Destinos"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Destinos();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a destino");

            }
        }

        private void TiposDeAutobusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Autobuses"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Autobusesform();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a autobuses");

            }
        }

        private void RutasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Rutas"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Rutas();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a rutas");

            }
        }

        private void ListaDePreciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Listadeprecios"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Listadeprecios();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a lista de precios");

            }
        }

        private void TiposDePasajesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("TiposdePasaje"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new TiposdePasaje();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a tipo de pasaje");

            }
        }
        private void RoladoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Rolado")) { 
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Rolado();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a rolado");

            }
        }

        private void AutobusesDiseñoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Diseño de Autobuses"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {

                form = new autobusdis();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a diseño de autobus");

            }
        }

        private void VentasToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //checkInternetAvaible();
            //if (CheckOpened("Ventas"))
            //{
            //    form.WindowState = FormWindowState.Maximized;
            //   // form.Dock = DockStyle.Fill;
            //    form.Show();
            //    form.Focus();

            //}
            //else
            //{

                form = new Ventas();
                //form.MdiParent = this;
               // form.WindowState = FormWindowState.Maximized;
               // form.Dock = DockStyle.Fill;
              // form.Show();
                Utilerias.LOG.acciones("ingresar a ventas");

           // }

        }

        private void CorridasToolStripMenuItem_Click(object sender, EventArgs e)
        {

            checkInternetAvaible();
            if (CheckOpened("Corridas"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {

                form = new Corridas();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a corridas");
                Utilerias.LOG.acciones("ingresar a corridas");

            }
        }

        private void ReporteDeGuiasToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened(" Reporte Guias"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {

                form = new Reporte_Guias();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a reporte de guias");

            }
        }

        private void ReporteCorteDeCajaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Reporte Corte de Caja"))

            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {

                form = new Reporte_Corte_de_Caja();


                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a reporte de caja");

            }
        }

        private void ConfiguraciónDeTarjetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Configuración de tarjetas"))

            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {

                form = new Configuración_de_tarjetas();


                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a reporte de configuración de tarjetas");

            }
        }

        private void ReporteDeTarjetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Reporte_de_tarjetas"))

            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {

                form = new Reporte_de_tarjetas();


                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a reporte de tarjetas");

            }
        }

        private void ReportesDeConductoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Reporte de Reclamos"))

            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {

                form = new Reporte_de_reclamos();


                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a reporte de reclamos");

            }
        }

        private void ReportesDeInfreccionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("infracciones"))

            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {

                form = new infracciones();


                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a reporte de infracciones");

            }
        }

        private void ZonasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Zonas"))

            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {

                form = new Zonas();


                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a zonas");

            }
        }

        private void NotificacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Notificaciones"))

            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {

                form = new Notificaciones();


                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a notificaciones");

            }
        }

        private void ConfiguraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
        
        }

        private void GuiasPagosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Pagos"))

            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {

                form = new PAGADOS();


                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("Guias Pagos");

            }
        }

        private void CambioDeAutobusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("cambio_de_autobus"))

            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {

                form = new cambio_de_autobus();


                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("Cambio de autobus");

            }
        }

 

        private void Label10_Click(object sender, EventArgs e)
        {

        }

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
          lbFecha.Text= DateTime.Now.ToString("D");


        }

        private void Panel77_Paint(object sender, PaintEventArgs e)
        {

        }

        private void admintrue()
        {
            usuarios.Visible = true;
            socios.Visible = true;
            conductores.Visible = true;
            sucursales.Visible = true;
            roles.Visible = true;
            autobuses.Visible = true;
            diseño.Visible = true;
            permisos.Visible = true;
            notificar.Visible = true;
            quejas.Visible = true;
            infracciones.Visible = true;
        }
 
        private void BtnListaClientes_Click(object sender, EventArgs e)
        {
            panelplaneacion.Visible = false;
            panelreportes.Visible = false;
            panelconfiguracion.Visible = false;
            panelventa.Visible = false;
           
            pagarguia.Visible = false;

        }
        private void Button5_Click(object sender, EventArgs e)
        {
            panelplaneacion.Visible = true;
            panelreportes.Visible = false;
            panelventa.Visible = false;
            if(mostrar==true)
            pagarguia.Visible = true;
            panelconfiguracion.Visible = false;


        }

        private void Button2_Click(object sender, EventArgs e)
        {
            panelconfiguracion.Visible = false;
            panelventa.Visible = false;
            panelplaneacion.Visible = true;
            pagarguia.Visible = false;
            panelreportes.Visible = true;
     
        }

        private void venta(object sender, EventArgs e)
        {
            panelconfiguracion.Visible = false;
            panelreportes.Visible = true;
            panelplaneacion.Visible = true;
            pagarguia.Visible = false;
            panelventa.Visible = true;
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            panelventa.Visible = true;
            pagarguia.Visible = false;
            panelreportes.Visible = true;
            panelplaneacion.Visible = true;

            panelconfiguracion.Visible = true;       


        }


        private void PopulateInstalledPrintersCombo()
        {
            // Add list of installed printers found to the combo box.
            // The pkInstalledPrinters string will be used to provide the display string.
            String pkInstalledPrinters;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                comboimpresora.Items.Add(pkInstalledPrinters);
            }
            
            comboimpresora.Text = Settings1.Default.impresora;

        }

        private void comboInstalledPrinters_SelectionChanged(object sender, System.EventArgs e)
        {

            // Set the printer to a printer in the combo box when the selection changes.

            if (comboimpresora.SelectedIndex != -1)
            {

                try
                {
                    // The combo box's Text property returns the selected item's text, which is the printer name.
                    impresoraname = comboimpresora.Text;
                    string dato = Settings1.Default.impresora;

                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    AppSettingsSection app = config.AppSettings;
                    Settings1.Default.impresora = impresoraname;
                    Settings1.Default.Save();
                    dato = Settings1.Default.impresora;


                    probar();
                }
                catch (Exception err)
                {
                    string error = err.Message;
                    MessageBox.Show("Impresora no conectada.");
                    string funcion = "llenarticket";
                    Utilerias.LOG.write("configuracion", funcion, error);


                }
            }
           

        }
        void llenarticket(object sender, PrintPageEventArgs e)
        {
            try
            {
                BarcodeLib.Barcode Codigobarras = new BarcodeLib.Barcode();
                Codigobarras.IncludeLabel = true;
                Graphics g = e.Graphics;
                Graphics g2 = e.Graphics;

                // Create solid brush.
                SolidBrush blueBrush = new SolidBrush(Color.Black);

                // Create rectangle for region.
                Rectangle fillRect = new Rectangle(0, 12, 230, 15);

                // Create region for fill.
                Region fillRegion = new Region(fillRect);

                // Fill region to screen.
                // g.DrawRectangle(Pens.Black, 3, 5, 340, 700);
                //g.DrawImage(imagensplash, 0, 0);
                Font fBody9 = new Font("Agency FB", 9, FontStyle.Regular);
                Font fBody7 = new Font("Agency FB", 7, FontStyle.Regular);

                Font fBody = new Font("Agency FB", 8, FontStyle.Regular);
                Font fBody10 = new Font("Agency FB", 10, FontStyle.Italic);
                Font fBody12 = new Font("Agency FB", 12, FontStyle.Bold | FontStyle.Italic);
                Font fBody18 = new Font("Agency FB", 14, FontStyle.Bold | FontStyle.Italic);
                Font fBody5 = new Font("Agency FB", 6, FontStyle.Regular);

                int espacio = 0;
                Color customColor = Color.FromArgb(255, Color.Black);
                Color customColor2 = Color.FromArgb(255, Color.White);

                SolidBrush sb = new SolidBrush(customColor);
                SolidBrush sb2 = new SolidBrush(customColor2);

                g.DrawString("Fecha: " + DateTime.Now.ToShortDateString(), fBody7, sb, 0, espacio);
                g.DrawString("Hora: " + DateTime.Now.ToShortTimeString(), fBody7, sb, 190, espacio);
                espacio = espacio + 10;
                g.DrawString("Prueba", fBody18, sb, 90, espacio);

                g.Dispose();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Impresora no conectada.");
                string funcion = "llenarticket";
                Utilerias.LOG.write("configuracion", funcion, error);


            }



        }

        private void probar()
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                PaperSize ps = new PaperSize("", 420, 30);
                pd.PrintPage += new PrintPageEventHandler(llenarticket);
                pd.PrintController = new StandardPrintController();

                pd.DefaultPageSettings.Margins.Left = 0;
                pd.DefaultPageSettings.Margins.Right = 0;
                pd.DefaultPageSettings.Margins.Top = 0;
                pd.DefaultPageSettings.Margins.Bottom = 0;
                pd.DefaultPageSettings.PaperSize = ps;
                pd.PrinterSettings.PrinterName = impresoraname;
                pd.Print();

                CrearTicket ticket = new CrearTicket();
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.CortaTicket();
                ticket.ImprimirTicket(Settings1.Default.impresora);

            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("No se detecto impresora.");
                string funcion = "buttonimprimir";
                Utilerias.LOG.write("configuracion", funcion, error);


            }
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            
                this.WindowState = FormWindowState.Minimized;
            

        }

        private void BtnCerrar_Click(object sender, EventArgs e)

        {
            Form mensaje = new Mensaje("¿Está seguro de cerrar la aplicación?",false);

            DialogResult resut = mensaje.ShowDialog();
            if (resut == DialogResult.OK)
            {
               
                Application.Exit();
            }
        }

      

        private void Timer2_Tick(object sender, EventArgs e)
        {
           // this.Hide(); // Oculta la forma actual

            timer2.Stop();
        }

        private void Panel51_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Panelconfiguracion_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button7_Click(object sender, EventArgs e)
        {
      

                try
                {
                Form mensaje = new Mensaje("¿Está seguro de cerrar sesión?",false);

                DialogResult resut = mensaje.ShowDialog();
                if (resut == DialogResult.OK)
                {


                    for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
                    {
                        if (Application.OpenForms[i].Name != "Splash")
                            Application.OpenForms[i].Close();
                    }

                    Form form = new Login();
                    form.MdiParent = this.MdiParent;
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                    this.Close();
                }

            }
            catch (Exception err)
                {
                    string error = err.Message;
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                    string funcion = "closestrip";
                    Utilerias.LOG.write(_clase, funcion, error);


                }

            

        }
        //
        // Declaraciones del API de Windows (y constantes usadas para mover el form)
        //
        const int WM_SYSCOMMAND = 0x112;
        const int MOUSE_MOVE = 0xF012;
        //
        // Declaraciones del API
        [System.Runtime.InteropServices.DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        //
        [System.Runtime.InteropServices.DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        //
        // función privada usada para mover el formulario actual
        private void moverForm()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0); ReleaseCapture();

        }

        private void Panelplaneacion_Paint(object sender, PaintEventArgs e)
        {

        }
        private void BtnNormal_Click(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Normal;

            btnNormal.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void BtnMaximizar_Click(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;


            btnMaximizar.Visible = false;
            btnNormal.Visible = true;
        }

        private void Panel51_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void PictureBox7_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Corte_de_Caja"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Corte_de_Caja();
                //form.MdiParent = this;
                form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                form.Show();
                Utilerias.LOG.acciones("ingresar a corte de caja");

            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Corte de Caja"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Corte_de_Caja();
                //form.MdiParent = this;
                form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                form.Show();
                Utilerias.LOG.acciones("ingresar a cprte de caja");

            }
        }

        private void Panelreportes_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Pago de Guias"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new pagoguias();
                //form.MdiParent = this;
                form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                form.Show();
                Utilerias.LOG.acciones("ingresar a pago de pagoguias");

            }
        }

        private void PanelBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            moverForm();
        }

        private void pictureBoxactualizar_Click(object sender, EventArgs e)
        {
            panelventa.Visible = true;
            pagarguia.Visible = false;
            panelreportes.Visible = true;
            panelplaneacion.Visible = true;

            panelconfiguracion.Visible = true;
        }

        private void buttonactualizar_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(ligaactualizacion);
            form = new update();
            form.Show();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Stop();
            verificaractualizacion();
            backgroundWorker1.RunWorkerAsync();
            mostraractualizacion();
        }


        private void Main_ChangeUICues(object sender, UICuesEventArgs e)
        {
            timer3.Start();
        }

        private void pictureBoxrutasdetalle_Click(object sender, EventArgs e)
        {
         
                checkInternetAvaible();
                if (CheckOpened("Reportedetallerutas"))
                {
                    form.WindowState = FormWindowState.Maximized;
                    // form.Dock = DockStyle.Fill;
                    form.Show();
                    form.Focus();

                }
                else
                {
                    form = new Reportedetallerutas();
                    //form.MdiParent = this;
                    //form.WindowState = FormWindowState.Maximized;
                    //form.Dock = DockStyle.Fill;
                    //form.Show();
                    Utilerias.LOG.acciones("ingresar a Reportedetallerutas");

                }
            

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("Reportehorarios"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new Reportehorarios();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a Reportehorarios");

            }
        }

        private void panelventa_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            checkInternetAvaible();
            if (CheckOpened("BoletosCancelados"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new BoletosCancelados();
                //form.MdiParent = this;
                //form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                //form.Show();
                Utilerias.LOG.acciones("ingresar a BoletosCancelados");

            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            OBTENERCHOFERESEJECUTIVO();
            OBTENERCHOFERESSUPRA();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            
                           checkInternetAvaible();
            if (CheckOpened("ReporteRolado"))
            {
                form.WindowState = FormWindowState.Maximized;
                // form.Dock = DockStyle.Fill;
                form.Show();
                form.Focus();

            }
            else
            {
                form = new ReporteRolado();
                //form.MdiParent = this;
                form.WindowState = FormWindowState.Maximized;
                //form.Dock = DockStyle.Fill;
                form.Show();
                Utilerias.LOG.acciones("ingresar a ReporteRolado");

            }
        }
    }

}
