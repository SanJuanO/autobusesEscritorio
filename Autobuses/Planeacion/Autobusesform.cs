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
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Planeacion
{
    public partial class Autobusesform : Form
    {

        public database db;
        ResultSet res = null;
        public BackgroundWorker worker;
        private int n = 0;
        private int _pk1;
        private string _linea_pk;
        private string _socio_pk;
        private string _socio_pk_trabajando;
        private string _modelo;
        private string _placas;
        private string _serie;
        private string _tipo;
        private string _eco;
        private string _chofer;
        private string _fecha_c;
        private string _fecha_m;
        private string _usuario;
        private string _search;
        private string _pdf;
        private byte[] _foto;
        private string _fotoBase64;
        private string _archivoNombre;
        private string _vigencia;


        public Autobusesform()
        {
            InitializeComponent();
            this.Show();
            titulo.Text = "Autobuses";
            permisorh();
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            timer1.Start();
            panelContenedorForm.Visible = false;


        }
            
        private void permisorh()
        {
            if (!LoginInfo.privilegios.Any(x => x == "Conductor-Autobus"))
            {


                comboLinea.Enabled = false;
                comboTipo.Enabled = false;
                txtPlacas.Enabled = false;
                txtEco.Enabled = false;
                comboSocio.Enabled = false;
                comboSocioTrabajando.Enabled = false;
                txtModelo.Enabled = false;
                txtSerie.Enabled = false;
                dateTimePicker1.Enabled = false;
                btnPoliza.Enabled = false;
                archivo.Enabled = false;
                archivo1.Enabled = false;
                btnFoto.Enabled = false;
                btnSave.Visible = false;
                btnAdd.Visible = false;
            }

        }
        private void DoubleBuffered(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        public void getRows(string search = "")
        {
            try
            {
                //quite el pdf y foto se obtendra en el onclick
                string sql = "SELECT ROW_NUMBER()over( order by PK1 asc) as 'NO',PK1,LINEA_PK,LINEA,SOCIO_PK,SOCIO_USUARIO,SOCIO,SOCIO_PK_TRABAJA,SOCIO_TRABAJA_USUARIO,SOCIO_TRABAJA,MODELO,PLACAS,SERIE,TIPO_PK,TIPO,ECO,PK_CHOFER,CHOFER_USUARIO,CHOFER,FECHA_C,FECHA_M,USUARIO,ARCHIVO_NOMBRE,VIGENCIA FROM VAUTOBUSES ";
                _foto = new byte[0];
                
                if (!string.IsNullOrEmpty(search))
                {
                    sql += "WHERE LINEA LIKE @SEARCH OR SOCIO LIKE @SEARCH OR SERIE LIKE @SEARCH OR PLACAS LIKE @SEARCH ";
                }
                
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@SEARCH", "%" + search + "%");

                if (dataGridViewAutobuses.InvokeRequired)
                {
                    dataGridViewAutobuses.Invoke(new Action(() =>{dataGridViewAutobuses.DataSource = db.Populate();}));
                }
                else
                {
                    dataGridViewAutobuses.DataSource = db.Populate();
                }

            }
            catch (Exception e)
            {
                string error = e.Message;

            }

        }
        public void getRowsOLD(string search = "")
        {
            try
            {
                int count = 1;
                //quite el pdf y foto se obtendra en el onclick
                string sql = "SELECT PK1,LINEA_PK,LINEA,SOCIO_PK,SOCIO,SOCIO_PK_TRABAJA,SOCIO_TRABAJA,MODELO,PLACAS,SERIE,TIPO_PK,TIPO,ECO,PK_CHOFER,CHOFER,FECHA_C,FECHA_M,USUARIO,ARCHIVO_NOMBRE,VIGENCIA,LATITUD,LONGITUD FROM VAUTOBUSES ";
                _foto = new byte[0];
                /*
                Byte[] array = Encoding.ASCII.GetBytes("aqui va el campo de la base");
                MemoryStream ms = new MemoryStream(array);
                Bitmap bmp = new Bitmap(ms);
                */
                if (!string.IsNullOrEmpty(search))
                {
                    sql += "WHERE LINEA LIKE @SEARCH OR SOCIO LIKE @SEARCH OR SERIE LIKE @SEARCH OR PLACAS LIKE @SEARCH ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + search + "%");

                }
                else
                {
                    db.PreparedSQL(sql);

                }

                res = db.getTable();
                if (dataGridViewAutobuses.InvokeRequired)
                {
                    while (res.Next())
                    {
                        dataGridViewAutobuses.Invoke(new Action(() =>
                        {
                            n = dataGridViewAutobuses.Rows.Add();

                            dataGridViewAutobuses.Rows[n].Cells["pk1"].Value = res.Get("PK1");
                            dataGridViewAutobuses.Rows[n].Cells["No"].Value = count;
                            dataGridViewAutobuses.Rows[n].Cells["linea_pk"].Value = res.Get("LINEA_PK");
                            dataGridViewAutobuses.Rows[n].Cells["linea"].Value = res.Get("LINEA");
                            dataGridViewAutobuses.Rows[n].Cells["Socio_pk"].Value = res.Get("SOCIO_PK");
                            dataGridViewAutobuses.Rows[n].Cells["Socio"].Value = res.Get("SOCIO");
                            dataGridViewAutobuses.Rows[n].Cells["Socio_pk_trabajando"].Value = res.Get("SOCIO_PK_TRABAJA");
                            dataGridViewAutobuses.Rows[n].Cells["SocioTrabajando"].Value = res.Get("SOCIO_TRABAJA");
                            dataGridViewAutobuses.Rows[n].Cells["modelo"].Value = res.Get("MODELO");
                            dataGridViewAutobuses.Rows[n].Cells["placas"].Value = res.Get("PLACAS");
                            dataGridViewAutobuses.Rows[n].Cells["serie"].Value = res.Get("SERIE");
                            dataGridViewAutobuses.Rows[n].Cells["tipoPk"].Value = res.Get("TIPO_PK");
                            dataGridViewAutobuses.Rows[n].Cells["Tipo"].Value = res.Get("TIPO");
                            dataGridViewAutobuses.Rows[n].Cells["eco"].Value = res.Get("ECO");
                            dataGridViewAutobuses.Rows[n].Cells["pkChofer"].Value = res.Get("PK_CHOFER");
                            dataGridViewAutobuses.Rows[n].Cells["chofer"].Value = res.Get("CHOFER");
                            //dataGridViewAutobuses.Rows[n].Cells["foto"].Value = (!string.IsNullOrEmpty(res.Get("FOTO")))?Convert.FromBase64String(res.Get("FOTO")): null;
                            //dataGridViewAutobuses.Rows[n].Cells["pdf"].Value = res.Get("PDF");
                            dataGridViewAutobuses.Rows[n].Cells["archivoNombre"].Value = res.Get("ARCHIVO_NOMBRE");
                            dataGridViewAutobuses.Rows[n].Cells["vigencia"].Value = res.Get("VIGENCIA");
                            dataGridViewAutobuses.Rows[n].Cells["fecha_c"].Value = res.Get("FECHA_C");
                            dataGridViewAutobuses.Rows[n].Cells["fecha_m"].Value = res.Get("FECHA_M");
                            dataGridViewAutobuses.Rows[n].Cells["modifico"].Value = res.Get("USUARIO");

                            count++;
                        }));
                    }
                }
                else
                {
                    while (res.Next())
                    {

                        n = dataGridViewAutobuses.Rows.Add();

                        dataGridViewAutobuses.Rows[n].Cells["pk1"].Value = res.Get("PK1");
                        dataGridViewAutobuses.Rows[n].Cells["No"].Value = count;
                        dataGridViewAutobuses.Rows[n].Cells["linea_pk"].Value = res.Get("LINEA_PK");
                        dataGridViewAutobuses.Rows[n].Cells["linea"].Value = res.Get("LINEA");
                        dataGridViewAutobuses.Rows[n].Cells["Socio_pk"].Value = res.Get("SOCIO_PK");
                        dataGridViewAutobuses.Rows[n].Cells["Socio"].Value = res.Get("SOCIO");
                        dataGridViewAutobuses.Rows[n].Cells["Socio_pk_trabajando"].Value = res.Get("SOCIO_PK_TRABAJA");
                        dataGridViewAutobuses.Rows[n].Cells["SocioTrabajando"].Value = res.Get("SOCIO_TRABAJA");
                        dataGridViewAutobuses.Rows[n].Cells["modelo"].Value = res.Get("MODELO");
                        dataGridViewAutobuses.Rows[n].Cells["placas"].Value = res.Get("PLACAS");
                        dataGridViewAutobuses.Rows[n].Cells["serie"].Value = res.Get("SERIE");
                        dataGridViewAutobuses.Rows[n].Cells["tipoPk"].Value = res.Get("TIPO_PK");
                        dataGridViewAutobuses.Rows[n].Cells["Tipo"].Value = res.Get("TIPO");
                        dataGridViewAutobuses.Rows[n].Cells["eco"].Value = res.Get("ECO");
                        dataGridViewAutobuses.Rows[n].Cells["pkChofer"].Value = res.Get("PK_CHOFER");
                        dataGridViewAutobuses.Rows[n].Cells["chofer"].Value = res.Get("CHOFER");
                        //dataGridViewAutobuses.Rows[n].Cells["foto"].Value = (!string.IsNullOrEmpty(res.Get("FOTO")))?Convert.FromBase64String(res.Get("FOTO")): null;
                        //dataGridViewAutobuses.Rows[n].Cells["pdf"].Value = res.Get("PDF");
                        dataGridViewAutobuses.Rows[n].Cells["archivoNombre"].Value = res.Get("ARCHIVO_NOMBRE");
                        dataGridViewAutobuses.Rows[n].Cells["vigencia"].Value = res.Get("VIGENCIA");
                        dataGridViewAutobuses.Rows[n].Cells["fecha_c"].Value = res.Get("FECHA_C");
                        dataGridViewAutobuses.Rows[n].Cells["fecha_m"].Value = res.Get("FECHA_M");
                        dataGridViewAutobuses.Rows[n].Cells["modifico"].Value = res.Get("USUARIO");

                        count++;
                    }
                }

            }
            catch (Exception e)
            {
                string error = e.Message;

            }

        }

        public void getDatosAdicionales()
        {
            
            if (comboSocio.Items != null && comboSocio.Items.Count > 0) {
                comboSocio.Items.Clear();
            }
            if (comboSocioTrabajando.Items != null && comboSocioTrabajando.Items.Count > 0)
            {
                comboSocioTrabajando.Items.Clear();
            }
            ComboboxItem item1 = new ComboboxItem();
            item1.Text="";
            item1.Value = "";
            comboSocioTrabajandoAddItemDelegate(item1);
            string sql = "SELECT * FROM LINEAS ORDER BY LINEA ASC";
            db.PreparedSQL(sql);
            res = db.getTable();

            while (res.Next())
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = res.Get("LINEA");
                item.Value = res.Get("PK1");
                comboLineaAddItemDelegate(item);
                //comboLinea.Items.Add(item);

                //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
            }

            //comboSocio.SelectedIndex = 0;

            string sql1 = "SELECT PK PK1,NOMBRE+' '+APELLIDOS AS SOCIO  FROM SOCIOS WHERE BORRADO=0 ORDER BY SOCIO ASC";
            db.PreparedSQL(sql1);
            res = db.getTable();

            while (res.Next())
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = res.Get("SOCIO");
                item.Value = res.Get("PK1");
                comboSocioAddItemDelegate(item);
                comboSocioTrabajandoAddItemDelegate(item);
                //comboSocio.Items.Add(item);
                //comboSocioTrabajando.Items.Add(item);

                //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
            }

            //comboSocio.SelectedIndex = 0;

            //string sql2 = "SELECT * FROM TIPOS_AUTOBUSES ";
            string sql2 = "SELECT * FROM TIPOAUTOBUS ORDER BY Descripcion ASC";
            db.PreparedSQL(sql2);
            res = db.getTable();

            while (res.Next())
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = res.Get("Descripcion");
                item.Value = res.Get("PK");
                comboTipoAddItemDelegate(item);
                //comboTipo.Items.Add(item);

                //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
            }

            string sql3 = "SELECT NOMBRE,APELLIDOS,PK USUARIO FROM CHOFERES WHERE BORRADO=0 ORDER BY NOMBRE,APELLIDOS";
            db.PreparedSQL(sql3);
            res = db.getTable();

            while (res.Next())
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = res.Get("NOMBRE") + " " + res.Get("APELLIDOS");
                item.Value = res.Get("USUARIO");
                comboChoferAddItemDelegate(item);
                //comboChofer.Items.Add(item);

                //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
            }

        }


        public void comboLineaAddItemDelegate(ComboboxItem item)
        {
            if (comboLinea.InvokeRequired)
            {
                comboLinea.Invoke(new Action(() =>
                {
                    comboLinea.Items.Add(item);
                }));
            }
            else
            {
                comboLinea.Items.Add(item);
            }
        }
        public void comboSocioTrabajandoAddItemDelegate(ComboboxItem item)
        {
            if (comboSocioTrabajando.InvokeRequired)
            {
                comboSocioTrabajando.Invoke(new Action(() =>
                {
                    comboSocioTrabajando.Items.Add(item);
                }));
            }
            else
            {
                comboSocioTrabajando.Items.Add(item);
            }
        }
        public void comboSocioAddItemDelegate(ComboboxItem item)
        {
            if (comboSocio.InvokeRequired)
            {
                comboSocio.Invoke(new Action(() =>
                {
                    comboSocio.Items.Add(item);
                }));
            }
            else
            {
                comboSocio.Items.Add(item);
            }
        }
        public void comboTipoAddItemDelegate(ComboboxItem item)
        {
            if (comboTipo.InvokeRequired)
            {
                comboTipo.Invoke(new Action(() =>
                {
                    comboTipo.Items.Add(item);
                }));
            }
            else
            {
                comboSocio.Items.Add(item);
            }
        }
        public void comboChoferAddItemDelegate(ComboboxItem item)
        {
            if (comboChofer.InvokeRequired)
            {
                comboChofer.Invoke(new Action(() =>
                {
                    comboChofer.Items.Add(item);
                }));
            }
            else
            {
                comboChofer.Items.Add(item);
            }
        }

        private void AutobusAdd_Click(object sender, EventArgs e)
        {


            
        }


        private Boolean ValidarInputs()
        {
            Boolean valido = true;
            _linea_pk = "";
            _socio_pk = "";
            _chofer = "";
            _tipo = "";
            _foto = (pictureBox1.Image!=null)?Utilerias.Utilerias.ImageToByteArray( pictureBox1.Image):null;
            _fotoBase64 = (_foto!=null)?Convert.ToBase64String(_foto):null;
            _archivoNombre = archivo1.Text;
            _vigencia = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            if (!String.IsNullOrEmpty(txtpk1.Text))
            {
                _pk1 = int.Parse(txtpk1.Text);
            }
            if (comboLinea.SelectedIndex!=-1 && (comboLinea.SelectedItem as ComboboxItem).Value != null)
            {
                _linea_pk = (comboLinea.SelectedItem as ComboboxItem).Value.ToString();
            }
            if (comboSocio.SelectedIndex != -1 && (comboSocio.SelectedItem as ComboboxItem).Value != null)
            {
                _socio_pk = (comboSocio.SelectedItem as ComboboxItem).Value.ToString();
            }
            if (comboSocioTrabajando.SelectedIndex != -1 && (comboSocioTrabajando.SelectedItem as ComboboxItem).Value != null)
            {
                _socio_pk_trabajando = (comboSocioTrabajando.SelectedItem as ComboboxItem).Value.ToString();
            }
            else {
                _socio_pk_trabajando = "";
            }
            if (comboTipo.SelectedIndex != -1 && (comboTipo.SelectedItem as ComboboxItem).Value != null)
            {
                _tipo = (comboTipo.SelectedItem as ComboboxItem).Value.ToString();
            }
            if (comboChofer.SelectedIndex != -1 && (comboChofer.SelectedItem as ComboboxItem).Value != null)
            {
                _chofer = (comboChofer.SelectedItem as ComboboxItem).Value.ToString();
            }
            _modelo = txtModelo.Text;
            _placas = txtPlacas.Text;
            _serie = txtSerie.Text;
            //_orden = int.TryParse(txtEco.Text, out int result)?result:-1;
            _eco = txtEco.Text;

            if (string.IsNullOrEmpty(_linea_pk))
            {
                MessageBox.Show("Es necesario ingresar liena!");
                comboLinea.Focus();
                valido = false;

            }
            else if (string.IsNullOrEmpty(_socio_pk))
            {
                MessageBox.Show("Es necesario ingresar Socio!");
                comboSocio.Focus();
                valido = false;

            }
            /*else if (string.IsNullOrEmpty(_socio_pk_trabajando))
            {
                MessageBox.Show("Es necesario ingresar Socio!");
                comboSocioTrabajando.Focus();
                valido = false;

            }*/
            else if (string.IsNullOrEmpty(_modelo) || !int.TryParse(_modelo,out int result))
            {
                MessageBox.Show("Es necesario ingresar modelo numérico!");
                txtModelo.Focus();
                valido = false;
            }
            else if (string.IsNullOrEmpty(_placas))
            {
                MessageBox.Show("Es necesario ingresar password!");
                txtPlacas.Focus();
                valido = false;
            }
            else if (string.IsNullOrEmpty(_serie))
            {
                MessageBox.Show("Es necesario ingresar capacidad de tipo número!");
                txtSerie.Focus();
                valido = false;
            }
            else if (string.IsNullOrEmpty(_tipo))
            {
                MessageBox.Show("Es necesario ingresar tipo de autobus!");
                comboTipo.Focus();
                valido = false;

            } else if (string.IsNullOrEmpty(_eco)) {
                MessageBox.Show("Es necesario ingresar el orden de rolado del autobus!");
                txtEco.Focus();
                valido = false;
            }
            else if (string.IsNullOrEmpty(_chofer))
            {
                MessageBox.Show("Es necesario ingresar chofer!");
                comboChofer.Focus();
                valido = false;

            }
            else if (string.IsNullOrEmpty(_vigencia))
            {
                MessageBox.Show("Es necesario ingresar vigencia de la póliza!");
                dateTimePicker1.Focus();
                valido = false;

            }
            /*else if (string.IsNullOrEmpty(_pdf))
            {
                MessageBox.Show("Es necesario ingresar Pòliza del autobus!");
                btnFoto.Focus();
                valido = false;

            }
            else if (string.IsNullOrEmpty(_fotoBase64))
            {
                MessageBox.Show("Es necesario ingresar Foto del autobus!");
                btnFoto.Focus();
                valido = false;

            }*/
            
            return valido;
        }

        private void DataGridViewAutobuses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            n = e.RowIndex;
            
            if (n != -1)
            {
                //obtengo foto y pdf para mostrarlo
                /*if (dataGridViewAutobuses.Rows[n].Cells["foto"].Value == null || dataGridViewAutobuses.Rows[n].Cells["pdf"].Value == null)
                { getPhotoPdf(dataGridViewAutobuses.Rows[n].Cells["pk1"].Value.ToString(), n); }
                */
                txtpk1.Text = (string)dataGridViewAutobuses.Rows[n].Cells["pk1"].Value.ToString();
                comboLinea.Text = dataGridViewAutobuses.Rows[n].Cells["linea"].Value.ToString();
                comboSocio.Text = dataGridViewAutobuses.Rows[n].Cells["Socio"].Value.ToString();
                comboSocioTrabajando.Text = dataGridViewAutobuses.Rows[n].Cells["SocioTrabajando"].Value.ToString();
                txtModelo.Text = (string)dataGridViewAutobuses.Rows[n].Cells["modelo"].Value.ToString();
                txtPlacas.Text = (string)dataGridViewAutobuses.Rows[n].Cells["placas"].Value.ToString();
                txtSerie.Text = (string)dataGridViewAutobuses.Rows[n].Cells["serie"].Value.ToString();
                comboTipo.Text = dataGridViewAutobuses.Rows[n].Cells["Tipo"].Value.ToString();
                txtEco.Text = dataGridViewAutobuses.Rows[n].Cells["eco"].Value.ToString();
                comboChofer.Text = dataGridViewAutobuses.Rows[n].Cells["chofer"].Value.ToString();
                //pictureBox1.Image = (dataGridViewAutobuses.Rows[n].Cells["foto"].Value!=null)? Image.Bytes_A_Imagen((byte[])dataGridViewAutobuses.Rows[n].Cells["foto"].Value):null;
                //archivo.Text = dataGridViewAutobuses.Rows[n].Cells["pdf"].Value.ToString();
                archivo1.Text = dataGridViewAutobuses.Rows[n].Cells["archivoNombre"].Value.ToString();
                dateTimePicker1.Value = (!string.IsNullOrEmpty(dataGridViewAutobuses.Rows[n].Cells["vigencia"].Value.ToString()))?DateTime.Parse(dataGridViewAutobuses.Rows[n].Cells["vigencia"].Value.ToString()):DateTime.Now;
                
                CONTROLES(1);
            }
            
        }

        private void AutobusRefresh_Click(object sender, EventArgs e)
        {
            //dataGridViewAutobuses.Rows.Clear();
            dataGridViewAutobuses.Refresh();
            getRows();
            cleanForm();
            CONTROLES(0);
        }

        private void AutobusDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show("¿Esta seguro de eliminar el registro?",
                                     "Confirmar eliminar",
                                     MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    string sql;
                    sql = "UPDATE AUTOBUSES SET BORRADO=1, USUARIO=@USUARIOM WHERE PK1 = @PK1";
                    _pk1 = int.Parse(txtpk1.Text);
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@USUARIOM", LoginInfo.PkUsuario);
                    db.command.Parameters.AddWithValue("@PK1", _pk1);

                    if (db.execute())
                    {

                        //dataGridViewAutobuses.Rows.Clear();
                        dataGridViewAutobuses.Refresh();
                        getRows();
                        cleanForm();
                        /*
                        if (n != -1)
                        {
                            dataGridViewUsuarios.Rows.RemoveAt(n);

                        }
                        */
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

        private void cleanForm()
        {
            txtpk1.Text = "";
            comboLinea.Text = "";
            comboLinea.SelectedIndex = -1;
            comboSocio.Text = "";
            comboSocio.SelectedIndex = -1;
            comboSocioTrabajando.Text = "";
            comboSocioTrabajando.SelectedIndex = -1;
            comboTipo.Text = "";
            comboTipo.SelectedIndex = -1;
            txtEco.Text = "";
            txtModelo.Text = "";
            txtPlacas.Text = "";
            txtSerie.Text = "";
            txtSearch.Text = "";
            comboChofer.Text = "";
            comboChofer.SelectedIndex = -1;
            archivo.Text = "";
            archivo1.Text = "";
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            AcroPDF1.Visible=false;
            panelContenedorForm.Visible = false;
            agregar.Visible = true;
            guardar.Visible = false;
            dataGridViewAutobuses.Visible = true;
        }

        private void AutobusSave_Click(object sender, EventArgs e)
        {
            if (ValidarInputs())
            {

                string sql = "UPDATE AUTOBUSES SET LINEA_PK=@LINEA, SOCIO_PK=@SOCIO," +
                             (!string.IsNullOrEmpty(_socio_pk_trabajando)?" SOCIO_PK_TRABAJA=@SOCIOTRABAJANDO,":" ") +
                             " MODELO=@MODELO, PLACAS=@PLACAS, SERIE=@SERIE, TIPO_PK=@TIPO, ECO=@ECO, PK_CHOFER=@CHOFER, " +
                             "USUARIO=@USUARIO, VIGENCIA=@VIGENCIA WHERE PK1=@PK1";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@LINEA", _linea_pk);
                db.command.Parameters.AddWithValue("@SOCIO", _socio_pk);
                db.command.Parameters.AddWithValue("@SOCIOTRABAJANDO", _socio_pk_trabajando);
                db.command.Parameters.AddWithValue("@MODELO", _modelo);
                db.command.Parameters.AddWithValue("@PLACAS", _placas);
                db.command.Parameters.AddWithValue("@SERIE", _serie);
                db.command.Parameters.AddWithValue("@TIPO", _tipo);
                db.command.Parameters.AddWithValue("@ECO", _eco);
                db.command.Parameters.AddWithValue("@CHOFER", _chofer);
                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                db.command.Parameters.AddWithValue("@VIGENCIA", _vigencia);
                db.command.Parameters.AddWithValue("@PK1", _pk1);

                if (db.execute())
                {
                    //dataGridViewAutobuses.Rows.Clear();
                    dataGridViewAutobuses.Refresh();
                    getRows();
                    cleanForm();
                    CONTROLES(0);
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el registro, intente de nuevo.");
                }
            }
        }

        private void AutobusExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridViewAutobuses,new List<string> { "pdf", "archivoNombre", "foto", "tipoPk", "Socio_pk", "linea_pk", "pk1", "socioTrabajandoUsuario", "Socio trabajando usuario" });
            }
            catch (SecurityException ex)
            {

            }
        }

        private void AutobusSearch_Click(object sender, EventArgs e)
        {
            _search = txtSearch.Text;
            //dataGridViewAutobuses.Rows.Clear();
            dataGridViewAutobuses.Refresh();
            getRows(_search);
        }

        private void SessionClose_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.cerrarSesion(this);
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void CONTROLES(int OPC) {

            switch (OPC) {
                case 0:
          
                    autobusSearch.Visible = true;

                    btnAdd.Enabled = true;
                    btnRefresb.Enabled = true;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnAdd.BackColor= Color.FromArgb(38, 45, 56);
                    btnRefresb.BackColor= Color.FromArgb(38, 45, 56);
                    btnDelete.BackColor = Color.White;
                    btnSave.BackColor = Color.White;
                    guardar.Visible = false;
                    insertar.Visible = true;
                    break;
                case 1:
  
                    autobusSearch.Visible = true;

                    btnAdd.Enabled = false;
                    btnRefresb.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = true;

                    btnAdd.BackColor = Color.White;
                    btnRefresb.BackColor = Color.FromArgb(38, 45, 56);
                    btnDelete.BackColor = Color.FromArgb(38, 45, 56);
                    btnSave.BackColor = Color.FromArgb(38, 45, 56);
                    guardar.Visible = true;
                    insertar.Visible = false;

                    break;
                    
            }

        }

        private void SearchEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AutobusSearch_Click(sender, e);
            }
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog()
            {
                FileName = "Select a pdf file",
                Filter = "Pdf files (*.pdf)|*.pdf",
                Title = "Open PDF file"
            };
            ClientSize = new Size(330, 360);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    progressBar1.Show();
                    progressBar1.Value = 20;
                    string filePath = openFileDialog1.FileName;
                    Byte[] bytes = File.ReadAllBytes(filePath);
                    progressBar1.Value = 30;
                    _pdf = Convert.ToBase64String(bytes);
                    archivo.Text = _pdf;
                    archivo1.Text = filePath.Split('\\')[filePath.Split('\\').Length-1];
                    _archivoNombre = archivo1.Text;
                    progressBar1.Value = 50;
                    if (!String.IsNullOrEmpty(txtpk1.Text))
                    {
                        progressBar1.Value = 60;
                        string sql = "UPDATE AUTOBUSES SET PDF=@PDF, ARCHIVO_NOMBRE=@ARCHIVO WHERE PK1=@PK1";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PDF", _pdf);
                        db.command.Parameters.AddWithValue("@ARCHIVO", _archivoNombre);
                        db.command.Parameters.AddWithValue("@PK1", txtpk1.Text);
                        progressBar1.Value = 70;
                        if (db.execute())
                        {
                            MessageBox.Show("¡Pòliza actualizada correctamente!");
                        }
                        progressBar1.Value = 90;
                    }
                    progressBar1.Value = 100;
                    progressBar1.Hide();
                    progressBar1.Value = 0;
                }
                catch (SecurityException ex)
                {
                    if (!Utilerias.Utilerias.CheckForInternetConnection())
                    {
                        MessageBox.Show("¡Verifique su conexion a internet!");
                    }
                    LOG.write("Autobusesform", "SelectButton_Click", ex.ToString());
                }
            }
        }

        private void BtnFoto_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog()
            {
                FileName = "Select a image file",
                Filter = "Image files (*.png;*.jpg)|*.jpg;*.png",
                Title = "Open image file"
            };
            ClientSize = new Size(330, 360);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    progressBar1.Show();
                    progressBar1.Value = 20;
                    string filePath = openFileDialog1.FileName;
                    Byte[] bytes = File.ReadAllBytes(filePath);
                    progressBar1.Value = 30;
                    pictureBox1.Image = Image.Bytes_A_Imagen(bytes);
                    _foto = bytes;
                    _fotoBase64 = (_foto != null) ? Convert.ToBase64String(_foto) : null;
                    progressBar1.Value = 50;

                    if (!String.IsNullOrEmpty(txtpk1.Text)) {
                        progressBar1.Value = 60;
                        string sql = "UPDATE AUTOBUSES SET FOTO=@FOTO WHERE PK1=@PK1";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@FOTO", _fotoBase64);
                        db.command.Parameters.AddWithValue("@PK1", txtpk1.Text);
                        progressBar1.Value = 70;
                        if (db.execute()) {
                            MessageBox.Show("¡Foto actualizada correctamente!");
                        }
                        progressBar1.Value = 90;
                    }
                    progressBar1.Value = 100;
                    progressBar1.Hide();
                    progressBar1.Value = 0;

                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Error de seguridad.\n\n mensaje: {ex.Message}\n\n" +
                    $"Detalles:\n\n{ex.StackTrace}");
                }
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {/*
            pictureBox2.Image = pictureBox1.Image;
            pictureBox2.Visible=true;*/
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            //pictureBox2.Visible = false;
        }

        public void openPDF() {
            
            string archivoPDF= Path.GetTempFileName() + "HelpFile.pdf";
            System.IO.FileStream stream = new FileStream(archivoPDF, FileMode.CreateNew);
            System.IO.BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Convert.FromBase64String(_pdf), 0, Convert.FromBase64String(_pdf).Length);
            writer.Close();
            if (AcroPDF1.InvokeRequired) {
                AcroPDF1.Invoke(new Action(()=> {
                    AcroPDF1.LoadFile(archivoPDF);
                    AcroPDF1.Visible = true;
                }));
            }
            else
            {
                AcroPDF1.LoadFile(archivoPDF);
                AcroPDF1.Visible = true;
            }
        }

        private void Archivo1_Click(object sender, EventArgs e)
        {/*
            if (!string.IsNullOrEmpty(archivo.Text)) {
                _pdf = archivo.Text;
                openPDF();
            }*/
        }

        public void getPhotoPdf(string pk1,int pos)
        {
            try
            {
                string sql = "SELECT FOTO,PDF FROM VAUTOBUSES ";
                _foto = new byte[0];
                /*
                Byte[] array = Encoding.ASCII.GetBytes("aqui va el campo de la base");
                MemoryStream ms = new MemoryStream(array);
                Bitmap bmp = new Bitmap(ms);
                */
                sql += "WHERE PK1=@PK ";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PK", pk1);

                res = db.getTable();

                while (res.Next())
                {

                    dataGridViewAutobuses.Rows[pos].Cells["foto"].Value = (!string.IsNullOrEmpty(res.Get("FOTO"))) ? Convert.FromBase64String(res.Get("FOTO")) : null;
                    dataGridViewAutobuses.Rows[pos].Cells["pdf"].Value = res.Get("PDF");

                }

            }
            catch (Exception e)
            {
                string error = e.Message;

            }

        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            getRows();
            getDatosAdicionales();


        }

        private void Autobusesform_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            AcroPDF1.Visible = false;

            comboLinea.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSocio.DropDownStyle = ComboBoxStyle.DropDownList;
            comboTipo.DropDownStyle = ComboBoxStyle.DropDownList;

            dataGridViewAutobuses.EnableHeadersVisualStyles = false;

            DoubleBuffered(dataGridViewAutobuses, true);

            db = new database();
            /*getRows();
            getDatosAdicionales();
            */
            backgroundWorker1.RunWorkerAsync();
            CONTROLES(0);

        }

        private void botonhuella(object sender, EventArgs e)
        {

        }

        private void Guardar_Click(object sender, EventArgs e)
        {

        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        private void Agregar_Click_1(object sender, EventArgs e)
        {

        }

        private void actualizarsuc(object sender, EventArgs e)
        {

        }

        private void actualizarrol(object sender, EventArgs e)
        {

        }

        private void DataGridViewAutobuses_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            panelContenedorForm.Visible = true;
            insertar.Visible = true;
            guardar.Visible = false;

        }

        private void BtnCerrar_Click_1(object sender, EventArgs e)
        {
            panelContenedorForm.Visible = false;
            dataGridViewAutobuses.Visible = true;
            cleanForm();

        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            panelContenedorForm.Visible = false;
            dataGridViewAutobuses.Visible = true;
            cleanForm();
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

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
         
        }

        private void Guardar_Click_1(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Show();
                progressBar1.Value = 20;
                if (ValidarInputs())
                {
                    progressBar1.Value = 40;

                    string sql = "UPDATE AUTOBUSES SET LINEA_PK=@LINEA, SOCIO_PK=@SOCIO," +
                                 " SOCIO_PK_TRABAJA=@SOCIOTRABAJANDO, MODELO=@MODELO, " +
                                 "PLACAS=@PLACAS, SERIE=@SERIE, TIPO_PK=@TIPO, ECO=@ECO, PK_CHOFER=@CHOFER, " +
                                 "USUARIO=@USUARIO, VIGENCIA=@VIGENCIA WHERE PK1=@PK1";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@LINEA", _linea_pk);
                    db.command.Parameters.AddWithValue("@SOCIO", _socio_pk);
                    db.command.Parameters.AddWithValue("@SOCIOTRABAJANDO", _socio_pk_trabajando);
                    db.command.Parameters.AddWithValue("@MODELO", _modelo);
                    db.command.Parameters.AddWithValue("@PLACAS", _placas);
                    db.command.Parameters.AddWithValue("@SERIE", _serie);
                    db.command.Parameters.AddWithValue("@TIPO", _tipo);
                    db.command.Parameters.AddWithValue("@ECO", _eco);
                    db.command.Parameters.AddWithValue("@CHOFER", _chofer);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                    db.command.Parameters.AddWithValue("@VIGENCIA", _vigencia);
                    db.command.Parameters.AddWithValue("@PK1", _pk1);
                    progressBar1.Value = 50;

                    if (db.execute())
                    {
                        progressBar1.Value = 70;

                        //dataGridViewAutobuses.Rows.Clear();
                        dataGridViewAutobuses.Refresh();
                        getRows();
                        cleanForm();
                        CONTROLES(0);
                        progressBar1.Value = 100;

                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el registro, intente de nuevo.");
                    }
                    progressBar1.Value = 0;
                    progressBar1.Hide();

                }
            }
            catch (Exception ex) {
                if (!Utilerias.Utilerias.CheckForInternetConnection()) {
                    MessageBox.Show("¡Verifica tu conexion a internet!");
                }else
                {
                    MessageBox.Show("¡Intenta nuevamente!");
                }
                LOG.write("Autobusesform", "Guardar_Click_1", ex.ToString());
                progressBar1.Value = 0;
                progressBar1.Hide();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            panelContenedorForm.Visible = true;
            insertar.Visible = false;
            guardar.Visible = true;
            dataGridViewAutobuses.Visible = false;
            progressBar1.Visible = true;
            progressBar1.Value = 20;
            timer2.Interval = 100;
            timer2.Start();

        }

        private void Insertar_Click(object sender, EventArgs e)
        {
            if (ValidarInputs())
            {

                string sql = "INSERT INTO AUTOBUSES(LINEA_PK,SOCIO_PK," +
                             (!string.IsNullOrEmpty(_socio_pk_trabajando) ? "SOCIO_PK_TRABAJA," : "") +
                             "MODELO,PLACAS,SERIE,TIPO_PK,ECO,PK_CHOFER,USUARIO,"+
                             (!string.IsNullOrEmpty(_fotoBase64) ? "FOTO," : "") + 
                             (!string.IsNullOrEmpty(_pdf) ? "PDF,ARCHIVO_NOMBRE," : "") + 
                             "VIGENCIA) " +
                             "VALUES(@LINEA,@SOCIO," +
                             (!string.IsNullOrEmpty(_socio_pk_trabajando) ? "@SOCIOTRABAJANDO," : "") +
                             "@MODELO,@PLACAS,@SERIE,@TIPO,@ECO,@CHOFER,@USUARIO,@FOTO,@PDF,@ARCHIVO,@VIGENCIA)";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@LINEA", _linea_pk);
                db.command.Parameters.AddWithValue("@SOCIO", _socio_pk);
                db.command.Parameters.AddWithValue("@SOCIOTRABAJANDO",_socio_pk_trabajando);
                db.command.Parameters.AddWithValue("@MODELO", _modelo);
                db.command.Parameters.AddWithValue("@PLACAS", _placas);
                db.command.Parameters.AddWithValue("@SERIE", _serie);
                db.command.Parameters.AddWithValue("@TIPO",_tipo);
                db.command.Parameters.AddWithValue("@ECO", _eco);
                db.command.Parameters.AddWithValue("@CHOFER", _chofer);
                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                if (!string.IsNullOrEmpty(_fotoBase64)){db.command.Parameters.AddWithValue("@FOTO", _fotoBase64);}
                if (!string.IsNullOrEmpty(_pdf)) {
                    db.command.Parameters.AddWithValue("@PDF", _pdf);
                    db.command.Parameters.AddWithValue("@ARCHIVO", _archivoNombre);
                }
                db.command.Parameters.AddWithValue("@VIGENCIA", _vigencia);

                if (db.execute())
                {
                    //dataGridViewAutobuses.Rows.Clear();
                    dataGridViewAutobuses.Refresh();
                    getRows();
                    cleanForm();

                }
                else
                {
                    MessageBox.Show("No se pudo insertar el registro, intente de nuevo.");
                }
            }
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

        private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(panelContenedorForm.Handle, 0x112, 0xf012, 0);
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            cargar();
        }

        private async void BackgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        public async Task cargar() {
            try
            {
                progressBar1.Value = 40;

                if (dataGridViewAutobuses.Rows[n].Cells["foto"].Value == null || dataGridViewAutobuses.Rows[n].Cells["pdf"].Value == null)
                {
                    progressBar1.Value = 50;

                    getPhotoPdf(dataGridViewAutobuses.Rows[n].Cells["pk1"].Value.ToString(), n);
                    progressBar1.Value = 60;

                }
                pictureBox1.Image = (dataGridViewAutobuses.Rows[n].Cells["foto"].Value != null) ? Image.Bytes_A_Imagen((byte[])dataGridViewAutobuses.Rows[n].Cells["foto"].Value) : null;
                progressBar1.Value = 70;
                archivo.Text = dataGridViewAutobuses.Rows[n].Cells["pdf"].Value.ToString();
                progressBar1.Value = 75;
                pictureBox2.Image = pictureBox1.Image;
                progressBar1.Value = 80;
                pictureBox2.Visible = true;
                progressBar1.Value = 85;
                if (!string.IsNullOrEmpty(archivo.Text))
                {
                    progressBar1.Value = 90;
                    _pdf = archivo.Text;
                    openPDF();
                }
                progressBar1.Value = 100;

                if (progressBar1.InvokeRequired)
                {
                    progressBar1.Invoke(new Action(() =>
                    {
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;
                    }));
                }
                else
                {
                    progressBar1.Value = 0;
                    progressBar1.Visible = false;
                }
            }
            catch (Exception e) {

                if (!Utilerias.Utilerias.CheckForInternetConnection()) {
                    MessageBox.Show("¡Verifique su conexion a internet!");
                }else
                {
                    MessageBox.Show("¡Intente nuevamente!");
                }
                if (progressBar1.InvokeRequired)
                {
                    progressBar1.Invoke(new Action(() =>
                    {
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;
                    }));
                }
                else
                {
                    progressBar1.Value = 0;
                    progressBar1.Visible = false;
                }
            }
        }

        private void BackgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }
    }
}
