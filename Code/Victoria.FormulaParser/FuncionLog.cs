using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class FuncionLog : ElementoFuncion
    {
        private static string simboloOperador = "log";

        public FuncionLog() { }

        public override string Valor()
        {
            return simboloOperador;
        }

        protected override double OperarInterno(List<double> argumentos)
        {
            if (argumentos.Count != 2)
            {
                throw new InvalidOperationException("La funcion log() recibió '" + argumentos.Count.ToString() + "' argumentos. [log(argumento, base)]");
            }

            return Math.Log(argumentos[0], argumentos[1]);
        }
    }
}
