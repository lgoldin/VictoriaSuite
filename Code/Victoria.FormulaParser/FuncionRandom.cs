using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class FuncionRandom : ElementoFuncion
    {
        private static string simboloOperador = "random";

        public FuncionRandom() { }

        public override string Valor()
        {
            return simboloOperador;
        }

        protected override double OperarInterno(List<double> argumentos)
        {
            if (argumentos.Count != 0)
            {
                throw new InvalidOperationException("La funcion random() recibió '" + argumentos.Count.ToString() + "' argumentos. [random()]");
            }

            Random rnd = new Random();

            return rnd.NextDouble();
        }
    }
}
