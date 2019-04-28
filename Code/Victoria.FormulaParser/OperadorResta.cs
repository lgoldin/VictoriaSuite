using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class OperadorResta : ElementoOperador
    {
        private static string simboloOperador = "-";
        private static readonly int valorDePrecedencia = 4;

        public OperadorResta() { }

        public override bool DeterminaSigno()
        {
            return true;
        }

        protected override double OperarInterno(double terminoIzquierdo, double terminoDerecho)
        {
            return terminoIzquierdo - terminoDerecho;
        }

        protected override double OperarInterno(double termino)
        {
            return -termino;
        }

        protected override double OperarInterno(double[] terminos)
        {
            throw new InvalidOperationException("El operador '" + this.Valor() + "' es binario.");
        }

        public override string Valor()
        {
            return simboloOperador;
        }

        public override bool EsFuncion()
        {
            return false;
        }

        public override bool EsSeparador()
        {
            return false;
        }

        protected override int GetValorDePrecedencia()
        {
            return valorDePrecedencia;
        }
    }
}
