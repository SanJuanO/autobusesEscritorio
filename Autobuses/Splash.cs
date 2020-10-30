using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();

            timer1.Interval = 4;
            timer1.Start();
        

            }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Form form = new Login();
            form.Show();
            form.Focus();
            this.Hide();
        }
    }
    
}
