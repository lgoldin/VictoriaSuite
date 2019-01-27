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
            double retorno;

            Random rnd = new Random();
            double minimo;
            double maximo;
            switch (argumentos.Count)
            {
                case 0:
                    minimo = 0;
                    maximo = 1;
                    break;
                case 2:
                    minimo = argumentos[0];
                    maximo = argumentos[1];
                    break;
                default:
                    throw new InvalidOperationException("La funcion random() recibió '" + argumentos.Count.ToString() + "' argumentos. [random() / random(mínimo, máximo)]");
            }

            retorno = rnd.NextDouble() * (maximo - minimo) + minimo;

            return retorno;
        }
    }
}
