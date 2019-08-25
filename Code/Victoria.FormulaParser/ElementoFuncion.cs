using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public abstract class ElementoFuncion: Elemento
    {
        private static Dictionary<string, ElementoFuncion> matrizDeInstancias =
            new Dictionary<string, ElementoFuncion>
            {
                { "int", new FuncionInt() },
                { "log", new FuncionLog() },
                { "random", new FuncionRandom() },
                { "sumatoria", new FuncionSumatoria() },
                { "not", new FuncionNot() },
                { "ln", new FuncionLn() },
                { "e", new FuncionE() },
                { "factorial", new FuncionFactorial() },
                { "pi", new FuncionPi() },
            };

        public static ElementoFuncion GetFuncion(string funcion)
        {

            logger.Info("Inicio Obtener Funcion");
            if (!matrizDeInstancias.ContainsKey(funcion))
            {

                logger.Error("Error: No se encontró la funcion '" + funcion );
                throw new InvalidOperationException("No se encontró la funcion '" + funcion + "'.");
            }


            logger.Info("Fin Obtener Funcion");
            return matrizDeInstancias[funcion];
        }

        public double Operar(List<double> argumentos)
        {

            logger.Info("Inicio Operar");
            double resultado = this.OperarInterno(argumentos);


            logger.Info("Fin Operar");
            return this.EvaluacionNaN(resultado);
        }

        protected abstract double OperarInterno(List<double> argumentos);

        public override bool EsNumerico()
        {
            return false;
        }

        public override bool EsOperador()
        {
            return false;
        }

        public override bool EsInicioDeAgrupacion()
        {
            return false;
        }

        public override bool EsFinDeAgrupacion()
        {
            return false;
        }

        public override bool EsFuncion()
        {
            return true;
        }

        public override bool EsSeparador()
        {
            return false;
        }

        public override bool DeterminaSigno()
        {
            return false;
        }
    }
}
