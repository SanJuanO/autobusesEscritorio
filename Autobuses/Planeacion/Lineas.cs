using ConnectDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autobuses.Utilerias;
using System.IO;
using System.Runtime.InteropServices;

namespace Autobuses.Planeacion
{
    public partial class Lineas : Form
    {

        public database db;
        ResultSet res = null;

        private int n = 0;
        private int _pk1;
        private string _linea;
        private string _razon_social;
        private string _rfc;
        private string _seach;
        private string _fecha_c;
        private string _fecha_m;
        

        public Lineas()
        {
            InitializeComponent();
            this.Show();
            btnMaximizar.Visible = false;
            btnNormal.Visible = true;
            titulo.Text = "Lineas";
            progressBar1.Hide();
        }

        public void getRows(string search = "")
        {
            try
            {
                progressBar1.Show();
                progressBar1.Value = 20;
                int count = 1;
                string sql = "SELECT * FROM LINEAS WHERE BORRADO=0";
                progressBar1.Value = 30;

                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND LINEA LIKE @SEARCH OR RAZON_SOCIAL LIKE @SEARCH OR RFC LIKE @SEARCH ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + search + "%");

                }
                else
                {
                    db.PreparedSQL(sql);

                }
                progressBar1.Value = 40;

                res = db.getTable();
                progressBar1.Value = 50;

                while (res.Next())
                {
                    
                    n = dataGridViewLineas.Rows.Add();

                    dataGridViewLineas.Rows[n].Cells[0].Value = res.Get("PK1");
                    dataGridViewLineas.Rows[n].Cells[1].Value = count;
                    dataGridViewLineas.Rows[n].Cells[2].Value = res.Get("LINEA");
                    dataGridViewLineas.Rows[n].Cells[3].Value = res.Get("RAZON_SOCIAL");
                    dataGridViewLineas.Rows[n].Cells[4].Value = res.Get("RFC");
                    dataGridViewLineas.Rows[n].Cells[5].Value = res.Get("FECHA_C");
                    dataGridViewLineas.Rows[n].Cells[6].Value = res.Get("FECHA_M");

                    count++;
                }
                progressBar1.Value = 80;

            }
            catch (Exception e)
            {
                string error = e.Message;

            }
            progressBar1.Value = 100;
            progressBar1.Value = 0;
            progressBar1.Hide();

        }

        private void LineaAdd_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            progressBar1.Increment(20);
            try
            {
                if (ValidarInputs())
                {
                    progressBar1.Increment(40);
                    string sql1 = "SELECT COUNT(PK1)MAX FROM LINEAS WHERE LINEA='" + _linea + "' ";
                    if (db.Count(sql1) > 0)
                    {
                        MessageBox.Show("Linea ya existe");
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;
                        return;
                    }
                    progressBar1.Increment(50);

                    string sql = "INSERT INTO LINEAS (LINEA,RAZON_SOCIAL,RFC,USUARIO) VALUES(@LINEA,@RAZON,@RFC,@USUARIO)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@LINEA", _linea);
                    db.command.Parameters.AddWithValue("@RAZON", _razon_social);
                    db.command.Parameters.AddWithValue("@RFC", _rfc);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.PkUsuario);
                    progressBar1.Increment(60);

                    if (db.execute())
                    {
                        progressBar1.Increment(70);

                        dataGridViewLineas.Rows.Clear();
                        dataGridViewLineas.Refresh();
                        getRows();
                        cleanForm();
                        controles(0);
                        progressBar1.Increment(90);

                    }
                    else
                    {
                        MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                    }
                    progressBar1.Increment(100);
                }
            }
            catch (Exception ex) {
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡Verifica tu conexion a internet!");
                }
                else {
                    MessageBox.Show("¡Intente nuevamente!");
                }
            }
            progressBar1.Hide();
            progressBar1.Increment(0);
        }

        private void cleanForm() {
            txtPk1.Text = "";
            txtLinea.Text = "";
            txtrazonsocial.Text = "";
            txtrfc.Text = "";
        }

        private Boolean ValidarInputs()
        {
            Boolean valido = true;
            if (!string.IsNullOrEmpty(txtPk1.Text))
            {
                _pk1 = int.Parse(txtPk1.Text);
            }
            _linea = txtLinea.Text;
            _razon_social = txtrazonsocial.Text;
            _rfc = txtrfc.Text;

            if (string.IsNullOrEmpty(_linea))
            {
                MessageBox.Show("Es necesario ingresar el nombre de la linea!");
                txtLinea.Focus();
                valido = false;

            } else if (string.IsNullOrEmpty(_razon_social)) {
                MessageBox.Show("Es necesario ingresar la razòn social de la linea!");
                txtLinea.Focus();
                valido = false;

            }
            else if (string.IsNullOrEmpty(_rfc))
            {
                MessageBox.Show("Es necesario ingresar el rfc de la linea!");
                txtLinea.Focus();
                valido = false;

            }else if (_rfc.Length !=13 && _rfc.Length!=12)
            {
                MessageBox.Show("RFC invalido!");
                txtLinea.Focus();
                valido = false;

            }

            return valido;
        }

        private void ToolStripButton7_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show("¿Esta seguro de eliminar el registro?",
                                     "Confirmar eliminar",
                                     MessageBoxButtons.YesNo);
                progressBar1.Show();
                progressBar1.Value = 20;

                if (confirmResult == DialogResult.Yes)
                {
                    progressBar1.Value = 30;

                    string sql;
                    //sql = "DELETE FROM LINEAS WHERE PK1 = @PK1";
                    sql = "UPDATE LINEAS SET BORRADO=1, USUARIO=@USUARIO WHERE PK1 = @PK1";
                    _pk1 = int.Parse(txtPk1.Text);
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.PkUsuario);
                    db.command.Parameters.AddWithValue("@PK1", _pk1);
                    progressBar1.Value = 50;


                    if (db.execute())
                    {

                        progressBar1.Value = 70;

                        dataGridViewLineas.Rows.Clear();
                        dataGridViewLineas.Refresh();
                        getRows();
                        cleanForm();
                        controles(0);
                        progressBar1.Value = 100;

                    }
                    else
                    {
                        progressBar1.Value = 70;
                        MessageBox.Show("No se pudo eliminar el registro, intente de nuevo.");
                    }
                }

            }
            catch (Exception err)
            {
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡Verifica tu conexion a internet!");
                }
                else {
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                }
                string error = err.Message;
                LOG.write("Lineas", "ToolStripButton7_Click", e.ToString());
            }
            progressBar1.Hide();
            progressBar1.Value = 0;

        }

        private void DataGridViewLineas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            n = e.RowIndex;

            if (n != -1)
            {
                txtPk1.Text=(string)dataGridViewLineas.Rows[n].Cells[0].Value;
                txtLinea.Text = (string)dataGridViewLineas.Rows[n].Cells[2].Value;
                txtrazonsocial.Text = (string)dataGridViewLineas.Rows[n].Cells[3].Value;
                txtrfc.Text = (string)dataGridViewLineas.Rows[n].Cells[4].Value;
                controles(1);
            }

        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            if (ValidarInputs())
            {
                string sql1 = "SELECT COUNT(PK1)MAX FROM LINEAS WHERE LINEA='" + _linea + "' AND NOT PK1="+_pk1+"";
                if (db.Count(sql1) > 0)
                {
                    MessageBox.Show("Linea ya existe");
                    progressBar1.Value = 0;
                    progressBar1.Visible = false;
                    return;
                }

                string sql = "UPDATE LINEAS SET LINEA=@LINEA, RAZON_SOCIAL=@RAZON, RFC=@RFC, USUARIO=@USUARIO WHERE PK1=@PK1";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@LINEA", _linea);
                db.command.Parameters.AddWithValue("@RAZON", _razon_social);
                db.command.Parameters.AddWithValue("@RFC", _rfc);
                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.PkUsuario);
                db.command.Parameters.AddWithValue("@PK1", _pk1);

                if (db.execute())
                {
                    dataGridViewLineas.Rows.Clear();
                    dataGridViewLineas.Refresh();
                    getRows();
                    cleanForm();
                    controles(0);
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el registro, intente de nuevo.");
                }
            }
        }

        private void ToolStripRefresh_Click(object sender, EventArgs e)
        {
            dataGridViewLineas.Rows.Clear();
            dataGridViewLineas.Refresh();
            cleanForm();
            getRows();
            controles(0);

        }

        private void ToolStripButton5_Click(object sender, EventArgs e)
        {
            dataGridViewLineas.Rows.Clear();
            dataGridViewLineas.Refresh();
            getRows(_seach);

        }

        private void ToolStripExportExcel_Click(object sender, EventArgs e)
        {
           Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridViewLineas);
        }

        private void ToolStripCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolStripSessionClose_Click(object sender, EventArgs e)
        {

            Utilerias.Utilerias.cerrarSesion(this);

        }


        public void controles(int OPC)
        {

            switch (OPC)
            {
                case 0:
                    btnAdd.Enabled = true;
                    btnRefresh.Enabled = true;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;

                    btnAdd.BackColor = Color.FromArgb(38, 45, 56);
                    btnRefresh.BackColor = Color.FromArgb(38, 45, 56);
                    btnDelete.BackColor = Color.White;
                    btnSave.BackColor = Color.White;
                    

                    break;
                case 1:
                    
                    btnAdd.Enabled = false;
                    btnRefresh.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = true;

                    btnAdd.BackColor = Color.White;
                    btnRefresh.BackColor = Color.FromArgb(38, 45, 56);
                    btnDelete.BackColor = Color.FromArgb(38, 45, 56);
                    btnSave.BackColor = Color.FromArgb(38, 45, 56);


                    break;

            }

        }

        private void SearchEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ToolStripButton5_Click(sender, e);
            }
        }

        private void Lineas_Load(object sender, EventArgs e)
        {
            db = new database();
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;

            this.WindowState = FormWindowState.Maximized;
            timer1.Interval = 1;
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            //dataGridViewLineas.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
            dataGridViewLineas.EnableHeadersVisualStyles = false;

            getRows();
            controles(0);
        }

        private void Lineas_Shown(object sender, EventArgs e)
        {
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            this.Focus();

            timer2.Start();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
        }

        private void BtnMaximizar_Click(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;


            btnMaximizar.Visible = false;
            btnNormal.Visible = true;
        }

        private void BtnNormal_Click(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Normal;

            btnNormal.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Timer2_Tick(object sender, EventArgs e)
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

        private void PanelBarraTitulo_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
