using System;

namespace Victoria.FormulaParser
{
    public abstract class Elemento
    {


        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));

        public abstract bool EsNumerico();

        public abstract bool EsOperador();
        
        public abstract bool EsInicioDeAgrupacion();
        
        public abstract bool EsFinDeAgrupacion();

        public abstract bool EsFuncion();

        public abstract bool EsSeparador();

        public abstract bool DeterminaSigno();

        public abstract string Valor();

        protected double EvaluacionNaN(double resultado)
        {
            //logger.Info("Inicio Evaluacion Numero a Numero");
            if (double.IsNaN(resultado) ||
                double.IsInfinity(resultado) ||
                double.IsNegativeInfinity(resultado) ||
                double.IsPositiveInfinity(resultado))
            {
                //logger.Error("Error: Evaluacion Numero a Numero");
                throw new IndefinicionMatematicaException(this.Valor(), resultado);
            }
            //logger.Info("Fin Evaluacion Numero a Numero");
            return resultado;
        }
    }
}
