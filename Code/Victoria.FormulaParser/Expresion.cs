using System;

namespace Victoria.FormulaParser
{
    public abstract class Expresion
    {
        public Expresion(Expresion expresionMadre)
        {
            this.ExpresionMadre = expresionMadre;
            this.Agrupada = false;
        }


        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));

        public Expresion ExpresionMadre { get; set; }

        public abstract bool EsBinaria();

        public abstract bool EsUnaria();

        public abstract bool EsFuncion();

        public abstract double GetValor();

        public bool Agrupada { get; set; }

        public Expresion PrimeraExpresion()
        {
            logger.Info("Inicio Primera Expresion");
            if (this.ExpresionMadre == null)
            {

                logger.Info("Fin Primera Expresion");
                return this;
            }

            logger.Info("Fin Primera Expresion");
            return this.ExpresionMadre.PrimeraExpresion();
        }
    }
}
