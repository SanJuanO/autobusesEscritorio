using Autobuses.Administracion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Autobuses
{
    public partial class Registration : Form
    {
        Byte[] dato;
        private string _clase = "registration";
        Bitmap imagen;
 
        string v;

        public Registration(string valor)
        {
            InitializeComponent();
            v = valor;
            this.Show();
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
        public void fingerPrintRegUserControl_RegistrationCompleted(object sender, StatusChangedEventArgs e)
        {
            try
            {
                dato = fingerPrintRegUserControl.datos();
                imagen = fingerPrintRegUserControl.imagenregreso();


                if (v == "socios")
                {
                    Socios soc = Owner as Socios;
                    soc.asignarhuella(dato, imagen);
                }
                if (v == "Usuarios")
                {
                    Usuarios us = Owner as Usuarios;
                    us.asignarhuella(dato, imagen);
             


                   
                }
                if (v == "Choferes")
                {
                   Choferes chof = Owner as Choferes;
                    chof.asignarhuella(dato, imagen);
                }
                if (v == "Socioalterno")
                {
                    Socios soc = Owner as Socios;
                    soc.asignarhuellaalterno(dato, imagen);
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "fingerprint";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }

        public void registerButton_Click(object sender, EventArgs e)
        {
   
        }

        private void Registration_Load(object sender, EventArgs e)
        {

        }

        private void Registration_Shown(object sender, EventArgs e)
        {
            //v = valor;

            this.fingerPrintRegUserControl.RegistrationCompletedStatusChanged += new StatusChangedEventHandler(fingerPrintRegUserControl_RegistrationCompleted);

        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
