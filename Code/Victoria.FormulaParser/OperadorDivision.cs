using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class OperadorDivision : ElementoOperador
    {
        private static string simboloOperador = "/";
        private static readonly int valorDePrecedencia = 3;

        public OperadorDivision() { }

        public override bool DeterminaSigno()
        {
            return false;
        }

        protected override double OperarInterno(double terminoIzquierdo, double terminoDerecho)
        {
            return terminoIzquierdo / terminoDerecho;
        }

        protected override double OperarInterno(double termino)
        {
            throw new InvalidOperationException("El operador '" + this.Valor() + "' es binario.");
        }

        protected override double OperarInterno(double[] terminos)
        {
            throw new InvalidOperationException("El operador '" + this.Valor() + "' es binario.");
        }

        public override string Valor()
        {
            return simboloOperador;
        }

        public override bool EsSeparador()
        {
            return false;
        }

        public override bool EsFuncion()
        {
            return false;
        }

        protected override int GetValorDePrecedencia()
        {
            return valorDePrecedencia;
        }
    }
}
