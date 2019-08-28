using DispensaTodo.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispensaTodo.Stock
{
    public class MonedasStock
    {
        #region Atributos
        private List<Banco> _listaBanco;
        private Banco banco;
        #endregion

        #region Propiedades
        public List<Banco> ListaBanco
        {
            get { return _listaBanco; }
            set { _listaBanco = value; }
        }
        #endregion

        #region constructor
        public MonedasStock()
        {
            ListaBanco = new List<Banco>
            {
                new Banco{Id=5, Denominacion = 5.00, Cantidad=100},
                new Banco {Id=1, Denominacion = 1.00, Cantidad=100},
                new Banco {Id=2,Denominacion = 0.25, Cantidad=100},
                new Banco{Id=3, Denominacion = 0.10, Cantidad=100},
                new Banco {Id = 4,Denominacion = 0.05, Cantidad=100}
            };
        }
        #endregion

        #region Metodos
        public void CalcularVuelto(double totalVuelto)
        {
            double vuelto = totalVuelto;
            int[] monedas = new int[5];

            for (monedas[0] = 0; totalVuelto >= 5.0; monedas[0]++)
            {
                if (ListaBanco[0].Cantidad == 0)
                {
                    break;
                }
                totalVuelto -= 5.0;
                ListaBanco[0].Cantidad--;
            }
            for (monedas[1] = 0; totalVuelto >= 1.0; monedas[1]++)
            {
                if (ListaBanco[1].Cantidad == 0)
                {
                    break;
                }
                totalVuelto -= 1.0;
                ListaBanco[1].Cantidad--;
            }
            for (monedas[2] = 0; totalVuelto >= 0.25; monedas[2]++)
            {
                if (ListaBanco[2].Cantidad == 0)
                {
                    break;
                }
                totalVuelto -= 0.25;
                ListaBanco[2].Cantidad--;
            }
            for (monedas[3] = 0; totalVuelto >= 0.10; monedas[3]++)
            {
                if (ListaBanco[3].Cantidad == 0)
                {
                    break;
                }
                totalVuelto -= 0.10;
                ListaBanco[3].Cantidad--;
            }
            for (monedas[4] = 0; totalVuelto > 0; monedas[4]++)
            {
                if (ListaBanco[4].Cantidad == 0)
                {
                    break;
                }
                totalVuelto -= 0.05;
                ListaBanco[4].Cantidad--;
            }
            Console.SetCursorPosition(88, 23);
            Colorful.Console.WriteLine(vuelto.ToString("N2"), Color.Yellow);
            Console.SetCursorPosition(32, 7);
            Console.WriteLine("Monedas de 5.00: " + monedas[0]);
            Console.SetCursorPosition(32, 8);
            Console.WriteLine("Monedas de 1.00: " + monedas[1]);
            Console.SetCursorPosition(32, 9);
            Console.WriteLine("Monedas de 0.25: " + monedas[2]);
            Console.SetCursorPosition(32, 10);
            Console.WriteLine("Monedas de 0.10: " + monedas[3]);
            Console.SetCursorPosition(32, 11);
            Console.WriteLine("Monedas de 0.05: " + monedas[4]);
        }

        public void AbonarMoneda(double moneda)
        {
            Banco banco = ListaBanco.Find(m => m.Denominacion.Equals(moneda));
            banco.Cantidad++;
        }

        public void administrarBancoMonedas()
        {
            int nCantidad, denoMoneda = 0;
            bool isNumber;
            Banco moneda;

            do
            {
                Console.Clear();
                Console.WriteLine("[1]Cambiar cantidad de monedas de $1.00");
                Console.WriteLine("[2]Cambiar cantidad de monedas de $0.25");
                Console.WriteLine("[3]Cambiar cantidad de monedas de $0.10");
                Console.WriteLine("[4]Cambiar cantidad de monedas de $0.05");
                isNumber = int.TryParse(Console.ReadLine(), out denoMoneda);
                moneda = ListaBanco.Find(m => m.Id.Equals(denoMoneda));
            } while (isNumber == false || moneda == null || denoMoneda < 0);

            do
            {
                Console.Clear();
                Console.WriteLine("Coloque la nueva cantidad de monedas para : " + moneda.Denominacion);
                isNumber = int.TryParse(Console.ReadLine(), out nCantidad);

            } while (isNumber == false || nCantidad < 0);
            Console.ReadKey();

            Console.WriteLine("Cantidad actual de monedas de " + moneda.Denominacion + ": " + moneda.Cantidad);
            Console.WriteLine();
            moneda.Cantidad = nCantidad;
            Console.WriteLine("Nueva Cantidad de monedas de " + moneda.Denominacion + ": " + moneda.Cantidad);
        }

        public void administrarBancoBilletes()
        {
            int nCantidadBilletes, denoBillete = 0;
            bool isNumber;
            Banco billete;


            do
            {
                Console.Clear();
                Console.WriteLine("   [1]Cambiar cantidad de billetes de $1.00");
                Console.WriteLine("   [5]Cambiar cantidad de billetes de $5");

                isNumber = int.TryParse(Console.ReadLine(), out denoBillete);
                billete = ListaBanco.Find(m => m.Id.Equals(denoBillete));
            } while (isNumber == false || billete == null || denoBillete != 5 && denoBillete !=1 );

            do
            {
                Console.WriteLine("Coloque la nueva cantidad de billetes de: $" + billete.Denominacion);
                isNumber = int.TryParse(Console.ReadLine(), out nCantidadBilletes);

            } while (isNumber == false || nCantidadBilletes < 0);
            Console.ReadKey();

            Console.WriteLine("Cantidad actual de Billetes de " + billete.Denominacion + ": " + billete.Cantidad);
            Console.WriteLine();
            billete.Cantidad = nCantidadBilletes;
            Console.WriteLine("Nueva Cantidad de Billetes de " + billete.Denominacion + ": " + billete.Cantidad);

        }
        
        public void detalleBanco()
        {
            double sumaGlobal = 0;
            int i = 1;
            foreach (var item in _listaBanco)
            {
                Console.WriteLine("[{0}] {1} - {2}", i++, item.Denominacion, item.Cantidad);


            }
            Console.WriteLine();
            Console.WriteLine();

            foreach (var item in _listaBanco)
            {
                sumaGlobal += item.Denominacion * item.Cantidad;
            }

            Console.WriteLine("La sumade todo el efectivo de la maquina es: $" + sumaGlobal);
        }
        #endregion
    }
}
