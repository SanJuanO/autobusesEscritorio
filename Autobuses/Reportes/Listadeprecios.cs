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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Reportes
{
    public partial class Listadeprecios : Form
    {
        public database db;
        ResultSet res = null;
        private string _clase = "listadeprecios";
        private int n = 0;
        private string origen;
        private string linea;
   

        public Listadeprecios()
        {


            InitializeComponent();
            this.Show();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            titulo.Text = "Reporte de precios";
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            timer1.Start();
          

        }

        public void OBTENERDESTINOS()
        {
            try
            {
                datagridlistaprecios.Rows.Clear();
                datagridlistaprecios.Columns.Clear();

                IDictionary<string, int> pasajesdiccionario = new Dictionary<string, int>();
                int count = 1;
                string sql = "SELECT PASAJE FROM TIPODEPASAJE WHERE PKLINEA=@LINEA AND BORRADO=0";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@LINEA", linea);


                datagridlistaprecios.Columns.Add("Destinos", "Destinos");

                res = db.getTable();

                while (res.Next())
                {
                    pasajesdiccionario.Add(res.Get("PASAJE"), count);
                    datagridlistaprecios.Columns.Add(res.Get("PASAJE"), res.Get("PASAJE"));


                    count++;
                }

                IDictionary<string, int> destinodiccionario = new Dictionary<string, int>();
                count = 0;
                sql = "SELECT DESTINO FROM DESTINOS where borrado=0";

                db.PreparedSQL(sql);


                res = db.getTable();

                while (res.Next())
                {

                    pasajesdiccionario.Add(res.Get("DESTINO"), count);
                    datagridlistaprecios.Rows.Add(res.Get("DESTINO"));


                    count++;
                }
                sql = "SELECT PASAJE,DESTINO,PRECIOCONDESCUENTO FROM VLISTAPRECIO WHERE LINEA_PK=@LINEA AND PK_ORIGEN=@ORIGEN AND BORRADO=0";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@LINEA", linea);


                db.command.Parameters.AddWithValue("@ORIGEN", origen);



                res = db.getTable();

                while (res.Next())
                {
                    int col = pasajesdiccionario[res.Get("PASAJE")];
                    int fila = pasajesdiccionario[res.Get("DESTINO")];
                    datagridlistaprecios.Rows[fila].Cells[col].Value = res.Get("PRECIOCONDESCUENTO");


                    count++;

                }




                datagridlistaprecios.Columns[0].DefaultCellStyle.BackColor = Color.LightSlateGray;

                datagridlistaprecios.Columns[0].Width = 180;
                datagridlistaprecios.Columns[0].DefaultCellStyle.ForeColor = Color.White;
            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getRows";
                Utilerias.LOG.write(_clase, funcion, error);


            }


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


          
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getDatosAdicionales";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void comboboxorigen(object sender, EventArgs e)
        {
            try
            {
                origen = (string)(comboBoxOrigen.SelectedItem as ComboboxItem).Value.ToString();

                comboboxlinea.Enabled = true;
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "comboboxorigen";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void comboboxline(object sender, EventArgs e)
        {
            try
            {
                linea = (string)(comboboxlinea.SelectedItem as ComboboxItem).Value.ToString();

                OBTENERDESTINOS();

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "comboboxlinea";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void ToolStripSessionClose_Click(object sender, EventArgs e)
        {
            try
            {

                for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
                {
                    if (Application.OpenForms[i].Name != "Main")
                        Application.OpenForms[i].Close();
                }

               // Program.Form.DesactivarMenu();

                Form form = new Login();
                form.MdiParent = this.MdiParent;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "closestrip";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void ToolStripCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Listadeprecios_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void Listadeprecios_StyleChanged(object sender, EventArgs e)
        {

        }

        private void Listadeprecios_Shown(object sender, EventArgs e)
        {
            db = new database();
            getDatosAdicionales();
            comboBoxOrigen.DropDownStyle = ComboBoxStyle.DropDownList;
            comboboxlinea.DropDownStyle = ComboBoxStyle.DropDownList;
            datagridlistaprecios.EnableHeadersVisualStyles = false;
            comboboxlinea.Enabled = false;
            DoubleBufferedd(datagridlistaprecios, true);

        }
        private void DoubleBufferedd(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
        private void ToolStripExportExcel_Click(object sender, EventArgs e)
        {

            ExportarDataGridViewExcel(datagridlistaprecios);


        }

        public void ExportarDataGridViewExcel(DataGridView grd)
        {
            try
            {
                SaveFileDialog fichero = new SaveFileDialog();
                fichero.Filter = "Excel (*.xls)|*.xls";
                if (fichero.ShowDialog() == DialogResult.OK)
                {
                    Microsoft.Office.Interop.Excel.Application aplicacion;
                    Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                    Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                    aplicacion = new Microsoft.Office.Interop.Excel.Application();
                    libros_trabajo = aplicacion.Workbooks.Add();
                    hoja_trabajo =
                        (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);

                    int StartCol = 1;
                    int StartRow = 1;

                    string or= comboBoxOrigen.SelectedItem.ToString();
                    string lin= comboboxlinea.SelectedItem.ToString();
                    Microsoft.Office.Interop.Excel.Range myRange1 = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[StartRow, StartCol];
                    myRange1.Value2 = "Origen: " + or;
                    Microsoft.Office.Interop.Excel.Range myRange2 = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[StartRow, StartCol+3];
                    myRange2.Value2 = "Linea: " + lin;


                    StartCol = 1;
                    StartRow = 3;

                    //Write Headers
                    for (int j = 0; j < grd.Columns.Count; j++)
                    {
                        Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[StartRow, StartCol + j];
                        myRange.Value2 = grd.Columns[j].HeaderText;
                    }




                    //Recorremos el DataGridView rellenando la hoja de trabajo
                    for (int i = 0; i < grd.Rows.Count; i++)
                    {
                        for (int j = 0; j < grd.Columns.Count; j++)
                        {
                            hoja_trabajo.Cells[i + 4, j + 1] = (grd.Rows[i].Cells[j].Value != null) ? grd.Rows[i].Cells[j].Value.ToString() : "";
                        }
                    }

            
                    libros_trabajo.SaveAs(fichero.FileName,
                        Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                    //libros_trabajo.Close(true);
                    //aplicacion.Quit();
                    aplicacion.Visible = true;
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                Form mensaje = new Mensaje("No se puede guaradar el excel en esa ubicacion", true);
                DialogResult resut = mensaje.ShowDialog();
                string funcion = "datetimerpicker1";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void Button2_Click(object sender, EventArgs e)
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

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void PanelBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

    }
    }


