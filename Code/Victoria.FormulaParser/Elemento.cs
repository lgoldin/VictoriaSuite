namespace Victoria.FormulaParser
{
    public abstract class Elemento
    {
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
            if (double.IsNaN(resultado) ||
                double.IsInfinity(resultado) ||
                double.IsNegativeInfinity(resultado) ||
                double.IsPositiveInfinity(resultado))
            {
                throw new IndefinicionMatematicaException(this.Valor(), resultado);
            }

            return resultado;
        }
    }
}
