using Autobuses.Utilerias;
using ConnectDB;
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

namespace Autobuses.Administracion
{
    public partial class Configuración_de_tarjetas : Form
    {
        public database db;
        ResultSet res = null;
        private string tarjetaturnocostoturno = "";
        private string tarjetaturnocostosalida = "";
        private string tarjetaturnocostopaso = "";
        private string linea;
        private string origen;
        private string destino;

        private string _search;
        private List<string> costos = new List<string>();

        private string _clase = "configuracion de tarjetas";
        public Configuración_de_tarjetas()
        {
            InitializeComponent();
            this.Show();

            btnNormal.Visible = true;
            btnMaximizar.Visible = false;
            titulo.Text = "Tarjetas";
        }

        private void permisos()
        {
            buttonagregar.Visible = false;
            buttonborrar.Visible = false;
            buttonguardar.Visible = false;
            if (LoginInfo.privilegios.Any(x => x == "agregar tarjetas"))
            {
                buttonagregar.Visible = true;

            }
            if (LoginInfo.privilegios.Any(x => x == "borrar tarjetas"))
            {
                buttonborrar.Visible = true;

            }
            if (LoginInfo.privilegios.Any(x => x == "modificar tarjetas"))
            {
                buttonguardar.Visible = true;

            }
        }

        public void getDatosAdicionales()
        {
            string sql = "SELECT LINEA,PK1 FROM LINEAS ";
            try
            {

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

                if (comboLinea != null && comboLinea.Items.Count > 0)
                {
                    comboLinea.SelectedIndex = 0;
                }



            }
            catch (Exception e)
            {
                LOG.write("ROLADO", "getDatosAdicionales", e.Message);
            }
        }
        public void getDatosAdicionalesdestino()
        {
            string sql = "SELECT DESTINO,PK1 FROM DESTINOS ";
            try
            {

                comboBoxorigen.Items.Clear();

                db.PreparedSQL(sql);
                res = db.getTable();


                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("DESTINO");
                    item.Value = res.Get("PK1");
                    comboBoxorigen.Items.Add(item);

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }

                if (comboBoxorigen != null && comboBoxorigen.Items.Count > 0)
                {
                    comboBoxorigen.SelectedIndex = 0;
                }



            }
            catch (Exception e)
            {
                LOG.write("ROLADO", "getDatosAdicionales", e.Message);
            }
        }

        public void getDatosAdicionalesdestinos()
        {
            string sql = "SELECT DESTINO,PK1 FROM DESTINOS ";
            try
            {

                comboBoxdestino.Items.Clear();

                db.PreparedSQL(sql);
                res = db.getTable();


                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("DESTINO");
                    item.Value = res.Get("PK1");
                    comboBoxdestino.Items.Add(item);

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }

                if (comboBoxdestino != null && comboBoxorigen.Items.Count > 0)
                {
                    comboBoxdestino.SelectedIndex = 0;
                }



            }
            catch (Exception e)
            {
                LOG.write("ROLADO", "getDatosAdicionales", e.Message);
            }
        }

        private void ToolStripCerrar_Click(object sender, EventArgs e)
        {


            this.Close();

        }

        private void ToolStripSessionClose_Click(object sender, EventArgs e)
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

        private void ComboLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            getDatosAdicionales();
            getRows();
            textBoxturno.Enabled = true;
            textBoxpaso.Enabled = true;
            textBoxsalida.Enabled = true;
        }

        private void Agregarbtn_Click(object sender, EventArgs e)
        {
            bool sepuede= true;
            linea = comboLinea.Text;
            origen = comboBoxorigen.Text;
            destino = comboBoxdestino.Text;
            if(origen == destino)
            {
                Form mensaje = new Mensaje("El destino no puede ser igual que el origen", true);

                mensaje.ShowDialog();
                return;
            }
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                string lineacontenida = dataGridView1.Rows[i].Cells["origenname"].Value.ToString();

                string origencontenido = dataGridView1.Rows[i].Cells["origenname"].Value.ToString();
                if (lineacontenida == linea && origencontenido == origen)
                {
                    MessageBox.Show("Ya tiene asignado costo");
                    sepuede = false;
                }
            }
            if (sepuede == true) {

                if (textBoxpaso.Text == "" || textBoxsalida.Text == "" || textBoxpaso.Text == "")
                {
                    labelpaso.Visible = true;
                    labelsalida.Visible = true;
                    labelturno.Visible = true;

                }

                else
                {
                    labelpaso.Visible = false;
                    labelsalida.Visible = false;
                    labelturno.Visible = false;
                    tarjetaturnocostoturno = textBoxturno.Text;
                    tarjetaturnocostosalida = textBoxsalida.Text;
                    tarjetaturnocostopaso = textBoxpaso.Text;

                    string sql = "INSERT INTO TARJETAS(LINEA,TARJETA,COSTO,ORIGEN,DESTINO,USUARIO) VALUES(@LINEA,@TARJETA,@COSTO,@ORIGEN,@DESTINO,@USUARIO)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@LINEA", linea);
                    db.command.Parameters.AddWithValue("@TARJETA", "TURNO");
                    db.command.Parameters.AddWithValue("@COSTO", tarjetaturnocostoturno);
                    db.command.Parameters.AddWithValue("@ORIGEN", origen);
                    db.command.Parameters.AddWithValue("@DESTINO", destino);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);

                    db.execute();

                     sql = "INSERT INTO TARJETAS(LINEA,TARJETA,COSTO,ORIGEN,DESTINO,USUARIO) VALUES(@LINEA,@TARJETA,@COSTO,@ORIGEN,@DESTINO,@USUARIO)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@LINEA", linea);
                    db.command.Parameters.AddWithValue("@TARJETA", "SALIDA");
                    db.command.Parameters.AddWithValue("@COSTO", tarjetaturnocostosalida);
                    db.command.Parameters.AddWithValue("@ORIGEN", origen);
                    db.command.Parameters.AddWithValue("@DESTINO", destino);
                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                    db.execute();


                     sql = "INSERT INTO TARJETAS(LINEA,TARJETA,COSTO,ORIGEN,DESTINO,USUARIO) VALUES(@LINEA,@TARJETA,@COSTO,@ORIGEN,@DESTINO,@USUARIO)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@LINEA", linea);
                    db.command.Parameters.AddWithValue("@TARJETA", "PASO");
                    db.command.Parameters.AddWithValue("@COSTO", tarjetaturnocostopaso);
                    db.command.Parameters.AddWithValue("@ORIGEN", origen);
                    db.command.Parameters.AddWithValue("@DESTINO", destino);

                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                    if (db.execute())
                    {
                        limpiar();

                    }

                }


            }
        }
        private void limpiar()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            textBoxturno.Text = "";
            textBoxsalida.Text = "";
            textBoxpaso.Text = "";
            getDatosAdicionales();
            labelpaso.Visible = false;
            labelsalida.Visible = false;
            labelturno.Visible = false;
            textBoxturno.Enabled = true;
            textBoxsalida.Enabled = true;
            textBoxpaso.Enabled = true;

            buttonagregar.Enabled = true;
            buttonguardar.Enabled = false;
            buttonborrar.Enabled = false;

            buttonagregar.BackColor= Color.FromArgb(38, 45, 56);
            buttonguardar.BackColor = Color.White;
            buttonborrar.BackColor = Color.White;


            getRows();
        }
        public void getRows(string search = "")
        {
            try
            {
                dataGridView1.Rows.Clear();
                int count = 1;
                string sql = "SELECT LINEA,TARJETA,COSTO,ORIGEN,DESTINO FROM TARJETAS ";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += "WHERE ORIGEN LIKE @SEARCH order by origen, destino";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + search + "%");

                }
                else
                {
                    sql += " order by origen, destino";

                    db.PreparedSQL(sql);

                }

                res = db.getTable();
                int n = 0;
                while (res.Next())
                {

                    n = dataGridView1.Rows.Add();

                    dataGridView1.Rows[n].Cells["lineaname"].Value = res.Get("LINEA");
                    dataGridView1.Rows[n].Cells["tarjetaname"].Value = res.Get("TARJETA");
                    double cost= res.GetDouble("COSTO");
                    dataGridView1.Rows[n].Cells["costoname"].Value = Utilerias.Utilerias.formatCurrency(cost);
                    dataGridView1.Rows[n].Cells["origenname"].Value = res.Get("ORIGEN");
                    dataGridView1.Rows[n].Cells["destinoname"].Value = res.Get("DESTINO");
                    dataGridView1.Rows[n].Cells["costodoubname"].Value = cost;



                    count++;
                }

            }
            catch (Exception e)
            {
                string error = e.Message;
                LOG.write("Rutas", "TARJETAS", e.Message, "Usuario");

            }

        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {

                int n = e.RowIndex;
                int c = e.ColumnIndex;
                textBoxturno.Enabled = true;
                textBoxsalida.Enabled = true;
                textBoxpaso.Enabled = true;

                buttonagregar.Enabled = false;
                buttonborrar.Enabled = true;
                buttonguardar.Enabled = true;
                buttonagregar.BackColor = Color.White;
                buttonguardar.BackColor = Color.FromArgb(38, 45, 56);
                buttonborrar.BackColor = Color.FromArgb(38, 45, 56);


                n = e.RowIndex;

                if (n != -1)
                {
                    linea = (string)dataGridView1.Rows[n].Cells[0].Value;


                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        string dato = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        if (dato == linea)
                        {
                            costos.Add(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        }
                    }
                    comboLinea.Items.Clear();
                    comboLinea.Items.Add(linea);
                    comboLinea.SelectedIndex = 0;
                    textBoxturno.Text = costos[0];
                    textBoxsalida.Text = costos[1];
                    textBoxpaso.Text = costos[2];
                    costos.Clear();


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

        private void Borrarbtn_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show("¿Esta seguro de eliminar el registro?",
                                     "Confirmar eliminar",
                                     MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    string sql;
                    sql = "DELETE FROM TARJETAS WHERE LINEA = @LINEA AND ORIGEN=@ORIGEN AND DESTINO=@DESTINO";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@LINEA", linea);
                    db.command.Parameters.AddWithValue("@DESTINO", destino);
                    db.command.Parameters.AddWithValue("@ORIGEN", origen);

                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("borro las tarjetas para la linea" + linea);
                        limpiar();

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

        private void Guardarbtn_Click(object sender, EventArgs e)
        {
            try
            {
                string linea2 = comboLinea.Text;
                string origen2 = comboBoxorigen.Text;
                string destino2 = comboBoxdestino.Text;
                if (destino2 == origen2)
                {
                    Form mensaje = new Mensaje("El destino no puede ser igual que el origen", true);

                    mensaje.ShowDialog();
                    return;
                }


                if (textBoxpaso.Text == "" || textBoxsalida.Text == "" || textBoxpaso.Text == "")
                {
                    labelpaso.Visible = true;
                    labelsalida.Visible = true;
                    labelturno.Visible = true;

                }

                else
                {
                    labelpaso.Visible = false;
                    labelsalida.Visible = false;
                    labelturno.Visible = false;
                    tarjetaturnocostoturno = textBoxturno.Text;
                    tarjetaturnocostosalida = textBoxsalida.Text;
                    tarjetaturnocostopaso = textBoxpaso.Text;

                    string sql = "UPDATE TARJETAS SET LINEA=@LINEA,DESTINO=@DESTINO2,COSTO=@COSTO,ORIGEN=@ORIGEN WHERE LINEA=@LINEA2 AND TARJETA=@TARJET AND ORIGEN=@ORIGEN2 AND DESTINO=@DESTINO2";


                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@LINEA2", linea);
                    db.command.Parameters.AddWithValue("@ORIGEN2", origen);
                    db.command.Parameters.AddWithValue("@DESTINO2", destino2);

                    db.command.Parameters.AddWithValue("@LINEA", linea2);
                    db.command.Parameters.AddWithValue("@COSTO", tarjetaturnocostoturno);
                    db.command.Parameters.AddWithValue("@TARJET", "TURNO");
                    db.command.Parameters.AddWithValue("@ORIGEN", origen2);

                    db.execute();
                     sql = "UPDATE TARJETAS SET LINEA=@LINEA,COSTO=@COSTO,DESTINO=@DESTINO2,ORIGEN=@ORIGEN WHERE LINEA=@LINEA2 AND TARJETA=@TARJET AND ORIGEN=@ORIGEN2 AND DESTINO=@DESTINO2";


                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@LINEA2", linea);
                    db.command.Parameters.AddWithValue("@LINEA", linea2);
                    db.command.Parameters.AddWithValue("@ORIGEN2", origen);
                    db.command.Parameters.AddWithValue("@DESTINO2", destino2);
                    db.command.Parameters.AddWithValue("@COSTO", tarjetaturnocostosalida);
                    db.command.Parameters.AddWithValue("@TARJET", "SALIDA");
                    db.command.Parameters.AddWithValue("@ORIGEN", origen2);

                    db.execute();
                     sql = "UPDATE TARJETAS SET LINEA=@LINEA,COSTO=@COSTO,DESTINO=@DESTINO2,ORIGEN=@ORIGEN WHERE LINEA=@LINEA2 AND TARJETA=@TARJET AND ORIGEN=@ORIGEN2 AND DESTINO=@DESTINO2";


                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@LINEA2", linea);
                    db.command.Parameters.AddWithValue("@LINEA", linea2);
                    db.command.Parameters.AddWithValue("@ORIGEN2", origen);
                    db.command.Parameters.AddWithValue("@DESTINO2", destino2);
                    db.command.Parameters.AddWithValue("@COSTO", tarjetaturnocostopaso);
                    db.command.Parameters.AddWithValue("@TARJET", "PASO");
                    db.command.Parameters.AddWithValue("@ORIGEN", origen2);


                    if (db.execute())
                    {
                        Form mensaje = new Mensaje("modificado", true);

                        mensaje.ShowDialog();
                        limpiar();

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

        private void Limpiarbtn_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void ToolStripRefresh_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void ComboLinea_MouseClick(object sender, MouseEventArgs e)
        {
            getDatosAdicionales();
            getRows();
            textBoxturno.Enabled = true;
            textBoxpaso.Enabled = true;
            textBoxsalida.Enabled = true;
        }

        private void DataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Buscar_Click(object sender, EventArgs e)
        {
            _search = toolStripTextBox1.Text;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            getRows(_search);
        }

        private void ToolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            _search = toolStripTextBox1.Text;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            getRows(_search);
        }

        private void Configuración_de_tarjetas_Load(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;
        }

        private void Configuración_de_tarjetas_Shown(object sender, EventArgs e)
        {
            db = new database();


            getDatosAdicionalesdestino();
            getDatosAdicionalesdestinos();
            limpiar();

            getRows();

            permisos();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            timer1.Interval = 1;
            timer1.Start();
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
        }

        private void ToolStripExportExcel_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridView1);

        }

        private void ComboLinea_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void ComboBoxorigen_Click(object sender, EventArgs e)
        {
        
            getRows();
            textBoxturno.Enabled = true;
            textBoxpaso.Enabled = true;
            textBoxsalida.Enabled = true;
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //METODOS PARA CERRAR,MAXIMIZAR, MINIMIZAR FORMULARIO------------------------------------------------------
        int lx, ly;
        int sw, sh;

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PanelBarraTitulo_MouseMove(object sender, MouseEventArgs e)
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

        private void comboBoxorigen_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            try
                {

                int n = e.RowIndex;
                int c = e.ColumnIndex;
                textBoxturno.Enabled = true;
                textBoxsalida.Enabled = true;
                textBoxpaso.Enabled = true;

                buttonagregar.Enabled = false;
                buttonborrar.Enabled = true;
                buttonguardar.Enabled = true;

                buttonagregar.BackColor = Color.White;
                buttonguardar.BackColor = Color.FromArgb(38, 45, 56);
                buttonborrar.BackColor = Color.FromArgb(38, 45, 56);


                n = e.RowIndex;

                if (n != -1)
                {
                    linea = (string)dataGridView1.Rows[n].Cells["lineaname"].Value;

                    origen = (string)dataGridView1.Rows[n].Cells["origenname"].Value;
                    destino = (string)dataGridView1.Rows[n].Cells["destinoname"].Value;


                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        string origenverificar = dataGridView1.Rows[i].Cells["origenname"].Value.ToString();
                        string lineaverificar = dataGridView1.Rows[i].Cells["lineaname"].Value.ToString();

                        if (origenverificar == origen && lineaverificar == linea)
                        {
                            costos.Add(dataGridView1.Rows[i].Cells["costodoubname"].Value.ToString());
                        }
                    }
                    comboBoxdestino.Text = dataGridView1.Rows[n].Cells["destinoname"].Value.ToString();
                    comboBoxorigen.Text = dataGridView1.Rows[n].Cells["origenname"].Value.ToString();
                    comboLinea.Text = dataGridView1.Rows[n].Cells["lineaname"].Value.ToString();

                    //comboLinea.Items.Add(linea);
                    //comboLinea.Items.Add(origen);
                    //comboBoxorigen.SelectedIndex = 0;
                    //comboLinea.SelectedIndex = 0;
                    textBoxturno.Text = costos[0];
                    textBoxsalida.Text = costos[1];
                    textBoxpaso.Text = costos[2];
                    costos.Clear();


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

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Buscar_Click(sender, e);
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
    }
}
