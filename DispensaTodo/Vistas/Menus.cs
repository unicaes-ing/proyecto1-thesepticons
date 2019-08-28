using System;
using DispensaTodo.Modelos;
using DispensaTodo.Stock;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace DispensaTodo.Vistas
{
    class Menus
    {
        #region Atributos
        BebidasStock bebidasStock;
        MonedasStock monedasStock;
        Bebida bebida;
        private double precio;
        private double totalDinero;
        #endregion

        #region Constructor
        public Menus(BebidasStock bebidas = null)
        {
            if (bebidas != null)
            {
                bebidasStock = bebidas;
            }
            monedasStock = new MonedasStock();
        }
        #endregion

        #region Menus
        public void MenuInicial()
        {
            int opc;
            bool isNumber;
            do
            {
                Console.Clear();
                Colorful.Console.WriteAscii("Menu");
                Console.WriteLine("1 - Comprar");
                Console.WriteLine("2 - Administracion");
                Console.WriteLine("3 - Salir");
                isNumber = int.TryParse(Console.ReadLine(), out opc);
            } while (isNumber == false || opc > 3 || opc < 0);

            procesarRespuesta(opc);
        }

        //menusVentas
        public void menuVentas()
        {
            bool isNumber;
            int opc;
            do
            {
                do
                {
                    Console.Clear();
                    maquina();

                    //Imprime las info de las bebidas
                    bebidasStock.ImprimirListaBebidas();

                    Console.SetCursorPosition(43, 19);
                    isNumber = int.TryParse(Console.ReadLine(), out opc);
                    bebida = bebidasStock.ListaBebidas.Find(b => b.Id.Equals(opc));
                } while (isNumber == false || opc < -1);


                if (opc == -1)
                {
                    MenuInicial();
                }

                if (bebida != null)
                {
                    if (bebida.CantidadMaxima <= 0)
                    {
                        Console.Clear();
                        maquina();
                        Console.SetCursorPosition(32, 7);
                        Colorful.Console.WriteLine("Se agotaron existencias", Color.DarkRed);
                        Console.SetCursorPosition(32, 8);
                        Colorful.Console.WriteLine("de " + bebida.Sabor, Color.DarkRed);
                        isNumber = false;
                        Console.ReadKey();
                    }
                }
            } while (isNumber == false || bebida == null || opc < 0);

            bool salir = false;
            double moneda = 0.0, vuelto = 0.0;
            totalDinero = 0.0;
            precio = bebida.Precio;

            do
            {
                do
                {
                    ResetearMaquina(precio, totalDinero);

                    Console.SetCursorPosition(88, 7);
                    isNumber = double.TryParse(Console.ReadLine(), out moneda);
                    Console.Beep();
                } while (isNumber == false);

                if (moneda == 0.05 || moneda == 0.10 || moneda == 0.25 || moneda == 1.00 || moneda == 5.00)
                {
                    precio -= moneda;
                    totalDinero += moneda;
                    monedasStock.AbonarMoneda(moneda);
                }
                else
                {
                    if (moneda == -1)
                    {
                        Console.SetCursorPosition(88, 23);
                        Colorful.Console.WriteLine(totalDinero.ToString("N2"), Color.Yellow);
                        Console.ReadKey();

                        CancelarTransaccion();
                    }
                    ResetearMaquina(precio, totalDinero);

                    Console.SetCursorPosition(83, 10);
                    Colorful.Console.WriteLine("Moneda no valida", Color.IndianRed);
                    Console.ReadKey();
                }

                if (Math.Round(precio, 2, MidpointRounding.AwayFromZero) <= 0.0)
                {
                    salir = true;
                    vuelto = Math.Round(totalDinero - bebida.Precio, 2, MidpointRounding.AwayFromZero);
                    animacionBebida(bebida.Id);
                    monedasStock.CalcularVuelto(vuelto);
                    bebida.CantidadMaxima--;
                }
            } while (salir == false);
            
            Console.ReadKey();
        }

        //MenuAdimistrador
        public void menuAdmin()
        {
            string seguir = "";
            vistaModoAdmin administrador = new vistaModoAdmin();
            administrador.contraseña();
            Console.Clear();

            Colorful.Console.WriteAscii("MENU ADMINISTRADOR");
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();

            bool isNumber;
            int opcAdmin;
            do
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("EN QUE TE PUEDO AYUDAR?");
                    Console.WriteLine("[1] Para administrar las bebidas");
                    Console.WriteLine("[2] Para administrar banco de monedas");
                    Console.WriteLine("[3] Para administrar banco de billetes");
                    Console.WriteLine("[4] Para conocer el detalle de la maquina");

                    isNumber = int.TryParse(Console.ReadLine(), out opcAdmin);
                } while (isNumber == false || opcAdmin > 4 || opcAdmin < 0);

                Console.Clear();
                
              
                switch (opcAdmin)
                {
                    case 1:
                        int subMenu;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("[1] Establecer Precios");
                            Console.WriteLine("[2] Establecer Cantidad de unidades");
                            isNumber = int.TryParse(Console.ReadLine(), out subMenu);

                        } while (isNumber == false || subMenu > 2 || subMenu < 0);
                        if (subMenu == 1)
                        {
                            bebidasStock.establecerPrecio();
                        }
                        else
                        {
                            if (subMenu == 2)
                            {
                                bebidasStock.establecerCantidadBebidas();
                            }
                        }
                        break;

                    case 2:
                        monedasStock.administrarBancoMonedas();
                        break;

                    case 3:
                        monedasStock.administrarBancoBilletes();
                        break;

                    case 4:
                        vistaModoAdmin admin = new vistaModoAdmin();
                        admin.detalleMaquina();
                        Console.WriteLine();
                        Console.WriteLine("Detalle Bebidas");
                        bebidasStock.totalBebidas();
                        Console.WriteLine();
                        Console.WriteLine("Detalle Efectivo");
                        monedasStock.detalleBanco();
                        Console.WriteLine();
                        Console.WriteLine();
                        break;

                    case 5:

                        break;
                    default:
                        break;
                }
                Console.WriteLine("Desea hacer otra accion? s/n");
                seguir = Convert.ToString(Console.ReadLine());
            } while (seguir == "s" || seguir == "S");

            MenuInicial();
        }
        #endregion

        #region Metodos
        public void todasLasBebidas()
        {
            Console.WriteLine("[1] Coca-Cola");
            Console.WriteLine("[2] Pepsi");
            Console.WriteLine("[3] Fanta");
            Console.WriteLine("[4] Tropical UVA");
            Console.WriteLine("[5] Del Valle");
            Console.WriteLine("[6] Cristal");
            Console.WriteLine("[7] Limón");
            Console.WriteLine("[8] RedBull");
            Console.WriteLine("De que Bebida quiere establecer valores?");
        }

        private void ResetearMaquina(double precio, double totalDinero)
        {
            Console.Clear();
            maquina();
            Console.SetCursorPosition(32, 7);
            Console.WriteLine("Ingrese el dinero");
            Console.SetCursorPosition(32, 8);
            Console.WriteLine("[1] {0}", bebida.Sabor);
            Console.SetCursorPosition(32, 9);
            Console.Write("Total a pagar: ");
            Colorful.Console.Write("$" + precio.ToString("N2"), Color.Orange);

            Console.SetCursorPosition(85, 12);
            Colorful.Console.WriteLine("Acumulado: " + totalDinero.ToString("N2"), Color.Green);
        }

        private void procesarRespuesta(int opc)
        {
            switch (opc)
            {
                case 1:
                    string resp;
                    menuVentas();
                    do
                    {
                        Console.Clear();
                        maquina();
                        Console.SetCursorPosition(32, 7);
                        Console.WriteLine("Precione 's' para");
                        Console.SetCursorPosition(32, 8);
                        Console.WriteLine("hacer otra compra.");
                        Console.SetCursorPosition(32, 9);
                        resp = Console.ReadLine().ToLower();

                        if (resp.ToLower() == "s") menuVentas();
                    } while (resp == "s");
                    MenuInicial();
                    break;
                case 2:
                    menuAdmin();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
            }
        }

        private void animacionBebida(int id)
        {
            int primer = 26, segundo = 27, tercero = 28;
            Color color = Color.DarkBlue;

            if (id == 1)color = Color.Red;
            if (id == 2)color = Color.Blue;
            if (id == 3)color = Color.Orange;
            if (id == 4)color = Color.Purple;
            if (id == 5)color = Color.DarkOrange;
            if (id == 6)color = Color.WhiteSmoke;
            if (id == 7)color = Color.LightYellow;
            if (id == 8)color = Color.DarkBlue;

            for (int i = 0; i <=2; i++)
            {
                Console.Clear();
                maquina();
                Console.SetCursorPosition(48, primer += i);
                Colorful.Console.WriteLine("███", color);
                Console.SetCursorPosition(48, segundo += i);
                Colorful.Console.WriteLine("███", color);
                Console.SetCursorPosition(48, tercero += i);
                Colorful.Console.WriteLine("███", color);
                Thread.Sleep(1000);
            }
        }

        private void CancelarTransaccion()
        {
            menuVentas();
        }
        #endregion

        //DibujoMaquina
        #region Maquina
        static void maquina()
        {
            Console.WriteLine("*╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗ *");
            Console.WriteLine("*╠═╦══════════════════════════════════════════════════════════════════════════════════════════════════╦═╣ *");
            Console.WriteLine("*╠═╩══════════════════════════════════════════════════════════════════════════════════════════════════╩═╣**");
            Console.WriteLine("*║                                                                                                      ║ *");
            Console.WriteLine("*║                                                                                                      ║ *");
            Console.WriteLine("*║            ╔═══════════╗   ╔═════════════════════════════════╗   ╔═══════════╗   Inserte su Dinero:  ║ *");
            Console.WriteLine("*║            ║           ║   ║                                 ║   ║           ║   ╔═════════╗  ╔╗     ║ *");
            Console.WriteLine("*║            ║           ║   ║                                 ║   ║           ║   ║ $       ║  ║║     ║ *");
            Console.WriteLine("*║            ║           ║   ║                                 ║   ║           ║   ╚═════════╝  ╚╝     ║ *");
            Console.WriteLine("*║            ║ ███ COCA  ║   ║                                 ║   ║ DEL   ███ ║                       ║ *");
            Console.WriteLine("*║            ║ ███ COLA  ║   ║                                 ║   ║ VALLE ███ ║                       ║ *");
            Console.WriteLine("*║            ║ ███       ║   ║                                 ║   ║       ███ ║   ╔═══════════════╗   ║ *");
            Console.WriteLine("*║            ║           ║   ║                                 ║   ║           ║   ║               ║   ║ *");
            Console.WriteLine("*║            ║           ║   ║                                 ║   ║           ║   ╚═══════════════╝   ║ *");
            Console.WriteLine("*║            ║           ║   ║                                 ║   ║           ║                       ║ *");
            Console.WriteLine("*║            ║ ███ PEPSI ║   ║                                 ║   ║CRISTAL███ ║                       ║ *");
            Console.WriteLine("*║            ║ ███       ║   ║                                 ║   ║       ███ ║                       ║ *");
            Console.WriteLine("*║            ║ ███       ║   ║                                 ║   ║       ███ ║                       ║ *");
            Console.WriteLine("*║            ║           ║   ╚═════╦═════════════════════╦═════╝   ║           ║                       ║ *");
            Console.WriteLine("*║            ║           ║         ║                     ║         ║           ║                       ║ *");
            Console.WriteLine("*║            ║           ║         ╚═════════════════════╝         ║           ║                       ║ *");
            Console.WriteLine("*║            ║           ║                                         ║           ║                       ║ *");
            Console.WriteLine("*║            ║ ███ FANTA ║        Precione -1 para cancelar        ║ LIMÓN ███ ║   ╔════════╗          ║ *");
            Console.WriteLine("*║            ║ ███       ║             la transacción              ║       ███ ║   ║ $      ║          ║ *");
            Console.WriteLine("*║            ║ ███       ║                                         ║       ███ ║   ╚════════╝          ║ *");
            Console.WriteLine("*║            ║           ║        ╔═════════════════════════╗      ║           ║     Cambio            ║ *");
            Console.WriteLine("*║            ║           ║        ║                         ║      ║           ║                       ║ *");
            Console.WriteLine("*║            ║           ║        ║                         ║      ║           ║                       ║ *");
            Console.WriteLine("*║            ║           ║        ╚═════════════════════════╝      ║           ║                       ║ *");
            Console.WriteLine("*║            ║           ║                                         ║           ║                       ║ *");
            Console.WriteLine("*║            ║ ███ UVA   ║                                         ║ RED   ███ ║                       ║ *");
            Console.WriteLine("*║            ║ ███       ║                                         ║ BULL  ███ ║                       ║ *");
            Console.WriteLine("*║            ║ ███       ║                                         ║       ███ ║                       ║ *");
            Console.WriteLine("*║            ║           ║                                         ║           ║                       ║ *");
            Console.WriteLine("*║            ╚═══════════╝                                         ╚═══════════╝                       ║ *");
            Console.WriteLine("*║                                                                                                      ║ *");
            Console.WriteLine("*║                                                                                                      ║ *");
            Console.WriteLine("*║                                                                                                      ║ *");
            Console.WriteLine("*║                                                                                                      ║ *");
            Console.WriteLine("*║                                                                                                      ║ *");
            Console.WriteLine("*║                                                                                                      ║ *");
            Console.WriteLine("*║                                                                                                      ║ *");
            Console.WriteLine("*╚═════════════════════════════════════════════════════════════════════════════════════════════════════╦╝ *");
            Console.WriteLine("**╚╦════╦════════════════════════════════════════════════════════════════════════════════════════╦════╦╝***");
            Console.WriteLine("***╚════╝                                                                                        ╚════╝***");

            #region Colors bebidas
            Console.SetCursorPosition(16, 9);
            Colorful.Console.WriteLine("███", Color.Red);
            Console.SetCursorPosition(16, 10);
            Colorful.Console.WriteLine("███", Color.Red);
            Console.SetCursorPosition(16, 11);
            Colorful.Console.WriteLine("███", Color.Red);

            Console.SetCursorPosition(16, 15);
            Colorful.Console.WriteLine("███", Color.Blue);
            Console.SetCursorPosition(16, 16);
            Colorful.Console.WriteLine("███", Color.Blue);
            Console.SetCursorPosition(16, 17);
            Colorful.Console.WriteLine("███", Color.Blue);

            Console.SetCursorPosition(16, 22);
            Colorful.Console.WriteLine("███", Color.Orange);
            Console.SetCursorPosition(16, 23);
            Colorful.Console.WriteLine("███", Color.Orange);
            Console.SetCursorPosition(16, 24);
            Colorful.Console.WriteLine("███", Color.Orange);

            Console.SetCursorPosition(16, 30);
            Colorful.Console.WriteLine("███", Color.Purple);
            Console.SetCursorPosition(16, 31);
            Colorful.Console.WriteLine("███", Color.Purple);
            Console.SetCursorPosition(16, 32);
            Colorful.Console.WriteLine("███", Color.Purple);

            //Derecha
            Console.SetCursorPosition(76, 9);
            Colorful.Console.WriteLine("███", Color.DarkOrange);
            Console.SetCursorPosition(76, 10);
            Colorful.Console.WriteLine("███", Color.DarkOrange);
            Console.SetCursorPosition(76, 11);
            Colorful.Console.WriteLine("███", Color.DarkOrange);

            Console.SetCursorPosition(76, 15);
            Colorful.Console.WriteLine("███", Color.WhiteSmoke);
            Console.SetCursorPosition(76, 16);
            Colorful.Console.WriteLine("███", Color.WhiteSmoke);
            Console.SetCursorPosition(76, 17);
            Colorful.Console.WriteLine("███", Color.WhiteSmoke);

            Console.SetCursorPosition(76, 22);
            Colorful.Console.WriteLine("███", Color.LightYellow);
            Console.SetCursorPosition(76, 23);
            Colorful.Console.WriteLine("███", Color.LightYellow);
            Console.SetCursorPosition(76, 24);
            Colorful.Console.WriteLine("███", Color.LightYellow);

            Console.SetCursorPosition(76, 30);
            Colorful.Console.WriteLine("███", Color.DarkBlue);
            Console.SetCursorPosition(76, 31);
            Colorful.Console.WriteLine("███", Color.DarkBlue);
            Console.SetCursorPosition(76, 32);
            Colorful.Console.WriteLine("███", Color.DarkBlue);

            Console.ResetColor();
            #endregion
        } 
        #endregion
    }
}
