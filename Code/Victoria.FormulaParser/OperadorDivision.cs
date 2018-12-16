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

        public override double Operar(double terminoIzquierdo, double terminoDerecho)
        {
            return terminoIzquierdo / terminoDerecho;
        }

        public override double Operar(double termino)
        {
            throw new InvalidOperationException("El operador '" + this.Valor() + "' es binario.");
        }

        public override double Operar(double[] terminos)
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
