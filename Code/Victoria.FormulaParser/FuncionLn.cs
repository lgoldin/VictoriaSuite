using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class FuncionLn : ElementoFuncion
    {
        private static string simboloOperador = "ln";

        public FuncionLn() { }

        public override string Valor()
        {
            return simboloOperador;
        }

        protected override double OperarInterno(List<double> argumentos)
        {
            if (argumentos.Count != 1)
            {
                throw new InvalidOperationException("La funcion ln() recibió '" + argumentos.Count.ToString() + "' argumentos. [ln(argumento)]");
            }

            return Math.Log(argumentos[0]);
        }
    }
}
