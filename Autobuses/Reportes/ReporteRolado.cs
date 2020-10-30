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
    public partial class ReporteRolado : Form
    {
        public database db;
        ResultSet res = null;
        private string _rol;
        private string _corrida;
        private int _linea_pk;
        private string fecha;

        private string linea;
        private string rol;
        public ReporteRolado()
        {
            InitializeComponent();
            db = new database();

            labelnombre.Text = LoginInfo.NombreID;
                labelapellido.Text = LoginInfo.ApellidoID;
            titulo.Text = "Reporte de Rol";
                MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
                Bitmap bmp = new Bitmap(ms);
                pictureBoxfoto.Image = bmp;
                labelcargo.Text = LoginInfo.rol;
                getDatosAdicionales();
                fecha = DateTime.Now.ToString("yyyy-MM-dd");
            dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(64, 69, 76);



        }
        public void getDatosAdicionales(int opc = 0, string search = "")
        {
            try
            {
                string sql = "SELECT LINEA,PK1 FROM LINEAS WHERE BORRADO=0";

                comboLineaDetalle.Items.Clear();

                    db.PreparedSQL(sql);
                    res = db.getTable();

                    while (res.Next())
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = res.Get("LINEA");
                        item.Value = res.Get("PK1");
                        comboLineaDetalle.Items.Add(item);
                        //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                    }

                comboLineaDetalle.SelectedIndex = 0;

                sql = "SELECT ROL,PK FROM ROLADOS WHERE BORRADO=0";
                comboRolDetalle.Items.Clear();

                    sql += " AND (PK_LINEA=@LINEA)";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@LINEA", linea);
               

                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("ROL");
                    item.Value = res.Get("PK");
                    comboRolDetalle.Items.Add(item);




                }
                comboRolDetalle.SelectedIndex = 0;

            }
            catch (Exception e)
            {

            }
        }

            public void getRows3()
        {
            progressBar2.Show();
            progressBar2.Minimum = 0;
            progressBar2.Maximum = 100;
            progressBar2.Value = 20;
            try
            {
                dataGridView3.Rows.Clear();

                int count = 1;
                string sql = "select * from VCORRIDAS_DIA_3 WHERE 1=1  AND PK_ROL = @ROL AND PK_LINEA=@LINEA AND FECHA=@FECHA ORDER BY PK_CORRIDA";

               

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@FECHA", fecha);
                db.command.Parameters.AddWithValue("@LINEA", linea);

                db.command.Parameters.AddWithValue("@ROL", rol);

                res = db.getTable();

                string valor = string.Empty;
                Color colorActual = Color.FromName("HotTrack");
                progressBar2.Maximum = res.Count + 20;
                progressBar2.Step = 1;
                int n3=0;
                while (res.Next())
                {
                    progressBar2.PerformStep();
                    n3 = dataGridView3.Rows.Add();

                    //DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dataGridView3.Rows[n].Cells["Ruta"];

                    dataGridView3.Rows[n3].Cells["pkDetalle"].Value = res.Get("PK");
                    dataGridView3.Rows[n3].Cells["noDetalle"].Value = count;
                    dataGridView3.Rows[n3].Cells["pkOrigenDetalle"].Value = res.Get("PK_ORIGEN");
                    dataGridView3.Rows[n3].Cells["origenDetalle"].Value = res.Get("ORIGEN");
                    dataGridView3.Rows[n3].Cells["horaDetalle"].Value = res.Get("SALIDA");
                    dataGridView3.Rows[n3].Cells["pkDestinoDetalle"].Value = res.Get("PK_DESTINO");
                    dataGridView3.Rows[n3].Cells["destinoDetalle"].Value = res.Get("DESTINO");
                    dataGridView3.Rows[n3].Cells["llegadaDetalle"].Value = res.Get("LLEGADA");
                    dataGridView3.Rows[n3].Cells["escalaDetalle"].Value = res.Get("ESCALA");
                    //dataGridView3.Rows[n3].Cells[7].Value = sumaHoras(res.Get("SALIDA"), res.Get("TIEMPO"));
                    dataGridView3.Rows[n3].Cells["rutaDescripcionDetalle"].Value = res.Get("RUTA");
                    dataGridView3.Rows[n3].Cells["ecoAutobusDetalle"].Value = res.Get("AUTOBUS");
                    dataGridView3.Rows[n3].Cells["corridaDetalle"].Value = res.Get("NO_CORRIDA");
                    dataGridView3.Rows[n3].Cells["fechaC3"].Value = "";//res.Get("FECHA_C");
                    dataGridView3.Rows[n3].Cells["fechaM3"].Value = "";//res.Get("FECHA_M");
                    dataGridView3.Rows[n3].Cells["modifico3"].Value = "";//res.Get("USUARIO");
                    dataGridView3.Rows[n3].Cells["pkCorridaRuta"].Value = res.Get("PK_CORRIDA_RUTA");
                    dataGridView3.Rows[n3].Cells["completo"].Value = res.Get("COMPLETO");

                    if (n3 == 0)
                    {
                        colorActual = Color.FromName("HotTrack");
                    }
                    else if (!dataGridView3.Rows[n3].Cells["corridaDetalle"].Value.ToString().Equals(dataGridView3.Rows[n3 - 1].Cells["corridaDetalle"].Value.ToString()))
                    {
                        colorActual = colorActual == Color.FromName("HotTrack") ? Color.FromArgb(38, 45, 56) : Color.FromName("HotTrack");
                    }

                    dataGridView3.Rows[n3].DefaultCellStyle.BackColor = colorActual;
                    count++;

                }
                //n = dataGridView1.Rows.Add();
            }
            catch (Exception e)
            {
            }

            progressBar2.Value = 0;
            progressBar2.Hide();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            fecha = dateTimePicker1.Value.ToString("yyyy-MM-dd");
        }

        private void buttonbuscar_Click(object sender, EventArgs e)
        {
            getRows3();
        }

        private void comboLineaDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
             linea = (comboLineaDetalle.SelectedItem as ComboboxItem).Value.ToString();
            comboRolDetalle.Text = "";
            string sql = "SELECT ROL,PK FROM ROLADOS WHERE BORRADO=0";
            comboRolDetalle.Items.Clear();

            sql += " AND (PK_LINEA=@LINEA)";
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@LINEA", linea);


            res = db.getTable();

            while (res.Next())
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = res.Get("ROL");
                item.Value = res.Get("PK");
                comboRolDetalle.Items.Add(item);




            }

        }

        private void comboRolDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
             rol = (comboRolDetalle.SelectedItem as ComboboxItem).Value.ToString();

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

        private void export2_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridView3);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExportarDataGridViewExcel();

        }
        public void ExportarDataGridViewExcel()
        {
            string rol = (comboRolDetalle.SelectedItem as ComboboxItem).Value.ToString();
            string sql = "SELECT * FROM VCORRIDAS_DETALLES WHERE PK_ROL=@ROL AND COMPLETO=1 ORDER BY CORRIDA, PK_CORRIDA_RUTA ASC";
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@ROL", rol);
            ResultSet res = db.getTable();

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

                /*
                //Write Headers
                for (int j = 0; j < 50; j++)
                {

                    Microsoft.Office.Interop.Excel.Range myRange = 
                        (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[StartRow, StartCol + j];
                    
                    myRange.Value2 = "ECO";
                }*/


                //Recorremos el DataGridView rellenando la hoja de trabajo
                string corridaActual = string.Empty;
                int i = 2, j = 1, num = 1;
                while (res.Next())
                {
                    if (String.IsNullOrEmpty(corridaActual))
                    {
                        corridaActual = res.Get("CORRIDA");
                        hoja_trabajo.Cells[1, j] = "SEC.";
                        hoja_trabajo.Cells[i, j] = num;
                        j++;
                        hoja_trabajo.Cells[1, j] = "PLACAS";
                        hoja_trabajo.Cells[i, j] = res.Get("PLACAS");
                        j++;
                        hoja_trabajo.Cells[1, j] = "ECO";
                        hoja_trabajo.Cells[i, j] = res.Get("ECO");
                        j++;
                        hoja_trabajo.Cells[1, j] = "Salida";
                        hoja_trabajo.Cells[i, j] = res.Get("SALIDA");
                        j++;
                        hoja_trabajo.Cells[1, j] = "Ruta";
                        hoja_trabajo.Cells[i, j] = res.Get("RUTA");
                    }
                    else if (corridaActual.Equals(res.Get("CORRIDA")))
                    {
                        j++;
                        hoja_trabajo.Cells[1, j] = "Salida";
                        hoja_trabajo.Cells[i, j] = res.Get("SALIDA");
                        j++;
                        hoja_trabajo.Cells[1, j] = "Ruta";
                        hoja_trabajo.Cells[i, j] = res.Get("RUTA");
                    }
                    else if (!corridaActual.Equals(res.Get("CORRIDA")))
                    {
                        i++; j = 1; num++;
                        corridaActual = res.Get("CORRIDA");
                        hoja_trabajo.Cells[1, j] = "SEC.";
                        hoja_trabajo.Cells[i, j] = num;
                        j++;
                        hoja_trabajo.Cells[1, j] = "PLACAS";
                        hoja_trabajo.Cells[i, j] = res.Get("PLACAS");
                        j++;
                        hoja_trabajo.Cells[i, j] = res.Get("ECO");
                        j++;
                        hoja_trabajo.Cells[i, j] = res.Get("SALIDA");
                        j++;
                        hoja_trabajo.Cells[i, j] = res.Get("RUTA");
                    }
                }
                /*
                    for (int j = 0; j < grd.Columns.Count; j++)
                    {
                        hoja_trabajo.Cells[i + 2, j + 1] = grd.Rows[i].Cells[j].Value.ToString();
                    }
                libros_trabajo.SaveAs(fichero.FileName,
                    Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                //libros_trabajo.Close(true);
                //aplicacion.Quit();
                */
                aplicacion.Visible = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }
    }

}
