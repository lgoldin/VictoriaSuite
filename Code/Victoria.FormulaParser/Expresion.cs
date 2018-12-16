namespace Victoria.FormulaParser
{
    public abstract class Expresion
    {
        public Expresion(Expresion expresionMadre)
        {
            this.ExpresionMadre = expresionMadre;
            this.Agrupada = false;
        }

        public Expresion ExpresionMadre { get; set; }

        public abstract bool EsBinaria();

        public abstract bool EsUnaria();

        public abstract bool EsFuncion();

        public abstract double GetValor();

        public bool Agrupada { get; set; }

        public Expresion PrimeraExpresion()
        {
            if (this.ExpresionMadre == null)
            {
                return this;
            }

            return this.ExpresionMadre.PrimeraExpresion();
        }
    }
}
