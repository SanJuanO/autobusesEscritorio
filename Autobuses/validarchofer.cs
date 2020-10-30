
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
    public partial class validarchofer : Form
    {

        string user = "";
        byte[] fingerPrint;
        List<string> usuario = new List<string>();
        List<string> usuariochofer= new List<string>();
        List<string> usuariosocio = new List<string>();
        public database db;
        ResultSet res = null;
        public string dest;
        public string orig;
        public string hor;
        public string lin;
        private SqlCommand cmd;
        public string ec;
        public string impo;
        public string sali;
        public string antic;
        public string totall;
        public string casetaa;
        public string disel;
        public string vsedena;
        public string i;
        public string validador;
        private int mayor;
        private string chofe = "";
        private string turnocosto;
        private string salidacosto;
        private string pasocosto;
        private bool turnoo;
        private bool salidaa;
        private bool pasoo;
        private string folioguia;
        private bool reporte=false;
        public validarchofer(string destino, string origen, string hora, string linea, string eco, string imp, string dis, string caset, string vsed, string iva, string anti, string tota, string vali, int may, string turno, string salida, string paso, bool turn, bool sal, bool pass)
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
            validador = vali;
            mayor = may;
            turnocosto = turno;
            salidacosto = salida;
            pasocosto = paso;
            turnoo = turn;
            salidaa = sal;
            pasoo = pass;
            InitializeComponent();
            timer2.Enabled = true;
            db = new database();
            //ValidateUser();

        }

        public validarchofer(string fol)
        {

            InitializeComponent();
            timer2.Enabled = true;
            db = new database();
            folioguia = fol;
            ValidateUserGUIA();
            reporte = true;
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
        void ValidateUser()
        {

            try
            {


                string sql = "SELECT PK_CHOFER FROM AUTOBUSES WHERE ECO=@ECO ";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ECO", ec);
                res = db.getTable();
                if (res.Next())
                {
                    chofe = res.Get("PK_CHOFER");
                }


                string cadenaDeConexion = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                using ( SqlConnection cn = new SqlConnection(cadenaDeConexion))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT HUELLA FROM CHOFERES WHERE USUARIO='" + chofe + "'", cn);
                    fingerPrint = (byte[])cmd.ExecuteScalar();
                    verificationUserControl1.Samples.Add(new DPFP.Template(new MemoryStream(fingerPrint)), null);
                    user = chofe;
                }
               
            }
            // ValidateCHOFER();

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error chofer, intente de nuevo.");
                string funcion = "validaruser";
                Utilerias.LOG.write("validarchofer", funcion, error);

            }

        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            try
            {

                    bool huella = verificationUserControl1.verificando();
                   // if (huella == true)
                   if ( true)
                    {
                        int nombre = verificationUserControl1.nombre();


                        timer2.Enabled = false;
                    

                    //GUIA padre = Owner as GUIA;
                    //padre.cerrartodo();
                    DialogResult = DialogResult.OK;
                    
                    this.Close();
                  
                    // guia.imprimirticket(true, chofe, validador);


                }



            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error cjpfer, intente de nuevo.");
                string funcion = "timmer2";
                Utilerias.LOG.write("validarchofer", funcion, error);

            }
        }





        void ValidateUserGUIA()
        {

            try
            {


                string sql = "SELECT USUARIO FROM SOCIOS ";


                db.PreparedSQL(sql);



                res = db.getTable();
                while (res.Next())
                {
                    usuariosocio.Add(res.Get("USUARIO"));
                }
                 sql = "SELECT USUARIO FROM CHOFERES ";

                db.PreparedSQL(sql);


                res = db.getTable();
                while (res.Next())
                {
                    usuariochofer.Add(res.Get("USUARIO"));
                }
                int cantotal = usuariochofer.Count() + usuariosocio.Count();
                for (int i = 0; i < cantotal; i++)
                {


                    string cadenaDeConexion = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                    using (SqlConnection cn = new SqlConnection(cadenaDeConexion))
                    {
                        
                        cn.Open();
                        if (i < usuariosocio.Count())
                        {
                             cmd = new SqlCommand("SELECT HUELLA FROM SOCIOS WHERE USUARIO='" + usuariosocio[i] + "'", cn);
                            fingerPrint = (byte[])cmd.ExecuteScalar();
                            verificationUserControl1.Samples.Add(new DPFP.Template(new MemoryStream(fingerPrint)), null);
                            user = usuariosocio[i];
                        }
                        else
                        {
                            if (i < usuariochofer.Count())
                            {
                                cmd = new SqlCommand("SELECT HUELLA FROM CHOFERES WHERE USUARIO='" + usuariochofer[i] + "'", cn);
                                fingerPrint = (byte[])cmd.ExecuteScalar();
                                verificationUserControl1.Samples.Add(new DPFP.Template(new MemoryStream(fingerPrint)), null);
                                user = usuariochofer[i];
                            }
                        }
                       


                    }

                }
                // ValidateCHOFER();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error chofer, intente de nuevo.");
                string funcion = "CREAR";
                Utilerias.LOG.write("validarusuario", funcion, error);

            }      
        }



    }
}
