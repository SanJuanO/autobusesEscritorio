using ConnectDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Reportes
{
    public partial class detallecortecaja : Form
    {
        public database db;
        ResultSet res = null;
        private string _clase = "reportedetalleruta";

        private int pk;
        private string usuario;
        private string fechainicio;
        private string fechafinal;


        public detallecortecaja(string _usuario, string _fechainicio, string _fechafinal)
            {
            InitializeComponent();
                        db = new database();

            usuario = _usuario;
            fechainicio = _fechainicio;
            fechafinal = _fechafinal;


            fecha();
            
        
        }
        private void fecha()
        {
            try
            {
                if (fechainicio.Length > 19)
                {
                    fechainicio=fechainicio.Substring(0, 19);
                }
                DateTime ini = DateTime.Parse(fechainicio);
                DateTime fin = DateTime.Parse(fechafinal);
                fechainicio = ini.ToString("yyyy-MM-dd HH:mm:ss");
                fechafinal = fin.ToString("yyyy-MM-dd HH:mm:ss");
                getRows();

            }
            catch (Exception)
            {
            }
        }


       
        private void getRows()
        {
            try
            {
                string sql = "SELECT FOLIO,LINEA,ORIGEN,DESTINOBOLETO,SALIDA,TARIFA,ASIENTO,PRECIO, " +
                    "STATUS,ECO,FORMADEPAGO,FOLIOTARJETA,DIGITOSTARJETA FROM VENDIDOS WHERE CORTE = 1 AND VENDEDOR =@VENDEDOR and " +
                    "FECHAC BETWEEN CONVERT(datetime, @FECHAINI, 121) AND CONVERT(datetime, @FECHAFINAL,121)  ";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@VENDEDOR", usuario);
                db.command.Parameters.AddWithValue("@FECHAFINAL", fechafinal);
                db.command.Parameters.AddWithValue("@FECHAINI", fechainicio);



                dataGridViewBOLETOS.Rows.Clear();
                int n = 0;


                res = db.getTable();
                double sum = 0;
                int vendidos = 0;
                int cancelados = 0;

                while (res.Next())

                {
                    n = dataGridViewBOLETOS.Rows.Add();

                    dataGridViewBOLETOS.Rows[n].Cells["folioname"].Value = res.Get("FOLIO");
                    dataGridViewBOLETOS.Rows[n].Cells["lineaname"].Value = res.Get("LINEA");
                    dataGridViewBOLETOS.Rows[n].Cells["origename"].Value = res.Get("ORIGEN");
                    dataGridViewBOLETOS.Rows[n].Cells["destinoname"].Value = res.Get("DESTINOBOLETO");
                    dataGridViewBOLETOS.Rows[n].Cells["salidaname"].Value = res.Get("SALIDA");
                    dataGridViewBOLETOS.Rows[n].Cells["asientoname"].Value = res.Get("ASIENTO");
                    dataGridViewBOLETOS.Rows[n].Cells["tarifaname"].Value = res.Get("TARIFA");
                    double prec= res.GetDouble("PRECIO");
                    dataGridViewBOLETOS.Rows[n].Cells["precionname"].Value = Utilerias.Utilerias.formatCurrency(prec);
                    dataGridViewBOLETOS.Rows[n].Cells["statusname"].Value = res.Get("STATUS");
                    dataGridViewBOLETOS.Rows[n].Cells["autobusname"].Value = res.Get("ECO");

                    dataGridViewBOLETOS.Rows[n].Cells["pagoname"].Value = res.Get("FORMADEPAGO");
                    dataGridViewBOLETOS.Rows[n].Cells["foliotarjetaname"].Value = res.Get("FOLIOTARJETA");
                    dataGridViewBOLETOS.Rows[n].Cells["digitoname"].Value = res.Get("DIGITOSTARJETA");
                    if (dataGridViewBOLETOS.Rows[n].Cells["statusname"].Value.ToString() == "CANCELADO")
                    {
                        dataGridViewBOLETOS.Rows[n].DefaultCellStyle.BackColor = Color.DarkRed;

                    }
                    //textboxvendidos.Text = vendidos.ToString();
                    //textBoxcancelados.Text = cancelados.ToString();
                    //textBoxtotal.Text = "$" + sum.ToString();
                }
            }   
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "detalleboleto";
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
        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
         
private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxsum.Text = "";


                               string _searchtool = Convert.ToString(textBoxbuscar.Text);

                string sql = "SELECT FOLIO,LINEA,ORIGEN,DESTINOBOLETO,SALIDA,TARIFA,ASIENTO,PRECIO, " +
                            "STATUS,ECO,FORMADEPAGO,FOLIOTARJETA,DIGITOSTARJETA FROM VENDIDOS WHERE CORTE = 1 AND VENDEDOR =@VENDEDOR and " +
                            "FECHAC BETWEEN CONVERT(datetime, @FECHAINI, 121) AND CONVERT(datetime, @FECHAFINAL,121) ";

                dataGridViewBOLETOS.Rows.Clear();

                if (!string.IsNullOrEmpty(_searchtool))
                {
                    sql += " AND ((FOLIO LIKE @SEARCH OR TARIFA LIKE @SEARCH OR STATUS LIKE @SEARCH OR FORMADEPAGO LIKE @SEARCH) OR (ASIENTO LIKE @SEARCH) )";
                }
                sql += " ORDER BY FOLIO ASC";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@SEARCH", "%" + _searchtool + "%");
                db.command.Parameters.AddWithValue("@VENDEDOR", usuario);
                db.command.Parameters.AddWithValue("@FECHAFINAL", fechafinal);
                db.command.Parameters.AddWithValue("@FECHAINI", fechainicio);
                res = db.getTable();
                int count = 0;
                int n = 0;
                double sum = 0;
                while (res.Next())

                {
                    string canc = res.Get("STATUS");
                    if (_searchtool == "CANCELADO")
                    {
                        canc = "100";
                    }
                    if (canc != "CANCELADO")
                    {
                        n = dataGridViewBOLETOS.Rows.Add();

                        dataGridViewBOLETOS.Rows[n].Cells["folioname"].Value = res.Get("FOLIO");
                        dataGridViewBOLETOS.Rows[n].Cells["lineaname"].Value = res.Get("LINEA");
                        dataGridViewBOLETOS.Rows[n].Cells["origename"].Value = res.Get("ORIGEN");
                        dataGridViewBOLETOS.Rows[n].Cells["destinoname"].Value = res.Get("DESTINOBOLETO");
                        dataGridViewBOLETOS.Rows[n].Cells["salidaname"].Value = res.Get("SALIDA");
                        dataGridViewBOLETOS.Rows[n].Cells["asientoname"].Value = res.Get("ASIENTO");
                        dataGridViewBOLETOS.Rows[n].Cells["tarifaname"].Value = res.Get("TARIFA");
                        dataGridViewBOLETOS.Rows[n].Cells["precionname"].Value = res.Get("PRECIO");
                        dataGridViewBOLETOS.Rows[n].Cells["statusname"].Value = res.Get("STATUS");
                        dataGridViewBOLETOS.Rows[n].Cells["autobusname"].Value = res.Get("ECO");

                        dataGridViewBOLETOS.Rows[n].Cells["pagoname"].Value = res.Get("FORMADEPAGO");
                        dataGridViewBOLETOS.Rows[n].Cells["foliotarjetaname"].Value = res.Get("FOLIOTARJETA");
                        dataGridViewBOLETOS.Rows[n].Cells["digitoname"].Value = res.Get("DIGITOSTARJETA");
                        if (dataGridViewBOLETOS.Rows[n].Cells["statusname"].Value.ToString() == "CANCELADO")
                        {
                            dataGridViewBOLETOS.Rows[n].DefaultCellStyle.BackColor = Color.DarkRed;

                        }
                        if (dataGridViewBOLETOS.Rows[n].Cells["statusname"].Value.ToString() != "CANCELADO")
                        {
                            sum += (double.TryParse(res.Get("PRECIO"), out double aux1)) ? res.GetDouble("PRECIO") : 0.0;
                        }
                        if (canc == "100")
                        {
                            sum += (double.TryParse(res.Get("PRECIO"), out double aux1)) ? res.GetDouble("PRECIO") : 0.0;

                        }
                        textBoxsum.Text = Utilerias.Utilerias.formatCurrency(sum);
                        //textBoxcancelados.Text = cancelados.ToString();
                        //textBoxtotal.Text = "$" + sum.ToString();
                    }
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

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxsum.Text = "";
            textBoxbuscar.Text = "";
            getRows();
        }

        private void textBoxbuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == (char)Keys.Enter))
            {
                try
                {


                    textBoxsum.Text = "";
                    string _searchtool = Convert.ToString(textBoxbuscar.Text);

                    string sql = "SELECT FOLIO,LINEA,ORIGEN,DESTINOBOLETO,SALIDA,TARIFA,ASIENTO,PRECIO, " +
                                "STATUS,ECO,FORMADEPAGO,FOLIOTARJETA,DIGITOSTARJETA FROM VENDIDOS WHERE CORTE = 1 AND VENDEDOR =@VENDEDOR and " +
                                "FECHAC BETWEEN CONVERT(datetime, @FECHAINI, 121) AND CONVERT(datetime, @FECHAFINAL,121) ";

                    dataGridViewBOLETOS.Rows.Clear();

                    if (!string.IsNullOrEmpty(_searchtool))
                    {
                        sql += " AND ((FOLIO LIKE @SEARCH OR TARIFA LIKE @SEARCH OR STATUS LIKE @SEARCH OR FORMADEPAGO LIKE @SEARCH) OR (ASIENTO LIKE @SEARCH) )";
                        }
                    sql += " ORDER BY FOLIO ASC";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SEARCH", "%" + _searchtool + "%");
                    db.command.Parameters.AddWithValue("@VENDEDOR", usuario);
                    db.command.Parameters.AddWithValue("@FECHAFINAL", fechafinal);
                    db.command.Parameters.AddWithValue("@FECHAINI", fechainicio);
                    res = db.getTable();
                    int count = 0;
                    int n = 0;
                    double sum = 0;
                    while (res.Next())

                    {
                        string canc = res.Get("STATUS");
                        if (_searchtool == "CANCELADO")
                        {
                            canc = "100";
                        }
                        if (canc != "CANCELADO" )
                        {
                            n = dataGridViewBOLETOS.Rows.Add();

                            dataGridViewBOLETOS.Rows[n].Cells["folioname"].Value = res.Get("FOLIO");
                            dataGridViewBOLETOS.Rows[n].Cells["lineaname"].Value = res.Get("LINEA");
                            dataGridViewBOLETOS.Rows[n].Cells["origename"].Value = res.Get("ORIGEN");
                            dataGridViewBOLETOS.Rows[n].Cells["destinoname"].Value = res.Get("DESTINOBOLETO");
                            dataGridViewBOLETOS.Rows[n].Cells["salidaname"].Value = res.Get("SALIDA");
                            dataGridViewBOLETOS.Rows[n].Cells["asientoname"].Value = res.Get("ASIENTO");
                            dataGridViewBOLETOS.Rows[n].Cells["tarifaname"].Value = res.Get("TARIFA");
                            dataGridViewBOLETOS.Rows[n].Cells["precionname"].Value = res.Get("PRECIO");
                            dataGridViewBOLETOS.Rows[n].Cells["statusname"].Value = res.Get("STATUS");
                            dataGridViewBOLETOS.Rows[n].Cells["autobusname"].Value = res.Get("ECO");

                            dataGridViewBOLETOS.Rows[n].Cells["pagoname"].Value = res.Get("FORMADEPAGO");
                            dataGridViewBOLETOS.Rows[n].Cells["foliotarjetaname"].Value = res.Get("FOLIOTARJETA");
                            dataGridViewBOLETOS.Rows[n].Cells["digitoname"].Value = res.Get("DIGITOSTARJETA");
                            if (dataGridViewBOLETOS.Rows[n].Cells["statusname"].Value.ToString() == "CANCELADO")
                            {
                                dataGridViewBOLETOS.Rows[n].DefaultCellStyle.BackColor = Color.DarkRed;

                            }
                            if (dataGridViewBOLETOS.Rows[n].Cells["statusname"].Value.ToString() != "CANCELADO")
                            {
                                sum += (double.TryParse(res.Get("PRECIO"), out double aux1)) ? res.GetDouble("PRECIO") : 0.0;
                            }
                            if (canc == "100")
                            {
                                sum += (double.TryParse(res.Get("PRECIO"), out double aux1)) ? res.GetDouble("PRECIO") : 0.0;

                            }
                            textBoxsum.Text = Utilerias.Utilerias.formatCurrency(sum);
                            //textBoxcancelados.Text = cancelados.ToString();
                            //textBoxtotal.Text = "$" + sum.ToString();
                        }
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
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
