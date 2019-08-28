using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispensaTodo.Vistas
{
   public class vistaModoAdmin
   {
        public void contraseña()
        {
            int thePassword = 1234, _password = 0;
            bool isNumber;

            do
            {
                Console.Clear();
                Console.WriteLine("Coloque la contrasenia del usuario");
                isNumber = int.TryParse(Console.ReadLine(), out _password);

            } while (_password != thePassword || isNumber == false);
        }

        public void detalleMaquina()
        {
            Console.Clear();
            DateTime fecha, hora;

            double cantidadDinero = 0;
            fecha = DateTime.Now;


            Console.WriteLine("Fecha: " + fecha.ToString("dddd-mmm-yyyy  h-mm-ss"));

        }
    }
}
