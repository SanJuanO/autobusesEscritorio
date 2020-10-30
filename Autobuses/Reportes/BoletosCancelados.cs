using Autobuses.Utilerias;
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
    public partial class BoletosCancelados : Form
    {
        private string _clase = "reportedetalleruta";
        private string linea = "";
        private string origen = "";
        private string fechainicio;
        public database db;
        ResultSet res = null;
        public BoletosCancelados()
        {
            InitializeComponent();
            db = new database();
            this.Show();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            titulo.Text = "Reporte de boletos cancelados";
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            timer1.Start();
            timer2.Start();
            this.WindowState = FormWindowState.Maximized;
            comboboxlinea.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxOrigen.DropDownStyle = ComboBoxStyle.DropDownList;


            btnMaximizar.Visible = false;
            btnNormal.Visible = true;

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
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡Verifique su conexiòn a internet!");
                }
                else
                {
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                }

            }
        }

        private void getRows()
        {
            try
            {

                datagriddetalle.Rows.Clear();
                string sql;

                sql = "SELECT PK,FECHA,FOLIO,LINEA,ORIGEN,DESTINOBOLETO,VENDEDOR,CANCELADO,HORA,TARIFA,PRECIO,STATUS,MOTIVOCANCELADO,SUCURSAL FROM VENDIDOS WHERE STATUS='CANCELADO' ";

                if (!string.IsNullOrEmpty(linea))
                {
                    sql += " AND LINEA=@LINEA";
                }
                if (!string.IsNullOrEmpty(origen))
                {
                    sql += " AND SUCURSAL=@ORIGEN";
                }

                if (!string.IsNullOrEmpty(fechainicio))
                {
                    sql += " AND CONVERT(DATETIME, FECHA,103)=CONVERT(DATETIME, @FECHA,103)";
                }


                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@LINEA", linea);
                db.command.Parameters.AddWithValue("@ORIGEN", origen);
                db.command.Parameters.AddWithValue("@FECHA", fechainicio);


                int n = 0;


                res = db.getTable();

                while (res.Next())
                {

                    n = datagriddetalle.Rows.Add();

                    datagriddetalle.Rows[n].Cells["pkname"].Value = res.Get("PK");
                    datagriddetalle.Rows[n].Cells["lineaname"].Value = res.Get("LINEA");
                    datagriddetalle.Rows[n].Cells["folioname"].Value = res.Get("FOLIO");

                    datagriddetalle.Rows[n].Cells["destinoname"].Value = res.Get("DESTINOBOLETO");
                    datagriddetalle.Rows[n].Cells["sucursalname"].Value = res.Get("SUCURSAL");
                    datagriddetalle.Rows[n].Cells["fechaname"].Value = res.Get("FECHA");
                    datagriddetalle.Rows[n].Cells["horaname"].Value = res.Get("HORA");
                    datagriddetalle.Rows[n].Cells["tarifaname"].Value = res.Get("TARIFA");
                    datagriddetalle.Rows[n].Cells["vendedorname"].Value = res.Get("VENDEDOR");
                    datagriddetalle.Rows[n].Cells["canceladoname"].Value = res.Get("CANCELADO");

                    datagriddetalle.Rows[n].Cells["precioname"].Value = "$" + res.Get("PRECIO");

                    datagriddetalle.Rows[n].Cells["statusname"].Value = res.Get("STATUS");
                    datagriddetalle.Rows[n].Cells["motivoname"].Value = res.Get("MOTIVOCANCELADO");

                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                string funcion = "getDatosAdicionales";
                Utilerias.LOG.write(_clase, funcion, error);
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
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
            fechainicio = DateTime.Now.ToString("dd/MM/yyyy");
            getDatosAdicionales();
            dateTimePicker1.Value = DateTime.Now;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            timer2.Stop();
            fechainicio = DateTime.Now.ToString("dd/MM/yyyy");
            getDatosAdicionales();
            dateTimePicker1.Value = DateTime.Now;
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

        private void PanelBarraTitulo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBoxOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            origen = comboBoxOrigen.SelectedItem.ToString();

            getRows();

        }

        private void comboboxlinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            linea = comboboxlinea.SelectedItem.ToString();

            getRows();

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            fechainicio = dateTimePicker1.Value.ToString("dd/MM/yyyy");

            getRows();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBoxOrigen.Text = "";
            origen = "";
            getRows();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(datagriddetalle, new List<string> { "pkname" });

        }
    }
}
