using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class FuncionPi : ElementoFuncion
    {
        private static string simboloOperador = "pi";

        public FuncionPi() { }

        public override string Valor()
        {
            return simboloOperador;
        }

        protected override double OperarInterno(List<double> argumentos)
        {
            if (argumentos.Count != 0)
            {
                throw new InvalidOperationException("La funcion pi() recibió '" + argumentos.Count.ToString() + "' argumentos. [pi()]");
            }

            return Math.PI;
        }
    }
}
