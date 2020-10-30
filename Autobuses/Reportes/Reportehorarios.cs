using Autobuses.Utilerias;
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Reportes
{
    public partial class Reportehorarios : Form
    {

        public database db;
        ResultSet res = null;
        private string _clase = "reportedetalleruta";
        private string linea = "";
        private string fechainicio;
        private bool inicio=true;
        Dictionary<string, double> dict = new Dictionary<string, double>();

        private string origen = "";
        private string destino = "";

        public Reportehorarios()
        {
            InitializeComponent();
            this.Show();
        }
        private void getDatosAdicionales()
        {
            try
            {

                string sql = "SELECT DESTINO,PK1 FROM DESTINOS WHERE BORRADO=0 ORDER BY DESTINO";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("DESTINO");
                    item.Value = res.Get("PK1");
                    comboBoxOrigen.Items.Add(item);
                    comboDestino.Items.Add(item);

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }

                sql = "SELECT LINEA,PK1 FROM LINEAS WHERE BORRADO=0 ORDER BY LINEA";
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
                string funcion = "getDatosAdicionales";
                Utilerias.LOG.write(_clase, funcion, error);
                if (!Utilerias.Utilerias.CheckForInternetConnection()) { 
                    MessageBox.Show("¡Verifique su conexiòn a internet!");
                }
                else
                {
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                }

            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            fechainicio = DateTime.Now.ToString("dd/MM/yyyy");
            getDatosAdicionales();
            dateTimePicker1.Value = DateTime.Now;

        }
        private void Reportehorarios_Shown(object sender, EventArgs e)
        {

            db = new database();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            titulo.Text = "Detalle de horarios";
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
      
            timer1.Start();
            timer2.Start();

            DoubleBufferedd(datagriddetalle, true);
        

        }
        private void DoubleBufferedd(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }


        private void agregarcolum()
        {
            string sql = "SELECT PASAJE,PORCENTAJE FROM TIPODEPASAJE WHERE BORRADO=0 AND PKLINEA=@LINEA";
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@LINEA", linea);

            res = db.getTable();
            progressBar1.Increment(20);

            while (res.Next())
            {
                datagriddetalle.Columns.Add(res.Get("PASAJE"), res.Get("PASAJE"));

                dict.Add(res.Get("PASAJE"), res.GetDouble("PORCENTAJE"));
            }

        }

        private void getRows()
        {
            progressBar1.Visible = true;
            datagriddetalle.Rows.Clear();
            string sql;
            progressBar1.Visible = true;

            sql = "SELECT LINEA,ORIGEN,DESTINO,FECHA,SALIDA,LLEGADA,PRECIO,KMS,TIEMPO FROM VCORRIDAS_DIA_PRECIOS WHERE (COMPLETO=1 OR ESCALA=1) ";

            if (!string.IsNullOrEmpty(linea)) 
            {
                sql += " AND PK_LINEA=@LINEA";
            }
            if (!string.IsNullOrEmpty(origen))
            {
                sql += " AND PK_ORIGEN=@ORIGEN";
            }
            if (!string.IsNullOrEmpty(destino))
            {
                sql += " AND PK_DESTINO=@DESTINO";
            }
            if (!string.IsNullOrEmpty(fechainicio))
            {
                sql += " AND CONVERT(DATETIME, FECHA,103)=CONVERT(DATETIME, @FECHA,103)";
            }

            sql += " GROUP BY LINEA,ORIGEN,DESTINO,FECHA,SALIDA,LLEGADA,PRECIO,KMS,TIEMPO ORDER BY SALIDA";

            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@LINEA", linea);
            db.command.Parameters.AddWithValue("@ORIGEN", origen);
            db.command.Parameters.AddWithValue("@DESTINO", destino);
            db.command.Parameters.AddWithValue("@FECHA", fechainicio);


            int n = 0;


            res = db.getTable();

            progressBar1.Maximum=res.Count;
            while (res.Next())
            {
                progressBar1.Increment(1);

                n = datagriddetalle.Rows.Add();

                //datagriddetalle.Rows[n].Cells["pkname"].Value = res.Get("PK");
                datagriddetalle.Rows[n].Cells["Lineaname"].Value = res.Get("LINEA");

                datagriddetalle.Rows[n].Cells["origenname"].Value = res.Get("ORIGEN");
                datagriddetalle.Rows[n].Cells["Destinoname"].Value = res.Get("DESTINO");
                datagriddetalle.Rows[n].Cells["fechaname"].Value = res.Get("FECHA");
               string sal=res.Get("SALIDA");
                datagriddetalle.Rows[n].Cells["salidaname"].Value = res.Get("SALIDA");
                datagriddetalle.Rows[n].Cells["llegadaname"].Value = res.Get("LLEGADA");

                datagriddetalle.Rows[n].Cells["Precioname"].Value = "$" + res.Get("PRECIO");
                double precio = res.GetDouble("PRECIO");

                datagriddetalle.Rows[n].Cells["kmname"].Value = res.Get("KMS");
                datagriddetalle.Rows[n].Cells["Tiemponame"].Value = res.Get("TIEMPO");

                for (
                    int i = 0; i < dict.Count; i++)
                {
                    double temp = precio * (dict.Values.ElementAt(i) / 100);
                    int condescuento = (Convert.ToInt32(Math.Floor(precio - temp)));

                    datagriddetalle.Rows[n].Cells[dict.Keys.ElementAt(i)].Value = "$ " + condescuento.ToString();
                }
            }
            progressBar1.Increment(20);
            progressBar1.Visible = false;
            progressBar1.Value = 0;



        }


        private DataTable Populate(string sqlCommand)
        {
            SqlConnection northwindConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);
            northwindConnection.Open();

            SqlCommand command = new SqlCommand(sqlCommand, northwindConnection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = command;

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            adapter.Fill(table);

            return table;
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
            
            for (int i = 0; i < dict.Count; i++)
            {
                datagriddetalle.Columns.Remove(dict.Keys.ElementAt(i));
            }
            dict.Clear();
            origen = (comboBoxOrigen.SelectedItem != null) ? (comboBoxOrigen.SelectedItem as ComboboxItem).Value.ToString() : "";
                if (!string.IsNullOrEmpty(linea) && !string.IsNullOrEmpty(fechainicio) && !string.IsNullOrEmpty(origen) && !string.IsNullOrEmpty(destino))
            {
                agregarcolum();
                getRows();
            }
            
        }

        private void comboboxlinea_SelectedIndexChanged(object sender, EventArgs e)
        {
                
            for (int i = 0; i < dict.Count; i++)
            {
                datagriddetalle.Columns.Remove(dict.Keys.ElementAt(i));
            }
            dict.Clear();
            linea = (comboboxlinea.SelectedItem != null) ? (comboboxlinea.SelectedItem as ComboboxItem).Value.ToString() : "";

            if (!string.IsNullOrEmpty(linea) && !string.IsNullOrEmpty(fechainicio) && !string.IsNullOrEmpty(origen) && !string.IsNullOrEmpty(destino))
            {
                agregarcolum();
                getRows();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
        
            comboBoxOrigen.Text = "";
            origen = "";
            getRows();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(datagriddetalle,new List<string> { "pkname" });

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Increment(20);
                for (int i = 0; i < dict.Count; i++)
                {
                    datagriddetalle.Columns.Remove(dict.Keys.ElementAt(i));
                }
                dict.Clear();

                //linea = (comboboxlinea.SelectedItem != null) ? (comboboxlinea.SelectedItem as ComboboxItem).Value.ToString() : "";
                fechainicio = dateTimePicker1.Value.ToString("dd/MM/yyyy");

                if (!string.IsNullOrEmpty(linea) && !string.IsNullOrEmpty(fechainicio) && !string.IsNullOrEmpty(origen) && !string.IsNullOrEmpty(destino)) {
                    agregarcolum();
                    getRows();
                }
                progressBar1.Visible = false;
                progressBar1.Value = 0;

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datetimerpicker1";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void Reportehorarios_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;


            btnMaximizar.Visible = false;
            btnNormal.Visible = true;
        }

        private void comboDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            for (int i = 0; i < dict.Count; i++)
            {
                datagriddetalle.Columns.Remove(dict.Keys.ElementAt(i));
            }
            dict.Clear();

            //linea = (comboboxlinea.SelectedItem != null) ? (comboboxlinea.SelectedItem as ComboboxItem).Value.ToString() : "";
            destino = comboDestino.SelectedItem != null ? (comboDestino.SelectedItem as ComboboxItem).Value.ToString() : "";

            if (!string.IsNullOrEmpty(linea) && !string.IsNullOrEmpty(fechainicio) && !string.IsNullOrEmpty(origen) && !string.IsNullOrEmpty(destino))
            {
                agregarcolum();
                getRows();
            }
            
        }
    }
}
