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

namespace Autobuses.Reportes
{
    public partial class asientosocupadosboleto : Form
    {
        public database db;
        ResultSet res = null;
        private string _clase = "reportedetalleruta";

        private int pk;
        private string origen;
        private string linea;
        private string fecha;
        private string hora;
        private string eco;
        private string llegada;
        private int disponibles;
        private float aportaciones = 0;
        private int ocupados;
        private float comision;
        private float tot=0;

        private float IVA;
        private float monto = 0;

        private float sumatoria = 0;
        private int cantidadasientos;


        private List<int> pkss = new List<int>();

        public asientosocupadosboleto()
        {
            InitializeComponent();
        }
        public asientosocupadosboleto(int _pk, string _origen, string _linea, string _fecha, string _hora, String _eco)
        {
            db = new database();
            InitializeComponent();
            pk = _pk;
            origen = _origen;
            linea = _linea;
            fecha = _fecha;
            hora = _hora;
            eco = _eco;
            getDatosAdicionales();
            cantidad();
            detalleboletos();
 
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
        
            timer1.Start();
        }



        private void getDatosAdicionales()
        {
            try
            {

                string sql = "SELECT  PK FROM VCORRIDAS_DIA_3 where LINEA = @LINEA AND FECHA=@FECHA  AND AUTOBUS =@ECO AND PK_CORRIDA_RUTA in (SELECT PK_CORRIDA_RUTA FROM VCORRIDAS_DIA_3 WHERE PK = @PK )";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ECO", eco);
                db.command.Parameters.AddWithValue("@LINEA", linea);
                db.command.Parameters.AddWithValue("@FECHA", fecha);
                db.command.Parameters.AddWithValue("@PK", pk);


                res = db.getTable();
              
                while (res.Next())
                {
                    pkss.Add(res.GetInt("PK"));
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

        private void cantidad()
        {
            string sql = "SELECT CANTIDAD_DE_ASIENTOS FROM VAUTOBUSES_1 WHERE ECO=@ECO";
                 db.PreparedSQL(sql);


            db.command.Parameters.AddWithValue("@ECO", eco);






            res = db.getTable();

            if (res.Next())

            {
                cantidadasientos = res.GetInt("CANTIDAD_DE_ASIENTOS");
            }
        }

        private void detalleboletosdestino()
        {

            try
            {
                string valor = string.Empty;
                Color colorActual = Color.FromArgb(0, 0, 64);


                string sql = "SELECT DESTINOBOLETO,count (DESTINOBOLETO) as CANTIDAD FROM VENDIDOS  WHERE STATUS='VENDIDO' AND PKCORRIDA in (" + string.Join(",", pkss) + ")  " +
                    " group by DESTINOBOLETO order by DESTINOBOLETO";
                db.PreparedSQL(sql);




                ocupados = 0;

                int n = 0;

                res = db.getTable();

                while (res.Next())


                {
                    n = dataGridViewdestinos.Rows.Add();

                    dataGridViewdestinos.Rows[n].Cells["destinoname"].Value = res.Get("DESTINOBOLETO");
                    dataGridViewdestinos.Rows[n].Cells["cantidadname"].Value = res.Get("CANTIDAD");
                }

             
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "DETALLEBOLETOSDESTINO";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void valores()
        {
            string sqlA = "SELECT VALOR FROM VARIABLES  WHERE NOMBRE='APORTACIONES' ";
            db.PreparedSQL(sqlA);


            res = db.getTable();
            if (res.Next())
            {
                aportaciones = res.GetFloat("VALOR");
                aportaciones = aportaciones * sumatoria;
            }
            string sql = "SELECT VALOR FROM VARIABLES  WHERE NOMBRE='IVA'";
            db.PreparedSQL(sql);


            res = db.getTable();
            if (res.Next())
            {
                IVA = res.GetFloat("VALOR");
                IVA = 100 + IVA;
            }
            sql = "SELECT CONVERT(FLOAT,VALOR) AS VALOR FROM VARIABLES  WHERE NOMBRE='COMISIONBANCO'";
            db.PreparedSQL(sql);


            res = db.getTable();
            if (res.Next())
            {
                comision = res.GetFloat("VALOR");
            }
            comisiondet();
            textBoxbruto.Text = Utilerias.Utilerias.formatCurrency(tot);
            float txttotal = (tot * 100 / IVA);
           
            float total = txttotal - (aportaciones )-(comision);
            textBoxdisponible.Text = Utilerias.Utilerias.formatCurrency(total);


        }

        private void comisiondet()
        {

            try
            {
                string valor = string.Empty;
                Color colorActual = Color.FromArgb(0, 0, 64);


                string sql = "SELECT CONVERT(FLOAT,PRECIO) as PRECIO FROM VENDIDOS  WHERE (FORMADEPAGO='T. DEBITO' OR FORMADEPAGO='T. CREDITO') AND STATUS='VENDIDO' " +
                    "AND PKCORRIDA  in (" + string.Join(",", pkss) + ")  ";
            
                db.PreparedSQL(sql);

                res = db.getTable();
                int count = 0;
                float sumtemp = 0;
                while (res.Next())


                {
                    sumtemp += res.GetFloat("PRECIO");
                    count++;
        
                }
               float com = ((sumtemp) * comision);
                com = com / 100;
                comision = (com * IVA) / 100;
            
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "detalleboleto";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void detalleboletos()
        {

            try
            {
                string valor = string.Empty;
                Color colorActual = Color.FromArgb(0, 0, 64);


                string sql = "SELECT TARIFA,count (TARIFA) as CANTIDAD, SUM(CONVERT(FLOAT,PRECIO)) AS PRECIO FROM VENDIDOS  WHERE STATUS='VENDIDO' AND PKCORRIDA in (" + string.Join(",",pkss)+ ")  " +
                    " group by tarifa,PRECIO order by TARIFA";
                db.PreparedSQL(sql);




                ocupados = 0;

                int n = 0;
                 sumatoria = 0;

                res = db.getTable();
              
                while (res.Next())


                {
                    n = dataGridViewgeneral.Rows.Add();
                    tot += res.GetFloat("PRECIO");
                    dataGridViewgeneral.Rows[n].Cells["tarifanameg"].Value = res.Get("TARIFA");
                    dataGridViewgeneral.Rows[n].Cells["cantidadnameg"].Value = res.Get("CANTIDAD");
                    sumatoria += res.GetInt("CANTIDAD");
                }

                disponiblestext.Text = (cantidadasientos-sumatoria).ToString();

                ocupadostext.Text = sumatoria.ToString();
                detalleboletos2();
                detalleboletosdestino();
                detalleboletosdestinodetalle();
                valores();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "detalleboleto";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void detalleboletosdestinodetalle()
        {

            try
            {
                string valor = string.Empty;
                Color colorActual = Color.FromArgb(0, 0, 64);


                string sql = "SELECT DESTINOBOLETO,count (DESTINOBOLETO) as CANTIDAD,VENDEDOR  FROM VENDIDOS  WHERE STATUS='VENDIDO' AND PKCORRIDA in (" + string.Join(",", pkss) + ") " +
                    " group by DESTINOBOLETO, VENDEDOR order by VENDEDOR, DESTINOBOLETO";
                db.PreparedSQL(sql);




                ocupados = 0;

                int n = 0;

                res = db.getTable();

                while (res.Next())


                {
                    n = dataGridViewdestinodetalle.Rows.Add();

                    dataGridViewdestinodetalle.Rows[n].Cells["destinodetalle"].Value = res.Get("DESTINOBOLETO");
                    dataGridViewdestinodetalle.Rows[n].Cells["cantidaddetalle"].Value = res.Get("CANTIDAD");
                    dataGridViewdestinodetalle.Rows[n].Cells["vendedordetalle"].Value = res.Get("VENDEDOR");

                }


            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "detalleboletoDESTINODETALLE";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void detalleboletos2()
        {

            try
            {
                string valor = string.Empty;
                Color colorActual = Color.FromArgb(0, 0, 64);


                string sql = "SELECT TARIFA,count (TARIFA) as CANTIDAD,VENDEDOR  FROM VENDIDOS  WHERE STATUS='VENDIDO' AND PKCORRIDA in (" + string.Join(",", pkss) + ") " +
                    " group by tarifa, VENDEDOR order by VENDEDOR, TARIFA";
                db.PreparedSQL(sql);




                ocupados = 0;

                int n = 0;

                res = db.getTable();

                while (res.Next())


                {
                    n = dataGridViewsubg.Rows.Add();

                    dataGridViewsubg.Rows[n].Cells["tarifanamesubg"].Value = res.Get("TARIFA");
                    dataGridViewsubg.Rows[n].Cells["cantidadnamesubg"].Value = res.Get("CANTIDAD");
                    dataGridViewsubg.Rows[n].Cells["vendedornamesubg"].Value = res.Get("VENDEDOR");

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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void PanelBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            moverForm();
        }

        private void asientosocupadosboleto_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F7)
            {
                this.Close();
            }
        }

        private void dataGridViewBOLETOS_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewgeneral_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           

                try
                {
                    string valor = string.Empty;
                    Color colorActual = Color.FromArgb(0, 0, 64);


                    string sql = "SELECT TARIFA,count (TARIFA) as CANTIDAD FROM VENDIDOS  WHERE taSTATUS='VENDIDO' AND PKCORRIDA in (" + string.Join(",", pkss) + ")  " +
                        " group by tarifa order by TARIFA";
                    db.PreparedSQL(sql);




                    ocupados = 0;

                    int n = 0;
                    int sumatoria = 0;

                    res = db.getTable();

                    while (res.Next())


                    {
                        n = dataGridViewgeneral.Rows.Add();

                        dataGridViewgeneral.Rows[n].Cells["tarifanameg"].Value = res.Get("TARIFA");
                        dataGridViewgeneral.Rows[n].Cells["cantidadnameg"].Value = res.Get("CANTIDAD");
                        sumatoria += res.GetInt("CANTIDAD");
                    }

                    disponiblestext.Text = (cantidadasientos - sumatoria).ToString();

                    ocupadostext.Text = sumatoria.ToString();
                }
                catch (Exception err)
                {
                    string error = err.Message;
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                    string funcion = "detalleboleto";
                    Utilerias.LOG.write(_clase, funcion, error);


                }
            
        }

        private void dataGridViewsubg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void asientosocupadosboleto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }
    }


}
