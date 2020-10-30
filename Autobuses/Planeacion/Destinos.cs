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

namespace Autobuses.Planeacion
{
    public partial class Destinos : Form
    {

        public database db;
        ResultSet res = null;

        private int n = 0;
        private int _pk1;
        private string _clave;
        private string _destino;
        private string _fecha_c;
        private string _fecha_m;
        private string _usuario;
        private string _search;

        public Destinos()
        {
            InitializeComponent();
            this.Show();

            titulo.Text = "Destinos";
            btnMaximizar.Visible = false;
            btnNormal.Visible = true;
            progressBar1.Hide();
        }

        private void MenuAdd_Click(object sender, EventArgs e)
        {

            progressBar1.Show();
            progressBar1.Value = 20;
            try
            {

                if (ValidarInputs())
                {

                    string sql1 = "SELECT COUNT(PK1)MAX FROM DESTINOS WHERE CLAVE='" + _clave + "' OR DESTINO='" + _destino + "' ";
                    if (db.Count(sql1) > 0)
                    {
                        MessageBox.Show("¡clave ó destino ya esta en uso porfavor cambie estos parametros!");
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;
                        return;
                    }

                    progressBar1.Value = 40;

                    string sql = "INSERT INTO DESTINOS(CLAVE,DESTINO,USUARIO) VALUES(@CLAVE,@DESTINO,@USUARIO)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@CLAVE", _clave);
                    db.command.Parameters.AddWithValue("@DESTINO", _destino);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.PkUsuario);
                    progressBar1.Value = 50;

                    if (db.execute())
                    {
                        progressBar1.Value = 70;

                        dataGridView.Rows.Clear();
                        dataGridView.Refresh();
                        getRows();
                        cleanForm();
                        controles(0);
                        progressBar1.Value = 85;

                    }
                    else
                    {
                        MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                    }
                    progressBar1.Value = 100;

                }
            }
            catch (Exception ex) {
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡Verifica tu cponexion a internet!");
                }
                else {
                    MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                }
                LOG.write("Destinos", "MenuAdd_Click", ex.ToString());
            }
            progressBar1.Hide();
            progressBar1.Value = 0;

        }


        private Boolean ValidarInputs()
        {
            Boolean valido = true;
            if (!String.IsNullOrEmpty(txtpk1.Text))
            {
                _pk1 = int.Parse(txtpk1.Text);
            }
            _clave = txtClave.Text;
            _destino = txtDestino.Text;
            
            if (string.IsNullOrEmpty(_clave))
            {
                MessageBox.Show("Es necesario ingresar la clave!");
                txtClave.Focus();
                valido = false;

            }
            else if (string.IsNullOrEmpty(_destino))
            {
                MessageBox.Show("Es necesario ingresar destino!");
                txtDestino.Focus();
                valido = false;

            }

            return valido;
        }

        private void cleanForm()
        {
            txtpk1.Text = "";
            txtClave.Text = "";
            txtDestino.Text = "";
            txtSearch.Text = "";
        }

        public void getRows(string search = "")
        {
            try
            {
                progressBar1.Show();
                progressBar1.Value = 20;
                int count = 1;
                string sql = "SELECT * FROM DESTINOS WHERE BORRADO=0";
                
                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND (CLAVE LIKE @SEARCH OR DESTINO LIKE @SEARCH) ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + search + "%");

                }
                else
                {
                    db.PreparedSQL(sql);

                }
                progressBar1.Value = 50;

                res = db.getTable();
                progressBar1.Value = 80;

                while (res.Next())
                {

                    n = dataGridView.Rows.Add();

                    dataGridView.Rows[n].Cells[0].Value = res.Get("PK1");
                    dataGridView.Rows[n].Cells[1].Value = count;
                    dataGridView.Rows[n].Cells[2].Value = res.Get("CLAVE");
                    dataGridView.Rows[n].Cells[3].Value = res.Get("DESTINO");
                    dataGridView.Rows[n].Cells[4].Value = res.Get("FECHA_C");
                    dataGridView.Rows[n].Cells[5].Value = res.Get("FECHA_M");
                    dataGridView.Rows[n].Cells[6].Value = res.Get("USUARIO");

                    count++;
                }
                progressBar1.Value = 100;

            }
            catch (Exception e)
            {
                string error = e.Message;
                LOG.write("Destinos","getRows",e.ToString(),LoginInfo.PkUsuario);

            }
            progressBar1.Hide();
            progressBar1.Value = 0;


        }

        public void controles(int OPC)
        {

            switch (OPC)
            {
                case 0:
                    
                    btnAdd.Enabled = true;
                    btnRefresb.Enabled = true;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;

                    btnAdd.BackColor = Color.FromArgb(38, 45, 56);
                    btnRefresb.BackColor = Color.FromArgb(38, 45, 56);
                    btnDelete.BackColor = Color.White;
                    btnSave.BackColor = Color.White;

                    break;
                case 1:
                    
                    btnAdd.Enabled = false;
                    btnRefresb.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = true;

                    btnAdd.BackColor = Color.White;
                    btnRefresb.BackColor = Color.FromArgb(38, 45, 56);
                    btnDelete.BackColor = Color.FromArgb(38, 45, 56);
                    btnSave.BackColor = Color.FromArgb(38, 45, 56);


                    break;

            }

        }

        private void MenuRefresh_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            dataGridView.Refresh();
            getRows();
            cleanForm();
            controles(0);
        }

        private void MenuDelete_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Show();
                progressBar1.Value = 20;
                var confirmResult = MessageBox.Show("¿Esta seguro de eliminar el registro?",
                                     "Confirmar eliminar",
                                     MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    progressBar1.Value = 30;

                    string sql;
                    //sql = "DELETE FROM DESTINOS WHERE PK1 = @PK1";
                    sql = "UPDATE DESTINOS SET BORRADO=1, USUARIO=@USUARIO WHERE PK1 = @PK1";
                    _pk1 = int.Parse(txtpk1.Text);
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.PkUsuario);
                    db.command.Parameters.AddWithValue("@PK1", _pk1);
                    progressBar1.Value = 50;
                    if (db.execute())
                    {
                        progressBar1.Value = 80;

                        dataGridView.Rows.Clear();
                        dataGridView.Refresh();
                        getRows();
                        cleanForm();
                        controles(0);
                        progressBar1.Value = 100;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el registro, intente de nuevo.");
                    }
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
            progressBar1.Hide();
            progressBar1.Value = 0;

        }

        private void MenuSave_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Show();
                progressBar1.Value = 20;

                if (ValidarInputs())
                {
                    string sql1 = "SELECT COUNT(PK1)MAX FROM DESTINOS WHERE (CLAVE='" + _clave + "' OR DESTINO='" + _destino + "') AND NOT PK1="+_pk1;
                    if (db.Count(sql1) > 0)
                    {
                        MessageBox.Show("¡clave ó destino ya esta en uso porfavor cambie estos parametros!");
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;
                        return;
                    }

                    progressBar1.Value = 40;

                    string sql = "UPDATE DESTINOS SET CLAVE=@CLAVE, DESTINO=@DESTINO, USUARIO=@USUARIO WHERE PK1=@PK1";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@CLAVE", _clave);
                    db.command.Parameters.AddWithValue("@DESTINO", _destino);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.PkUsuario);
                    db.command.Parameters.AddWithValue("@PK1", _pk1);
                    progressBar1.Value = 60;

                    if (db.execute())
                    {
                        progressBar1.Value = 80;

                        dataGridView.Rows.Clear();
                        dataGridView.Refresh();
                        getRows();
                        cleanForm();
                        controles(0);
                        progressBar1.Value = 100;

                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el registro, intente de nuevo.");
                    }
                }
            }
            catch (Exception ex) {

                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡Verifica tu conexion a internet!");
                }
                else {
                    MessageBox.Show("¡Intenta nuevamente!");
                }
                LOG.write("Destinos", "MenuSave_Click", ex.ToString());
            }
            progressBar1.Hide();
            progressBar1.Value = 0;
        }

        private void MenuExportExcel_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridView);
        }

        private void MenuSearch_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            dataGridView.Refresh();
            _search = txtSearch.Text;
            getRows(_search);
        }

        private void DataGridViewCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            n = e.RowIndex;

            if (n != -1)
            {
                txtpk1.Text = (string)dataGridView.Rows[n].Cells[0].Value;
                txtClave.Text = (string)dataGridView.Rows[n].Cells[2].Value;
                txtDestino.Text = (string)dataGridView.Rows[n].Cells[3].Value;
                controles(1);
            }
        }

        private void SearchEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MenuSearch_Click(sender,e);
            }
        }

        private void ToolStripCerrar_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void ToolStripSessionClose_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.cerrarSesion(this);
        }

        private void Destinos_Load(object sender, EventArgs e)
        {
            db = new database();
            this.WindowState = FormWindowState.Maximized;
            timer1.Interval = 1;
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            //dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
            dataGridView.EnableHeadersVisualStyles = false;
            getRows();
            controles(0);
        }

        private void BtnMaximizar_Click(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;


            btnMaximizar.Visible = false;
            btnNormal.Visible = true;
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

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void Destinos_Shown(object sender, EventArgs e)
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
