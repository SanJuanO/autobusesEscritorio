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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Administracion
{
    public partial class Permisos : Form
    {
        public database db;
        ResultSet res = null;
        ResultSet res2 = null;

        private int n = 0;

        private int _PK1;
        private string _privilegio;
        private string _descripcion;
        private string _search;
        private string _user;
        private string _categoria;
        private string _clase;
        private Boolean _validador=false;
        private string verificar;

        public Permisos()
        {
            InitializeComponent();
            this.Show();
            titulo.Text = "Permisos";

        }

        private void permisos()
        {
            buttonagregar.Visible = false;
            buttonborrar.Visible = false;
            buttonguardar.Visible = false;
            if (LoginInfo.privilegios.Any(x => x == "agregar permisos"))
            {
                buttonagregar.Visible = true;

            }
            if (LoginInfo.privilegios.Any(x => x == "borrar permisos"))
            {
                buttonborrar.Visible = true;

            }
            if (LoginInfo.privilegios.Any(x => x == "modificar permisos"))
            {
                buttonguardar.Visible = true;

            }
        }

        public void getDatosAdicionales()
        {
            try
            {

                string sql = "SELECT * FROM ROLES ";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("ROLE");
                    item.Value = res.Get("ID");
                    comboBoxRole.Items.Add(item);
                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }
                comboBoxRole.SelectedIndex = 0;

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getDatosAdicionales";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        public void getRows(string search = "")
        {

            try
            {
                int count = 1;
                string comboboxactual = comboBoxRole.SelectedItem.ToString();

                string sql = "SELECT PRIVILEGIO,CATEGORIA,DESCRIPCION,ROLE,PRIV,ID,P FROM VISTAROLPRIV WHERE ROLE=@ROLE order by categoria ASC";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ROLE", comboboxactual);


              

                 res = db.getTable();

                if (dataGridViewpermisos.InvokeRequired)
                {
                    while (res.Next())

                    {
                        dataGridViewpermisos.Invoke(new Action(() =>
                        {

                            n = dataGridViewpermisos.Rows.Add();

                            dataGridViewpermisos.Rows[n].Cells[0].Value = count;
                            dataGridViewpermisos.Rows[n].Cells[1].Value = res.Get("PRIVILEGIO");
                            dataGridViewpermisos.Rows[n].Cells[2].Value = res.Get("CATEGORIA");
                            dataGridViewpermisos.Rows[n].Cells[3].Value = res.Get("DESCRIPCION");
                            dataGridViewpermisos.Rows[n].Cells[4].Value = res.Get("ROLE");
                            dataGridViewpermisos.Rows[n].Cells[6].Value = res.Get("PRIV");
                            string pk_priv = dataGridViewpermisos.Rows[n].Cells[6].Value.ToString();
                            if (pk_priv == "")
                            {
                                dataGridViewpermisos.Rows[n].Cells[5].Value = 0;
                            }
                            if (pk_priv != "")
                            {
                                dataGridViewpermisos.Rows[n].Cells[5].Value = 1;
                            }
                            dataGridViewpermisos.Rows[n].Cells[7].Value = res.Get("ID");
                            dataGridViewpermisos.Rows[n].Cells[8].Value = res.Get("P");
                        }

                            ));
                        count++;
                    }

                }
                else
                {
                    while (res.Next())
                    {
                        n = dataGridViewpermisos.Rows.Add();

                        dataGridViewpermisos.Rows[n].Cells[0].Value = count;
                        dataGridViewpermisos.Rows[n].Cells[1].Value = res.Get("PRIVILEGIO");
                        dataGridViewpermisos.Rows[n].Cells[2].Value = res.Get("CATEGORIA");
                        dataGridViewpermisos.Rows[n].Cells[3].Value = res.Get("DESCRIPCION");
                        dataGridViewpermisos.Rows[n].Cells[4].Value = res.Get("ROLE");
                        dataGridViewpermisos.Rows[n].Cells[6].Value = res.Get("PRIV");
                        string pk_priv = dataGridViewpermisos.Rows[n].Cells[6].Value.ToString();
                        if (pk_priv == "")
                        {
                            dataGridViewpermisos.Rows[n].Cells[5].Value = 0;
                        }
                        if (pk_priv != "")
                        {
                            dataGridViewpermisos.Rows[n].Cells[5].Value = 1;
                        }
                        dataGridViewpermisos.Rows[n].Cells[7].Value = res.Get("ID");
                        dataGridViewpermisos.Rows[n].Cells[8].Value = res.Get("P");

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

     
        private void Check(object sender, DataGridViewCellEventArgs e)
        {

            _validador = true;
            try
            {
                int c = e.ColumnIndex;
                n = e.RowIndex;
                if (n != -1 && c == 5)
                {
                    if (LoginInfo.privilegios.Any(x => x == "modificar permisos"))
                    {
                        buttonguardar.Visible = true;



                        string permisos = (string)dataGridViewpermisos.Rows[n].Cells[6].Value;
                        string ID = (string)dataGridViewpermisos.Rows[n].Cells[7].Value;
                        string P = (string)dataGridViewpermisos.Rows[n].Cells[8].Value;
                        string per = (string)dataGridViewpermisos.Rows[n].Cells[1].Value;
                        string roool = (string)dataGridViewpermisos.Rows[n].Cells[4].Value;



                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dataGridViewpermisos.Rows[n].Cells[c];
                        if (chk.Selected == true)

                        {
                            string act = (string)dataGridViewpermisos.Rows[n].Cells[5].Value.ToString();
                            string val = act;

                            if (act == "0")
                            {
                                chk.Selected = true;

                                string sql = "INSERT INTO  ROLES_PRIVILEGIOS(PK_ROL,PK_PRIVILEGIO) VALUES(@PERMISOS,@ID) ";


                                db.PreparedSQL(sql);
                                db.command.Parameters.AddWithValue("@PERMISOS", ID);
                                db.command.Parameters.AddWithValue("@ID", P);

                                if (db.execute())
                                {
                                    Form mensaje = new Mensaje("Permiso activado", true);

                                    DialogResult resut = mensaje.ShowDialog(); 
                                    limpiar();
                                    _validador = false;
                                    Utilerias.LOG.acciones("activo el permiso" + per + " AL ROL " + roool);
                                }
                                else
                                {
                                    MessageBox.Show("No se pudo actualizar el registro, intente de nuevo.");
                                }
                            }
                            else if (act == "1")
                            {
                                chk.Selected = false;

                                string sql = "DELETE ROLES_PRIVILEGIOS WHERE PK_PRIVILEGIO=@PERMISOS AND PK_ROL=@ID ";


                                db.PreparedSQL(sql);
                                db.command.Parameters.AddWithValue("@PERMISOS", permisos);
                                db.command.Parameters.AddWithValue("@ID", ID);
                                if (db.execute())
                                {
                                    Form mensaje = new Mensaje("Permiso desactivado", true);

                                    DialogResult resut = mensaje.ShowDialog();
                                    dataGridViewpermisos.Refresh();
                                    limpiar();
                                    Utilerias.LOG.acciones("desactivo el permiso" + per + " AL ROL " + roool);

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
                    else
                    {
                        Form mensaje = new Mensaje("No tienes permisos", true);

                        DialogResult resut = mensaje.ShowDialog();
                    }
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "Check";
                Utilerias.LOG.write(_clase, funcion, error);


            }




        }
        private void ComboBoxRole_SelectedIndexChanged(object sender, EventArgs e)
        {
        
                try
            {
                string rolee;
                rolee = (string)(comboBoxRole.SelectedItem as ComboboxItem).Value.ToString();
                string sql = "SELECT PRIVILEGIO,CATEGORIA,DESCRIPCION,ROLE,PRIV,ID,P FROM VISTAROLPRIV ";

                int count = 1;
                    dataGridViewpermisos.Rows.Clear();

                    sql += "WHERE ID = @SEARCH ORDER BY CATEGORIA";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH",rolee);

                
              
                res = db.getTable();

                while (res.Next())
                {
                    n = dataGridViewpermisos.Rows.Add();
                    dataGridViewpermisos.Rows[n].Cells[0].Value = count;
                    dataGridViewpermisos.Rows[n].Cells[1].Value = res.Get("PRIVILEGIO");
                    dataGridViewpermisos.Rows[n].Cells[2].Value = res.Get("CATEGORIA");
                    dataGridViewpermisos.Rows[n].Cells[3].Value = res.Get("DESCRIPCION");
                    dataGridViewpermisos.Rows[n].Cells[4].Value = res.Get("ROLE");
                    dataGridViewpermisos.Rows[n].Cells[6].Value = res.Get("PRIV");
                    string pk_priv = dataGridViewpermisos.Rows[n].Cells[6].Value.ToString();
                    if (pk_priv == "")
                    {
                        dataGridViewpermisos.Rows[n].Cells[5].Value = 0;
                    }
                    if (pk_priv != "")
                    {
                        dataGridViewpermisos.Rows[n].Cells[5].Value = 1;
                    }
                    dataGridViewpermisos.Rows[n].Cells[7].Value = res.Get("ID");
                    dataGridViewpermisos.Rows[n].Cells[8].Value = res.Get("P");

                    count++;

                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "ComboBoxRole_SelectedIndexChanged";
                Utilerias.LOG.write(funcion, funcion, error, funcion);


            }

        }
        private Boolean ValidarInput()
        {
            Boolean validado = true;

            try
            {
                _privilegio = txtprivilegio.Text;
                _descripcion = textdescripcion.Text;
                _categoria = categoria.Text;
                if (_privilegio == "")
                {
                    labelnombre.Visible = true;
                    validado = false;
                }
                if (_descripcion == "")
                {
                    labeldescripcion.Visible = true;
                    validado = false;
                }
                if (_categoria == "")
                {
                    labelcategoria.Visible = true;
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
        private void limpiarerrores()
        {
            labelcategoria.Visible = false;
            labelnombre.Visible = false;
            labeldescripcion.Visible = false;

        }
        private void insert(object sender, EventArgs e)
        {
            try
            {


                if (ValidarInput())
                {


                    string sql = "INSERT INTO PRIVILEGIOS(PRIVILEGIO,DESCRIPCION,CATEGORIA) VALUES(@PRIVILEGIO,@DESCRIPCION,@CATEGORIA)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PRIVILEGIO", _privilegio);
                    db.command.Parameters.AddWithValue("@DESCRIPCION", _descripcion);
                    db.command.Parameters.AddWithValue("@CATEGORIA", _categoria);

                    if (db.execute())
                    {

                        Utilerias.LOG.acciones("agrego un permiso ");
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

        private void Borrar(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show("¿Esta seguro de eliminar el registro?",
                                   "Confirmar eliminar",
                                   MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    string sql;
                    sql = "DELETE FROM PRIVILEGIOS WHERE PRIVILEGIO = @PK1";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK1", txtprivilegio.Text);

                    if (db.execute())
                    {

                        if (n != -1)
                        {
                            Utilerias.LOG.acciones("borro un permiso");

                          
                            buttonborrar.Enabled = false;
                            buttonguardar.Enabled = false;
                            buttonagregar.Enabled = true;
                            dataGridViewpermisos.Rows.RemoveAt(n);
                            txtprivilegio.Text = "";
                            textdescripcion.Text = "";
                            categoria.Text = "";
                            limpiarerrores();

                        }
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
                string funcion = "borrar";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void DataGridViewpermisos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                n = e.RowIndex;

                if (_validador == false)
                {

                    if (n != -1)
                    {
                        txtprivilegio.Text = (string)dataGridViewpermisos.Rows[n].Cells[1].Value;
                        categoria.Text = (string)dataGridViewpermisos.Rows[n].Cells[2].Value;
                        textdescripcion.Text = (string)dataGridViewpermisos.Rows[n].Cells[3].Value;
                        _PK1 = (int)dataGridViewpermisos.Rows[n].Cells[4].Value;
                 
                        buttonborrar.Enabled = true;
                        buttonguardar.Enabled = true;
                        buttonagregar.Enabled = false;
                        limpiarerrores();

                    }
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "DataGridViewpermisos_CellContentClick";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void buscador(object sender, EventArgs e)
        {

        }

        private void updat(object sender, EventArgs e)
        {
            try
            {
                string pk = (string)dataGridViewpermisos.Rows[n].Cells[8].Value;
                if (ValidarInput())
                {


                    string sql = "UPDATE PRIVILEGIOS SET PRIVILEGIO=@PRIVILEGIO,DESCRIPCION=@DESCRIPCION,CATEGORIA=@CATEGORIA WHERE PK1=@PK1 ";


                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK1", pk);
                    db.command.Parameters.AddWithValue("@PRIVILEGIO", _privilegio);
                    db.command.Parameters.AddWithValue("@CATEGORIA", _categoria);
                    db.command.Parameters.AddWithValue("@DESCRIPCION", _descripcion);
                    if (db.execute())
                    {
                        limpiar();

                    }
                }
         

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "updat";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        private void limpiar()
        {
            txtprivilegio.Text = "";
            textdescripcion.Text = "";
            categoria.Text = "";
            buttonborrar.Enabled = false;
            buttonguardar.Enabled = false;
            buttonagregar.Enabled = true;

            buttonborrar.BackColor=Color.White;
            buttonguardar.BackColor=Color.White;
            buttonagregar.BackColor=Color.FromArgb(38, 45, 53);
            limpiarerrores();
            dataGridViewpermisos.Rows.Clear();
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

        private void CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
            try
            {
                 n = e.RowIndex;

              

                    if (n != -1)
                    {
                        txtprivilegio.Text = (string)dataGridViewpermisos.Rows[n].Cells[1].Value;
                        categoria.Text = (string)dataGridViewpermisos.Rows[n].Cells[2].Value;
                        textdescripcion.Text = (string)dataGridViewpermisos.Rows[n].Cells[3].Value;
                    buttonagregar.BackColor = Color.White;
                    buttonborrar.BackColor = Color.FromArgb(38, 45, 53);
                    buttonguardar.BackColor = Color.FromArgb(38, 45, 53);
                    buttonborrar.Enabled = true;
                        buttonguardar.Enabled = true;
                        buttonagregar.Enabled = false;
                    }
                
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "CellContentClick";
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

                //Program.Form.DesactivarMenu();

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
        private void buscar(object sender, EventArgs e)
        {
            try
            {
                _search =serch.Text;
                string sql = "SELECT * FROM VISTAROLPRIV ";
                int count = 1;
                if (!string.IsNullOrEmpty(_search))
                {
                    dataGridViewpermisos.Rows.Clear();

                    sql += "WHERE PRIVILEGIO LIKE @SEARCH OR ROLE LIKE @SEARCH ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + _search + "%");

                }
                else
                {
                    db.PreparedSQL(sql);

                }

                res = db.getTable();

                while (res.Next())
                {
                    n = dataGridViewpermisos.Rows.Add();

                    dataGridViewpermisos.Rows[n].Cells[0].Value = count;
                    dataGridViewpermisos.Rows[n].Cells[1].Value = res.Get("PRIVILEGIO");
                    dataGridViewpermisos.Rows[n].Cells[2].Value = res.Get("CATEGORIA");
                    dataGridViewpermisos.Rows[n].Cells[3].Value = res.Get("DESCRIPCION");
                    dataGridViewpermisos.Rows[n].Cells[4].Value = res.Get("ROLE");
                    dataGridViewpermisos.Rows[n].Cells[6].Value = res.Get("PRIV");
                    string pk_priv = dataGridViewpermisos.Rows[n].Cells[6].Value.ToString();
                    if (pk_priv == "")
                    {
                        dataGridViewpermisos.Rows[n].Cells[5].Value = 0;
                    }
                    if (pk_priv != "")
                    {
                        dataGridViewpermisos.Rows[n].Cells[5].Value = 1;
                    }
                    dataGridViewpermisos.Rows[n].Cells[7].Value = res.Get("ID");
                    dataGridViewpermisos.Rows[n].Cells[8].Value = res.Get("P");

                    count++;

                }
                if (serch.Text == "")
                {
                    dataGridViewpermisos.Rows.Clear();
                    dataGridViewpermisos.Refresh();
                    buscador(sender, e);
                 
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "buscar";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void bucartext(object sender, KeyEventArgs e)
        {
            buscar(sender, e);
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            getRows();
        }

        private void Permisos_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void Permisos_Shown(object sender, EventArgs e)
        {
            db = new database();
            comboBoxRole.DropDownStyle = ComboBoxStyle.DropDownList;
      
            buttonborrar.Enabled = false;
            buttonguardar.Enabled = false;
            buttonborrar.BackColor = Color.White;
            buttonguardar.BackColor = Color.White;
            dataGridViewpermisos.EnableHeadersVisualStyles = false;
            timer1.Interval = 1;
            labelname.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            timer1.Start();
            timer2.Start();
            DoubleBufferedd(dataGridViewpermisos, true);

        }
        private void DoubleBufferedd(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            limpiarerrores();
            permisos();
            getDatosAdicionales();
            getRows();
            timer1.Stop();
        }

        private void ToolStripExportExcel_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridViewpermisos);

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
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

    }


}
