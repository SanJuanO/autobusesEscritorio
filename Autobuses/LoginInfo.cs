using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autobuses
{
   
    public static class LoginInfo
    {




        public static string UserID;
        public static string PkUsuario="";
        public static string NombreID="";
        public static string ApellidoID="";
        public static string pkidroles = "";
        public static bool isLoggedIn = false;
        public static string Sucursal="";
        public static string ingreso ="";
        public static int iva = 0;

        public static  byte[] imagenfoto;
        public static string rol;
       public static  List<byte[]> fingerPrintchoferessupra = new List<byte[]>();
        public static List<byte[]> fingerPrintchoferesejecutivo = new List<byte[]>();

       public static List<string> supra = new List<string>();
      public static  List<string> ejecutivo = new List<string>();

        public static List<string> privilegios = new List<string>();



    }




}
