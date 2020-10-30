using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ConnectDB;
using DPFP;
using DPFP.Capture;
using System.Linq;
using System.Drawing.Imaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Reflection;

namespace Autobuses.Administracion
{

    public partial class Socios : Form
    {




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
        private int inicio = 0;
        private int final = 10;
        private int cantidad = 10;
        private int total = 0;


        private string _search;
        private string _searchtool;
        private bool MODIFICAR = false;
        private string _dato1;
        private string _user;
        private Byte[] huell;
        private Byte[] fotobyte;
        private Bitmap huella1;
        private Bitmap huella2;
        private Bitmap huella3;
        private Bitmap huella4;
        private string huella164;
        private string huella264;
        private string huella364;
        private string huella464;
        private string _clase = "socios";

        private int huella = 0;
        private Boolean faltahuella = false;


        public Socios()
        {
            InitializeComponent();
            this.Show();
            titulo.Text = "Socios";
        }

        private void permisos()
        {
            buttonagregar.Visible = false;
            buttonborrar.Visible = false;
            buttonguardar.Visible = false;
            if (LoginInfo.privilegios.Any(x => x == "agregar socios"))
            {
                buttonagregar.Visible = true;
            }
            if (LoginInfo.privilegios.Any(x => x == "borrar socios"))
            {
                buttonborrar.Visible = true;
            }
            if (LoginInfo.privilegios.Any(x => x == "modificar socios"))
            {
                buttonguardar.Visible = true;
            }
        }

        public void getRows(string search = "")
        {
            try
            {


                buttonborrar.Enabled = false;
                buttonguardar.Enabled = false;
                buttonagregar.Enabled = true;
                dataGridViewUsuarios.Rows.Clear();
                int count = 1;
                string sql = "SELECT PK,USUARIO,NOMBRE,PASSWORD,APELLIDOS,TELEFONO,CORREO," +
                    "ACTIVO,FECHA_C,CALLE,ESTADO,MUNICIPIO,COLONIA,CP FROM SOCIOS WHERE BORRADO=0 ORDER BY APELLIDOS OFFSET @INICIO ROWS FETCH NEXT @CANTIDAD ROWS ONLY ";
                if (!string.IsNullOrEmpty(search))
                {
                   string sql2 = "SELECT PK,USUARIO,NOMBRE,PASSWORD,APELLIDOS,TELEFONO,CORREO,ACTIVO,FECHA_C,CALLE,ESTADO,MUNICIPIO,COLONIA,CP " +
                        "FROM SOCIOS WHERE BORRADO=0  AND (USUARIO LIKE @SEARCH OR NOMBRE LIKE @SEARCH OR APELLIDOS LIKE @SEARCH) ORDER BY APELLIDOS ";
                    db.PreparedSQL(sql2);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + search + "%");
                }
                else
                {
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + search + "%");
                    db.command.Parameters.AddWithValue("@CANTIDAD",cantidad);

                    db.command.Parameters.AddWithValue("@INICIO", inicio);


                }
                res = db.getTable();
                while (res.Next())
                {
                    n = dataGridViewUsuarios.Rows.Add();
                    dataGridViewUsuarios.Rows[n].Cells[0].Value = count;
                    dataGridViewUsuarios.Rows[n].Cells["PkName"].Value = res.Get("PK");
                    dataGridViewUsuarios.Rows[n].Cells["UsuarioName"].Value = res.Get("USUARIO");
                    dataGridViewUsuarios.Rows[n].Cells["NombreName"].Value = res.Get("NOMBRE");
                    dataGridViewUsuarios.Rows[n].Cells["PasswordName"].Value = res.Get("PASSWORD");
                    dataGridViewUsuarios.Rows[n].Cells["ApellidosName"].Value = res.Get("APELLIDOS");
                    dataGridViewUsuarios.Rows[n].Cells["TelefonoName"].Value = res.Get("TELEFONO");
                    dataGridViewUsuarios.Rows[n].Cells["CorreoName"].Value = res.Get("CORREO");
                    dataGridViewUsuarios.Rows[n].Cells["ActivoName"].Value = res.Get("ACTIVO");
                    _registro = res.Get("FECHA_C");
                    dataGridViewUsuarios.Rows[n].Cells["RegistroName"].Value = _registro;
                    dataGridViewUsuarios.Rows[n].Cells["CalleName"].Value = res.Get("CALLE");
                    dataGridViewUsuarios.Rows[n].Cells["EstadoName"].Value = res.Get("ESTADO");
                    dataGridViewUsuarios.Rows[n].Cells["MunicipioName"].Value = res.Get("MUNICIPIO");
                    dataGridViewUsuarios.Rows[n].Cells["ColoniaName"].Value = res.Get("COLONIA");
                    dataGridViewUsuarios.Rows[n].Cells["CPName"].Value = res.Get("CP");
                    count++;
                }
                if (dataGridViewUsuarios.CurrentRow != null && dataGridViewUsuarios.CurrentRow != null)
                    dataGridViewUsuarios.CurrentRow.Selected = false;
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getRows";
                Utilerias.LOG.write(_clase, funcion, error);
            }
        }

        private void selecciondatagrid(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
               columna = e.ColumnIndex;
                fila = e.RowIndex;
                if (fila != -1 && columna != 7)
                {
                    agregar.Visible = false;
                    guardar.Visible = true;
                    buttonborrar.Enabled = true;
                    buttonagregar.Enabled = false;
                    buttonguardar.Enabled = true;
                    buttonborrar.BackColor = Color.FromArgb(38, 45, 56);
                    buttonguardar.BackColor = Color.FromArgb(38, 45, 56);

                    buttonagregar.BackColor = Color.White;

                }
            }
            catch (Exception err)
            {
                string error = err.ToString();
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "selecciondatagrid";
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
                if (btnCrearPdf.InvokeRequired)
                {

                    btnCrearPdf.Invoke(new Action(() =>
                    {

                        btnCrearPdf.Enabled = false;
                    }));
                }
                else
                {

                    btnCancelar.Enabled = false;

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
                    progressBar1.Increment(40);
                }

               string pksociooo= (string)dataGridViewUsuarios.Rows[fila].Cells["PkName"].Value;
                string sql = "SELECT IMAG,HUELLA1 FROM SOCIOS WHERE PK=@PK ";
                db.PreparedSQL(sql);
                 db.command.Parameters.AddWithValue("@PK", pksociooo);
                res = db.getTable();
                if (res.Next())
                {
                    dataGridViewUsuarios.Rows[pos].Cells["ImagenName"].Value = (!string.IsNullOrEmpty(res.Get("IMAG"))) ? Convert.FromBase64String(res.Get("IMAG")) : null;
                    dataGridViewUsuarios.Rows[pos].Cells["ImgHuellaName"].Value = (!string.IsNullOrEmpty(res.Get("HUELLA1"))) ? Convert.FromBase64String(res.Get("HUELLA1")) : null;
                }
                if (progressBar1.InvokeRequired)
                {

                    progressBar1.Invoke(new Action(() =>
                    {
                        progressBar1.Increment(40);
                    }));
                }
                else
                {

               
                    progressBar1.Increment(40);
                }
                MemoryStream ms = new MemoryStream((byte[])dataGridViewUsuarios.Rows[fila].Cells["ImagenName"].Value);
                Bitmap bmp = new Bitmap(ms);
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                MemoryStream ms2 = new MemoryStream((byte[])dataGridViewUsuarios.Rows[fila].Cells["ImgHuellaName"].Value);
                Bitmap bmp2 = new Bitmap(ms2);
                huelladig.Image = bmp2;
                if (progressBar1.InvokeRequired)
                {

                    progressBar1.Invoke(new Action(() =>
                    {

                        progressBar1.Visible = false;
                        progressBar1.Value = 60;
                    }));
                }
                else
                {


                    progressBar1.Visible = false;
                    progressBar1.Value = 50;
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
                if (btnCrearPdf.InvokeRequired)
                {

                    btnCrearPdf.Invoke(new Action(() =>
                    {

                        btnCrearPdf.Enabled = true;
                    }));
                }
                else
                {

                    btnCrearPdf.Enabled = true;

                }

            }
            catch (Exception e)
            {
                string error = e.ToString();
                string funcion = "getPhotoPdf";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error al obtener foto y pdf, intente de nuevo.");
            }
        }

        public void limpiarboton()
        {
            try
            {
                dataGridViewUsuarios.Rows.Clear();
                dataGridViewUsuarios.Refresh();
                getRows();
                buttonguardar.Enabled = false;
                buttonagregar.Enabled = true;
                buttonborrar.Enabled = true;
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "limpiar";
                Utilerias.LOG.write(_clase, funcion, error);
            }
        }

        private void limpiarboton(object sender, EventArgs e)
        {
            limpiarboton();
        }

        private void errorborrar()
        {
            LabelErrorUsuario.Visible = false;
            LabelErrorPassword.Visible = false;
            LabelErrorNombre.Visible = false;
            LabelErrorApellidos.Visible = false;
            LabelErrorCalle.Visible = false;
            LabelErrorCP.Visible = false;
            LabelErrorEstado.Visible = false;
            LabelErrorMunicipio.Visible = false;
            LabelErrorColonia.Visible = false;
            LabelErrorHuella.Visible = false;
            LabelErrorFoto.Visible = false;
            LabelErrorTelenofo.Visible = false;
            LabelErrorCorreo.Visible = false;
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

                _calle = textCalle.Text;
                _cp = textCP.Text;
                _estado = textEstado.Text;
                _municipio = textMunicipio.Text;
                _colonia = textColonia.Text;

                _telefono = textTelefono.Text;
                _correo = textCorreo.Text;

                if (option == 1 && string.IsNullOrEmpty(_dato1))
                {

                    _dato1 = Convert.ToBase64String((byte[])dataGridViewUsuarios.Rows[fila].Cells["imagenname"].Value);

                }


                if (option == 0 && faltahuella == false)
                {
                    LabelErrorHuella.Visible = true;
                    valido = false;
                }

                if (string.IsNullOrEmpty(_usuario))
                {
                    LabelErrorUsuario.Visible = true;
                    valido = false;
                }
                if (string.IsNullOrEmpty(_password))
                {
                    LabelErrorPassword.Visible = true;
                    valido = false;
                }
                if (string.IsNullOrEmpty(_nombre))
                {
                    LabelErrorNombre.Visible = true;
                    valido = false;
                }
                if (string.IsNullOrEmpty(_apellidos))
                {
                    LabelErrorApellidos.Visible = true;
                    valido = false;
                }
                if (string.IsNullOrEmpty(_calle))
                {
                    LabelErrorCalle.Visible = true;
                    valido = false;
                }
                if (string.IsNullOrEmpty(_cp))
                {
                    LabelErrorCP.Visible = true;
                    valido = false;
                }
                if (string.IsNullOrEmpty(_estado))
                {
                    LabelErrorEstado.Visible = true;
                    valido = false;
                }
                if (string.IsNullOrEmpty(_colonia))
                {
                    LabelErrorColonia.Visible = true;
                    valido = false;
                }
                if (string.IsNullOrEmpty(_municipio))
                {
                    LabelErrorMunicipio.Visible = true;
                    valido = false;
                }
                if (string.IsNullOrEmpty(_telefono))
                {
                    LabelErrorTelenofo.Visible = true;
                    valido = false;
                }
                if (string.IsNullOrEmpty(_correo))
                {
                    LabelErrorCorreo.Visible = true;
                    valido = false;
                }
                if (string.IsNullOrEmpty(_telefono))
                {
                    LabelErrorTelenofo.Visible = true;
                    valido = false;
                }
                if (string.IsNullOrEmpty(_dato1))
                {
                    LabelErrorFoto.Visible = true;
                    valido = false;
                }
            }
            catch (Exception err)
            {
                string error = err.ToString();
                string funcion = "ValidarInputsinsert";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
            return valido;
        }

        private void insertar(object sender, EventArgs e)
        {
            MODIFICAR = false;

            dataGridViewUsuarios.Visible = false;
            btnCrearPdf.Visible = false;
            panelContenedorForm.Visible = true;
            guardar.Visible = false;
            agregar.Visible = true;
            tabControlsocio.SelectedTab = tabPagesocio;

            panelalterno.Visible = false;
            txtUsuario.Focus();

        }

        private void eliminar(object sender, EventArgs e)
        {
            try
            {

                Form mensaje = new Mensaje("¿Está seguro de eliminar el registro?", false);
                    DialogResult resut = mensaje.ShowDialog();
                if (resut == DialogResult.OK)
                {

                    string pk = (string)dataGridViewUsuarios.Rows[fila].Cells["PkName"].Value;
                    string sql;
                    sql = "UPDATE SOCIOS SET BORRADO=1, USUARIO_M=@USER WHERE PK = @PK";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", pk);
                    db.command.Parameters.AddWithValue("@USER", LoginInfo.PkUsuario);

                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("borro un socio " + _user);
                        clear();
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

        private void onKeyEnterSearch(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dataGridViewUsuarios.Rows.Clear();
                    dataGridViewUsuarios.Refresh();
                    getRows(textBoxbuscar.Text);
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                string funcion = "onKeyEnterSearch";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
        }

        private void foto(object sender, EventArgs e)
        {
            try
            {
                if (CheckOpened("Tomar Foto"))
                {


                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                    progressBar1.Visible = false;
                    progressBar1.Value = 0;
                }
                else {
                    string dato = "Socios";
                    Form form = new TomarFoto(dato);
                    AddOwnedForm(form);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                    progressBar1.Visible = false;
                    progressBar1.Value = 0;
                }
            }

            catch (Exception err)
            {
                string error = err.Message;
                string funcion = "foto";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
        }
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
        Form form;
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
                    form = new Registration("socios");
                    AddOwnedForm(form);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                string funcion = "botonhuella";
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
                    string sql = "update SOCIOS set HUELLA=@HUELLA,HUELLA1=@HUELLA1,USUARIO_M=@US where PK=@pk";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@US", LoginInfo.UserID);
                    db.command.Parameters.AddWithValue("@pk", txtPk.Text);
                    db.command.Parameters.AddWithValue("@HUELLA", huell);
                    db.command.Parameters.AddWithValue("@HUELLA1", huella164);
                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("actualizO huella " + "PKSOCIO=" + txtPk.Text + LoginInfo.UserID);
                        Form mensaje = new Mensaje("Se actualizo huella", true);

                        DialogResult resut = mensaje.ShowDialog();

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
                        progressBar1.Invoke(new Action(() => {
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
                    string sql = "update SOCIOS set IMAG=@IMAG,USUARIO_M=@US where PK=@pk";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@US", LoginInfo.UserID);
                    db.command.Parameters.AddWithValue("@pk", txtPk.Text) ;
                    db.command.Parameters.AddWithValue("@IMAG", _dato1);
                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("actualizo contrato " + "pksocio=" + txtPk.Text + LoginInfo.UserID);


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
                        progressBar1.Invoke(new Action(() => {
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
                    progressBar1.Invoke(new Action(() => {
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
        public void asignarhuellaimag()
        {
            try
            {
                MemoryStream ms = new MemoryStream((byte[])huell);
                Bitmap bmp = new Bitmap(ms);
                huelladig.Image = bmp;
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "asignarhuellaimag";
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

        private void update(object sender, EventArgs e)
        {

            try
            {

                tabControlsocio.SelectedTab = tabPagesocio;
                panelalterno.Visible = true;
                int n = fila;
                int c = columna;
                if ( n != -1 && c != 7)
                {
                    MODIFICAR = true;

                    buttonguardar.Enabled = false;
                    buttonguardar.BackColor = Color.White;
                    dataGridViewUsuarios.Visible = false;
                    txtPk.Text = (string)dataGridViewUsuarios.Rows[n].Cells["PkName"].Value;

                    //obtengo foto y pdf para mostrarlo
                     backgroundWorker1.RunWorkerAsync(); 

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
                    textColonia.Text = (string)dataGridViewUsuarios.Rows[n].Cells["ColoniaName"].Value;
                    textCP.Text = (string)dataGridViewUsuarios.Rows[n].Cells["CPName"].Value;



                }
                btnCrearPdf.Visible = true;
                progressBar2.Value = 0;
                progressBar2.Visible = false;
                panelContenedorForm.Visible = true;
                txtUsuario.Focus();


            }
            catch (Exception err)
            {
                string error = err.ToString();
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "selecciondatagrid";
                Utilerias.LOG.write(_clase, funcion, error);
            }


           
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            dataGridViewUsuarios.Rows.Clear();
            dataGridViewUsuarios.Refresh();
            getRows(Search.Text);

        }

        private void Check(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                n = e.RowIndex;
                int c = e.ColumnIndex;
                if (n > -1 && c == 7)
                {
                    if (LoginInfo.privilegios.Any(x => x == "modificar socios"))
                    {
                        buttonguardar.Visible = true;

                        string pk = (string)dataGridViewUsuarios.Rows[n].Cells["PkName"].Value;
                        Boolean.TryParse(dataGridViewUsuarios.Rows[n].Cells["ActivoName"].Value.ToString(), out bool act);
                        act = !act;
                        string sql = "UPDATE SOCIOS SET ACTIVO=@ACTIVO,USUARIO_M=@USUARIOM WHERE PK=@PK ";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PK", pk);
                        db.command.Parameters.AddWithValue("@USUARIOM", LoginInfo.PkUsuario);
                        db.command.Parameters.AddWithValue("@ACTIVO", act);

                        if (db.execute())
                        {
                            if (!act)
                            {
                                Form mensaje = new Mensaje("Socio desactivado", true);
                                DialogResult resut = mensaje.ShowDialog();
                            }
                            else
                            {
                                Form mensaje = new Mensaje("Socio activado", true);
                                DialogResult resut = mensaje.ShowDialog();
                            }
                        }
                        else
                        {
                            Form mensaje = new Mensaje("No se puede desactivar", true);
                            DialogResult resut = mensaje.ShowDialog();
                        }
                        getRows(textBoxbuscar.Text);
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
            try
            {
                huelladig.Image = null;
                pictureBoxalternohuella.Image = null;

                faltahuella = false;
                huella1 = null;
                huell = null;
            }
            catch (Exception err)
            {
                string error = err.Message;
                string funcion = "clearhuella";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
        }

        public void clear()
        {
            try
            {

                buttonborrar.BackColor = Color.White;
                buttonguardar.BackColor = Color.White;
                textBoxnombrealterno.Text = "";
                textBoxcorreoalterno.Text = "";
                textBoxtelefonoalterno.Text = "";
                pictureBoxsocioalterno.Image = null;

                buttonagregar.BackColor = Color.FromArgb(38, 45, 56);
                dataGridViewUsuarios.Rows.Clear();
                dataGridViewUsuarios.Refresh();
                getRows();
                textBoxbuscar.Text = "";
                errorborrar();
                txtPk.Text = "";
                txtUsuario.Text = "";
                txtPassword.Text = "";
                txtNombre.Text = "";
                txtApellidos.Text = "";
                textTelefono.Text = "";
                textCorreo.Text = "";
                textBoxbuscar.Text = "";
                textCalle.Text = "";
                textEstado.Text = "";
                textMunicipio.Text = "";
                textColonia.Text = "";
                textCP.Text = "";
                pictureBox1.Image = null;
                _dato1 = null;
                buttonguardar.Enabled = false;
                buttonborrar.Enabled = false;
                buttonagregar.Enabled = true;
                clearhuella();
                dataGridViewUsuarios.Visible = true;
                txtUsuario.Focus();
                panelContenedorForm.Visible = false;
                dataGridViewUsuarios.Visible = true;
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "clear";
                Utilerias.LOG.write(_clase, funcion, error);
            }
        }

        private void clear(object sender, EventArgs e)
        {
            clear();
        }

        private void Socios_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        private void consultar()
        {
            try
            {
                string sql = "SELECT COUNT (PK) AS TOTAL from SOCIOS where ACTIVO=1 and borrado=0";
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
        private void Socios_Shown(object sender, EventArgs e)
        {
            dataGridViewUsuarios.EnableHeadersVisualStyles = false;
            progressBar2.Visible = false;
            db = new database();


            comboBoxcantidad.DropDownStyle = ComboBoxStyle.DropDownList;
            buttonborrar.BackColor = Color.White;
            buttonguardar.BackColor = Color.White;

            buttonagregar.BackColor = Color.FromArgb(38, 45, 56);

            buttonguardar.Enabled = false;
            buttonborrar.Enabled = false;
            consultar();
            comboBoxcantidad.Text = "10";
            cantidad = 10;
            getRows();
            errorborrar();

            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            permisos();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            panelContenedorForm.Visible = false;
            timer1.Start();
            DoubleBufferedd(dataGridViewUsuarios, true);

        }
        private void DoubleBufferedd(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        private void ToolStripExportExcel_Click(object sender, EventArgs e)
        {
            progressBar2.Visible = true;
            progressBar2.Increment(20);
            progressBar2.Increment(40);
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridViewUsuarios, new List<string> {"PasswordName","ImagenName","ImgHuellaName","PkName"});
            progressBar2.Increment(100);
            progressBar2.Visible = false;
            progressBar2.Value = 0;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            string sql = "";
            try
            {
                progressBar1.Visible = true;
                progressBar1.Increment(20);
                if (ValidarInputsinsert(1))
                {

                    string sql1 = "SELECT COUNT(PK) MAX FROM SOCIOS WHERE USUARIO='" + _usuario + "' AND NOT PK="+ txtPk.Text;

                    if (db.Count(sql1) > 0)
                    {
                        MessageBox.Show("Usuario ya existe");
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;
                        return;
                    }

                    sql = "UPDATE SOCIOS SET USUARIO_M=@USUARIOM,USUARIO=@USUARIO,PASSWORD=@PASSWORD,NOMBRE=@NOMBRE,APELLIDOS=@APELLIDOS,TELEFONO=@TELEFONO,CORREO=@CORREO," +
                            "CALLE=@CALLE,ESTADO=@ESTADO,MUNICIPIO=@MUNICIPIO,COLONIA=@COLONIA,CP=@CP" +
    
                            " WHERE PK=@PK ";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@USUARIOM", LoginInfo.PkUsuario);
                    db.command.Parameters.AddWithValue("@USUARIO", _usuario);
                    db.command.Parameters.AddWithValue("@PASSWORD", _password);
                    db.command.Parameters.AddWithValue("@NOMBRE", _nombre);
                    db.command.Parameters.AddWithValue("@APELLIDOS", _apellidos);
                    db.command.Parameters.AddWithValue("@TELEFONO", _telefono);
                    db.command.Parameters.AddWithValue("@CORREO", _correo);
                    progressBar1.Increment(40);
                    db.command.Parameters.AddWithValue("@CALLE", _calle);
                    db.command.Parameters.AddWithValue("@ESTADO", _estado);
                    db.command.Parameters.AddWithValue("@MUNICIPIO", _municipio);
                    db.command.Parameters.AddWithValue("@COLONIA", _colonia);
                    db.command.Parameters.AddWithValue("@CP", _cp);
                    db.command.Parameters.AddWithValue("@PK", txtPk.Text);

            
                    progressBar1.Increment(10);
                    if (db.execute())
                    {

                        buttonborrar.BackColor = Color.White;
                        buttonguardar.BackColor = Color.White;

                        buttonagregar.BackColor = Color.FromArgb(38, 45, 56);
                        progressBar1.Increment(10);
                        Utilerias.LOG.acciones("modifico un socio " + _user);
                        panelContenedorForm.Visible = false;
                        dataGridViewUsuarios.Visible = true;
                        limpiarboton();
                        clear();
                        progressBar1.Visible = false;
                        progressBar1.Value = 0;
                    }
                  
                }
                else {
              
                    progressBar1.Value = 0;
                    progressBar1.Visible = false;

             
                }


            }
            catch (Exception err)
            {
                string error = err.ToString();
                string funcion = "update";
                Utilerias.LOG.write(_clase, funcion, error + sql);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
        }

        private void Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Visible = true;
                progressBar1.Increment(40);

                if (ValidarInputsinsert(0))
                {
                    _activo = 1;
                    string sql1 = "SELECT COUNT(PK) MAX FROM SOCIOS WHERE USUARIO='" + _usuario + "'";

                    if (db.Count(sql1) > 0)
                    {
                        MessageBox.Show("Usuario ya existe");
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;
                        return;
                    }

                    string sql = "INSERT INTO SOCIOS(USUARIO,PASSWORD,NOMBRE,APELLIDOS,TELEFONO,CORREO,ACTIVO,CALLE,ESTADO,MUNICIPIO,COLONIA,CP,IMAG,HUELLA,HUELLA1,USUARIO_M)" +
                                             " VALUES(@USUARIO,@PASSWORD,@NOMBRE,@APELLIDOS,@TELEFONO,@CORREO,@ACTIVO,@CALLE,@ESTADO,@MUNICIPIO,@COLONIA,@CP,@IMAG,@HUELLA,@HUELLA1,@USUARIOM)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@USUARIO", _usuario);
                    db.command.Parameters.AddWithValue("@PASSWORD", _password);
                    db.command.Parameters.AddWithValue("@NOMBRE", _nombre);
                    db.command.Parameters.AddWithValue("@APELLIDOS", _apellidos);
                    db.command.Parameters.AddWithValue("@TELEFONO", _telefono);
                    db.command.Parameters.AddWithValue("@CORREO", _correo);
                    db.command.Parameters.AddWithValue("@ACTIVO", _activo);
                    progressBar1.Increment(20);
                    db.command.Parameters.AddWithValue("@CALLE", _calle);
                    db.command.Parameters.AddWithValue("@ESTADO", _estado);
                    db.command.Parameters.AddWithValue("@MUNICIPIO", _municipio);
                    db.command.Parameters.AddWithValue("@COLONIA", _colonia);   
                    db.command.Parameters.AddWithValue("@CP", _cp);
                    db.command.Parameters.AddWithValue("@IMAG", _dato1);
                    db.command.Parameters.AddWithValue("@HUELLA", huell);
                    db.command.Parameters.AddWithValue("@HUELLA1", huella164);
                    db.command.Parameters.AddWithValue("@USUARIOM", LoginInfo.PkUsuario);
                    progressBar1.Increment(20);

                    if (db.execute())
                    {
                        progressBar1.Increment(100);
                        panelContenedorForm.Visible = false;

                        Utilerias.LOG.acciones("agrego un socio " + _usuario);
                        limpiarboton();
                        clear();
                        progressBar1.Visible = false;
                        progressBar1.Value = 0;
                    }
                }
                else
                {
                  
                    progressBar1.Value = 0;
                    progressBar1.Visible = false;
                }
            }
            catch (Exception err)
            {
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                string error = err.ToString();
                string funcion = "insertar";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {

            panelContenedorForm.Visible = false;
            clear();
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            panelContenedorForm.Visible = false;
            clear();
        }

        private void Button1_Click(object sender, EventArgs e)
        {


            dataGridViewUsuarios.Refresh();
            dataGridViewUsuarios.Rows.Clear();
            getRows(textBoxbuscar.Text);
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void Button8_Click(object sender, EventArgs e)
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
                    title.Add("Expediente de Socio");
                    doc.Add(title);
                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph("\n"));
                    PdfPTable table2 = new PdfPTable(2);
                    table2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table2.WidthPercentage = 100;

                    PdfPCell cell2 = new PdfPCell();

                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Paragraph p = new Paragraph();

                    progressBar1.Increment(80);

                    string usu = txtUsuario.Text;
                    string nombre = txtNombre.Text + " " + txtApellidos.Text;
                    string telefono = textTelefono.Text;
                    string correo = textCorreo.Text;
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
                string error = err.ToString();
                string funcion = "Button8_Click";
                Utilerias.LOG.write(_clase, funcion, error);
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

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            getPhotoPdf((string)dataGridViewUsuarios.Rows[fila].Cells["UsuarioName"].Value, fila, (string)dataGridViewUsuarios.Rows[fila].Cells["PkName"].Value);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonalterno_Click(object sender, EventArgs e)
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
                    progressBar1.Visible = true;
                    progressBar1.Value = 50;
                    form = new Registration("Socioalterno");
                    AddOwnedForm(form);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "buttonalternohuella";
                Utilerias.LOG.write(_clase, funcion, error);
            }

        }

        public void asignarhuellaalterno(Byte[] dato, Bitmap imgg)
        {
            try
            {
                ImageConverter converter = new ImageConverter();
                byte[] ima = (byte[])converter.ConvertTo(imgg, typeof(byte[]));

                Bitmap huella1 = imgg;


                string huella164 = Convert.ToBase64String(ima);


                Byte[] huell = dato;

                byte[] huel = Convert.FromBase64String(huella164);
                MemoryStream ms = new MemoryStream(huel);
                Bitmap bmp = new Bitmap(ms);
                pictureBoxalternohuella.Image = bmp;
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
              

                string sql = "update SOCIOS set HUELLAALTERNO=@HUELLA,HUELLA1ALTERNO=@HUELLA1,USUARIO_M=@US where PK=@pk";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@US", LoginInfo.PkUsuario);
                    db.command.Parameters.AddWithValue("@pk", txtPk.Text);
                    db.command.Parameters.AddWithValue("@HUELLA", huell);
                    db.command.Parameters.AddWithValue("@HUELLA1", huella164);
                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("actualizO huella " + "pkchofer=" + txtPk.Text + LoginInfo.UserID);
                        Form mensaje = new Mensaje("Se actualizo huella alterna", true);

                        DialogResult resut = mensaje.ShowDialog();

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
                    progressBar1.Invoke(new Action(() => {
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
            catch (Exception err)
            {
                if (progressBar1.InvokeRequired)
                {
                    progressBar1.Invoke(new Action(() => {
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
                string funcion = "asignarhuellaalterno";
                Utilerias.LOG.write(_clase, funcion, error);
            }
        }

        private void buttonfotoalterno_Click(object sender, EventArgs e)
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
                    progressBar1.Visible = true;
                    progressBar1.Value = 50;
                    string dato = "socioalterno";
                    Form form = new TomarFoto(dato);
                    AddOwnedForm(form);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                }

            }
            catch (Exception err)
            {
                if (progressBar1.InvokeRequired)
                {
                    progressBar1.Invoke(new Action(() => {
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
                string funcion = "foto";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

            public void AsignarFotoalterno(Bitmap img)
            {

                try
                {
                    pictureBoxsocioalterno.Image = img;

                    ImageConverter converter = new ImageConverter();
                    byte[] ima2 = (byte[])converter.ConvertTo(img, typeof(byte[]));

                    pictureBoxsocioalterno.SizeMode = PictureBoxSizeMode.StretchImage;

                    _dato1 = Convert.ToBase64String(ima2);
                    
                        if (progressBar1.InvokeRequired)
                        {
                            progressBar1.Invoke(new Action(() => {
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
                        string sql = "update socios set IMAGEALTERNO=@IMAG,USUARIO_M=@US where PK=@pk";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@US", LoginInfo.PkUsuario);
                        db.command.Parameters.AddWithValue("@pk", txtPk.Text);
                        db.command.Parameters.AddWithValue("@IMAG", _dato1);
                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("actualizo contrato " + "pksocio=" + txtPk.Text + LoginInfo.PkUsuario);
              

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
                            progressBar1.Invoke(new Action(() => {
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


                if (progressBar1.InvokeRequired)
                {
                    progressBar1.Invoke(new Action(() => {
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

            catch (Exception err)
                {
                    if (progressBar1.InvokeRequired)
                    {
                        progressBar1.Invoke(new Action(() => {
                            progressBar1.Visible = false;
                            progressBar1.Value = 0;

                        }
                        ));
                    }
                    else
                    {
                    progressBar1.Visible = false;
                    progressBar1.Value = 0;
                }
                    string error = err.Message;
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                    string funcion = "asignarfoto";
                    Utilerias.LOG.write(_clase, funcion, error);


                }




            
        }

        private void guardaralterno_Click(object sender, EventArgs e)
        {
                 string sql = "UPDATE SOCIOS SET USUARIO_M=@USUARIOM,ALTERNONOMBRE=@ALTERNONOMBRE,TELEFONOALTERNO=@TELEFONOALTERNO,CORREOALTERNO=@CORREOALTERNO " +
                            "WHERE PK=@PK ";

                    db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@PK", txtPk.Text);

            db.command.Parameters.AddWithValue("@USUARIOM", LoginInfo.PkUsuario);
                    db.command.Parameters.AddWithValue("@ALTERNONOMBRE", textBoxnombrealterno.Text);
                    db.command.Parameters.AddWithValue("@TELEFONOALTERNO", textBoxtelefonoalterno.Text);
                    db.command.Parameters.AddWithValue("@CORREOALTERNO", textBoxcorreoalterno.Text);
                   
                    progressBar1.Increment(10);
            if (db.execute())
            {
                Form mensaje1 = new Mensaje("Datos del socio alterno modificado", true);

                mensaje1.ShowDialog();
                Utilerias.LOG.acciones("actualizO datos " + "socio=" + txtPk.Text + LoginInfo.PkUsuario);

            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {

        }

        private void tabControlsocio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                TabControl seleccion = (TabControl)sender;
                switch (seleccion.SelectedIndex)
                {
                    case 1:
                        getdatosalterno();
                        break;
            


                }

                if (progressBar1.InvokeRequired)
                {
                    progressBar1.Invoke(new Action(() => {
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
            catch (Exception err)
            {

                if (progressBar1.InvokeRequired)
                {
                    progressBar1.Invoke(new Action(() => {
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

                Utilerias.LOG.write(_clase, "timerdocumentos", err.ToString());

            }
        }
        private void getdatosalterno()
        {
            textBoxnombrealterno.Focus();
            string sql = "SELECT CORREOALTERNO,HUELLAALTERNO,HUELLA1ALTERNO,ALTERNONOMBRE, TELEFONOALTERNO,IMAGEALTERNO FROM SOCIOS WHERE BORRADO=0 AND PK=@PK";
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@PK", txtPk.Text);


           ResultSet res = db.getTable();
            if (res.Next())
            {
                textBoxcorreoalterno.Text= res.Get("CORREOALTERNO");
                textBoxtelefonoalterno.Text = res.Get("TELEFONOALTERNO");
                textBoxnombrealterno.Text = res.Get("ALTERNONOMBRE");



                byte[] imagenalterno = (!string.IsNullOrEmpty(res.Get("IMAGEALTERNO"))) ? Convert.FromBase64String(res.Get("IMAGEALTERNO")) : null;
                byte[] huellaimagen = (!string.IsNullOrEmpty(res.Get("HUELLA1ALTERNO"))) ? Convert.FromBase64String(res.Get("HUELLA1ALTERNO")) : null;

                if (imagenalterno != null)
                {
                    MemoryStream ms = new MemoryStream(imagenalterno);
                    Bitmap bmp = new Bitmap(ms);
                    pictureBoxsocioalterno.Image = bmp;
                    pictureBoxsocioalterno.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                if (huellaimagen != null)
                {
                    MemoryStream ms2 = new MemoryStream(huellaimagen);
                    Bitmap bmp2 = new Bitmap(ms2);
                    pictureBoxalternohuella.Image = bmp2;
                }

        }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {

                Form mensaje = new Mensaje("¿Está seguro de eliminar al socio alterno?", false);
                DialogResult resut = mensaje.ShowDialog();
                if (resut == DialogResult.OK)
                {

                    string pk = (string)dataGridViewUsuarios.Rows[fila].Cells["PkName"].Value;
                    string sql;
                    sql = "UPDATE SOCIOS SET  CORREOALTERNO=null,HUELLAALTERNO=null,HUELLA1ALTERNO=null,ALTERNONOMBRE=null, TELEFONOALTERNO=null,IMAGEALTERNO=null, USUARIO_M=@USER WHERE PK = @PK";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", pk);
                    db.command.Parameters.AddWithValue("@USER", LoginInfo.PkUsuario);

                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("borro socio alterno de  " + _user+"pk="+pk+"el usuario="+LoginInfo.PkUsuario);
                        clear();
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

        private void textBoxtotal_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
