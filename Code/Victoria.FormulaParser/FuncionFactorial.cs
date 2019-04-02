using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class FuncionFactorial : ElementoFuncion
    {
        private static string simboloOperador = "factorial";

        public FuncionFactorial() { }

        public override string Valor()
        {
            return simboloOperador;
        }

        protected override double OperarInterno(List<double> argumentos)
        {
            if (argumentos.Count != 1)
            {
                throw new InvalidOperationException("La funcion factorial() recibió '" + argumentos.Count.ToString() + "' argumentos. [factorial(entero positivo)]");
            }

            if (argumentos[0] < 0 ||
                !this.EsEntero(argumentos[0]))
            {
                throw new InvalidOperationException("El argumento de la funcion factorial() debe ser entero positivo. Se recibió '" + argumentos[0].ToString() + "' argumentos. [factorial(entero positivo)]");
            }

            long argumento = (long) argumentos[0];

            long result = 1;
            while (argumento > 1)
            {
                result = result * argumento;
                argumento = argumento - 1;
            }

            return result;
        }

        private bool EsEntero(double numero)
        {
            return numero == Math.Truncate(numero);
        }
    }
}
