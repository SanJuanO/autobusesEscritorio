using ConnectDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses
{
    public partial class update : Form
    {
        database db;
        Uri uri;
        string filename = Path.GetTempPath()+"\\atah.exe";  //@"C:\Temp\atah.exe";
        public update()
        {
            InitializeComponent();
            try
            {
                string sql = "SELECT VALOR,DESCRIPCION FROM VARIABLES WHERE NOMBRE='ACTUALIZACION' ";
                db = new database();
                db.PreparedSQL(sql);
                ResultSet res = db.getTable();
                if (res.Next())
                {
                    uri = new Uri(res.Get("DESCRIPCION"));
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "verificaracutalizacion";
                Utilerias.LOG.write("update", funcion, error);
            }
        }



        private void update_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                WebClient wc = new WebClient();
                wc.DownloadFileAsync(uri, filename);
                //wc.DownloadDataAsync(uri, filename);
                wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
                wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if (progressBar1.Value == progressBar1.Maximum)
            {
                progressBar1.Value = 0;
            }
        }
        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                //MessageBox.Show("Download complete!, running exe", "Completed!");
                Process.Start(filename);
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Unable to download exe, please check your connection", "Download failed!");
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
