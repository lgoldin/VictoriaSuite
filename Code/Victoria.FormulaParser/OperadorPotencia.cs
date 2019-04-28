using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class OperadorPotencia : ElementoOperador
    {
        private static string simboloOperador = "^";
        private static readonly int valorDePrecedencia = 2;

        public OperadorPotencia() { }

        public override bool DeterminaSigno()
        {
            return false;
        }

        protected override double OperarInterno(double terminoIzquierdo, double terminoDerecho)
        {
            return Math.Pow(terminoIzquierdo, terminoDerecho);
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
