using System;
using System.Collections.Generic;
using System.Linq;

namespace Victoria.FormulaParser
{
    public class FuncionSumatoria : ElementoFuncion
    {
        private static string simboloOperador = "sumatoria";

        public FuncionSumatoria() { }

        public override string Valor()
        {
            return simboloOperador;
        }

        protected override double OperarInterno(List<double> argumentos)
        {
            if (argumentos.Count == 0)
            {
                throw new InvalidOperationException("La funcion sumatoria() recibió '" + argumentos.Count.ToString() + "' argumentos. [sumatoria(número1, número2, ..., númeroN)]");
            }

            return argumentos.Sum();
        }
    }
}
