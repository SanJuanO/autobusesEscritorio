using Autobuses.Utilerias;
using ConnectDB;
using MyAttendance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Planeacion
{
    public partial class Corridas : Form
    {
        public TimeSpan TIEMPO_CARGA = TimeSpan.FromMinutes(10);
        public const int OPERACION_GENERAR_CORRIDAS = 0;
        public const int OPERACION_ACTUALIZAR_DETALLE = 1;
        public const int OPERACION_GENERAR_EXTRA = 2;
        public bool CARGA_DATOS = true;
        private int contando = 0;
        private string contraseña = "";
        public bool CARGA_DATOS_EXTRA = true;
        public int PK_CORRIDA_COMPLETO = -1;
        public string OLD_AUTOBUS = "";
        public string OLD_HORARIO = "";
        public int operacion = -1;
        public database db;
        ResultSet res = null;
        int n = 0;
        byte[] fingerPrint;
        Form mensaje;
        string mostrarMensaje = "";


        public Corridas()
        {

            InitializeComponent();
            //this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
            titulo.Text = "Corridas";
            this.Show();
            btnMaximizar.Visible = false;
            btnNormal.Visible = true;
            progressBar1.Hide();

            mensaje = new Mensaje("", true);


            if (LoginInfo.privilegios.Any(x => x == "Generar corridas"))
            {
                btnGenerar.Visible = true;
            }
            if (LoginInfo.privilegios.Any(x => x == "Guardar escala"))
            {
                btnGuardarEscala.Visible = true;
            }
            if (LoginInfo.privilegios.Any(x => x == "Agregar extra"))
            {
                btnAddExtra.Visible = true;
            }

            controles(0);
            controles(2);
            controles(4);


        }

        private void DoubleBuffered(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
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
        public void getRows(string origen = "", string fecha = "", string pkRol = "", string pkLinea = "")
        {
            /*
            string sql = "select ROW_NUMBER()over( order by PK asc) as 'no',[PK],[PK_LINEA],[LINEA],[PK_ROL],[ROL],[PK_CORRIDA],[NO_CORRIDA],[CORRIDA_DESCRIPCION],[PK_AUTOBUS],[AUTOBUS],[PK_ORIGEN],[ORIGEN],[SALIDA],[PK_DESTINO],[DESTINO],[LLEGADA],[ESCALA],[FECHA],[PK_RUTA],[RUTA],[PK_CORRIDA_RUTA],[BLOQUEADO],[GUIA],[FECHA_C],[FECHA_M],[USUARIO] " +
                " FROM VCORRIDAS_DIA_2 WHERE COMPLETO=1 ";*/
            setValueProgressBarDelegate(80);

            /*
            string sql = "select ROW_NUMBER()over( order by PK asc) as 'no',[PK_LINEA],[LINEA],[PK_ROL],[ROL],[PK_CORRIDA],[NO_CORRIDA],[CORRIDA_DESCRIPCION],[PK_AUTOBUS],[AUTOBUS],[PK_ORIGEN],[ORIGEN],[SALIDA],[PK_DESTINO_COMPLETO]AS PK_DESTINO,[DESTINO_COMPLETO]AS DESTINO,[LLEGADA],[ESCALA],[FECHA],[PK_RUTA],[RUTA],[PK_CORRIDA_RUTA],[BLOQUEADO],[GUIA] " +
                         " FROM VCORRIDAS_DIA_1 WHERE ESCALA=1 ";*/
            string sql = "SELECT ROW_NUMBER()over( order by SALIDA asc) as 'no',PK_LINEA,LINEA,PK_ROL,ROL,PK_CORRIDA,NO_CORRIDA,CORRIDA_DESCRIPCION,PK_AUTOBUS,AUTOBUS," +
                         " PK_ORIGEN,ORIGEN,SALIDA,PK_DESTINO_COMPLETO PK_DESTINO,DESTINO_COMPLETO DESTINO,LLEGADA_COMPLETO LLEGADA," +
                         " ESCALA,PK_RUTA,RUTA,FECHA,PK_CORRIDA_RUTA,BLOQUEADO,GUIA " +
                         " FROM VCORRIDAS_DIA_1 WHERE 1=1 ";

            string sql2 = "select COUNT(PK) MAX FROM VCORRIDAS_DIA_1 WHERE 1=1 ";


            if (!string.IsNullOrEmpty(origen))
            {
                sql += " AND PK_ORIGEN = " + origen;
            }
            if (!string.IsNullOrEmpty(fecha))
            {
                sql += " AND FECHA = '" + fecha + "'";
                sql2 += " AND FECHA = '" + fecha + "'";
            }
            if (!string.IsNullOrEmpty(pkRol))
            {
                sql += " AND PK_ROL = " + pkRol;
                sql2 += " AND PK_ROL = " + pkRol;
            }
            if (!string.IsNullOrEmpty(pkLinea))
            {
                sql += " AND PK_LINEA = " + pkLinea;
                sql2 += " AND PK_LINEA = " + pkLinea;
            }

            sql += " GROUP BY PK_LINEA,LINEA,PK_ROL," +
                         " ROL,PK_CORRIDA,NO_CORRIDA,CORRIDA_DESCRIPCION,PK_AUTOBUS,AUTOBUS,PK_ORIGEN,ORIGEN,SALIDA," +
                         " PK_DESTINO_COMPLETO,DESTINO_COMPLETO,LLEGADA_COMPLETO,ESCALA,PK_RUTA,RUTA,FECHA," +
                         " PK_CORRIDA_RUTA,BLOQUEADO,GUIA ORDER BY SALIDA ";
            /*
            //db.PreparedSQL(sql2);
            db.command.Parameters.AddWithValue("@FECHA", fecha);
            db.command.Parameters.AddWithValue("@ROL", pkRol);
            db.command.Parameters.AddWithValue("@LINEA", pkLinea);*/
            //ResultSet res1 = db.getTable(sql2);
            if (db.Count(sql2) > 0)
            {
                if (btnGenerar.InvokeRequired)
                {
                    btnGenerar.Invoke(new Action(() => { btnGenerar.Enabled = false; btnGenerar.BackColor = Color.White; }));
                }
                else { btnGenerar.Enabled = false; btnGenerar.BackColor = Color.White; }
            }
            else
            {
                if (btnGenerar.InvokeRequired)
                {
                    btnGenerar.Invoke(new Action(() => { btnGenerar.Enabled = true; btnGenerar.BackColor = Color.FromArgb(38, 45, 56); }));
                }
                else { btnGenerar.Enabled = true; btnGenerar.BackColor = Color.FromArgb(38, 45, 56); }
            }


            /*
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@ORIGEN", origen);
            db.command.Parameters.AddWithValue("@FECHA", fecha);
            db.command.Parameters.AddWithValue("@ROL", pkRol);
            db.command.Parameters.AddWithValue("@LINEA", pkLinea);

            ResultSet res = db.getTable();
            */
            bool rows = false;

            if (dataGridView0.InvokeRequired)
            {
                setValueProgressBarDelegate(90);
                dataGridView0.Invoke(new Action(() => rows = dataGridView0.Rows != null));
                dataGridView0.Invoke(new Action(() =>
                {
                    dataGridView0.DataSource = Populate(sql);
                }
                ));
                setValueProgressBarDelegate(100);

            }
            else
            {
                try
                {
                    if (dataGridView0.Rows != null)
                    {
                        dataGridView0.Rows.Clear();
                    }
                }
                catch { }
                setValueProgressBarDelegate(90);
                dataGridView0.DataSource = Populate(sql);
                setValueProgressBarDelegate(100);
            }

            //limpiaGrid0
            controles(6);

        }

        public void getRowsOLD(string origen = "", string fecha = "", string pkRol = "", string pkLinea = "")
        {
            string sql = "select * FROM VCORRIDAS_DIA_1 WHERE COMPLETO=1 ";
            string sql2 = "select * FROM VCORRIDAS_DIA_1 WHERE 1=1 ";

            if (!string.IsNullOrEmpty(origen))
            {
                sql += " AND PK_ORIGEN = @ORIGEN";
            }
            if (!string.IsNullOrEmpty(fecha))
            {
                sql += " AND FECHA = @FECHA";
                sql2 += " AND FECHA = @FECHA";
            }
            if (!string.IsNullOrEmpty(pkRol))
            {
                sql += " AND PK_ROL = @ROL";
                sql2 += " AND PK_ROL = @ROL";
            }
            if (!string.IsNullOrEmpty(pkLinea))
            {
                sql += " AND PK_LINEA = @LINEA";
                sql2 += " AND PK_LINEA = @LINEA";
            }

            db.PreparedSQL(sql2);
            db.command.Parameters.AddWithValue("@FECHA", fecha);
            db.command.Parameters.AddWithValue("@ROL", pkRol);
            db.command.Parameters.AddWithValue("@LINEA", pkLinea);
            ResultSet res1 = db.getTable();
            if (res1.HasRows)
            {
                if (btnGenerar.InvokeRequired)
                {
                    btnGenerar.Invoke(new Action(() => btnGenerar.Enabled = false));
                }
                else { btnGenerar.Enabled = false; }
            }
            else
            {
                if (btnGenerar.InvokeRequired)
                {
                    btnGenerar.Invoke(new Action(() => btnGenerar.Enabled = true));
                }
                else { btnGenerar.Enabled = true; }
            }

            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@ORIGEN", origen);
            db.command.Parameters.AddWithValue("@FECHA", fecha);
            db.command.Parameters.AddWithValue("@ROL", pkRol);
            db.command.Parameters.AddWithValue("@LINEA", pkLinea);

            ResultSet res = db.getTable();
            bool rows = false;

            if (dataGridView0.InvokeRequired)
            {
                dataGridView0.Invoke(new Action(() => rows = dataGridView0.Rows != null));
                try
                {
                    if (rows)
                    {
                        dataGridView0.Invoke(new Action(() => dataGridView0.Rows.Clear()));
                    }
                }
                catch { }
                if (res.HasRows)
                {
                    while (res.Next())
                    {

                        dataGridView0.Invoke(new Action(() =>
                        {
                            n = dataGridView0.Rows.Add();
                            dataGridView0.Rows[n].Cells["pk"].Value = res.Get("PK");
                            dataGridView0.Rows[n].Cells["no"].Value = n + 1;
                            dataGridView0.Rows[n].Cells["pkLinea"].Value = res.Get("PK_LINEA");
                            dataGridView0.Rows[n].Cells["linea"].Value = res.Get("LINEA");
                            dataGridView0.Rows[n].Cells["pkRol"].Value = res.Get("PK_ROL");
                            dataGridView0.Rows[n].Cells["rol"].Value = res.Get("ROL");
                            dataGridView0.Rows[n].Cells["pkCorrida"].Value = res.Get("PK_CORRIDA");
                            dataGridView0.Rows[n].Cells["noCorrida"].Value = res.Get("NO_CORRIDA");
                            dataGridView0.Rows[n].Cells["corridaDescripcion"].Value = res.Get("CORRIDA_DESCRIPCION");
                            dataGridView0.Rows[n].Cells["pkAutobus"].Value = res.Get("PK_AUTOBUS");
                            dataGridView0.Rows[n].Cells["autobus"].Value = res.Get("AUTOBUS");
                            dataGridView0.Rows[n].Cells["pkOrigen"].Value = res.Get("PK_ORIGEN");
                            dataGridView0.Rows[n].Cells["origen"].Value = res.Get("ORIGEN");
                            dataGridView0.Rows[n].Cells["salida"].Value = res.Get("SALIDA");
                            dataGridView0.Rows[n].Cells["pkDestino"].Value = res.Get("PK_DESTINO");
                            dataGridView0.Rows[n].Cells["destino"].Value = res.Get("DESTINO");
                            dataGridView0.Rows[n].Cells["llegada"].Value = res.Get("LLEGADA");
                            dataGridView0.Rows[n].Cells["escala"].Value = res.Get("ESCALA");
                            //dataGridView0.Rows[n].Cells["rutaDescripcion"].Value = res.Get("RUTA_DESCRIPCION");
                            dataGridView0.Rows[n].Cells["pkRuta"].Value = res.Get("PK_RUTA");
                            dataGridView0.Rows[n].Cells["rutaDescripcion"].Value = res.Get("RUTA");
                            dataGridView0.Rows[n].Cells["fecha"].Value = res.Get("FECHA");
                            dataGridView0.Rows[n].Cells["pkCorridaRuta"].Value = res.Get("PK_CORRIDA_RUTA");
                            dataGridView0.Rows[n].Cells["bloqueado"].Value = res.Get("BLOQUEADO");
                            dataGridView0.Rows[n].Cells["guia"].Value = res.Get("GUIA");
                            dataGridView0.Rows[n].Cells["fecha_c"].Value = res.Get("FECHA_C");
                            dataGridView0.Rows[n].Cells["fecha_m"].Value = res.Get("FECHA_M");
                            dataGridView0.Rows[n].Cells["usuario"].Value = res.Get("USUARIO");
                        }
                        ));

                    }
                }
            }
            else
            {
                try
                {
                    if (dataGridView0.Rows != null)
                    {
                        dataGridView0.Rows.Clear();
                    }
                }
                catch { }
                if (res.HasRows)
                {
                    while (res.Next())
                    {
                        n = dataGridView0.Rows.Add();

                        dataGridView0.Rows[n].Cells["pk"].Value = res.Get("PK");
                        dataGridView0.Rows[n].Cells["no"].Value = n + 1;
                        dataGridView0.Rows[n].Cells["pkLinea"].Value = res.Get("PK_LINEA");
                        dataGridView0.Rows[n].Cells["linea"].Value = res.Get("LINEA");
                        dataGridView0.Rows[n].Cells["pkRol"].Value = res.Get("PK_ROL");
                        dataGridView0.Rows[n].Cells["rol"].Value = res.Get("ROL");
                        dataGridView0.Rows[n].Cells["pkCorrida"].Value = res.Get("PK_CORRIDA");
                        dataGridView0.Rows[n].Cells["noCorrida"].Value = res.Get("NO_CORRIDA");
                        dataGridView0.Rows[n].Cells["corridaDescripcion"].Value = res.Get("CORRIDA_DESCRIPCION");
                        dataGridView0.Rows[n].Cells["pkAutobus"].Value = res.Get("PK_AUTOBUS");
                        dataGridView0.Rows[n].Cells["autobus"].Value = res.Get("AUTOBUS");
                        dataGridView0.Rows[n].Cells["pkOrigen"].Value = res.Get("PK_ORIGEN");
                        dataGridView0.Rows[n].Cells["origen"].Value = res.Get("ORIGEN");
                        dataGridView0.Rows[n].Cells["salida"].Value = res.Get("SALIDA");
                        dataGridView0.Rows[n].Cells["pkDestino"].Value = res.Get("PK_DESTINO");
                        dataGridView0.Rows[n].Cells["destino"].Value = res.Get("DESTINO");
                        dataGridView0.Rows[n].Cells["llegada"].Value = res.Get("LLEGADA");
                        dataGridView0.Rows[n].Cells["escala"].Value = res.Get("ESCALA");
                        //dataGridView0.Rows[n].Cells["rutaDescripcion"].Value = res.Get("RUTA_DESCRIPCION");
                        dataGridView0.Rows[n].Cells["pkRuta"].Value = res.Get("PK_RUTA");
                        dataGridView0.Rows[n].Cells["rutaDescripcion"].Value = res.Get("RUTA");
                        dataGridView0.Rows[n].Cells["fecha"].Value = res.Get("FECHA");
                        dataGridView0.Rows[n].Cells["pkCorridaRuta"].Value = res.Get("PK_CORRIDA_RUTA");
                        dataGridView0.Rows[n].Cells["bloqueado"].Value = res.Get("BLOQUEADO");
                        dataGridView0.Rows[n].Cells["guia"].Value = res.Get("GUIA");
                        /*
                        dataGridView0.Rows[n].Cells["fecha_c"].Value = res.Get("FECHA_C");
                        dataGridView0.Rows[n].Cells["fecha_m"].Value = res.Get("FECHA_M");
                        dataGridView0.Rows[n].Cells["usuario"].Value = res.Get("USUARIO");
                        */
                    }
                }
            }
        }

        public void comboLineaAddItemDelegate(ComboboxItem item)
        {
            if (comboLinea.InvokeRequired) { comboLinea.Invoke(new Action(() => comboLinea.Items.Add(item))); }
            else { comboLinea.Items.Add(item); }
        }
        public void comboLineaSetSelectIndexDelegate(int index)
        {
            if (comboLinea.InvokeRequired) { comboLinea.Invoke(new Action(() => comboLinea.SelectedIndex = index)); }
            else { comboLinea.SelectedIndex = index; }
        }
        public void comboRolAddItemDelegate(ComboboxItem item)
        {
            if (comboRol.InvokeRequired) { comboRol.Invoke(new Action(() => comboRol.Items.Add(item))); }
            else { comboRol.Items.Add(item); }
        }
        public void comboRolSetSelectIndexDelegate(int index)
        {
            if (comboRol.InvokeRequired) { comboRol.Invoke(new Action(() => comboRol.SelectedIndex = index)); }
            else { comboRol.SelectedIndex = index; }
        }
        public void comboEscalaAddItemDelegate(ComboboxItem item)
        {
            if (comboEscala.InvokeRequired) { comboEscala.Invoke(new Action(() => comboEscala.Items.Add(item))); }
            else { comboEscala.Items.Add(item); }
        }
        public void comboEscalaSetSelectIndexDelegate(int index)
        {
            if (comboEscala.InvokeRequired) { comboEscala.Invoke(new Action(() => comboEscala.SelectedIndex = index)); }
            else { comboEscala.SelectedIndex = index; }
        }
        public void comboLinea1AddItemDelegate(ComboboxItem item)
        {
            if (comboLinea1.InvokeRequired) { comboLinea1.Invoke(new Action(() => comboLinea1.Items.Add(item))); }
            else { comboLinea1.Items.Add(item); }
        }
        public void comboLinea1SetSelectIndexDelegate(int index)
        {
            if (comboLinea1.InvokeRequired) { comboLinea1.Invoke(new Action(() => comboLinea1.SelectedIndex = index)); }
            else { comboLinea1.SelectedIndex = index; }
        }
        public void comboRol1AddItemDelegate(ComboboxItem item)
        {
            if (comboRol1.InvokeRequired) { comboRol1.Invoke(new Action(() => comboRol1.Items.Add(item))); }
            else { comboRol1.Items.Add(item); }
        }
        public void comboRol1SetSelectIndexDelegate(int index)
        {
            if (comboRol1.InvokeRequired) { comboRol1.Invoke(new Action(() => comboRol1.SelectedIndex = index)); }
            else { comboRol1.SelectedIndex = index; }
        }
        public void comboRuta1AddItemDelegate(ComboboxItem item)
        {
            if (comboRuta1.InvokeRequired) { comboRuta1.Invoke(new Action(() => comboRuta1.Items.Add(item))); }
            else { comboRuta1.Items.Add(item); }
        }
        public void comboRuta1SetSelectIndexDelegate(int index)
        {
            if (comboRuta1.InvokeRequired) { comboRuta1.Invoke(new Action(() => comboRuta1.SelectedIndex = index)); }
            else { comboRuta1.SelectedIndex = index; }
        }

        public void setValueProgressBarDelegate(int valor) {
            if (progressBar1.InvokeRequired)
            {

                progressBar1.Invoke(new Action(() => { progressBar1.Value = valor; }));
            }
            else
            {
                progressBar1.Value = valor;
            }
        }


        /*OPCION 0 LINEAS OPCION 1 ROLADOS*/
        public void getDatosAdicionales(int opc = 0, string linea = "", string rol = "")
        {
            setValueProgressBarDelegate(30);

            string sql = "SELECT * FROM LINEAS WHERE BORRADO=0 ";
            try
            {
                if (opc == 0)
                {
                    try
                    {
                        comboLinea.Items.Clear();
                    }
                    catch { }
                    setValueProgressBarDelegate(35);

                    db.PreparedSQL(sql);
                    res = db.getTable();

                    ComboboxItem item = new ComboboxItem();
                    item.Text = "TODAS";
                    item.Value = "";
                    comboLineaAddItemDelegate(item);
                    setValueProgressBarDelegate(40);

                    while (res.Next())
                    {
                        item = new ComboboxItem();
                        item.Text = res.Get("LINEA");
                        item.Value = res.Get("PK1");
                        comboLineaAddItemDelegate(item);

                        //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                    }
                    setValueProgressBarDelegate(45);

                    /*
                    if (comboLinea != null && comboLinea.Items.Count > 0)
                    {
                        comboLineaSetSelectIndexDelegate(0);
                    }*/

                }
                else if (opc == 1)
                {
                    setValueProgressBarDelegate(30);

                    sql = "SELECT * FROM ROLADOS WHERE BORRADO=0 ";
                    try
                    {
                        comboRol.Items.Clear();
                    }
                    catch { }
                    setValueProgressBarDelegate(35);

                    if (!String.IsNullOrEmpty(linea))
                    {
                        sql += " AND PK_LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@LINEA", linea);
                    }
                    else
                    {
                        db.PreparedSQL(sql);
                    }

                    res = db.getTable();
                    setValueProgressBarDelegate(40);

                    ComboboxItem item = new ComboboxItem();
                    item.Text = "Todos";
                    item.Value = "";
                    comboRolAddItemDelegate(item);

                    while (res.Next())
                    {
                        item = new ComboboxItem();
                        item.Text = res.Get("ROL");
                        item.Value = res.Get("PK");
                        comboRolAddItemDelegate(item);

                        //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                    }
                    setValueProgressBarDelegate(45);
                    comboRolSetSelectIndexDelegate(-1);
                    comboRol.Text = "";
                    comboEscalaSetSelectIndexDelegate(-1);
                    comboEscala.Text = "";
                    //limpiar datagrid0
                    controles(7);
                    /*
                    if (comboRol != null && comboRol.Items.Count > 0)
                    {
                        comboRolSetSelectIndexDelegate(0);
                    }*/
                }
                else if (opc == 2)
                {
                    try
                    {
                        if (comboEscala != null && comboEscala.Items.Count > 0)
                            comboEscala.Items.Clear();
                    }
                    catch { }
                    setValueProgressBarDelegate(30);

                    sql = "SELECT * FROM VISTAROLPRIV where PRIV IS NOT NULL AND id = " + LoginInfo.pkidroles + " and P=(SELECT PK1 FROM PRIVILEGIOS WHERE PRIVILEGIO='Listar todos las escalas')";
                    db.PreparedSQL(sql);
                    ResultSet res = db.getTable();
                    ComboboxItem item = new ComboboxItem();
                    if (res.HasRows)
                    {
                        sql = "SELECT * FROM DESTINOS WHERE BORRADO=0 ";
                        item.Text = "Todas";
                        item.Value = "";
                        comboEscalaAddItemDelegate(item);
                    }
                    else
                    {
                        sql = "SELECT * FROM DESTINOS WHERE BORRADO=0 AND DESTINO='" + LoginInfo.Sucursal + "'";
                    }
                    setValueProgressBarDelegate(35);

                    sql += " ORDER BY DESTINO";

                    db.PreparedSQL(sql);

                    res = db.getTable();

                    while (res.Next())
                    {
                        item = new ComboboxItem();
                        item.Text = res.Get("DESTINO");
                        item.Value = res.Get("PK1");
                        comboEscalaAddItemDelegate(item);

                        //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                    }
                    setValueProgressBarDelegate(40);

                    /*
                    if (comboEscala != null && comboEscala.Items.Count > 0) {
                        comboEscalaSetSelectIndexDelegate(0);
                    }*/
                }
                else if (opc == 3)
                {
                    sql = "Select * from LINEAS WHERE BORRADO=0 ";
                    try
                    {
                        if (comboLinea1.Items != null)
                            comboLinea1.Items.Clear();
                    }
                    catch
                    { }
                    setValueProgressBarDelegate(30);

                    db.PreparedSQL(sql);
                    res = db.getTable();

                    while (res.Next())
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = res.Get("LINEA");
                        item.Value = res.Get("PK1");
                        comboLinea1AddItemDelegate(item);

                        //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                    }
                    setValueProgressBarDelegate(40);

                }
                else if (opc == 4)
                {
                    sql = "SELECT * FROM ROLADOS WHERE BORRADO=0 ";
                    try
                    {
                        if (comboRol1.Items != null)
                            comboRol1.Items.Clear();
                    }
                    catch { }
                    setValueProgressBarDelegate(30);

                    if (!String.IsNullOrEmpty(linea))
                    {
                        sql += " AND PK_LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@LINEA", linea);
                    }
                    else
                    {
                        db.PreparedSQL(sql);
                    }

                    res = db.getTable();
                    setValueProgressBarDelegate(40);

                    while (res.Next())
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = res.Get("ROL");
                        item.Value = res.Get("PK");
                        comboRol1AddItemDelegate(item);

                        //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                    }
                    setValueProgressBarDelegate(40);

                }
                else if (opc == 5)
                {
                    try
                    {
                        if (comboRuta1.Items != null)
                            comboRuta1.Items.Clear();
                    }
                    catch { }
                    setValueProgressBarDelegate(30);

                    sql = "SELECT * FROM RUTAS WHERE BORRADO=0 ";

                    if (!String.IsNullOrEmpty(linea))
                    {
                        sql += " AND LINEA_PK=@LINEA ";
                    }

                    string origen = (comboEscala.SelectedItem != null) && (comboEscala.SelectedItem as ComboboxItem).Text != null ? (comboEscala.SelectedItem as ComboboxItem).Text : "";

                    if (!String.IsNullOrEmpty(origen))
                    {
                        sql += " AND PK IN(SELECT PK_RUTA FROM VRUTAS_DESTINOS WHERE ORIGEN = @ORIGEN AND COMPLETO=1 GROUP BY PK_RUTA) ";
                    }

                    sql += " ORDER BY RUTA ASC";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@ORIGEN", origen);
                    db.command.Parameters.AddWithValue("@LINEA", linea);


                    setValueProgressBarDelegate(40);

                    res = db.getTable();

                    while (res.Next())
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = res.Get("RUTA");
                        item.Value = res.Get("PK");
                        comboRuta1AddItemDelegate(item);

                        //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                    }
                    comboRuta1.Text = "";
                    controles(2);
                    setValueProgressBarDelegate(45);

                }
            }
            catch (Exception e)
            {
                LOG.write("ROLADO", "getDatosAdicionales", e.Message);
            }
        }

        private void ComboLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            progressBar1.Show();
            progressBar1.Value = 20;

            string linea = (comboLinea.SelectedItem != null) ? (comboLinea.SelectedItem as ComboboxItem).Value.ToString() : "";
            //if (!string.IsNullOrEmpty(linea))
            //{

                try
                {
                    dataGridView0.Rows.Clear();
                }
                catch { }

                getDatosAdicionales(1, linea);
                /*
                if (backgroundWorker2.IsBusy)
                {
                    backgroundWorker2.CancelAsync();
                }
                backgroundWorker2.RunWorkerAsync();
                comboLinea.Focus();
                */
                progressBar1.Value = 50;
                if (comboLinea.SelectedItem != null && comboRol.SelectedItem != null && comboEscala.SelectedItem != null)
                {
                    recargaGrid();
                }
            //}
            progressBar1.Hide();
            progressBar1.Value = 0;


        }

        private void ComboRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            progressBar1.Show();
            setValueProgressBarDelegate(20);
            string rol = (comboRol.SelectedItem as ComboboxItem).Value.ToString();
            //dataGridView0.Rows.Clear();
            /*
            if (backgroundWorker2.IsBusy)
            {
                backgroundWorker2.CancelAsync();
            }
            backgroundWorker2.RunWorkerAsync();
            */
            setValueProgressBarDelegate(50);
            if (comboLinea.SelectedItem != null && comboRol.SelectedItem != null && comboEscala.SelectedItem != null)
            {
                recargaGrid();
            }

            progressBar1.Hide();
            setValueProgressBarDelegate(0);
        }

        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            operacion = OPERACION_GENERAR_CORRIDAS;
            //verificationUserControl1.Show();
            groupBoxHuella.Show();
            verificationUserControl1.Focus();
            ValidateUser();

        }

        public void generaCorridasDia()
        {

            DateTime dtIni = dTPicker.Value.Date;
            DateTime dtfin = dTPicker2.Value.Date;
            TimeSpan ts = dtfin - dtIni;
            string sql = string.Empty;
            string escala = string.Empty;
            string linea = string.Empty; string rol = string.Empty; string fe;
            if (comboEscala.InvokeRequired)
            {
                //ComboboxItem ITE=(ComboboxItem)comboEscala.SelectedItem;
                comboEscala.Invoke(new Action(() => escala = comboEscala.SelectedItem != null ? (comboEscala.SelectedItem as ComboboxItem).Value.ToString() : ""));
            }
            else
            {
                escala = (comboEscala.SelectedItem as ComboboxItem).Value.ToString();
            }
            if (comboLinea.InvokeRequired)
            {
                comboLinea.Invoke(new Action(() => {
                    if (comboLinea.SelectedItem == null)
                    {
                        MessageBox.Show("Selecciona el rol a generar"); comboLinea.Focus();
                        return;
                    }
                    linea = (comboLinea.SelectedItem as ComboboxItem).Value.ToString();
                }));
            }
            else
            {
                linea = (comboLinea.SelectedItem as ComboboxItem).Value.ToString();
            }
            if (comboRol.InvokeRequired)
            {
                comboRol.Invoke(new Action(() => {
                    if (comboRol.SelectedItem == null) {
                        MessageBox.Show("Selecciona el rol a generar"); comboRol.Focus();
                        return;
                    }
                    rol = (comboRol.SelectedItem as ComboboxItem).Value.ToString();
                }
                ));
            }
            else
            {
                rol = (comboRol.SelectedItem as ComboboxItem).Value.ToString();
            }
            if (dTPicker.InvokeRequired)
            {
                dTPicker.Invoke(new Action(() => fe = dTPicker.Value.Date.ToString("yyyy-MM-dd")));
            }
            else
            {
                fe = dTPicker.Value.Date.ToString("yyyy-MM-dd");
            }

            List<string> Lista = new List<string>();

            for (int i = 0; i <= ts.Days; i++)
            {
                fe = dtIni.AddDays(i).ToString("yyyy-MM-dd");

                sql = "select * from CORRIDAS_DIA WHERE PK_LINEA = @LINEA AND PK_ROL = @ROL AND CONVERT(VARCHAR(10),FECHA,120)= @FECHA";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@LINEA", linea);
                db.command.Parameters.AddWithValue("@ROL", rol);
                db.command.Parameters.AddWithValue("@FECHA", fe);
                ResultSet resultSet = db.getTable();

                if (!resultSet.HasRows)
                {
                    Lista.Add(fe);
                    sql = "CREATE_ROLADO_DIARIO";
                    List<Parametros> lista = new List<Parametros>();

                    Parametros pa = new Parametros();
                    pa.nombreParametro = "@LINEA";
                    pa.tipoParametro = SqlDbType.BigInt;
                    pa.direccion = ParameterDirection.Input;
                    pa.value = linea;
                    lista.Add(pa);

                    pa = new Parametros();
                    pa.nombreParametro = "@ROL";
                    pa.tipoParametro = SqlDbType.BigInt;
                    pa.direccion = ParameterDirection.Input;
                    pa.value = rol;
                    lista.Add(pa);

                    pa = new Parametros();
                    pa.nombreParametro = "@FECHA";
                    pa.tipoParametro = SqlDbType.Date;
                    pa.direccion = ParameterDirection.Input;
                    pa.value = fe;
                    lista.Add(pa);

                    pa = new Parametros();
                    pa.nombreParametro = "@USUARIO";
                    pa.tipoParametro = SqlDbType.VarChar;
                    pa.direccion = ParameterDirection.Input;
                    pa.value = LoginInfo.UserID;
                    lista.Add(pa);

                    db.ExecuteStoreProcedure(sql, lista);
                    getRows(escala, fe, rol, linea);
                }
            }

            MessageBox.Show("¡Fechas Roladas!\n" + String.Join("\n", Lista));
            //verificationUserControl1.Hide();
            //verificationUserControl1.Samples.Clear();
        }

        void ValidateUser()
        {
            fingerPrint = db.selectByteArray("SELECT HUELLA FROM USUARIOS WHERE USUARIO='" + LoginInfo.UserID + "'");
            verificationUserControl1.Samples.Add(new DPFP.Template(new MemoryStream(fingerPrint)), null);
        }

        private void ToolStripButton9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolStripButton8_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.cerrarSesion(this);
        }

        private void DataGridView0_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int n = e.RowIndex;
            if (n == -1) return;
            controles(1);
            int pkRol = 0;
            string fecha = "";
            fecha = (dataGridView0.Rows[n].Cells["fecha"].Value != null) ?
                 dataGridView0.Rows[n].Cells["fecha"].Value.ToString() : "";
            pkRol = (dataGridView0.Rows[n].Cells["pkRol"].Value != null) ?
                 int.Parse(dataGridView0.Rows[n].Cells["pkRol"].Value.ToString()) : -1;
            int pkLinea = (dataGridView0.Rows[n].Cells["pkLinea"].Value != null) ?
                int.Parse(dataGridView0.Rows[n].Cells["pkLinea"].Value.ToString()) : -1;
            int pkRuta = (dataGridView0.Rows[n].Cells["pkRuta"].Value != null) ?
                int.Parse(dataGridView0.Rows[n].Cells["pkRuta"].Value.ToString()) : -1;
            int pkCorridaRuta = (dataGridView0.Rows[n].Cells["pkCorridaRuta"].Value != null) ?
                int.Parse(dataGridView0.Rows[n].Cells["pkCorridaRuta"].Value.ToString()) : -1;

            //PK_ORIGEN=@ORIGEN AND
            /*string sql = "select  CD1.*,(SELECT COUNT (*) FROM VENDIDOS VE WHERE VE.PKCORRIDA= CD1.PK) VENDIDOS," +
                         "case when CD1.COMPLETO = 1 THEN  (SELECT count(*) ASIENTOS FROM DESCTIPOAUTOBUS WHERE PKTIPOAUTOBUS=2011 and OBJETO='asiento')  WHEN CD1.COMPLETO=0 THEN 0 END as ASIENTOS " +
                         " FROM VCORRIDAS_DIA_1 CD1 " +
                         " WHERE PK_RUTA=@RUTA " +
                         " AND PK_CORRIDA_RUTA=@PKCORRIDARUTA " +
                         " AND FECHA=@FECHA " +
                         " AND PK_ROL=@ROL " +
                         " AND PK_LINEA=@LINEA " +
                         " ORDER BY PK_LINEA,PK_ROL,NO_CORRIDA,SALIDA ASC";
            */
            string sql = @"select  CD1.*,(SELECT COUNT (*) FROM VENDIDOS VE WHERE VE.PKCORRIDA= CD1.PK) VENDIDOS,
                            case when CD1.COMPLETO = 1 THEN  (SELECT count(*) ASIENTOS FROM DESCTIPOAUTOBUS WHERE PKTIPOAUTOBUS=2011 and OBJETO='asiento')  
                            WHEN CD1.COMPLETO=0 THEN 0 END as ASIENTOS  
                            ,VRD.TIEMPO
                            FROM VCORRIDAS_DIA_1 CD1  
                            INNER JOIN VRUTAS_DETALLE VRD ON(VRD.PK =CD1.PK_RUTA AND VRD.PK_ORIGEN=CD1.PK_ORIGEN AND VRD.PK_DESTINO=CD1.PK_DESTINO AND VRD.LINEA_PK=CD1.PK_LINEA)
                            WHERE CD1.PK_RUTA=@RUTA  AND CD1.PK_CORRIDA_RUTA=@PKCORRIDARUTA  AND CD1.FECHA=@FECHA  AND CD1.PK_ROL=@ROL  AND CD1.PK_LINEA=@LINEA  
                            ORDER BY CD1.COMPLETO DESC";

            db.PreparedSQL(sql);
            //db.command.Parameters.AddWithValue("@ORIGEN", origen);
            db.command.Parameters.AddWithValue("@FECHA", fecha);
            db.command.Parameters.AddWithValue("@ROL", pkRol);
            db.command.Parameters.AddWithValue("@LINEA", pkLinea);
            db.command.Parameters.AddWithValue("@RUTA", pkRuta);
            db.command.Parameters.AddWithValue("@PKCORRIDARUTA", pkCorridaRuta);

            ResultSet res = db.getTable();
            int n1;
            try
            {
                if (dataGridView1 != null && dataGridView1.Rows != null) { dataGridView1.Rows.Clear(); }
            }
            catch { }

            CARGA_DATOS = false;
            while (res.Next())
            {

                n1 = dataGridView1.Rows.Add();
                dataGridView1.Rows[n1].Cells["pk1"].Value = res.Get("PK");
                dataGridView1.Rows[n1].Cells["no1"].Value = n1 + 1;
                dataGridView1.Rows[n1].Cells["pkAutobus1"].Value = res.Get("PK_AUTOBUS");
                dataGridView1.Rows[n1].Cells["autobus1"].Value = res.Get("AUTOBUS");
                dataGridView1.Rows[n1].Cells["autobusOld1"].Value = res.Get("AUTOBUS");
                dataGridView1.Rows[n1].Cells["pkOrigen1"].Value = res.Get("PK_ORIGEN");
                dataGridView1.Rows[n1].Cells["origen1"].Value = res.Get("ORIGEN");
                dataGridView1.Rows[n1].Cells["pkDestino1"].Value = res.Get("PK_DESTINO");
                dataGridView1.Rows[n1].Cells["destino1"].Value = res.Get("DESTINO");
                dataGridView1.Rows[n1].Cells["salida1"].Value = res.Get("SALIDA");
                dataGridView1.Rows[n1].Cells["salidaOld1"].Value = res.Get("SALIDA");
                dataGridView1.Rows[n1].Cells["llegada1"].Value = res.Get("LLEGADA");
                dataGridView1.Rows[n1].Cells["llegadaOld1"].Value = res.Get("LLEGADA");
                dataGridView1.Rows[n1].Cells["completo1"].Value = res.Get("COMPLETO");
                if (bool.TryParse(res.Get("COMPLETO"), out bool val)) {
                    if (val) {
                        PK_CORRIDA_COMPLETO = res.GetInt("PK");
                    }
                }
                dataGridView1.Rows[n1].Cells["e1"].Value = res.Get("ESCALA");
                dataGridView1.Rows[n1].Cells["eOld1"].Value = res.Get("ESCALA");
                dataGridView1.Rows[n1].Cells["b1"].Value = res.Get("BLOQUEADO");
                dataGridView1.Rows[n1].Cells["bOld1"].Value = res.Get("BLOQUEADO");
                dataGridView1.Rows[n1].Cells["d"].Value = res.Get("VENDIDOS");
                dataGridView1.Rows[n1].Cells["a"].Value = res.Get("ASIENTOS");
                dataGridView1.Rows[n1].Cells["o"].Value = res.Get("ASIENTOS");
                dataGridView1.Rows[n1].Cells["tiempo1"].Value = res.Get("TIEMPO");
                dataGridView1.Rows[n1].Cells["fecha1"].Value = res.Get("FECHA");

            }
            CARGA_DATOS = true;

            if ( PK_CORRIDA_COMPLETO > -1)
            {
                getDetallesActividadCorrida();
            }

        }

        private void DTPicker_ValueChanged(object sender, EventArgs e)
        {
            dTPicker2.MinDate = dTPicker.Value;
            dTPicker2.Value = dTPicker.Value;
            if (comboLinea.SelectedItem != null && comboRol.SelectedItem != null && comboEscala.SelectedItem != null)
            {
                backgroundWorker2.RunWorkerAsync();
            }
            //recargaGrid();
        }

        private void ComboEscala_SelectedIndexChanged(object sender, EventArgs e)
        {
            progressBar1.Show();
            setValueProgressBarDelegate(20);
            try
            {
                if (comboRuta1.Items != null) { comboRuta1.Items.Clear(); }
            }
            catch { }
            try
            {
                if (dataGridView2.Rows != null) { dataGridView2.Rows.Clear(); }
            }
            catch { }
            string linea = comboLinea1.SelectedItem != null && (comboLinea1.SelectedItem as ComboboxItem).Value != null ?
                (comboLinea1.SelectedItem as ComboboxItem).Value.ToString() : "";
            try
            {
                if (dataGridView2.Rows != null) { dataGridView2.Rows.Clear(); }
            }
            catch { }

            getDatosAdicionales(5, linea);
            /*
            if (backgroundWorker2.IsBusy) {
                backgroundWorker2.CancelAsync();
            }
            backgroundWorker2.RunWorkerAsync();
            */
            setValueProgressBarDelegate(50);
            if (comboLinea.SelectedItem != null && comboRol.SelectedItem != null && comboEscala.SelectedItem != null)
            {
                recargaGrid();
            }
            progressBar1.Hide();
            progressBar1.Value = 0;
        }

        private void TabControl1_Click(object sender, EventArgs e)
        {
            getDatosAdicionales(3);
            comboRol1.Items.Clear();
            comboRol1.Text = "";
            comboRuta1.Items.Clear();
            comboRuta1.Text = "";
            dataGridView2.Rows.Clear();
            btnAddExtra.Enabled = false;
            btnAddExtra.BackColor = Color.White;

            if (tabControl1.SelectedTab.Name.Equals("detallesInfo") && PK_CORRIDA_COMPLETO >-1) {
                getDetallesActividadCorrida();
            }
            
        }

        public void getDetallesActividadCorrida() {

            string sql = "SELECT * FROM VLOG_CORRIDA WHERE PK_CORRIDA_DIA="+PK_CORRIDA_COMPLETO+" ORDER BY FECHA_C DESC";

            dataGridViewDetallesActividad.DataSource= db.Populate(sql);

        }

        private void ComboLinea1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string linea = (comboLinea1.SelectedItem as ComboboxItem).Value != null ?
                (comboLinea1.SelectedItem as ComboboxItem).Value.ToString() : "";
            try { if (dataGridView2.Rows != null) { dataGridView2.Rows.Clear(); } }
            catch { }
            try { if (comboRuta1.Items != null) { comboRuta1.Items.Clear(); } }
            catch { }
            comboRuta1.Text = "";

            getDatosAdicionales(4, linea);
        }

        private void ComboRol1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string linea = (comboLinea1.SelectedItem as ComboboxItem).Value != null ?
                (comboLinea1.SelectedItem as ComboboxItem).Value.ToString() : "";
            try { if (dataGridView2.Rows != null) { dataGridView2.Rows.Clear(); } }
            catch { }
            getDatosAdicionales(5, linea);

        }

        private void ComboRuta1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ruta = comboRuta1.SelectedItem != null && (comboRuta1.SelectedItem as ComboboxItem).Value != null ?
                (comboRuta1.SelectedItem as ComboboxItem).Value.ToString() : "";
            try { if (dataGridView2.Rows != null) { dataGridView2.Rows.Clear(); } }
            catch { }
            getDatosRutas(ruta);
        }

        public void getDatosRutas(string ruta = "")
        {
            string sql = "SELECT * FROM VRUTAS_DESTINOS";

            if (!string.IsNullOrEmpty(ruta))
            {
                sql += " WHERE PK_RUTA=@RUTA";
            }
            sql += " ORDER BY COMPLETO DESC";
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@RUTA", ruta);
            ResultSet res = db.getTable();
            int n = -1;
            CARGA_DATOS_EXTRA = false;
            while (res.Next())
            {
                n = dataGridView2.Rows.Add();
                dataGridView2.Rows[n].Cells["tiempo2"].Value = res.Get("TIEMPO");
                dataGridView2.Rows[n].Cells["salida2"].Value = "00:00";
                dataGridView2.Rows[n].Cells["llegada2"].Value = sumaHoras("00:00", res.Get("TIEMPO"));
                dataGridView2.Rows[n].Cells["pkRuta2"].Value = res.Get("PK_RUTA");
                dataGridView2.Rows[n].Cells["pkOrigen2"].Value = res.Get("PK_ORIGEN");
                dataGridView2.Rows[n].Cells["origen2"].Value = res.Get("ORIGEN");
                dataGridView2.Rows[n].Cells["pkDestino2"].Value = res.Get("PK_DESTINO");
                dataGridView2.Rows[n].Cells["destino2"].Value = res.Get("DESTINO");
                dataGridView2.Rows[n].Cells["completo2"].Value = res.Get("COMPLETO");
                dataGridView2.Rows[n].Cells["escala2"].Value = true;


            }
            CARGA_DATOS_EXTRA = true;

        }

        public string sumaHoras(string tiempo1, string tiempo2)
        {

            TimeSpan salida = TimeSpan.Parse(tiempo1);
            TimeSpan tiempo = TimeSpan.Parse(tiempo2);
            salida = salida.Add(tiempo);


            return salida.ToString(@"hh\:mm");
        }

        private void DataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int n2 = e.RowIndex;
            string stringValue = "";
            int valor = -1;
            
            if (n2 > -1 && e.ColumnIndex == dataGridView2.Rows[n2].Cells["salida2"].ColumnIndex && CARGA_DATOS_EXTRA)
            {
                CARGA_DATOS_EXTRA = false;
                if (dataGridView2.Rows[n2].Cells["salida2"].Value != null)
                    stringValue = dataGridView2.Rows[n2].Cells["salida2"].Value.ToString();

                if (stringValue != null && stringValue.Contains(":")) { stringValue = stringValue.Replace(":", ""); }

                if (!int.TryParse(stringValue, out valor))
                {
                    //btnAddExtra.Enabled = false;
                    MessageBox.Show("¡Error por favor introducir una hora valida!");
                    stringValue = "";
                }

                if (stringValue != null && stringValue.Length == 4)
                {
                    dataGridView2.Rows[n2].Cells["salida2"].Value = stringValue.Substring(0, 2) + ":" + stringValue.Substring(2, 2);
                    dataGridView2.Rows[n2].Cells["llegada2"].Value = sumaHoras(dataGridView2.Rows[n2].Cells["salida2"].Value.ToString(), dataGridView2.Rows[n2].Cells["tiempo2"].Value.ToString());
                }
                else if (stringValue != null && stringValue.Length == 3)
                {
                    dataGridView2.Rows[n2].Cells["salida2"].Value = "0" + stringValue.Substring(0, 1) + ":" + stringValue.Substring(1, 2);
                    dataGridView2.Rows[n2].Cells["llegada2"].Value = sumaHoras(dataGridView2.Rows[n2].Cells["salida2"].Value.ToString(), dataGridView2.Rows[n2].Cells["tiempo2"].Value.ToString());

                }
                else if (stringValue != null && stringValue.Length == 2)
                {
                    dataGridView2.Rows[n2].Cells["salida2"].Value = stringValue + ":00";
                    dataGridView2.Rows[n2].Cells["llegada2"].Value = sumaHoras(dataGridView2.Rows[n2].Cells["salida2"].Value.ToString(), dataGridView2.Rows[n2].Cells["tiempo2"].Value.ToString());

                }
                else if (stringValue != null && stringValue.Length == 1)
                {
                    dataGridView2.Rows[n2].Cells["salida2"].Value = "0" + stringValue + ":00";
                    dataGridView2.Rows[n2].Cells["llegada2"].Value = sumaHoras(dataGridView2.Rows[n2].Cells["salida2"].Value.ToString(), dataGridView2.Rows[n2].Cells["tiempo2"].Value.ToString());

                }
                else if (stringValue != null && stringValue.Length == 0)
                {
                    dataGridView2.Rows[n2].Cells["salida2"].Value = "00:00";
                    dataGridView2.Rows[n2].Cells["llegada2"].Value = sumaHoras(dataGridView2.Rows[n2].Cells["salida2"].Value.ToString(), dataGridView2.Rows[n2].Cells["tiempo2"].Value.ToString());

                }

                if (!TimeSpan.TryParse(dataGridView2.Rows[n2].Cells["salida2"].Value.ToString(), out TimeSpan tiempo))
                {
                    dataGridView1.Rows[n2].Cells["salida2"].Value = "00:00";
                    MessageBox.Show("¡Por favor introducir una hora valida!");
                    CARGA_DATOS_EXTRA = true;
                    return;
                }

                if (n2 == 0)
                {
                    Dictionary<string, int> destinos = new Dictionary<string, int>();

                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        try
                        {
                            if (row.Cells["pkOrigen2"].Value.ToString().Equals(dataGridView2.Rows[0].Cells["pkOrigen2"].Value.ToString()))
                            {
                                row.Cells["salida2"].Value = dataGridView2.Rows[0].Cells["salida2"].Value;
                                TimeSpan.TryParse(row.Cells["tiempo2"].Value.ToString(), out TimeSpan timeTemp);
                                row.Cells["llegada2"].Value = timeTemp.Add(tiempo).ToString(@"hh\:mm");
                                destinos.Add(row.Cells["pkDestino2"].Value.ToString(), row.Index);
                            }
                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                        }
                    }

                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        try
                        {
                            if (destinos.ContainsKey(row.Cells["pkOrigen2"].Value.ToString()))
                            {
                                int index = destinos[row.Cells["pkOrigen2"].Value.ToString()];
                                TimeSpan.TryParse(dataGridView2.Rows[index].Cells["llegada2"].Value.ToString(), out TimeSpan timeTemp);
                                row.Cells["salida2"].Value = timeTemp.Add(TIEMPO_CARGA).ToString(@"hh\:mm");

                                TimeSpan.TryParse(row.Cells["tiempo2"].Value.ToString(), out TimeSpan timeTemp2);
                                row.Cells["llegada2"].Value = timeTemp.Add(timeTemp2).ToString(@"hh\:mm");
                            }
                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                        }
                    }
                }

                CARGA_DATOS_EXTRA = true;
            }
            if (n2 > -1 && e.ColumnIndex == dataGridView2.Rows[n2].Cells["llegada2"].ColumnIndex && CARGA_DATOS_EXTRA)
            {
                CARGA_DATOS_EXTRA = false;
                stringValue = "";
                if (dataGridView2.Rows[n2].Cells["llegada2"].Value != null)
                    stringValue = dataGridView2.Rows[n2].Cells["llegada2"].Value.ToString();
                if (stringValue != null && stringValue.Contains(":")) { stringValue = stringValue.Replace(":", ""); }

                if (!int.TryParse(stringValue, out valor))
                {
                    //btnAddExtra.Enabled = false;
                    MessageBox.Show("¡Error por favor introducir una hora valida!");
                    stringValue = "";
                }

                if (stringValue != null && stringValue.Length == 4)
                {
                    dataGridView2.Rows[n2].Cells["llegada2"].Value = stringValue.Substring(0, 2) + ":" + stringValue.Substring(2, 2);
                }
                else if (stringValue != null && stringValue.Length == 3)
                {
                    dataGridView2.Rows[n2].Cells["llegada2"].Value = "0" + stringValue.Substring(0, 1) + ":" + stringValue.Substring(1, 2);
                }
                else if (stringValue != null && stringValue.Length == 2)
                {
                    dataGridView2.Rows[n2].Cells["llegada2"].Value = stringValue + ":00";
                }
                else if (stringValue != null && stringValue.Length == 1)
                {
                    dataGridView2.Rows[n2].Cells["llegada2"].Value = "0" + stringValue + ":00";
                }
                else if (stringValue != null && stringValue.Length == 0)
                {
                    dataGridView2.Rows[n2].Cells["llegada2"].Value = "00:00";
                }
                CARGA_DATOS_EXTRA = true;
            }
            else if (n2 > -1 && e.ColumnIndex == dataGridView2.Rows[n2].Cells["autobus2"].ColumnIndex && CARGA_DATOS_EXTRA)
            {
                CARGA_DATOS_EXTRA = false;
                if (n2 == 0)
                {
                    int pkAutobus = -1;
                    string pkautobus = "";
                    string sqlPkAutobus = "select * FROM AUTOBUSES WHERE ECO=@ECO";
                    db.PreparedSQL(sqlPkAutobus);
                    if (dataGridView2.Rows[n2].Cells["autobus2"].Value != null)
                    {
                        pkautobus = dataGridView2.Rows[n2].Cells["autobus2"].Value.ToString();
                    }
                    db.command.Parameters.AddWithValue("@ECO", pkautobus);
                    ResultSet res = db.getTable();
                    if (res.Next()) { pkAutobus = int.Parse(res.Get("PK1")); }
                    if (pkAutobus == -1)
                    {
                        dataGridView2.Rows[n2].Cells["pkAutobus2"].Value = "";
                        MessageBox.Show("¡Error por favor introducir un autobus valido!");
                        if (dataGridView2.Rows[0].Cells["pkAutobus2"].Value != null && !string.IsNullOrEmpty(dataGridView2.Rows[0].Cells["pkAutobus2"].Value.ToString()))
                        {
                            btnAddExtra.Enabled = true;
                            btnAddExtra.BackColor = Color.FromArgb(38, 45, 56);
                        }
                        else
                        {
                            btnAddExtra.Enabled = false;
                            btnAddExtra.BackColor = Color.White;

                        }
                        CARGA_DATOS_EXTRA = true;
                        return;
                    }
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        row.Cells["pkAutobus2"].Value = pkAutobus;
                        row.Cells["autobus2"].Value = dataGridView2.Rows[n2].Cells["autobus2"].Value;
                    }
                    if (dataGridView2.Rows[0].Cells["pkAutobus2"].Value != null && !string.IsNullOrEmpty(dataGridView2.Rows[0].Cells["pkAutobus2"].Value.ToString()))
                    {
                        btnAddExtra.Enabled = true;
                        btnAddExtra.BackColor = Color.FromArgb(38, 45, 56);

                    }
                    else
                    {
                        btnAddExtra.Enabled = false;
                        btnAddExtra.BackColor = Color.White;

                    }
                }
                CARGA_DATOS_EXTRA = true;
            }
            else if (n2 > -1 && e.ColumnIndex == dataGridView2.Rows[n2].Cells["bloqueado2"].ColumnIndex && CARGA_DATOS_EXTRA)
            {
                CARGA_DATOS_EXTRA = false;
                if (n2 == 0)
                {
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        row.Cells["bloqueado2"].Value = dataGridView2.Rows[n2].Cells["bloqueado2"].Value;
                    }
                }
                CARGA_DATOS_EXTRA = true;
            }
        }

        private void BtnAddExtra_Click(object sender, EventArgs e)
        {
            //verificationUserControl1.Show();
            bool existe = false;
            string pklinea = "";
            string siExiste = "";
            string sqlexisteUnion = "";
            ComboboxItem item = null;
            if (comboLinea1.InvokeRequired)
            {
                comboLinea1.Invoke(new Action(() => item = (ComboboxItem)comboLinea1.SelectedItem));
                pklinea = item != null && item.Value != null ? item.Value.ToString() : "";
            }
            else
            {
                pklinea = (comboLinea1.SelectedItem != null && (comboLinea1.SelectedItem as ComboboxItem).Value != null) ?
                                (comboLinea1.SelectedItem as ComboboxItem).Value.ToString() : "";
            }
            string sqlExiste = "SELECT CONCAT('LINEA:',LINEA,', AUTOBUS:',AUTOBUS,', ORIGEN:',ORIGEN,', DESTINO:',DESTINO,', SALIDA:',SALIDA,', FECHA:',FECHA) MENSAJE " +
                               " FROM VCORRIDAS_DIA_1 WHERE PK_LINEA = @PKLINEA AND PK_AUTOBUS = @PKAUTOBUS " +
                               " AND PK_ORIGEN = @PKORIGEN AND SALIDA = '@SALIDA' AND PK_DESTINO = @PKDESTINO AND FECHA = '@FECHA'";

            int pkAutobus = -1;
            int pkOrigen = -1;
            string salida = "";
            int pkDestino = -1;
            string fecha = DateTime.Now.ToString("yyyy-MM-dd");

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                pkAutobus = row.Cells["pkAutobus2"].Value != null ? int.Parse(row.Cells["pkAutobus2"].Value.ToString()) : -1;
                pkOrigen = row.Cells["pkOrigen2"].Value != null ? int.Parse(row.Cells["pkOrigen2"].Value.ToString()) : -1;
                salida = row.Cells["salida2"].Value != null ? row.Cells["salida2"].Value.ToString() : "";
                pkDestino = row.Cells["pkDestino2"].Value != null ? int.Parse(row.Cells["pkDestino2"].Value.ToString()) : -1;

                sqlexisteUnion = sqlExiste;
                sqlexisteUnion = sqlexisteUnion.Replace("@PKLINEA", pklinea);
                sqlexisteUnion = sqlexisteUnion.Replace("@PKAUTOBUS", "" + pkAutobus);
                sqlexisteUnion = sqlexisteUnion.Replace("@PKORIGEN", "" + pkOrigen);
                sqlexisteUnion = sqlexisteUnion.Replace("@SALIDA", salida);
                sqlexisteUnion = sqlexisteUnion.Replace("@PKDESTINO", "" + pkDestino);
                sqlexisteUnion = sqlexisteUnion.Replace("@FECHA", fecha);

                ResultSet consultaResult = db.getTable(sqlexisteUnion);
                if (consultaResult.Next())
                {
                    existe = true;
                    if (string.IsNullOrEmpty(siExiste))
                    {
                        siExiste = consultaResult.Get("MENSAJE");
                    }
                    else
                    {
                        siExiste += "\n" + consultaResult.Get("MENSAJE");
                    }
                }
            }

            if (!existe)
            {
                operacion = OPERACION_GENERAR_EXTRA;
                groupBoxHuella.Show();
                verificationUserControl1.Focus();
                ValidateUser();
            }
            else {
                Form mensaje = new Mensaje("No se puede agregar extra debido a que uno o más horarios ya existen:\n" + siExiste, true,false);
                mensaje.ShowDialog();
            }

        }

        public void addExtra()
        {
            string salida_completo = "00:00", llegada_completo = "00:00";
            int origen_completo = -1, destino_completo = -1;
            List<string> pkList = new List<string>();
            string pklinea = string.Empty, pkrol = string.Empty, pkRuta = string.Empty;
            ComboboxItem item = null;
            string sql = "INSERT INTO CORRIDAS_DIA" +
                        "(PK_LINEA, PK_ROL, PK_CORRIDA, NO_CORRIDA, CORRIDA_DESCRIPCION," +
                        "PK_AUTOBUS, PK_ORIGEN, SALIDA, PK_DESTINO, LLEGADA, ESCALA, FECHA, " +
                        "PK_RUTA, PK_CORRIDA_RUTA,BLOQUEADO,COMPLETO,USUARIO) " +
                        "VALUES(@PKlINEA,@PKROL," +
                        "@PKCORRIDA,@NOCORRIDA," +
                        "@CORRIDADES,@PKAUTOBUS,@PKORIGEN,@SALIDA,@PKDESTINO,@LLEGADA,@ESCALA,@FECHA," +
                        "@PKRUTA,@PKCORRIDARUTA,@BLOQUEADO,@COMPLETO,@USUARIO)";

            if (comboLinea1.InvokeRequired)
            {
                comboLinea1.Invoke(new Action(() => item = (ComboboxItem)comboLinea1.SelectedItem));
                pklinea = item != null && item.Value != null ? item.Value.ToString() : "";
            }
            else
            {
                pklinea = (comboLinea1.SelectedItem != null && (comboLinea1.SelectedItem as ComboboxItem).Value != null) ?
                                (comboLinea1.SelectedItem as ComboboxItem).Value.ToString() : "";
            }
            if (comboRol1.InvokeRequired)
            {
                comboRol1.Invoke(new Action(() => item = (ComboboxItem)comboRol1.SelectedItem));
                pkrol = item != null && item.Value != null ? item.Value.ToString() : "";
            }
            else
            {
                pkrol = (comboRol1.SelectedItem != null && (comboRol1.SelectedItem as ComboboxItem).Value != null) ?
                        (comboRol1.SelectedItem as ComboboxItem).Value.ToString() : "";
            }
            string sqlpkCorrida = "(select max(PK_CORRIDA)+1 as CORRIDA FROM CORRIDAS_DIA WHERE FECHA='" + DateTime.Now.ToString("yyyy - MM - dd") + "')";
            int pkCorrida = -1;
            string sqlNoCorrida = "(select max(NO_CORRIDA)+1 AS NOCORRIDA FROM CORRIDAS_DIA WHERE FECHA='" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            int noCorrida = -1;
            string corridaDes = "EXTRA";
            int pkAutobus = -1;
            int pkOrigen = -1;
            string salida = "";
            int pkDestino = -1;
            string llegada = "";
            string escala = "";
            string fecha = DateTime.Now.ToString("yyyy-MM-dd");
            if (comboRuta1.InvokeRequired)
            {
                comboRuta1.Invoke(new Action(() => item = (ComboboxItem)comboRuta1.SelectedItem));
                pkRuta = item != null && item.Value != null ? item.Value.ToString() : "";
            }
            else
            {
                pkRuta = (comboRuta1.SelectedItem != null && (comboRuta1.SelectedItem as ComboboxItem).Value != null) ?
                            (comboRuta1.SelectedItem as ComboboxItem).Value.ToString() : "";
            }

            string sqlPkCorridaRuta = "select max(pk_corrida_ruta)+1 as PKCORRIDARUTA from CORRIDAS_DIA where fecha='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            int pkCorridaRuta = -1;
            string bloqueado = "false";
            string completo = "false";

            db.PreparedSQL(sqlpkCorrida);
            ResultSet res = db.getTable();
            if (res.Next()) { pkCorrida = int.Parse(res.Get("CORRIDA")); }

            db.PreparedSQL(sqlNoCorrida);
            res = db.getTable();
            if (res.Next()) { noCorrida = int.Parse(res.Get("NOCORRIDA")); }

            db.PreparedSQL(sqlPkCorridaRuta);
            res = db.getTable();
            if (res.Next()) { pkCorridaRuta = int.Parse(res.Get("PKCORRIDARUTA")); }

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {

                pkAutobus = row.Cells["pkAutobus2"].Value != null ? int.Parse(row.Cells["pkAutobus2"].Value.ToString()) : -1;
                pkOrigen = row.Cells["pkOrigen2"].Value != null ? int.Parse(row.Cells["pkOrigen2"].Value.ToString()) : -1;
                salida = row.Cells["salida2"].Value != null ? row.Cells["salida2"].Value.ToString() : "";
                pkDestino = row.Cells["pkDestino2"].Value != null ? int.Parse(row.Cells["pkDestino2"].Value.ToString()) : -1;
                llegada = row.Cells["llegada2"].Value != null ? row.Cells["llegada2"].Value.ToString() : "";
                escala = row.Cells["escala2"].Value != null ? row.Cells["escala2"].Value.ToString() : "false";
                bloqueado = row.Cells["bloqueado2"].Value != null ? row.Cells["bloqueado2"].Value.ToString() : "false";
                completo = row.Cells["completo2"].Value != null ? row.Cells["completo2"].Value.ToString() : "false";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PKlINEA", pklinea);
                db.command.Parameters.AddWithValue("@PKROL", pkrol);
                db.command.Parameters.AddWithValue("@PKCORRIDA", pkCorrida);
                db.command.Parameters.AddWithValue("@NOCORRIDA", noCorrida);
                db.command.Parameters.AddWithValue("@CORRIDADES", corridaDes);
                db.command.Parameters.AddWithValue("@PKAUTOBUS", pkAutobus);
                db.command.Parameters.AddWithValue("@PKORIGEN", pkOrigen);
                db.command.Parameters.AddWithValue("@SALIDA", salida);
                db.command.Parameters.AddWithValue("@PKDESTINO", pkDestino);
                db.command.Parameters.AddWithValue("@LLEGADA", llegada);
                db.command.Parameters.AddWithValue("@ESCALA", escala);
                db.command.Parameters.AddWithValue("@FECHA", fecha);
                db.command.Parameters.AddWithValue("@PKRUTA", pkRuta);
                db.command.Parameters.AddWithValue("@PKCORRIDARUTA", pkCorridaRuta);
                db.command.Parameters.AddWithValue("@BLOQUEADO", bloqueado);
                db.command.Parameters.AddWithValue("@COMPLETO", completo);
                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                pkList.Add(db.executeId());
                if (Boolean.Parse(completo))
                {
                    salida_completo = salida;
                    llegada_completo = llegada;
                    origen_completo = pkOrigen;
                    destino_completo = pkDestino;
                    PK_CORRIDA_COMPLETO = int.Parse(pkList.Last());
                }
            }

            if (pkList.Count > 0)
            {
                string sqlupdate = "UPDATE CORRIDAS_DIA SET PK_ORIGEN_COMPLETO=@PKORIGENCOMPLETO,SALIDA_COMPLETO=@SALIDACOMPLETO," +
                                   " PK_DESTINO_COMPLETO=@PKDESTINOCOMPLETO,LLEGADA_COMPLETO=@LLEGADACOMPLETO " +
                                   " WHERE PK IN(" + String.Join(",", pkList.ToArray()) + ")";
                db.PreparedSQL(sqlupdate);
                db.command.Parameters.AddWithValue("@PKORIGENCOMPLETO", origen_completo);
                db.command.Parameters.AddWithValue("@SALIDACOMPLETO", salida_completo);
                db.command.Parameters.AddWithValue("@PKDESTINOCOMPLETO", destino_completo);
                db.command.Parameters.AddWithValue("@LLEGADACOMPLETO", llegada_completo);
                db.execute();

                string sqlLog = "INSERT INTO LOG_CORRIDAS (PK_CORRIDA_DIA,FUNCION,DESCRIPCION,USUARIO,SUCURSAL)" +
                                " VALUES(@CORRIDA,@FUNCION,(SELECT CONCAT('se creo un extra con ruta: ', RUTA)DESCRIPCION FROM RUTAS WHERE PK =@PKRUTA),@USUARIO,@SUCURSAL)";

                db.PreparedSQL(sqlLog);
                db.command.Parameters.AddWithValue("@CORRIDA", PK_CORRIDA_COMPLETO);
                db.command.Parameters.AddWithValue("@FUNCION", "Creaciòn de extra");
                db.command.Parameters.AddWithValue("@PKRUTA", pkRuta);
                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                db.command.Parameters.AddWithValue("@SUCURSAL", LoginInfo.Sucursal);
                db.execute();

            }

            if (btnAddExtra.InvokeRequired) { btnAddExtra.Invoke(new Action(() => { btnAddExtra.Enabled = false; btnAddExtra.BackColor = Color.White; })); } else { btnAddExtra.Enabled = false; btnAddExtra.BackColor = Color.White; }
            if (comboLinea1.InvokeRequired) { comboLinea1.Invoke(new Action(() => comboLinea1.Text = "")); } else { comboLinea1.Text = ""; }
            if (comboRol1.InvokeRequired) { comboRol1.Invoke(new Action(() => comboRol1.Text = "")); } else { comboRol1.Text = ""; }
            if (comboRuta1.InvokeRequired) { comboRuta1.Invoke(new Action(() => comboRuta1.Text = "")); } else { comboRuta1.Text = ""; }

            try
            {
                if (dataGridView2.InvokeRequired) { dataGridView2.Invoke(new Action(() => dataGridView2.Rows.Clear())); }
                else { if (dataGridView2.Rows != null) { dataGridView2.Rows.Clear(); } }
            }
            catch { }

            //backgroundWorker2.RunWorkerAsync();
            recargaGrid();

        }

        public void verMensaje() {
            mensaje = new Mensaje(mostrarMensaje, true);
            mensaje.ShowDialog();
        }


        private void BtnGuardarEscala_Click(object sender, EventArgs e)
        {
            verificationUserControl1.limpiarhuella();
            operacion = OPERACION_ACTUALIZAR_DETALLE;
            //verificationUserControl1.Show();
            groupBoxHuella.Show();
            verificationUserControl1.Focus();
            ValidateUser();
            controles(0);
        }

        public void guardarEscala()
        {
            string sqlUpdateBoletos = "";
            string noModificadas = "";
            string salidaCompleto = "00:00", llegadaCompleto = "00:00";
            string noEncontrados = "";
            List<string> erroresHoras = new List<string>();
            bool horasValidate = true;
            string sql = "UPDATE CORRIDAS_DIA SET PK_AUTOBUS=@PKAUTOBUS,SALIDA=@SALIDA,LLEGADA=@LLEGADA," +
                         " ESCALA=@ESCALA,BLOQUEADO=@BLOQUEADO,SALIDA_COMPLETO=@SALIDACOMPLETO," +
                         " LLEGADA_COMPLETO=@LLEGADACOMPLETO, USUARIO=@USUARIO WHERE PK=@PK";
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                

                string completo = row.Cells["completo1"].Value != null ? row.Cells["completo1"].Value.ToString() : "false";
                string salida1 = row.Cells["salida1"].Value.ToString();
                string llegada1 = row.Cells["llegada1"].Value.ToString();

                if (!TimeSpan.TryParse(salida1, out TimeSpan result))
                {
                    horasValidate = false;
                    erroresHoras.Add("'" + salida1 + "'");
                    //MessageBox.Show("Error en formato Hora HH:mm '"+salida1+"'");
                }
                if (!TimeSpan.TryParse(llegada1, out TimeSpan result1))
                {
                    horasValidate = false;
                    erroresHoras.Add("'" + llegada1 + "'");
                    //MessageBox.Show("Error en formato Hora HH:mm '" + llegada1 + "'");
                }

                if (Boolean.Parse(completo))
                {
                    salidaCompleto = salida1;
                    llegadaCompleto = llegada1;
                }

            }

            if (horasValidate)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {

                    int pk = row.Cells["pk1"].Value != null ? int.Parse(row.Cells["pk1"].Value.ToString()) : -1;
                    int pkAutobusOLD = row.Cells["Autobus1"].Value != null ? int.Parse(row.Cells["pkAutobus1"].Value.ToString()) : -1;
                    int pkAutobus = row.Cells["Autobus1"].Value != null ? int.Parse(row.Cells["Autobus1"].Value.ToString()) : -1;
                    int pkAutobusEco = row.Cells["Autobus1"].Value != null ? int.Parse(row.Cells["Autobus1"].Value.ToString()) : -1;
                    string salida = row.Cells["salida1"].Value != null ? row.Cells["salida1"].Value.ToString() : "00:00";
                    string llegada = row.Cells["llegada1"].Value != null ? row.Cells["llegada1"].Value.ToString() : "00:00";
                    string escala = row.Cells["e1"].Value != null ? row.Cells["e1"].Value.ToString() : "false";
                    string bloqueado = row.Cells["b1"].Value != null ? row.Cells["b1"].Value.ToString() : "false";
                    
                    int autobusOld1 = row.Cells["autobusOld1"].Value != null ? int.Parse(row.Cells["autobusOld1"].Value.ToString()) : -1;
                    string salidaOld1 = row.Cells["salidaOld1"].Value != null ? row.Cells["salidaOld1"].Value.ToString() : "00:00";
                    string llegadaOld1 = row.Cells["llegadaOld1"].Value != null ? row.Cells["llegadaOld1"].Value.ToString() : "00:00";
                    string escalaOld1 = row.Cells["eOld1"].Value != null ? row.Cells["eOld1"].Value.ToString() : "false";
                    string bloqueadoOld1 = row.Cells["bOld1"].Value != null ? row.Cells["bOld1"].Value.ToString() : "false";
                    string origen = row.Cells["origen1"].Value != null ? row.Cells["origen1"].Value.ToString() : "";
                    string destino = row.Cells["destino1"].Value != null ? row.Cells["destino1"].Value.ToString() : "";
                    string fecha = row.Cells["fecha1"].Value != null ? row.Cells["fecha1"].Value.ToString() : "";

                    string sql3 = "SELECT GUIA FROM CORRIDAS_DIA WHERE PK=@PK ";
                    db.PreparedSQL(sql3);
                    db.command.Parameters.AddWithValue("PK",pk);
                    ResultSet res1 = db.getTable();
                    if (res1.Next())
                    {
                        bool guia = res1.GetBool("GUIA");
                        if (!guia)
                        {


                            string sql2 = "SELECT PK1 FROM AUTOBUSES WHERE ECO=@ECO";
                            string sql4 = "INSERT INTO LOG_CORRIDAS (PK_CORRIDA_DIA,FUNCION,DESCRIPCION,USUARIO,SUCURSAL)" +
                                          " VALUES(@CORRIDA,@FUNCION,@DESCRIPCION,@USUARIO,@SUCURSAL)";
                            string descripcion = "";
                            db.PreparedSQL(sql2);
                            db.command.Parameters.AddWithValue("@ECO", pkAutobusEco);
                            ResultSet res = db.getTable();
                            if (res.Next())
                            {
                                pkAutobus = res.GetInt("PK1");
                                db.PreparedSQL(sql);
                                db.command.Parameters.AddWithValue("@PKAUTOBUS", pkAutobus);
                                db.command.Parameters.AddWithValue("@SALIDA", salida);
                                db.command.Parameters.AddWithValue("@LLEGADA", llegada);
                                db.command.Parameters.AddWithValue("@ESCALA", escala);
                                db.command.Parameters.AddWithValue("@BLOQUEADO", bloqueado);
                                db.command.Parameters.AddWithValue("@SALIDACOMPLETO", salidaCompleto);
                                db.command.Parameters.AddWithValue("@LLEGADACOMPLETO", llegadaCompleto);
                                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                                db.command.Parameters.AddWithValue("@PK", pk);
                                db.execute();
                                if (!pkAutobusEco.Equals(autobusOld1)) {
                                    descripcion += " Cambio de autobus "+ autobusOld1 + " a "+ pkAutobusEco;
                                    executaProcedimientoCambioAutobus(pkAutobusOLD.ToString(),autobusOld1.ToString(),pk.ToString(),pkAutobus.ToString(),pkAutobusEco.ToString());

                                }
                                if (!salida.Equals(salidaOld1)) {
                                    descripcion += (!string.IsNullOrEmpty(descripcion)?",\n ":"")+"Cambio de salida de:" + salidaOld1 + " a " + salida;
                                    sqlUpdateBoletos += " UPDATE VENDIDOS SET HORA='"+salida+"', SALIDA='" + fecha + " " + salida + " ' WHERE PKCORRIDA =" + pk + ";";
                                }
                                if (!llegada.Equals(llegadaOld1)) {
                                    descripcion += (!string.IsNullOrEmpty(descripcion) ? ",\n " : "") + "Cambio de llegada de:" + llegadaOld1 + " a " + llegada;
                                    try
                                    {
                                        DateTime sal = DateTime.Parse(fecha + " " + salida);
                                        DateTime lle = DateTime.Parse(fecha + " " + llegada);
                                        if (sal > lle)
                                        {
                                            lle.AddDays(1);
                                        }
                                        sqlUpdateBoletos += " UPDATE VENDIDOS SET LLEGADA='" + lle.ToString("yyyy-MM-dd HH:mm") + " ' WHERE PKCORRIDA =" + pk + ";";
                                    }
                                    catch (Exception e) { erroresHoras.Add(e.Message); }
                                }
                                if (!escala.Equals(escalaOld1)) {
                                    descripcion += (!string.IsNullOrEmpty(descripcion) ? ",\n " : "") + "Cambio de escala de:" + escalaOld1 + " a " + escala;
                                }
                                if (!bloqueado.Equals(bloqueadoOld1)) {
                                    descripcion += (!string.IsNullOrEmpty(descripcion) ? ",\n " : "") + "Cambio de bloqueo de:" + escalaOld1 + " a " + escala;
                                }
                                if (!string.IsNullOrEmpty(descripcion)) {
                                    descripcion += ",\n Origen: "+origen+" Destino: "+destino + " corrida: "+pk;

                                    db.PreparedSQL(sql4);
                                    db.command.Parameters.AddWithValue("@CORRIDA", PK_CORRIDA_COMPLETO);
                                    db.command.Parameters.AddWithValue("@FUNCION", "Modifica corrida");
                                    db.command.Parameters.AddWithValue("@DESCRIPCION", descripcion);
                                    db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                                    db.command.Parameters.AddWithValue("@SUCURSAL", LoginInfo.Sucursal);
                                    db.execute();
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(noEncontrados))
                                {
                                    noEncontrados = "" + pkAutobus;
                                }
                                else
                                {
                                    noEncontrados = "," + pkAutobus;
                                }
                            }
                        }
                        else {
                            if (string.IsNullOrEmpty(noModificadas))
                            {
                                noModificadas += "" + pk;
                            }
                            else {
                                noModificadas += "," + pk;
                            }

                        }

                    }

                }
                if (!string.IsNullOrEmpty(sqlUpdateBoletos))
                {
                    db.PreparedSQL(sqlUpdateBoletos);
                    db.execute();
                }
            }
            else
            {
                MessageBox.Show("se encontraron errores en el formato de algunas horas, " +
                                 "por favor de corregirlas para poder guardar\n" +
                                 " " + string.Join(",", erroresHoras.ToArray()));
            }
            try
            {
                if (dataGridView1.InvokeRequired)
                {
                    if (dataGridView1.Rows != null) { dataGridView1.Invoke(new Action(() => dataGridView1.Rows.Clear())); }
                }
                else
                {
                    if (dataGridView1.Rows != null) { dataGridView1.Rows.Clear(); }
                }
            }
            catch { }

            //backgroundWorker2.RunWorkerAsync();
            recargaGrid();
            if (!string.IsNullOrEmpty(noEncontrados))
            {
                //MessageBox.Show("¡Autobuses " + noEncontrados + " no encontrados!");
                /*
                Mensaje msg = new Mensaje("¡Autobuses " + noEncontrados + " no encontrados!", true);
                msg.Show();*/
                if (textBoxAdvertencia.InvokeRequired)
                {
                    textBoxAdvertencia.Invoke(new Action(() =>
                    {
                        textBoxAdvertencia.Text += "\n¡Autobuses " + noEncontrados + " no encontrados!";
                    }));
                }
                else
                {
                    textBoxAdvertencia.Text += "\n¡Autobuses " + noEncontrados + " no encontrados!";
                }
            }

            if (!string.IsNullOrEmpty(noModificadas))
            {
                /*Mensaje msg = new Mensaje("¡Algunas corridas no se pudieron modificar ya que tenian guia generada!",true);
                msg.Show();*/
                if (textBoxAdvertencia.InvokeRequired)
                {
                    textBoxAdvertencia.Invoke(new Action(() =>
                    {
                        textBoxAdvertencia.Text += "\n¡Algunas corridas no se pudieron modificar ya que tenian guia generada!";
                        textBoxAdvertencia.AutoSize = true;
                    }));
                }
                else { 
                        textBoxAdvertencia.Text += "\n¡Algunas corridas no se pudieron modificar ya que tenian guia generada!";
                        textBoxAdvertencia.AutoSize = true;
                }

            }
            if (!string.IsNullOrEmpty(textBoxAdvertencia.Text))
            {
                if (panelAdvertencia.InvokeRequired)
                {
                    panelAdvertencia.Invoke(new Action(() =>
                    {
                        panelAdvertencia.AutoSize = true;
                        panelAdvertencia.Visible = true;

                    }));
                }
                else
                {
                    panelAdvertencia.Visible = true;
                }
            }

        }

        public bool executaProcedimientoCambioAutobus(string pkAutobusOld,string ecoAutobusOld,string pkcorrida, string pkAutobusNew, string ecoAutobusNew) {

            string sql = "SP_CAMBIO_AUTOBUS";
            List<Parametros> lista = new List<Parametros>();

            Parametros pa = new Parametros();
            pa.nombreParametro = "@PK_AUTOBUS_ACTUAL ";
            pa.tipoParametro = SqlDbType.VarChar;
            pa.direccion = ParameterDirection.Input;
            pa.value = pkAutobusOld;
            lista.Add(pa);

            pa = new Parametros();
            pa.nombreParametro = "@ECOAUTOBUS_ACTUAL";
            pa.tipoParametro = SqlDbType.VarChar;
            pa.direccion = ParameterDirection.Input;
            pa.value = ecoAutobusOld;
            lista.Add(pa);

            pa = new Parametros();
            pa.nombreParametro = "@PK_CORRIDA";
            pa.tipoParametro = SqlDbType.BigInt;
            pa.direccion = ParameterDirection.Input;
            pa.value = pkcorrida;
            lista.Add(pa);

            pa = new Parametros();
            pa.nombreParametro = "@PK_AUTOBUS_NUEVO";
            pa.tipoParametro = SqlDbType.VarChar;
            pa.direccion = ParameterDirection.Input;
            pa.value = pkAutobusNew;
            lista.Add(pa);
            
            pa = new Parametros();
            pa.nombreParametro = "@ECOAUTOBUS_NUEVO";
            pa.tipoParametro = SqlDbType.VarChar;
            pa.direccion = ParameterDirection.Input;
            pa.value = ecoAutobusNew;
            lista.Add(pa);

            pa = new Parametros();
            pa.nombreParametro = "@MENSAJE";
            pa.tipoParametro = SqlDbType.VarChar;
            pa.direccion = ParameterDirection.Output;
            pa.longitudParametro = 100;
            pa.value = ecoAutobusNew;
            lista.Add(pa);
            db.ExecuteStoreProcedure2(sql, lista);
            string error=db.command.Parameters["@MENSAJE"].Value.ToString();
            if (!string.IsNullOrEmpty(error))
            {
                if (textBoxAdvertencia.InvokeRequired) {
                    textBoxAdvertencia.Invoke(new Action(() => {
                        textBoxAdvertencia.Text = textBoxAdvertencia.Text + "\n" + error;

                    }));
                }
                else {
                    textBoxAdvertencia.Text = textBoxAdvertencia.Text+"\n"+ error;
                }
                

            }

            return true;

        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int n2 = e.RowIndex;

            if (n2 > -1 && e.ColumnIndex == dataGridView1.Rows[n2].Cells["salida1"].ColumnIndex && CARGA_DATOS)
            {
                CARGA_DATOS = false;
                string stringValue = "";
                if (dataGridView1.Rows[n2].Cells["salida1"].Value != null)
                    stringValue = dataGridView1.Rows[n2].Cells["salida1"].Value.ToString();

                dataGridView1.Rows[n2].Cells["salida1"].Value = calculoDeHoraTexto(stringValue);

                if (!TimeSpan.TryParse(dataGridView1.Rows[n2].Cells["salida1"].Value.ToString(), out TimeSpan tiempo))
                {
                    dataGridView1.Rows[n2].Cells["salida1"].Value = "00:00";
                    MessageBox.Show("¡Por favor introducir una hora valida!");
                    CARGA_DATOS = true;
                    return;
                }

                if (n2 == 0)
                {
                    Dictionary<string, int> destinos = new Dictionary<string, int>();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        try
                        {
                            if (row.Cells["pkOrigen1"].Value.ToString().Equals(dataGridView1.Rows[0].Cells["pkOrigen1"].Value.ToString()))
                            {
                                row.Cells["salida1"].Value = dataGridView1.Rows[0].Cells["salida1"].Value;
                                TimeSpan.TryParse(row.Cells["tiempo1"].Value.ToString(), out TimeSpan timeTemp);
                                row.Cells["llegada1"].Value = timeTemp.Add(tiempo).ToString(@"hh\:mm");
                                destinos.Add(row.Cells["pkDestino1"].Value.ToString(), row.Index);
                            }
                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                        }
                    }


                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        try
                        {
                            if (destinos.ContainsKey(row.Cells["pkOrigen1"].Value.ToString()))
                            {
                                int index = destinos[row.Cells["pkOrigen1"].Value.ToString()];
                                TimeSpan.TryParse(dataGridView1.Rows[index].Cells["llegada1"].Value.ToString(), out TimeSpan timeTemp);
                                row.Cells["salida1"].Value = timeTemp.Add(TIEMPO_CARGA).ToString(@"hh\:mm");

                                TimeSpan.TryParse(row.Cells["tiempo1"].Value.ToString(), out TimeSpan timeTemp2);
                                row.Cells["llegada1"].Value = timeTemp.Add(timeTemp2).ToString(@"hh\:mm");
                            }
                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                        }
                    }

                }
                CARGA_DATOS = true;
            }
            else if (n2 == 0 && e.ColumnIndex == dataGridView1.Rows[n2].Cells["autobus1"].ColumnIndex)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells["autobus1"].Value = dataGridView1.Rows[n2].Cells["autobus1"].Value;
                }
            }
            else if (n2 > -1 && e.ColumnIndex == dataGridView1.Rows[n2].Cells["llegada1"].ColumnIndex && CARGA_DATOS) { 
                CARGA_DATOS = false;
                string stringValue = "";
                if (dataGridView1.Rows[n2].Cells["llegada1"].Value != null)
                    stringValue = dataGridView1.Rows[n2].Cells["llegada1"].Value.ToString();

                dataGridView1.Rows[n2].Cells["llegada1"].Value=calculoDeHoraTexto(stringValue);

                if (!TimeSpan.TryParse(dataGridView1.Rows[n2].Cells["llegada1"].Value.ToString(), out TimeSpan tiempo))
                {
                    dataGridView1.Rows[n2].Cells["llegada1"].Value = "00:00";
                    MessageBox.Show("¡Por favor introducir una hora valida!");
                    CARGA_DATOS = true;
                    return;
                }

                Dictionary<string, int> destinos = new Dictionary<string, int>();
                TimeSpan auxTiempo1 = TimeSpan.Parse("-"+dataGridView1.Rows[n2].Cells["llegadaOld1"].Value.ToString());
                TimeSpan tiempoDiferencia = tiempo.Add(auxTiempo1);

                destinos.Add(dataGridView1.Rows[n2].Cells["pkDestino1"].Value.ToString(), n2);

                if (TimeSpan.TryParse(dataGridView1.Rows[n2].Cells["tiempo1"].Value.ToString(), out TimeSpan tiempoEscala))
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        try
                        {
                            if (row.Cells["pkOrigen1"].Value.ToString().Equals(dataGridView1.Rows[n2].Cells["pkOrigen1"].Value.ToString()) 
                                && TimeSpan.TryParse(row.Cells["tiempo1"].Value.ToString(), out TimeSpan tiempoViaje))
                            {
                                if (tiempoViaje > tiempoEscala)
                                {
                                    tiempoViaje= tiempoViaje.Add(tiempoDiferencia);
                                    TimeSpan.TryParse(row.Cells["salida1"].Value.ToString(), out TimeSpan timeSalida);
                                    row.Cells["llegada1"].Value = timeSalida.Add(tiempoViaje).ToString(@"hh\:mm");
                                    destinos.Add(row.Cells["pkDestino1"].Value.ToString(), row.Index);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                        }
                    }

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        try
                        {
                            if (destinos.ContainsKey(row.Cells["pkOrigen1"].Value.ToString()))
                            {
                                int index = destinos[row.Cells["pkOrigen1"].Value.ToString()];
                                TimeSpan.TryParse(dataGridView1.Rows[index].Cells["llegada1"].Value.ToString(), out TimeSpan timeTemp);
                                row.Cells["salida1"].Value = timeTemp.Add(TIEMPO_CARGA).ToString(@"hh\:mm");

                                TimeSpan.TryParse(row.Cells["tiempo1"].Value.ToString(), out TimeSpan timeTemp2);
                                row.Cells["llegada1"].Value = timeTemp.Add(timeTemp2).ToString(@"hh\:mm");
                            }
                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                        }
                    }

                }
                CARGA_DATOS = true;
            }
        }

        public string calculoDeHoraTexto(string stringValue) {
            
            string aux = stringValue;

            stringValue = stringValue.Replace(":", "");

            if (stringValue != null && stringValue.Length == 4)
            {
                aux = stringValue.Substring(0, 2) + ":" + stringValue.Substring(2, 2);
            }
            else if (stringValue != null && stringValue.Length == 3)
            {
                aux = "0" + stringValue.Substring(0, 1) + ":" + stringValue.Substring(1, 2);
            }
            else if (stringValue != null && stringValue.Length == 2)
            {
                aux = stringValue + ":00";
            }
            else if (stringValue != null && stringValue.Length == 1)
            {
                aux = "0" + stringValue + ":00";
            }
            else if (stringValue != null && stringValue.Length == 0)
            {
                aux = "00:00";
            }

            return aux;

        }

        private void MenuRefreshDetalle_Click(object sender, EventArgs e)
        {
            //backgroundWorker2.RunWorkerAsync();
            recargaGrid();
        }

        public void recargaGrid()
        {
            string origen = string.Empty, fecha = string.Empty, pkRol = string.Empty, pkLinea = string.Empty;
            ComboboxItem item = null;
            setValueProgressBarDelegate(60);

            if (comboEscala.InvokeRequired)
            {
                comboEscala.Invoke(new Action(() => {
                    comboEscala.Enabled = false;
                    item = (ComboboxItem)comboEscala.SelectedItem;
                }));
                origen = (item != null && item.Value != null) ? item.Value.ToString() : "";
            }
            else
            {
                comboEscala.Enabled = false;
                origen = (comboEscala != null && comboEscala.SelectedItem != null && (comboEscala.SelectedItem as ComboboxItem).Value != null) ? (comboEscala.SelectedItem as ComboboxItem).Value.ToString() : "";
            }
            setValueProgressBarDelegate(70);

            if (comboRol.InvokeRequired)
            {
                comboRol.Invoke(new Action(() => {
                    comboRol.Enabled = false;
                    item = (ComboboxItem)comboRol.SelectedItem;
                }));
                pkRol = (item != null && item.Value != null) ? item.Value.ToString() : "";
            }
            else
            {
                comboRol.Enabled = false;
                pkRol = (comboRol != null && comboRol.SelectedItem != null && (comboRol.SelectedItem as ComboboxItem).Value != null) ? (comboRol.SelectedItem as ComboboxItem).Value.ToString() : "";
            }
            setValueProgressBarDelegate(80);

            if (dTPicker.InvokeRequired)
            {
                dTPicker.Invoke(new Action(() => {
                    dTPicker.Enabled = false;
                    fecha = dTPicker.Value.Date.ToString("yyyy-MM-dd");
                }));
            }
            else
            {
                dTPicker.Enabled = false;
                fecha = dTPicker.Value.Date.ToString("yyyy-MM-dd");
            }
            if (comboLinea.InvokeRequired)
            {
                comboLinea.Invoke(new Action(() =>
                {
                    comboLinea.Enabled = false;
                    item = (ComboboxItem)comboLinea.SelectedItem;
                }));
                pkLinea = (item != null && item.Value != null) ? item.Value.ToString() : "";
            }
            else
            {
                comboLinea.Enabled = false;
                pkLinea = (comboLinea != null && comboLinea.SelectedItem != null && (comboLinea.SelectedItem as ComboboxItem).Value != null) ? (comboLinea.SelectedItem as ComboboxItem).Value.ToString() : "";
            }
            if (dTPicker2.InvokeRequired)
            {
                dTPicker2.Invoke(new Action(() => { dTPicker2.Enabled = false; }));

            }
            else {
                dTPicker2.Enabled = false;
            }
            getRows(origen, fecha, pkRol, pkLinea);
            if (comboEscala.InvokeRequired)
            {
                comboEscala.Invoke(new Action(() => {
                    comboEscala.Enabled = true;
                }));
            }
            else
            {
                comboEscala.Enabled = true;
            }
            if (comboRol.InvokeRequired)
            {
                comboRol.Invoke(new Action(() => {
                    comboRol.Enabled = true;
                }));
            }
            else
            {
                comboRol.Enabled = true;
            }
            if (dTPicker.InvokeRequired)
            {
                dTPicker.Invoke(new Action(() => {
                    dTPicker.Enabled = true;
                }));
            }
            else
            {
                dTPicker.Enabled = true;
            }
            if (comboLinea.InvokeRequired)
            {
                comboLinea.Invoke(new Action(() =>
                {
                    comboLinea.Enabled = true;
                }));
            }
            else
            {
                comboLinea.Enabled = true;
            }
            if (dTPicker2.InvokeRequired)
            {
                dTPicker2.Invoke(new Action(() => { dTPicker2.Enabled = true; }));

            }
            else
            {
                dTPicker2.Enabled = true;
            }
            setVisibleProgressBar1Delegate(false);
            setValueProgressBarDelegate(0);
        }

        public void setVisibleProgressBar1Delegate(bool valor) {

            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() => { progressBar1.Visible = valor; }));
            }
            else {
                progressBar1.Visible = valor;
            }
        }

        private void VerificationUserControl1_VerificationStatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (verificationUserControl1.InvokeRequired)
            {
                bool band = false;
                bool band2 = false;
                groupBoxHuella.Invoke(new Action(() =>
                {
                    groupBoxHuella.Visible = false;
                  

                }));
                textBoxcontraseña.Invoke(new Action(() =>
                {
                    textBoxcontraseña.Visible = false;
                    textBoxcontraseña.Text = "";
                }));

               
                contando = 0;
                verificationUserControl1.Invoke(new Action(() =>
                {
                    band = verificationUserControl1.Validate();
                    band2 = verificationUserControl1.IsVerificationComplete;
                }));
                if (band && band2)
                {
                    panelcontraseña.Visible = false;
                    contando = 0;
                    switch (operacion)
                    {
                        case OPERACION_GENERAR_CORRIDAS:
                            generaCorridasDia();
                            break;
                        case OPERACION_ACTUALIZAR_DETALLE:
                            guardarEscala();
                            break;
                        case OPERACION_GENERAR_EXTRA:
                            addExtra();
                            break;
                    }
                    if (groupBoxHuella.InvokeRequired)
                    {
                        groupBoxHuella.Invoke(new Action(() =>
                        {
                            groupBoxHuella.Hide();

                        }));
                    }
                    else {
                            groupBoxHuella.Hide();
                    }
                    verificationUserControl1.Invoke(new Action(() => {
                        //verificationUserControl1.Hide();
                        verificationUserControl1.Samples.Clear();
                        verificationUserControl1.IsVerificationComplete = false;
                        verificationUserControl1.img = null;
                        verificationUserControl1.Init();
                        verificationUserControl1.limpiarhuella();
                    }));

                }
            }
            else
            {
                if (groupBoxHuella.InvokeRequired)
                {
                    groupBoxHuella.Invoke(new Action(() =>
                    {
                        groupBoxHuella.Hide();
                    }));
                }
                else
                {
                    groupBoxHuella.Hide();
                }

                if (verificationUserControl1.IsVerificationComplete && verificationUserControl1.Validate())
                {
                    generaCorridasDia();
                    //verificationUserControl1.Hide();
                    verificationUserControl1.Samples.Clear();
                    verificationUserControl1.IsVerificationComplete = false;
                    verificationUserControl1.img = null;
                    verificationUserControl1.Init();
                    //verificationUserControl1.Start();
                    verificationUserControl1.limpiarhuella();
                    verificationUserControl1.FingerPrintPicture.Image = null;
                    verificationUserControl1.Stop();


                }
            }
        }

        private void VerificationUserControl1_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.Escape)
            {
                operacion = -1;
                VerificationUserControl verificacion = (VerificationUserControl)sender;
                //verificacion.Hide();
                verificacion.Samples.Clear();
                verificacion.IsVerificationComplete = false;
                verificacion.img = null;
                verificacion.Init();
                verificacion.Start();
                verificacion.FingerPrintPicture.Image = null;
                //this.Close();
                controles(1);
                groupBoxHuella.Hide();
            }

        }


        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            comboLinea.SuspendLayout();
            comboRol.SuspendLayout();
            getDatosAdicionales(0);
            getDatosAdicionales(2);
            comboLinea.ResumeLayout();
            comboRol.ResumeLayout();

        }

        private void BackgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            setVisibleProgressBar1Delegate(true);
            setValueProgressBarDelegate(20);
            recargaGrid();
            setVisibleProgressBar1Delegate(false);
            setValueProgressBarDelegate(0);

        }

        private void Corridas_Load(object sender, EventArgs e)
        {
            db = new database();
            this.WindowState = FormWindowState.Maximized;
            timer1.Interval = 1;
            timer1.Start();

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            backgroundWorker2.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync();

            //dataGridView0.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
            dataGridView0.EnableHeadersVisualStyles = false;
            //dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
            dataGridView1.EnableHeadersVisualStyles = false;
            //dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
            dataGridView2.EnableHeadersVisualStyles = false;
            comboLinea.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEscala.DropDownStyle = ComboBoxStyle.DropDownList;
            comboLinea1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRol1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRuta1.DropDownStyle = ComboBoxStyle.DropDownList;

            //verificationUserControl1.Hide();
            groupBoxHuella.Hide();
            DoubleBuffered(dataGridView0, true);
            /*
            comboLinea.SuspendLayout();
            comboRol.SuspendLayout();
            getDatosAdicionales(0);
            getDatosAdicionales(2);
            comboLinea.ResumeLayout();
            comboLinea.ResumeLayout();
            */


            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");

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

        private void Corridas_Shown(object sender, EventArgs e)
        {
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            this.Focus();
            supercontra();
            timer2.Start();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);

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

        public void controles(int opt) {
            switch (opt) {
                case 0:
                    if (btnGuardarEscala.InvokeRequired) {
                        btnGuardarEscala.Invoke(new Action(()=> {
                            btnGuardarEscala.Enabled = false;
                            btnGuardarEscala.BackColor = Color.White;
                        }));
                    }
                    else
                    {
                        btnGuardarEscala.Enabled = false;
                        btnGuardarEscala.BackColor = Color.White;
                    }
                    break;
                case 1:
                    btnGuardarEscala.Enabled = true;
                    btnGuardarEscala.BackColor = Color.FromArgb(38, 45, 56);
                    break;
                case 2:
                    btnAddExtra.Enabled = false;
                    btnAddExtra.BackColor = Color.White;
                    break;
                case 3:
                    btnAddExtra.Enabled = true;
                    btnAddExtra.BackColor = Color.FromArgb(38, 45, 56);
                    break;
                case 4:
                    btnGenerar.Enabled = false;
                    btnGenerar.BackColor = Color.White;
                    break;
                case 5:
                    btnGenerar.Enabled = true;
                    btnGenerar.BackColor = Color.FromArgb(38, 45, 56);
                    break;
                case 6:
                    if (dataGridView1.InvokeRequired)
                    {
                        dataGridView1.Invoke(new Action(() =>
                        {
                            if (dataGridView1.Rows != null && dataGridView1.Rows.Count > 0)
                            {
                                dataGridView1.Rows.Clear();
                            }
                        }));
                    }
                    else {
                        if (dataGridView1.Rows != null && dataGridView1.Rows.Count>0)
                        {
                            dataGridView1.Rows.Clear();
                        }
                    }
                    controles(0);
                    break;
                case 7:
                    if (dataGridView0.InvokeRequired)
                    {
                        dataGridView0.Invoke(new Action(() =>
                        {
                            DataTable dt = (DataTable)dataGridView0.DataSource;
                            dt.Clear();
                        }));
                    }
                    else {
                        if (dataGridView0.Rows != null && dataGridView0.Rows.Count>0)
                        {
                            DataTable dt = (DataTable)dataGridView0.DataSource;
                            dt.Clear();
                        }
                    }
                    controles(6);
                    break;
            }
        }

        private void dataGridView2_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int n2 = e.RowIndex;
            if (n2 > -1 && e.ColumnIndex == dataGridView2.Rows[n2].Cells["autobus2"].ColumnIndex)
            {
                btnAddExtra.Enabled = false;
                btnAddExtra.BackColor = Color.White;
            }
        }

        private void verificationUserControl1_BackColorChanged(object sender, EventArgs e)
        {
            contando += 1;
            if (contando >= 6)
            {
                contando = 0;
     
                contando = 0;
                panelcontraseña.Visible = true;
                textBoxcontraseña.Visible = true;
                textBoxcontraseña.Text = "";
                textBoxcontraseña.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (contraseña == textBoxcontraseña.Text)
            {
                groupBoxHuella.Visible = false;
                textBoxcontraseña.Visible = false;
                textBoxcontraseña.Text = "";
                contando = 0;

                panelcontraseña.Visible = false;
                textBoxcontraseña.Text = "";

                switch (operacion)
                {
                    case OPERACION_GENERAR_CORRIDAS:
                        generaCorridasDia();
                        break;
                    case OPERACION_ACTUALIZAR_DETALLE:
                        guardarEscala();
                        break;
                    case OPERACION_GENERAR_EXTRA:
                        addExtra();
                        break;
                }
              
                        groupBoxHuella.Hide();
             
                    verificationUserControl1.limpiarhuella();
                verificationUserControl1.Stop();
         
            }
            else
            {
                contraseñaerror.Visible = true;
            }
        }
        private void supercontra()
        {
            try
            {
                string sql = "SELECT VALOR FROM VARIABLES WHERE NOMBRE='CONTRASEÑA'";


                db.PreparedSQL(sql);
                res = db.getTable();
                if (res.Next())
                {
                    contraseña = res.Get("VALOR");

                }

            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "supercontra";

            }
        }

        private void buttonAceptarAdvertencia_Click(object sender, EventArgs e)
        {
            panelAdvertencia.Visible = false;
            textBoxAdvertencia.Text = "";
        }

        private void textBoxcontraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == (char)Keys.Enter))
            {
                if (contraseña == textBoxcontraseña.Text)
                {
                    groupBoxHuella.Visible = false;
                    textBoxcontraseña.Visible = false;
                    textBoxcontraseña.Text = "";
                    contando = 0;

                    panelcontraseña.Visible = false;
                    textBoxcontraseña.Text = "";

                    switch (operacion)
                    {
                        case OPERACION_GENERAR_CORRIDAS:
                            generaCorridasDia();
                            break;
                        case OPERACION_ACTUALIZAR_DETALLE:
                            guardarEscala();
                            break;
                        case OPERACION_GENERAR_EXTRA:
                            addExtra();
                            break;
                    }

                    groupBoxHuella.Hide();

                    verificationUserControl1.limpiarhuella();
                    verificationUserControl1.Stop();

                }
                else
                {
                    contraseñaerror.Visible = true;
                }
            }
        }
    }
}
