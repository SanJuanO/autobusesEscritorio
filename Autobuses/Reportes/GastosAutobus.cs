using ConnectDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Reportes
{
    public partial class GastosAutobus : Form
    {
        private string fecha;
        private string eco;
        public database db;
        ResultSet res = null;
        private string _clase = "reportedetalleruta";


        public GastosAutobus(string _fecha,string _eco)
        {
            InitializeComponent();
            db = new database();

            fecha = _fecha;
            eco = _eco;

            getDatosAdicionales();
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            dataGridViewBOLETOS.Focus();
            timer1.Start();
        }
        private void getDatosAdicionales() {
            try
            {

                string sql = "SELECT GU.FECHA,GU.AUTOBUS,GU.CHOFER,GU.SUCURSAL,GU.ANTICIPO, CD.RUTA FROM GUIA GU" +
 " LEFT JOIN VCORRIDAS_DIA_3 CD ON(CD.PK = GU.PKCORRIDA) WHERE GU.AUTOBUS = @ECO AND GU.FECHA = @FECHA ORDER BY GU.PK DESC";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ECO", eco);
                db.command.Parameters.AddWithValue("@FECHA", fecha);

                double sumatoriagasto = 0;
                double sumatoriagastoultima = 0;

                res = db.getTable();
                int n = 0;
                string rutatemp;
              
                while (res.Next())
                {
                    n = dataGridViewBOLETOS.Rows.Add();


                    dataGridViewBOLETOS.Rows[n].Cells["fechaname"].Value = res.Get("FECHA");
                    dataGridViewBOLETOS.Rows[n].Cells["autobusname"].Value = res.Get("AUTOBUS");
                    dataGridViewBOLETOS.Rows[n].Cells["conductorname"].Value = res.Get("CHOFER");
                    dataGridViewBOLETOS.Rows[n].Cells["rutaname"].Value = res.Get("RUTA");
                    
                    dataGridViewBOLETOS.Rows[n].Cells["sucursalname"].Value = res.Get("SUCURSAL");
                    dataGridViewBOLETOS.Rows[n].Cells["gastoname"].Value = res.Get("ANTICIPO");
                    sumatoriagasto += res.GetDouble("ANTICIPO");

                    int nt = n - 1;
                    if (nt < 0) { nt = 0; }
                    if (dataGridViewBOLETOS.Rows[n].Cells["rutaname"].Value.ToString().Equals(dataGridViewBOLETOS.Rows[nt].Cells["rutaname"].Value.ToString()))
                    {

                        sumatoriagastoultima += res.GetDouble("ANTICIPO");
                    }


                }
                ocupadostext.Text = "$" + sumatoriagasto;
                textBoxultima.Text = "$" + sumatoriagastoultima;
            }
            catch (Exception err)
            {
                string error = err.Message;
                string funcion = "getdatosadicionales";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
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

    }
}
