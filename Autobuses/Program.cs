using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses
{
    static class Program
    {
      //  public static Main Form;
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
               Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
      
               Application.Run(new Splash());
            }
                    catch (Exception e)
            {
                string error = e.Message;
            }
        }
    }
}
