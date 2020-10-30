using Autobuses.Utilerias;
using ConnectDB;
using System;
using System.Collections;
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
    public partial class TiposdePasaje : Form
    {
        public database db;
        private int n = 0;
        ResultSet res = null;
        string _clase="Tipos de pasaje";
        string _pasaje;

        string _color;
        string _porcentaje;
        string _linea;
        string _user;
        string _pk;
        int _permitidos;

        private string _search;
        public TiposdePasaje()
        {

            InitializeComponent();
            this.Show();

            btnNormal.Visible = true;
            btnMaximizar.Visible = false;
            titulo.Text = "Tipo de pasaje";
        }

        private void permisos()
        {
            buttonagregar.Visible = false;
            buttonborrar.Visible = false;
            buttonmodificar.Visible = false;
            if (LoginInfo.privilegios.Any(x => x == "Agregar Tipo de Pasaje"))
            {
                buttonagregar.Visible = true;

            }
            if (LoginInfo.privilegios.Any(x => x == "Borrar Tipo de Pasaje"))
            {
                buttonborrar.Visible = true;

            }
            if (LoginInfo.privilegios.Any(x => x == "Modificar Tipo de Pasaje"))
            {
                buttonmodificar.Visible = true;

            }
        }

        private void colores()
        {
            try
            {
                KnownColor enumColor = new KnownColor();
                Array Colors = Enum.GetValues(enumColor.GetType());
                ArrayList ALColor = new ArrayList();


                foreach (object clr in Colors)
                    if (!Color.FromKnownColor((KnownColor)clr).IsSystemColor)
                        ALColor.Add(clr.ToString());


                for (int i = 1; i < ALColor.Count; i++)
                {
                    ComboboxItem item = new ComboboxItem();

                    item.Text = ALColor[i].ToString();
                
                    comboBoxcolor.Items.Add(item);

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "colores";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
   
    
        public void getDatosAdicionales()
        {
            try
            {
                comboBoxlinea.Items.Clear();
                comboBoxpermitidos.Items.Clear();

                string sql = "SELECT LINEA,PK1 FROM LINEAS WHERE BORRADO=0 ";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("LINEA");
                    item.Value = res.Get("PK1");
                
                    comboBoxlinea.Items.Add(item);
                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }
                comboBoxlinea.SelectedItem = null;
                ComboboxItem item0 = new ComboboxItem();
                item0.Text = "SIN LIMITES";
                item0.Value = 100;

                comboBoxpermitidos.Items.Add(item0);


                for (int i=0;i<=30; i++)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = (i + 1).ToString();
                    item.Value = (i + 1);

                    comboBoxpermitidos.Items.Add(item);
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
        private void buscar(object sender, EventArgs e)
        {
            try
            {
                _search = serch.Text;
                string sql = "SELECT * FROM VISTATIPODEPASAJE ";
                int count = 1;
                if (!string.IsNullOrEmpty(_search))
                {
                    listapasajeros.Rows.Clear();

                    sql += "WHERE PASAJE LIKE @SEARCH OR LINEA LIKE @SEARCH ";
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
                    n = listapasajeros.Rows.Add();


                    listapasajeros.Rows[n].Cells[0].Value = count;

                    listapasajeros.Rows[n].Cells[1].Value = res.Get("PASAJE");
                    listapasajeros.Rows[n].Cells[2].Value = res.Get("LINEA");
                    listapasajeros.Rows[n].Cells[3].Value = res.Get("PORCENTAJE");
                    listapasajeros.Rows[n].Cells[4].Value = res.Get("PKLINEA");
                    listapasajeros.Rows[n].Cells["pktpname"].Value = res.Get("PKI");



                    count++;
                }
                if (serch.Text == "")
                {
                    listapasajeros.Rows.Clear();
                    listapasajeros.Refresh();
                    getRows();

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

              //  Program.Form.DesactivarMenu();

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
        private void bucartext(object sender, KeyEventArgs e)
        {
            buscar(sender, e);
        }

        private void agregar(object sender, EventArgs e)
        {
            try
            {


                if (ValidarInput())
                {

                    string sql1 = "SELECT count(PASAJE) MAX FROM TIPODEPASAJE WHERE PASAJE='" +_pasaje + "' AND PKLINEA='"+_linea+"'";

                    if (db.Count(sql1) > 0)
                    {
                        Form mensaje = new Mensaje("El tipo de pasaje ya existe", true);

                        DialogResult resut = mensaje.ShowDialog();
                        limpiar(sender, e);

                        return;
                    }

                    string sql = "INSERT INTO TIPODEPASAJE (PASAJE, PKLINEA, PORCENTAJE,BORRADO,PERMITIDOS,ACTIVO,COLOR) VALUES(@PASAJE,@LINEA,@PORCENTAJE,0,@PERMITIDOS,1,@COLOR) ";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PASAJE", _pasaje);
                    db.command.Parameters.AddWithValue("@LINEA", _linea);
                    db.command.Parameters.AddWithValue("@PORCENTAJE", _porcentaje);
                    db.command.Parameters.AddWithValue("@PERMITIDOS", _permitidos);
                    db.command.Parameters.AddWithValue("@COLOR", _color);

                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("agrego un tipo de pasaje" + _pasaje);
                            Form mensaje = new Mensaje("Agregado", true);

                        DialogResult resut = mensaje.ShowDialog();
                        limpiar(sender,e);
                        getDatosAdicionales();
                        listapasajeros.CurrentRow.Selected = false;

                    }



                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "agregar";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        private Boolean ValidarInput()
        {
            Boolean validado = true;
            try
            {
                _pasaje = txtnombre.Text;
                //_linea = txt.Text;
                _porcentaje = txtporcentaje.Text;
                if (_pasaje == "")
                {
                    labelnombre.Visible = true;
                    validado = false;
                }
                if (_permitidos == null)
                {
                    labelpermitidos.Visible = true;
                    validado = false;
                }
                if (comboBoxlinea.SelectedItem != null)
                {
                    _linea = (comboBoxlinea.SelectedItem as ComboboxItem).Value.ToString();


                }
                if (comboBoxcolor.SelectedItem != null)
                {
                    _color = comboBoxcolor.SelectedItem.ToString();


                }
               else
                {
                    labelcolor.Visible = true;
                    validado = false;
                }

                if (comboBoxpermitidos.SelectedItem != null)
                {
                    _permitidos =int.Parse( (comboBoxpermitidos.SelectedItem as ComboboxItem).Value.ToString());

                }
                if (_linea == null)
                {
                    labelcolor.Visible = true;
                    validado = false;
                }
                int ejem = 0;

                if (!int.TryParse(_porcentaje, out ejem))
                {
                    labelporcentaje.Visible = true;
                    validado = false;

                }
                if (_linea == "" || _linea == "0")
                {
                    labellinea.Visible = true;
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

        private Boolean ValidarInputmodificar()
        {
            Boolean validado = true;

            try
            {
                _pasaje = txtnombre.Text;
                //_linea = txt.Text;
                _porcentaje = txtporcentaje.Text;
                if (_pasaje == "")
                {
                    labelnombre.Visible = true;
                    validado = false;
                }
                if (comboBoxlinea.SelectedItem != null)
                {
                    _linea = (comboBoxlinea.SelectedItem as ComboboxItem).Value.ToString();
                }
                if (comboBoxpermitidos.SelectedItem != null)
                {
                    _permitidos = int.Parse((comboBoxpermitidos.SelectedItem as ComboboxItem).Value.ToString());
                }
                if (comboBoxcolor.SelectedItem != null)
                {
                    _color = (comboBoxcolor.SelectedItem.ToString());
                }

                int ejem = 0;

                if (!int.TryParse(_porcentaje, out ejem))
                {
                    labelporcentaje.Visible = true;
                    validado = false;

                }
                if (_linea == "" || _linea == "0")
                {
                    labellinea.Visible = true;
                    validado = false;
                }



            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "validarinputmodif";
                Utilerias.LOG.write(_clase, funcion, error);


            }

            return validado;

        }
        private void limpiarerrores()
        {
            labellinea.Visible = false;
            labelnombre.Visible = false;
            labelporcentaje.Visible = false;
            labelpermitidos.Visible = false;
            labelcolor.Visible = false;

        }


        public void getRows()
        {
            try
            {
                int count = 1;
                string sql = "SELECT * FROM VISTATIPODEPASAJE WHERE BORRADO=0 ORDER BY LINEA";

           
                    db.PreparedSQL(sql);

                

                res = db.getTable();

                while (res.Next())
                {
                    n = listapasajeros.Rows.Add();


                    listapasajeros.Rows[n].Cells[0].Value = count;

                    listapasajeros.Rows[n].Cells["pasajename"].Value = res.Get("PASAJE");
                    listapasajeros.Rows[n].Cells["lineaname"].Value = res.Get("LINEA");
                    listapasajeros.Rows[n].Cells["porcentajename"].Value = res.Get("PORCENTAJE");
                    listapasajeros.Rows[n].Cells["pklineaname"].Value = res.Get("PKLINEA");
                    listapasajeros.Rows[n].Cells["pktpname"].Value = res.Get("PKI");
                    listapasajeros.Rows[n].Cells["activarname"].Value = res.Get("ACTIVO");
                    listapasajeros.Rows[n].Cells["colorname"].Value = res.Get("COLOR");

                    int temp = res.GetInt("PERMITIDOS");
                    if (temp == 100)
                    {
                        listapasajeros.Rows[n].Cells["descuentospermitidosname"].Value = "SIN LIMITE";
                    }
                    else
                        listapasajeros.Rows[n].Cells["descuentospermitidosname"].Value = temp;


                    count++;
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

        private void seleccionlista(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                n = e.RowIndex;

                int c = e.ColumnIndex;
                if (n != -1 && c != 8)
                {
               

                


                        txtnombre.Text = (string)listapasajeros.Rows[n].Cells["pasajename"].Value;
                        string temp = (string)listapasajeros.Rows[n].Cells["descuentospermitidosname"].Value.ToString();
                    if (temp=="SIN LIMITE")
                    comboBoxpermitidos.Text = "SIN LIMITE";
                    else
                        comboBoxpermitidos.Text = temp;


                    _pk = (string) listapasajeros.Rows[n].Cells["pktpname"].Value;
                    comboBoxcolor.Text = listapasajeros.Rows[n].Cells["colorname"].Value.ToString();
                     _linea = (listapasajeros.Rows[n].Cells["pklineaname"].Value.ToString());
                    comboBoxlinea.Text = listapasajeros.Rows[n].Cells["lineaname"].Value.ToString();
                    txtporcentaje.Text = listapasajeros.Rows[n].Cells["porcentajename"].Value.ToString();

                    //string valor =(listapasajeros.Rows[n].Cells["lineaname"].Value.ToString());
                    //seleccionlinea(valor);

                    buttonagregar.Enabled = false;
                        buttonagregar.BackColor = Color.White;
                        buttonmodificar.BackColor = Color.FromArgb(38, 45, 53);
                        buttonborrar.BackColor = Color.FromArgb(38, 45, 53);
                    buttonmodificar.Enabled = true;
                    buttonborrar.Enabled = true;
                    limpiarerrores();

                    }
                
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "sellecionlista";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        private void seleccionlinea(string valor)
        {
            try
            {
                
                comboBoxlinea.Items.Clear();

                ComboboxItem item = new ComboboxItem();
                item.Value = 0;
                item.Text = valor;
                comboBoxlinea.Items.Add(item);
                comboBoxlinea.SelectedIndex = 0;


            }

            
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "seleccionlinea";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }



        private void limpiar(object sender, EventArgs e)
        {
            try
            {
                serch.Text = "";
                listapasajeros.Rows.Clear();
                getRows();
                txtnombre.Text = "";
                txtporcentaje.Text = "";
                buttonborrar.Enabled = false;
                buttonmodificar.Enabled = false;
                buttonborrar.BackColor = Color.White;
                buttonmodificar.BackColor = Color.White;
                buttonagregar.Enabled = true;
                buttonagregar.BackColor = Color.FromArgb(38, 45, 53);
                pictureBoxcolor.BackColor = Color.FromArgb(38, 45, 53);

                limpiarerrores();
                comboBoxlinea.SelectedItem = null;
                comboBoxpermitidos.SelectedItem = null;
                comboBoxcolor.SelectedItem = null;

                comboBoxpermitidos.Text = "";

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "seleccionlinea";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void actualizar(object sender, EventArgs e)
        {
            getDatosAdicionales();
        }

        private void actualizarinfo(object sender, EventArgs e)
        {
            try
            {
            
               

                if (ValidarInputmodificar())
                {


                    string sql1 = "SELECT COUNT(PKI) FROM TIPODEPASAJE  WHERE PASAJE =@PASAJE AND PKLINEA=@LINEA AND NOT PKI=@PK";


                    db.PreparedSQL(sql1);
                    db.command.Parameters.AddWithValue("@PASAJE", _pasaje);
                    db.command.Parameters.AddWithValue("@PK", _pk);
                    db.command.Parameters.AddWithValue("@LINEA", _linea);
                    res = db.getTable();

                    if (res.Next()) {
                        string sql = "UPDATE TIPODEPASAJE SET PASAJE=@PASAJE,PKLINEA=@LINEA,PORCENTAJE=@PORCENTAJE, PERMITIDOS=@PERMITIDOS, COLOR=@COLOR,USUARIO_M=@USUARIO_M WHERE PKI=@PK";




                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PASAJE", _pasaje);
                        db.command.Parameters.AddWithValue("@PK", _pk);

                        db.command.Parameters.AddWithValue("@LINEA", _linea);
                        db.command.Parameters.AddWithValue("@PORCENTAJE", _porcentaje);
                        db.command.Parameters.AddWithValue("@PERMITIDOS", _permitidos);
                        db.command.Parameters.AddWithValue("@USUARIO_M", LoginInfo.UserID);
                        db.command.Parameters.AddWithValue("@COLOR", _color);

                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico un tipo de pasaje" + _user);

                            listapasajeros.Rows.Clear();
                            getRows();
                            txtnombre.Text = "";
                            txtporcentaje.Text = "";
                            comboBoxlinea.SelectedItem = null;
                            buttonagregar.Enabled = true;
                            buttonmodificar.Enabled = false;
                            buttonborrar.Enabled = false;
                            comboBoxpermitidos.SelectedItem = null;
                            comboBoxpermitidos.Text = "";

                            buttonagregar.BackColor = Color.FromArgb(38, 45, 56);
                            buttonmodificar.BackColor = Color.White;
                            buttonborrar.BackColor = Color.White;

                            limpiarerrores();
                            getDatosAdicionales();
                            listapasajeros.CurrentRow.Selected = false;

                        }
                    }
                    else
                    {
                        Form mensaje = new Mensaje("No se pudo realizar el registro, ya existe uno", true);
                        mensaje.ShowDialog();
                    }
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "actualizarinfo";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void Borrar_Click(object sender, EventArgs e)
        {
         
                try
                {
                Form mensaje = new Mensaje("¿Está seguro de borrar el registro?", false);

                DialogResult resut = mensaje.ShowDialog();
                if (resut == DialogResult.OK)
                {
                    _pasaje = txtnombre.Text;
                        string sql;
                        sql = "UPDATE  TIPODEPASAJE SET BORRADO=1 WHERE PKI = @PK";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PK", _pk);

                        if (db.execute())
                        {
                        Utilerias.LOG.acciones("borro un tipo de pasaje" + " USUARIO: " + LoginInfo.PkUsuario);

                        if (n != -1)
                            {
                                listapasajeros.Rows.RemoveAt(n);
                            txtnombre.Focus();
                            limpiar(sender , e);
                           
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
                    string funcion = "borrar_click";
                    Utilerias.LOG.write(_clase, funcion, error);

                }


            


        }


        private void TiposdePasaje_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void TiposdePasaje_Shown(object sender, EventArgs e)
        {
            db = new database();

            getRows();
            getDatosAdicionales();
            txtnombre.Focus();
            //listapasajeros.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
            listapasajeros.EnableHeadersVisualStyles = false;
        
            buttonborrar.Enabled = false;
            buttonmodificar.Enabled = false;

            buttonborrar.BackColor = Color.White;
            buttonmodificar.BackColor = Color.White;
            limpiarerrores();

            permisos();
            comboBoxlinea.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxcolor.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxpermitidos.DropDownStyle = ComboBoxStyle.DropDownList;

            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            this.Focus();
            colores();
            timer1.Start();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);



        }

        private void ToolStripExportExcel_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(listapasajeros);

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }


        
        private void BtnMinimizar_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void BtnNormal_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;

            btnNormal.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void BtnMaximizar_Click_1(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;


            btnMaximizar.Visible = false;
            btnNormal.Visible = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void Check(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                n = e.RowIndex;
                int c = e.ColumnIndex;
                if (n != -1 && c == 8)
                {
                   


                        string pkname = (string)listapasajeros.Rows[n].Cells["pktpname"].Value;

                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)listapasajeros.Rows[n].Cells[c];
                        if (chk.Selected == true)

                        {
                            string act = (string)listapasajeros.Rows[n].Cells["activarname"].Value.ToString();
                            string val = act;

                            if (act == "False")
                            {
                                string sql = "UPDATE TIPODEPASAJE SET ACTIVO=@ACTIVO,USUARIO_M=@USUARIO  WHERE PKI=@pk and borrado=0";

                                db.PreparedSQL(sql);
                                db.command.Parameters.AddWithValue("@pk", pkname);
                                db.command.Parameters.AddWithValue("@ACTIVO", 1);
                                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);

                                if (db.execute())
                                {
                                    Form mensaje = new Mensaje("Se activo el tipo de pasaje", true);
                                    DialogResult resut = mensaje.ShowDialog();
                                    listapasajeros.Refresh();
                                    listapasajeros.Rows.Clear();
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
                            string sql = "UPDATE TIPODEPASAJE SET ACTIVO=@ACTIVO,USUARIO_M=@USUARIO  WHERE PKI=@pk and borrado=0";

                            db.PreparedSQL(sql);
                                db.command.Parameters.AddWithValue("@pk", pkname);
                                db.command.Parameters.AddWithValue("@ACTIVO", 0);
                                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);

                                if (db.execute())
                                {
                                    Form mensaje = new Mensaje("Se desactivo el tipo de pasaje", true);
                                    DialogResult resut = mensaje.ShowDialog();
                                    listapasajeros.Refresh();
                                    listapasajeros.Rows.Clear();
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
          
                
            }
            catch (Exception err)
            {
                string error = err.Message;
                string funcion = "check";
                Utilerias.LOG.write(_clase, funcion, error);
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBoxcolor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxcolor.SelectedItem != null)
            {
                string obtenercol = comboBoxcolor.SelectedItem.ToString();
                Color col = Color.FromName(obtenercol);
                pictureBoxcolor.BackColor = col;
            }
        }
    }
}
