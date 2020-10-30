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
namespace Autobuses.Administracion
{
    public partial class infracciones : Form
    {
        public database db;
        ResultSet res = null;
        private int status;
        private string conductor = "";
        private string autobus = "";
        private string reporte = "";
        private string socio = "";
        private string _serch = "";
        private string _clase = "reporte de reclamos";
        private string fechafiltro=DateTime.Now.ToString("yyy-MM-dd");

        public infracciones()
        {
            InitializeComponent();
            this.Show();
            titulo.Text = "Infracciones";

        }




        public void getDatosAdicionales()
        {


            try
            {
                comboBoxconductor.Items.Clear();

                string sql = "SELECT PK,NOMBRE,APELLIDOS FROM CHOFERES WHERE BORRADO=0 ORDER BY NOMBRE";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("NOMBRE") + " " + res.Get("APELLIDOS");
                    item.Value = res.Get("PK");
                    comboBoxconductor.Items.Add(item);

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getdatosadicionales";
                Utilerias.LOG.write(_clase, funcion, error);


            }


        }

        public void getRows(string fecha="")
        {
            
            try
            {
                if(fecha=="")
                {
                    fecha = fechafiltro;
                }
                dataGridView1.Rows.Clear();
                conductor = comboBoxconductor.Items.ToString();

                string sql = "SELECT * FROM INFRACCION ";

                if (!string.IsNullOrEmpty(fecha))
                {
                    sql += "WHERE convert(date,FECHA_C,104) LIKE @SEARCH";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + fecha );

                }
                else
                {
                    db.PreparedSQL(sql);

                }

                int n;

                res = db.getTable();
                while (res.Next())
                {
                    n = dataGridView1.Rows.Add();

                    dataGridView1.Rows[n].Cells[0].Value = res.Get("AUTOBUS");
                    dataGridView1.Rows[n].Cells[1].Value = res.Get("CONDUCTOR");
                    dataGridView1.Rows[n].Cells[2].Value = res.Get("SOCIO");
                    dataGridView1.Rows[n].Cells[3].Value = res.Get("INFRACCION");
                    dataGridView1.Rows[n].Cells[4].Value = res.Get("STATUS");
                   // dataGridView1.Rows[n].Cells["pkname"].Value = res.Get("pk");

                    n++;

                }

            }
            catch (Exception e)
            {
                string error = e.Message;

            }

        }

        private void GroupBox2_Enter(object sender, EventArgs e)
        {

        }
        private void limpiar()
        {

            dataGridView1.Rows.Clear();
            textBoxreporte.Text = "";
            comboBoxconductor.SelectedItem = null;
            getRows();
        }
        private void datoschofer()
        {
            string pkchofer = (string)(comboBoxconductor.SelectedItem as ComboboxItem).Value;

            string sql = "SELECT ECO,SOCIO_PK FROM AUTOBUSES WHERE PK_CHOFER=@CONDUCTOR ";
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@CONDUCTOR", pkchofer);

            int n;

            res = db.getTable();
            if (res.Next())
            {
                labelconduct.Visible = false;
                labelreporte.Visible = false;

                autobus = res.Get("ECO");
                socio = res.Get("SOCIO_PK");



            }

        }

        private void Buttonvender_Click(object sender, EventArgs e)
        {
            try
                {
                string pkconductor = (string)(comboBoxconductor.SelectedItem as ComboboxItem).Value;
                conductor = comboBoxconductor.SelectedItem.ToString();
                status = 1;
                reporte = textBoxreporte.Text;
                datoschofer();
                if (conductor != "" && reporte != "")
                {

                    string sql = "INSERT INTO INFRACCION(AUTOBUS,INFRACCION,STATUS,CONDUCTOR,SOCIO,PKCHOFER)" +
                                         " VALUES(@AUTOBUS,@INFRACCION,@STATUS,@CONDUCTOR,@SOCIO,@PK)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@CONDUCTOR", conductor);
                    db.command.Parameters.AddWithValue("@STATUS", status);
                    db.command.Parameters.AddWithValue("@SOCIO", socio);
                    db.command.Parameters.AddWithValue("@PK", pkconductor);
                    db.command.Parameters.AddWithValue("@INFRACCION", reporte);
                    db.command.Parameters.AddWithValue("@AUTOBUS", autobus);




                    if (db.execute())
                    {
                        conductor = "";
                        autobus = "";
                        reporte = "";
                        socio = "";
                        limpiar();
                        Utilerias.LOG.acciones("agregar reporte de reclamo al " + conductor);

                    }
                }

                else
                {

                    labelconduct.Visible = true;
                    labelreporte.Visible = true;
                }



            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "insertar";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int n;
                n = e.RowIndex;
                int c = e.ColumnIndex;
                string user = (string)dataGridView1.Rows[n].Cells[1].Value;
                if (n != -1 && c == 4)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dataGridView1.Rows[n].Cells[c];
                    if (chk.Selected == true)

                    {
                        string act = (string)dataGridView1.Rows[n].Cells[c].Value.ToString();
                        string val = act;

                        if (act == "False")
                        {
                            chk.Selected = false;

                            string sql = "UPDATE INFRACCION SET STATUS=@ACTIVO  WHERE CONDUCTOR=@CONDUCTOR  ";


                            db.PreparedSQL(sql);
                            db.command.Parameters.AddWithValue("@CONDUCTOR", user);
                            db.command.Parameters.AddWithValue("@ACTIVO", 1);

                            if (db.execute())
                            {
                                Utilerias.LOG.acciones("activar reporte " + user + "por " + LoginInfo.PkUsuario);

                                Form mensaje1 = new Mensaje("Reporte Activado", true);

                                mensaje1.ShowDialog();
                                limpiar();
                            }
                            else
                            {
                                MessageBox.Show("No se pudo actualizar el registro, intente de nuevo.");
                            }
                        }
                        else if (act == "True")
                        {
                            chk.Selected = false;

                            string sql = "UPDATE INFRACCION SET STATUS=@ACTIVO  WHERE CONDUCTOR=@CONDUCTOR  ";


                            db.PreparedSQL(sql);
                            db.command.Parameters.AddWithValue("@CONDUCTOR", user);
                            db.command.Parameters.AddWithValue("@ACTIVO", 0);
                            if (db.execute())
                            {
                                Utilerias.LOG.acciones("activar reporte " + user + "por " + LoginInfo.PkUsuario);

                                Form mensaje1 = new Mensaje("Reporte desactivado", true);

                                mensaje1.ShowDialog(); limpiar();

                            }
                            else
                            {
                                MessageBox.Show("No se pudo actualizar el registro, intente de nuevo.");
                            }
                        }


                    }
                    else
                    {

                        chk.Selected = true;
                    }
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "check";
                Utilerias.LOG.write(_clase, funcion, error);
            }



        }
        private void Buttonbuscar(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT * FROM INFRACCION ";
                int n;
                if (!string.IsNullOrEmpty(_serch))
                {
                    dataGridView1.Rows.Clear();

                    sql += "WHERE CONDUCTOR LIKE @SEARCH OR SOCIO LIKE @SEARCH OR STATUS LIKE @SEARCH ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + _serch + "%");

                }
                else
                {
                    db.PreparedSQL(sql);

                }

                res = db.getTable();

                while (res.Next())
                {
                    n = dataGridView1.Rows.Add();




                    dataGridView1.Rows[n].Cells[0].Value = res.Get("AUTOBUS");
                    dataGridView1.Rows[n].Cells[1].Value = res.Get("CONDUCTOR");
                    dataGridView1.Rows[n].Cells[2].Value = res.Get("SOCIO");
                    dataGridView1.Rows[n].Cells[3].Value = res.Get("INFRACCION");
                    dataGridView1.Rows[n].Cells[4].Value = res.Get("STATUS");
                    n++;

                }
               

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "buttonbuscar";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void ToolStripSessionClose_Click(object sender, EventArgs e)
        {
            this.Close();

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

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            fechafiltro = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            getRows(fechafiltro);

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Infracciones_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void Infracciones_Shown(object sender, EventArgs e)
        {

            dataGridView1.EnableHeadersVisualStyles = false;
            db = new database();
            comboBoxconductor.DropDownStyle = ComboBoxStyle.DropDownList;
      
            labelconduct.Visible = false;
            labelreporte.Visible = false;
            timer1.Interval = 1;
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            timer1.Start();
            timer2.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            getDatosAdicionales();
            getRows();
            timer1.Stop();
        }

        private void ToolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            Buttonbuscar(sender,e);
        }

        private void ToolStripExportExcel_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridView1);
            
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void Timer2_Tick(object sender, EventArgs e)
        {

            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void Buttonvender_Click_1(object sender, EventArgs e)
        {
            limpiar();
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

    }

}
