using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victoria.FormulaParser
{
    public class IndefinicionMatematicaException : Exception
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));

        public IndefinicionMatematicaException(string mensaje, string operador, double valorNoNumerico) : base(mensaje)
        {

            //logger.Info("Inicio Indefinicion Matematica");
            this.Operador = operador;
            this.ValorNoNumerico = valorNoNumerico;
            //logger.Info("Fin Indefinicion Matematica");
        }

        public IndefinicionMatematicaException(string operador, double valorNoNumerico)
            : this("El resultado es indefinido.", operador, valorNoNumerico) { }

        public string Operador { get ; set ; }

        public double ValorNoNumerico { get ; set ; }
   }
}
