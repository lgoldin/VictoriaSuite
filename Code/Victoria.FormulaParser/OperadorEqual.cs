using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class OperadorEqual : OperadorLogico
    {
        private static string simboloOperador = "==";
        private static readonly int valorDePrecedencia = 6;

        public OperadorEqual() { }

        public override double Operar(double terminoIzquierdo, double terminoDerecho)
        {
            return this.AsDouble(terminoIzquierdo == terminoDerecho);
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

        protected override int GetValorDePrecedencia()
        {
            return valorDePrecedencia;
        }
    }
}
