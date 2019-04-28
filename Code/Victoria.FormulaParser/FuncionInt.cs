using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class FuncionInt : ElementoFuncion
    {
        private static string simboloOperador = "int";

        public FuncionInt() { }

        public override string Valor()
        {
            return simboloOperador;
        }

        protected override double OperarInterno(List<double> argumentos)
        {
            if (argumentos.Count != 1)
            {
                throw new InvalidOperationException("La funcion int() recibió '" + argumentos.Count.ToString() + "' argumentos. [int(número)]");
            }

            return Math.Truncate(argumentos[0]);
        }
    }
}
