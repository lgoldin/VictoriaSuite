﻿using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class OperadorGreaterThanOrEqualsTo : OperadorLogico
    {
        private static string simboloOperador = ">=";
        private static readonly int valorDePrecedencia = 5;

        public OperadorGreaterThanOrEqualsTo() { }

        protected override double OperarInterno(double terminoIzquierdo, double terminoDerecho)
        {
            return this.AsDouble(terminoIzquierdo >= terminoDerecho);
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

        protected override int GetValorDePrecedencia()
        {
            return valorDePrecedencia;
        }
    }
}
