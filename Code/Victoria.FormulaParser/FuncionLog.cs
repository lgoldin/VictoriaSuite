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
            double argumento;
            double logBase;

            switch (argumentos.Count)
            {
                case 1:
                    logBase = 10;
                    break;
                case 2:
                    logBase = argumentos[1];
                    break;
                default:
                    throw new InvalidOperationException("La funcion log() recibió '" + argumentos.Count.ToString() + "' argumentos. [log(argumento) / log(argumento, base)]");
            }

            argumento = argumentos[0];

            return Math.Log(argumento, logBase);
        }
    }
}
