using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispensaTodo.Modelos
{
    public class Bebida
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Sabor { get; set; }
        public double Precio { get; set; }
        public int CantidadMaxima { get; set; }
    }
}
