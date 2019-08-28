using DispensaTodo.Stock;
using DispensaTodo.Vistas;
using System;

namespace DispensaTodo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Configuraciones
            Console.Title = "Disapensa todo";
           
            BebidasStock bebidasStock = new BebidasStock();
            Menus menus = new Menus(bebidasStock);

             menus.MenuInicial();

            Console.ReadKey();
        }
    }
}
