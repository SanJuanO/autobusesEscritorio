using Autobuses.Planeacion;
using Autobuses.Reportes;
using ConnectDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses
{
    public partial class validarusuario : Form
    {

        string user = "";
        byte[] fingerPrint;
        List<string> usuario = new List<string>();
        public database db;
        ResultSet res = null;
        public string dest;
        public string orig;
        public string hor;
        public string lin;
        public string ec;
        public string impo;
        public string sali;
        public string antic;
        public string totall;
        public string casetaa;
        public string disel;
        public string vsedena;
        public string i;
        private int mayor;
        public string turnocosto;
        public string salidacosto;
        private string pasocosto;
        private bool turnoo;
        private bool salidaa;
        private bool pasoo;
        private string folio;
        private bool guia;

        public validarusuario(string destino, string origen, string hora, string linea, string eco,string imp, string dis, string caset,string vsed,string iva, string anti,string tota,int may,string turno,string salida,string paso,bool turn,bool sal,bool pass)
        {

            dest = destino;
            orig = origen;
            hor = hora;
            lin = linea;
            ec = eco;
            impo = imp;
            casetaa = caset;
            disel = dis;
            vsedena = vsed;
            i = iva;
            antic = anti;
            totall = tota;
            mayor = may;
            turnocosto = turno;
            salidacosto = salida;
            pasocosto = paso;
            turnoo = turn;
            salidaa = sal;
            pasoo = pass;
            InitializeComponent();
            verificationUserControl1.Stop();

            db = new database();
            ValidateUser();
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

        private void PanelBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            moverForm();
        }
        public validarusuario(string f)
        {
            
            InitializeComponent();
            guia=true;
            folio = f;
            db = new database();
            ValidateUser();
        }
    
        void ValidateUser()
        {

            try
            {

           

                    string cadenaDeConexion = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                    using (SqlConnection cn = new SqlConnection(cadenaDeConexion))
                    {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand("SELECT HUELLA FROM USUARIOS WHERE USUARIO='" +LoginInfo.UserID + "'", cn);
                        fingerPrint = (byte[])cmd.ExecuteScalar();
                        verificationUserControl1.Samples.Add(new DPFP.Template(new MemoryStream(fingerPrint)), null);
                    

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "validaruser";
                Utilerias.LOG.write("validarusuario", funcion, error);

            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                bool huella = verificationUserControl1.verificando();
                if (huella == true)
                {
                    int nombre = verificationUserControl1.nombre();


                    timer2.Enabled = false;
                    this.Close();

             


                    this.Close();
                }
            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "timer2";
                Utilerias.LOG.write("validarusuario", funcion, error);

            }
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
