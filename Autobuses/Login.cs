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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConnectDB;
using MyAttendance;

namespace Autobuses
{
    public partial class Login : Form
    {
        loading loading;
        public database db;
        private string _usuario;
        private string _password;
        ResultSet res = null;
        string user = "";
        int progress = 0;
        byte[] fingerPrint;
        private string _clase = "login";
        bool internet = true;
        List<string> iList = new List<string>();
        bool validar=false;
        public Login()
        {
            InitializeComponent();
            LoginUsuario.Focus();
            groupBoxusuario.Visible = true;
            db = new database();
            progressBar1.Visible = false;
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

                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("Error al conectarse a internet");
                    return;
                }
                else
                {
              

                        string cadenaDeConexion = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                        using (SqlConnection cn = new SqlConnection(cadenaDeConexion))
                        {
                            cn.Open();
                            SqlCommand cmd = new SqlCommand("SELECT HUELLA FROM USUARIOS WHERE USUARIO='" + LoginUsuario.Text + "'", cn);
                            fingerPrint = (byte[])cmd.ExecuteScalar();
                            verificationUserControl.Samples.Add(new DPFP.Template(new MemoryStream(fingerPrint)), null);
                        }

                    
                }
            }
            catch (Exception err)
            {
                internet = false;
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "validaruser";
                Utilerias.LOG.write(_clase, funcion, error);

            }

        }


        private  void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                backgroundWorkerLogin.RunWorkerAsync();
            }

            catch (Exception err)
            {
            
      
            }
        }

        public void   mostrar ()
        {
            loading = new loading();

            loading.Show();
            loading.Visible = false;


        }
        public void terminar()
        {
            if (loading != null && loading.Visible)
                loading.Close();
        }

        private void LoginEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                backgroundWorkerLogin.RunWorkerAsync();
            }
        }

 
        public void ValidarAcceso()
        {
            try
            {
                if (progressBar1.InvokeRequired)
                {

                    progressBar1.Invoke(new Action(() =>
                    {
                        progressBar1.Visible = true;
                    }));
                }
                else
                {

                    progressBar1.Visible = true;
                }

                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("Error al conectarse a internet");
                    return;
                }
                else
                {

                    if (validar != true)
                        backgroundWorkerLogin.ReportProgress(20);

                    //mostrar();
                    _usuario = LoginUsuario.Text;
                    _password = LoginPassword.Text;
                    bool huella = verificationUserControl.verificando();
                    int nombre = verificationUserControl.nombre();
                    ResultSet res = null;
                    string sql = "SELECT * FROM USUARIOS WHERE USUARIO= @USER AND PASSWORD= @PASS AND ACTIVO=1";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@USER", _usuario);
                    db.command.Parameters.AddWithValue("@PASS", _password);
                    res = db.getTable();
                    if (validar != true)
                        backgroundWorkerLogin.ReportProgress(50);

                    if (huella == true)
                    {

                        progressBar1.Increment(70);

                        LoginInfo.ingreso = DateTime.Now.ToString();
                        LoginInfo.UserID = LoginUsuario.Text;

                        string sql2 = "SELECT PK,SUCURSAL,NOMBRE,APELLIDOS, ID,IMAG,ROLE FROM Vista1 WHERE USUARIO=@USUARIO AND ACTIVO=1";


                        db.PreparedSQL(sql2);
                        db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                        res = db.getTable();
                        if (res.Next())
                        {

                            LoginInfo.PkUsuario = res.Get("PK");
                            LoginInfo.pkidroles = res.Get("ID");
                            LoginInfo.Sucursal = res.Get("SUCURSAL");
                            LoginInfo.NombreID = res.Get("NOMBRE");
                            LoginInfo.ApellidoID = res.Get("APELLIDOS");
                            LoginInfo.imagenfoto =  Convert.FromBase64String(res.Get("IMAG"));
                            LoginInfo.rol = res.Get("ROLE");



                        }
                        progressBar1.Increment(90);

                        permisos();
                        Form mainn =new Main();
                        mainn.Show();
                        mainn.Focus();
                        if (validar != true)
                            backgroundWorkerLogin.ReportProgress(90);

                        //this.Close();


                    }
                    else if (res.HasRows)
                    {

                        LoginInfo.UserID = _usuario;

                        string sql2 = "SELECT PK, SUCURSAL,NOMBRE,APELLIDOS, ID,IMAG,ROLE FROM Vista1 WHERE USUARIO=@USUARIO AND ACTIVO=1";


                        db.PreparedSQL(sql2);
                        db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                        res = db.getTable();
                        if (res.Next())
                        {

                            LoginInfo.PkUsuario = res.Get("PK");
                            LoginInfo.pkidroles = res.Get("ID");
                            LoginInfo.Sucursal = res.Get("SUCURSAL");
                            LoginInfo.NombreID = res.Get("NOMBRE");
                            LoginInfo.ApellidoID = res.Get("APELLIDOS");
                            LoginInfo.imagenfoto = Convert.FromBase64String(res.Get("IMAG"));
                            LoginInfo.rol = res.Get("ROLE");



                        }
                        permisos();

                        if (this.InvokeRequired)
                            this.Invoke(new Action(() =>
                            {
                                Form mainn = new Main();
                                mainn.Show();
                                mainn.Focus();
                                this.Close();

                            }));
                        else
                        {
                            backgroundWorkerLogin.ReportProgress(90);

                            Form mainn = new Main ();
                            mainn.Show();
                            mainn.Focus();
                            this.Close();
                        }
                            


                      
                       
                    



                    }
                    else
                    {
                        if (validar != true)
                            backgroundWorkerLogin.ReportProgress(90);

                        Form mensaje = new Mensaje("Contraseña o usuario incorrecto",true);

                        DialogResult resut = mensaje.ShowDialog();
                      
                        if (progressBar1.InvokeRequired)
                        {

                            progressBar1.Invoke(new Action(() =>
                            {
                                progressBar1.Visible = false;
                            }));
                        }
                        else
                        {

                            progressBar1.Visible = false;
                        }
                        if (LoginPassword.InvokeRequired)
                        {
                            LoginPassword.Invoke(new Action(() =>
                            {
                                LoginPassword.Text = "";

                            }));
                        }
                        else
                        {
                            LoginPassword.Text = "";

                        }
                        if (LoginUsuario.InvokeRequired)
                        {

                            LoginUsuario.Invoke(new Action(() =>
                            {
                          

                            }));
                        }
                        else
                        {
                            LoginUsuario.Text = "";
                            btnIngresar.Enabled = true;
                            btnIngresar.Visible = true;
                        }
                      
                    }

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "validaracceso";
                Utilerias.LOG.write(_clase, funcion, error);
               


            }
        }



        private void permisos()
        {
            try
            {
                string sql2 = "SELECT PRIVILEGIO FROM VISTAROLPRIV WHERE ID=@ID AND PRIV IS NOT NULL";


                db.PreparedSQL(sql2);
                db.command.Parameters.AddWithValue("@ID", LoginInfo.pkidroles);

                LoginInfo.privilegios.Clear();

                res = db.getTable();
                while (res.Next())
                {
                    LoginInfo.privilegios.Add(res.Get("PRIVILEGIO"));

                }

                db.execute();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "permisos";
                Utilerias.LOG.write(_clase, funcion, error);
            }
        }
       
        private void Timer2_Tick(object sender, EventArgs e)
            { 
            try
            {

                bool huella = verificationUserControl.verificando();
                if (huella == true)
                {
                    progressBar1.Visible = true;

                  
                    validar = true;
                    timer2.Stop();


                    ValidarAcceso();
                    progressBar1.Increment(100);

                    this.Close();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "timer";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }
        public class NewProgressBar : ProgressBar
{
    public NewProgressBar()
    {
        this.SetStyle(ControlStyles.UserPaint, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Rectangle rec = e.ClipRectangle;

        rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
        if(ProgressBarRenderer.IsSupported)
           ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
        rec.Height = rec.Height - 4;
        e.Graphics.FillRectangle(Brushes.Red, 2, 2, rec.Width, rec.Height);
    }
}void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            ValidarAcceso();
            backgroundWorkerLogin.ReportProgress(100);
            progressBar1.Value = 10;


            //if (loading.InvokeRequired)
            //    loading.Invoke(new Action(() => { loading.Visible = false; }));
            //else
            //    loading.Visible = false;

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
           
        }

        private void BackgroundWorkerLogin_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
         
            progress = e.ProgressPercentage;
            progressBar1.Increment(progress);
            if (progress == 100)
            {
                progressBar1.Value = 0;

            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            db = new database();
        }

        private void Login_Shown(object sender, EventArgs e)
        {

            this.Width = 275;
            this.Height = 265;
            this.CenterToScreen();
            LoginUsuario.Focus();
  
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Width = 275;
            this.Height = 270;
            this.CenterToScreen();
            groupBoxusuario.Visible = true;
            groupBoxingresar.Visible = false;
        }

        private void GroupBox1_BackColorChanged(object sender, EventArgs e)
        {
        }

        private void VerificationUserControl_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void buttonusuario_Click(object sender, EventArgs e)
        {
            try
            {
                buttonusuario.Enabled = false;
                _usuario = LoginUsuario.Text;

                string sql = "SELECT NOMBRE,APELLIDOS,SUCURSAL,IMAG FROM VISTA1 WHERE ACTIVO=1 AND USUARIO=@USUARIO";


                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@USUARIO", LoginUsuario.Text);
                res = db.getTable();
                if (res.Next())
                {
                    labelnombre.Text = res.Get("NOMBRE") +" "+ res.Get("APELLIDOS");
                    labelsucursal.Text = res.Get("SUCURSAL");
                    byte[] imagen = (!string.IsNullOrEmpty(res.Get("IMAG"))) ? Convert.FromBase64String(res.Get("IMAG")) : null;

                    pictureBox1.Image = (imagen != null) ? Image.Bytes_A_Imagen((byte[]) imagen) : null;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                    groupBoxusuario.Visible = false;
                    groupBoxingresar.Visible = true;
                    this.Width = 450;
                    this.Height = 385;
                    ValidateUser();
                    LoginPassword.Focus();
                    buttonusuario.Enabled = true;
             

                }
                else
                {
                    buttonusuario.Enabled = true;
                    error.Visible = true;   
                    error.Text = "Usuario incorrecto";
                }
                this.CenterToScreen();

            }
            catch (Exception err)
            {
                internet = false;
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "ingresar";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form mensaje = new Mensaje("¿Está seguro de cerrar la aplicación?", false);

            DialogResult resut = mensaje.ShowDialog();
            if (resut == DialogResult.OK)
            {

                Application.Exit();
            }
        }

        private void LoginUsuario_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void LoginUsuario_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void LoginUsuario_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonusuario_Click(sender, e);
            }

        }

        private void LoginPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnIngresar_Click(sender, e);  
            }

        }
    }
}
