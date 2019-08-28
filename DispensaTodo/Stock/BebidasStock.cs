using DispensaTodo.Modelos;
using System;
using DispensaTodo.Vistas;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispensaTodo.Stock
{
    class BebidasStock
    {
        #region Atributos
        private List<Bebida> _listaBebidas;
        private Bebida bebida;
        #endregion

        #region Propiedades
        public List<Bebida> ListaBebidas
        {
            get { return _listaBebidas; }
            set { _listaBebidas = value; }
        }
        #endregion

        #region Constructor
        public BebidasStock()
        {
            ListaBebidas = new List<Bebida>
            {
                new Bebida { Id = 1, Tipo = "Soda", Sabor = "Coca cola", Precio = 0.65, CantidadMaxima=100 },
                new Bebida { Id = 2, Tipo = "Soda", Sabor = "Pepsi", Precio = 0.60, CantidadMaxima=100 },
                new Bebida { Id = 3, Tipo = "Soda", Sabor = "Fanta", Precio = 0.45, CantidadMaxima=100 },
                new Bebida { Id = 4, Tipo = "Soda", Sabor = "Tropical UVA", Precio = 0.50, CantidadMaxima=100 },
                new Bebida { Id = 5, Tipo = "Jugo", Sabor = "Del valle", Precio = 0.40, CantidadMaxima=100 },
                new Bebida { Id = 6, Tipo = "Agua", Sabor = "Cristal", Precio = 0.7, CantidadMaxima=100 },
                new Bebida { Id = 7, Tipo = "TÉ", Sabor = "Limón", Precio = 1.00, CantidadMaxima=100 },
                new Bebida { Id = 8, Tipo = "Energizánte", Sabor = "RedBull", Precio = 1.25, CantidadMaxima=100 }
            };
        }
        #endregion

        #region Metodos
        public void ImprimirListaBebidas()
        {
            int i = 1, posicion = 7;
            foreach (var item in ListaBebidas)
            {
                Console.SetCursorPosition(32, posicion);
                Console.WriteLine("[{0}] {1} - ${2:N2}", i++, item.Sabor, item.Precio);
                posicion++;
            }
        }
        public void ImprimirListaBebidasAdmin()
        {
            int i = 1;
            foreach (var item in ListaBebidas)
            {
                Console.WriteLine("[{0}] {1} - {2}", i++, item.Sabor, item.Precio, item.CantidadMaxima);
            }
        }

        public void establecerPrecio()
        {
            Console.Clear();
            string seguir = "";
            int id = 0;
            Menus menuBebidas = new Menus();
            bool isNomber;
            double nPrecio = 0;

            do
            {
                do
                {
                    Console.Clear();
                    menuBebidas.todasLasBebidas();
                    isNomber = int.TryParse(Console.ReadLine(), out id);
                    bebida = ListaBebidas.Find(b => b.Id.Equals(id));
                } while (isNomber == false || bebida == null);

                do
                {
                    Console.Clear();
                    Console.WriteLine("Coloque el nuevo precio para " + bebida.Sabor);
                    isNomber = double.TryParse(Console.ReadLine(), out nPrecio);
                } while (isNomber == false || nPrecio <= 0);

                Console.WriteLine("precio actual " + bebida.Precio);
                bebida.Precio = nPrecio;

                Console.WriteLine("Nuevo precio " + bebida.Precio + "\n");
                ImprimirListaBebidasAdmin();


                Console.WriteLine("Desea establecer precio para otra bebida? s/n");
                seguir = Console.ReadLine();
            } while (seguir == "SI" || seguir == "si" || seguir == "S" || seguir == "s");

        }

        public void establecerCantidadBebidas()
        {
            bool isNomber;
            string seguir = "";
            int id = 0;
            Menus menuBebidas = new Menus();
            int nCantidad = 0;

            do
            {
                do
                {
                    Console.Clear();
                    menuBebidas.todasLasBebidas();
                    isNomber = int.TryParse(Console.ReadLine(), out id);
                    bebida = ListaBebidas.Find(c => c.Id.Equals(id));
                } while (isNomber == false || bebida == null);



                do
                {
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Coloque la nueva cantidad de unidades para " + bebida.Sabor);
                        isNomber = int.TryParse(Console.ReadLine(), out nCantidad);
                    } while (isNomber == false || nCantidad < 0);


                    if (nCantidad > 100)
                    {
                        Console.WriteLine("El limite que puede almacenarse es 100 unidades");

                        Console.ReadKey();
                        Console.Clear();
                    }


                } while (nCantidad > 100);

                Console.WriteLine("cantidad actual: " + bebida.CantidadMaxima);
                bebida.CantidadMaxima = nCantidad;

                Console.WriteLine("Nueva Cantidad " + bebida.CantidadMaxima + "\n");
                Console.ReadKey();
                Console.Clear();

                Console.WriteLine("desea cambiar la cantidad de unidades de otra bebida? s/n");
                seguir = Console.ReadLine();
            } while (seguir == "SI" || seguir == "si" || seguir == "S" || seguir == "s");

        }

        public void totalBebidas()
        {
            int totalBebidas = 0;
            int i = 1;

            //global
            foreach (var item in ListaBebidas)
            {
                Console.WriteLine("[{0}] {1} - {2}", i++, item.Sabor, item.CantidadMaxima);


            }
            Console.WriteLine();
            Console.WriteLine();

            //individual
            foreach (var item in ListaBebidas)
            {
                totalBebidas += item.CantidadMaxima;
            }


            Console.WriteLine("La suma de todas las bebidas es: " + totalBebidas);
        }
        #endregion
    }
}
