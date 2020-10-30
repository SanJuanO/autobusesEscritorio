using ConnectDB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Utilerias
{
    class Utilerias
    {
        public static void cerrarSesion(Form ventana)
        {
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].Name != "Main")
                    Application.OpenForms[i].Close();
            }

            //Program.Form.DesactivarMenu();

            Form form = new Login();
            form.MdiParent = ventana.MdiParent;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
        }


        /// <summary>
        /// Método que exporta a un fichero Excel el contenido de un DataGridView
        /// </summary>
        /// <param name="grd">DataGridView que contiene los datos a exportar</param>
        public static void ExportarDataGridViewExcel(DataGridView grd)
        {
                SaveFileDialog fichero = new SaveFileDialog();
                fichero.Filter = "Excel (*.xls)|*.xls";
                if (fichero.ShowDialog() == DialogResult.OK)
                {
                Microsoft.Office.Interop.Excel.Application aplicacion;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                aplicacion = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplicacion.Workbooks.Add();
                hoja_trabajo =
                    (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);

                int StartCol = 1;
                int StartRow = 1;


                //Write Headers
                for (int j = 0; j < grd.Columns.Count; j++)
                {
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[StartRow, StartCol + j];
                    myRange.Value2 = grd.Columns[j].HeaderText;
                }


                //Recorremos el DataGridView rellenando la hoja de trabajo
                for (int i = 0; i < grd.Rows.Count; i++)
                {
                    for (int j = 0; j < grd.Columns.Count; j++)
                    {
                        hoja_trabajo.Cells[i + 2, j + 1] = (grd.Rows[i].Cells[j].Value!=null)?grd.Rows[i].Cells[j].Value.ToString():"";
                    }
                }
                libros_trabajo.SaveAs(fichero.FileName,
                    Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                //libros_trabajo.Close(true);
                //aplicacion.Quit();
                aplicacion.Visible = true;
            }
        }

        public static void ExportarDataGridViewExcel(DataGridView grd,List<string>ExcluirColumnas)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "Excel (*.xls)|*.xls";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application aplicacion;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                aplicacion = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplicacion.Workbooks.Add();
                hoja_trabajo =
                    (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);

                int StartCol = 1;
                int StartRow = 1;
                int columna = 0;

                //Write Headers
                for (int j = 0; j < grd.Columns.Count; j++)
                {
                    if (!ExcluirColumnas.Contains(grd.Columns[j].Name.ToString()))
                    {
                        Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)hoja_trabajo.Cells[StartRow, StartCol + (columna++)];
                        myRange.Value2 = grd.Columns[j].HeaderText;
                    }
                }

                //Recorremos el DataGridView rellenando la hoja de trabajo
                for (int i = 0; i < grd.Rows.Count; i++)
                {
                columna = 0;
                    for (int j = 0; j < grd.Columns.Count; j++)
                    {
                        if (!ExcluirColumnas.Contains(grd.Columns[j].Name.ToString()))
                        {
                            hoja_trabajo.Cells[i + 2, (columna++) + 1] = (grd.Rows[i].Cells[j].Value != null) ? grd.Rows[i].Cells[j].Value.ToString() : "";
                        }
                    }
                }
                try
                {
                    libros_trabajo.SaveAs(fichero.FileName,
                        Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                }
                catch (Exception e) {
                    MessageBox.Show("¡Error al guardar el excel, guarde manualmente!");

                }
                //libros_trabajo.Close(true);
                //aplicacion.Quit();
                aplicacion.Visible = true;
            }
        }

        internal static void cerrarSesion()
        {
            throw new NotImplementedException();
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static string formatCurrency(double valor) {
            string currencyformat = "";

            currencyformat = valor.ToString("#,0.00", CultureInfo.InvariantCulture);
            if (valor < 0) {
                currencyformat = "-$ "+currencyformat.Replace("-","");
            }
            else {
                currencyformat = "$ " + currencyformat;
            }

            return currencyformat;
        }

    }

    class LOG : ConnectDB.database {

        private static database db;
        private static string sql;
        public static string Error;
        public LOG() {
            db = new database();
            sql = "";
            Error = "";
        }

       
        public static bool write(string Clase, string Funcion, string Descripcion, string Usuario) {

            try
            {
                sql = "INSERT INTO LOG (CLASE,FUNCION,DESCRIPCION,USUARIO) VALUES(@CLASE,@FUNCION,@DESCRIPCION,@USUARIO)";
                LOG.db.PreparedSQL(sql);
                LOG.db.command.Parameters.AddWithValue("@CLASE", Clase);
                db.command.Parameters.AddWithValue("@FUNCION", Funcion);
                db.command.Parameters.AddWithValue("@DESCRIPCION", Descripcion);
                db.command.Parameters.AddWithValue("@USUARIO", Usuario);

                if (db.execute())
                {
                    return true;
                }
            }
            catch (Exception e) {
                Error = "Error Log:" + e.Message;
            }

            return false;

        }

        public static bool write(string Clase, string Funcion, string Descripcion)
        {

            try
            {
                sql = "INSERT INTO LOG (CLASE,FUNCION,DESCRIPCION,USUARIO) VALUES(@CLASE,@FUNCION,@DESCRIPCION,@USUARIO)";
                LOG.db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@CLASE", Clase);
                db.command.Parameters.AddWithValue("@FUNCION", Funcion);
                db.command.Parameters.AddWithValue("@DESCRIPCION", Descripcion);
                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);

                if (db.execute())
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Error = "Error Log:" + e.Message;
            }

            return false;
        }


        public static void acciones(string accion)
        {
            try
            {
                db = new database();
                sql = "INSERT INTO ACCIONES (ACCION,USUARIO) VALUES(@ACCION,@USUARIO)";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ACCION", accion);
                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);

                if (db.execute())
                {
                }
            }
            catch (Exception e)
            {
                Error = "Error Log:" + e.Message;
            }

        }


    } 

}

