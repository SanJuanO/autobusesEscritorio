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
    public partial class Rutas : Form
    {

        public database db;
        ResultSet res = null;

        private int n = 0;
        private int nd = 0;
        private int _pk1;
        private string _ruta;
        private string _descripcion;
        private string _linea_pk;
        private string _tarjetas;
        private string _casetas;
        private string _sueldo;
        private string _fecha_c;
        private string _fecha_m;
        private string _usuario;
        private string _search;

        private int _pk_detalle;
        private int _comboRuta;
        private string _comboRutaTexto;
        private int _pk_origen;
        private int _pk_destino;
        private float _precio;
        private int _kms;
        private string _tiempo;
        private string _completo;


        public Rutas()
        {
            InitializeComponent();
            this.Show();
            btnMaximizar.Visible = false;
            btnNormal.Visible = true;
            titulo.Text = "Rutas";
            progressBar1.Hide();
            progressBar2.Hide();
        }

        public void getRows(string search = "", string linea="")
        {
            try
            {
                try { dataGridView.Rows.Clear(); } catch { }
                int count = 1;
                string sql = "SELECT * FROM VRUTAS WHERE BORRADO=0 ";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND (RUTA LIKE @SEARCH OR DESCRIPCION LIKE @SEARCH) ";
                }
                if (!string.IsNullOrEmpty(linea)) {
                    sql += " AND LINEA_PK = @LINEA";
                }
                sql += " ORDER BY RUTA";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@SEARCH", "%" + search + "%");
                db.command.Parameters.AddWithValue("@LINEA", linea);

                res = db.getTable();

                while (res.Next())
                {

                    n = dataGridView.Rows.Add();

                    dataGridView.Rows[n].Cells["pk1"].Value = res.Get("PK");
                    dataGridView.Rows[n].Cells["No"].Value = count;
                    dataGridView.Rows[n].Cells["ruta"].Value = res.Get("RUTA");
                    dataGridView.Rows[n].Cells["Descripcion"].Value = res.Get("DESCRIPCION");
                    dataGridView.Rows[n].Cells["linea_pk"].Value = res.Get("LINEA_PK");
                    dataGridView.Rows[n].Cells["Linea"].Value = res.Get("LINEA");
                    dataGridView.Rows[n].Cells["Tarjetas"].Value = res.Get("TARJETAS");
                    dataGridView.Rows[n].Cells["Casetas"].Value = res.Get("CASETAS");
                    dataGridView.Rows[n].Cells["Sueldo"].Value = res.Get("SUELDO");
                    dataGridView.Rows[n].Cells["fecha_c"].Value = res.Get("FECHA_C");
                    dataGridView.Rows[n].Cells["fecha_m"].Value = res.Get("FECHA_M");
                    dataGridView.Rows[n].Cells["modifico"].Value = res.Get("USUARIO");

                    count++;
                }

            }
            catch (Exception e)
            {
                string error = e.Message;
                LOG.write("Rutas", "getRows", e.Message, "Usuario");

            }

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
                case 3:

                    btnAddDetalle.Enabled = true;
                    btnCleanDetalle.Enabled = true;
                    btnDeleteDetalle.Enabled = false;
                    btnSaveDetalle.Enabled = false;

                    btnAddDetalle.BackColor = Color.FromArgb(38, 45, 56);
                    btnCleanDetalle.BackColor = Color.FromArgb(38, 45, 56);
                    btnDeleteDetalle.BackColor = Color.White;
                    btnSaveDetalle.BackColor = Color.White;

                    break;
                case 4:

                    btnAddDetalle.Enabled = false;
                    btnCleanDetalle.Enabled = true;
                    btnDeleteDetalle.Enabled = true;
                    btnSaveDetalle.Enabled = true;

                    btnAddDetalle.BackColor = Color.White;
                    btnCleanDetalle.BackColor = Color.FromArgb(38, 45, 56);
                    btnDeleteDetalle.BackColor = Color.FromArgb(38, 45, 56);
                    btnSaveDetalle.BackColor = Color.FromArgb(38, 45, 56);

                    break;
            }

        }

        public void getDatosAdicionales(int opc=0,string linea="")
        {
            ComboboxItem linea_seleccionada=null;
            comboRuta.Items.Clear();
            comboOrigen.Items.Clear();
            comboDestino.Items.Clear();
            string sql = string.Empty;

            sql = "SELECT * FROM RUTAS WHERE BORRADO=0";
            if (!string.IsNullOrEmpty(linea)) {
                sql += " AND LINEA_PK=@LINEA";
            }
            sql += " ORDER BY DESCRIPCION";
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@LINEA",linea);
            res = db.getTable();

            while (res.Next())
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = res.Get("RUTA");
                item.Value = res.Get("PK");
                comboRuta.Items.Add(item);

                //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
            }

            sql = "SELECT * FROM DESTINOS WHERE BORRADO=0 ORDER BY DESTINO";
            db.PreparedSQL(sql);
            res = db.getTable();

            while (res.Next())
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = res.Get("DESTINO");
                item.Value = res.Get("PK1");
                comboOrigen.Items.Add(item);
                comboDestino.Items.Add(item);
                //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
            }

            if (opc == 0)
            {
                sql = "SELECT * FROM LINEAS WHERE BORRADO=0 ORDER BY LINEA";
                if (comboLinea.SelectedItem != null)
                {
                    linea_seleccionada = (ComboboxItem)comboLinea.SelectedItem;
                }
                comboLinea.Items.Clear();

                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("LINEA");
                    item.Value = res.Get("PK1");
                    comboLinea.Items.Add(item);

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }
                if (linea_seleccionada != null) { comboLinea.Text = linea_seleccionada.Text; }
                else { if (comboLinea.Items != null && comboLinea.Items.Count > 0) { comboLinea.SelectedIndex = 0; } }
            }

        }

        private Boolean ValidarInputs(int opc)
        {
            Boolean valido = true;
            if (opc==1) //valida 
            {
                _linea_pk = "";
                
                _pk1 = int.TryParse(txtpk1.Text,out int _pk11)?_pk11:0;
                
                _ruta = txtRuta.Text;
                _descripcion = txtDescripcion.Text;
                _tarjetas = txtTarjetas.Text;
                _casetas = txtCasetas.Text;
                _sueldo = txtSueldo.Text;

                if (comboLinea.Items.Count > 0 && comboLinea.SelectedItem != null && (comboLinea.SelectedItem as ComboboxItem).Value != null)
                {
                    _linea_pk = (comboLinea.SelectedItem as ComboboxItem).Value.ToString();
                }

                if (comboLinea.Items.Count == 0 || comboLinea.SelectedItem == null || string.IsNullOrEmpty((comboLinea.SelectedItem as ComboboxItem).Value.ToString()))
                {
                    MessageBox.Show("Es necesario ingresar liena!");
                    comboLinea.Focus();
                    valido = false;

                }
                else if (string.IsNullOrEmpty(_ruta))
                {
                    MessageBox.Show("Es necesario ingresar el nombre de la ruta!");
                    txtRuta.Focus();
                    valido = false;
                }
                else if (string.IsNullOrEmpty(_descripcion))
                {
                    MessageBox.Show("Es necesario ingresar la descripción de la ruta!");
                    txtDescripcion.Focus();
                    valido = false;
                }
                else if (string.IsNullOrEmpty(_tarjetas)) {
                    MessageBox.Show("Es necesario ingresar el monto de tarjetas para esta ruta!");
                    txtDescripcion.Focus();
                    valido = false;
                }else if (string.IsNullOrEmpty(_casetas)) {
                    MessageBox.Show("Es necesario ingresar el monto de casetas para esta ruta!");
                    txtDescripcion.Focus();
                    valido = false;
                }else if (string.IsNullOrEmpty(_sueldo)) {
                    MessageBox.Show("Es necesario ingresar el monto de sueldo para esta ruta!");
                    txtDescripcion.Focus();
                    valido = false;
                }

            }
            else if (opc == 2) {//validate Detalle
                _comboRuta = -1;
                _pk_origen = -1;
                _pk_destino = -1;
                _pk_detalle = -1;

                if (comboRuta.Items.Count > 0 && comboRuta.SelectedItem!=null && (comboRuta.SelectedItem as ComboboxItem).Value != null)
                {
                    _comboRuta = int.Parse((comboRuta.SelectedItem as ComboboxItem).Value.ToString());
                    _comboRutaTexto = (comboRuta.SelectedItem as ComboboxItem).Text.ToString();
                }
                if (comboOrigen.Items.Count > 0 && comboOrigen.SelectedItem!=null && (comboOrigen.SelectedItem as ComboboxItem).Value != null)
                {
                    _pk_origen = int.Parse((comboOrigen.SelectedItem as ComboboxItem).Value.ToString());
                }
                if (comboDestino.Items.Count > 0 && comboDestino.SelectedItem!=null &&(comboDestino.SelectedItem as ComboboxItem).Value != null)
                {
                    _pk_destino = int.Parse((comboDestino.SelectedItem as ComboboxItem).Value.ToString());
                }

                if (!String.IsNullOrEmpty(txtPkDetalle.Text))
                {
                    _pk_detalle = int.Parse(txtPkDetalle.Text);
                }
                _precio=int.TryParse(txtPrecio.Text, out int _precio1) ?_precio1:-1;
                _kms=int.TryParse(txtKm.Text, out int _km1) ? _km1 :  -1;
                _tiempo = txtTiempo.Text;
                _completo = checkBoxCompleta.Checked.ToString();


                if (_comboRuta==-1)
                {
                    MessageBox.Show("Es necesario ingresar la ruta!");
                    comboRuta.Focus();
                    valido = false;

                }
                else if (_pk_origen==-1)
                {
                    MessageBox.Show("Es necesario ingresar el lugar de origen!");
                    comboOrigen.Focus();
                    valido = false;

                }
                else if (_pk_destino==-1)
                {
                    MessageBox.Show("Es necesario ingresar el lugar de destino!");
                    comboDestino.Focus();
                    valido = false;

                }
                else if (_precio==-1)
                {
                    MessageBox.Show("Es necesario ingresar el precio!");
                    txtPrecio.Focus();
                    valido = false;

                }
                else if (_kms==-1)
                {
                    MessageBox.Show("Es necesario ingresar los kilometros!");
                    txtKm.Focus();
                    valido = false;

                }
                else if (string.IsNullOrEmpty(_tiempo))
                {
                    MessageBox.Show("Es necesario ingresar el tiempo!");
                    txtTiempo.Focus();
                    valido = false;

                }
            }

            return valido;
        }

        private void cleanForm(int opc)
        {
            if (opc == 1)
            {
                txtpk1.Text = "";
                txtRuta.Text = "";
                txtDescripcion.Text = "";
                txtTarjetas.Text = "";
                txtCasetas.Text = "";
                txtSueldo.Text = "";
                search.Text = "";
                /*
                if (comboLinea.Items.Count > 0)
                {
                    comboLinea.SelectedIndex = 0;
                }
                */
                
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                if (comboLinea.SelectedItem != null)
                {
                    getDatosAdicionales(0, (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
                }
                else { getDatosAdicionales(0); }

            }
            else if (opc == 2) { // clean detalle
                txtPrecio.Text = "";
                txtKm.Text = "";
                txtTiempo.Text = "";
                checkBoxCompleta.Checked = false;
                /*
                if (comboOrigen.Items.Count > 0)
                {
                    comboOrigen.SelectedIndex = 0;
                }
                if (comboDestino.Items.Count > 0)
                {
                    comboDestino.SelectedIndex = 0;
                }
                */
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                if (comboLinea.SelectedItem != null && (comboLinea.SelectedItem as ComboboxItem).Value != null)
                {
                    getDatosAdicionales(1, (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
                }
                else { getDatosAdicionales(1); }
            }
            

        }

        private void MenuAdd_Click(object sender, EventArgs e)
        {
            progressBar1.Show();
            progressBar1.Value = 20;
            try
            {
                if (ValidarInputs(1))
                {
                    progressBar1.Value = 40;
                    string sql1 = "SELECT COUNT(PK) MAX FROM RUTAS WHERE RUTA='"+_ruta+"' AND LINEA_PK='"+_linea_pk+"'";
                    if (db.Count(sql1) > 0) {
                        MessageBox.Show("¡Ruta ya esta en uso en esta linea porfavor cambie estos parametros!");
                        progressBar1.Value = 0;
                        progressBar1.Hide();
                        return;
                    }

                    string sql = "INSERT INTO RUTAS(RUTA,DESCRIPCION,LINEA_PK,TARJETAS,CASETAS,SUELDO,USUARIO) " +
                                 "VALUES(@RUTA,@DESCRIPCION,@LINEA,@TARJETAS,@CASETAS,@SUELDO,@USUARIO)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@RUTA", _ruta);
                    db.command.Parameters.AddWithValue("@DESCRIPCION", _descripcion);
                    db.command.Parameters.AddWithValue("@LINEA", _linea_pk);
                    db.command.Parameters.AddWithValue("@TARJETAS", _tarjetas);
                    db.command.Parameters.AddWithValue("@CASETAS", _casetas);
                    db.command.Parameters.AddWithValue("@SUELDO", _sueldo);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.PkUsuario);
                    progressBar1.Value = 60;
                    if (db.execute())
                    {
                        dataGridView.Rows.Clear();
                        dataGridView.Refresh();
                        cleanForm(1);
                        progressBar1.Value = 80;
                        if (comboLinea.SelectedItem != null)
                        {
                            getDatosAdicionales(0, (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
                        }
                        else { getDatosAdicionales(0); }
                        if (comboLinea.SelectedItem != null)
                        {
                            getRows("", (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
                        }
                        else { getRows(); }
                        controles(0);
                        progressBar1.Value = 100;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                    }
                }
            }
            catch (Exception ex) {
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡Verifica tu conexion a internet!");
                }
                else {
                    MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                }
                LOG.write("Rutas", "MenuAdd_Click", ex.ToString());
            }
            progressBar1.Hide();
            progressBar1.Value = 0;
        }

        private void MenuRefresh_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            dataGridView.Refresh();
            if (comboLinea.SelectedItem != null)
            {
                getRows("", (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
            }
            else { getRows(); }
            cleanForm(1);
            controles(0);
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            getRowsDetalle();
            cleanForm(2);
            controles(3);
        }

        private void MenuDelete_Click(object sender, EventArgs e)
        {
            progressBar1.Show();
            progressBar1.Value = 20;
            try
            {
                var confirmResult = MessageBox.Show("¿Esta seguro de eliminar el registro?",
                                     "Confirmar eliminar",
                                     MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    progressBar1.Value = 40;

                    string sql;
                    sql = "UPDATE RUTAS SET BORRADO=1,USUARIO=@USUARIO FROM RUTAS WHERE PK = @PK";
                    _pk1 = int.Parse(txtpk1.Text);
                    _comboRutaTexto = (comboRuta.SelectedItem as ComboboxItem).Text;
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.PkUsuario);
                    db.command.Parameters.AddWithValue("@PK", _pk1);
                    progressBar1.Value = 60;

                    if (db.execute())
                    {
                        progressBar1.Value = 80;

                        dataGridView.Rows.Clear();
                        dataGridView.Refresh();
                        if (comboLinea.SelectedItem != null)
                        {
                            getRows("", (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
                        }
                        else { getRows(); }
                        cleanForm(1);
                        progressBar1.Value = 90;

                        if (comboLinea.SelectedItem != null)
                        {
                            getDatosAdicionales(0, (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
                        }
                        else { getDatosAdicionales(0); }
                        progressBar1.Value = 90;
                        comboRuta.Text = _comboRutaTexto;

                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el registro, intente de nuevo.");
                    }
                }

            }
            catch (Exception ex)
            {
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡Verifica tu conexion a internet!");
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el registro, intente de nuevo.");
                }
                LOG.write("Rutas", "MenuDelete_Click", ex.ToString());
            }
            progressBar1.Hide();
            progressBar1.Value = 0;
        }

        private void MenuSave_Click(object sender, EventArgs e)
        {
            progressBar1.Show();
            progressBar1.Value = 20;
            try
            {
                if (ValidarInputs(1))
                {
                    progressBar1.Value = 40;

                    string sql1 = "SELECT COUNT(PK) MAX FROM RUTAS WHERE RUTA='" + _ruta + "' AND LINEA_PK='" + _linea_pk + "' AND NOT PK =" + _pk1 + "";
                    if (db.Count(sql1) > 0)
                    {
                        MessageBox.Show("¡Ruta ya esta en uso en esta linea porfavor cambie estos parametros!");
                        progressBar1.Value = 0;
                        progressBar1.Hide();
                        return;
                    }
                    string sql = "UPDATE RUTAS SET RUTA=@RUTA, DESCRIPCION=@DESCRIPCION, LINEA_PK=@LINEA, TARJETAS=@TARJETAS, CASETAS=@CASETAS, SUELDO=@SUELDO, USUARIO=@USUARIO WHERE PK=@PK1";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@RUTA", _ruta);
                    db.command.Parameters.AddWithValue("@DESCRIPCION", _descripcion);
                    db.command.Parameters.AddWithValue("@LINEA", _linea_pk);
                    db.command.Parameters.AddWithValue("@TARJETAS", _tarjetas);
                    db.command.Parameters.AddWithValue("@CASETAS", _casetas);
                    db.command.Parameters.AddWithValue("@SUELDO", _sueldo);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.PkUsuario);
                    db.command.Parameters.AddWithValue("@PK1", _pk1);
                    progressBar1.Value = 60;

                    if (db.execute())
                    {
                        dataGridView.Rows.Clear();
                        dataGridView.Refresh();
                        progressBar1.Value = 80;

                        if (comboLinea.SelectedItem != null)
                        {
                            getRows("", (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
                        }
                        else { getRows(); }
                        cleanForm(1);
                        progressBar1.Value = 90;

                        if (comboLinea.SelectedItem != null)
                        {
                            getDatosAdicionales(0, (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
                        }
                        else { getDatosAdicionales(0); }
                        progressBar1.Value = 100;

                        controles(0);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el registro, intente de nuevo.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡Verifica tu conexion a internet!");
                }
                else
                {
                    MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                }
                LOG.write("Rutas", "MenuSave_Click", ex.ToString());
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
            _search = search.Text;
            if (comboLinea.SelectedItem != null)
            {
                getRows(_search, (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
            }
            else { getRows(_search); }
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

        private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            n = e.RowIndex;

            if (n != -1)
            {
                cleanForm(2);
                txtpk1.Text = (string)dataGridView.Rows[n].Cells["pk1"].Value;
                txtRuta.Text = (string)dataGridView.Rows[n].Cells["ruta"].Value;
                txtDescripcion.Text = (string)dataGridView.Rows[n].Cells["descripcion"].Value;
                txtTarjetas.Text = (string)dataGridView.Rows[n].Cells["Tarjetas"].Value.ToString();
                txtCasetas.Text = (string)dataGridView.Rows[n].Cells["Casetas"].Value.ToString();
                txtSueldo.Text = (string)dataGridView.Rows[n].Cells["Sueldo"].Value.ToString();
                comboLinea.Text = dataGridView.Rows[n].Cells["linea"].Value.ToString();
                comboRuta.Text = dataGridView.Rows[n].Cells["ruta"].Value.ToString();
                controles(1);
            }
        }

        public void getRowsDetalle(string search = "")
        {
            try
            {

                int count = 1;
                string sql = "SELECT * FROM VRUTAS_DESTINOS ";
                
                if (!string.IsNullOrEmpty(search))
                {
                    sql += "WHERE PK_RUTA = @SEARCH ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH",   int.Parse(search) );

                }
                else
                {
                    db.PreparedSQL(sql);

                }

                res = db.getTable();

                while (res.Next())
                {

                    nd = dataGridView1.Rows.Add();

                    dataGridView1.Rows[nd].Cells["pk"].Value = res.Get("PK");
                    dataGridView1.Rows[nd].Cells["no1"].Value = count;
                    dataGridView1.Rows[nd].Cells["Ruta_pk"].Value = res.Get("PK_RUTA");
                    dataGridView1.Rows[nd].Cells["ruta1"].Value = res.Get("RUTA");
                    dataGridView1.Rows[nd].Cells["pkOrigen"].Value = res.Get("PK_ORIGEN");
                    dataGridView1.Rows[nd].Cells["origen"].Value = res.Get("ORIGEN");
                    dataGridView1.Rows[nd].Cells["pkDestino"].Value = res.Get("PK_DESTINO");
                    dataGridView1.Rows[nd].Cells["destino"].Value = res.Get("DESTINO");
                    dataGridView1.Rows[nd].Cells["precio"].Value = res.Get("PRECIO");
                    dataGridView1.Rows[nd].Cells["distancia"].Value = res.Get("KMS");
                    dataGridView1.Rows[nd].Cells["tiempo"].Value = res.Get("TIEMPO");
                    dataGridView1.Rows[nd].Cells["completa"].Value = res.Get("COMPLETO");
                    dataGridView1.Rows[nd].Cells["fechaC"].Value = res.Get("FECHA_C");
                    dataGridView1.Rows[nd].Cells["fechaM"].Value = res.Get("FECHA_M");
                    dataGridView1.Rows[nd].Cells["usuario"].Value = res.Get("USUARIO");

                    count++;
                }

            }
            catch (Exception e)
            {
                string error = e.Message;
                LOG.write("RUTAS", "getRowsDetalle", e.Message);

            }

        }

        private void ComboRuta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboRuta.SelectedItem!=null && comboRuta.Items.Count > 0 && (comboRuta.SelectedItem as ComboboxItem).Value != null)
            {
                _comboRuta = int.Parse((comboRuta.SelectedItem as ComboboxItem).Value.ToString());
            }

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            getRowsDetalle(""+_comboRuta);
        }

        private void Menu2Add_Click(object sender, EventArgs e)
        {
            progressBar2.Show();
            progressBar2.Value = 20;
            try
            {
                if (ValidarInputs(2))
                {
                    progressBar2.Value = 40;

                    string sql = "INSERT INTO RUTAS_DESTINOS(PK_RUTA,PK_ORIGEN,PK_DESTINO,PRECIO,KMS,TIEMPO,COMPLETO,USUARIO)" +
                                 " VALUES(@RUTA,@ORIGEN,@DESTINO,@PRECIO,@KMS,@TIEMPO,@COMPLETO,@USUARIO)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@RUTA", _comboRuta);
                    db.command.Parameters.AddWithValue("@ORIGEN", _pk_origen);
                    db.command.Parameters.AddWithValue("@DESTINO", _pk_destino);
                    db.command.Parameters.AddWithValue("@PRECIO", _precio);
                    db.command.Parameters.AddWithValue("@KMS", _kms);
                    db.command.Parameters.AddWithValue("@TIEMPO", _tiempo);
                    db.command.Parameters.AddWithValue("@COMPLETO", _completo);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.PkUsuario);
                    progressBar2.Value = 60;

                    if (db.execute())
                    {
                        dataGridView1.Rows.Clear();
                        dataGridView1.Refresh();
                        getRowsDetalle();
                        cleanForm(2);
                        progressBar2.Value = 80;

                        if (comboLinea.SelectedItem != null)
                        {
                            getDatosAdicionales(1, (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
                        }
                        else { getDatosAdicionales(1); }
                        progressBar2.Value = 100;
                        controles(3);
                        comboRuta.Text = _comboRutaTexto;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡Verifica tu conexion a internet!");
                }
                else
                {
                    MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                }
                LOG.write("Rutas", "Menu2Add_Click", ex.ToString());
            }
            progressBar2.Hide();
            progressBar2.Value = 0;

        }

        private void Menu2Refresh_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            getRowsDetalle();
            cleanForm(2);
            controles(3);
        }

        private void Menu2Delete_Click(object sender, EventArgs e)
        {
            progressBar2.Show();
            progressBar2.Value = 20;
            try
            {
                var confirmResult = MessageBox.Show("¿Esta seguro de eliminar el registro?",
                                     "Confirmar eliminar",
                                     MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    progressBar2.Value = 40;
                    string sql;
                    sql = "DELETE FROM RUTAS_DESTINOS WHERE PK = @PK";
                    _pk_detalle = int.Parse(txtPkDetalle.Text);
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", _pk_detalle);
                    progressBar2.Value = 60;

                    if (db.execute())
                    {
                        progressBar2.Value = 80;

                        dataGridView1.Rows.Clear();
                        dataGridView1.Refresh();
                        getRowsDetalle();
                        cleanForm(2);
                        controles(3);
                        progressBar2.Value = 90;

                        if (comboLinea.SelectedItem != null)
                        {
                            getDatosAdicionales(1, (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
                        }
                        else { getDatosAdicionales(1); }
                        progressBar2.Value = 100;
                        comboRuta.Text = _comboRutaTexto;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el registro, intente de nuevo.");
                    }
                }

            }
            catch (Exception ex)
            {
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡Verifica tu conexion a internet!");
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el registro, intente de nuevo.");
                }
                LOG.write("Rutas", "Menu2Delete_Click", ex.ToString());
            }
            progressBar2.Hide();
            progressBar2.Value = 0;
        }

        private void Menu2Save_Click(object sender, EventArgs e)
        {
            progressBar2.Show();
            progressBar2.Value = 20;
            try
            {
                if (ValidarInputs(2))
                {
                    progressBar2.Value = 40;

                    string sql = "UPDATE RUTAS_DESTINOS SET PK_ORIGEN=@ORIGEN, PK_DESTINO=@DESTINO, " +
                                 "PRECIO=@PRECIO, KMS=@KMS, TIEMPO=@TIEMPO, COMPLETO=@COMPLETO, USUARIO=@USUARIO WHERE PK=@PK";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@ORIGEN", _pk_origen);
                    db.command.Parameters.AddWithValue("@DESTINO", _pk_destino);
                    db.command.Parameters.AddWithValue("@PRECIO", _precio);
                    db.command.Parameters.AddWithValue("@KMS", _kms);
                    db.command.Parameters.AddWithValue("@TIEMPO", _tiempo);
                    db.command.Parameters.AddWithValue("@COMPLETO", _completo);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.PkUsuario);
                    db.command.Parameters.AddWithValue("@PK", _pk_detalle);
                    progressBar2.Value = 60;

                    if (db.execute())
                    {
                        progressBar2.Value = 80;

                        dataGridView1.Rows.Clear();
                        dataGridView1.Refresh();
                        getRowsDetalle();
                        cleanForm(2);
                        progressBar2.Value = 90;

                        if (comboLinea.SelectedItem != null)
                        {
                            getDatosAdicionales(1, (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
                        }
                        else { getDatosAdicionales(1); }
                        progressBar2.Value = 100;

                        controles(3);
                        comboRuta.Text = _comboRutaTexto;

                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el registro, intente de nuevo.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    MessageBox.Show("¡Verifica tu conexion a internet!");
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el registro, intente de nuevo.");
                }
                LOG.write("Rutas", "Menu2Save_Click", ex.ToString());
            }
            progressBar2.Hide();
            progressBar2.Value = 0;
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                nd = e.RowIndex;

                if (nd != -1)
                {
                    txtPkDetalle.Text = (string)dataGridView1.Rows[nd].Cells[0].Value;
                    comboRuta.Text = (string)dataGridView1.Rows[nd].Cells[3].Value;
                    comboOrigen.Text = (string)dataGridView1.Rows[nd].Cells[5].Value;
                    comboDestino.Text = dataGridView1.Rows[nd].Cells[7].Value.ToString();
                    txtPrecio.Text = dataGridView1.Rows[nd].Cells[8].Value.ToString();
                    txtKm.Text = dataGridView1.Rows[nd].Cells[9].Value.ToString();
                    txtTiempo.Text = dataGridView1.Rows[nd].Cells[10].Value.ToString();
                    _comboRutaTexto = (comboRuta.SelectedItem as ComboboxItem).Text.ToString();
                    checkBoxCompleta.Checked = Boolean.Parse(dataGridView1.Rows[nd].Cells["completa"].Value.ToString());
                    controles(4);
                }
            }
            catch (Exception ex) {
                LOG.write("Rutas", "DataGridView1_CellContentClick",ex.Message);
            }

        }

        private void GroupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void TxtTiempo_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void TxtTiempo_Leave(object sender, EventArgs e)
        {
            TextBox tiempo = (TextBox)sender;
            string stringValue = tiempo.Text;
            if (stringValue != null && stringValue.Length == 4)
            {
                tiempo.Text = tiempo.Text.Substring(0,2) + ":"+tiempo.Text.Substring(2,2);
            }
            else if (stringValue != null && stringValue.Length == 3)
            {
                tiempo.Text = "0" + tiempo.Text.Substring(0, 1) + ":" + tiempo.Text.Substring(1, 2);
            }
            else if (stringValue != null && stringValue.Length == 2)
            {
                tiempo.Text = tiempo.Text + ":00";
            }
            else if (stringValue != null && stringValue.Length == 1)
            {
                tiempo.Text = "0" + tiempo.Text + ":00";
            }
        }

        private void ComboLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            ComboBox aux = (ComboBox)sender;
            if(aux.SelectedItem != null && (aux.SelectedItem as ComboboxItem).Value != null)
            getDatosAdicionales(1, (aux.SelectedItem as ComboboxItem).Value.ToString());
            if (dataGridView.Rows != null)
            {
                dataGridView.Rows.Clear();
            }
            if (aux.SelectedItem != null)
            {
                getRows("", (aux.SelectedItem as ComboboxItem).Value.ToString());
            }
            else { getRows(); }
        }

        private void Rutas_Load(object sender, EventArgs e)
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

            //dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
            dataGridView1.EnableHeadersVisualStyles = false;


            getDatosAdicionales();
            if (comboLinea.SelectedItem != null)
            {
                getRows("", (comboLinea.SelectedItem as ComboboxItem).Value.ToString());
            }
            else { getRows(); }
            controles(0);
            controles(3);
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

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void Rutas_Shown(object sender, EventArgs e)
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
