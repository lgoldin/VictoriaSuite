using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victoria.FormulaParser
{
    public class IndefinicionMatematicaException : Exception
    {
        public IndefinicionMatematicaException(string mensaje, string operador, double valorNoNumerico) : base(mensaje)
        {
            this.Operador = operador;
            this.ValorNoNumerico = valorNoNumerico;
        }

        public IndefinicionMatematicaException(string operador, double valorNoNumerico)
            : this("El resultado es indefinido.", operador, valorNoNumerico) { }

        public string Operador { get ; set ; }

        public double ValorNoNumerico { get ; set ; }
   }
}
