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

namespace Autobuses.Administracion
{
    public partial class Zonas : Form
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
        

        public Zonas()
        {
            InitializeComponent();
            this.Show();
            titulo.Text = "Zonas";


        }

        public void getRows(string search = "")
        {
            try
            {
                int count = 1;
                string sql = "SELECT * FROM ZONAS ";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += "WHERE ZONA LIKE @SEARCH ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + search + "%");

                }
                else
                {
                    db.PreparedSQL(sql);

                }

                res = db.getTable();

                while (res.Next())
                {
                    
                    n = dataGridViewLineas.Rows.Add();

                    dataGridViewLineas.Rows[n].Cells[0].Value = res.Get("PK");
                    dataGridViewLineas.Rows[n].Cells[1].Value = count;
                    dataGridViewLineas.Rows[n].Cells[2].Value = res.Get("ZONA");
                    dataGridViewLineas.Rows[n].Cells[3].Value = res.Get("DESCRIPCION");
                    

                    count++;
                }

            }
            catch (Exception e)
            {
                string error = e.Message;

            }

        }

        private void LineaAdd_Click(object sender, EventArgs e)
        {
            if (ValidarInputs())
            {

                string sql = "INSERT INTO ZONAs (ZONA,DESCRIPCION) VALUES(@ZONA,@DESCRIPCION)";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ZONA", _linea);
                db.command.Parameters.AddWithValue("@DESCRIPCION", _razon_social);
    

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
                    MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                }
            }
        }

        private void cleanForm() {
            txtPk1.Text = "";
            txtLinea.Text = "";
            txtrazonsocial.Text = "";
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
       

            return valido;
        }

        private void ToolStripButton7_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show("¿Esta seguro de eliminar el registro?",
                                     "Confirmar eliminar",
                                     MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    string sql;
                    sql = "DELETE FROM ZONAS WHERE PK = @PK1";
                    _pk1 = int.Parse(txtPk1.Text);
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK1", _pk1);

                    if (db.execute())
                    {

                        dataGridViewLineas.Rows.Clear();
                        dataGridViewLineas.Refresh();
                        getRows();
                        cleanForm();
                        /*
                        if (n != -1)
                        {
                            dataGridViewUsuarios.Rows.RemoveAt(n);

                        }*/

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

        }

        private void DataGridViewLineas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            n = e.RowIndex;

            if (n != -1)
            {
                txtPk1.Text=(string)dataGridViewLineas.Rows[n].Cells[0].Value;
                txtLinea.Text = (string)dataGridViewLineas.Rows[n].Cells[2].Value;
                txtrazonsocial.Text = (string)dataGridViewLineas.Rows[n].Cells[3].Value;
                controles(1);
            }

        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            if (ValidarInputs())
            {
                
                string sql = "UPDATE ZONAS SET ZONA=@ZONA, DESCRIPCION=@DESCRIPCION WHERE PK=@PK1";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ZONA", _linea);
                db.command.Parameters.AddWithValue("@DESCRIPCION", _razon_social);
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
            getRows();
            controles(0);

        }

        private void ToolStripButton5_Click(object sender, EventArgs e)
        {
            _seach = txtSearch.Text;
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
                   
                    menuSearch.Visible = true;
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
                 
                    menuSearch.Visible = true;
                    btnAdd.Enabled = false;
                    btnRefresh.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = true;

                    btnAdd.BackColor = Color.White;
                    btnRefresh.BackColor = Color.FromArgb(38, 45, 56);
                    btnDelete.BackColor = Color.FromArgb(38, 45, 56); ;
                    btnSave.BackColor = Color.FromArgb(38, 45, 56); ;

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

        private void Zonas_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void Zonas_Shown(object sender, EventArgs e)
        {


           dataGridViewLineas.EnableHeadersVisualStyles = false;


            db = new database();
            timer1.Interval = 1;
            timer1.Start();
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            timer2.Start();

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            getRows();
            controles(0);
            timer1.Stop();
        }

        private void ToolStripExportExcel_Click_1(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridViewLineas);

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

        private void PanelBarraTitulo_MouseMove(object sender, MouseEventArgs e)
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
