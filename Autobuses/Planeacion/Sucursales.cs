using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ConnectDB;
using Autobuses.Utilerias;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace Autobuses.Planeacion
{


    public partial class Sucursales : Form
    {

        public database db;
        ResultSet res = null;

        ResultSet res2 = null;
        Bitmap image;

        private int n = 0;

        private string _sucursal;

        private string _calle;
        private string _estado;
        private string _municipio;
        private string _colonia;
        private string _cp;
        private string _pkzona;

        private string _zona;
        private string _folio;
        private string _search;
        private string _searchtool;
        private int _id;
        private int _valorid;
        private string _clase = "sucursales";
        private string _user;
        private int fila;
        private int columna;


        public Sucursales()
        {
            InitializeComponent();
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            timer2.Start();
            this.Show();
            titulo.Text = "Sucursales";
        }

        public void Estados()
        {

            try
            {

                string sql = "select c_estado, D_estado from sepomex GROUP BY c_estado,D_estado order by D_estado";

                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Value = res.Get("c_estado");
                    item.Text = res.Get("D_estado");
                    comboBoxEstados.Items.Add(item);

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }

               // comboBoxEstados.SelectedIndex = 0;
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "ESTADO";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void comboBoxEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox cmb = (ComboBox)sender;
                int selectedIndex = cmb.SelectedIndex;
                string selectedText = cmb.SelectedItem.ToString();
                string valor = (cmb.SelectedItem as ComboboxItem).Value.ToString();


                Municipios(valor);
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "COMBOBOXESTADOS";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }


        private void Municipios(string valor)
        {
            try
            {
                comboBoxMunicipios.Items.Clear();

                string sql = "select c_mnpio, D_mnpio from sepomex where c_estado = '" + valor + "' group by c_mnpio, D_mnpio";

                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Value = res.Get("c_mnpio");
                    item.Text = res.Get("D_mnpio");
                    comboBoxMunicipios.Items.Add(item);

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }

                //                comboBoxMunicipios.SelectedIndex = 0;

                string selectedText = comboBoxMunicipios.Items[0].ToString();
                string value = (comboBoxMunicipios.Items[0] as ComboboxItem).Value.ToString();

                Colonias(value);
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "MUNICIPIOS";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }


        private void Colonias(string municipio = "")
        {

            try
            {
                comboBoxColonias.Items.Clear();

                string CP = textCP.Text;
                comboBoxColonias.Items.Clear();

                string sql = "select d_codigo, d_asenta from sepomex where c_mnpio = '" + municipio + "' ";

                if (!string.IsNullOrEmpty(CP))
                {
                    sql += " AND d_codigo = '" + CP + "'";
                }

                sql += " order by d_asenta";

                db.PreparedSQL(sql);
                res = db.getTable();
                if (res.Next())
                {
                    while (res.Next())
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Value = res.Get("d_codigo");
                        item.Text = res.Get("d_asenta");
                        comboBoxColonias.Items.Add(item);

                        //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                    }

                  //  comboBoxColonias.SelectedIndex = 0;
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "COLONIAS";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void comboBoxMunicipios_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedText = comboBoxMunicipios.SelectedItem.ToString();
                string value = (comboBoxMunicipios.SelectedItem as ComboboxItem).Value.ToString();

                Colonias(value);
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "COMBOBOXMUNICIPIO";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }




        public void getRows(string search = "")
        {
            try
            {
                int count = 1;
                string sql = "SELECT ID,FOLIO,SUCURSAL,CALLE,ESTADO,MUNICIPIO,COLONIA,CP,ZONA,PKZONA, cobrartarjetas FROM SUCURSALES WHERE BORRADO=0 ";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += "SELECT ID,FOLIO,SUCURSAL,CALLE,ESTADO,MUNICIPIO,COLONIA,CP,ZONA,PKZONA, cobrartarjetas FROM SUCURSALES  " +
                        "WHERE USUARIO LIKE @SEARCH OR NOMBRE LIKE @SEARCH OR APELLIDOS LIKE @SEARCH  AND BORRADO=0";
                }
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@SEARCH", "%" + search + "%");
                res = db.getTable();
                while (res.Next())
                {
                    n = dataGridViewUsuarios.Rows.Add();


                    dataGridViewUsuarios.Rows[n].Cells["NoName"].Value = count;
                    dataGridViewUsuarios.Rows[n].Cells["IdName"].Value = res.Get("ID");
                    dataGridViewUsuarios.Rows[n].Cells["FolioName"].Value = res.Get("FOLIO");
                    dataGridViewUsuarios.Rows[n].Cells["SucursalName"].Value = res.Get("SUCURSAL");
                    dataGridViewUsuarios.Rows[n].Cells["CalleName"].Value = res.Get("CALLE");
                    dataGridViewUsuarios.Rows[n].Cells["EstadoName"].Value = res.Get("ESTADO");
                    dataGridViewUsuarios.Rows[n].Cells["MunicipioName"].Value = res.Get("MUNICIPIO");
                    dataGridViewUsuarios.Rows[n].Cells["ColoniaName"].Value = res.Get("COLONIA");
                    dataGridViewUsuarios.Rows[n].Cells["CpName"].Value = res.Get("CP");
                    dataGridViewUsuarios.Rows[n].Cells["IdName"].Value = res.GetInt("ID");
                    dataGridViewUsuarios.Rows[n].Cells["ZonamName"].Value = res.Get("ZONA");
                    dataGridViewUsuarios.Rows[n].Cells["pkzonaName"].Value = res.Get("PKZONA");
                    dataGridViewUsuarios.Rows[n].Cells["CobrartajretasName"].Value = res.Get("cobrartarjetas");
                    count++;
                    dataGridViewUsuarios.CurrentRow.Selected = false;
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


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {


                n = e.RowIndex;


                int c = e.ColumnIndex;
                if (n != -1 && c != 7)
                {
                    txtSucursal.ReadOnly = true;
                    fila = n;
                    columna = c;
                 
                        textFolio.Text = (string)dataGridViewUsuarios.Rows[n].Cells["FolioName"].Value;

                        txtSucursal.Text = (string)dataGridViewUsuarios.Rows[n].Cells["SucursalName"].Value;
                        _user = (string)dataGridViewUsuarios.Rows[n].Cells["SucursalName"].Value;
                        textCalle.Text = (string)dataGridViewUsuarios.Rows[n].Cells["CalleName"].Value;

                        textCP.Text = (string)dataGridViewUsuarios.Rows[n].Cells["CpName"].Value;

                        Button4_Click(sender, e);
                    string valor0 = (string)dataGridViewUsuarios.Rows[n].Cells["EstadoName"].Value;
                    selectestado(valor0);
                    string valor = (string)dataGridViewUsuarios.Rows[n].Cells["MunicipioName"].Value;
                        selectmunicipio(valor);
                        string valor2 = (string)dataGridViewUsuarios.Rows[n].Cells["ColoniaName"].Value;
                        selectcolonias(valor2);
                        textFolio.Focus();

                        button2.Enabled = true;
                        button7.Enabled = true;
                        button1.Enabled = false;
                    button1.BackColor = Color.White;

                    button7.BackColor = Color.FromArgb(38, 45, 53);
                    button2.BackColor = Color.FromArgb(38, 45, 53);
                    comboSocio.Text = dataGridViewUsuarios.Rows[n].Cells["ZonamName"].Value.ToString();
                        checkBox1.Checked =Convert.ToBoolean( dataGridViewUsuarios.Rows[n].Cells["CobrartajretasName"].Value);

                    }
                
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "DATAGRIVIEW1";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        public void getDatosAdicionales()
        {
            if (comboSocio.Items != null && comboSocio.Items.Count > 0)
            {
                comboSocio.Items.Clear();
            }

            string sql = "SELECT ZONA,PK FROM ZONAS ";
            db.PreparedSQL(sql);
            res = db.getTable();

            while (res.Next())
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = res.Get("ZONA");
                item.Value = res.Get("PK");
                comboSocio.Items.Add(item);

                //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
            }

        }


        private void selectmunicipio(string valor)
        {
            try
            {

                comboBoxMunicipios.Items.Clear();

                ComboboxItem item = new ComboboxItem();
                item.Value = 0;
                item.Text = valor;
                comboBoxMunicipios.Items.Add(item);

                comboBoxMunicipios.SelectedIndex = 0;

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "selectMUNICIPIO";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void selectestado(string valor)
        {
            try
            {

                comboBoxMunicipios.Items.Clear();

                ComboboxItem item = new ComboboxItem();
                item.Value = 0;
                item.Text = valor;
                comboBoxEstados.Items.Add(item);

                comboBoxEstados.SelectedIndex = 0;

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "selectMUNICIPIO";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void selectcolonias(string valor)
        {
            try
            {

                comboBoxColonias.Items.Clear();

                ComboboxItem item = new ComboboxItem();
                item.Value = 0;
                item.Text = valor;
                comboBoxColonias.Items.Add(item);

                comboBoxColonias.SelectedIndex = 0;

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "selectcolonias";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void actualizarbtn(object sender, EventArgs e)
        {


            try
            {


                dataGridViewUsuarios.Rows.Clear();
                dataGridViewUsuarios.Refresh();
                getRows();
                button7.Enabled = false;
                button1.Enabled = true;

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "actualizarbtn";
                Utilerias.LOG.write(_clase, funcion, error);


            }


        }
        private void errorborrar()
        {
            labelf.Visible = false;
            label15.Visible = false;
            label20.Visible = false;
            label21.Visible = false;
            label22.Visible = false;
            label23.Visible = false;
            label28.Visible = false;
            label4.Visible = false;


        }

        private Boolean ValidarInputsinsert()
        {
            Boolean valido = true;

            try
            {


                errorborrar();

                _sucursal = txtSucursal.Text;

                _folio = textFolio.Text;

                _calle = textCalle.Text;

                _cp = textCP.Text;

                if (comboBoxEstados.SelectedItem != null)
                {
                    _estado = comboBoxEstados.SelectedItem.ToString();

                }

                if (comboBoxMunicipios.SelectedItem != null)
                {
                    _municipio = comboBoxMunicipios.SelectedItem.ToString();

                }
                if (comboBoxColonias.SelectedItem != null)
                {
                    _colonia = comboBoxColonias.SelectedItem.ToString();

                }
                if (comboSocio.SelectedItem != null)
                {
                    _zona = comboSocio.SelectedItem.ToString();
                    _pkzona = (comboSocio.SelectedItem as ComboboxItem).Value.ToString();



                }
                if (string.IsNullOrEmpty(_folio))
                {
                    validando();

                    valido = false;
                }
                if (string.IsNullOrEmpty(_sucursal))
                {
                    validando();

                    valido = false;
                }
                if (string.IsNullOrEmpty(_calle))
                {
                    validando();

                    valido = false;
                }
                if (string.IsNullOrEmpty(_cp))
                {
                    validando();

                    valido = false;
                }
                if (string.IsNullOrEmpty(_estado))
                {
                    validando();

                    valido = false;
                }
                if (string.IsNullOrEmpty(_colonia))
                {
                    validando();

                    valido = false;
                }
                if (string.IsNullOrEmpty(_municipio))
                {
                    validando();

                    valido = false;
                }
                if (string.IsNullOrEmpty(_zona))
                {
                    validando();

                    valido = false;
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "validarinputinsert";
                Utilerias.LOG.write(_clase, funcion, error);


            }
            return valido;

        }

        private void validando()
        {
            try
            {
                if (string.IsNullOrEmpty(_folio))
                {
                    labelf.Visible = true;

                }
                if (string.IsNullOrEmpty(_zona))
                {
                    label4.Visible = true;

                }
                if (string.IsNullOrEmpty(_sucursal))
                {
                    label15.Visible = true;

                }

                if (string.IsNullOrEmpty(_calle))
                {
                    label20.Visible = true;
                }
                if (string.IsNullOrEmpty(_cp))
                {
                    label28.Visible = true;
                }
                if (string.IsNullOrEmpty(_estado))
                {
                    label21.Visible = true;
                }
                if (string.IsNullOrEmpty(_colonia))
                {
                    label23.Visible = true;
                }
                if (string.IsNullOrEmpty(_municipio))
                {
                    label22.Visible = true;
                }


            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "validando";
                Utilerias.LOG.write(_clase, funcion, error);


            }


        }

        private void btninsert(object sender, EventArgs e)
        {
            try
            {
                if (ValidarInputsinsert())
                {


                    string sql = "INSERT INTO SUCURSALES(FOLIO,SUCURSAL,CALLE,ESTADO,MUNICIPIO,COLONIA,CP,ZONA,PKZONA,BORRADO,USUARIO)" +
                                             " VALUES(@FOLIO,@SUCURSAL,@CALLE,@ESTADO,@MUNICIPIO,@COLONIA,@CP,@ZONA,@PKZONA,0,@USUARIO)";

                    db.PreparedSQL(sql);

                    db.command.Parameters.AddWithValue("@FOLIO", _folio);
                    db.command.Parameters.AddWithValue("@SUCURSAL", _sucursal);
                    db.command.Parameters.AddWithValue("@CALLE", _calle);
                    db.command.Parameters.AddWithValue("@ESTADO", _estado);
                    db.command.Parameters.AddWithValue("@MUNICIPIO", _municipio);
                    db.command.Parameters.AddWithValue("@COLONIA", _colonia);
                    db.command.Parameters.AddWithValue("@CP", _cp);
                    db.command.Parameters.AddWithValue("@ZONA", _zona);
                    db.command.Parameters.AddWithValue("@PKZONA", _pkzona);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                    if (db.execute())
                    {
                        delelte(sender, e);

                        Utilerias.LOG.acciones("agrego una sucursal " + _sucursal);

                       
                    }
                    else
                    {
                        MessageBox.Show("Faltan datos por llenar.");
                    }

                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "btninsert";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }



        private void btndelete(object sender, EventArgs e)
        {

            try
            {
                var confirmResult = MessageBox.Show("¿Esta seguro de eliminar el registro?",
                                     "Confirmar eliminar",
                                     MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    string sql;
                    sql = "DELETE FROM SUCURSALES WHERE SUCURSAL = @SUCURSAL";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SUCURSAL", _user);

                    if (db.execute())
                    {

                        if (n != -1)
                        {
                            dataGridViewUsuarios.Rows.RemoveAt(n);

                        }
                        delelte(sender, e);
                        textFolio.Focus();

                        button1.Enabled = true;
                        button2.Enabled = false;

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
                string funcion = "btndelete";
                Utilerias.LOG.write(_clase, funcion, error);


            }


        }


        private void limpiarbtn(object sender, EventArgs e)
        {
            try
            {


                txtSucursal.Text = "";

                textCalle.Text = "";
                textCP.Text = "";



                dataGridViewUsuarios.Rows.Clear();
                dataGridViewUsuarios.Refresh();
                getRows();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "limpiarbtn";
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


        //insertar


        private void btnupdate(object sender, EventArgs e)
        {
            try
            {
                string idd = (string)dataGridViewUsuarios.Rows[fila].Cells["IdName"].Value.ToString();


                if (ValidarInputsinsert())
                {


                    string sql = "UPDATE SUCURSALES SET FOLIO=@FOLIO, SUCURSAL=@SUCURSAL,CALLE=@CALLE,ESTADO=@ESTADO,MUNICIPIO=@MUNICIPIO,COLONIA=@COLONIA,USUARIO=@USUARIO," +
                        "CP=@CP  WHERE ID = @id";

                    db.PreparedSQL(sql);

                    db.command.Parameters.AddWithValue("@FOLIO", _folio);
                    db.command.Parameters.AddWithValue("@SUCURSAL", _sucursal);
                    db.command.Parameters.AddWithValue("@CALLE", _calle);
                    db.command.Parameters.AddWithValue("@ESTADO", _estado);
                    db.command.Parameters.AddWithValue("@MUNICIPIO", _municipio);
                    db.command.Parameters.AddWithValue("@COLONIA", _colonia);
                    db.command.Parameters.AddWithValue("@CP", _cp);
                    db.command.Parameters.AddWithValue("@ID", idd);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);

                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("modifico sucursal " + idd+" usuario"+LoginInfo.UserID);


                        delelte(sender, e);


                    }
                    
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "btnupdate";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }




        private void clearbtn(object sender, EventArgs e)
        {
            dataGridViewUsuarios.Rows.Clear();
            dataGridViewUsuarios.Refresh();
            getRows();
            button7.Enabled = false;
            button1.Enabled = true;
            txtSucursal.Text = "";


            textCalle.Text = "";
            textCP.Text = "";

        }



        private void delelte(object sender, EventArgs e)
        {
            try
            {

                txtSucursal.ReadOnly = false;
                txtSucursal.ReadOnly = false;
                errorborrar();
                button7.Enabled = false;
                button1.Enabled = true;
                button2.BackColor = Color.White;
                button2.Enabled = false;
                button7.BackColor = Color.White;
                button1.BackColor = Color.FromArgb(38, 45, 53);
                textFolio.Text = "";
                txtSucursal.Text = "";
                textCalle.Text = "";
                toolStripTextBox1.Text = "";
                comboSocio.Text = "";
                textCP.Text = "";
                comboBoxEstados.SelectedItem = 0;
                comboBoxMunicipios.SelectedItem = 0;
                comboBoxColonias.SelectedItem = 0;
                Estados();
                textFolio.Focus();
                comboBoxEstados.Items.Clear();
                comboBoxMunicipios.Items.Clear();
                comboBoxColonias.Items.Clear() ;
                dataGridViewUsuarios.Rows.Clear();
                getRows();

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "delelte";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void btneliminar(object sender, EventArgs e)
        {
            try
            {
                Form mensaje = new Mensaje("¿Está seguro de borrar la sucrusal?", false);

                DialogResult resut = mensaje.ShowDialog();
                if (resut == DialogResult.OK)
                {
                    string id = dataGridViewUsuarios.Rows[fila].Cells["IdName"].Value.ToString();
                    string sql;
                    sql = "UPDATE SUCURSALES SET BORRADO=1 WHERE ID= @ID";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@ID", id);

                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("BORRO UNA SUCURSA ID "+id+ "user: "+ LoginInfo.UserID);

                        if (n != -1)
                        {
                            delelte(sender, e)
 ;
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
                string funcion = "btneliminar";
                Utilerias.LOG.write(_clase, funcion, error);


            }


        }

        private void Button6_Click(object sender, EventArgs e)
        {
            dataGridViewUsuarios.Rows.Clear();
            dataGridViewUsuarios.Refresh();
            getRows();
            button7.Enabled = false;
            button1.Enabled = true;
        }
        private void Estadoscp(string valor)
        {
            try
            {


                comboBoxEstados.Items.Clear();

                string sql = "select c_estado, D_estado from SEPOMEX WHERE d_codigo = '" + valor + "' group by c_estado, D_estado";

                db.PreparedSQL(sql);
                res = db.getTable();

                if (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Value = res.Get("c_estado");
                    item.Text = res.Get("D_estado");
                    comboBoxEstados.Items.Add(item);
                
                    
                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }

               
                
                string selectedText = comboBoxEstados.Items[0].ToString();
                string value = (comboBoxEstados.Items[0] as ComboboxItem).Value.ToString();

                Municipios(value);

            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Codigo postal incorrecto.");
                string funcion = "estadocp";
                Estados();
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            _cp = textCP.Text;

            Estadoscp(_cp);




        }

        private void GroupBox2_Enter(object sender, EventArgs e)
        {

        }



        private void buscar(object sender, KeyEventArgs e)
        {
            btnbuscar(sender, e);

        }

        private void btnbuscar(object sender, EventArgs e)
        {
            try
            {

              
                _search = toolStripTextBox1.Text;
               
                if (!string.IsNullOrEmpty(_search))
                {
                    dataGridViewUsuarios.Rows.Clear();

                   string sql= "SELECT ID,FOLIO,SUCURSAL,CALLE,ESTADO,MUNICIPIO,COLONIA,CP,ZONA,PKZONA, cobrartarjetas FROM SUCURSALES  " +
                        "WHERE SUCURSAL LIKE @SEARCH OR FOLIO LIKE @SEARCH AND BORRADO=0";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + _search + "%");

                }
              

                res = db.getTable();

                while (res.Next())
                {
                    n = dataGridViewUsuarios.Rows.Add();


                    dataGridViewUsuarios.Rows[n].Cells["NoName"].Value = n;
                    dataGridViewUsuarios.Rows[n].Cells["IdName"].Value = res.Get("ID");
                    dataGridViewUsuarios.Rows[n].Cells["FolioName"].Value = res.Get("FOLIO");
                    dataGridViewUsuarios.Rows[n].Cells["SucursalName"].Value = res.Get("SUCURSAL");
                    dataGridViewUsuarios.Rows[n].Cells["CalleName"].Value = res.Get("CALLE");
                    dataGridViewUsuarios.Rows[n].Cells["EstadoName"].Value = res.Get("ESTADO");
                    dataGridViewUsuarios.Rows[n].Cells["MunicipioName"].Value = res.Get("MUNICIPIO");
                    dataGridViewUsuarios.Rows[n].Cells["ColoniaName"].Value = res.Get("COLONIA");
                    dataGridViewUsuarios.Rows[n].Cells["CpName"].Value = res.Get("CP");
                    dataGridViewUsuarios.Rows[n].Cells["IdName"].Value = res.GetInt("ID");
                    dataGridViewUsuarios.Rows[n].Cells["ZonamName"].Value = res.Get("ZONA");
                    dataGridViewUsuarios.Rows[n].Cells["pkzonaName"].Value = res.Get("PKZONA");
                    dataGridViewUsuarios.Rows[n].Cells["CobrartajretasName"].Value = res.Get("cobrartarjetas");
            
                    dataGridViewUsuarios.CurrentRow.Selected = false;



                }
                if (toolStripTextBox1.Text == "")
                {
                    clearbtn(sender,e);
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "btnbuscar";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void DataGridViewUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            comboSocio.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBoxEstados.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxMunicipios.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxColonias.DropDownStyle = ComboBoxStyle.DropDownList;
            dataGridViewUsuarios.EnableHeadersVisualStyles = false;

            db = new database();
            getRows();

            button2.Enabled = false;
            button7.Enabled = false;
            button2.BackColor = Color.White;
            button7.BackColor = Color.White;
            errorborrar();
            Estados();
            textFolio.Focus();
            getDatosAdicionales();
            timer1.Stop();

        }

        private void Sucursales_Load(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;
            timer1.Interval = 1;
            timer1.Start();

        }

        private void Sucursales_Shown(object sender, EventArgs e)
        {


        }

        private void ToolStripExportExcel_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridViewUsuarios);

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        //METODOS PARA CERRAR,MAXIMIZAR, MINIMIZAR FORMULARIO------------------------------------------------------
        int lx, ly;
        int sw, sh;

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



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string id = dataGridViewUsuarios.Rows[fila].Cells["IdName"].Value.ToString();

            if (checkBox1.Checked == true)
            {


                string sql = "UPDATE SUCURSALES set COBRARTARJETAS=@COBRARTARJETAS WHERE ID=@ID";

                db.PreparedSQL(sql);

                db.command.Parameters.AddWithValue("@COBRARTARJETAS", true);
                db.command.Parameters.AddWithValue("@ID", id);

                if (db.execute())
                {
                    Utilerias.LOG.acciones("activar pago de tarjetas " + LoginInfo.UserID);
                    Form mensaje = new Mensaje("Se activo el cobro automatico ", true);

                    DialogResult resut = mensaje.ShowDialog();
                    clearbtn(sender, e);

                }
            }
            else
            {
                string sql2 = "UPDATE SUCURSALES set COBRARTARJETAS=@COBRARTARJETAS WHERE ID=@ID";

                db.PreparedSQL(sql2);

                db.command.Parameters.AddWithValue("@COBRARTARJETAS", false);
                db.command.Parameters.AddWithValue("@ID", id);
                if (db.execute())
                {
                    Utilerias.LOG.acciones("activar pago de tarjetas " + LoginInfo.UserID);
                    Form mensaje = new Mensaje("Se desactivo cobro automatico ", true);

                    DialogResult resut = mensaje.ShowDialog();
                    clearbtn(sender, e);

                }
            }

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
