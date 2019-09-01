using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public abstract class ElementoOperador : Elemento
    {
        private static Dictionary<string, ElementoOperador> matrizDeInstancias =
            new Dictionary<string, ElementoOperador>
            {
                { "+", new OperadorSuma() },
                { "-", new OperadorResta() },
                { "*", new OperadorMultiplicacion() },
                { "/", new OperadorDivision() },
                { "^", new OperadorPotencia() },
                { "%", new OperadorModulo() },
                { "||", new OperadorOr() },
                { "&&", new OperadorAnd() },
                { "==", new OperadorEqual() },
                { "<", new OperadorLessThan() },
                { ">", new OperadorGreaterThan() },
                { "<=", new OperadorLessThanOrEqualsTo() },
                { ">=", new OperadorGreaterThanOrEqualsTo() },
                { "!=", new OperadorNotEqual() },
            };

        public static ElementoOperador GetOperador(string operador)
        {

            //logger.Info("Inicio Obtener Operador");
            if (!matrizDeInstancias.ContainsKey(operador))
            {
                //logger.Error("Error: No se encontro el operador");
                throw new InvalidOperationException("No se encontró el operador '" + operador + "'.");
            }

            //logger.Info("Fin Obtener Operador");
            return matrizDeInstancias[operador];
        }

        public double Operar(double terminoIzquierdo, double terminoDerecho)
        {
            double resultado = this.OperarInterno(terminoIzquierdo, terminoDerecho);

            return this.EvaluacionNaN(resultado);
        }

        public double Operar(double termino)
        {
            return this.OperarInterno(termino);
        }

        public double Operar(double[] terminos)
        {
            return this.OperarInterno(terminos);
        }

        protected abstract double OperarInterno(double terminoIzquierdo, double terminoDerecho);

        protected abstract double OperarInterno(double termino);

        protected abstract double OperarInterno(double[] terminos);

        public bool TienePrecedencia(ElementoOperador operador)
        {
            return this.GetValorDePrecedencia() <= operador.GetValorDePrecedencia();
        }

        public override bool EsNumerico()
        {
            return false;
        }

        public override bool EsOperador()
        {
            return true;
        }

        public override bool EsInicioDeAgrupacion()
        {
            return false;
        }

        public override bool EsFinDeAgrupacion()
        {
            return false;
        }

        protected abstract int GetValorDePrecedencia();
    }
}
