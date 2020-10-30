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
using ConnectDB;

namespace Autobuses
{
    public partial class Roles : Form
    {
        public database db;
        ResultSet res = null;
        private int n = 0;

        private int _ID;
        private string _role;
        private string _clase="roles";
        private string _user;

        public Roles()
        {
            InitializeComponent();
            this.Show();
            titulo.Text = "Roles";

        }

        private void permisos()
        {
            buttonagregar.Visible = false;
     
            buttonguardar.Visible = false;
            if (LoginInfo.privilegios.Any(x => x == "agregar roles"))
            {
                buttonagregar.Visible = true;

            }
          
            if (LoginInfo.privilegios.Any(x => x == "modificar roles"))
            {
                buttonguardar.Visible = true;
            }
        }


        public void getRows(string search = "")
        {
            try
            {
                int count = 1;
                string sql = "SELECT ROLE,ID FROM ROLES ";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += "WHERE ROLE LIKE @SEARCH ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + search + "%");

                }
                else
                {
                    db.PreparedSQL(sql);

                }

                res = db.getTable();
                if (dataGridViewRoles.InvokeRequired)
                {
                    while (res.Next())
                    {
                        dataGridViewRoles.Invoke(new Action(() =>
                        {
                            n = dataGridViewRoles.Rows.Add();

                            dataGridViewRoles.Rows[n].Cells[0].Value = count;
                            dataGridViewRoles.Rows[n].Cells[1].Value = res.Get("ROLE");
                            dataGridViewRoles.Rows[n].Cells[2].Value = res.GetInt("ID");


                        }

                            ));
                        count++;
                    }
                }
                else
                {
                    while (res.Next())
                    {

                        n = dataGridViewRoles.Rows.Add();

                        dataGridViewRoles.Rows[n].Cells[0].Value = count;
                        dataGridViewRoles.Rows[n].Cells[1].Value = res.Get("ROLE");
                        dataGridViewRoles.Rows[n].Cells[2].Value = res.GetInt("ID");


                        count++;


                    }

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getRows";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void buttonactu(object sender, EventArgs e)
        {
            try
            {
                dataGridViewRoles.Rows.Clear();
                dataGridViewRoles.Refresh();
                getRows();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "buttonactu";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private Boolean ValidarInput()
        {
            Boolean validado = true;
            try
            {
                _role = txtRole.Text;
                if (_role == "")
                {
                    labelroles.Visible = true;
                    validado = false;
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "validarinput";
                Utilerias.LOG.write(_clase, funcion, error);


            }
            return validado;
        }

        private void insert(object sender, EventArgs e)
        {
            try
            {


                if (ValidarInput())
                {


                    string sql = "INSERT INTO ROLES(ROLE) VALUES(@ROLE)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@ROLE", _role);

                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("agrego un rol :"+ _role);
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                    }


                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "insert";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void borrar(object sender, EventArgs e)
        {

        }

        private void dataGridViewRoles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {


                n = e.RowIndex;

                if (n != -1)
                {
                  buttonguardar.BackColor= Color.FromArgb(38, 45, 53);
                    buttonagregar.BackColor = Color.White;
                    _user = (string)dataGridViewRoles.Rows[n].Cells[1].Value; 
                    txtRole.Text = (string)dataGridViewRoles.Rows[n].Cells[1].Value;
                    _ID = (int)dataGridViewRoles.Rows[n].Cells[2].Value;
                    buttonguardar.Enabled = true;
                    buttonagregar.Enabled = false;
               
                    labelroles.Visible = false;

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datagridviewroles";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void update(object sender, EventArgs e)
        {
            try
            {
                if (ValidarInput())
                {
                    String rol = dataGridViewRoles.Rows[n].Cells[1].Value.ToString();


                    string sql = "UPDATE ROLES SET ROLE=@ROLE WHERE ID=@ID ";


                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@ID", _ID);
                    db.command.Parameters.AddWithValue("@ROLE", _role);

                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("actualizo el rol "+rol);
                        dataGridViewRoles.Rows.Clear();
                        dataGridViewRoles.Refresh();
                        getRows();
                        buttonagregar.Enabled = true;
                      
                        buttonguardar.Enabled = false;
                        txtRole.Text = "";
                        labelroles.Visible = false;

                    }
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el registro, intente de nuevo.");
                }

            
                }
            catch (Exception err)
            {
                string error = err.Message;
        MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "update";
        Utilerias.LOG.write(_clase, funcion, error);


            }

}

        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            insert(sender, e);
        }

        private void ToolStripButton7_Click(object sender, EventArgs e)
        {
            borrar( sender,  e);
        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            update(sender, e);
        }

        private void toolbuscar(object sender, EventArgs e)
        {
            try
            {
                string search = buscar.Text;
                string sql = "SELECT ROLE,ID FROM ROLES ";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += "WHERE ROLE LIKE @SEARCH ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + search + "%");
                    dataGridViewRoles.Rows.Clear();

                }
                else
                {
                    db.PreparedSQL(sql);

                }

                res = db.getTable();

                while (res.Next())
                {

                    n = dataGridViewRoles.Rows.Add();
                    dataGridViewRoles.Rows[n].Cells[0].Value = n + 1;
                    dataGridViewRoles.Rows[n].Cells[1].Value = res.Get("ROLE");
                    dataGridViewRoles.Rows[n].Cells[2].Value = res.GetInt("ID");
               

                }
                if (buscar.Text == "")
                {
                    dataGridViewRoles.Rows.Clear();
                    dataGridViewRoles.Refresh();
                    getRows();

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "toolbuscar";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void Buscar_KeyUp(object sender, KeyEventArgs e)
        {
                toolbuscar(sender, e);


        }

        private void limpiar()
        {
            txtRole.Text = "";
            buttonagregar.Enabled = true;
            buttonguardar.BackColor = Color.White;
            buttonagregar.BackColor = Color.FromArgb(38, 45, 53);
            buttonguardar.Enabled = false;
            labelroles.Visible = false;
            dataGridViewRoles.Rows.Clear();
            getRows();
        }
        private void clear(object sender, EventArgs e)
        {

            try
            {
                limpiar();
            }
            catch (Exception err)
            {


                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "clear";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }


        private void toolStripCerrar_Click(object sender, EventArgs e)
        {
      
                this.Close();
          
        }
        private void toolStripSessionClose_Click(object sender, EventArgs e)
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
                string funcion = "toolStripSessionClose_Click";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            getRows();

        }

        private void Roles_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void Roles_Shown(object sender, EventArgs e)
        {
            db = new database();
            buttonguardar.BackColor = Color.White;
            buttonguardar.Enabled = false;
            dataGridViewRoles.EnableHeadersVisualStyles = false;
            labelroles.Visible = false;

            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            timer1.Interval = 1;

            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            timer1.Start();
            permisos();
            backgroundWorker1.RunWorkerAsync();
        }

        private void ToolStripExportExcel_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridViewRoles);

        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

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
    }
}
