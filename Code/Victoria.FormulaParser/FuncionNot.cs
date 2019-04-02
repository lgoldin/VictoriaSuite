using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class FuncionNot : ElementoFuncion
    {
        private static string simboloOperador = "not";

        public FuncionNot() { }

        public override string Valor()
        {
            return simboloOperador;
        }

        protected override double OperarInterno(List<double> argumentos)
        {
            if (argumentos.Count != 1)
            {
                throw new InvalidOperationException("La funcion not() recibió '" + argumentos.Count.ToString() + "' argumentos. [not(booleano)]");
            }

            return argumentos[0] == 0 ? 1 : 0;
        }
    }
}
