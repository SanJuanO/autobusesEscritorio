using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Autobuses
{
    public static class Image
    {

        //METODO PARA REDIMENSIONAR LA IMAGEN
        public static String Redimensionar(Bitmap imagen)
        {

            try
            {
                string nombre = "user";

                //RUTA DEL DIRECTORIO TEMPORAL
                String DirTemp = Path.GetTempPath() + @"\" + nombre + ".jpg";

                //IMAGEN ORIGINAL A REDIMENSIONAR
                // Bitmap imagen = new Bitmap(Imagen_Original);

                //CREAMOS UN MAPA DE BIT CON LAS DIMENSIONES QUE QUEREMOS PARA LA NUEVA IMAGEN
                //Bitmap nuevaImagen = new Bitmap(imagen.Width, Imagen_Original.Height);

                // Bitmap nuevaImagen = new Bitmap(imagen.Width, imagen.Height);

                //CREAMOS UN NUEVO GRAFICO
                Graphics gr = Graphics.FromImage(imagen);
                //DIBUJAMOS LA NUEVA IMAGEN
                
                gr.DrawImage(imagen, 0, 0, imagen.Width, imagen.Height);
                //LIBERAMOS RECURSOS
                gr.Dispose();
                //GUARDAMOS LA NUEVA IMAGEN ESPECIFICAMOS LA RUTA Y EL FORMATO
                imagen.Save(DirTemp, System.Drawing.Imaging.ImageFormat.Jpeg);
                //LIBERAMOS RECURSOS
                imagen.Dispose();

                return DirTemp;
            }
            catch(Exception e)
            {

                return null;
            }

        }


        //FUNCION PARA CONVERTIR LA IMAGEN A BYTES

        public static Byte[] Imagen_A_Bytes(String ruta)

        {

            FileStream foto = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            Byte[] arreglo = new Byte[foto.Length];

            BinaryReader reader = new BinaryReader(foto);

            arreglo = reader.ReadBytes(Convert.ToInt32(foto.Length));

            return arreglo;

        }

        //FUNCION PARA CONVERTIR DE BYTES A IMAGEN

        public static Bitmap Bytes_A_Imagen(Byte[] ImgBytes)

        {

            Bitmap imagen = null;

            Byte[] bytes = (Byte[])(ImgBytes);

            MemoryStream ms = new MemoryStream(bytes);

            imagen = new Bitmap(ms);

            return imagen;

        }
    }
}
