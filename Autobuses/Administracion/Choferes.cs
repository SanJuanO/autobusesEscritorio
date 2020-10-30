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
using System.Configuration;
using System.Drawing.Imaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Paragraph = iTextSharp.text.Paragraph;
using System.Reflection;

namespace Autobuses.Administracion
{
    public partial class Choferes : Form
    {
        public database db;
            ResultSet res = null;
            Bitmap image;
       private int progress = 0;
        private int n = 0;
            private string _usuario;
            private string _password;
            private string _nombre;
            private string _apellidos;
            private string _telefono;
            private string _correo;
            private int _activo;
            private bool MODIFICAR=false;
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
        private int fila;
        private string _linea="";
        private int columna;
        private int inicio = 0;
        private int final = 10;
        private int cantidad = 10;
        private int total = 0;

        private bool excelval = false;
        private string _clase = "choferes";
            private string _searchtool;
            private bool _validar = false;
            private string _dato1;
        private string pkchof;
        private DataGridView export=null;
        private string licenc;
        private string examen;
        private string fechaexamen;
        private string fechalicencia;
        private string _user;

        private string _pdf;
        private byte[] decbuff;

        private Byte _foto;
        private Byte[] fotobyte;
        private Byte[] huell;
        private Bitmap huella1;

        private string huella164;

        private int huella = 0;
        private Boolean faltahuella = false;
        private string fechacontrato;
        private string contrato;
        private TabControl seleccion;
        public Choferes()
            {
                InitializeComponent();
            this.Show();
            titulo.Text = "Conductores";

        }

        private void permisos()
        {
            buttonagregar.Visible = false;
            buttonborrar.Visible = false;
            buttonguardar.Visible = false;
            if (LoginInfo.privilegios.Any(x => x == "agregar conductores"))
            {
                buttonagregar.Visible = true;

            }
            if (LoginInfo.privilegios.Any(x => x == "borrar conductores"))
            {
                buttonborrar.Visible = true;

            }
            if (LoginInfo.privilegios.Any(x => x == "modificar  conductores"))
            {
                buttonguardar.Visible = true;

            }
        }


        public void getRows(string search = "")
            {
            try
            {
                dataGridViewUsuarios.Rows.Clear();
                limiando();
                int count = 1;
                string sql = "SELECT PK,USUARIO,NOMBRE,PASSWORD,APELLIDOS,TELEFONO,CORREO,ACTIVO,FECHA_C," +
                    "CALLE,ESTADO,MUNICIPIO,COLONIA,CP,CADUCIDADLICENCIA,CADUCIDADEXAMEN,CADUCIDADCONTRATO,LINEA FROM CHOFERES where BORRADO=0 " +
                    " ORDER BY APELLIDOS OFFSET @INICIO ROWS FETCH NEXT @CANTIDAD ROWS ONLY ";
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
                            dataGridViewUsuarios.Rows[n].Cells["Noname"].Value = count;
                            dataGridViewUsuarios.Rows[n].Cells["pkname"].Value = res.Get("PK");

                            dataGridViewUsuarios.Rows[n].Cells["Usuarioname"].Value = res.Get("USUARIO");
                            dataGridViewUsuarios.Rows[n].Cells["Passwordname"].Value = res.Get("PASSWORD");

                           
                            dataGridViewUsuarios.Rows[n].Cells["nombrename"].Value = res.Get("NOMBRE");
                            dataGridViewUsuarios.Rows[n].Cells["Apellidosname"].Value = res.Get("APELLIDOS");
                            dataGridViewUsuarios.Rows[n].Cells["telefononame"].Value = res.Get("TELEFONO");
                            dataGridViewUsuarios.Rows[n].Cells["Correoname"].Value = res.Get("CORREO");
                            dataGridViewUsuarios.Rows[n].Cells["activoname"].Value = res.Get("ACTIVO");

                            _registro = res.Get("FECHA_C");
                            dataGridViewUsuarios.Rows[n].Cells["RegistroName"].Value = _registro;
                            dataGridViewUsuarios.Rows[n].Cells["Callename"].Value = res.Get("CALLE");
                            dataGridViewUsuarios.Rows[n].Cells["Estadoname"].Value = res.Get("ESTADO");
                            dataGridViewUsuarios.Rows[n].Cells["municipioname"].Value = res.Get("MUNICIPIO");
                            dataGridViewUsuarios.Rows[n].Cells["colonianame"].Value = res.Get("COLONIA");
                            dataGridViewUsuarios.Rows[n].Cells["cpname"].Value = res.Get("CP");

                            dataGridViewUsuarios.Rows[n].Cells["fechalicencianame"].Value = res.Get("CADUCIDADLICENCIA");
                            dataGridViewUsuarios.Rows[n].Cells["fechaexamenname"].Value = res.Get("CADUCIDADEXAMEN");
                            dataGridViewUsuarios.Rows[n].Cells["fechacontratoname"].Value = res.Get("CADUCIDADCONTRATO");
                            dataGridViewUsuarios.Rows[n].Cells["lineaname"].Value = res.Get("LINEA");

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
                        dataGridViewUsuarios.Rows[n].Cells["Noname"].Value = count;
                        dataGridViewUsuarios.Rows[n].Cells["pkname"].Value = res.Get("PK");

                        dataGridViewUsuarios.Rows[n].Cells["Usuarioname"].Value = res.Get("USUARIO");
                        dataGridViewUsuarios.Rows[n].Cells["Passwordname"].Value = res.Get("PASSWORD");


                        dataGridViewUsuarios.Rows[n].Cells["nombrename"].Value = res.Get("NOMBRE");
                        dataGridViewUsuarios.Rows[n].Cells["Apellidosname"].Value = res.Get("APELLIDOS");
                        dataGridViewUsuarios.Rows[n].Cells["telefononame"].Value = res.Get("TELEFONO");
                        dataGridViewUsuarios.Rows[n].Cells["Correoname"].Value = res.Get("CORREO");
                        dataGridViewUsuarios.Rows[n].Cells["activoname"].Value = res.Get("ACTIVO");

                        _registro = res.Get("FECHA_C");
                        dataGridViewUsuarios.Rows[n].Cells["RegistroName"].Value = _registro;
                        dataGridViewUsuarios.Rows[n].Cells["Callename"].Value = res.Get("CALLE");
                        dataGridViewUsuarios.Rows[n].Cells["Estadoname"].Value = res.Get("ESTADO");
                        dataGridViewUsuarios.Rows[n].Cells["municipioname"].Value = res.Get("MUNICIPIO");
                        dataGridViewUsuarios.Rows[n].Cells["colonianame"].Value = res.Get("COLONIA");
                        dataGridViewUsuarios.Rows[n].Cells["cpname"].Value = res.Get("CP");

                        dataGridViewUsuarios.Rows[n].Cells["fechalicencianame"].Value = res.Get("CADUCIDADLICENCIA");
                        dataGridViewUsuarios.Rows[n].Cells["fechaexamenname"].Value = res.Get("CADUCIDADEXAMEN");
                        dataGridViewUsuarios.Rows[n].Cells["fechacontratoname"].Value = res.Get("CADUCIDADCONTRATO");
                        dataGridViewUsuarios.Rows[n].Cells["lineaname"].Value = res.Get("LINEA");

                        count++;
                    }
                }
                dataGridViewUsuarios.CurrentRow.Selected = false;

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getrows";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


            fila = e.RowIndex;
            columna = e.ColumnIndex;
            if (columna != 7 && fila != -1)
            {

                buttonborrar.BackColor = Color.FromArgb(38, 45, 56);
                buttonguardar.BackColor = Color.FromArgb(38, 45, 56);

                buttonagregar.BackColor = Color.White;

                buttonborrar.Enabled = true;
                buttonguardar.Enabled = true;
                buttonagregar.Enabled = false;
            }

        }
        public void getPhotoPdf(string usermandado, int pos)
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
                if (buttonpdf.InvokeRequired)
                {

                    buttonpdf.Invoke(new Action(() =>
                    {

                        buttonpdf.Enabled = false;
                    }));
                }
                else
                {

                    buttonpdf.Enabled = false;

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


                int n = fila;
                string sql = "SELECT IMAG,HUELLA1 FROM choferes ";
                /*
                Byte[] array = Encoding.ASCII.GetBytes("aqui va el campo de la base");
                MemoryStream ms = new MemoryStream(array);
                Bitmap bmp = new Bitmap(ms);
                */
                sql += "WHERE USUARIO=@USUARIO ";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@USUARIO", usermandado);
                if (progressBar1.InvokeRequired)
                {

                    progressBar1.Invoke(new Action(() =>
                    {

                        progressBar1.Increment(10);
                    }));
                }
                else
                {


                    progressBar1.Increment(10);
                }
                ResultSet res = db.getTable();
                if (progressBar1.InvokeRequired)
                {

                    progressBar1.Invoke(new Action(() =>
                    {

                        progressBar1.Increment(20);
                    }));
                }
                else
                {


                    progressBar1.Increment(20);
                }
                if (res.Next())
                {


                    dataGridViewUsuarios.Rows[pos].Cells["imagenname"].Value = (!string.IsNullOrEmpty(res.Get("IMAG"))) ? Convert.FromBase64String(res.Get("IMAG")) : null;
                    dataGridViewUsuarios.Rows[pos].Cells["huella1name"].Value = (!string.IsNullOrEmpty(res.Get("HUELLA1"))) ? Convert.FromBase64String(res.Get("HUELLA1")) : null;
       
                    //dataGridViewUsuarios.Rows[pos].Cells["licencianame"].Value = res.Get("LICENCIA");
                    //dataGridViewUsuarios.Rows[pos].Cells["examenname"].Value = res.Get("EXAMEN");
                    //dataGridViewUsuarios.Rows[pos].Cells["contratoname"].Value = res.Get("CONTRATO");

                }
                if (dataGridViewUsuarios.Rows[pos].Cells["imagenname"].Value != null)
                {

                    pictureBox1.Image = (dataGridViewUsuarios.Rows[n].Cells["imagenname"].Value != null) ? Image.Bytes_A_Imagen((byte[])dataGridViewUsuarios.Rows[n].Cells["imagenname"].Value) : null;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                if (dataGridViewUsuarios.Rows[pos].Cells["huella1name"].Value != null)
                {

                    huelladig.Image = (dataGridViewUsuarios.Rows[n].Cells["huella1name"].Value != null) ? Image.Bytes_A_Imagen((byte[])dataGridViewUsuarios.Rows[n].Cells["huella1name"].Value) : null;
                    huelladig.SizeMode = PictureBoxSizeMode.StretchImage;
                }

                

                fechaexamen = dateTimePicker1.Value.ToString("yyy-MM-dd");
                fechalicencia = dateTimePicker2.Value.ToString("yyy-MM-dd");

               
                fechacontrato = dateTimePicker3.Value.ToString("yyy-MM-dd");



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

                        guardar.Enabled = true;
                    }));
                }
                else
                {

                    guardar.Enabled = true;

                }
                if (buttonpdf.InvokeRequired)
                {

                    buttonpdf.Invoke(new Action(() =>
                    {

                        buttonpdf.Enabled = true;
                    }));
                }
                else
                {

                    buttonpdf.Enabled = true;

                }
             
            }
            catch (Exception e)
            {
                string error = e.Message;
                if (progressBar1.InvokeRequired)
                {

                    progressBar1.Invoke(new Action(() =>
                    {

                        progressBar1.Visible = false;
                        progressBar1.Value = 0;
                    }));
                }
                else
                {


                    progressBar1.Visible = false;
                    progressBar1.Value = 0;
                }
            }

        }
        public void openPDF()
        {

            string archivoPDF = Path.GetTempFileName() + "HelpFile.pdf";
            System.IO.FileStream stream = new FileStream(archivoPDF, FileMode.CreateNew);
            System.IO.BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Convert.FromBase64String(_pdf), 0, Convert.FromBase64String(_pdf).Length);
            writer.Close();
            if (axAcroPDF1.InvokeRequired)
            {

                axAcroPDF1.Invoke(new Action(() =>
                {
                    axAcroPDF1.LoadFile(archivoPDF);
                    axAcroPDF1.Visible = true;
                }));
            }
            else
            {

                axAcroPDF1.LoadFile(archivoPDF);
                axAcroPDF1.Visible = true;
            }

         


            //axAcroPDF1.setShowToolbar(false);
            //axAcroPDF1.setPageMode();
        }

        public void openPDF2()
        {

            string archivoPDF = Path.GetTempFileName() + "HelpFile.pdf";
            System.IO.FileStream stream = new FileStream(archivoPDF, FileMode.CreateNew);
            System.IO.BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Convert.FromBase64String(_pdf), 0, Convert.FromBase64String(_pdf).Length);
            writer.Close();
           

            if (axAcroPDF3.InvokeRequired)
            {

                axAcroPDF3.Invoke(new Action(() =>
                {
                    axAcroPDF3.LoadFile(archivoPDF);
                    axAcroPDF3.Visible = true;
                }));
            }
            else
            {

                axAcroPDF3.LoadFile(archivoPDF);
                axAcroPDF3.Visible = true;
            }
            //axAcroPDF1.setShowToolbar(false);
            //axAcroPDF1.setPageMode();
        }
        public void openPDF3()
        {

            string archivoPDF = Path.GetTempFileName() + "HelpFile.pdf";
            System.IO.FileStream stream = new FileStream(archivoPDF, FileMode.CreateNew);
            System.IO.BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Convert.FromBase64String(_pdf), 0, Convert.FromBase64String(_pdf).Length);
            writer.Close();

            if (axAcroPDF4.InvokeRequired)
            {

                axAcroPDF4.Invoke(new Action(() =>
                {
                    axAcroPDF4.LoadFile(archivoPDF);
                    axAcroPDF4.Visible = true;
                }));
            }
            else
            {

                axAcroPDF4.LoadFile(archivoPDF);
                axAcroPDF4.Visible = true;
            }

            //axAcroPDF1.setShowToolbar(false);
            //axAcroPDF1.setPageMode();
        }
        private void Limpiardatagrid(object sender, EventArgs e)
            {
            try
            {
                dataGridViewUsuarios.Rows.Clear();
                dataGridViewUsuarios.Refresh();
                getRows();
                buttonguardar.Enabled = false;
                buttonagregar.Enabled = true;
            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "limpiardatagrid";
                Utilerias.LOG.write(_clase, funcion, error);


            }



        }
        private void errorborrar()
            {
            labelhuella.Visible = false;
                label15.Visible = false;
                label17.Visible = false;
                label18.Visible = false;
                label19.Visible = false;
                label20.Visible = false;
                label21.Visible = false;
                label22.Visible = false;
                label23.Visible = false;
            labelfechaexamen.Visible = false;
            labelfechalicencia.Visible = false;
            labelexamen.Visible = false;
            labellicencia.Visible = false;
                label26.Visible = false;
                label27.Visible = false;
                label28.Visible = false;
                label29.Visible = false;
            labelcontrato.Visible = false;
            labelfechacont.Visible = false;

            labellinea.Visible = false;

        }

        private Boolean ValidarInputsinsert()
        {
            errorborrar();
            Boolean valido = true;
            try
            {
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



             
                if (string.IsNullOrEmpty(_linea))
                {
                    validando();

                    label29.Visible = true;
                    valido = false;

                }
              
                if (string.IsNullOrEmpty(fechacontrato))
                {
                    validando();
                    valido = false;

                }
             
                if (string.IsNullOrEmpty(fechalicencia))
                {
                    validando();
                    valido = false;

                }
                if (string.IsNullOrEmpty(fechaexamen))
                {
                    validando();
                    valido = false;

                }

                if (string.IsNullOrEmpty(_usuario))
                {
                    validando();
                    valido = false;

                }
                else if (string.IsNullOrEmpty(_password))
                {
                    validando();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_nombre))
                {
                    validando();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_apellidos))
                {
                    validando();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_calle))
                {
                    validando();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_cp))
                {
                    validando();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_estado))
                {
                    validando();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_colonia))
                {
                    validando();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_municipio))
                {
                    validando();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_telefono))
                {
                    validando();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_correo))
                {
                    validando();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_telefono))
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
                progressBar1.Visible = false;
                progressBar1.Value = 0;
             
                if (string.IsNullOrEmpty(fechaexamen))
                {

                    labelexamen.Visible = true;
                }
                if (string.IsNullOrEmpty(_linea))
                {

                    labellinea.Visible = true;
                }

                if (string.IsNullOrEmpty(fechalicencia))
                {

                    labelfechalicencia.Visible = true;
                }
             
          
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
                if (string.IsNullOrEmpty(_telefono))
                {
                    label27.Visible = true;
                }
             
                if (string.IsNullOrEmpty(fechacontrato))
                {
                    labelfechacont.Visible = true;

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


        private Boolean ValidarInputsinsert2()
        {
            errorborrar();
            Boolean valido = true;
            try
            {
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
   
                if (string.IsNullOrEmpty(_usuario))
                {
                    validando2();
                    valido = false;

                }
                if (_usuario=="")
                {
                    validando2();
                    valido = false;

                }
                else if (string.IsNullOrEmpty(_password))
                {
                    validando2();

                    valido = false;
                }
                if (_password == "")
                {
                    validando2();
                    valido = false;

                }
                else if (string.IsNullOrEmpty(_nombre))
                {
                    validando2();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_apellidos))
                {
                    validando2();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_calle))
                {
                    validando2();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_cp))
                {
                    validando2();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_estado))
                {
                    validando2();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_colonia))
                {
                    validando2();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_municipio))
                {
                    validando2();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_telefono))
                {
                    validando2();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_correo))
                {
                    validando2();

                    valido = false;
                }
                else if (string.IsNullOrEmpty(_telefono))
                {
                    validando2();

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

        private void validando2()
        {
            try
            {
                if (_usuario == "")
                {

                    label15.Visible = true;

                }
                if (_password == "")
                {
                    label17.Visible = true;

                }

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
                if (string.IsNullOrEmpty(_telefono))
                {
                    label27.Visible = true;
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

        private void clearhuella()
        {
            try
            {
                huelladig.Image = null;
                faltahuella = false;
                huella1 = null;

                huell = null;
            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "clearhuella";
                Utilerias.LOG.write(_clase, funcion, error);


            }


        }
        /*BOTON INSERTAR*/
        private void insertando(object sender, EventArgs e)
            {
            groupBoxh.Visible = false;
            buttoncontrato.Visible = false;
            buttonexamen.Visible = false;
            buttonlicencia.Visible = false;
            dataGridViewUsuarios.Visible = false;
            fechacontrato = null;
            fechaexamen = null;
            fechalicencia = null;
            examen = null;
            contrato = null;
            licenc = null;
            buttonpdf.Visible = false;
            panelContenedorForm.Visible = true;
            Informacion.SelectedTab = tabPage1;
            guardar.Visible = false;
            agregar.Visible = true;
            txtUsuario.Focus();
            _linea = "";
            getDatosAdicionaleslinea();


        }



        private void btneliminar(object sender, EventArgs e)
            {


            try
            {
                Form mensaje = new Mensaje("¿Está seguro de eliminar el registro?", false);

                DialogResult resut = mensaje.ShowDialog();
                if (resut == DialogResult.OK)
                {

                    string pk = (string)dataGridViewUsuarios.Rows[fila].Cells["pkname"].Value;
                    string sql;
                    sql = "UPDATE  CHOFERES SET BORRADO=1, USUARIO_M=@US WHERE PK = @PK ";
                    _usuario = txtUsuario.Text;
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", pk);
                    db.command.Parameters.AddWithValue("@US", LoginInfo.UserID);

                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("elimino al conductor" + _user);

                        lim(sender, e);
                        txtUsuario.Focus();
                        buttonborrar.Enabled = false;
                        buttonguardar.Enabled = false;
                        buttonagregar.Enabled = true;

                        dataGridViewUsuarios.Visible = true;
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
                    string dato = "Choferes";
                    Form form = new TomarFoto(dato);
                    AddOwnedForm(form);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                }
              
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "foto";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }


        public void AsignarFoto(Bitmap img)
            {

            try { 
                pictureBox1.Image = img;

                ImageConverter converter = new ImageConverter();
                byte[] ima=(byte[])converter.ConvertTo(img, typeof(byte[]));

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
                    string sql = "update choferes set IMAG=@IMAG,USUARIO_M=@US where PK=@pk";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@US", LoginInfo.UserID);
                    db.command.Parameters.AddWithValue("@pk", pkchof);
                    db.command.Parameters.AddWithValue("@IMAG", _dato1);
                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("actualizo contrato " + "pkchofer=" + pkchof + LoginInfo.UserID);
                    

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



            private void toolStripRefresh_Click(object sender, EventArgs e)
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
                string funcion = "toolstriprefresh";
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
                string funcion = "toolstripsesionclose";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }








            private void updatee(object sender, EventArgs e)
            {
            try
            {
                groupBoxh.Visible = true;

                int c = columna;
                int n = fila;
                if (n != -1 && c != 7 )
                {
                    MODIFICAR = true;
                    getDatosAdicionaleslinea();
                    buttoncontrato.Visible = true;
                    buttonexamen.Visible = true;
                    buttonlicencia.Visible = true;
                    panelContenedorForm.Visible = true;
                    Informacion.SelectedTab = tabPage1;
                    txtUsuario.Focus();
                    buttonguardar.Enabled = false;
                    buttonguardar.BackColor = Color.White;
                    buttonpdf.Visible = true;
                   
                    guardar.Visible = true;
                    dataGridViewUsuarios.Visible = false;
                    agregar.Visible = false;
                
                    guardar.Enabled = false;
                    buttonfoto.Enabled = false;
                    buttonHULLA.Enabled = false;
                    //obtengo foto y pdf para mostrarlo
                 
                  
                     backgroundWorker1.RunWorkerAsync(); 
                    pkchof = (string)dataGridViewUsuarios.Rows[n].Cells["pkname"].Value;

                    txtUsuario.Text = (string)dataGridViewUsuarios.Rows[n].Cells["Usuarioname"].Value;
                    txtPassword.Text = (string)dataGridViewUsuarios.Rows[n].Cells["Passwordname"].Value;
                    _user = (string)dataGridViewUsuarios.Rows[n].Cells[1].Value;
                    txtNombre.Text = (string)dataGridViewUsuarios.Rows[n].Cells["nombrename"].Value;
                    txtApellidos.Text = (string)dataGridViewUsuarios.Rows[n].Cells["Apellidosname"].Value;

                    textTelefono.Text = (string)dataGridViewUsuarios.Rows[n].Cells["telefononame"].Value;
                    textCorreo.Text = (string)dataGridViewUsuarios.Rows[n].Cells["Correoname"].Value;

                    textCalle.Text = (string)dataGridViewUsuarios.Rows[n].Cells["Callename"].Value;
                    textEstado.Text = (string)dataGridViewUsuarios.Rows[n].Cells["Estadoname"].Value;
                    textMunicipio.Text = (string)dataGridViewUsuarios.Rows[n].Cells["municipioname"].Value;
                    textColonia.Text = (string)dataGridViewUsuarios.Rows[n].Cells["colonianame"].Value;
                    textCP.Text = (string)dataGridViewUsuarios.Rows[n].Cells["cpname"].Value;

                   
                    _linea = (string)dataGridViewUsuarios.Rows[n].Cells["lineaname"].Value;
                    comboLinea.Text=_linea;
                    dateTimePicker1.Value = DateTime.Parse(dataGridViewUsuarios.Rows[n].Cells["fechaexamenname"].Value.ToString());
               
                    dateTimePicker2.Value =DateTime.Parse(dataGridViewUsuarios.Rows[n].Cells["fechalicencianame"].Value.ToString());
                    dateTimePicker3.Value = DateTime.Parse(dataGridViewUsuarios.Rows[n].Cells["fechacontratoname"].Value.ToString());
                
                    guardar.Enabled = true;
                    buttonfoto.Enabled = true;
                    buttonHULLA.Enabled = true;

                    fechaexamen = (string)dataGridViewUsuarios.Rows[n].Cells["fechaexamenname"].Value;
                    fechacontrato = (string)dataGridViewUsuarios.Rows[n].Cells["fechacontratoname"].Value;
                    fechalicencia = (string)dataGridViewUsuarios.Rows[n].Cells["fechalicencianame"].Value;

                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                progressBar2.Visible = false;
                progressBar2.Value = 0;
                dataGridViewUsuarios.Visible = true;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "dataGridView1_CellContentClick";
                Utilerias.LOG.write(_clase, funcion, error);


            }

   
        }

        private void ToolStripButton4_Click(object sender, EventArgs e)
            {
                insertando(sender, e);
            }

            private void ToolStripButton2_Click(object sender, EventArgs e)
            {
                updatee(sender, e);
            }

            private void buscador(object sender, EventArgs e)
            {
            try
            {
                buttonborrar.Enabled = false;
                buttonguardar.Enabled = false;
                buttonagregar.Enabled = true;
                dataGridViewUsuarios.Refresh();
                dataGridViewUsuarios.Rows.Clear();

                errorborrar();

                _searchtool = Convert.ToString(textBoxbuscar.Text);

                string sql = "SELECT PK,USUARIO,NOMBRE,PASSWORD,APELLIDOS,TELEFONO,CORREO,ACTIVO,FECHA_C," +
                    "CALLE,ESTADO,MUNICIPIO,COLONIA,CP,CADUCIDADLICENCIA,CADUCIDADEXAMEN,CADUCIDADCONTRATO,LINEA FROM CHOFERES WHERE BORRADO=0 ";

                dataGridViewUsuarios.Rows.Clear();

                if (!string.IsNullOrEmpty(_searchtool))
                {
                    sql += " AND (USUARIO LIKE @SEARCH OR NOMBRE LIKE @SEARCH OR APELLIDOS LIKE @SEARCH) ";
                }
                sql += " ORDER BY APELLIDOS ASC";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@SEARCH", "%" + _searchtool + "%");
                res = db.getTable();
                int count = 0;
                while (res.Next())
                {

                    n = dataGridViewUsuarios.Rows.Add();
                    dataGridViewUsuarios.Rows[n].Cells[0].Value = count;
                    dataGridViewUsuarios.Rows[n].Cells["pkname"].Value = res.Get("PK");
                    dataGridViewUsuarios.Rows[n].Cells["Usuarioname"].Value = res.Get("USUARIO");
                    dataGridViewUsuarios.Rows[n].Cells["Passwordname"].Value = res.Get("PASSWORD");
                    dataGridViewUsuarios.Rows[n].Cells["Nombrename"].Value = res.Get("NOMBRE");
                    dataGridViewUsuarios.Rows[n].Cells["Apellidosname"].Value = res.Get("APELLIDOS");
                    dataGridViewUsuarios.Rows[n].Cells["Telefononame"].Value = res.Get("TELEFONO");
                    dataGridViewUsuarios.Rows[n].Cells["Correoname"].Value = res.Get("CORREO");
                    dataGridViewUsuarios.Rows[n].Cells["Activoname"].Value = res.Get("ACTIVO");
                    _registro = res.Get("FECHA_C");
                    dataGridViewUsuarios.Rows[n].Cells["Registroname"].Value = _registro;
                    dataGridViewUsuarios.Rows[n].Cells["Callename"].Value = res.Get("CALLE");
                    dataGridViewUsuarios.Rows[n].Cells["Estadoname"].Value = res.Get("ESTADO");
                    dataGridViewUsuarios.Rows[n].Cells["Municipioname"].Value = res.Get("MUNICIPIO");
                    dataGridViewUsuarios.Rows[n].Cells["Colonianame"].Value = res.Get("COLONIA");
                    dataGridViewUsuarios.Rows[n].Cells["CPname"].Value = res.Get("CP");
                    dataGridViewUsuarios.Rows[n].Cells["fechalicencianame"].Value = res.Get("CADUCIDADLICENCIA");
                    dataGridViewUsuarios.Rows[n].Cells["fechaexamenname"].Value = res.Get("CADUCIDADEXAMEN");
                    dataGridViewUsuarios.Rows[n].Cells["fechacontratoname"].Value = res.Get("CADUCIDADCONTRATO");
                    dataGridViewUsuarios.Rows[n].Cells["lineaname"].Value = res.Get("LINEA");

                    count++;
                    dataGridViewUsuarios.CurrentRow.Selected = false;
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                string funcion = "buscador";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
        }

        private void Check(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                n = e.RowIndex;
                int c = e.ColumnIndex;
                if (n != -1 && c == 7)
                {
                    if (LoginInfo.privilegios.Any(x => x == "modificar  conductores"))
                    {
                        buttonguardar.Visible = true;


                        string pkname = (string)dataGridViewUsuarios.Rows[n].Cells["pkname"].Value;

                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dataGridViewUsuarios.Rows[n].Cells[c];
                        if (chk.Selected == true)

                        {
                            string act = (string)dataGridViewUsuarios.Rows[n].Cells[7].Value.ToString();
                            string val = act;

                            if (act == "False")
                            {
                                string sql = "UPDATE CHOFERES SET ACTIVO=@ACTIVO,USUARIO_M=@USUARIO  WHERE pk=@pk and borrado=0";

                                db.PreparedSQL(sql);
                                db.command.Parameters.AddWithValue("@pk", pkname);
                                db.command.Parameters.AddWithValue("@ACTIVO", 1);
                                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);

                                if (db.execute())
                                {
                                    Form mensaje = new Mensaje("Se activo el conductor", true);
                                    DialogResult resut = mensaje.ShowDialog();
                                    dataGridViewUsuarios.Refresh();
                                    dataGridViewUsuarios.Rows.Clear();
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
                                string sql = "UPDATE CHOFERES SET ACTIVO=@ACTIVO,USUARIO_M=@USUARIO WHERE pk=@pk and borrado=0 ";

                                db.PreparedSQL(sql);
                                db.command.Parameters.AddWithValue("@pk", pkname);
                                db.command.Parameters.AddWithValue("@ACTIVO", 0);
                                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);

                                if (db.execute())
                                {
                                    Form mensaje = new Mensaje("Se desactivo el conductor", true);
                                    DialogResult resut = mensaje.ShowDialog();
                                    dataGridViewUsuarios.Refresh();
                                    dataGridViewUsuarios.Rows.Clear();
                                    getRows();
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
                string funcion = "check";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
        }

        private void limiando()
        {
            errorborrar();
           // pictureBox2.Image = null;
            buttonguardar.Enabled = false;
            buttonagregar.Enabled = true;
            clearhuella();
            textBoxbuscar.Text = "";
            txtUsuario.Text = "";
            txtPassword.Text = "";
            txtNombre.Text = "";
            txtApellidos.Text = "";
            textTelefono.Text = "";
            textCorreo.Text = "";
            pkchof = "";
            textCalle.Text = "";
            textEstado.Text = "";
            textMunicipio.Text = "";
            textColonia.Text = "";
            textCP.Text = "";
            pictureBox1.Image = null;
            _dato1 = null;
            txtUsuario.Focus();
            axAcroPDF1.Visible = false;
            axAcroPDF4.Visible = false;
            axAcroPDF3.Visible = false;
        }

        private void lim(object sender, EventArgs e)
        {
           try
            {
                enableinfo();
                MODIFICAR = false;
                dataGridViewUsuarios.Visible = true;
                fechacontrato = null;
                fechaexamen = null;
                fechalicencia = null;
                examen = null;
                contrato = null;
                licenc = null;
                dataGridViewUsuarios.Refresh();
                dataGridViewUsuarios.Rows.Clear();
                buttonborrar.Enabled = false;
                buttonguardar.Enabled = false;
                buttonagregar.Enabled = true;

                buttonborrar.BackColor = Color.White;
                buttonguardar.BackColor = Color.White;
                n = -1;
                buttonagregar.BackColor = Color.FromArgb(38, 45, 56);
                contrato = null;
                examen = null;
                licenc = null;
                panelContenedorForm.Visible = false;
                errorborrar();
                getRows();

            }
            catch (Exception err)
            {
                string error = err.Message;
                string funcion = "limp";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
        }

        private void borrarbtn(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show("¿Esta seguro de eliminar el registro?",
                                     "Confirmar eliminar",
                                     MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    string sql;
                    sql = "UPDATE CHOFERES SET BORRADO=1 USUARIO_M=@US WHERE USUARIO = @USUARIO";
                    _usuario = txtUsuario.Text;
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@USUARIO", _usuario);
                    db.command.Parameters.AddWithValue("@US", LoginInfo.UserID);

                    if (db.execute())
                    {
                        if (n != -1)
                        {
                            dataGridViewUsuarios.Rows.RemoveAt(n);
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
                string funcion = "borrarbtn";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
        }


        private void ToolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buscador(sender, e);
            }
        }

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

        private void ButtonHULLA_Click(object sender, EventArgs e)
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
                    form = new Registration("Choferes");
                    AddOwnedForm(form);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "buttonhuella_click";
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
                    //pkchof = (string)dataGridViewUsuarios.Rows[fila].Cells["pkname"].Value;
                    string sql = "update choferes set HUELLA=@HUELLA,HUELLA1=@HUELLA1,USUARIO_M=@US where PK=@pk";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@US", LoginInfo.UserID);
                    db.command.Parameters.AddWithValue("@pk", pkchof);
                    db.command.Parameters.AddWithValue("@HUELLA", huell);
                    db.command.Parameters.AddWithValue("@HUELLA1", huella164);
                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("actualizO huella " + "pkchofer=" + pkchof + LoginInfo.UserID);
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
                            Form mensaje = new Mensaje("No se pudo actualizar la huella", true);

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
                string funcion = "asignarhuella";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
        }

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
                string funcion = "asignarhuellaimag";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
        }

        private void Buttonsubirlicencia_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string lic = openFileDialog1.FileName;
                    licenc = Convert.ToBase64String(File.ReadAllBytes(lic));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido");
            }
        }
        private void Buttonick_contrato(object sender, EventArgs e)
        {
            try
            {
                bloquearinfo();
                progressBar1.Visible = true;
                progressBar1.Value = 50;
                timercontrato.Start();
            }
            catch (Exception ex)
            {
                progressBar1.Visible = false;
                progressBar1.Value = 0;
                MessageBox.Show("El archivo seleccionado no es un tipo válido");
            }
        }

        private void Buttonick_examen(object sender, EventArgs e)
        {
            try
            {
                bloquearinfo();

                progressBar1.Visible = true;
                progressBar1.Value = 50;
                timerexamen.Start();
       
            }
            catch (Exception ex)
            {
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                MessageBox.Show("El archivo seleccionado no es un tipo válido");
            }
        }

        private void Buttonick_licencia(object sender, EventArgs e)
        {
            try
            {
                bloquearinfo();
                progressBar1.Visible = true;
                progressBar1.Value = 50;
                timerlicencia.Start();
            }
            catch (Exception ex)
            {
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                MessageBox.Show("El archivo seleccionado no es un tipo válido");
            }
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            fechaexamen= dateTimePicker1.Value.ToString("yyyy-MM-dd");
        }

        private void DateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            fechacontrato = dateTimePicker3.Value.ToString("yyyy-MM-dd");
        }

        private void DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            fechalicencia = dateTimePicker2.Value.ToString("yyyy-MM-dd");

        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void Choferes_Load(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;

        }

        private void consultar()
        {
            try
            {
                string sql = "SELECT COUNT (PK) AS TOTAL from CHOFERES where ACTIVO=1 and borrado=0";
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
        private void Choferes_Shown(object sender, EventArgs e)
        {
            progressBardocumento.Visible = false;
            comboBoxcantidad.DropDownStyle = ComboBoxStyle.DropDownList;

            dataGridViewUsuarios.EnableHeadersVisualStyles = false;
            progressBar2.Visible = false;
            db = new database();
            buttonborrar.Enabled = false;
            buttonguardar.Enabled = false;

            buttonborrar.BackColor = Color.White;
            buttonguardar.BackColor = Color.White;

            dataGridViewUsuarios.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //or even better .DisableResizing. Most time consumption enum is DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders 

            // set it to false if not needed 
            dataGridViewUsuarios.RowHeadersVisible = false;
            consultar();
            comboBoxcantidad.Text = "10";

            buttonagregar.BackColor = Color.FromArgb(38,45,56);
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            this.Focus();
            progressBar1.Visible = false;
            timer1.Interval = 1;
            timer1.Start();
            timer2.Start();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            panelContenedorForm.Visible = false;
            DoubleBufferedd(dataGridViewUsuarios, true);

        }
        private void DoubleBufferedd(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            getRows();
            permisos();
            errorborrar();
          
        }
        public DataGridViewRow CloneWithValues(DataGridViewRow row)
        {
            DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();
            for (Int32 index = 0; index < row.Cells.Count; index++)
            {
                clonedRow.Cells[index].Value = row.Cells[index].Value;
            }
            return clonedRow;
        }

   
        private void ToolStripExportExcel_Click_1(object sender, EventArgs e)
        {
            try
            {
              
                progressBar2.Visible = true;
                progressBar2.Increment(20);
               // lim(sender, e);
               
               
               
                progressBar2.Increment(40);
                Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridViewUsuarios, new List<string> { "imagenname","Passwordname", "licencianame", "contratoname", "pkname","huella1name", "sucid", "chofername","examenname" });
                progressBar2.Increment(100);


                progressBar2.Visible = false;
                progressBar2.Value = 0;


            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("No puede llamarse del mismo nombre");
                string funcion = "updatee";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void GroupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void PanelContenedorForm_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            try
            {
             


                if (string.IsNullOrEmpty(_dato1))
                {

                    _dato1 = Convert.ToBase64String((byte[])dataGridViewUsuarios.Rows[fila].Cells["imagenname"].Value);

                }

                if (ValidarInputsinsert2())
                {
                    progressBar1.Visible = true;
                    string sql1 = "SELECT count(USUARIO) MAX FROM CHOFERES WHERE USUARIO='" + _usuario + "' AND NOT PK='" + pkchof + "'";


                    if (db.Count(sql1) > 0)
                    {
                        Form mensaje = new Mensaje("El usuario ya existe", true);

                        DialogResult resut = mensaje.ShowDialog();
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;
                        return;
                    }


                    string sql = "UPDATE CHOFERES SET USUARIO_M=@US,USUARIO=@USUARIO,PASSWORD=@PASSWORD,NOMBRE=@NOMBRE," +
                        "APELLIDOS=@APELLIDOS,TELEFONO=@TELEFONO,CORREO=@CORREO," +
                            "CALLE=@CALLE,ESTADO=@ESTADO,MUNICIPIO=@MUNICIPIO,COLONIA=@COLONIA,CP=@CP,LINEA=@LINEA," +            
                            "CADUCIDADEXAMEN=@CADUCIDADEXAMEN,CADUCIDADLICENCIA=@CADUCIDADLICENCIA,CADUCIDADCONTRATO=@CADUCIDADCONTRATO " +
                            "WHERE PK=@PK ";
                    progressBar1.Increment(20);

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@US", LoginInfo.UserID);
                    db.command.Parameters.AddWithValue("@USUARIO", _usuario);
                    db.command.Parameters.AddWithValue("@PASSWORD", _password);
                    db.command.Parameters.AddWithValue("@NOMBRE", _nombre);
                    db.command.Parameters.AddWithValue("@APELLIDOS", _apellidos);
                    //db.command.Parameters.AddWithValue("@IMAGEN", fotobyte);
                    db.command.Parameters.AddWithValue("@TELEFONO", _telefono);
                    db.command.Parameters.AddWithValue("@CORREO", _correo);
                    // db.command.Parameters.AddWithValue("@ACTIVO", _activo);

                    db.command.Parameters.AddWithValue("@CALLE", _calle);
                    db.command.Parameters.AddWithValue("@ESTADO", _estado);
                    db.command.Parameters.AddWithValue("@MUNICIPIO", _municipio);
                    db.command.Parameters.AddWithValue("@COLONIA", _colonia);
                    db.command.Parameters.AddWithValue("@CP", _cp);
                    db.command.Parameters.AddWithValue("@LINEA", _linea);

                    db.command.Parameters.AddWithValue("@user", _user);
                    if (!string.IsNullOrEmpty(huella164))
                    {
                        db.command.Parameters.AddWithValue("@HUELLA1", huella164);
                    }
                    progressBar1.Increment(20);
                
                    db.command.Parameters.AddWithValue("@PK", pkchof);
                    progressBar1.Increment(20);
                   // db.command.Parameters.AddWithValue("@LICENCIA", licenc);
                    //db.command.Parameters.AddWithValue("@EXAMEN", examen);
                    db.command.Parameters.AddWithValue("@CADUCIDADEXAMEN", fechaexamen);
                    db.command.Parameters.AddWithValue("@CADUCIDADLICENCIA", fechalicencia);
                    //db.command.Parameters.AddWithValue("@CONTRATO", contrato);
                    db.command.Parameters.AddWithValue("@CADUCIDADCONTRATO", fechacontrato);

                    progressBar1.Increment(20);
                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("modifico conductor " + LoginInfo.UserID);

                        buttonborrar.BackColor = Color.White;
                        buttonguardar.BackColor = Color.White;

                        buttonagregar.BackColor = Color.FromArgb(38, 45, 56);
                        contrato = "";

                        panelContenedorForm.Visible = false;
                        dataGridViewUsuarios.Visible = true;
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
                        //pictureBox2.Image = null;
                        _dato1 = null;
                        txtUsuario.Focus();
                        buttonborrar.Enabled = false;
                        buttonguardar.Enabled = false;
                        buttonagregar.Enabled = true;
                        progressBar1.Increment(100);
                        progressBar1.Visible = false;
                        progressBar1.Value = 0;
                        dataGridViewUsuarios.Rows.Clear();
                        dataGridViewUsuarios.Refresh();
                        lim(sender, e);

                            }
                    else
                    {
                        
                        progressBar1.Visible = false;
                        progressBar1.Value = 0;
                    }
                }
                else
                {
                    progressBar1.Visible = false;
                    progressBar1.Value = 0;
                }
            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "updatee";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void Agregar_Click(object sender, EventArgs e)
        {
            try
            {

                progressBar1.Visible = true;
                progressBar1.Increment(30);
                if (ValidarInputsinsert())
                {
                    string sql1 = "SELECT count(USUARIO) MAX FROM CHOFERES WHERE USUARIO='" + _usuario + "'";
                    if (db.Count(sql1) > 0)
                    {
                        Form mensaje = new Mensaje("El usuario ya existe", true);

                        DialogResult resut = mensaje.ShowDialog();
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;
                        return;
                    }
                    progressBar1.Increment(30);
                    _activo = 1;

                    string sql = "INSERT INTO CHOFERES(USUARIO,PASSWORD,NOMBRE,APELLIDOS,TELEFONO,CORREO,ACTIVO,CALLE,ESTADO,MUNICIPIO,COLONIA,CP,CADUCIDADLICENCIA,CADUCIDADEXAMEN,CADUCIDADCONTRATO,USUARIO_M,LINEA)" +
                                             " VALUES(@USUARIO,@PASSWORD,@NOMBRE,@APELLIDOS,@TELEFONO,@CORREO,@ACTIVO,@CALLE,@ESTADO,@MUNICIPIO,@COLONIA,@CP,@CADUCIDADLICENCIA,@CADUCIDADEXAMEN,@CADUCIDADCONTRATO,@US,@LINEA)";
              
                   
                   
                  
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@US", LoginInfo.UserID);
                    //db.command.Parameters.AddWithValue("@IMAG", _dato1);
                    db.command.Parameters.AddWithValue("@USUARIO", _usuario);
                    db.command.Parameters.AddWithValue("@PASSWORD", _password);
                    db.command.Parameters.AddWithValue("@NOMBRE", _nombre);
                    db.command.Parameters.AddWithValue("@APELLIDOS", _apellidos);
                    //  db.command.Parameters.AddWithValue("@IMAGEN", fotobyte);

                    db.command.Parameters.AddWithValue("@LINEA", _linea);
                    db.command.Parameters.AddWithValue("@TELEFONO", _telefono);
                    db.command.Parameters.AddWithValue("@CORREO", _correo);
                    db.command.Parameters.AddWithValue("@ACTIVO", _activo);
                    progressBar1.Increment(30);
                    //db.command.Parameters.AddWithValue("@REGISTRO", _registro);
                    db.command.Parameters.AddWithValue("@CALLE", _calle);
                    db.command.Parameters.AddWithValue("@ESTADO", _estado);
                    db.command.Parameters.AddWithValue("@MUNICIPIO", _municipio);
                    db.command.Parameters.AddWithValue("@COLONIA", _colonia);
                    db.command.Parameters.AddWithValue("@CP", _cp);       
                    db.command.Parameters.AddWithValue("@CADUCIDADEXAMEN", fechaexamen);
                    db.command.Parameters.AddWithValue("@CADUCIDADLICENCIA", fechalicencia);          
                    db.command.Parameters.AddWithValue("@CADUCIDADCONTRATO", fechacontrato);
                  
                    //db.command.Parameters.AddWithValue("@HUELLA", huell);
                    //db.command.Parameters.AddWithValue("@HUELLA1", huella164);
                  
                    progressBar1.Increment(30);

                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("agrego conductor " + _usuario);
                        panelContenedorForm.Visible = false;
                        toolStripRefresh_Click(sender, e);
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
                        _linea = "";
                        //pictureBox2.Image = null;
                        progressBar1.Visible = false;
                        progressBar1.Value = 0;
                        pictureBox1.Image = null;
                        _dato1 = null;
                        _dato1 = null;

                        buttonborrar.BackColor = Color.White;
                        buttonguardar.BackColor = Color.White;

                        buttonagregar.BackColor = Color.FromArgb(38, 45, 56);
                        txtUsuario.Focus();
                        dataGridViewUsuarios.Visible = true;

                    }
                    else
                    {

                        if (!Utilerias.Utilerias.CheckForInternetConnection())
                        {
                            Form mensaje = new Mensaje("Verifique la conexion a internet", true);
                            mensaje.Show();

                        }
                        else
                        {
                            Form mensaje = new Mensaje("No se pudo agregar  intente mas tarde", true);
                            mensaje.Show();

                        }
                        progressBar1.Visible = false;
                        progressBar1.Value = 0;
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

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            panelContenedorForm.Visible = false;
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            panelContenedorForm.Visible = false;
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

        private void PanelContenedorForm_Paint_1(object sender, PaintEventArgs e)
        {


        }

        private void BtnCerrar_Click_1(object sender, EventArgs e)
        {
            panelContenedorForm.Visible = false;
            lim(sender,e);
        }

        private void BtnCancelar_Click_1(object sender, EventArgs e)
        {
            lim(sender,e);
            panelContenedorForm.Visible = false;
            panelContenedorForm.Visible = false;
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void GroupBox4_Enter_1(object sender, EventArgs e)
        {

        }
        public void getDatosAdicionaleslinea()
        {
            try
            {
                comboLinea.Items.Clear();

                string sql = "SELECT LINEA,PK1 FROM LINEAS WHERE BORRADO=0";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("LINEA");
                    item.Value = res.Get("PK1");

                    comboLinea.Items.Add(item);

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

        private void GroupBox5_Enter(object sender, EventArgs e)
        {

        }

        //private void Button8_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        progressBar1.Visible = true;
        //        progressBar1.Increment(30);
        //        SaveFileDialog fichero = new SaveFileDialog();
        //        fichero.Filter = "PDF (*.pdf)|*.pdf";
        //        if (fichero.ShowDialog() == DialogResult.OK)
        //        {
        //            progressBar1.Increment(50);

        //            string documento = fichero.FileName;
        //            Document doc = new Document();
        //            PdfWriter.GetInstance(doc, new FileStream(documento, FileMode.Create));
        //            doc.Open();

        //           // iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Autobuses.Properties.Resources.llavess, System.Drawing.Imaging.ImageFormat.Png);
        //            logo.Alignment = Element.ALIGN_LEFT;
        //            logo.ScaleToFit(150f, 100f);
        //            doc.Add(new Paragraph("\n"));



        //            PdfPTable table = new PdfPTable(3);
        //            table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            table.WidthPercentage = 100;

        //            PdfPCell cell = new PdfPCell();

        //            cell = new PdfPCell(logo);
        //            cell.BorderColor = BaseColor.WHITE;
        //            table.AddCell(cell);
        //            Paragraph p2 = new Paragraph();

        //            p2.Add(new Chunk("\n"));

        //            cell = new PdfPCell(p2);
        //            cell.BorderColor = BaseColor.WHITE;
        //            table.AddCell(cell);
        //            Paragraph d = new Paragraph();
        //            d.Add(new Chunk("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy")));
        //            d.Add(new Chunk("\n"));
        //            d.Add(new Chunk("Hora: " + DateTime.Now.ToString("HH: mm tt")));



        //            cell = new PdfPCell(d);
        //            cell.BorderColor = BaseColor.WHITE;
        //            table.AddCell(cell);

        //            doc.Add(table);
        //            doc.Add(new Paragraph("\n"));

        //            Paragraph title = new Paragraph();
        //            title.Font = FontFactory.GetFont(FontFactory.TIMES, 36f, BaseColor.BLACK);
        //            title.Alignment = Element.ALIGN_CENTER;
        //            title.Add("Expediente de Conductor");
        //            doc.Add(title);
        //            doc.Add(new Paragraph("\n"));
        //            doc.Add(new Paragraph("\n"));
        //            doc.Add(new Paragraph("\n"));
        //            PdfPTable table2 = new PdfPTable(2);
        //            table2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //            table2.WidthPercentage = 100;

        //            PdfPCell cell2
        //                = new PdfPCell();

        //            MemoryStream ms = new MemoryStream();
        //            pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //            Paragraph p = new Paragraph();


        //            progressBar1.Increment(80);

        //            string usu = txtUsuario.Text;
        //            string nombre = txtNombre.Text + " " + txtApellidos.Text;
        //            string telefono = textTelefono.Text;
        //            string correo = textCorreo.Text;
                  

        //            string calle = textCalle.Text;

        //            string estado = textEstado.Text;
        //            string municipio = textMunicipio.Text;
        //            string colonia = textColonia.Text;
        //            p.Add(new Chunk("\n"));
        //            ///p.Add(TextRefsAggregater.PlainifyRichText(discussion.Background));
        //            p.Add(("Usuario: " + usu));
        //            p.Add(new Chunk("\n"));
        //            p.Add(new Chunk("\n"));
        //            p.Add(("Nombre: " + nombre));
        //            p.Add(new Chunk("\n"));
        //            p.Add(new Chunk("\n"));
        //            p.Add(("Calle: " + calle));
        //            p.Add(new Chunk("\n"));
        //            p.Add(new Chunk("\n"));
        //            p.Add(("Estado: " + estado));
        //            p.Add(new Chunk("\n"));
        //            p.Add(new Chunk("\n"));
        //            p.Add(("Municipio: " + municipio));
        //            p.Add(new Chunk("\n"));
        //            p.Add(new Chunk("\n"));
        //            p.Add(new Paragraph("Colonia: " + colonia));
        //            p.Add(new Chunk("\n"));
        //            p.Add(new Chunk("\n"));
        //            p.Add(("Telefono: " + telefono));
        //            p.Add(new Chunk("\n"));
        //            p.Add(new Chunk("\n"));
        //            p.Add(("Correo: " + correo));
        //            p.Add(new Chunk("\n"));
        //            p.Alignment = Element.ALIGN_JUSTIFIED_ALL;
        //            p.Alignment = Element.ALIGN_LEFT;
        //            cell2 = new PdfPCell(p);
        //            cell2.BorderColor = BaseColor.WHITE;
        //            table2.AddCell(cell2);
        //            byte[] buff = ms.GetBuffer();
        //            iTextSharp.text.Image PatientSign = iTextSharp.text.Image.GetInstance(buff);
        //            PatientSign.ScaleToFit(200f, 250f);

        //            PatientSign.Alignment = Element.ALIGN_RIGHT;
        //            cell2 = new PdfPCell(PatientSign);
        //            cell2.BorderColor = BaseColor.WHITE;
        //            table2.AddCell(cell2);

        //            doc.Add(table2);

        //            doc.Close();
        //            System.Diagnostics.Process.Start(fichero.FileName);

        //        }

        //        progressBar1.Increment(100);
        //        progressBar1.Visible = false;
        //        progressBar1.Value = 0;
        //    }
        //    catch (Exception err)
        //    {
        //        string error = err.Message;
        //        MessageBox.Show("Ocurrio un Error, intente de nuevo.");
        //    }
        //}

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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            getPhotoPdf((string)dataGridViewUsuarios.Rows[fila].Cells["Usuarioname"].Value, fila);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void Informacion_Click(object sender, EventArgs e)
        {

            

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

      
        private void getlicencias()
        {
            try
            {
                if (licenc == null)
                {
                    pkchof = (string)dataGridViewUsuarios.Rows[fila].Cells["pkname"].Value;
                    string sql = "SELECT LICENCIA FROM choferes where PK=@PK";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", pkchof);
                    ResultSet res = db.getTable();
                    if (res.Next())
                    {
                        licenc = res.Get("LICENCIA");
                        _pdf = licenc;
                        openPDF3();
                    }
                    else
                    {
                        Form mensaje = new Mensaje("No existe el archivo", true);

                        DialogResult resut = mensaje.ShowDialog();

                        progressBardocumento.Visible = false;
                        progressBardocumento.Value = 0;
                    }
                }
            }
            catch (Exception e)
            {

                progressBardocumento.Visible = false;
                progressBardocumento.Value = 0;
                LOG.write(_clase, "getlicencias", e.ToString());

            }


        }

        private void Informacion_SelectedIndexChanged(object sender, EventArgs e)
        {
             seleccion = (TabControl)sender;
            progressBardocumento.Visible = true;
            progressBardocumento.Value = 80;


            timerdocumentos.Start();
            
        }

        private void TabPage1_Click(object sender, EventArgs e)
        {

        }

        private void timerprogress1_Tick(object sender, EventArgs e)
        {
            try {
                timercontrato.Stop();
                openFileDialog1 = new OpenFileDialog()
            {
                FileName = "Select a pdf file",
                Filter = "Pdf files (*.pdf)|*.pdf",
                Title = "Open PDF file"
            };
            ClientSize = new Size(330, 360);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                    progressBar1.Increment(10);
                string documento = openFileDialog1.FileName;
                // Leemos todos los bytes del archivo y luego lo guardamos como Base64 en un string.
                contrato = Convert.ToBase64String(File.ReadAllBytes(documento));
                //actualizar contrato
                pkchof = (string)dataGridViewUsuarios.Rows[fila].Cells["pkname"].Value;
                string sql = "update choferes set CONTRATO=@contrato,usuario_m=@us where PK=@pk";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@US", LoginInfo.UserID);
                db.command.Parameters.AddWithValue("@pk", pkchof);
                db.command.Parameters.AddWithValue("@contrato", contrato);
                    progressBar1.Increment(10);
                    if (db.execute())
                {
                        progressBar1.Increment(10);
                        Utilerias.LOG.acciones("actualizo contrato " + "pkchofer=" + pkchof + LoginInfo.UserID);
                    Form mensaje = new Mensaje("Se actualizo contrato", true);

                    DialogResult resut = mensaje.ShowDialog();

                    _pdf = contrato;
                    openPDF2();
                }
                else
                {
                        progressBar1.Increment(10);
                        if (!Utilerias.Utilerias.CheckForInternetConnection())
                        {
                            Form mensaje1 = new Mensaje("No se pudo actualizar el contrato, verifique conexion a internet", true);

                            mensaje1.ShowDialog();
                        }
                        else
                        {
                            Form mensaje = new Mensaje("No se pudo actualizar el contrato", true);

                            mensaje.ShowDialog();
                        }
                }


            }
            progressBar1.Value = 0;
            progressBar1.Visible = false;
                enableinfo();

            }
            catch (Exception err)
            {
                enableinfo();

                progressBar1.Value = 0;
                progressBar1.Visible = false;
                Form mensaje = new Mensaje("Mo se pudo actualizar el contrato", true);

                mensaje.ShowDialog();
                LOG.write(_clase, "timercontrato", err.ToString());
            }

        }

        private void timerexamen_Tick(object sender, EventArgs e)
        {
            try
            {
                timerexamen.Stop();
                openFileDialog1 = new OpenFileDialog()
                {
                    FileName = "Select a pdf file",
                    Filter = "Pdf files (*.pdf)|*.pdf",
                    Title = "Open PDF file"
                };
                progressBar1.Increment(10);
                ClientSize = new Size(330, 360);
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    progressBar1.Increment(10);
                    string documento = openFileDialog1.FileName;
                    // Leemos todos los bytes del archivo y luego lo guardamos como Base64 en un string.
                    examen = Convert.ToBase64String(File.ReadAllBytes(documento));
                    //actualizar contrato
                    pkchof = (string)dataGridViewUsuarios.Rows[fila].Cells["pkname"].Value;
                    string sql = "update CHOFERES set EXAMEN=@examen,USUARIO_M=@US where PK=@pk";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@US", LoginInfo.UserID);
                    db.command.Parameters.AddWithValue("@pk", pkchof);
                    db.command.Parameters.AddWithValue("@examen", examen);
                    progressBar1.Increment(10);
                    if (db.execute())
                    {
                        progressBar1.Increment(10);
                        Utilerias.LOG.acciones("actualizo examen " + "pkchofer=" + pkchof + LoginInfo.UserID);
                        Form mensaje = new Mensaje("Se actualizo examen", true);

                        DialogResult resut = mensaje.ShowDialog();

                        _pdf = examen;
                        openPDF();
                    }
                    else
                    {
                        if (!Utilerias.Utilerias.CheckForInternetConnection())
                        {
                            Form mensaje1 = new Mensaje("No se pudo actualizar el examen, verifique conexion a internet", true);

                            mensaje1.ShowDialog();
                        }
                        else
                        {
                            Form mensaje = new Mensaje("No se pudo actualizar el examen", true);

                            mensaje.ShowDialog();
                        }
                    }
                }
                enableinfo();

                progressBar1.Value = 0;
                progressBar1.Visible = false;
            }
              
            catch (Exception err)
            {
                enableinfo();

                progressBar1.Value = 0;
                progressBar1.Visible = false;
                Form mensaje = new Mensaje("Mo se pudo actualizar el examen", true);

                mensaje.ShowDialog();
                LOG.write(_clase, "timerexamen", err.ToString());
            }

        }

        private void timerlicencia_Tick(object sender, EventArgs e)
        {
            try
            {
                timerlicencia.Stop();
                openFileDialog1 = new OpenFileDialog()
                {
                    FileName = "Select a pdf file",
                    Filter = "Pdf files (*.pdf)|*.pdf",
                    Title = "Open PDF file"
                };
                progressBar1.Increment(10);
                ClientSize = new Size(330, 360);
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    progressBar1.Increment(10);
                    string documento = openFileDialog1.FileName;
                    // Leemos todos los bytes del archivo y luego lo guardamos como Base64 en un string.
                    licenc = Convert.ToBase64String(File.ReadAllBytes(documento));
                    //actualizar contrato
                    pkchof = (string)dataGridViewUsuarios.Rows[fila].Cells["pkname"].Value;
                    string sql = "update CHOFERES set LICENCIA=@LICENCIA,USUARIO_M=@US where PK=@pk";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@US", LoginInfo.UserID);
                    db.command.Parameters.AddWithValue("@pk", pkchof);
                    db.command.Parameters.AddWithValue("@LICENCIA", licenc);
                    if (db.execute())
                    {
                        progressBar1.Increment(10);
                        Utilerias.LOG.acciones("actualizo licencia " + "pkchofer=" + pkchof + LoginInfo.UserID);
                        Form mensaje = new Mensaje("Se actualizo la licencia", true);

                        DialogResult resut = mensaje.ShowDialog();

                        _pdf = licenc;
                        openPDF3();
                    }
                    else
                    {
                        progressBar1.Increment(10);
                        if (!Utilerias.Utilerias.CheckForInternetConnection())
                        {
                            Form mensaje1 = new Mensaje("No se pudo actualizar la licecia, verifique conexion a internet", true);

                            mensaje1.ShowDialog();
                        }
                        else
                        {
                            Form mensaje = new Mensaje("No se pudo actualizar la licencia", true);

                            mensaje.ShowDialog();
                        }
                    }
                }
                enableinfo();

                progressBar1.Value = 0;
                progressBar1.Visible = false;
            }

            catch (Exception err)
            {
                enableinfo();

                progressBar1.Value = 0;
                progressBar1.Visible = false;
                Form mensaje = new Mensaje("Mo se pudo actualizar la licencia", true);

                mensaje.ShowDialog();
                LOG.write(_clase, "tiemrlicencia", err.ToString());
            }

        }

        private void bloquearinfo()
        {
            buttoncontrato.Enabled = false;
            buttonexamen.Enabled = false;
            buttonfoto.Enabled = false;
            buttonHULLA.Enabled = false;
            buttonlicencia.Enabled = false;
            buttonpdf.Enabled = false;
            guardar.Enabled = false;
            agregar.Enabled = false;

        }
        private void enableinfo()
        {
            guardar.Enabled = true;
            agregar.Enabled = true;
            buttoncontrato.Enabled = true;
            buttonexamen.Enabled = true;
            buttonfoto.Enabled = true;
            buttonHULLA.Enabled = true;
            buttonlicencia.Enabled = true;
            buttonpdf.Enabled = true;

        }

        private void backgroundWorkerprogres_DoWork(object sender, DoWorkEventArgs e)
        {
            if (progressBardocumento.InvokeRequired)
            {

                progressBardocumento.Invoke(new Action(() =>
                {
                    progressBardocumento.Visible = true;
                    progressBardocumento.Increment(20);
                }));
            }
            else
            {

                progressBardocumento.Visible = true;
                progressBardocumento.Increment(20);
            }

        }

        private void timerdocumentos_Tick(object sender, EventArgs e)
        {
            try
            {


                timerdocumentos.Stop();
                switch (seleccion.SelectedIndex)
                {
                    case 1:
                        getexamen();
                        break;
                    case 2:
                        getcontrato();
                        break;
                    case 3:
                        getlicencias();
                        break;


                }


                progressBardocumento.Visible = false;
                progressBardocumento.Value = 0;
            }
            catch (Exception err)
            {

                progressBardocumento.Visible = false;
                progressBardocumento.Value = 0;
                LOG.write(_clase, "timerdocumentos", err.ToString());

            }
        }

        private void Informacion_QueryAccessibilityHelp(object sender, QueryAccessibilityHelpEventArgs e)
        {

        }

        private void comboLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            _linea = comboLinea.SelectedItem.ToString();

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

        private void comboBoxcantidad_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            cantidad = int.Parse(comboBoxcantidad.Text);
            inicio = 0;
            final = cantidad;
            getRows();
        }

        private void getcontrato()
        {
            try
            {
                if (contrato == null)
                {
                    pkchof = (string)dataGridViewUsuarios.Rows[fila].Cells["pkname"].Value;
                    string sql = "SELECT CONTRATO FROM choferes where PK=@PK";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", pkchof);
                    ResultSet res = db.getTable();
                    if (res.Next())
                    {
                        contrato = res.Get("CONTRATO");
                        _pdf = contrato;
                        openPDF2();
                    }
                    else
                    {
                        Form mensaje = new Mensaje("No existe el archivo", true);
                        progressBardocumento.Visible = false;
                        progressBardocumento.Value = 0;
                        DialogResult resut = mensaje.ShowDialog();
                    }
                }
            }
            catch (Exception e)
            {

                progressBardocumento.Visible = false;
                progressBardocumento.Value = 0;
                LOG.write(_clase, "getcontrato", e.ToString());

            }


        }

        private void getexamen()
        {
            try
            {
                if (examen == null)
                {
                    pkchof = (string)dataGridViewUsuarios.Rows[fila].Cells["pkname"].Value;
                    string sql = "SELECT EXAMEN FROM choferes where PK=@PK";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", pkchof);
                    ResultSet res = db.getTable();
                    if (res.Next())
                    {
                        examen = res.Get("EXAMEN");
                        _pdf = examen;
                        openPDF();
                    }
                    else
                    {
                        Form mensaje = new Mensaje("No existe el archivo", true);

                        DialogResult resut = mensaje.ShowDialog();

                        progressBardocumento.Visible = false;
                        progressBardocumento.Value = 0;
                    }
                }
            }
            catch (Exception e)
            {
                progressBardocumento.Visible = false;
                progressBardocumento.Value = 0;
                LOG.write(_clase, "getexamen", e.ToString());

            }


        }


    }

}
