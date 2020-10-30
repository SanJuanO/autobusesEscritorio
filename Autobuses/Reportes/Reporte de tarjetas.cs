using Autobuses.Utilerias;
using ConnectDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Reportes
{
    public partial class Reporte_de_tarjetas : Form
    {
        public database db;
        ResultSet res = null;
        private string _sucursal;
        private string _autobus;
        private string _status;
        private string _linea;
        private string status;
        private string conductor;
        private string fechainicio;
        private double turno = 0.0;
        private double paso = 0.0;
        private double salida = 0.0;
        private double general = 0.0;

        private string fechafinal;
        private string _clase="reporte tarjetas";
        public Reporte_de_tarjetas()
        {
            InitializeComponent();
            this.Show();
        }




        public void getDatosAdicionalesLINEA()
        {
            comboBoxlinea.Items.Clear();

            string sql3 = "SELECT LINEA,PK1 FROM LINEAS ";
            db.PreparedSQL(sql3);
            res = db.getTable();

            while (res.Next())
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = res.Get("LINEA");
                item.Value = res.Get("PK1");

                comboBoxlinea.Items.Add(item);

            }
   

        }
        public void getDatosAdicionalessucursal()
        {
            comboBoxsucursal.Items.Clear();

            string sql3 = "SELECT DESTINO FROM DESTINOS ";
            db.PreparedSQL(sql3);
            res = db.getTable();

            while (res.Next())
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = res.Get("DESTINO");

                comboBoxsucursal.Items.Add(item);

            }


        }
        public void getRows(string search = "")
        {
            try
            {
                dataGridViewguias.Rows.Clear();
                string sql = "SELECT * FROM VTARJETAS WHERE STATUS=@STATUS AND PK_CHOFER=@CONDUCTOR AND FECHA BETWEEN @FECHAINICIO AND @FECHAFINAL ";
                    db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@STATUS", status);
                db.command.Parameters.AddWithValue("@CONDUCTOR", conductor);
                db.command.Parameters.AddWithValue("@FECHAINICIO", fechainicio);
                db.command.Parameters.AddWithValue("@FECHAFINAL", fechafinal);
                int n;
                res = db.getTable();
                while (res.Next())
                {
                    n = dataGridViewguias.Rows.Add();
                    dataGridViewguias.Rows[n].Cells[0].Value = res.Get("Descripcion");
                    dataGridViewguias.Rows[n].Cells[1].Value = res.Get("PK_CHOFER");
                    dataGridViewguias.Rows[n].Cells[2].Value = res.Get("ECO");
                    dataGridViewguias.Rows[n].Cells[3].Value = res.Get("TARJETA");
                    dataGridViewguias.Rows[n].Cells[4].Value = res.Get("COSTO");
                    dataGridViewguias.Rows[n].Cells[5].Value = res.Get("LINEA");
                    dataGridViewguias.Rows[n].Cells[6].Value = res.Get("STATUS");


                    n++;
                }
                db.execute();

            }
            catch (Exception e)
            {
                string error = e.Message;

            }

        }

        private void DateTimePickerinicio_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                fechainicio = dateTimePickerinicio.Value.ToString("dd/MM/yyyy");
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datetimerpicker1";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void DateTimePickerfinal_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                fechafinal = dateTimePickerfinal.Value.ToString("dd/MM/yyyy");
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datetimerpicker1";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                dataGridViewguias.Rows.Clear();
                _status = "";
                _sucursal = "";
                _autobus = "";
                _linea = "";


                if ((comboBoxsucursal.SelectedItem) != null)
                {
                    _sucursal = comboBoxsucursal.SelectedItem.ToString();

                }
           
                if ((comboBoxlinea.SelectedItem) != null)
                {

                    _linea = comboBoxlinea.SelectedItem.ToString();

                }
                


                    getRows( _sucursal, _linea, fechainicio, fechafinal);
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "buscar";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        public void getRows( string sucursal = "", string linea = "", string ini = "", string ter = "")
        {
            try
            {
                int count = 1;
                string sql = "SELECT * FROM TARJETAS_STATUS";

                if ( sucursal != "" && linea != "" && ini != "" && ter != "")
                {
                    sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                        " AND ORIGEN=@ORIGEN AND LINEA=@LINEA ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@INICIO", ini);
                    db.command.Parameters.AddWithValue("@FINAL", ter);
                    db.command.Parameters.AddWithValue("@ORIGEN", sucursal);
                    db.command.Parameters.AddWithValue("@LINEA", linea);

                }
                if (sucursal != "" && linea == "" && ini != "" && ter != "")
                {
                    sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                        " AND ORIGEN=@ORIGEN ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@INICIO", ini);
                    db.command.Parameters.AddWithValue("@FINAL", ter);
                    db.command.Parameters.AddWithValue("@ORIGEN", sucursal);

                }
                if (sucursal == "" && linea == "" && ini != "" && ter != "")
                {
                    sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                        " ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@INICIO", ini);
                    db.command.Parameters.AddWithValue("@FINAL", ter);

                }

                res = db.getTable();
                int n = 0;


                while (res.Next())
                {
                    n = dataGridViewguias.Rows.Add();


                    dataGridViewguias.Rows[n].Cells[0].Value = res.Get("HORASALIDA");
                    dataGridViewguias.Rows[n].Cells[1].Value = res.Get("ORIGEN");
                    dataGridViewguias.Rows[n].Cells[2].Value = res.Get("ECO");
                    dataGridViewguias.Rows[n].Cells[5].Value = res.Get("TARJETAPASO");
                    dataGridViewguias.Rows[n].Cells[6].Value = res.Get("COSTOPASO");
                    dataGridViewguias.Rows[n].Cells[7].Value = res.Get("TARJETASALIDA");
                    dataGridViewguias.Rows[n].Cells[8].Value = res.Get("COSTOSALIDA");
                    dataGridViewguias.Rows[n].Cells[3].Value = res.Get("TARJETATURNO");
                    dataGridViewguias.Rows[n].Cells[4].Value = res.Get("COSTOTURNO");

                    




                    count++;
                }

                int cantidad = dataGridViewguias.Rows.Count;
            for(int i = 0; i < cantidad; i++)
                {
                    turno += Convert.ToDouble(dataGridViewguias.Rows[i].Cells[4].Value.ToString());

                    paso += Convert.ToDouble(dataGridViewguias.Rows[i].Cells[6].Value.ToString());
                    salida += Convert.ToDouble(dataGridViewguias.Rows[i].Cells[8].Value);

                }
                texturno.Text = "$" + turno;
                textpaso.Text = "$" + paso;
                textsalida.Text = "$" + salida;
                general = turno + paso + salida;
                textgeneral.Text = "$" + general;
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getRows";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }


        private void SessionClose_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.cerrarSesion(this);
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Reporte_de_tarjetas_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void Reporte_de_tarjetas_Shown(object sender, EventArgs e)
        {
            dataGridViewguias.EnableHeadersVisualStyles = false;
            db = new database();
            comboBoxlinea.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxsucursal.DropDownStyle = ComboBoxStyle.DropDownList;
            timer1.Interval = 1;
            timer1.Start();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            titulo.Text = "Reporte de Tarjetas";
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            getDatosAdicionalesLINEA();
            getDatosAdicionalessucursal();
            fechainicio = DateTime.Now.ToString("dd/MM/yyyy");
            fechafinal = DateTime.Now.ToString("dd/MM/yyyy");
            timer1.Stop();
        }

        private void ToolStripExportExcel_Click(object sender, EventArgs e)
        {
                   ExportarDataGridViewExcel(dataGridViewguias);

        }

        public  void ExportarDataGridViewExcel(DataGridView grd)
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


      Microsoft.Office.Interop.Excel.Range myRange1 = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[StartRow , StartCol];
                    myRange1.Value2 = "Fecha de inicio: " + fechainicio;
                    Microsoft.Office.Interop.Excel.Range myRange2 = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[StartRow , StartCol+3];
                    myRange2.Value2 =  "Fecha de Termino: " + fechafinal;
                    Microsoft.Office.Interop.Excel.Range myRange3 = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[StartRow, StartCol + 6];

                    myRange3.Value2 = "Sucursal: " + _sucursal;
                    Microsoft.Office.Interop.Excel.Range myRange4 = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[StartRow, StartCol + 8];

                    myRange4.Value2 = "Linea: " + _linea;


                     StartCol = 1;
                     StartRow = 2;

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

                    Microsoft.Office.Interop.Excel.Range myRange5 = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[grd.Rows.Count+5, StartCol];
                    myRange5.Value2 = "Total T. turno: " + turno;
                    Microsoft.Office.Interop.Excel.Range myRange6 = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[grd.Rows.Count+6, StartCol ];
                    myRange6.Value2 = "Total T. paso: " + paso;
                    Microsoft.Office.Interop.Excel.Range myRange7 = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[grd.Rows.Count+7, StartCol ];

                    myRange7.Value2 = "Total T. salida: " + salida;
                    Microsoft.Office.Interop.Excel.Range myRange8 = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[grd.Rows.Count+8, StartCol ];

                    myRange8.Value2 = "Total General: " + general;

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
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datetimerpicker1";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DateTimePickerinicio_ValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                fechainicio = dateTimePickerinicio.Value.ToString("dd/MM/yyyy");
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datetimerpicker1";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                fechafinal = dateTimePickerfinal.Value.ToString("dd/MM/yyyy");
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datetimerpicker1";
                Utilerias.LOG.write(_clase, funcion, error);


            }
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
        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void GroupBox2_Enter(object sender, EventArgs e)
        {

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
