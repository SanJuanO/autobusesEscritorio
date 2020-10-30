using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Threading;
using System.Drawing.Imaging;
using System.IO;
using Autobuses.Administracion;
using AForge.Video.DirectShow;
using WindowsFormsApplication1;

namespace Autobuses
{
    public partial class TomarFoto : Form
    {
        string _dato;
        private string _clase = "tomarfoto";
        
   

        // Declare required methods
       WebCam webCam;




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

        public TomarFoto(string dato)
        {
            try
            {
                InitializeComponent();
                _dato = dato;
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                timer2.Start();


            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error tomar foto, intente de nuevo.");
                string funcion = "tomarfoto";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }

   
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                webCam.StopCaptura();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "button1";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image != null)
                {

                    if (_dato == "Usuarios")
                    {
                        Bitmap img = new Bitmap(pictureBox1.Image,320,240);
                        
                        Usuarios usuarios = Owner as Usuarios;

                        usuarios.AsignarFoto(img);

                        this.Close();

                    }
                    if (_dato == "Choferes")
                    {
                        Bitmap img = new Bitmap(pictureBox1.Image, 320, 240);

                        Choferes chofer = Owner as Choferes;

                        chofer.AsignarFoto(img);

                        this.Close();

                    }
                    if (_dato == "Socios")
                    {
                        Bitmap img = new Bitmap(pictureBox1.Image, 320, 240);

                        Socios socios = Owner as Socios;

                        socios.AsignarFoto(img);

                        this.Close();

                    }
                    if (_dato == "socioalterno")
                    {
                        this.WindowState = FormWindowState.Minimized;
                        Bitmap img = new Bitmap(pictureBox1.Image, 320, 240);

                        Socios socios = Owner as Socios;

                        socios.AsignarFotoalterno(img);

                        this.Close();

                    }
                    
                }
                else
                {
                    MessageBox.Show("El driver de la webcam no esta instalado .");

                }


            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "button2(seleccionarfoto)";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }

        private void TomarFoto_Load(object sender, EventArgs e)
        {
            webCam = new WebCam(1000, pictureBox1, comboBox1);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            actualizar();
        }

        private void actualizar()
        {
            comboBox1.Items.Clear();
            webCam.Listar();
            try
            {
                comboBox1.SelectedIndex = 0;
                webCam.EncenderCamara(Convert.ToInt16(comboBox1.SelectedIndex.ToString().Substring(0, 1)));

            }
            catch (Exception err)
            {
                string error = err.Message;
                Form mensaje = new Mensaje("No existe un dispositivo de camara donde se pueda conectar", true);

                mensaje.ShowDialog(); string funcion = "actualizar";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            actualizar();
        }

        private void TomarFoto_Shown(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            timer1.Start();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                webCam.EncenderCamara(Convert.ToInt16(comboBox1.SelectedIndex.ToString().Substring(0, 1)));

            }
            catch (Exception err)
            {
                string error = err.Message;
                //MessageBox.Show("Ocurrio un Error tomar foto, intente de nuevo.");
                string funcion = "button3";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCerrar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }
    }
}
