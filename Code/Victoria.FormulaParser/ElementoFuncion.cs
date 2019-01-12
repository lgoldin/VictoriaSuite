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
            };

        public static ElementoFuncion GetFuncion(string funcion)
        {
            if (!matrizDeInstancias.ContainsKey(funcion))
            {
                throw new InvalidOperationException("No se encontró la funcion '" + funcion + "'.");
            }

            return matrizDeInstancias[funcion];
        }

        public abstract double Operar(List<double> argumentos);

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
