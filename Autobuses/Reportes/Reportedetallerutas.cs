using Autobuses.Utilerias;
using ConnectDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Reportes
{
    public partial class Reportedetallerutas : Form
    {
        public database db;
        ResultSet res = null;
        private string _clase = "reportedetalleruta";
        private string linea="";
        private string origen="" ;
        Dictionary<string, double> dict = new Dictionary<string, double>();
        public Reportedetallerutas()
        {
            InitializeComponent();
            this.Show();
        }

        private void Reportedetallerutas_Shown(object sender, EventArgs e)
        {

            db = new database();
            getDatosAdicionales();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            titulo.Text = "Detalle de rutas";
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            timer1.Start();
            DoubleBufferedd(datagriddetalle,true);


        }
        private void agregarcolum()
        {
            string sql = "SELECT PASAJE,PORCENTAJE FROM TIPODEPASAJE WHERE BORRADO=0 AND PKLINEA=@LINEA";
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@LINEA", linea);

            res = db.getTable();
           
            while (res.Next())
            {
                datagriddetalle.Columns.Add(res.Get("PASAJE"), res.Get("PASAJE"));

                dict.Add(res.Get("PASAJE"), res.GetDouble("PORCENTAJE"));
            }

        }
        private void DoubleBufferedd(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
        private void getDatosAdicionales()
        {
            try
            {

                string sql = "SELECT DESTINO,PK1 FROM DESTINOS WHERE BORRADO=0";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("DESTINO");
                    item.Value = res.Get("PK1");
                    comboBoxOrigen.Items.Add(item);

                
                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }

                sql = "SELECT LINEA,PK1 FROM LINEAS WHERE BORRADO=0";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item2 = new ComboboxItem();
                    item2.Text = res.Get("LINEA");
                    item2.Value = res.Get("PK1");
                    comboboxlinea.Items.Add(item2);
                 

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }
                if (comboboxlinea.Items.Count > 0)
                {
                    comboboxlinea.SelectedIndex = 0;
                    linea = (comboboxlinea.SelectedItem != null) ? (comboboxlinea.SelectedItem as ComboboxItem).Value.ToString() : "";
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getDatosAdicionales";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
            private void getRows()
            {
            datagriddetalle.Rows.Clear();
            string sql;
            if (origen == "" && linea=="")
            {
                 sql = "SELECT * FROM VRUTAS_DETALLE";
                db.PreparedSQL(sql);
            }
            else if (linea =="" && origen!="")
            {
                sql = "SELECT * FROM VRUTAS_DETALLE WHERE PK_ORIGEN=@ORIGEN";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ORIGEN", origen);

            }
            else if (linea != "" && origen == "")
            {
                sql = "SELECT * FROM VRUTAS_DETALLE WHERE LINEA_PK=@LINEA";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@LINEA", linea);

            }
            else if (linea != "" && origen != "")
            {
                sql = "SELECT * FROM VRUTAS_DETALLE WHERE LINEA_PK=@LINEA AND PK_ORIGEN=@ORIGEN";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@LINEA", linea);
                db.command.Parameters.AddWithValue("@ORIGEN", origen);

            }


            int n = 0;


                res = db.getTable();

                while (res.Next())
                {
                n = datagriddetalle.Rows.Add();

                datagriddetalle.Rows[n].Cells["pkname"].Value = res.Get("PK");
                datagriddetalle.Rows[n].Cells["Rutaname"].Value = res.Get("RUTA");
                datagriddetalle.Rows[n].Cells["Descripcionname"].Value = res.Get("DESCRIPCION");
                datagriddetalle.Rows[n].Cells["Lineaname"].Value = res.Get("LINEA");
                datagriddetalle.Rows[n].Cells["Claveorigenname"].Value = res.Get("CLAVE_ORIGEN");
                datagriddetalle.Rows[n].Cells["origenname"].Value = res.Get("ORIGEN");
                datagriddetalle.Rows[n].Cells["Clavedestinoname"].Value = res.Get("CLAVE_DESTINO");
                datagriddetalle.Rows[n].Cells["Destinoname"].Value = res.Get("DESTINO");
                datagriddetalle.Rows[n].Cells["Precioname"].Value ="$"+ res.Get("PRECIO");
                double precio = res.GetDouble("PRECIO");

                datagriddetalle.Rows[n].Cells["kmname"].Value = res.Get("KMS");
                datagriddetalle.Rows[n].Cells["Tiemponame"].Value = res.Get("TIEMPO");

                for (
                    int i = 0; i < dict.Count; i++)
                {
                    double temp = precio *(dict.Values.ElementAt(i)/100);
                    int condescuento = (Convert.ToInt32(Math.Floor(precio - temp)));

                    datagriddetalle.Rows[n].Cells[dict.Keys.ElementAt(i)].Value = "$ " + condescuento.ToString();
                }
            }
     

            }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

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

        private void PanelBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            moverForm();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void comboBoxOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {

             origen = (comboBoxOrigen.SelectedItem != null) ? (comboBoxOrigen.SelectedItem as ComboboxItem).Value.ToString() : "";
            getRows();
        }

        private void comboboxlinea_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            for (int i = 0; i < dict.Count; i++) {
                datagriddetalle.Columns.Remove(dict.Keys.ElementAt(i));
                    }
            dict.Clear();
            linea = (comboboxlinea.SelectedItem != null) ? (comboboxlinea.SelectedItem as ComboboxItem).Value.ToString() : "";
            agregarcolum();
            getRows();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboboxlinea.Text = "";
            comboBoxOrigen.Text = "";
            linea = "";
            origen = "";

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Utilerias.Utilerias.ExportarDataGridViewExcel(datagriddetalle,new List<string> { "pkname" });

        }

        private void Reportedetallerutas_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;


            btnMaximizar.Visible = false;
            btnNormal.Visible = true;
        }
    }


}
