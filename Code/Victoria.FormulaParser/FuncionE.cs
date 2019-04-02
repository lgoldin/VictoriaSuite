using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class FuncionE : ElementoFuncion
    {
        private static string simboloOperador = "e";

        public FuncionE() { }

        public override string Valor()
        {
            return simboloOperador;
        }

        protected override double OperarInterno(List<double> argumentos)
        {
            double exponente;
            switch (argumentos.Count)
            {
                case 0:
                    exponente = 1;
                    break;
                case 1:
                    exponente = argumentos[0];
                    break;
                default:
                    throw new InvalidOperationException("La funcion e() recibió '" + argumentos.Count.ToString() + "' argumentos. [e() / e(exponente)]");
            }

            return Math.Pow(Math.E, exponente);
        }
    }
}
