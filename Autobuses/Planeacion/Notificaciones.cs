using ConnectDB;
using Json.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Planeacion
{
    public partial class Notificaciones : Form
    {
        public database db;
        public bool click_all_socios = true;
        public bool click_all_conductores = true;

        public Notificaciones()
        {
            InitializeComponent();
            this.Show();
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            titulo.Text = "Notificaciones";

            labelcargo.Text = LoginInfo.rol;
            timer2.Start();

            if (LoginInfo.privilegios.Any(x => x == "Enviar notificaciones"))
            {
                btnEnviar.Visible = true;
            }
            


            }

        public void getSocios() {

            if (LoginInfo.privilegios.Any(x => x == "Ver socios"))
            {
                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.Invoke(new Action(() => { dataGridView1.Visible = true; }));
                }
                else { dataGridView1.Visible = true; }

                if (btnMarcaSocio.InvokeRequired)
                {
                    btnMarcaSocio.Invoke(new Action(() => { btnMarcaSocio.Visible = true; }));
                }
                else { btnMarcaSocio.Visible = true; }

                string sql = "SELECT USUARIO, CONCAT(NOMBRE,' ',APELLIDOS)NOMBRE,convert(bit,1) AS ENVIAR FROM SOCIOS";
                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.Invoke(new Action(() => { dataGridView1.DataSource = db.Populate(sql);}));
                }
                else
                {
                    dataGridView1.DataSource = db.Populate(sql);
                }
            }
            else
            {
                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.Invoke(new Action(() => { dataGridView1.Visible = false; }));
                }
                else { dataGridView1.Visible = false; }

                if (btnMarcaSocio.InvokeRequired)
                {
                    btnMarcaSocio.Invoke(new Action(() => { btnMarcaSocio.Visible = false; }));
                }
                else { btnMarcaSocio.Visible = false; }
                
            }
                
        }
        public void getChoferes() {

            if (LoginInfo.privilegios.Any(x => x == "ver conductores"))
            {
                if (dataGridView2.InvokeRequired)
                {
                    dataGridView2.Invoke(new Action(() => { dataGridView2.Visible = true; }));
                }
                else { dataGridView2.Visible = true; }

                if (btnMarcaConductor.InvokeRequired)
                {
                    btnMarcaConductor.Invoke(new Action(() => { btnMarcaConductor.Visible = true; }));
                }
                else { btnMarcaConductor.Visible = true; }
                
                

                string sql = "SELECT USUARIO, CONCAT(NOMBRE,' ',APELLIDOS)NOMBRE,convert(bit,1) AS ENVIARC FROM CHOFERES";
                if (dataGridView2.InvokeRequired)
                {
                    dataGridView2.Invoke(new Action(() => { dataGridView2.DataSource = db.Populate(sql);  }));
                }
                else
                {
                    dataGridView2.DataSource = db.Populate(sql);
                    
                }
            }
            else {
                if (dataGridView2.InvokeRequired)
                {
                    dataGridView2.Invoke(new Action(() => { dataGridView2.Visible = false; }));
                }
                else { dataGridView2.Visible = false; }

                if (btnMarcaConductor.InvokeRequired)
                {
                    btnMarcaConductor.Invoke(new Action(() => { btnMarcaConductor.Visible = false; }));
                }
                else { btnMarcaConductor.Visible = false; }
            }
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            getSocios();
        }

        private void BackgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            getChoferes();
        }

        private void SessionClose_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.cerrarSesion(this);
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            click_all_socios = !click_all_socios;
            for (int i = 0; i < dataGridView1.RowCount; i++) {
                dataGridView1.Rows[i].Cells["ENVIAR"].Value = click_all_socios;
            }
            if (click_all_socios) {
                btnMarcaSocio.Text = "Desmarcar todos los socios";
            }else
            {
                btnMarcaSocio.Text = "Marcar todos los socios";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            click_all_conductores = !click_all_conductores;
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                dataGridView2.Rows[i].Cells["ENVIARC"].Value = click_all_conductores;
            }
            if (click_all_conductores)
            {
                btnMarcaConductor.Text = "Desmarcar todos los conductores";
            }
            else
            {
                btnMarcaConductor.Text = "Marcar todos los conductores";
            }
        }

        private async void BtnEnviar_ClickAsync(object sender, EventArgs e)
        {
            List<string> socios = new List<string>();
            List<PARTNERS> partners = new List<PARTNERS>();
            List<PARTNERS> conductores = new List<PARTNERS>();
            if (string.IsNullOrEmpty(tbTitle.Text))
            {
                tbTitle.Focus();
                MessageBox.Show("Falta título de notificación");
                return;
            }
            if (string.IsNullOrEmpty(tbMessage.Text))
            {
                tbMessage.Focus();
                MessageBox.Show("Falta mensaje de notificación");
                return;
            }
            //string notificacion = "\"NOTIFICATION\":{ \"TITLE\":\"" + tbTitle.Text + "\",\"MESSAGE\":\"" + tbMessage.Text + "\"}";
            NOTIFICATION noti = new NOTIFICATION();
            noti.TITLE =tbTitle.Text;
            noti.MESSAGE = tbMessage.Text;

            PARTNERS p;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (Boolean.Parse(dataGridView1.Rows[i].Cells["ENVIAR"].Value.ToString()) ==true) {
                    p = new PARTNERS();
                    p.USUARIO = dataGridView1.Rows[i].Cells["USUARIO"].Value.ToString();
                    partners.Add(p);
                    //socios.Add("{\"USUARIO\":\"" + dataGridView1.Rows[i].Cells["USUARIO"].Value.ToString()+ "\"");
                }
            }
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                if (Boolean.Parse(dataGridView2.Rows[i].Cells["ENVIARC"].Value.ToString()) ==true) {
                    p = new PARTNERS();
                    p.USUARIO = dataGridView2.Rows[i].Cells["CONDUCTOR"].Value.ToString();
                    conductores.Add(p);
                    //socios.Add("{\"USUARIO\":\"" + dataGridView1.Rows[i].Cells["USUARIO"].Value.ToString()+ "\"");
                }
            }

            if (partners.Count == 0) { partners = null; }
            if (conductores.Count == 0) { conductores = null; }

            NotificationDataModel data = new NotificationDataModel();
                data.DRIVERS = conductores;
                data.PARTNERS = partners;
                data.NOTIFICATION=noti;
            await RunAsync(data);
        }

        async Task RunAsync(NotificationDataModel data)
        {
            
            var myContent = JsonNet.Serialize(data);
            var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("https://localhost:44333/api/SendPushNotificationPartners/");
            client.BaseAddress = new Uri("https://appi-atah.azurewebsites.net/api/SendPushNotificationPartners/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var responsevar = "";
                HttpResponseMessage response = await client.PostAsync("https://appi-atah.azurewebsites.net/api/SendPushNotificationPartners/", stringContent);
                //HttpResponseMessage response = await client.PostAsync("https://localhost:44333/api/SendPushNotificationPartners/", stringContent);
                if (response.IsSuccessStatusCode)
                {
                    responsevar = await response.Content.ReadAsStringAsync();
                }
                Respuesta res= JsonNet.Deserialize<Respuesta>(responsevar);
                
                MessageBox.Show(res.mensaje);
                clean();
            }
            catch (Exception e) {
                string error = e.Message;
                MessageBox.Show("Error con el servicio intente màs tarde");
            }

        }
        public void clean() {
            if (tbTitle.InvokeRequired)
            {
                tbTitle.Invoke(new Action(() => tbTitle.Text = ""));
            }
            else {
                tbTitle.Text = "";
            }
            if (tbMessage.InvokeRequired)
            {
                tbMessage.Invoke(new Action(() => tbMessage.Text = ""));
            }
            else {
                tbMessage.Text = "";
            }
        }

        private void Notificaciones_Load(object sender, EventArgs e)
        {
            db = new database();
            this.WindowState = FormWindowState.Maximized;
            timer1.Interval = 1;
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            backgroundWorker1.RunWorkerAsync();
            backgroundWorker2.RunWorkerAsync();
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView2.EnableHeadersVisualStyles = false;
        }

        private void TableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

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

        private void Button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            if (r > -1) {
                int c = e.ColumnIndex;
                if (c == 2) {
                    dataGridView1.Rows[r].Cells[c].Value = !Boolean.Parse(dataGridView1.Rows[r].Cells[c].Value.ToString());
                }
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            if (r > -1)
            {
                int c = e.ColumnIndex;
                if (c == 2)
                {
                    dataGridView2.Rows[r].Cells[c].Value = !Boolean.Parse(dataGridView2.Rows[r].Cells[c].Value.ToString());
                }
            }
        }
    }
    public class PARTNERS {
        public string USUARIO { get; set; }
    }
    public class NOTIFICATION {
        public string TITLE { get; set; }
        public string MESSAGE { get; set; }
        public string DATA { get; set; }
    }
    public class NotificationDataModel
    {
        public NOTIFICATION NOTIFICATION { get; set; }
        public List<PARTNERS> PARTNERS { get; set; }
        public List<PARTNERS> DRIVERS { get; set; }
    }

    public class Respuesta {
        public int result { get; set; }
        public string mensaje { get; set; }
    }
}
