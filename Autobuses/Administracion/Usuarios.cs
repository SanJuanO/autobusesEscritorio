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
using DPFP;
using DPFP.Capture;
using System.Drawing.Imaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Reflection;

namespace Autobuses
{

    public partial class Usuarios : Form
    {


        Bitmap img = null;
        public database db;
        ResultSet res = null;
        Bitmap image;
        private int fila;
        private int columna;
        private int n = 0;
        private string _usuario;
        private string _password;
        private string _nombre;
        private string _apellidos;
        private string _telefono;
        private string _correo;
        private int _activo;
        private string _sucursal;
        private string _rol;
        private string _sucursal2;
        private string _rol2;
        private string _registro;
        private string _calle;
        private string _estado;
        private string _municipio;
        private string _colonia;
        private string _cp;
        private string _search;
        private bool MODIFICAR = false;
        private string _searchtool;
        private bool _validar = false;
        private string _dato1;
        private string _clase = "usuarios";
        private string _user;

        private Byte _foto;
        private Byte[] fotobyte;
        private Byte[] huell;
        private Bitmap huella1;
        private int inicio =0;
        private int final = 10;
        private int cantidad = 10;
        private int total = 0;

        private string huella164;

        private int huella = 0;
        private Boolean faltahuella = false;
        public Usuarios()
        {
            InitializeComponent();
            this.Show();
            titulo.Text = "Usuarios";

        }

        private void permisos()
        {
            buttonagregar.Visible = false;
            buttonborrar.Visible = false;
            buttonguardar.Visible = false;
            if (LoginInfo.privilegios.Any(x => x == "agregar usuarios"))
            {
                buttonagregar.Visible = true;

            }
            if (LoginInfo.privilegios.Any(x => x == "borrar a usuarios"))
            {

                buttonborrar.Visible = true;

            }
            if (LoginInfo.privilegios.Any(x => x == "modificar usuarios"))
            {

                buttonguardar.Visible = true;

            }
        }



        public void getRows()
        {
            try
            {
                int count = 1;
                dataGridViewUsuarios.Rows.Clear();
                string sql = "SELECT PK,USUARIO,NOMBRE,PASSWORD,APELLIDOS,TELEFONO,CORREO,ACTIVO,SUCURSAL,ROLE,FECHA_C, CALLE,ESTADO,MUNICIPIO,COLONIA,CP,ID,SUCID " +
                    "FROM VISTA1 WHERE BORRADO=0 ORDER BY APELLIDOS OFFSET @INICIO ROWS FETCH NEXT @CANTIDAD ROWS ONLY";


                db.PreparedSQL(sql);

                db.command.Parameters.AddWithValue("@CANTIDAD", cantidad);

                db.command.Parameters.AddWithValue("@INICIO", inicio);

                res = db.getTable();


                if (dataGridViewUsuarios.InvokeRequired)
                {
                    while (res.Next())

                    {
                        dataGridViewUsuarios.Invoke(new Action(() =>
                        {

                            n = dataGridViewUsuarios.Rows.Add();
                            dataGridViewUsuarios.Rows[n].Cells["PkName"].Value = res.Get("PK");
                            dataGridViewUsuarios.Rows[n].Cells["NoName"].Value = count;
                            dataGridViewUsuarios.Rows[n].Cells["UsuarioName"].Value = res.Get("USUARIO");
                            dataGridViewUsuarios.Rows[n].Cells["NombreName"].Value = res.Get("NOMBRE");
                            dataGridViewUsuarios.Rows[n].Cells["PasswordName"].Value = res.Get("PASSWORD");

                            dataGridViewUsuarios.Rows[n].Cells["ApellidosName"].Value = res.Get("APELLIDOS");
                            dataGridViewUsuarios.Rows[n].Cells["TelefonoName"].Value = res.Get("TELEFONO");
                            dataGridViewUsuarios.Rows[n].Cells["CorreoName"].Value = res.Get("CORREO");
                            dataGridViewUsuarios.Rows[n].Cells["ActivoName"].Value = res.Get("ACTIVO");
                            dataGridViewUsuarios.Rows[n].Cells["SucursalName"].Value = res.Get("SUCURSAL");
                            dataGridViewUsuarios.Rows[n].Cells["RolName"].Value = res.Get("ROLE");
                            _registro = res.Get("FECHA_C");
                            dataGridViewUsuarios.Rows[n].Cells["RegistroName"].Value = _registro;
                            dataGridViewUsuarios.Rows[n].Cells["CalleName"].Value = res.Get("CALLE");
                            dataGridViewUsuarios.Rows[n].Cells["EstadoName"].Value = res.Get("ESTADO");
                            dataGridViewUsuarios.Rows[n].Cells["MunicipioName"].Value = res.Get("MUNICIPIO");
                            dataGridViewUsuarios.Rows[n].Cells["ColoniaNAme"].Value = res.Get("COLONIA");
                            dataGridViewUsuarios.Rows[n].Cells["CpNAme"].Value = res.Get("CP");
                            dataGridViewUsuarios.Rows[n].Cells["rolidName"].Value = res.Get("ID");
                            dataGridViewUsuarios.Rows[n].Cells["sucidName"].Value = res.Get("SUCID");
                            //dataGridViewUsuarios.Rows[n].Cells[16].Value = Convert.FromBase64String(res.Get("IMAG"));
                            //dataGridViewUsuarios.Rows[n].Cells[19].Value = Convert.FromBase64String(res.Get("HUELLA1"));
                        }

                            ));
                        count++;
                    }

                }
                else
                {
                    while (res.Next())
                    {
                        n = dataGridViewUsuarios.Rows.Add();
                        dataGridViewUsuarios.Rows[n].Cells["PkName"].Value = res.Get("PK");
                        dataGridViewUsuarios.Rows[n].Cells["NoName"].Value = count;
                        dataGridViewUsuarios.Rows[n].Cells["UsuarioName"].Value = res.Get("USUARIO");
                        dataGridViewUsuarios.Rows[n].Cells["NombreName"].Value = res.Get("NOMBRE");
                        dataGridViewUsuarios.Rows[n].Cells["PasswordName"].Value = res.Get("PASSWORD");

                        dataGridViewUsuarios.Rows[n].Cells["ApellidosName"].Value = res.Get("APELLIDOS");
                        dataGridViewUsuarios.Rows[n].Cells["TelefonoName"].Value = res.Get("TELEFONO");
                        dataGridViewUsuarios.Rows[n].Cells["CorreoName"].Value = res.Get("CORREO");
                        dataGridViewUsuarios.Rows[n].Cells["ActivoName"].Value = res.Get("ACTIVO");
                        dataGridViewUsuarios.Rows[n].Cells["SucursalName"].Value = res.Get("SUCURSAL");
                        dataGridViewUsuarios.Rows[n].Cells["RolName"].Value = res.Get("ROLE");
                        _registro = res.Get("FECHA_C");
                        dataGridViewUsuarios.Rows[n].Cells["RegistroName"].Value = _registro;
                        dataGridViewUsuarios.Rows[n].Cells["CalleName"].Value = res.Get("CALLE");
                        dataGridViewUsuarios.Rows[n].Cells["EstadoName"].Value = res.Get("ESTADO");
                        dataGridViewUsuarios.Rows[n].Cells["MunicipioName"].Value = res.Get("MUNICIPIO");
                        dataGridViewUsuarios.Rows[n].Cells["ColoniaNAme"].Value = res.Get("COLONIA");
                        dataGridViewUsuarios.Rows[n].Cells["CpNAme"].Value = res.Get("CP");
                        dataGridViewUsuarios.Rows[n].Cells["rolidName"].Value = res.Get("ID");
                        dataGridViewUsuarios.Rows[n].Cells["sucidName"].Value = res.Get("SUCID");
                        //dataGridViewUsuarios.Rows[n].Cells[16].Value = Convert.FromBase64String(res.Get("IMAG"));
                        //dataGridViewUsuarios.Rows[n].Cells[19].Value = Convert.FromBase64String(res.Get("HUELLA1"));
                        count++;
                    }
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = " ws";
                Utilerias.LOG.write(_clase, funcion, error);
            }

        }


        public void getDatosAdicionales2()
        {
            try
            {
                if (comboBoxSucursal.InvokeRequired)
                {
                    comboBoxSucursal.Invoke(new Action(() =>
                    {
                        comboBoxSucursal.Items.Clear();

                    }
                        ));

                }
                else
                {
                    comboBoxSucursal.Items.Clear();

                }
                string sql = "SELECT SUCURSAL,ID FROM SUCURSALES ORDER BY SUCURSAL ASC";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("SUCURSAL");
                    item.Value = res.Get("ID");
                    if (comboBoxSucursal.InvokeRequired)
                    {
                        comboBoxSucursal.Invoke(new Action(() =>
                        {
                            comboBoxSucursal.Items.Add(item);

                        }
                            ));

                    }
                    else
                    {
                        comboBoxSucursal.Items.Add(item);

                    }

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getDatosAdicionales2";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        public void getDatosAdicionales()
        {
            try
            {
                if (comboBoxRole.InvokeRequired)
                {
                    comboBoxRole.Invoke(new Action(() =>
                    {
                        comboBoxRole.Items.Clear();

                    }
                        ));

                }
                else
                {
                    comboBoxRole.Items.Clear();

                }

                string sql = "SELECT ROLE,ID FROM ROLES ORDER BY ROLE ASC";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("ROLE");
                    item.Value = res.Get("ID");
                    if (comboBoxRole.InvokeRequired)
                    {
                        comboBoxRole.Invoke(new Action(() =>
                        {
                            comboBoxRole.Items.Add(item);

                        }
                            ));

                    }
                    else
                    {
                        comboBoxRole.Items.Add(item);

                    }

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getDatosAdicionales";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        public void getDatosAdicionales3()
        {
            try
            {

                string sql = "SELECT SUCURSAL,ID FROM SUCURSALES ";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("SUCURSAL");
                    item.Value = res.Get("ID");

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getDatosAdicionales3";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        public void getDatosAdicionales4()
        {
            try
            {


                string sql = "SELECT ROLE,ID FROM ROLES ";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("ROLE");
                    item.Value = res.Get("ID");

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getDatosAdicionales4";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        //asignado
        private void asignado(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                fila = e.RowIndex;
                columna = e.ColumnIndex;

                if (fila != -1 && columna != 7)
                {
                    buttonguardar.Enabled = true;
                    buttonagregar.Enabled = false;
                    buttonborrar.Enabled = true;


                    buttonborrar.BackColor = Color.FromArgb(38, 45, 56);
                    buttonguardar.BackColor = Color.FromArgb(38, 45, 56);

                    buttonagregar.BackColor = Color.White;
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "asignando";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        public void getPhotoPdf(string usermandado, int pos, string pk)
        {
            try
            {
                if (guardar.InvokeRequired)
                {

                    guardar.Invoke(new Action(() =>
                    {

                        guardar.Enabled = false;
                    }));
                }
                else
                {

                    guardar.Enabled = false;

                }
                if (button3.InvokeRequired)
                {

                    button3.Invoke(new Action(() =>
                    {

                        button3.Enabled = false;
                    }));
                }
                else
                {

                    button3.Enabled = false;

                }
                if (progressBar1.InvokeRequired)
                {

                    progressBar1.Invoke(new Action(() =>
                    {
                        progressBar1.Visible = true;
                        progressBar1.Increment(20);
                    }));
                }
                else
                {

                    progressBar1.Visible = true;
                    progressBar1.Increment(20);
                }

                string sql = "SELECT IMAG,HUELLA1 FROM usuarios WHERE BORRADO=0 ";
                /*
                Byte[] array = Encoding.ASCII.GetBytes("aqui va el campo de la base");
                MemoryStream ms = new MemoryStream(array);
                Bitmap bmp = new Bitmap(ms);
                */
                sql += "AND PK=@PK";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PK", pk);
                if (progressBar1.InvokeRequired)
                {

                    progressBar1.Invoke(new Action(() =>
                    {

                        progressBar1.Increment(30);
                    }));
                }
                else
                {


                    progressBar1.Increment(30);
                }
                res = db.getTable();
                if (progressBar1.InvokeRequired)
                {

                    progressBar1.Invoke(new Action(() =>
                    {

                        progressBar1.Increment(30);
                    }));
                }
                else
                {


                    progressBar1.Increment(30);
                }
                if (res.Next())
                {

                    dataGridViewUsuarios.Rows[pos].Cells["imagenName"].Value = (!string.IsNullOrEmpty(res.Get("IMAG"))) ? Convert.FromBase64String(res.Get("IMAG")) : null;

                    dataGridViewUsuarios.Rows[pos].Cells["imghuellaName"].Value = (!string.IsNullOrEmpty(res.Get("HUELLA1"))) ? Convert.FromBase64String(res.Get("HUELLA1")) : null;

                }
                if (dataGridViewUsuarios.Rows[pos].Cells["imagenName"].Value != null)
                {
                    MemoryStream ms = new MemoryStream((byte[])dataGridViewUsuarios.Rows[fila].Cells["imagenName"].Value);
                    Bitmap bmp = new Bitmap(ms);
                    pictureBox1.Image = bmp;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                }
                if (dataGridViewUsuarios.Rows[pos].Cells["imghuellaName"].Value != null)
                {
                    MemoryStream ms2 = new MemoryStream((byte[])dataGridViewUsuarios.Rows[fila].Cells["imghuellaName"].Value);
                    Bitmap bmp2 = new Bitmap(ms2);
                    huelladig.Image = bmp2;
                }
                if (guardar.InvokeRequired)
                {

                    guardar.Invoke(new Action(() =>
                    {

                        guardar.Visible = true;
                        guardar.Enabled = true;
                    }));
                }
                else
                {
                    guardar.Visible = true;
                    guardar.Enabled = true;

                }
                if (button3.InvokeRequired)
                {

                    button3.Invoke(new Action(() =>
                    {

                        button3.Enabled = true;
                    }));
                }
                else
                {

                    button3.Enabled = true;

                }

                if (progressBar1.InvokeRequired)
                {

                    progressBar1.Invoke(new Action(() =>
                    {

                        progressBar1.Visible = false;
                        progressBar1.Value = 50;
                    }));
                }
                else
                {


                    progressBar1.Visible = false;
                    progressBar1.Value = 50;
                }
            }
            catch (Exception e)
            {
                string error = e.Message;

            }

        }


        private void seleccionrol(string valor)
        {
            try
            {

                comboBoxRole.Items.Clear();

                ComboboxItem item = new ComboboxItem();
                item.Value = 0;
                item.Text = valor;
                comboBoxRole.Items.Add(item);

                comboBoxRole.SelectedIndex = 0;


            }


            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "seleccionrol";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void seleccionsuc(string valor)
        {
            try
            {

                comboBoxSucursal.Items.Clear();

                ComboboxItem item = new ComboboxItem();
                item.Value = 0;
                item.Text = valor;
                comboBoxSucursal.Items.Add(item);

                comboBoxSucursal.SelectedIndex = 0;


            }


            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "seleccionsuc";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        /*BOTON ACTUALIZAR*/
        private void botonactualizar(object sender, EventArgs e)
        {
            try
            {


                buttonborrar.BackColor = Color.White;
                buttonguardar.BackColor = Color.White;

                buttonagregar.BackColor = Color.FromArgb(38, 45, 56);
                dataGridViewUsuarios.Rows.Clear();
                getRows();
                buttonguardar.Enabled = false;
                buttonagregar.Enabled = true;



            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "actualizar";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        private void errorborrar()
        {

            label15.Visible = false;
            label17.Visible = false;
            label18.Visible = false;
            label19.Visible = false;
            label20.Visible = false;
            label21.Visible = false;
            label22.Visible = false;
            label23.Visible = false;
            label24.Visible = false;
            label25.Visible = false;
            label26.Visible = false;
            label27.Visible = false;
            label28.Visible = false;
            label29.Visible = false;
            labelhuella.Visible = false;
            txtid.Text = "";

        }

        private Boolean ValidarInputsinsert(int option)
        {
            Boolean valido = true;
            try
            {


                errorborrar();

                _usuario = txtUsuario.Text;
                _password = txtPassword.Text;
                _nombre = txtNombre.Text;
                _apellidos = txtApellidos.Text;
                _telefono = textTelefono.Text;
                _correo = textCorreo.Text;
                _calle = textCalle.Text;
                _estado = textEstado.Text;
                _municipio = textMunicipio.Text;
                _colonia = textColonia.Text;
                _cp = textCP.Text;

                if (comboBoxRole.SelectedItem != null)
                {
                    _rol = (comboBoxRole.SelectedItem as ComboboxItem).Value.ToString();
                }

                if (comboBoxSucursal.SelectedItem != null)
                {
                    _sucursal = (comboBoxSucursal.SelectedItem as ComboboxItem).Value.ToString();
                }



                if (string.IsNullOrEmpty(_usuario))
                {
                    validando();
                    valido = false;

                }
                if (string.IsNullOrEmpty(_password))
                {
                    validando();

                    valido = false;
                }
                if (string.IsNullOrEmpty(_nombre))
                {
                    validando();

                    valido = false;
                }
                if (string.IsNullOrEmpty(_apellidos))
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
                if (string.IsNullOrEmpty(_telefono))
                {
                    validando();

                    valido = false;
                }
                if (string.IsNullOrEmpty(_correo))
                {
                    validando();

                    valido = false;
                }
                if (string.IsNullOrEmpty(_telefono))
                {
                    validando();

                    valido = false;
                }

                //if (string.IsNullOrEmpty(_dato1))
                //{
                //    validando();
                //    valido = false;
                //}
                if (string.IsNullOrEmpty(_rol))
                {
                    validando();

                    valido = false;
                }
                if (string.IsNullOrEmpty(_sucursal))
                {
                    validando();

                    valido = false;
                }
                //if (option == 0 && huell ==null)
                //{
                //    validando();
                //    labelhuella.Visible = true;
                //    valido = false;
                //}

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
                progressBar1.Visible = false;
                progressBar1.Value = 0;

                if (string.IsNullOrEmpty(_usuario))
                {

                    label15.Visible = true;
                }
                if (string.IsNullOrEmpty(_password))
                {
                    label17.Visible = true;
                }
                if (string.IsNullOrEmpty(_nombre))
                {
                    label18.Visible = true;
                }
                if (string.IsNullOrEmpty(_apellidos))
                {
                    label19.Visible = true;
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
                if (string.IsNullOrEmpty(_telefono))
                {
                    label27.Visible = true;
                }
                if (string.IsNullOrEmpty(_correo))
                {
                    label26.Visible = true;
                }

                //if (string.IsNullOrEmpty(_dato1))
                //{
                //    label29.Visible = true;

                //}
                if (string.IsNullOrEmpty(_rol) || _rol == "0")
                {
                    label24.Visible = true;
                }
                if (string.IsNullOrEmpty(_sucursal) || _sucursal == "0")
                {
                    label25.Visible = true;

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

        private void botoninsertar(object sender, EventArgs e)
        {
            dataGridViewUsuarios.Visible = false;
            button3.Visible = false;
            groupBoxh.Visible = false;
            panelContenedorForm.Visible = true;
            guardar.Visible = false;
            agregar.Visible = true;
            button3.Visible = false;
            txtUsuario.Focus();
            MODIFICAR = false;


        }

        private void botoneliminar(object sender, EventArgs e)
        {

            try
            {
                Form mensaje = new Mensaje("¿Está seguro de eliminar el registro?", false);

                DialogResult resut = mensaje.ShowDialog();
                if (resut == DialogResult.OK)
                {

                    string pk = (string)dataGridViewUsuarios.Rows[fila].Cells["PkName"].Value;

                    string sql;
                    sql = "UPDATE USUARIOS SET BORRADO=1, USUARIO_M=@USUARIOM WHERE PK = @PK AND BORRADO=0";


                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", pk);
                    db.command.Parameters.AddWithValue("@USUARIOM", LoginInfo.PkUsuario);

                    if (db.execute())
                    {

                        Utilerias.LOG.acciones("borro el usuario " + _user);

                        if (fila != -1)
                        {
                            dataGridViewUsuarios.Rows.RemoveAt(fila);

                        }
                        limpiarr(sender, e);
                        txtUsuario.Focus();
                        dataGridViewUsuarios.Visible = true;
                        buttonguardar.Enabled = false;
                        buttonborrar.Enabled = false;


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
                string funcion = "eliminar";
                Utilerias.LOG.write(_clase, funcion, error);

            }


        }




        private void onKeyEnterSearchtool(object sender, KeyEventArgs e)
        {
            //  Buttonbuscar(sender, e);
        }


        private void foto(object sender, EventArgs e)
        {
            try
            {
                if (CheckOpened("Tomar Foto"))
                {


                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                }
                else
                {
                    string dato = "Usuarios";
                    Form form = new TomarFoto(dato);
                    AddOwnedForm(form);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                }


            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error foto funcion, intente de nuevo.");
                string funcion = "foto";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        public void AsignarFoto(Bitmap img)
        {

            try
            {
                pictureBox1.Image = img;

                ImageConverter converter = new ImageConverter();

                byte[] ima = (byte[])converter.ConvertTo(img, typeof(byte[]));

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                _dato1 = Convert.ToBase64String(ima);
                if (MODIFICAR == true)
                {
                    if (progressBar1.InvokeRequired)
                    {
                        progressBar1.Invoke(new Action(() =>
                        {
                            progressBar1.Value = 50;
                            progressBar1.Visible = true;

                        }
                        ));
                    }
                    else
                    {
                        progressBar1.Value = 50;
                        progressBar1.Visible = true;
                    }
                    //actualizar contrato
                    string sql = "update USUARIOS set IMAG=@IMAG,USUARIO_M=@US where PK=@pk";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@US", LoginInfo.UserID);
                    db.command.Parameters.AddWithValue("@pk", txtPk.Text);
                    db.command.Parameters.AddWithValue("@IMAG", _dato1);
                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("actualizo contrato " + "pkchofer=" + txtPk.Text + LoginInfo.UserID);


                    }
                    else
                    {
                        if (!Utilerias.Utilerias.CheckForInternetConnection())
                        {
                            Form mensaje1 = new Mensaje("No se pudo actualizar la fotografia, verifique conexion a internet", true);

                            mensaje1.ShowDialog();
                        }
                        else
                        {
                            Form mensaje = new Mensaje("No se pudo actualizar la fotografia", true);

                            mensaje.ShowDialog();
                        }
                    }
                    if (progressBar1.InvokeRequired)
                    {
                        progressBar1.Invoke(new Action(() =>
                        {
                            progressBar1.Value = 0;
                            progressBar1.Visible = false;

                        }
                        ));
                    }
                    else
                    {
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;
                    }

                }


            }

            catch (Exception err)
            {
                if (progressBar1.InvokeRequired)
                {
                    progressBar1.Invoke(new Action(() =>
                    {
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;

                    }
                    ));
                }
                else
                {
                    progressBar1.Value = 0;
                    progressBar1.Visible = false;
                }
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "asignarfoto";
                Utilerias.LOG.write(_clase, funcion, error);


            }




        }
        //public void AsignarFoto(Bitmap img)
        //{
        //    try
        //    {
        //        pictureBox1.Image = img;

        //        ImageConverter converter = new ImageConverter();
        //        byte[] ima = (byte[])converter.ConvertTo(img, typeof(byte[]));

        //        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

        //        _dato1 = Convert.ToBase64String(ima);


        //    }
        //    catch (Exception err)
        //    {
        //        string error = err.Message;
        //        MessageBox.Show("Ocurrio un Error, intente de nuevo.");
        //        string funcion = "asignarfoto";
        //        Utilerias.LOG.write(_clase, funcion, error);
        //    }



        //}

        private void usuarios_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                /*
                ContextMenuStrip mymenu = new ContextMenuStrip();

                mymenu.Items.Add("Ocultar columna");
                mymenu.Items[0].Name = "ColHidden";
                mymenu.Items[0].Tag = e.ColumnIndex;

                mymenu.Show(dataGridViewUsuarios, new Point(e.X, e.Y));

                /*
                dataGridViewUsuarios.CurrentCell = dataGridViewUsuarios.Rows[e.RowIndex].Cells[e.ColumnIndex];

                ContextMenuStrip menu = new ContextMenuStrip();
                menu.Items.Add("agregar").Name = "AGREGAR";
                menu.Items.Add("eliminar").Name = "Eliminar";
                menu.Items.Add("detalles").Name = "DETALLES";


                //Obtienes las coordenadas de la celda seleccionada. 
                Rectangle coordenada = dataGridViewUsuarios.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                int anchoCelda = coordenada.Location.X; //Ancho de la localizacion de la celda
                int altoCelda = coordenada.Location.Y;  //Alto de la localizacion de la celda

                //Y para mostrar el menú lo haces de esta forma:  
                int X = anchoCelda + dataGridViewUsuarios.Location.X;
                int Y = dataGridViewUsuarios.Location.Y-40;

                menu.Show(dataGridViewUsuarios, new Point(X, Y));*/
            }

        }



        private void refresh(object sender, EventArgs e)
        {
            try
            {


                txtUsuario.Text = "";
                txtPassword.Text = "";
                txtNombre.Text = "";
                txtApellidos.Text = "";
                textCalle.Text = "";
                textCP.Text = "";
                textColonia.Text = "";
                textEstado.Text = "";
                textMunicipio.Text = "";
                textTelefono.Text = "";
                textCorreo.Text = "";
                dataGridViewUsuarios.Rows.Clear();
                dataGridViewUsuarios.Refresh();
                getRows();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "refresh";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void toolStripCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripExportExcel_Click(object sender, EventArgs e)
        {
            progressBar2.Visible = true;
            progressBar2.Increment(40);
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridViewUsuarios, new List<string> { "imagenName", "PasswordName", "imghuellaName", "sucidName", "rolidName", "PkName" });
            progressBar2.Increment(100);
            progressBar2.Visible = false;
            progressBar2.Value = 0;
        }



        private void update(object sender, EventArgs e)
        {
            try
            {
                groupBoxh.Visible = true;

                int n = fila;
                int c = columna;
                if (n != -1 && c != 7)
                {
                    MODIFICAR = true;

                    dataGridViewUsuarios.Visible = false;
                    button3.Visible = true;
                    guardar.Visible = true;
                    agregar.Visible = false;
                    buttonguardar.Enabled = false;
                    buttonguardar.BackColor = Color.White;
                    panelContenedorForm.Visible = true;
                    txtUsuario.Focus();
                    txtPk.Text = (string)dataGridViewUsuarios.Rows[n].Cells["PkName"].Value;


                    backgroundWorker2.RunWorkerAsync();


                    txtUsuario.Text = (string)dataGridViewUsuarios.Rows[n].Cells["UsuarioName"].Value;
                    txtPassword.Text = (string)dataGridViewUsuarios.Rows[n].Cells["PasswordName"].Value;

                    _user = (string)dataGridViewUsuarios.Rows[n].Cells["UsuarioName"].Value;
                    txtNombre.Text = (string)dataGridViewUsuarios.Rows[n].Cells["NombreName"].Value;
                    txtApellidos.Text = (string)dataGridViewUsuarios.Rows[n].Cells["ApellidosName"].Value;
                    textTelefono.Text = (string)dataGridViewUsuarios.Rows[n].Cells["TelefonoName"].Value;
                    textCorreo.Text = (string)dataGridViewUsuarios.Rows[n].Cells["CorreoName"].Value;
                    textCalle.Text = (string)dataGridViewUsuarios.Rows[n].Cells["CalleName"].Value;
                    textEstado.Text = (string)dataGridViewUsuarios.Rows[n].Cells["EstadoName"].Value;
                    textMunicipio.Text = (string)dataGridViewUsuarios.Rows[n].Cells["MunicipioName"].Value;
                    textColonia.Text = (string)dataGridViewUsuarios.Rows[n].Cells["ColoniaNAme"].Value;
                    textCP.Text = (string)dataGridViewUsuarios.Rows[n].Cells["CpNAme"].Value;


                    string valorrol = (string)dataGridViewUsuarios.Rows[n].Cells["RolName"].Value;
                    //seleccionrol(valorrol);
                    comboBoxRole.Text = valorrol;
                    string valorsuc = (string)dataGridViewUsuarios.Rows[fila].Cells["SucursalName"].Value;
                    //seleccionsuc(valorsuc);
                    comboBoxSucursal.Text = valorsuc;



                }



            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "asignando";
                Utilerias.LOG.write(_clase, funcion, error);


            }



        }


        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            botoninsertar(sender, e);
        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            update(sender, e);
        }

        private void buscar(object sender, EventArgs e)
        {
            try
            {
                buttonborrar.Enabled = false;
                buttonguardar.Enabled = false;
                buttonagregar.Enabled = true;

                dataGridViewUsuarios.Rows.Clear();
                _searchtool = txtid.Text;

                string sql = "SELECT PK,USUARIO,NOMBRE,PASSWORD,APELLIDOS,TELEFONO,CORREO,ACTIVO,SUCURSAL,ROLE,FECHA_C,CALLE,ESTADO,MUNICIPIO,COLONIA,CP,ID,SUCID FROM VISTA1 ORDER BY APELLIDOS";

                if (!string.IsNullOrEmpty(_searchtool))
                {
                    dataGridViewUsuarios.Rows.Clear();

                    string sql2 = "SELECT PK,USUARIO,NOMBRE,PASSWORD,APELLIDOS,TELEFONO,CORREO,ACTIVO,SUCURSAL,ROLE,FECHA_C,CALLE,ESTADO,MUNICIPIO,COLONIA,CP,ID,SUCID FROM VISTA1 " +
                        "WHERE USUARIO LIKE @SEARCH OR NOMBRE LIKE @SEARCH OR APELLIDOS LIKE @SEARCH ORDER BY USUARIO";
                    db.PreparedSQL(sql2);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + _searchtool + "%");
                    dataGridViewUsuarios.Rows.Clear();
                    dataGridViewUsuarios.Refresh();
                }
                else
                {
                    sql += " ORDER BY USUARIO ";
                    db.PreparedSQL(sql);

                }

                res = db.getTable();


                while (res.Next())
                {

                    n = dataGridViewUsuarios.Rows.Add();
                    dataGridViewUsuarios.Rows[n].Cells["PkName"].Value = res.Get("PK");

                    dataGridViewUsuarios.Rows[n].Cells["UsuarioName"].Value = res.Get("USUARIO");
                    dataGridViewUsuarios.Rows[n].Cells["NombreName"].Value = res.Get("NOMBRE");
                    dataGridViewUsuarios.Rows[n].Cells["PasswordName"].Value = res.Get("PASSWORD");

                    dataGridViewUsuarios.Rows[n].Cells["ApellidosName"].Value = res.Get("APELLIDOS");
                    dataGridViewUsuarios.Rows[n].Cells["TelefonoName"].Value = res.Get("TELEFONO");
                    dataGridViewUsuarios.Rows[n].Cells["CorreoName"].Value = res.Get("CORREO");
                    dataGridViewUsuarios.Rows[n].Cells["ActivoName"].Value = res.Get("ACTIVO");
                    dataGridViewUsuarios.Rows[n].Cells["SucursalName"].Value = res.Get("SUCURSAL");
                    dataGridViewUsuarios.Rows[n].Cells["RolName"].Value = res.Get("ROLE");
                    _registro = res.Get("FECHA_C");
                    dataGridViewUsuarios.Rows[n].Cells["RegistroName"].Value = _registro;
                    dataGridViewUsuarios.Rows[n].Cells["CalleName"].Value = res.Get("CALLE");
                    dataGridViewUsuarios.Rows[n].Cells["EstadoName"].Value = res.Get("ESTADO");
                    dataGridViewUsuarios.Rows[n].Cells["MunicipioName"].Value = res.Get("MUNICIPIO");
                    dataGridViewUsuarios.Rows[n].Cells["ColoniaNAme"].Value = res.Get("COLONIA");
                    dataGridViewUsuarios.Rows[n].Cells["CpNAme"].Value = res.Get("CP");
                    dataGridViewUsuarios.Rows[n].Cells["rolidName"].Value = res.Get("ID");
                    dataGridViewUsuarios.Rows[n].Cells["sucidName"].Value = res.Get("SUCID");
                    //dataGridViewUsuarios.Rows[n].Cells[16].Value = Convert.FromBase64String(res.Get("IMAG"));
                    //dataGridViewUsuarios.Rows[n].Cells[19].Value = Convert.FromBase64String(res.Get("HUELLA1"));
                    dataGridViewUsuarios.CurrentRow.Selected = false;
                }
                if (txtid.Text == "")
                {
                    dataGridViewUsuarios.Rows.Clear();
                    dataGridViewUsuarios.Refresh();
                    getRows();

                    dataGridViewUsuarios.CurrentRow.Selected = false;
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



        //private void buscarcombobox2(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        _search = (comboBoxRole2.SelectedItem as ComboboxItem).Value.ToString();
        //        string sql = "SELECT * FROM VISTA1 ";

        //        if (!string.IsNullOrEmpty(_search))
        //        {
        //            dataGridViewUsuarios.Rows.Clear();

        //            sql += "WHERE ROL LIKE @SEARCH ";
        //            db.PreparedSQL(sql);
        //            db.command.Parameters.AddWithValue("@SEARCH", "%" + _search + "%");

        //        }
        //        else
        //        {
        //            db.PreparedSQL(sql);

        //        }

        //        res = db.getTable();

        //        while (res.Next())
        //        {
        //            n = dataGridViewUsuarios.Rows.Add();



        //            dataGridViewUsuarios.Rows[n].Cells[1].Value = res.Get("USUARIO");
        //            dataGridViewUsuarios.Rows[n].Cells[3].Value = res.Get("NOMBRE");
        //            dataGridViewUsuarios.Rows[n].Cells[4].Value = res.Get("APELLIDOS");
        //            dataGridViewUsuarios.Rows[n].Cells[5].Value = res.Get("TELEFONO");
        //            dataGridViewUsuarios.Rows[n].Cells[6].Value = res.Get("CORREO");

        //            //activo
        //            dataGridViewUsuarios.Rows[n].Cells[8].Value = res.Get("SUCURSAL");
        //            dataGridViewUsuarios.Rows[n].Cells[9].Value = res.Get("ROLE");
        //            //registro
        //            _registro = res.Get("FECHA_C");
        //            dataGridViewUsuarios.Rows[n].Cells[10].Value = _registro;
        //            dataGridViewUsuarios.Rows[n].Cells[11].Value = res.Get("CALLE");
        //            dataGridViewUsuarios.Rows[n].Cells[12].Value = res.Get("ESTADO");
        //            dataGridViewUsuarios.Rows[n].Cells[13].Value = res.Get("MUNICIPIO");
        //            dataGridViewUsuarios.Rows[n].Cells[14].Value = res.Get("COLONIA");
        //            dataGridViewUsuarios.Rows[n].Cells[15].Value = res.Get("CP");
        //            dataGridViewUsuarios.Rows[n].Cells[16].Value = Encoding.ASCII.GetBytes(res.Get("IMAG"));

        //        }

        //    }
        //    catch (Exception err)
        //    {
        //        string error = err.Message;
        //        MessageBox.Show("Ocurrio un Error, intente de nuevo.");
        //        string funcion = "buscarcobox2";
        //        Utilerias.LOG.write(_clase, funcion, error);


        //    }

        //}

        //private void buscarsucbox(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        _search = (comboBoxSucursal2.SelectedItem as ComboboxItem).Value.ToString();
        //        string sql = "SELECT * FROM VISTA1 ";

        //        if (!string.IsNullOrEmpty(_search))
        //        {
        //            dataGridViewUsuarios.Rows.Clear();

        //            sql += "WHERE USSUC LIKE @SEARCH ";
        //            db.PreparedSQL(sql);
        //            db.command.Parameters.AddWithValue("@SEARCH", "%" + _search + "%");

        //        }
        //        else
        //        {
        //            db.PreparedSQL(sql);

        //        }

        //        res = db.getTable();

        //        while (res.Next())
        //        {
        //            n = dataGridViewUsuarios.Rows.Add();



        //            dataGridViewUsuarios.Rows[n].Cells[1].Value = res.Get("USUARIO");
        //            dataGridViewUsuarios.Rows[n].Cells[3].Value = res.Get("NOMBRE");
        //            dataGridViewUsuarios.Rows[n].Cells[4].Value = res.Get("APELLIDOS");
        //            dataGridViewUsuarios.Rows[n].Cells[5].Value = res.Get("TELEFONO");
        //            dataGridViewUsuarios.Rows[n].Cells[6].Value = res.Get("CORREO");

        //            //activo
        //            dataGridViewUsuarios.Rows[n].Cells[8].Value = res.Get("SUCURSAL");
        //            dataGridViewUsuarios.Rows[n].Cells[9].Value = res.Get("ROLE");
        //            //registro
        //            _registro = res.Get("FECHA_C");
        //            dataGridViewUsuarios.Rows[n].Cells[10].Value = _registro;
        //            dataGridViewUsuarios.Rows[n].Cells[11].Value = res.Get("CALLE");
        //            dataGridViewUsuarios.Rows[n].Cells[12].Value = res.Get("ESTADO");
        //            dataGridViewUsuarios.Rows[n].Cells[13].Value = res.Get("MUNICIPIO");
        //            dataGridViewUsuarios.Rows[n].Cells[14].Value = res.Get("COLONIA");
        //            dataGridViewUsuarios.Rows[n].Cells[15].Value = res.Get("CP");
        //            dataGridViewUsuarios.Rows[n].Cells[16].Value = Encoding.ASCII.GetBytes(res.Get("IMAG"));

        //        }

        //    }
        //    catch (Exception err)
        //    {
        //        string error = err.Message;
        //        MessageBox.Show("Ocurrio un Error, intente de nuevo.");
        //        string funcion = "buscarsucbox";
        //        Utilerias.LOG.write(_clase, funcion, error);


        //    }
        //}


        //private void buscarcoboxrol(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        comboBoxSucursal2.Enabled = false;
        //        _search = (string)(comboBoxRole2.SelectedItem as ComboboxItem).Value.ToString(); ;
        //        string sql = "SELECT * FROM VISTA1 ";

        //        if (!string.IsNullOrEmpty(_search))
        //        {
        //            dataGridViewUsuarios.Rows.Clear();

        //            sql += "WHERE ID LIKE @SEARCH OR NOMBRE LIKE @SEARCH OR APELLIDOS LIKE @SEARCH ";
        //            db.PreparedSQL(sql);
        //            db.command.Parameters.AddWithValue("@SEARCH", "%" + _search + "%");

        //        }
        //        else
        //        {
        //            db.PreparedSQL(sql);

        //        }

        //        res = db.getTable();

        //        while (res.Next())
        //        {
        //            n = dataGridViewUsuarios.Rows.Add();



        //            dataGridViewUsuarios.Rows[n].Cells[1].Value = res.Get("USUARIO");
        //            dataGridViewUsuarios.Rows[n].Cells[3].Value = res.Get("NOMBRE");
        //            dataGridViewUsuarios.Rows[n].Cells[4].Value = res.Get("APELLIDOS");
        //            dataGridViewUsuarios.Rows[n].Cells[5].Value = res.Get("TELEFONO");
        //            dataGridViewUsuarios.Rows[n].Cells[6].Value = res.Get("CORREO");

        //            //activo
        //            dataGridViewUsuarios.Rows[n].Cells[8].Value = res.Get("SUCURSAL");
        //            dataGridViewUsuarios.Rows[n].Cells[9].Value = res.Get("ROLE");
        //            //registro
        //            _registro = res.Get("FECHA_C");
        //            dataGridViewUsuarios.Rows[n].Cells[10].Value = _registro;
        //            dataGridViewUsuarios.Rows[n].Cells[11].Value = res.Get("CALLE");
        //            dataGridViewUsuarios.Rows[n].Cells[12].Value = res.Get("ESTADO");
        //            dataGridViewUsuarios.Rows[n].Cells[13].Value = res.Get("MUNICIPIO");
        //            dataGridViewUsuarios.Rows[n].Cells[14].Value = res.Get("COLONIA");
        //            dataGridViewUsuarios.Rows[n].Cells[15].Value = res.Get("CP");
        //            dataGridViewUsuarios.Rows[n].Cells[16].Value = Encoding.ASCII.GetBytes(res.Get("IMAG"));

        //        }

        //    }
        //    catch (Exception err)
        //    {
        //        string error = err.Message;
        //        MessageBox.Show("Ocurrio un Error, intente de nuevo.");
        //        string funcion = "buscarcoboxrol";
        //        Utilerias.LOG.write(_clase, funcion, error);


        //    }
        //}

        //private void buscarsucr2(object sender, EventArgs e)
        //{
        //    comboBoxRole2.Enabled = false;
        //    try
        //    {

        //            _search = (string)(comboBoxSucursal2.SelectedItem as ComboboxItem).Value.ToString();

        //        string sql = "SELECT * FROM VISTA1 ";

        //        if (!string.IsNullOrEmpty(_search))
        //        {
        //            dataGridViewUsuarios.Rows.Clear();

        //            sql += "WHERE SUCID LIKE @SEARCH OR NOMBRE LIKE @SEARCH OR APELLIDOS LIKE @SEARCH ";
        //            db.PreparedSQL(sql);
        //            db.command.Parameters.AddWithValue("@SEARCH", "%" + _search + "%");

        //        }
        //        else
        //        {
        //            db.PreparedSQL(sql);

        //        }

        //        res = db.getTable();

        //        while (res.Next())
        //        {
        //            n = dataGridViewUsuarios.Rows.Add();



        //            dataGridViewUsuarios.Rows[n].Cells[1].Value = res.Get("USUARIO");
        //            dataGridViewUsuarios.Rows[n].Cells[3].Value = res.Get("NOMBRE");
        //            dataGridViewUsuarios.Rows[n].Cells[4].Value = res.Get("APELLIDOS");
        //            dataGridViewUsuarios.Rows[n].Cells[5].Value = res.Get("TELEFONO");
        //            dataGridViewUsuarios.Rows[n].Cells[6].Value = res.Get("CORREO");

        //            //activo
        //            dataGridViewUsuarios.Rows[n].Cells[8].Value = res.Get("SUCURSAL");
        //            dataGridViewUsuarios.Rows[n].Cells[9].Value = res.Get("ROLE");
        //            //registro
        //            _registro = res.Get("FECHA_C");
        //            dataGridViewUsuarios.Rows[n].Cells[10].Value = _registro;
        //            dataGridViewUsuarios.Rows[n].Cells[11].Value = res.Get("CALLE");
        //            dataGridViewUsuarios.Rows[n].Cells[12].Value = res.Get("ESTADO");
        //            dataGridViewUsuarios.Rows[n].Cells[13].Value = res.Get("MUNICIPIO");
        //            dataGridViewUsuarios.Rows[n].Cells[14].Value = res.Get("COLONIA");
        //            dataGridViewUsuarios.Rows[n].Cells[15].Value = res.Get("CP");
        //            dataGridViewUsuarios.Rows[n].Cells[16].Value = Encoding.ASCII.GetBytes(res.Get("IMAG"));

        //        }
        //        db.execute();
        //    }

        //    catch (Exception err)
        //    {
        //        string error = err.Message;
        //        MessageBox.Show("Ocurrio un Error, intente de nuevo.");
        //        string funcion = "buscarsucr2";
        //        Utilerias.LOG.write(_clase, funcion, error);


        //    }
        //}

        private void Check(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                n = e.RowIndex;
                int c = e.ColumnIndex;
                if (n >= 0 && c == 7)
                {
                    if (LoginInfo.privilegios.Any(x => x == "modificar usuarios"))
                    {

                        buttonguardar.Visible = true;



                        string user = (string)dataGridViewUsuarios.Rows[n].Cells["PkName"].Value;

                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dataGridViewUsuarios.Rows[n].Cells[c];
                        if (chk.Selected == true)

                        {
                            string act = (string)dataGridViewUsuarios.Rows[n].Cells[7].Value.ToString();
                            string val = act;

                            if (act == "False")
                            {

                                string sql = "UPDATE USUARIOS SET ACTIVO=@ACTIVO, USUARIO_M=@USUARIOM WHERE PK=@PK  AND BORRADO=0";


                                db.PreparedSQL(sql);
                                db.command.Parameters.AddWithValue("@PK", user);
                                db.command.Parameters.AddWithValue("@USUARIOM", LoginInfo.PkUsuario);
                                db.command.Parameters.AddWithValue("@ACTIVO", 1);

                                if (db.execute())
                                {
                                    Form mensaje = new Mensaje("El usuario activado", true);

                                    DialogResult resut = mensaje.ShowDialog();
                                    dataGridViewUsuarios.Rows.Clear();
                                    dataGridViewUsuarios.Refresh();
                                    getRows();

                                }
                                else
                                {
                                    Form mensaje = new Mensaje("no se puede acutalizar el registro", true);

                                    DialogResult resut = mensaje.ShowDialog();
                                }
                            }
                            else if (act == "True")
                            {


                                string sql = "UPDATE USUARIOS SET ACTIVO=@ACTIVO, USUARIO_M=@USUARIOM WHERE PK=@PK AND BORRADO=0";


                                db.PreparedSQL(sql);
                                db.command.Parameters.AddWithValue("@PK", user);
                                db.command.Parameters.AddWithValue("@USUARIOM", LoginInfo.PkUsuario);
                                db.command.Parameters.AddWithValue("@ACTIVO", 0);
                                if (db.execute())
                                {
                                    Form mensaje = new Mensaje("El usuario desctivado", true);

                                    DialogResult resut = mensaje.ShowDialog();
                                    dataGridViewUsuarios.Rows.Clear();
                                    dataGridViewUsuarios.Refresh();
                                    getRows();
                                    txtUsuario.Focus();

                                }
                                else
                                {
                                    Form mensaje = new Mensaje("no se puede acutalizar el registro", true);

                                    DialogResult resut = mensaje.ShowDialog();
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
                string funcion = "check";
                Utilerias.LOG.write(_clase, funcion, error);
            }



        }
        private void clearhuella()
        {
            huelladig.Image = null;
            faltahuella = false;
            huella1 = null;

            huell = null;

        }

        public void clearForm()
        {

            comboBoxRole.SelectedItem = null;
            comboBoxSucursal.SelectedItem = null;
            txtPk.Text = "";
            txtUsuario.Text = "";
            txtPassword.Text = "";
            txtNombre.Text = "";
            txtApellidos.Text = "";
            textTelefono.Text = "";
            textCorreo.Text = "";
            clearhuella();
            textCalle.Text = "";
            textEstado.Text = "";
            textMunicipio.Text = "";
            textColonia.Text = "";
            textCP.Text = "";
            pictureBox1.Image = null;
            _dato1 = null;
            txtid.Text = "";

        }

        private void limpiarr(object sender, EventArgs e)
        {
            try
            {
                panelContenedorForm.Visible = false;
                dataGridViewUsuarios.Visible = true;
                dataGridViewUsuarios.Rows.Clear();
                dataGridViewUsuarios.Refresh();
                getRows();
                buttonguardar.Enabled = false;
                buttonagregar.Enabled = true;
                dataGridViewUsuarios.Visible = true;


                buttonborrar.BackColor = Color.White;
                buttonguardar.BackColor = Color.White;

                buttonagregar.BackColor = Color.FromArgb(38, 45, 56);
                errorborrar();

                comboBoxRole.SelectedItem = null;
                comboBoxSucursal.SelectedItem = null;
                txtPk.Text = "";
                txtUsuario.Text = "";
                txtPassword.Text = "";
                txtNombre.Text = "";
                txtApellidos.Text = "";
                textTelefono.Text = "";
                textCorreo.Text = "";
                clearhuella();
                textCalle.Text = "";
                textEstado.Text = "";
                textMunicipio.Text = "";
                textColonia.Text = "";
                textCP.Text = "";
                pictureBox1.Image = null;
                _dato1 = null;

                buttonborrar.Enabled = false;
                //comboBoxRole2.SelectedIndex= ;
                //  comboBoxSucursal2.SelectedIndex = ;
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "limpiarr";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void ToolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buscar(sender, e);

            }

        }


        private void actualizarrol(object sender, EventArgs e)
        {

            getDatosAdicionales();
        }

        private void actualizarsuc(object sender, EventArgs e)
        {
            getDatosAdicionales2();
        }

        //private void actualizarsucfil(object sender, EventArgs e)
        //{
        //    comboBoxSucursal2.Items.Clear();
        //    getDatosAdicionales3();
        //}

        //private void actualizarrolfil(object sender, EventArgs e)
        //{
        //    comboBoxRole2.Items.Clear();
        //    getDatosAdicionales4();
        //}

        Form form;
        private bool CheckOpened(string name)
        {

            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    form = frm;
                    return true;
                }
            }
            return false;
        }
        private void botonhuella(object sender, EventArgs e)
        {
            try
            {
                if (CheckOpened("Registration"))
                {

                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                }
                else
                {
                    form = new Registration("Usuarios");
                    AddOwnedForm(form);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "botonhuella";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        public void asignarhuella(Byte[] dato, Bitmap imgg)
        {
            try
            {

                ImageConverter converter = new ImageConverter();
                byte[] ima = (byte[])converter.ConvertTo(imgg, typeof(byte[]));
                huella1 = imgg;
                huella164 = Convert.ToBase64String(ima);
                huell = dato;
                faltahuella = true;
                byte[] huel = Convert.FromBase64String(huella164);
                MemoryStream ms = new MemoryStream(huel);
                Bitmap bmp = new Bitmap(ms);
                huelladig.Image = bmp;
                if (form.InvokeRequired)
                {
                    form.Invoke(new Action(() =>
                    {
                        form.Close();
                    }
                    ));
                }
                else
                {
                    form.Close();
                }

                if (MODIFICAR == true)
                {
                    if (progressBar1.InvokeRequired)
                    {
                        progressBar1.Invoke(new Action(() =>
                        {
                            progressBar1.Value = 50;
                            progressBar1.Visible = true;

                        }
                        ));
                    }
                    else
                    {
                        progressBar1.Value = 50;
                        progressBar1.Visible = true;
                    }
                    //actualizar contrato
                    //pkchof = (string)dataGridViewUsuarios.Rows[fila].Cells["pkname"].Value;
                    string sql = "update USUARIOS set HUELLA=@HUELLA,HUELLA1=@HUELLA1,USUARIO_M=@US where PK=@pk";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@US", LoginInfo.UserID);
                    db.command.Parameters.AddWithValue("@pk", txtPk.Text);
                    db.command.Parameters.AddWithValue("@HUELLA", huell);
                    db.command.Parameters.AddWithValue("@HUELLA1", huella164);
                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("actualizO huella " + "PKUSUARIO=" + txtPk.Text + "PKUSU=" + LoginInfo.UserID);
                        Form mensaje = new Mensaje("Se actualizo huella", true);




                    }
                    else
                    {
                        if (!Utilerias.Utilerias.CheckForInternetConnection())
                        {
                            Form mensaje1 = new Mensaje("No se pudo actualizar la huella, verifique conexion a internet", true);

                            mensaje1.ShowDialog();
                        }
                        else
                        {
                            Form mensaje = new Mensaje("Mo se pudo actualizar la huella", true);

                            mensaje.ShowDialog();
                        }
                    }
                    if (progressBar1.InvokeRequired)
                    {
                        progressBar1.Invoke(new Action(() =>
                        {
                            progressBar1.Value = 0;
                            progressBar1.Visible = false;

                        }
                        ));
                    }
                    else
                    {
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;
                    }
                }


            }
            catch (Exception err)
            {
                if (progressBar1.InvokeRequired)
                {
                    progressBar1.Invoke(new Action(() =>
                    {
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;

                    }
                    ));
                }
                else
                {
                    progressBar1.Value = 0;
                    progressBar1.Visible = false;
                }
                string error = err.Message;
                string funcion = "asignarhuella";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
        }


        //public void asignarhuella(Byte[] dato, Bitmap imgg)
        //{

        //    try
        //    {

        //        ImageConverter converter = new ImageConverter();
        //        byte[] ima = (byte[])converter.ConvertTo(imgg, typeof(byte[]));

        //        huella1 = imgg;


        //        huella164 = Convert.ToBase64String(ima);


        //        huell = dato;

        //        faltahuella = true;
        //        byte[] huel = Convert.FromBase64String(huella164);
        //        MemoryStream ms = new MemoryStream(huel);
        //        Bitmap bmp = new Bitmap(ms);
        //        huelladig.Image = bmp;
        //        if (form.InvokeRequired)
        //        {
        //            form.Invoke(new Action(() =>
        //            {
        //                form.Close();
        //            }
        //            ));
        //        }
        //        else
        //        {
        //            form.Close();

        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        string error = err.Message;
        //        MessageBox.Show("Ocurrio un Error, intente de nuevo.");
        //        string funcion = "asignarhuella";
        //        Utilerias.LOG.write(_clase, funcion, error);


        //    }

        //}


        public void asignarhuellaimag()
        {
            try
            {
                MemoryStream ms0 = new MemoryStream((byte[])huell);
                Bitmap bmp0 = new Bitmap(ms0);

                huelladig.Image = bmp0;

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "asignarhuellaimagen";
                Utilerias.LOG.write(_clase, funcion, error);


            }


        }

        private void Usuarios_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            // this.Dock = DockStyle.Fill;

        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            comboBoxcantidad.Text = "10";
            getDatosAdicionales();
            getDatosAdicionales2();
        }
        private void consultar()
        {
            try
            {
                string sql = "SELECT COUNT (PK) AS TOTAL from USUARIOS where borrado=   0";
                db.PreparedSQL(sql);

                res = db.getTable();


                if (res.Next())
                {
                    total = res.GetInt("TOTAL");

                }
                textBoxtotal.Text = total.ToString();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "clear";
            }

        }
        private void Usuarios_Shown(object sender, EventArgs e)
        {
            comboBoxRole.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSucursal.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxcantidad.DropDownStyle = ComboBoxStyle.DropDownList;

            buttonborrar.BackColor = Color.White;
            buttonguardar.BackColor = Color.White;

            buttonagregar.BackColor = Color.FromArgb(38, 45, 56);
            agregar.Visible = false;
            guardar.Visible = false;
            dataGridViewUsuarios.EnableHeadersVisualStyles = false;
            db = new database();
            buttonguardar.Enabled = false;
            panelContenedorForm.Visible = false;
            comboBoxcantidad.Text = "10";
            cantidad = 10;
            consultar();

            buttonborrar.Enabled = false;
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
            timer2.Start();
            dataGridViewUsuarios.ClearSelection();
            progressBar1.Visible = false;
            permisos();
            progressBar2.Visible = false;
            DoubleBufferedd(dataGridViewUsuarios, true);
            getRows();

        }
        private void DoubleBufferedd(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            errorborrar();
            // permisos();
            backgroundWorker1.RunWorkerAsync();

            timer1.Stop();


        }

        private void Agregar_click(object sender, EventArgs e)
        {

        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            panelContenedorForm.Visible = false;

        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {

            limpiarr(sender, e);
            panelContenedorForm.Visible = false;
            clearForm();
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            try
            {


                if (string.IsNullOrEmpty(_dato1))
                {

                    _dato1 = Convert.ToBase64String((byte[])dataGridViewUsuarios.Rows[fila].Cells["imagenName"].Value);
                }

                if (ValidarInputsinsert(1))
                {
                    if (progressBar1.InvokeRequired)
                    {

                        progressBar1.Invoke(new Action(() =>
                        {
                            progressBar1.Visible = true;
                            progressBar1.Increment(20);
                        }));
                    }
                    else
                    {

                        progressBar1.Visible = true;
                        progressBar1.Increment(20);
                    }


                    string sql = "UPDATE USUARIOS SET USUARIO_M=@USUARIOM,USUARIO=@USUARIO,PASSWORD=@PASSWORD,NOMBRE=@NOMBRE,APELLIDOS=@APELLIDOS,TELEFONO=@TELEFONO,CORREO=@CORREO," +
                            "SUCURSAL=@SUCURSAL,ROL=@ROL,CALLE=@CALLE,ESTADO=@ESTADO,MUNICIPIO=@MUNICIPIO,COLONIA=@COLONIA,CP=@CP " +

                            " WHERE PK=@PK AND BORRADO=0";


                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@USUARIOM", LoginInfo.PkUsuario);
                    db.command.Parameters.AddWithValue("@USUARIO", _usuario);
                    db.command.Parameters.AddWithValue("@PASSWORD", _password);
                    db.command.Parameters.AddWithValue("@NOMBRE", _nombre);
                    db.command.Parameters.AddWithValue("@APELLIDOS", _apellidos);
                    //db.command.Parameters.AddWithValue("@IMAGEN", fotobyte);
                    db.command.Parameters.AddWithValue("@TELEFONO", _telefono);
                    db.command.Parameters.AddWithValue("@CORREO", _correo);
                    // db.command.Parameters.AddWithValue("@ACTIVO", _activo);
                    db.command.Parameters.AddWithValue("@SUCURSAL", _sucursal);
                    db.command.Parameters.AddWithValue("@ROL", _rol);
                    //db.command.Parameters.AddWithValue("@REGISTRO", _registro);
                    db.command.Parameters.AddWithValue("@CALLE", _calle);
                    db.command.Parameters.AddWithValue("@ESTADO", _estado);
                    db.command.Parameters.AddWithValue("@MUNICIPIO", _municipio);
                    db.command.Parameters.AddWithValue("@COLONIA", _colonia);
                    db.command.Parameters.AddWithValue("@CP", _cp);
                    db.command.Parameters.AddWithValue("@PK", txtPk.Text);
                    progressBar1.Increment(30);



                    if (db.execute())
                    {

                        Utilerias.LOG.acciones("modifico el usuario " + _user);
                        panelContenedorForm.Visible = false;
                        progressBar1.Increment(100);

                        botonactualizar(sender, e);
                        comboBoxRole.SelectedItem = null;
                        comboBoxSucursal.SelectedItem = null;

                        clearhuella();
                        txtUsuario.Text = "";
                        txtPassword.Text = "";
                        txtNombre.Text = "";
                        txtApellidos.Text = "";
                        textTelefono.Text = "";
                        textCorreo.Text = "";
                        txtid.Text = "";
                        dataGridViewUsuarios.Visible = true;
                        textCalle.Text = "";
                        textEstado.Text = "";
                        textMunicipio.Text = "";
                        textColonia.Text = "";
                        textCP.Text = "";
                        pictureBox1.Image = null;
                        huelladig.Image = null;
                        _dato1 = null;
                        comboBoxRole.SelectedItem = null;
                        comboBoxSucursal.SelectedItem = null;
                        _rol = null;
                        _sucursal = null;
                        txtUsuario.Focus();
                        progressBar1.Visible = false;
                        progressBar1.Value = 0;
                        txtPk.Text = "";


                        buttonborrar.Enabled = false;

                    }

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

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }



        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            //  Buttonbuscar(sender, e);
            //  Buttonbuscar(sender, e);
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void Agregar_Click_1(object sender, EventArgs e)
        {
            try
            {

                progressBar1.Visible = true;
                progressBar1.Increment(30);
                if (ValidarInputsinsert(0))
                {
                    string sql1 = "SELECT count(USUARIO) MAX FROM USUARIOS WHERE USUARIO='" + _usuario + "'";
                    if (db.Count(sql1) > 0)
                    {
                        Form mensaje = new Mensaje("El usuario ya existe", true);

                        DialogResult resut = mensaje.ShowDialog();
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;
                        return;
                    }

                    _activo = 1;

                    string sql = "INSERT INTO USUARIOS(USUARIO,PASSWORD,NOMBRE,APELLIDOS,TELEFONO,CORREO,ACTIVO,SUCURSAL,ROL,CALLE,ESTADO,MUNICIPIO,COLONIA,CP,USUARIO_M)" +
                                             " VALUES(@USUARIO,@PASSWORD,@NOMBRE,@APELLIDOS,@TELEFONO,@CORREO,@ACTIVO,@SUCURSAL,@ROL,@CALLE,@ESTADO,@MUNICIPIO,@COLONIA,@CP,@USUARIOM)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@USUARIO", _usuario);
                    db.command.Parameters.AddWithValue("@PASSWORD", _password);
                    db.command.Parameters.AddWithValue("@NOMBRE", _nombre);
                    db.command.Parameters.AddWithValue("@APELLIDOS", _apellidos);
                    //  db.command.Parameters.AddWithValue("@IMAGEN", fotobyte);


                    db.command.Parameters.AddWithValue("@TELEFONO", _telefono);
                    db.command.Parameters.AddWithValue("@CORREO", _correo);
                    db.command.Parameters.AddWithValue("@ACTIVO", _activo);
                    db.command.Parameters.AddWithValue("@SUCURSAL", _sucursal);
                    db.command.Parameters.AddWithValue("@ROL", _rol);
                    progressBar1.Increment(30);
                    //db.command.Parameters.AddWithValue("@REGISTRO", _registro);
                    db.command.Parameters.AddWithValue("@CALLE", _calle);
                    db.command.Parameters.AddWithValue("@ESTADO", _estado);
                    db.command.Parameters.AddWithValue("@MUNICIPIO", _municipio);
                    db.command.Parameters.AddWithValue("@COLONIA", _colonia);
                    db.command.Parameters.AddWithValue("@CP", _cp);
                    //db.command.Parameters.AddWithValue("@IMAG", _dato1);
                    //db.command.Parameters.AddWithValue("@HUELLA", huell);

                    //db.command.Parameters.AddWithValue("@HUELLA1", huella164);
                    db.command.Parameters.AddWithValue("@USUARIOM", LoginInfo.PkUsuario);
                    progressBar1.Increment(30);

                    //dataGridViewUsuarios.Rows[n].Cells[19].Value = Convert.FromBase64String(res.Get("HUELLA1"));
                    if (db.execute())
                    {
                        progressBar1.Increment(70);


                        panelContenedorForm.Visible = false;
                        txtUsuario.Text = "";
                        txtPassword.Text = "";
                        txtNombre.Text = "";
                        txtApellidos.Text = "";
                        textTelefono.Text = "";
                        textCorreo.Text = "";
                        botonactualizar(sender, e);
                        textCalle.Text = "";
                        textEstado.Text = "";
                        textMunicipio.Text = "";
                        textColonia.Text = "";
                        textCP.Text = "";
                        progressBar1.Increment(100);
                        txtid.Text = "";
                        comboBoxRole.SelectedItem = null;
                        comboBoxSucursal.SelectedItem = null;
                        pictureBox1.Image = null;
                        huelladig.Image = null;
                        progressBar1.Visible = false;
                        progressBar1.Value = 0;
                        _dato1 = null;
                        txtUsuario.Focus();
                        txtPk.Text = "";
                        dataGridViewUsuarios.Visible = true;
                    }
                }


            }
            catch (Exception err)
            {
                string error = err.Message;
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
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

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GroupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void Txtid_TextChanged(object sender, EventArgs e)
        {

        }

        private void Vista_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Visible = true;
                progressBar1.Increment(30);
                SaveFileDialog fichero = new SaveFileDialog();
                fichero.Filter = "PDF (*.pdf)|*.pdf";
                if (fichero.ShowDialog() == DialogResult.OK)
                {
                    progressBar1.Increment(50);

                    string documento = fichero.FileName;
                    Document doc = new Document();
                    PdfWriter.GetInstance(doc, new FileStream(documento, FileMode.Create));
                    doc.Open();

                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Autobuses.Properties.Resources.llavess, System.Drawing.Imaging.ImageFormat.Png);
                    logo.Alignment = Element.ALIGN_LEFT;
                    logo.ScaleToFit(150f, 100f);
                    doc.Add(new Paragraph("\n"));



                    PdfPTable table = new PdfPTable(3);
                    table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.WidthPercentage = 100;

                    PdfPCell cell = new PdfPCell();

                    cell = new PdfPCell(logo);
                    cell.BorderColor = BaseColor.WHITE;
                    table.AddCell(cell);
                    Paragraph p2 = new Paragraph();

                    p2.Add(new Chunk("\n"));

                    cell = new PdfPCell(p2);
                    cell.BorderColor = BaseColor.WHITE;
                    table.AddCell(cell);
                    Paragraph d = new Paragraph();
                    d.Add(new Chunk("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy")));
                    d.Add(new Chunk("\n"));
                    d.Add(new Chunk("Hora: " + DateTime.Now.ToString("HH: mm tt")));



                    cell = new PdfPCell(d);
                    cell.BorderColor = BaseColor.WHITE;
                    table.AddCell(cell);

                    doc.Add(table);
                    doc.Add(new Paragraph("\n"));

                    Paragraph title = new Paragraph();
                    title.Font = FontFactory.GetFont(FontFactory.TIMES, 36f, BaseColor.BLACK);
                    title.Alignment = Element.ALIGN_CENTER;
                    title.Add("Expediente de usuario");
                    doc.Add(title);
                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph("\n"));
                    PdfPTable table2 = new PdfPTable(2);
                    table2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table2.WidthPercentage = 100;

                    PdfPCell cell2
                        = new PdfPCell();

                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Paragraph p = new Paragraph();


                    progressBar1.Increment(80);

                    string usu = txtUsuario.Text;
                    string nombre = txtNombre.Text + " " + txtApellidos.Text;
                    string telefono = textTelefono.Text;
                    string correo = textCorreo.Text;
                    string rol = comboBoxRole.Text;
                    string sucursal = comboBoxSucursal.Text;


                    string calle = textCalle.Text;

                    string estado = textEstado.Text;
                    string municipio = textMunicipio.Text;
                    string colonia = textColonia.Text;
                    p.Add(new Chunk("\n"));
                    ///p.Add(TextRefsAggregater.PlainifyRichText(discussion.Background));
                    p.Add(("Usuario: " + usu));
                    p.Add(new Chunk("\n"));
                    p.Add(new Chunk("\n"));
                    p.Add(("Nombre: " + nombre));
                    p.Add(new Chunk("\n"));
                    p.Add(new Chunk("\n"));
                    p.Add(("Calle: " + calle));
                    p.Add(new Chunk("\n"));
                    p.Add(new Chunk("\n"));
                    p.Add(("Estado: " + estado));
                    p.Add(new Chunk("\n"));
                    p.Add(new Chunk("\n"));
                    p.Add(("Municipio: " + municipio));
                    p.Add(new Chunk("\n"));
                    p.Add(new Chunk("\n"));
                    p.Add(new Paragraph("Colonia: " + colonia));
                    p.Add(new Chunk("\n"));
                    p.Add(new Chunk("\n"));
                    p.Add(("Telefono: " + telefono));
                    p.Add(new Chunk("\n"));
                    p.Add(new Chunk("\n"));
                    p.Add(("Correo: " + correo));
                    p.Add(new Chunk("\n"));
                    p.Add(new Chunk("\n"));
                    p.Add(new Paragraph("Rol: " + rol));
                    p.Add(new Chunk("\n"));
                    p.Add(new Chunk("\n"));
                    p.Add(("Sucursal: " + sucursal));
                    p.Add(new Chunk("\n"));
                    p.Add(new Chunk("\n"));
                    p.Alignment = Element.ALIGN_JUSTIFIED_ALL;
                    p.Alignment = Element.ALIGN_LEFT;
                    cell2 = new PdfPCell(p);
                    cell2.BorderColor = BaseColor.WHITE;
                    table2.AddCell(cell2);
                    byte[] buff = ms.GetBuffer();
                    iTextSharp.text.Image PatientSign = iTextSharp.text.Image.GetInstance(buff);
                    PatientSign.ScaleToFit(200f, 250f);

                    PatientSign.Alignment = Element.ALIGN_RIGHT;
                    cell2 = new PdfPCell(PatientSign);
                    cell2.BorderColor = BaseColor.WHITE;
                    table2.AddCell(cell2);

                    doc.Add(table2);

                    doc.Close();
                    System.Diagnostics.Process.Start(fichero.FileName);
                }

                progressBar1.Increment(100);
                progressBar1.Visible = false;
                progressBar1.Value = 0;
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
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

        private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(panelContenedorForm.Handle, 0x112, 0xf012, 0); ReleaseCapture();
        }

        private void dataGridViewUsuarios_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            getPhotoPdf((string)dataGridViewUsuarios.Rows[fila].Cells["PkName"].Value, fila, (string)dataGridViewUsuarios.Rows[fila].Cells["PkName"].Value);
        }


        private void siguiente_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.Rows.Count == cantidad)
            {

                inicio += cantidad;
                final += cantidad;
                getRows();
            }

        }

        private void atras_Click(object sender, EventArgs e)
        {
            if ((inicio - cantidad) >= 0)
            {
                inicio -= cantidad;
                final -= cantidad;
                getRows();
            }

        }

        private void comboBoxcantidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            cantidad = int.Parse(comboBoxcantidad.Text);
            inicio = 0;
            final = cantidad;
            getRows();
        }
    }

}