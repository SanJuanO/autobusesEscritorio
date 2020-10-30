using Autobuses.Utilerias;
using ConnectDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Planeacion
{
    public partial class Rolado : Form
    {
        private string TIEMPO_CARGA = "00:10";
        public database db;
        ResultSet res = null;

        private int n = 0;
        private int n0 = 0;
        private int n2 = 0;
        private int nd = 0;
        private int n3 = 0;
        private int _pkCorrida;
        private int _pkRol;
        private int _pkCorridaRuta;
        private string _rol;
        private string _corrida;
        private int _linea_pk;
        private int _pk_autobus;
        private string _fecha_c;
        private string _fecha_m;
        private string _usuario;
        private string _search;
        private List<int> rutasAdd;
        private List<int> rutasDeleted;
        private List<int> rutasUpdate;
        private bool nuevo = true;

        public Rolado()
        {
            InitializeComponent();
            this.Show();
            titulo.Text = "Rolado";
            btnMaximizar.Visible = false;
            btnNormal.Visible = true;
            progressBar3.Hide();
            progressBar4.Hide();
            
        }

        private void DoubleBuffered(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        public void getRows1(string search = "")
        {
            try
            {
                int maximo = 0;
                dataGridView1.Rows.Clear();
                int count = 1;
                string sql = "SELECT * FROM VCORRIDAS ";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += "WHERE PK_ROL = @SEARCH ORDER BY CORRIDA ASC";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", search );

                }
                else
                {
                    sql += " ORDER BY CORRIDA ASC";
                    db.PreparedSQL(sql);

                }

                res = db.getTable();

                while (res.Next())
                {

                    n = dataGridView1.Rows.Add();

                    dataGridView1.Rows[n].Cells[0].Value = res.Get("PK");
                    dataGridView1.Rows[n].Cells[1].Value = count;
                    dataGridView1.Rows[n].Cells[2].Value = res.Get("PK_ROL");
                    dataGridView1.Rows[n].Cells[3].Value = res.Get("ROL");
                    dataGridView1.Rows[n].Cells[4].Value = res.Get("CORRIDA");
                    dataGridView1.Rows[n].Cells[5].Value = res.Get("PK_AUTOBUS");
                    dataGridView1.Rows[n].Cells[6].Value = res.Get("ECO");
                    dataGridView1.Rows[n].Cells[7].Value = res.Get("FECHA_C");
                    dataGridView1.Rows[n].Cells[8].Value = res.Get("FECHA_M");
                    dataGridView1.Rows[n].Cells[9].Value = res.Get("USUARIO");
                    if (maximo < res.GetInt("CORRIDA")) {
                        maximo = res.GetInt("CORRIDA");
                    }
                    count++;

                }
                try
                {
                    txtCorrida.Items.Clear();
                }
                catch { }
                for (int i = 1; i <= maximo+1; i++) {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = i.ToString();
                    item.Value = i;
                    txtCorrida.Items.Add(item);
                }


            }
            catch (Exception e)
            {
                LOG.write("ROLADO", "getRows", e.Message);
            }

        }

        /*OPCION 0 LINEAS OPCION 1 ROLADOS*/
        public void getDatosAdicionales(int opc = 0,string search="")
        {
            string sql = "SELECT LINEA,PK1 FROM LINEAS WHERE BORRADO=0";
            try
            {
                if (opc == 0)
                {
                    comboLineaDetalle.Items.Clear();
                    comboLinea.Items.Clear();

                    db.PreparedSQL(sql);
                    res = db.getTable();

                    while (res.Next())
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = res.Get("LINEA");
                        item.Value = res.Get("PK1");
                        comboLinea.Items.Add(item);
                        comboLineaDetalle.Items.Add(item);

                        //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                    }
                    if (comboLinea.Items != null && comboLinea.Items.Count > 0) {
                        comboLinea.SelectedIndex = 0;
                    }
                }
                else if (opc == 1)
                {
                    sql = "SELECT ROL,PK FROM ROLADOS WHERE BORRADO=0";
                    comboRolDetalle.Items.Clear();
                    comboRol.Items.Clear();

                    if (!String.IsNullOrEmpty(search))
                    {
                        sql += " AND (PK_LINEA=@LINEA)";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@LINEA", search);
                    }
                    else
                    {
                        db.PreparedSQL(sql);
                    }

                    res = db.getTable();

                    while (res.Next())
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = res.Get("ROL");
                        item.Value = res.Get("PK");
                        comboRol.Items.Add(item);
                        comboRolDetalle.Items.Add(item);

                        //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                    }
                }
                else if (opc == 2)
                {
                    sql = "SELECT ECO,PK1 FROM AUTOBUSES WHERE BORRADO=0";
                    comboAutobus.Items.Clear();

                    if (!String.IsNullOrEmpty(search))
                    {
                        sql += " AND (LINEA_PK=@LINEA)";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@LINEA", search);
                    }
                    else
                    {
                        db.PreparedSQL(sql);
                    }

                    res = db.getTable();

                    while (res.Next())
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = res.Get("ECO");
                        item.Value = res.Get("PK1");
                        comboAutobus.Items.Add(item);

                        //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                    }
                }
                else if (opc == 3) {
                    string pkLinea = (comboLinea.SelectedItem as ComboboxItem).Value.ToString();
                    DataGridViewComboBoxColumn comboboxColumn = dataGridView2.Columns["Ruta"] as DataGridViewComboBoxColumn;
                    comboboxColumn.DataSource = Populate("SELECT  RUTA,PK FROM VRUTAS WHERE BORRADO=0 AND LINEA_PK = " + pkLinea + " ORDER BY RUTA");
                    comboboxColumn.DisplayMember = "RUTA";
                    comboboxColumn.ValueMember = "PK";
                }
                else if (opc == 4)
                {
                    sql = "SELECT ROL,PK FROM ROLADOS WHERE BORRADO=0 ";
                    comboRolDetalle.Items.Clear();

                    if (!String.IsNullOrEmpty(search))
                    {
                        sql += " AND PK_LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@LINEA", search);
                    }
                    else
                    {
                        db.PreparedSQL(sql);
                    }

                    res = db.getTable();

                    while (res.Next())
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = res.Get("ROL");
                        item.Value = res.Get("PK");
                        comboRolDetalle.Items.Add(item);

                        //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                if (!Utilerias.Utilerias.CheckForInternetConnection()) {
                    MessageBox.Show("¡Verifique su conexion de internet!");
                }else
                {
                    MessageBox.Show("¡Error al optener datos adicionales, intente más tarde!");
                }
                LOG.write("ROLADO", "getDatosAdicionales", e.Message);
            }
        }

        public void controles(int OPC)
        {

            switch (OPC)
            {
                case 0:
             
                    btnAdd0.Enabled = true;
                    btnRefresh0.Enabled = true;
                    btnDelete0.Enabled = false;
                    btnSave0.Enabled = false;


                    btnAdd0.BackColor = Color.FromArgb(38, 45, 56);
                    btnRefresh0.BackColor = Color.FromArgb(38, 45, 56);
                    btnDelete0.BackColor = Color.White;
                    btnSave0.BackColor = Color.White;

                    break;
                case 1:
                    
                    btnAdd0.Enabled = false;
                    btnRefresh0.Enabled = true;
                    btnDelete0.Enabled = true;
                    btnSave0.Enabled = true;

                    btnAdd0.BackColor = Color.White;
                    btnRefresh0.BackColor = Color.FromArgb(38, 45, 56);
                    btnDelete0.BackColor = Color.FromArgb(38, 45, 56);
                    btnSave0.BackColor = Color.FromArgb(38, 45, 56);

                    break;
                case 2:
                    
                    btnAdd1.Enabled = true;
                    btnRefresh1.Enabled = true;
                    btnDelete1.Enabled = false;
                    btnSave1.Enabled = false;
                    btnSaveDetalles.Enabled = false;

                    btnAdd1.BackColor = Color.FromArgb(38, 45, 56);
                    btnRefresh1.BackColor = Color.FromArgb(38, 45, 56);
                    btnDelete1.BackColor = Color.White;
                    btnSave1.BackColor = Color.White;
                    btnSaveDetalles.BackColor= Color.White;

                    break;
                case 3:
                    
                    btnAdd1.Enabled = false;
                    btnRefresh1.Enabled = true;
                    btnDelete1.Enabled = true;
                    btnSave1.Enabled = true;
                    btnSaveDetalles.Enabled = true;

                    btnAdd1.BackColor = Color.White;
                    btnRefresh1.BackColor = Color.FromArgb(38, 45, 56);
                    btnDelete1.BackColor = Color.FromArgb(38, 45, 56);
                    btnSave1.BackColor = Color.FromArgb(38, 45, 56);
                    btnSaveDetalles.BackColor = Color.FromArgb(38, 45, 56); ;

                    break;
            }

        }


        private Boolean ValidarInputs(int opc)
        {
            Boolean valido = true;

            if (opc == 0)
            {
                _pkRol = int.TryParse(txtPkRol.Text, out int _pk11) ? _pk11 : -1;
                _linea_pk = -1;
                _rol = txtRol.Text;

                if (comboLinea.Items.Count > 0 && comboLinea.SelectedItem != null && (comboLinea.SelectedItem as ComboboxItem).Value != null)
                {
                    _linea_pk = int.TryParse((comboLinea.SelectedItem as ComboboxItem).Value.ToString(), out int valor) ? valor : -1;
                }

                if (_linea_pk == -1)
                {

                    MessageBox.Show("Es necesario ingresar liena!");
                    comboLinea.Focus();
                    valido = false;

                }
                else if (string.IsNullOrEmpty(_rol))
                {

                    MessageBox.Show("Es necesario ingresar el nombre del rol!");
                    txtCorrida.Focus();
                    valido = false;

                }
                
            }
            else if (opc == 1)
            {
                _pk_autobus = -1;
                _pkCorrida = int.TryParse(txtpkCorrida.Text, out int _pk11) ? _pk11 : -1;
                _corrida = txtCorrida.Text;

                if (comboRol.Items.Count > 0 && comboRol.SelectedItem != null && (comboRol.SelectedItem as ComboboxItem).Value != null)
                {
                    _pkRol = int.TryParse((comboRol.SelectedItem as ComboboxItem).Value.ToString(),out int valor)?valor:-1;
                    _rol = (comboRol.SelectedItem as ComboboxItem).Text.ToString();
                }

                if (comboAutobus.Items.Count > 0 && comboAutobus.SelectedItem != null && (comboAutobus.SelectedItem as ComboboxItem).Value != null)
                {
                    _pk_autobus = int.TryParse((comboAutobus.SelectedItem as ComboboxItem).Value.ToString(), out int valor) ? valor : -1;
                }

                if (_pkRol==-1)
                {

                    MessageBox.Show("Es necesario ingresar liena!");
                    comboRol.Focus();
                    valido = false;

                }
                else if (string.IsNullOrEmpty(_corrida))
                {

                    MessageBox.Show("Es necesario ingresar el número de corrida!");
                    txtCorrida.Focus();
                    valido = false;

                }
                else if (_pk_autobus==-1)
                {

                    MessageBox.Show("Es necesario ingresar el nombre del rol!");
                    comboAutobus.Focus();
                    valido = false;

                }

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells["corridanum"].Value.Equals(_corrida))
                    {
                        MessageBox.Show("El número de corrida ya existe!");
                        txtCorrida.Focus();
                        valido = false;
                    }
                }


            }
            
            else if (opc == 2)
            {
                _pk_autobus = -1;
                _pkCorrida = int.TryParse(txtpkCorrida.Text, out int _pk11) ? _pk11 : -1;
                _corrida = txtCorrida.Text;

                if (comboRol.Items.Count > 0 && comboRol.SelectedItem != null && (comboRol.SelectedItem as ComboboxItem).Value != null)
                {
                    _pkRol = int.TryParse((comboRol.SelectedItem as ComboboxItem).Value.ToString(),out int valor)?valor:-1;
                    _rol = (comboRol.SelectedItem as ComboboxItem).Text.ToString();
                }

                if (comboAutobus.Items.Count > 0 && comboAutobus.SelectedItem != null && (comboAutobus.SelectedItem as ComboboxItem).Value != null)
                {
                    _pk_autobus = int.TryParse((comboAutobus.SelectedItem as ComboboxItem).Value.ToString(), out int valor) ? valor : -1;
                }

                if (_pkRol==-1)
                {

                    MessageBox.Show("Es necesario ingresar liena!");
                    comboRol.Focus();
                    valido = false;

                }
                else if (string.IsNullOrEmpty(_corrida))
                {

                    MessageBox.Show("Es necesario ingresar el número de corrida!");
                    txtCorrida.Focus();
                    valido = false;

                }
                else if (_pk_autobus==-1)
                {

                    MessageBox.Show("Es necesario ingresar el nombre del rol!");
                    comboAutobus.Focus();
                    valido = false;

                }

            }

            return valido;

        }


        private void cleanForm(int opc)
        {
            if (opc == 0) {//Limpia Rolados
                txtRol.Text = "";
                if (comboLinea.Items.Count > 0) {
                    comboLinea.SelectedIndex = 0;
                }
                txtPkRol.Text = "";
            }
            else if (opc == 1)
            {
                
                txtpkCorrida.Text = "";
                txtCorrida.Text = "";

                getDatosAdicionales(1);
                if (comboRol.Items.Count > 0)
                {
                    comboRol.SelectedIndex = 0;
                }
                if (!string.IsNullOrEmpty(txtRol.Text))
                {
                    comboRol.Text = txtRol.Text;
                }
                else if (!String.IsNullOrEmpty(_rol)) {
                    comboRol.Text = _rol;
                }

            }

        }

        private void MenuAdd1_Click(object sender, EventArgs e)
        {
            progressBar4.Show();
            progressBar4.Value = 20;
            try
            {
                if (ValidarInputs(1))
                {
                    progressBar4.Value = 40;

                    string sql = "INSERT INTO CORRIDAS(PK_ROL,CORRIDA,PK_AUTOBUS,USUARIO) VALUES('@ROL','@CORRIDA',@AUTOBUS,'@USUARIO')";
                    sql = sql.Replace("@ROL", _pkRol.ToString());
                    sql = sql.Replace("@CORRIDA", _corrida);
                    sql = sql.Replace("@AUTOBUS", _pk_autobus.ToString());
                    sql = sql.Replace("@USUARIO", LoginInfo.UserID);

                    progressBar4.Value = 60;

                    string id = db.executeId(sql);
                    progressBar4.Value = 80;

                    if (!String.IsNullOrEmpty(id) && int.TryParse(id, out int resul) && resul > 0)
                    {
                        progressBar4.Value = 90;

                        txtpkCorrida.Text = id;
                        saveRutasCorridas();
                        dataGridView1.Rows.Clear();
                        dataGridView1.Refresh();
                        rutasUpdate.Clear();//limpiar antes de usar datagridView1.Clear(); para que la funcion de remove no elimine los elementos
                        dataGridView2.DataSource = null;
                        dataGridView2.Rows.Clear();
                        getRows1(_pkRol.ToString());
                        cleanForm(1);
                        //getDatosAdicionales();
                        controles(0);
                        progressBar4.Value = 100;

                    }
                    else
                    {
                        MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                    }
                }
            }
            catch (Exception err)
            {
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡verifique su conexion de internet!");
                }
                else
                {
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                }
                string error = err.ToString();
                LOG.write("Rolado", "MenuAdd1_Click", error);
            }
            progressBar4.Hide();
            progressBar4.Value = 0;

        }


        private void MenuRefresh1_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            try
            {
                rutasUpdate.Clear();//limpiar antes de usar datagridView1.Clear(); para que la funcion de remove no elimine los elementos
                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
            }
            catch (Exception ex) { String error = ex.Message; }
            getRows1(_pkRol.ToString());
            cleanForm(1);
            controles(2);
        }

        private void MenuDelete1_Click(object sender, EventArgs e)
        {
            progressBar4.Show();
            progressBar4.Value=20;
            try
            {
                var confirmResult = MessageBox.Show("¿Esta seguro de eliminar el registro?",
                                     "Confirmar eliminar",
                                     MessageBoxButtons.YesNo);
                progressBar4.Value=30;
                if (confirmResult == DialogResult.Yes)
                {
                    string sql;
                    sql = "DELETE FROM CORRIDAS WHERE PK = @PK";
                    _pkCorrida = int.Parse(txtpkCorrida.Text);
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", _pkCorrida);
                    progressBar4.Value=50;

                    if (db.execute())
                    {
                        progressBar4.Value=80;
                        sql = "DELETE FROM CORRIDAS_RUTAS WHERE PK_CORRIDA=@CORRIDA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@CORRIDA", _pkCorrida);
                        db.execute();

                        rutasUpdate.Clear();//limpiar antes de usar datagridView1.Clear(); para que la funcion de remove no elimine los elementos
                        dataGridView2.DataSource = null;
                        dataGridView2.Rows.Clear();

                        dataGridView1.Rows.Clear();
                        dataGridView1.Refresh();

                        getRows1(_pkRol.ToString());
                        cleanForm(1);
                        getDatosAdicionales(1);
                        comboRol.Text = _rol;
                        controles(0);
                        progressBar4.Value=100;

                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el registro, intente de nuevo.");
                    }
                }

            }
            catch (Exception err)
            {
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡verifique su conexion de internet!");
                }
                else
                {
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                }
                string error = err.Message;
                LOG.write("ROLADO", "MenuDelete1_Click", error);
            }
            progressBar4.Hide();
            progressBar4.Value = 0;
        }

        private void MenuSave1_Click(object sender, EventArgs e)
        {
            progressBar4.Show();
            progressBar4.Value = 20;
            try
            {
                if (ValidarInputs(2))
                {
                    progressBar4.Value = 40;

                    string sql = "UPDATE CORRIDAS SET PK_ROL=@ROL, CORRIDA=@CORRIDA, PK_AUTOBUS=@AUTOBUS, USUARIO=@USUARIO WHERE PK=@PK";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@ROL", _pkRol);
                    db.command.Parameters.AddWithValue("@CORRIDA", _corrida);
                    db.command.Parameters.AddWithValue("@AUTOBUS", _pk_autobus);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                    db.command.Parameters.AddWithValue("@PK", _pkCorrida);
                    progressBar4.Value = 60;

                    if (db.execute())
                    {
                        progressBar4.Value = 70;

                        saveRutasCorridas();
                        progressBar4.Value = 80;

                        rutasUpdate.Clear();//limpiar antes de usar datagridView1.Clear(); para que la funcion de remove no elimine los elementos
                        dataGridView1.Rows.Clear();
                        dataGridView1.Refresh();
                        dataGridView2.DataSource = null;
                        dataGridView2.Rows.Clear();
                        getRows1(_pkRol.ToString());
                        cleanForm(1);
                        getDatosAdicionales(1);
                        comboRol.Text = _rol;
                        controles(2);
                        progressBar4.Value = 100;

                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el registro, intente de nuevo.");
                    }
                }
            }
            catch (Exception err)
            {
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡verifique su conexion de internet!");
                }
                else
                {
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                }
                string error = err.Message;
                LOG.write("ROLADO", "MenuSave1_Click", error);
            }
            progressBar4.Hide();
            progressBar4.Value = 0;
        }

        private void MenuExportExcel_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridView1);
        }

        private void MenuSearch_Click(object sender, EventArgs e)
        {
            /*
            _search = txtSearch.Text;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            getRows(_search);
            */
        }

        private void SearchEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MenuSearch_Click(sender, e);
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

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            n = e.RowIndex;

            if (n != -1)
            {
                try {
                    rutasUpdate.Clear();//limpiar antes de usar datagridView1.Clear(); para que la funcion de remove no elimine los elementos
                    dataGridView2.DataSource = null;
                    dataGridView2.Rows.Clear();
                }
                catch (Exception ex) { string error = ex.Message; }
                txtpkCorrida.Text = (string)dataGridView1.Rows[n].Cells[0].Value;
                comboRol.Text = dataGridView1.Rows[n].Cells[3].Value.ToString();
                comboAutobus.Text = dataGridView1.Rows[n].Cells["ECO"].Value.ToString();
                txtCorrida.Text = (string)dataGridView1.Rows[n].Cells[4].Value;
                controles(3);
                getRows2(txtpkCorrida.Text);
                //getRows1(txtpkCorrida.Text);

            }
        }

        private void ComboLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox comboBox = (ComboBox)sender;
                txtCorrida.Text = "";
                txtpkCorrida.Text = "";
                rutasUpdate.Clear();//limpiar antes de usar datagridView1.Clear(); para que la funcion de remove no elimine los elementos
                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();

                txtPkRol.Text=(comboBox.SelectedItem as ComboboxItem).Value.ToString();
                getRows1(txtPkRol.Text);

                int count = 1;
                string sql = "SELECT * FROM VRUTAS ";
                
                if (comboRol.Items.Count > 0 && comboRol.SelectedItem != null && (comboRol.SelectedItem as ComboboxItem).Value != null)
                {
                    _linea_pk = int.TryParse((comboRol.SelectedItem as ComboboxItem).Value.ToString(), out int valor) ? valor : -1;
                }

                if (_linea_pk!=-1)
                {
                    sql += "WHERE LINEA_PK = @SEARCH ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", _linea_pk);

                }

                res = db.getTable();
                /*
                DataGridViewComboBoxColumn comboboxColumn = dataGridView2.Columns["Ruta"] as DataGridViewComboBoxColumn;

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("RUTA");
                    item.Value = res.Get("PK");
                    comboboxColumn.Items.Add(item);
                }
                comboboxColumn.DataSource = Populate("Select  RUTA,PK FROM VRUTAS where LINEA_PK="+_linea_pk);
                comboboxColumn.DisplayMember = "RUTA";
                comboboxColumn.ValueMember = "PK";
                */
            }
            catch (Exception eX)
            {
                LOG.write("ROLADO", "ComboLinea_SelectedIndexChanged", eX.Message);
            }
            
        }

        public bool llenaDatos() {
            try
            {

                int count = 1;
                string sql = "SELECT * FROM VRUTAS ";

                if (comboRol.Items.Count > 0 && comboRol.SelectedItem != null && (comboRol.SelectedItem as ComboboxItem).Value != null)
                {
                    _linea_pk = int.TryParse((comboRol.SelectedItem as ComboboxItem).Value.ToString(), out int valor) ? valor : -1;
                }

                if (_linea_pk != -1)
                {
                    sql += "WHERE LINEA_PK = @SEARCH ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", _linea_pk);

                }

                res = db.getTable();
                /*
                DataGridViewComboBoxColumn comboboxColumn = dataGridView2.Columns["Ruta"] as DataGridViewComboBoxColumn;

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("RUTA");
                    item.Value = res.Get("PK");
                    comboboxColumn.Items.Add(item);
                }
                comboboxColumn.DisplayMember = "RUTA";
                comboboxColumn.ValueMember = "PK";
                comboboxColumn.DataSource = Populate("Select  RUTA,PK FROM VRUTAS where LINEA_PK="+_linea_pk);
                */

            }
            catch (Exception eX)
            {
                LOG.write("ROLADO", "llenaDatos", eX.Message);
            }
            return true;
        }

        private DataTable Populate(string sqlCommand)
        {
            SqlConnection northwindConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);
            northwindConnection.Open();

            SqlCommand command = new SqlCommand(sqlCommand, northwindConnection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = command;

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            adapter.Fill(table);

            return table;
        }

        private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            Console.WriteLine("add LINEA: "+ e.RowIndex);
             
            /*
            if (e.RowIndex>0 && dataGridView1.Rows[e.RowIndex-1].Cells[0].Value == null)
            {
                rutasAdd.Add(e.RowIndex - 1);
            }
            */
            //ComboLinea_SelectedIndexChanged(sender, e);
        }

        private void DataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            Console.WriteLine("LINEA: " + e.RowIndex);
            if (rutasUpdate.Count > 0 && rutasUpdate.Count > e.RowIndex)
            {
                int pk_corrida_ruta = rutasUpdate.ElementAt(e.RowIndex);
                if (Delete_Corrida_Ruta(pk_corrida_ruta))
                {
                    Delete_Corrida_Ruta_detalles(pk_corrida_ruta);
                    rutasUpdate.Remove(rutasUpdate.ElementAt(e.RowIndex));
                    MessageBox.Show("¡Registro eliminado!");
                }
            }
        }

        public bool Delete_Corrida_Ruta(int pkCorridaRuta) {

            try {

                string sql = "DELETE FROM CORRIDAS_RUTAS WHERE PK=@PK";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PK", pkCorridaRuta);
                if (db.execute()) {
                    return true;
                }

            }
            catch (Exception ex) {
                LOG.write("Rolado", "Delete_Corrida_Ruta()", ex.Message);
            }

            return false;
        }

        public bool Delete_Corrida_Ruta_detalles(int pkCorridaRuta) {

            try
            {

                string sql = "DELETE FROM CORRIDAS_RUTAS_DETALLES WHERE PK_CORRIDA_RUTA=@PK";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PK", pkCorridaRuta);
                if (db.execute())
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                LOG.write("Rolado", "Delete_Corrida_Ruta_detalles()", ex.Message);
            }

            return false;
        }

        public void getRows2(string search = "")
        {
            try
            {
                nuevo = false;
                int count = 1;
                string sql = "SELECT * FROM CORRIDAS_RUTAS ";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += "WHERE PK_CORRIDA = @SEARCH ORDER BY PK ASC";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", search);

                }
                else
                {
                    sql += " ORDER BY PK ASC";
                    db.PreparedSQL(sql);

                }

                res = db.getTable();

                while (res.Next())
                {

                    n = dataGridView2.Rows.Add();

                    DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dataGridView2.Rows[n].Cells["Ruta"];

                    dataGridView2.Rows[n].Cells[0].Value = res.Get("PK");
                    dataGridView2.Rows[n].Cells[1].Value = count;

                    DataRow rowaux=null;
                    string pkLinea = (comboLinea.SelectedItem as ComboboxItem).Value.ToString();
                    foreach (DataRow row in Populate("Select  RUTA, PK FROM VRUTAS where LINEA_PK = "+pkLinea).Rows) {
                            if (row["PK"].ToString().Equals(res.Get("PK_RUTA"))) {
                                rowaux =row;
                            }
                    }

                    cell.Value = rowaux["PK"];
                    /*
                    DataGridViewCellStyle Time_Obj = new DataGridViewCellStyle();
                    Time_Obj.Format = "HH:mm";*/
                    String SALIDA= res.Get("SALIDA");
                    dataGridView2.Rows[n].Cells["Salida"].Value = res.Get("SALIDA");
                    dataGridView2.Rows[n].Cells["L"].Value = res.Get("L");
                    dataGridView2.Rows[n].Cells["M"].Value = res.Get("M");
                    dataGridView2.Rows[n].Cells["MI"].Value = res.Get("MI");
                    dataGridView2.Rows[n].Cells["J"].Value = res.Get("J");
                    dataGridView2.Rows[n].Cells["V"].Value = res.Get("V");
                    dataGridView2.Rows[n].Cells["S"].Value = res.Get("S");
                    dataGridView2.Rows[n].Cells["D"].Value = res.Get("D");
                    rutasUpdate.Add(int.Parse(res.Get("PK")));


                    count++;

                }
                //n = dataGridView1.Rows.Add();
                nuevo = true;
            }
            catch (Exception e)
            {
                LOG.write("ROLADO", "getRows1", e.Message);
            }

        }

        public bool saveRutasCorridas(int index=-1) {
            int max = 0;
            if (index == -1)
            {
                if (dataGridView2.Rows != null) { max = dataGridView2.Rows.Count; }
                progressBar1.Visible = true;progressBar1.Minimum = 1;progressBar1.Maximum = max;
                progressBar1.Value = 1;progressBar1.Step=1;
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {

                    progressBar1.PerformStep();
                    int indice = dataGridView2.Rows.IndexOf(row);

                    if (indice < dataGridView2.Rows.Count - 1)
                    {
                        string temp = dataGridView2.Rows[indice].Cells["Salida"].FormattedValue.ToString();
                        if (temp == "")
                        {
                            progressBar1.Visible = false;
                            Form mensaje = new Mensaje("Falta agregar la hora de salida", true);
                            mensaje.ShowDialog();

                            return false;
                        }

                    }

                    if ((row.Cells[0].Value == null || String.IsNullOrEmpty(row.Cells[0].Value.ToString())) && row.Cells["Ruta"].Value != null)
                    {
                        AddRutaofRole(indice, 1);
                    }
                    else if ((row.Cells[0].Value != null && !String.IsNullOrEmpty(row.Cells[0].Value.ToString())) && row.Cells["Ruta"].Value != null)
                    {
                        AddRutaofRole(indice, 2);
                    }
                }
                progressBar1.Visible = false;
            }
            else if (index!=-1) {
                DataGridViewRow row = dataGridView2.Rows[index];

                if ((row.Cells[0].Value == null || String.IsNullOrEmpty(row.Cells[0].Value.ToString())) && row.Cells["Ruta"].Value != null)
                {
                    AddRutaofRole(index, 1);
                }
                else if ((row.Cells[0].Value != null && !String.IsNullOrEmpty(row.Cells[0].Value.ToString())) && row.Cells["Ruta"].Value != null)
                {
                    AddRutaofRole(index, 2);
                }
            }
            return true;
        }

        public bool AddRutaofRole(int index,int opc) {
            string salida2 = "00:00";
            string tiempoCompleto = "00:00";
            int ruta_pk=-1, _pk=-1,pkOrigenCompleto=-1,pkDestinoCompleto=-1;
            //DataGridViewComboBoxColumn comboboxColumn = dataGridView1.Columns["Ruta"] as DataGridViewComboBoxColumn;
            if (dataGridView2.Rows[index].Cells[0].Value != null)
            {
                _pk = int.TryParse(dataGridView2.Rows[index].Cells[0].Value.ToString(), out int result2) ? result2 : -1;
            }
            if (dataGridView2.Rows[index].Cells["Ruta"].Value != null)
            {
                ruta_pk = int.TryParse(dataGridView2.Rows[index].Cells["Ruta"].Value.ToString(), out int result) ? result : -1;
            }
            if (!String.IsNullOrEmpty(txtpkCorrida.Text))
            {
                _pkCorrida = int.TryParse(txtpkCorrida.Text, out int result1) ? result1 : -1;
            }

            string sql = String.Empty;
            string salida = dataGridView2.Rows[index].Cells["Salida"].FormattedValue.ToString();
            string lunes = (dataGridView2.Rows[index].Cells["L"].Value!=null)?dataGridView2.Rows[index].Cells["L"].Value.ToString():"false";
            string martes = (dataGridView2.Rows[index].Cells["M"].Value != null) ? dataGridView2.Rows[index].Cells["M"].Value.ToString():"false";
            string miercoles = (dataGridView2.Rows[index].Cells["MI"].Value != null) ? dataGridView2.Rows[index].Cells["Mi"].Value.ToString():"false";
            string jueves = (dataGridView2.Rows[index].Cells["J"].Value != null) ? dataGridView2.Rows[index].Cells["J"].Value.ToString():"false";
            string viernes = (dataGridView2.Rows[index].Cells["V"].Value != null) ? dataGridView2.Rows[index].Cells["V"].Value.ToString():"false";
            string sabado = (dataGridView2.Rows[index].Cells["S"].Value != null) ? dataGridView2.Rows[index].Cells["S"].Value.ToString():"false";
            string domingo = (dataGridView2.Rows[index].Cells["D"].Value != null) ? dataGridView2.Rows[index].Cells["D"].Value.ToString():"false";

            bool ban = false;
            if (opc == 1)//INSERT
            {
                sql = "INSERT INTO CORRIDAS_RUTAS (PK_CORRIDA,PK_RUTA,SALIDA,L,M,MI,J,V,S,D,USUARIO) " +
                    "VALUES(@CORRIDA,@RUTA,'@SALIDA','@L','@MA','@MI','@J','@V','@S','@D','@USUARIO')";
                /*
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@CORRIDA", _pkCorrida);
                db.command.Parameters.AddWithValue("@SALIDA", salida);
                db.command.Parameters.AddWithValue("@RUTA", ruta_pk);*/

                sql = sql.Replace("@CORRIDA", ""+_pkCorrida);
                sql = sql.Replace("@SALIDA", ""+salida);
                sql = sql.Replace("@RUTA", ""+ruta_pk);
                sql = sql.Replace("@L", lunes);
                sql = sql.Replace("@MA", martes);
                sql = sql.Replace("@MI", miercoles);
                sql = sql.Replace("@J", jueves);
                sql = sql.Replace("@V", viernes);
                sql = sql.Replace("@S", sabado);
                sql = sql.Replace("@D", domingo);
                sql = sql.Replace("@USUARIO", LoginInfo.UserID);

                string id=db.executeId(sql);
                if (!String.IsNullOrEmpty(id)) {
                    dataGridView2.Rows[index].Cells[0].Value = id;
                    _pk = int.TryParse(dataGridView2.Rows[index].Cells[0].Value.ToString(), out int result2) ? result2 : -1;
                    ban = true;
                }

            }
            else if (opc == 2) {//UPDATE
                sql = "UPDATE CORRIDAS_RUTAS SET PK_CORRIDA=@CORRIDA,PK_RUTA=@RUTA,SALIDA=@SALIDA,L=@L,M=@M,MI=@MI,J=@J,V=@V,S=@S,D=@D, USUARIO=@USUARIO " +
                      "WHERE PK=@PK";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@CORRIDA", _pkCorrida);
                db.command.Parameters.AddWithValue("@RUTA", ruta_pk);
                db.command.Parameters.AddWithValue("@SALIDA", salida);
                db.command.Parameters.AddWithValue("@L", lunes);
                db.command.Parameters.AddWithValue("@M", martes);
                db.command.Parameters.AddWithValue("@MI", miercoles);
                db.command.Parameters.AddWithValue("@J", jueves);
                db.command.Parameters.AddWithValue("@V", viernes);
                db.command.Parameters.AddWithValue("@S", sabado);
                db.command.Parameters.AddWithValue("@D", domingo);
                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                db.command.Parameters.AddWithValue("@PK", _pk);
                ban = db.execute();
            }
            
            if (ban)
            {
                if (opc == 2)
                {
                    string sql0 = "DELETE FROM CORRIDAS_RUTAS_DETALLES WHERE PK_CORRIDA_RUTA=@PK ";
                    db.PreparedSQL(sql0);
                    db.command.Parameters.AddWithValue("@PK",_pk);
                    db.execute();
                }
                    string sql2 = "INSERT INTO CORRIDAS_RUTAS_DETALLES " +
                                  "(PK_CORRIDA_RUTA,PK_ORIGEN,SALIDA,PK_DESTINO,LLEGADA,ESCALA,COMPLETO,PK_ORIGEN_COMPLETO,SALIDA_COMPLETO,PK_DESTINO_COMPLETO,LLEGADA_COMPLETO,USUARIO) " +
                                  "VALUES(@CORRIDARUTA,@ORIGEN,@SALIDA,@DESTINO,@LLEGADA,@ESCALA,@COMPLETO,@PK_ORIGEN_COMPLETO,@SALIDA_COMPLETO,@PK_DESTINO_COMPLETO,@LLEGADA_COMPLETO,@USUARIO) ";
                    string sql3 = " SELECT * FROM RUTAS_DESTINOS WHERE PK_RUTA=@PK";
                /*START obtengo el completo para saber el origen al cual no se debe desfazar al salida*/
                string sql4 = "SELECT PK_ORIGEN,PK_DESTINO,TIEMPO FROM RUTAS_DESTINOS WHERE PK_RUTA=@PK AND COMPLETO=1";
                    db.PreparedSQL(sql4);
                db.command.Parameters.AddWithValue("@PK",ruta_pk);
                    ResultSet res4 = db.getTable();
                if (res4.Next()) {
                    pkOrigenCompleto = int.Parse(res4.Get("PK_ORIGEN"));
                    pkDestinoCompleto = int.Parse(res4.Get("PK_DESTINO"));
                    tiempoCompleto = res4.Get("TIEMPO");
                } else {
                    sql4 = "SELECT RUTA, DESCRIPCION, LINEA FROM RUTAS " +
                           "INNER JOIN LINEAS ON(PK1= LINEA_PK) WHERE PK = @PK";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", ruta_pk);
                    res4 = db.getTable();
                    if (res4.Next()) {
                        MessageBox.Show("Ruta: " + res4.Get("RUTA") +", descripcion: "+ res4.Get("DESCRIPCION") + ", Linea: " +res4.Get("LINEA") + "no contiene completo");
                    }
                }
                /*END obtengo el completo para saber el origen al cual no se debe desfazar al salida*/
                db.PreparedSQL(sql3);
                db.command.Parameters.AddWithValue("@PK", ruta_pk);
                ResultSet res = db.getTable();
                while (res.Next())
                {
                    string llegada = "00:00";
                    if (pkOrigenCompleto != -1 && !res.Get("PK_ORIGEN").Equals(pkOrigenCompleto.ToString()))
                    {
                        string sql5 = "SELECT TIEMPO FROM RUTAS_DESTINOS WHERE PK_RUTA=@PKRUTA AND PK_ORIGEN=@ORIGEN AND PK_DESTINO = @DESTINO ";
                        db.PreparedSQL(sql5);
                        db.command.Parameters.AddWithValue("@PKRUTA", ruta_pk);
                        db.command.Parameters.AddWithValue("@DESTINO", res.Get("PK_ORIGEN"));
                        db.command.Parameters.AddWithValue("@ORIGEN", pkOrigenCompleto);
                        ResultSet resultTiempo = db.getTable();
                        string tiempo1 = "00:00";
                        if (resultTiempo.Next()) { tiempo1 = resultTiempo.Get("TIEMPO"); }
                        salida2 = sumaHoras(salida, TIEMPO_CARGA);
                        salida2 = sumaHoras(tiempo1, salida2);
                        llegada = sumaHoras(salida, res.Get("TIEMPO"));
                        llegada = sumaHoras(tiempo1, llegada);

                    }
                    else
                    {
                        salida2 = salida;
                        llegada = sumaHoras(res.Get("TIEMPO"), salida);
                    }
                    //string llegada = sumaHoras(res.Get("TIEMPO"), salida);
                    db.PreparedSQL(sql2);
                    db.command.Parameters.AddWithValue("@CORRIDARUTA", _pk);
                    db.command.Parameters.AddWithValue("@ORIGEN", res.Get("PK_ORIGEN"));
                    db.command.Parameters.AddWithValue("@SALIDA", salida2);
                    db.command.Parameters.AddWithValue("@DESTINO", res.Get("PK_DESTINO"));
                    db.command.Parameters.AddWithValue("@LLEGADA", llegada);
                    db.command.Parameters.AddWithValue("@ESCALA", res.Get("COMPLETO"));
                    db.command.Parameters.AddWithValue("@COMPLETO", res.Get("COMPLETO"));

                    db.command.Parameters.AddWithValue("@PK_ORIGEN_COMPLETO", pkOrigenCompleto);
                    db.command.Parameters.AddWithValue("@SALIDA_COMPLETO", salida);
                    db.command.Parameters.AddWithValue("@PK_DESTINO_COMPLETO", pkDestinoCompleto);
                    db.command.Parameters.AddWithValue("@LLEGADA_COMPLETO", sumaHoras(tiempoCompleto,salida));

                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                    db.execute();

                }

                return true;
            }
            else
            {
                MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
            }
            

            return false;

        }

        private void Rolado_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            timer1.Interval = 1;
            timer1.Start();

        }

        public void getRows3(string pkRol = "",string dia="")
        {
            progressBar2.Show();
            progressBar2.Minimum = 0;
            progressBar2.Maximum = 100;
            progressBar2.Value = 20;
            try
            {

                int count = 1;
                string sql = "SELECT * FROM VCORRIDAS_DETALLES WHERE 1=1 ";

                if (!string.IsNullOrEmpty(pkRol))
                {
                    sql += " AND PK_ROL = @PKROL ";
                }
                if (!string.IsNullOrEmpty(dia))
                {
                    sql += " AND PK_CORRIDA_RUTA in(SELECT PK FROM CORRIDAS_RUTAS WHERE "+dia+" = 1) ";
                }
                
                sql += " ORDER BY CORRIDA, PK_CORRIDA_RUTA ASC";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PKROL", pkRol);
                res = db.getTable();
                
                string valor = string.Empty;
                Color colorActual = Color.FromName("Highlight");
                progressBar2.Maximum = res.Count+20;
                progressBar2.Step = 1;
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
                    dataGridView3.Rows[n3].Cells["escalaDetalle"].Value= res.Get("ESCALA");
                    //dataGridView3.Rows[n3].Cells[7].Value = sumaHoras(res.Get("SALIDA"), res.Get("TIEMPO"));
                    dataGridView3.Rows[n3].Cells["rutaDescripcionDetalle"].Value = res.Get("RUTA");
                    dataGridView3.Rows[n3].Cells["ecoAutobusDetalle"].Value = res.Get("ECO");
                    dataGridView3.Rows[n3].Cells["corridaDetalle"].Value = res.Get("CORRIDA");
                    dataGridView3.Rows[n3].Cells["fechaC3"].Value = "";//res.Get("FECHA_C");
                    dataGridView3.Rows[n3].Cells["fechaM3"].Value = "";//res.Get("FECHA_M");
                    dataGridView3.Rows[n3].Cells["modifico3"].Value = "";//res.Get("USUARIO");
                    dataGridView3.Rows[n3].Cells["pkCorridaRuta"].Value = res.Get("PK_CORRIDA_RUTA");
                    dataGridView3.Rows[n3].Cells["completo"].Value = res.Get("COMPLETO");

                    if (n3 == 0) {
                        colorActual = Color.FromName("Highlight");
                    }
                    else if (!dataGridView3.Rows[n3].Cells["corridaDetalle"].Value.ToString().Equals(dataGridView3.Rows[n3-1].Cells["corridaDetalle"].Value.ToString())) {
                        colorActual = colorActual == Color.FromName("Highlight") ? Color.FromArgb(38, 45, 56) : Color.FromName("Highlight");
                    }
                         
                    dataGridView3.Rows[n3].DefaultCellStyle.BackColor = colorActual;
                    count++;

                }
                //n = dataGridView1.Rows.Add();
            }
            catch (Exception e)
            {
                LOG.write("ROLADO", "getRows1", e.Message);
            }

            progressBar2.Value = 0;
            progressBar2.Hide();
        }
        
        public string sumaHoras(string tiempo1, string tiempo2) {

            TimeSpan salida = TimeSpan.Parse(tiempo1);
            TimeSpan tiempo = TimeSpan.Parse(tiempo2);
            salida = salida.Add(tiempo);


            return salida.ToString(@"hh\:mm");
        }

        public void getRows0(string search = "")
        {
            try
            {

                int count = 1;
                string sql = "SELECT * FROM VROLADOS ";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += "WHERE ROL LIKE @SEARCH OR LINEA LIKE @SEARCH";
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

                    n0 = dataGridView0.Rows.Add();

                    dataGridView0.Rows[n0].Cells[0].Value = res.Get("PK");
                    dataGridView0.Rows[n0].Cells[1].Value = count;
                    dataGridView0.Rows[n0].Cells[2].Value = res.Get("PK_LINEA");
                    dataGridView0.Rows[n0].Cells[3].Value = res.Get("LINEA");
                    dataGridView0.Rows[n0].Cells[4].Value = res.Get("ROL");
                    dataGridView0.Rows[n0].Cells[5].Value = res.Get("FECHA_C");
                    dataGridView0.Rows[n0].Cells[6].Value = res.Get("FECHA_M");
                    dataGridView0.Rows[n0].Cells[7].Value = res.Get("USUARIO");

                    count++;

                }

            }
            catch (Exception e)
            {
                LOG.write("ROLADO", "getRows", e.Message);
            }

        }

        private void MenuAdd0_Click(object sender, EventArgs e)
        {
            progressBar3.Show();
            progressBar3.Value = 20;
            try
            {
                if (ValidarInputs(0))
                {

                    progressBar3.Value = 40;

                    string sql = "INSERT INTO ROLADOS(PK_LINEA,ROL,USUARIO) VALUES(@LINEA,@ROL,@USUARIO)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@LINEA", _linea_pk);
                    db.command.Parameters.AddWithValue("@ROL", _rol);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                    progressBar3.Value = 70;

                    if (db.execute())
                    {
                        progressBar3.Value = 90;

                        dataGridView0.Rows.Clear();
                        dataGridView0.Refresh();
                        getRows0();
                        cleanForm(0);
                        getDatosAdicionales();
                        controles(0);
                        progressBar3.Value = 100;

                    }
                    else
                    {
                        MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                    }
                }
            }
            catch (Exception ex) {
                if (!Utilerias.Utilerias.CheckForInternetConnection()) {
                    MessageBox.Show("¡Verifique su conexion a internet!");
                }else
                {
                    MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                }
                LOG.write("Rolado", "MenuAdd0_Click", ex.ToString());
            }
            progressBar3.Hide();
            progressBar3.Value = 0;
        }

        private void MenuRefresh0_Click(object sender, EventArgs e)
        {
            dataGridView0.Rows.Clear();
            dataGridView0.Refresh();
            getRows0();
            cleanForm(0);
            controles(0);
        }

        private void MenuDelete0_Click(object sender, EventArgs e)
        {
            progressBar3.Show();
            progressBar3.Value = 20;

            try
            {
                var confirmResult = MessageBox.Show("¿Esta seguro de eliminar el registro?",
                                     "Confirmar eliminar",
                                     MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    progressBar3.Value = 40;

                    string sql;
                    sql = "DELETE FROM ROLADOS WHERE PK = @PK";
                    _pkRol = int.Parse(txtPkRol.Text);
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", _pkRol);
                    progressBar3.Value = 50;

                    if (db.execute())
                    {
                        progressBar3.Value = 60;

                        sql = "DELETE FROM CORRIDAS WHERE PK_ROL=@ROL";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@ROL", _pkRol);
                        db.execute();
                        progressBar3.Value = 70;

                        sql = "DELETE FROM ROLADOS_CORRIDAS WHERE PK_ROL=@ROL";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@ROL", _pkRol);
                        db.execute();
                        progressBar3.Value = 80;

                        dataGridView0.Rows.Clear();
                        dataGridView0.Refresh();
                        getRows0();
                        cleanForm(0);
                        getDatosAdicionales();
                        controles(0);
                        progressBar3.Value = 100;

                    }
                    else
                    {
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
                string error = err.ToString();
                LOG.write("ROLADO", "MenuDelete_Click", error);
            }
            progressBar3.Hide();
            progressBar3.Value = 0;
        }

        private void MenuSave0_Click(object sender, EventArgs e)
        {
            progressBar3.Show();
            progressBar3.Value = 20;
            try
            {
                if (ValidarInputs(0))
                {
                    progressBar3.Value = 30;

                    string sql = "UPDATE ROLADOS SET PK_LINEA=@LINEA, ROL=@ROL, USUARIO=@USUARIO WHERE PK=@PK";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@LINEA", _linea_pk);
                    db.command.Parameters.AddWithValue("@ROL", _rol);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                    db.command.Parameters.AddWithValue("@PK", _pkRol);
                    progressBar3.Value = 60;

                    if (db.execute())
                    {
                        progressBar3.Value = 80;

                        dataGridView0.Rows.Clear();
                        dataGridView0.Refresh();
                        getRows0();
                        cleanForm(0);
                        getDatosAdicionales();
                        controles(0);
                        progressBar3.Value = 100;

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
                else
                {
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                }
                string error = ex.ToString();
                LOG.write("ROLADO", "MenuDelete_Click", error);
            }
            progressBar3.Hide();
            progressBar3.Value = 0;
        }

        private void DataGridView0_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            n0 = e.RowIndex;

            if (n0 != -1)
            {
                txtPkRol.Text = (string)dataGridView0.Rows[n0].Cells[0].Value;
                comboLinea.Text = dataGridView0.Rows[n0].Cells[3].Value.ToString();
                txtRol.Text = (string)dataGridView0.Rows[n0].Cells[4].Value;
                controles(1);
            }
        }

        private void MenuExportExcel0_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridView0);
        }

        private void SearchEnter0(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MenuSearch0_Click(sender, e);
            }
        }

        private void MenuSearch0_Click(object sender, EventArgs e)
        {

            dataGridView0.Rows.Clear();
            dataGridView0.Refresh();
            getRows0(_search);
        }

        private void CloseSesion_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.cerrarSesion(this);
        }

        private void MenuClose0_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataGridView0_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            progressBar4.Show();
            progressBar4.Value = 20;
            tb1.SelectedIndex = 1;

            n0 = e.RowIndex;
            _pkRol = int.Parse(txtPkRol.Text);
            string _dia = (comboDia.SelectedItem!=null)?(comboDia.SelectedItem as ComboboxItem).Value.ToString():"";

            if (n0 != -1)
            {
                progressBar4.Value = 40;

                try
                {
                    rutasUpdate.Clear();//limpiar antes de usar datagridView1.Clear(); para que la funcion de remove no elimine los elementos
                    dataGridView2.DataSource = null;
                    dataGridView2.Rows.Clear();
                }
                catch (Exception ex) { String error = ex.Message; }
                progressBar4.Value = 50;


                getDatosAdicionales(1);
                progressBar4.Value = 65;

                comboRol.Text = (string)dataGridView0.Rows[n0].Cells[4].Value;
                getRows1(_pkRol.ToString());
                progressBar4.Value = 85;

                /*PARA OPTIMIZAR SE COMENTAN 4 LINEAS PARA QUE NO RECARGE EL GRID VIEW DETALLE
                comboLineaDetalle.Text=(string)dataGridView0.Rows[n0].Cells[3].Value;
                comboRolDetalle.Text=(string)dataGridView0.Rows[n0].Cells[4].Value;
                dataGridView3.Rows.Clear();
                getRows3(_pkRol.ToString(),_dia);
                */
                getDatosAdicionales(2, (string)dataGridView0.Rows[n0].Cells["lineaPk"].Value);
                progressBar4.Value = 100;

                controles(2);

            }
            progressBar4.Hide();
            progressBar4.Value = 0;
        }

        public void getRows4(string pk_rol = "", string pk_corrida="")
        {
            try
            {

                int count = 1;
                string sql = "SELECT * FROM VCORRIDAS_DETALLE ";


                if (!string.IsNullOrEmpty(pk_rol) && !string.IsNullOrEmpty(pk_corrida))
                {
                    sql += "WHERE PK_ROL = @ROL and PK_CORRIDA = @CORRIDA ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@ROL", pk_rol);
                    db.command.Parameters.AddWithValue("@CORRIDA", pk_corrida);
                }
                else if (!string.IsNullOrEmpty(pk_rol))
                {
                    sql += "WHERE PK_ROL = @ROL ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@ROL", pk_rol);
                }
                else if (!string.IsNullOrEmpty(pk_corrida))
                {
                    sql += "WHERE PK_CORRIDA = @CORRIDA ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@CORRIDA", pk_corrida);
                }
                else
                {
                    db.PreparedSQL(sql);

                }

                res = db.getTable();

                while (res.Next())
                {

                    n3 = dataGridView3.Rows.Add();

                    //dataGridView3.Rows[n3].Cells[0].Value = res.Get("PK");
                    dataGridView3.Rows[n3].Cells[1].Value = count;
                    dataGridView3.Rows[n3].Cells[2].Value = res.Get("PK_ORIGEN");
                    dataGridView3.Rows[n3].Cells[3].Value = res.Get("ORIGEN");
                    dataGridView3.Rows[n3].Cells[5].Value = res.Get("PK_DESTINO");
                    dataGridView3.Rows[n3].Cells[6].Value = res.Get("DESTINO");
                    /*
                    dataGridView3.Rows[n3].Cells[17].Value = res.Get("FECHA_C");
                    dataGridView3.Rows[n3].Cells[18].Value = res.Get("FECHA_M");
                    dataGridView3.Rows[n3].Cells[19].Value = res.Get("USUARIO");
                    */
                    count++;

                }

            }
            catch (Exception e)
            {
                LOG.write("ROLADO", "getRows", e.Message);
            }

        }

        private void BtnSave2_Click(object sender, EventArgs e)
        {

            saveRutasCorridas();
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            rutasUpdate.Clear();//limpiar antes de usar datagridView1.Clear(); para que la funcion de remove no elimine los elementos
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            getRows1(_pkRol.ToString());
            cleanForm(1);
            //getDatosAdicionales();
            controles(0);
        }

        private void DataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            n2 = e.RowIndex;

            if (n2>-1 && e.ColumnIndex == 3)
            {
                
                string stringValue = dataGridView2.Rows[n2].Cells["Salida"].Value.ToString();
                stringValue = stringValue.Replace(":", "");
                if (stringValue != null && stringValue.Length == 4)
                {
                    dataGridView2.Rows[n2].Cells[3].Value = stringValue.Substring(0, 2) + ":" + stringValue.Substring(2, 2);
                }
                else if (stringValue != null && stringValue.Length == 3)
                {
                    dataGridView2.Rows[n2].Cells[3].Value = "0" + stringValue.Substring(0, 1) + ":" + stringValue.Substring(1, 2);
                }
                else if (stringValue != null && stringValue.Length == 2)
                {
                    dataGridView2.Rows[n2].Cells[3].Value = stringValue + ":00";
                }
                else if (stringValue != null && stringValue.Length == 1)
                {
                    dataGridView2.Rows[n2].Cells[3].Value = "0" + stringValue + ":00";
                }
            }
            /*
            if (n2>-1 && dataGridView2.Rows[n2].Cells["Salida"].Value != null 
                && dataGridView2.Rows[n2].Cells["Ruta"].Value != null 
                && !String.IsNullOrEmpty(txtpkCorrida.Text) && nuevo)
            {
                saveRutasCorridas(n2);
            }*/
        }

        private void ComboLinea_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;

            string pk_linea = (combo.SelectedItem as ComboboxItem).Value.ToString();
            getDatosAdicionales(1, pk_linea);
            getDatosAdicionales(2, pk_linea);
            try
            {
                if (dataGridView2.Rows != null)
                {
                    rutasUpdate.Clear();//limpiar antes de usar datagridView1.Clear(); para que la funcion de remove no elimine los elementos
                    dataGridView2.DataSource = null;
                    dataGridView2.Rows.Clear();
                }
            }
            catch (Exception ex) {
                LOG.write("Rolado.cs", "ComboLinea_SelectedIndexChanged_1", "Error al limpiar data grid 2:"+ex.Message);
            }
            getDatosAdicionales(3, pk_linea);


        }

        private void DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnSaveDetalles_Click(object sender, EventArgs e)
        {
            saveRutasCorridas();
        }

        private void BtnGuardar3_Click(object sender, EventArgs e)
        {
            saveRutasCorridasDetalle();
        }

        public bool saveRutasCorridasDetalle() {
            int max = 0;
            string sql = "UPDATE CORRIDAS_RUTAS_DETALLES SET SALIDA=@SALIDA, LLEGADA=@LLEGADA, ESCALA=@ESCALA, USUARIO=@USUARIO " +
                         "WHERE PK=@PK ";
            if (dataGridView3.Rows != null) { max = dataGridView3.Rows.Count; }
            progressBar2.Visible = true; progressBar2.Minimum = 1; progressBar2.Maximum = max;
             progressBar2.Value = 1; progressBar2.Step = 1;

            foreach (DataGridViewRow row in dataGridView3.Rows) {
                progressBar2.PerformStep();
                if ((row.Cells["pkDetalle"].Value != null && !String.IsNullOrEmpty(row.Cells["pkDetalle"].Value.ToString())) && row.Cells["llegadaDetalle"].Value != null && row.Cells["horaDetalle"].Value !=null)
                {
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SALIDA", row.Cells["horaDetalle"].Value.ToString());
                    db.command.Parameters.AddWithValue("@LLEGADA", row.Cells["llegadaDetalle"].Value.ToString());
                    db.command.Parameters.AddWithValue("@Escala", row.Cells["escalaDetalle"].Value.ToString());
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                    db.command.Parameters.AddWithValue("@PK", row.Cells["pkDetalle"].Value.ToString());
                    db.execute();

                    if (Boolean.Parse(row.Cells["completo"].Value.ToString())) {
                        string sql1 = "UPDATE CORRIDAS_RUTAS_DETALLES " +
                                      "SET PK_ORIGEN_COMPLETO=@PKORIGENCOMPLETO,SALIDA_COMPLETO=@SALIDACOMPLETO," +
                                      "PK_DESTINO_COMPLETO=@PKDESTINOCOMPLETO,LLEGADA_COMPLETO=@LLEGADACOMPLETO " +
                                      "WHERE PK_CORRIDA_RUTA =@PKCORRIDARUTA";
                        db.PreparedSQL(sql1);
                        db.command.Parameters.AddWithValue("@PKORIGENCOMPLETO", row.Cells["pkOrigenDetalle"].Value.ToString());
                        db.command.Parameters.AddWithValue("@SALIDACOMPLETO", row.Cells["horaDetalle"].Value.ToString());
                        db.command.Parameters.AddWithValue("@PKDESTINOCOMPLETO", row.Cells["pkDestinoDetalle"].Value.ToString());
                        db.command.Parameters.AddWithValue("@LLEGADACOMPLETO", row.Cells["llegadaDetalle"].Value.ToString());
                        db.command.Parameters.AddWithValue("@PKCORRIDARUTA ", row.Cells["pkCorridaRuta"].Value.ToString());
                        db.execute();
                    }

                }
            }

            progressBar2.Visible = false;
            return false;
        }

        private void MenuSaveDetalle_Click(object sender, EventArgs e)
        {
            saveRutasCorridasDetalle();
        }

        private void MenuRefreshDetalle_Click(object sender, EventArgs e)
        {
            /*
            string linea = (comboLineaDetalle.SelectedItem!=null)?(comboLineaDetalle.SelectedItem as ComboboxItem).Value.ToString():"";
            string rol= (comboRolDetalle.SelectedItem!=null)?(comboRolDetalle.SelectedItem as ComboboxItem).Value.ToString():"";
            string dia= (comboDia.SelectedItem!=null)?(comboDia.SelectedItem as ComboboxItem).Value.ToString():"";
            dataGridView3.Rows.Clear();
            getRows3(rol,dia);*/
            updateDataGrid();
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
                int i = 2, j = 1, num = 1 ;
                while (res.Next()) {
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
                    else if (!corridaActual.Equals(res.Get("CORRIDA"))) {
                        i++;j = 1;num++;
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

        private void ToolStripButton6_Click(object sender, EventArgs e)
        {
            ExportarDataGridViewExcel();
        }

        private void ComboLineaDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
            string linea=(comboLineaDetalle.SelectedItem as ComboboxItem).Value.ToString();
            getDatosAdicionales(4, linea);
        }

        private void ComboRolDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDataGrid();
        }

        private void Export2_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridView3);
        }

        private void ComboDia_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDataGrid();
        }

        public void updateDataGrid() {

            string linea = (comboLineaDetalle.SelectedItem !=null)?(comboLineaDetalle.SelectedItem as ComboboxItem).Value.ToString():"";
            string rol = comboRolDetalle.SelectedItem!=null?(comboRolDetalle.SelectedItem as ComboboxItem).Value.ToString():"";
            string dia = comboDia.SelectedItem!=null?(comboDia.SelectedItem as ComboboxItem).Value.ToString():"";

            if (!string.IsNullOrEmpty(rol) && !string.IsNullOrEmpty(dia))
            {
                dataGridView3.Rows.Clear();
                getRows3(rol, dia);
            }


        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                timer1.Stop();
                progressBar3.Show();
                progressBar3.Value = 20;

                rutasAdd = new List<int>();
                rutasDeleted = new List<int>();
                rutasUpdate = new List<int>();
                //dataGridView0.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
                dataGridView0.EnableHeadersVisualStyles = false;
                //dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
                dataGridView1.EnableHeadersVisualStyles = false;
                //dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
                dataGridView2.EnableHeadersVisualStyles = false;
                //dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
                dataGridView3.EnableHeadersVisualStyles = false;
                progressBar3.Value = 40;

                db = new database();

                DoubleBuffered(dataGridView0, true);
                DoubleBuffered(dataGridView1, true);
                DoubleBuffered(dataGridView2, true);
                DoubleBuffered(dataGridView3, true);
                progressBar3.Value = 60;

                getRows0();
                progressBar3.Value = 80;

                getDatosAdicionales();
                getDatosAdicionales(1);
                controles(0);
                _linea_pk = 1;
                //llenaDatos();

                comboDia.Items.Clear();
                ComboboxItem aux = new ComboboxItem();
                aux.Value = "L";
                aux.Text = "Lunes";
                comboDia.Items.Add(aux);
                aux = new ComboboxItem();
                aux.Value = "M";
                aux.Text = "Martes";
                comboDia.Items.Add(aux);
                aux = new ComboboxItem();
                aux.Value = "MI";
                aux.Text = "Miercoles";
                comboDia.Items.Add(aux);
                aux = new ComboboxItem();
                aux.Value = "J";
                aux.Text = "Jueves";
                comboDia.Items.Add(aux);
                aux = new ComboboxItem();
                aux.Value = "V";
                aux.Text = "Viernes";
                comboDia.Items.Add(aux);
                aux = new ComboboxItem();
                aux.Value = "S";
                aux.Text = "Sabado";
                comboDia.Items.Add(aux);
                aux = new ComboboxItem();
                aux.Value = "D";
                aux.Text = "Domingo";
                comboDia.Items.Add(aux);
                //comboDia.SelectedIndex = 0;
                progressBar3.Value = 90;

                string pkLinea = (comboLinea.SelectedItem as ComboboxItem).Value.ToString();
                DataGridViewComboBoxColumn comboboxColumn = dataGridView2.Columns["Ruta"] as DataGridViewComboBoxColumn;
                comboboxColumn.DataSource = Populate("Select  RUTA,PK FROM VRUTAS WHERE LINEA_PK = " + pkLinea + " ORDER BY RUTA");
                comboboxColumn.DisplayMember = "RUTA";
                comboboxColumn.ValueMember = "PK";
                progressBar3.Value = 100;

                cleanForm(1);
                controles(2);
            }
            catch (Exception eX)
            {
                LOG.write("ROLADO", "getRows", eX.Message);
            }
            progressBar3.Hide();
            progressBar3.Value = 0;
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

        private void Rolado_Shown(object sender, EventArgs e)
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

        private void GroupBox2_Enter(object sender, EventArgs e)
        {

        }

    }
}
