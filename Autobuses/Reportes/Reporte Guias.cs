    using Autobuses.Utilerias;
    using ConnectDB;
    using Gma.QrCodeNet.Encoding;
    using Gma.QrCodeNet.Encoding.Windows.Render;
    using MyAttendance;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Printing;
    using System.IO;
    using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    namespace Autobuses.Reportes
    {
        public partial class Reporte_Guias : Form
        {
            public database db;
            ResultSet res = null;
            Bitmap imagen;
            private string _clase = "reporteguia";
            private int n = 0;
            private string dia;
            private string folio;
            private string folioguia;
            private string importeguia;
            private string gastosguia;
            private string tarjetasguia;
            private string ivaguia;
            private string totalguia;
            private string pkguia;
            private string status;
            private string origen;
            private string destino;
            private string fecha;
            private string autobus;
            private string boleto;
            private string importe;
            private string dsalida;
            private string comtaq;
            private string compban;
            private string aportacion;
            private string diesel;
            private string caseta;
            private string tarjeta;
            private string vseden;
            private string iva;
            private string socior;
            private string anticipo;
            private string total;
            private string foliobuscar="";
            private string _linea;
            private string sucursal;
            private string validador;
            private int espacio = 220;
            private string chofer;
            private string linea;
            private int tamaño;
            private string hora;
            private List<string> asiento = new List<string>();
            private List<string> pasajero = new List<string>();
            private List<string> destinopasajero = new List<string>();
            private List<string> foliopasajero = new List<string>();
            private List<string> tarifapasajero = new List<string>();
            private List<string> preciopasajero = new List<string>();
            private string fechainicio;
            private string fechatermino;
            private string sucursalbusqueda=LoginInfo.Sucursal;
            private string recibedinero;
            private string fechaseleccionada;
            private string _socio;
            private string _autobus;
            private string _status;
            private string _conductor;
            private double importetext = 0.0;
           private double gastostext = 0.0;
            private double tarjetastext = 0.0;
           private  double ivatext = 0.0;
           private double totaltext = 0.0;
            private string pagadooactivo = "";
            private string pk_guia;

            private int cantidadfolios=0;
            private double imported = 0.0;
           private double gastosd = 0.0;
            private double tarjetasd = 0.0;
            private double ivad = 0.0;
           private  double totald = 0.0;


            byte[] fingerPrint;
            List<string> usuariochofer = new List<string>();
            private List<string> foliosporpagar = new List<string>();



            public Reporte_Guias()
            {

                InitializeComponent();
            

                this.Show();

            }



            private void permisos()
            {

        
            }


            private void DataGridViewguias_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                try {
                  int  n = e.RowIndex;

                    if (n != -1)
                    {
                 
                        pk_guia = (string)dataGridViewguias.Rows[n].Cells[27].Value;
                        iradetalle();
                    }
                
                }
                catch (Exception err)
                {
                    string error = err.Message;
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                    string funcion = "datagridviewguia";
                    Utilerias.LOG.write(_clase, funcion, error);


                }

            }
            private bool CheckOpened(string name)
            {

                Form form = null;

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
            private void iradetalle()
            {
                try
                {


                    Form form = null;
                    if (CheckOpened("Detalleguia"))
                    {
                        form.StartPosition = FormStartPosition.CenterScreen;
                        form.Show();

                    }
                    else
                    {

                        form = new Detallguia(pk_guia);
                        AddOwnedForm(form);
                        form.StartPosition = FormStartPosition.CenterScreen;
                        form.Show();
                        Utilerias.LOG.acciones("ingreso a detalleguia ");


                    }




                }
                catch (Exception err)
                {
                    string error = err.Message;
                    // MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                    string funcion = "guiaclick";
                    Utilerias.LOG.write(_clase, funcion, error);


                }
            }



            public void getdatastatus()
            {
                try
                {
                    combostatus.Items.Clear();

               
                        ComboboxItem item = new ComboboxItem();
                        item.Text = "PAGADA";
                        combostatus.Items.Add(item);
                    ComboboxItem item2 = new ComboboxItem();
                    item2.Text = "ACTIVA";
                    combostatus.Items.Add(item2);

                }
                catch (Exception err)
                {
                    string error = err.Message;
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                    string funcion = "getDatosAdicionales";
                    Utilerias.LOG.write(_clase, funcion, error);


                }

            }

            public void getDatosAdicionaleschoferes()
            {
                try
                {
                    comboconductor.Items.Clear();

                    string sql = "SELECT PK, NOMBRE,APELLIDOS FROM CHOFERES WHERE BORRADO=0 ORDER BY NOMBRE";
                    db.PreparedSQL(sql);
                    res = db.getTable();

                    while (res.Next())
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = res.Get("NOMBRE")+" "+ res.Get("APELLIDOS");
                    item.Value = res.Get("PK");
                    comboconductor.Items.Add(item);

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
            public void getDatosAdicionaleslinea()
            {
                try
                {
                    comboBoxlinea.Items.Clear();

                    string sql = "SELECT LINEA,PK1 FROM LINEAS WHERE BORRADO=0";
                    db.PreparedSQL(sql);
                    res = db.getTable();

                    while (res.Next())
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = res.Get("LINEA");
                        item.Value = res.Get("PK1");

                        comboBoxlinea.Items.Add(item);

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

            public void getDatosAdicionalessocios()
            {
                try
                {
                    combosocio.Items.Clear();

                    string sql = "SELECT PK,NOMBRE,APELLIDOS FROM SOCIOS WHERE BORRADO=0 ORDER BY NOMBRE";
                    db.PreparedSQL(sql);
                    res = db.getTable();

                    while (res.Next())
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = res.Get("NOMBRE")+" "+ res.Get("APELLIDOS");
                    item.Value = res.Get("PK");
                        combosocio.Items.Add(item);

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
            public void getDatosAdicionalesautobus()
            {
                try
                {
                    comboautobus.Items.Clear();

                    string sql = "SELECT PK1, ECO FROM AUTOBUSES WHERE BORRADO=0 ORDER BY ECO";
                    db.PreparedSQL(sql);
                    res = db.getTable();

                    while (res.Next())
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = res.Get("ECO");
                        item.Value = res.Get("PK1");
                        comboautobus.Items.Add(item);

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

                private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
                {
                    try
                    {
                        fechainicio = dateTimePicker1.Value.ToString("dd/MM/yyyy");
                    }
                    catch (Exception err)
                    {
                        string error = err.Message;
                        MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                        string funcion = "datetimerpicker1";
                        Utilerias.LOG.write(_clase, funcion, error);


                    }
                }
   
                private void DateTimePicker2_ValueChanged(object sender, EventArgs e)
                {
                    try
                    {
                        fechatermino = dateTimePicker2.Value.ToString("dd/MM/yyyy");
                    }
                    catch (Exception err)
                    {
                        string error = err.Message;
                        MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                        string funcion = "datetimepicker2";
                        Utilerias.LOG.write(_clase, funcion, error);


                    }
                }
            private void buscar(object sender, EventArgs e)
            {
                try
                {
                    dataGridViewguias.Rows.Clear();
                    _status = "";
                    _socio = "";
                    _autobus = "";
                    _conductor = "";
         

                    if ((combostatus.SelectedItem) != null)
                    {

                        _status = combostatus.SelectedItem.ToString();
                    }
                    if ((combosocio.SelectedItem) != null)
                    {
                        _socio = combosocio.SelectedItem.ToString();
                       // ValidateUserGUIA();

                    }
                    if ((comboautobus.SelectedItem) != null)
                    {
                        _autobus = comboautobus.SelectedItem.ToString();
                        //ValidateUserGUIA();

                    }
                    if ((comboconductor.SelectedItem) != null)
                    {

                        _conductor = comboconductor.SelectedItem.ToString();
                        //ValidateUserGUIA();

                    }


                    getRows(_status, _socio, _conductor, _autobus, fechainicio,fechatermino);
                }


                catch (Exception err)
                {
                    string error = err.Message;
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                    string funcion = "getRows";
                    Utilerias.LOG.write(_clase, funcion, error);


                }
            }



        
            public void getRows(string status = "", string socio = "", string conductor = "", string autobus = "", string ini = "",string ter="")
            {
                try
                {
                    int count = 1;
                    string sql = "SELECT * FROM GUIA";
                
                    if (status != "" && socio != "" && conductor != "" && autobus != "" && ini !="" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND STATUS=@STATUS AND SOCIO=@SOCIO AND CHOFER=@CONDUCTOR AND AUTOBUS=@AUTOBUS AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@STATUS", status);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@SOCIO", socio);
                        db.command.Parameters.AddWithValue("@CONDUCTOR", conductor);
                        db.command.Parameters.AddWithValue("@AUTOBUS", autobus);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);

                    }
                    //1
                    if (status == "" && socio != "" && conductor != "" && autobus != "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND SOCIO=@SOCIO AND CHOFER=@CONDUCTOR AND AUTOBUS=@AUTOBUS AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@SOCIO", socio);
                        db.command.Parameters.AddWithValue("@CONDUCTOR", conductor);
                        db.command.Parameters.AddWithValue("@AUTOBUS", autobus);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);


                    }
                    //2
                    if (status != "" && socio == "" && conductor != "" && autobus != "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND STATUS=@STATUS AND CHOFER=@CONDUCTOR AND AUTOBUS=@AUTOBUS AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@STATUS", status);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@CONDUCTOR", conductor);
                        db.command.Parameters.AddWithValue("@AUTOBUS", autobus);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);


                    }
                    //3
                    if (status == "" && socio == "" && conductor != "" && autobus != "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND CHOFER=@CONDUCTOR AND AUTOBUS=@AUTOBUS AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@CONDUCTOR", conductor);
                        db.command.Parameters.AddWithValue("@AUTOBUS", autobus);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);

                    }
                    //4
                    if (status != "" && socio != "" && conductor == "" && autobus != "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND STATUS=@STATUS AND SOCIO=@SOCIO AND AUTOBUS=@AUTOBUS AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@STATUS", status);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@SOCIO", socio);
                        db.command.Parameters.AddWithValue("@AUTOBUS", autobus);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);


                    }
                    //5
                    if (status == "" && socio != "" && conductor == "" && autobus != "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND SOCIO=@SOCIO AND AUTOBUS=@AUTOBUS AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@SOCIO", socio);
                        db.command.Parameters.AddWithValue("@AUTOBUS", autobus);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);

                    }
                    //6
                    if (status != "" && socio == "" && conductor == "" && autobus != "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND STATUS=@STATUS AND AUTOBUS=@AUTOBUS AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@STATUS", status);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@AUTOBUS", autobus);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);

                    }
                    //7
                    if (status == "" && socio == "" && conductor == "" && autobus != "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND AUTOBUS=@AUTOBUS AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@AUTOBUS", autobus);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);

                    }
                    //8
                    if (status != "" && socio != "" && conductor != "" && autobus == "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND STATUS=@STATUS AND SOCIO=@SOCIO AND CHOFER=@CONDUCTOR AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@STATUS", status);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@SOCIO", socio);
                        db.command.Parameters.AddWithValue("@CONDUCTOR", conductor);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);

                    }
                    //9
                    if (status == "" && socio != "" && conductor != "" && autobus == "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND SOCIO=@SOCIO AND CHOFER=@CONDUCTOR AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@SOCIO", socio);
                        db.command.Parameters.AddWithValue("@CONDUCTOR", conductor);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);

                    }
                    //10
                    if (status != "" && socio == "" && conductor != "" && autobus == "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND STATUS=@STATUS AND CHOFER=@CONDUCTOR AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@STATUS", status);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@CONDUCTOR", conductor);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);

                    }
                    //11
                    if (status == "" && socio == "" && conductor != "" && autobus == "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            " AND CHOFER=@CONDUCTOR AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@CONDUCTOR", conductor);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);

                    }
                    //12
                    if (status != "" && socio != "" && conductor == "" && autobus == "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND STATUS=@STATUS AND SOCIO=@SOCIO AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@CONDUCTOR", conductor);
                        db.command.Parameters.AddWithValue("@SOCIO", socio);
                        db.command.Parameters.AddWithValue("@STATUS", status);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);

                    }

                    //13
                    if (status == "" && socio != "" && conductor == "" && autobus == "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND SOCIO=@SOCIO AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@SOCIO", socio);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);

                    }
                    //14
                    if (status != "" && socio == "" && conductor == "" && autobus == "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND STATUS=@STATUS AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@STATUS", status);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);

                    }
                    //15
                    if (status == "" && socio == "" && conductor == "" && autobus == "" && ini != "" && ter != "")
                    {
                        sql += " WHERE  CONVERT(DATETIME, FECHA,103) BETWEEN CONVERT(DATETIME, @INICIO, 103) AND CONVERT(DATETIME, @FINAL, 103) " +
                            "AND AUTOBUS=@AUTOBUS AND LINEA=@LINEA";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@INICIO", ini);
                        db.command.Parameters.AddWithValue("@FINAL", ter);
                        db.command.Parameters.AddWithValue("@LINEA", _linea);

                    }




                    res = db.getTable();

                bool pagarsepuede = false;
                importetext = 0.0;
                gastostext = 0.0;
                tarjetastext = 0.0;
                ivatext = 0.0;
                totaltext = 0.0;


                while (res.Next())
                        {
                            n = dataGridViewguias.Rows.Add();


                            dataGridViewguias.Rows[n].Cells["dianame"].Value = res.Get("FECHA");
                            dataGridViewguias.Rows[n].Cells["folioname"].Value = res.Get("FOLIO");
                            dataGridViewguias.Rows[n].Cells["estadoname"].Value = res.Get("STATUS");
                            dataGridViewguias.Rows[n].Cells["origenname"].Value = res.Get("ORIGEN");
                            dataGridViewguias.Rows[n].Cells["destinoname"].Value = res.Get("DESTINO");
                            string fech = res.Get("HORA");
                          //  dataGridViewguias.Rows[n].Cells[5].Value = res.Get("FECHA") + " " + fech;
                            dataGridViewguias.Rows[n].Cells["autobusname"].Value = res.Get("AUTOBUS");
                            dataGridViewguias.Rows[n].Cells["boletoname"].Value = res.Get("BOLETOS");


                    double impr = (double.TryParse(res.Get("IMPORTE"), out double aux1)) ? res.GetDouble("IMPORTE") : 0.0;
                    double compbanr = (double.TryParse(res.Get("COMPBAN"), out double aux2)) ? res.GetDouble("COMPBAN") : 0.0;
                    double tsalidar = (double.TryParse(res.Get("TSALIDA"), out double aux3)) ? res.GetDouble("TSALIDA") : 0.0;
                    double tturnor = (double.TryParse(res.Get("TTURNO"), out double aux4)) ? res.GetDouble("TTURNO") : 0.0;
                    double ttpasor = (double.TryParse(res.Get("TPASO"), out double aux5)) ? res.GetDouble("TPASO") : 0.0;
                    double ivar = (double.TryParse(res.Get("IVA"), out double aux6)) ? res.GetDouble("IVA") : 0.0;
                    double anticipor = (double.TryParse(res.Get("ANTICIPO"), out double aux7)) ? res.GetDouble("ANTICIPO") : 0.0;
                    double totalr = (double.TryParse(res.Get("TOTAL"), out double aux9)) ? res.GetDouble("TOTAL") : 0.0;
                    double  aportacionr = (double.TryParse(res.Get("APORTACION"), out double aux11)) ? res.GetDouble("APORTACION") : 0.0;

                    dataGridViewguias.Rows[n].Cells["importename"].Value = Utilerias.Utilerias.formatCurrency(impr);
                            dataGridViewguias.Rows[n].Cells[8].Value = res.Get("COMTAQ");
                            dataGridViewguias.Rows[n].Cells[9].Value = Utilerias.Utilerias.formatCurrency(compbanr);
                            dataGridViewguias.Rows[n].Cells[10].Value = Utilerias.Utilerias.formatCurrency(aportacionr);
                            dataGridViewguias.Rows[n].Cells[11].Value = res.Get("DIESEL");
                            dataGridViewguias.Rows[n].Cells[12].Value = res.Get("CASETA");
                            dataGridViewguias.Rows[n].Cells["salidaname"].Value = Utilerias.Utilerias.formatCurrency(tsalidar);
                            dataGridViewguias.Rows[n].Cells["turnoname"].Value = Utilerias.Utilerias.formatCurrency(tturnor);
                            dataGridViewguias.Rows[n].Cells["pasoname"].Value = Utilerias.Utilerias.formatCurrency(ttpasor);
                            //dataGridViewguias.Rows[n].Cells[16].Value = res.Get("VSEDENA");
                            dataGridViewguias.Rows[n].Cells["ivaname"].Value = Utilerias.Utilerias.formatCurrency(ivar);
                            dataGridViewguias.Rows[n].Cells["gastosname"].Value = Utilerias.Utilerias.formatCurrency(anticipor);
                            dataGridViewguias.Rows[n].Cells["totalname"].Value = Utilerias.Utilerias.formatCurrency(totalr);
                            dataGridViewguias.Rows[n].Cells["sucursalname"].Value = res.Get("SUCURSAL");
                            dataGridViewguias.Rows[n].Cells["validadorname"].Value = res.Get("validador");
                            dataGridViewguias.Rows[n].Cells["chofername"].Value = res.Get("CHOFER");
                            dataGridViewguias.Rows[n].Cells["lineaname"].Value = res.Get("LINEA");
                            dataGridViewguias.Rows[n].Cells["horaname"].Value = res.Get("HORA");
                            dataGridViewguias.Rows[n].Cells["socioname"].Value = res.Get("SOCIO");
                        dataGridViewguias.Rows[n].Cells["pkname"].Value = res.Get("PK");


                    importetext += impr;
                    gastostext += anticipor;
                    tarjetastext += tturnor;
                    tarjetastext += tsalidar;
                    tarjetastext += ttpasor;
                    ivatext += ivar;
                    totaltext += totalr;
                    pagadooactivo = (string)dataGridViewguias.Rows[n].Cells[2].Value;
                    if (pagadooactivo == "PAGADA")
                    {
                        pagarsepuede = true;
                    }

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

            private void obtenerdetalledeboletos()
            {
                try
                {

                    string sql = "SELECT * FROM VENDIDOS WHERE PKGUIA=@PKGUIA";


                    db.PreparedSQL(sql);

                    db.command.Parameters.AddWithValue("@PKGUIA", pk_guia);
            

                    int n = 0;

                    res = db.getTable();

                    while (res.Next())
                    {

                      asiento.Add( res.Get("ASIENTO"));
                        pasajero.Add(res.Get("PASAJERO"));
                        destinopasajero.Add(res.Get("DESTINO"));
                        foliopasajero.Add(res.Get("FOLIO"));
                        tarifapasajero.Add(res.Get("TARIFA"));
                        preciopasajero.Add(res.Get("PRECIO"));
              
                    }
                    if (db.execute())
                    {
                    }

                }
                catch (Exception err)
                {
                    string error = err.Message;
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                    string funcion = "obtenerdetalledeboletos";
                    Utilerias.LOG.write(_clase, funcion, error);


                }
            }

            private void codigoqr(string folio)
            {
                try
                {


                    string valor = folio;

                    QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                    QrCode qrCode = new QrCode();
                    qrEncoder.TryEncode(valor, out qrCode);

                    GraphicsRenderer renderer = new GraphicsRenderer(new FixedCodeSize(400, QuietZoneModules.Zero), Brushes.Black, Brushes.White);

                    MemoryStream ms = new MemoryStream();

                    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
                    var imageTemporal = new Bitmap(ms);
                    imagen = new Bitmap(imageTemporal, new Size(new Point(100, 100)));
                    //imagen.Save("imagen.png", ImageFormat.Png);

                    // Guardar en el disco duro la imagen (Carpeta del proyecto)

                }
                catch (Exception err)
                {
                    string error = err.Message;
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                    string funcion = "codigoqr";
                    Utilerias.LOG.write(_clase, funcion, error);


                }
            }


            void llenarticket(object sender, PrintPageEventArgs e)
            {
                try
                {
                    codigoqr(folio);

                    Graphics g = e.Graphics;
                    // g.DrawRectangle(Pens.Black, 3, 5, 340, 700);
                    // g.DrawImage(imagensplash, 10, 0);
                    Font fBody7 = new Font("Lucida", 7, FontStyle.Bold);
                    Font fBody5 = new Font("Lucida", 5, FontStyle.Bold);

                    Font fBody9 = new Font("Lucida", 9, FontStyle.Bold);
                    Font fBody = new Font("Lucida", 8, FontStyle.Bold);
                    Font fBody10 = new Font("Lucida", 10, FontStyle.Bold);
                    Font fBody12 = new Font("Lucida", 12, FontStyle.Bold);
                    Font fBody18 = new Font("Lucida", 18, FontStyle.Bold);

                    Color customColor = Color.FromArgb(255, Color.Black);
                    SolidBrush sb = new SolidBrush(customColor);
                    espacio = 0;
                    g.DrawString("Fecha :", fBody, sb, 2, espacio);
                    g.DrawString(DateTime.Now.ToShortDateString(), fBody, sb, 48, espacio);
                    g.DrawString("Hora :", fBody, sb, 170, espacio);
                    g.DrawString(DateTime.Now.ToShortTimeString(), fBody, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("Pago de Guias", fBody18, sb, 50, espacio);
                    espacio = espacio + 30;
                    g.DrawString("SUCURSAL: " + LoginInfo.Sucursal, fBody10, sb, 0, espacio);
                    g.DrawString("Linea: " + _linea, fBody10, sb, 150, espacio);

                    espacio = espacio + 20;

                    g.DrawString("Folio: "+folio, fBody10, sb, 2, espacio);
                    g.DrawString("Guias: " + cantidadfolios.ToString(), fBody10, sb, 150, espacio);

               
                    espacio = espacio + 20;
                    g.DrawString("Usuario: " + LoginInfo.UserID, fBody10, sb, 0, espacio);
                    g.DrawString("Socio: " + _socio, fBody10, sb, 150, espacio);
                    espacio = espacio + 20;
                    g.DrawString("Rango de fechas: ", fBody10, sb, 70, espacio);
                    espacio = espacio + 20;
                    g.DrawString("Inicio: " + fechainicio, fBody, sb, 0, espacio);
                    g.DrawString("Termino: " + fechatermino, fBody, sb, 140, espacio);
                    espacio = espacio + 20;


                    g.DrawString("-------------------------------------------------------------------------------------", fBody7, sb, 0, espacio);
                    espacio = espacio + 10;

                    g.DrawString("FOLIO", fBody5, sb, 0, espacio);
                    g.DrawString("IMPORTE", fBody5, sb, 45, espacio);
                    g.DrawString("GASTOS", fBody5, sb, 85, espacio);
                    g.DrawString("TARJETAS", fBody5, sb, 130, espacio);
                    g.DrawString("IVA", fBody5, sb, 185, espacio);
                    g.DrawString("TOTAL", fBody5, sb, 225, espacio);
                    espacio = espacio + 10;

                    g.DrawString("-------------------------------------------------------------------------------------", fBody7, sb, 0, espacio);
                    espacio = espacio + 20;

                    for (int i = 0; i < asiento.Count(); i++)
                    {


                        g.DrawString(asiento[i], fBody5, sb, 0, espacio);
                        g.DrawString("$" + pasajero[i], fBody5, sb, 45, espacio);
                        g.DrawString("$" + destinopasajero[i], fBody5, sb, 85, espacio);
                        g.DrawString("$" + foliopasajero[i], fBody5, sb, 130, espacio);
                        g.DrawString("$" + tarifapasajero[i], fBody5, sb, 185, espacio);
                        g.DrawString("$" + preciopasajero[i], fBody5, sb, 225, espacio);

                        espacio = espacio + 20;
                    }

                    espacio = espacio + 10;
                    g.DrawString("--------------------------------------------------------------------------------------", fBody7, sb, 0, espacio - 20);
                    espacio = espacio + 10;
                    g.DrawImage(imagen, 20, espacio);

                    g.DrawString("Importe: "+ "$" + importetext.ToString(), fBody7, sb, 170, espacio);
                    espacio = espacio + 20;
                    g.DrawString("Gastos: "+ "$" + gastostext.ToString(), fBody7, sb, 170, espacio);
                    espacio = espacio + 20;
                    g.DrawString("Tarjetas: "+ "$" + tarjetastext.ToString(), fBody7, sb, 170, espacio);
                    espacio = espacio + 20;
                    g.DrawString("IVA: "+ "$" + ivatext.ToString(), fBody7, sb, 170, espacio);
                    espacio = espacio + 20;
                    g.DrawString("Total: "+ "$" + totaltext.ToString(), fBody7, sb, 170, espacio);

                    g.Dispose();
                }
                catch (Exception err)
                {
                    string error = err.Message;
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                    string funcion = "llenarticket";
                    Utilerias.LOG.write(_clase, funcion, error);


                }
            }

            private void imprimirr()
            {
                try
                {
                    tamaño = 720 + ((cantidadfolios) * 20);
                    PrintDocument pd = new PrintDocument();
                    PaperSize ps = new PaperSize("", 420, tamaño);

                    pd.PrintPage += new PrintPageEventHandler(llenarticket);
                    pd.PrintController = new StandardPrintController();
                    pd.DefaultPageSettings.Margins.Left = 0;
                    pd.DefaultPageSettings.Margins.Right = 0;
                    pd.DefaultPageSettings.Margins.Top = 0;
                    pd.DefaultPageSettings.Margins.Bottom = 0;
                    pd.DefaultPageSettings.PaperSize = ps;
                    pd.PrinterSettings.PrinterName = Settings1.Default.impresora;
                    pd.Print();
                    Utilerias.LOG.acciones("imprimir pago guia " + folio);

                    asiento.Clear();
                    pasajero.Clear();
                    destinopasajero.Clear();
                    foliopasajero.Clear();
                    tarifapasajero.Clear();
                    preciopasajero.Clear(); 
                }

                catch (Exception err)
                {
                    string error = err.Message;
                    MessageBox.Show("Seleccione de nuevo una impresora.");
                    string funcion = "imprimir";
                    Utilerias.LOG.write(_clase, funcion, error);


                }

            }

            private void Reimprimir_Click(object sender, EventArgs e)
            {
                imprimirr();
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
                    string funcion = "closestrip";
                    Utilerias.LOG.write(_clase, funcion, error);


                }
            }

            public String GenerateRandom()
            {
                Random randomGenerate = new System.Random();
                String sPassword = "";
                sPassword = Convert.ToString(randomGenerate.Next(00000001, 99999999));
                return sPassword.Substring(sPassword.Length - 8, 8);
            }
            private void act()
            {
                try
                {
                 
                    if (foliobuscar != "")
                    {
                        int count = 1;
                        string sql = "SELECT * FROM GUIA";

                        sql += " WHERE  FOLIO=@FOLIO ";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@FOLIO", foliobuscar);


                        res = db.getTable();

                        if (res.Next())
                        {
                            n = dataGridViewguias.Rows.Add();


                            dataGridViewguias.Rows[n].Cells[0].Value = res.Get("FECHA");
                            dataGridViewguias.Rows[n].Cells[1].Value = res.Get("FOLIO");
                            dataGridViewguias.Rows[n].Cells[2].Value = res.Get("STATUS");
                            status = res.Get("STATUS");
                            dataGridViewguias.Rows[n].Cells[3].Value = res.Get("ORIGEN");
                            dataGridViewguias.Rows[n].Cells[4].Value = res.Get("DESTINO");
                            string fech = res.Get("HORA");
                            dataGridViewguias.Rows[n].Cells[5].Value = res.Get("FECHA") + " " + fech;
                            dataGridViewguias.Rows[n].Cells[6].Value = res.Get("AUTOBUS");
                            dataGridViewguias.Rows[n].Cells[7].Value = res.Get("BOLETOS");
                            dataGridViewguias.Rows[n].Cells[8].Value = res.Get("IMPORTE");
                            dataGridViewguias.Rows[n].Cells[9].Value = res.Get("COMTAQ");
                            dataGridViewguias.Rows[n].Cells[10].Value = res.Get("COMPBAN");
                            dataGridViewguias.Rows[n].Cells[11].Value = res.Get("APORTACION");
                            dataGridViewguias.Rows[n].Cells[12].Value = res.Get("DIESEL");
                            dataGridViewguias.Rows[n].Cells[13].Value = res.Get("CASETA");
                            dataGridViewguias.Rows[n].Cells[14].Value = res.Get("TSALIDA");
                            dataGridViewguias.Rows[n].Cells[15].Value = res.Get("TTURNO");
                            dataGridViewguias.Rows[n].Cells[16].Value = res.Get("TPASO");
                            dataGridViewguias.Rows[n].Cells[17].Value = res.Get("VSEDENA");
                            dataGridViewguias.Rows[n].Cells[18].Value = res.Get("IVA");
                            dataGridViewguias.Rows[n].Cells[19].Value = res.Get("ANTICIPO");
                            dataGridViewguias.Rows[n].Cells[20].Value = res.Get("TOTAL");
                            dataGridViewguias.Rows[n].Cells[21].Value = res.Get("SUCURSAL");
                            dataGridViewguias.Rows[n].Cells[22].Value = res.Get("validador");
                            dataGridViewguias.Rows[n].Cells[23].Value = res.Get("CHOFER");
                            dataGridViewguias.Rows[n].Cells[24].Value = res.Get("LINEA");
                            dataGridViewguias.Rows[n].Cells[25].Value = res.Get("HORA");
                            dataGridViewguias.Rows[n].Cells[26].Value = res.Get("SOCIO");




                            count++;
                        }

                        db.execute();

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
            private void Buscarfolio_Click(object sender, EventArgs e) { 
                act();
            }

           
            private void Limpiar_Click(object sender, EventArgs e)
            {
                limp();
            }
            private void limp()
            {
                combostatus.Enabled = false;
                combosocio.Enabled = false;
                comboconductor.Enabled = false;
                comboautobus.Enabled = false;
               
                comboconductor.Text = null;
                comboautobus.Text = null;
                combosocio.Text = null;
                combostatus.Text = null;
                comboBoxlinea.Text = "";
                sucursalbusqueda = "";
                dataGridViewguias.Rows.Clear();
                comboBoxlinea.Items.Clear();
                getDatosAdicionaleslinea();
               
        
            }

            private void Reporte_Guias_Load(object sender, EventArgs e)
            {
                this.WindowState = FormWindowState.Maximized;

            }

            private void Reporte_Guias_Shown(object sender, EventArgs e)
            {
                db = new database();


                combostatus.DropDownStyle = ComboBoxStyle.DropDownList;
                combosocio.DropDownStyle = ComboBoxStyle.DropDownList;
                comboconductor.DropDownStyle = ComboBoxStyle.DropDownList;
                comboautobus.DropDownStyle = ComboBoxStyle.DropDownList;

                dataGridViewguias.EnableHeadersVisualStyles = false;
                fechainicio = DateTime.Now.ToString("dd/MM/yyyy");
                fechatermino = DateTime.Now.ToString("dd/MM/yyyy");
                getdatastatus();
                getDatosAdicionalesautobus();
                getDatosAdicionaleschoferes();
                getDatosAdicionalessocios();
                getDatosAdicionaleslinea();

            titulo.Text = "Reporte de Guias";
                permisos();
            
                comboBoxlinea.DropDownStyle = ComboBoxStyle.DropDownList;

                combostatus.Enabled = false;
                combosocio.Enabled = false;
                comboconductor.Enabled = false;
                comboautobus.Enabled = false;
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            timer1.Interval = 1;

            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
         
            pictureBoxfoto.Image = bmp;
            timer1.Start();
        }

            private void ComboBoxlinea_SelectedIndexChanged(object sender, EventArgs e)
            {
                combostatus.Enabled = true;
                combosocio.Enabled = true;
                comboconductor.Enabled = true;
                comboautobus.Enabled = true;
                _linea = comboBoxlinea.SelectedItem.ToString();

            }

            private void ToolStripExportExcel_Click(object sender, EventArgs e)
            {
                Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridViewguias);

            }

            private void BtnMinimizar_Click(object sender, EventArgs e)
            {
                      this.WindowState = FormWindowState.Minimized;
            }

            private void Button3_Click(object sender, EventArgs e)
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


        private void Timer1_Tick(object sender, EventArgs e)
            {
                lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
                lbFecha.Text = DateTime.Now.ToString("D");

            }

        private void PanelBarraTitulo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GroupBox2_Enter(object sender, EventArgs e)
        {

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
    