namespace Victoria.FormulaParser
{
    public class ExpresionUnaria : Expresion
    {
        public ExpresionUnaria(
            Expresion expresionMadre,
            Expresion termino,
            ElementoOperador operador) : base(expresionMadre)
        {
            this.Termino = termino;
            this.Operador = operador;
        }

        public Expresion Termino { get; set; }

        public ElementoOperador Operador { get; set; }

        public override bool EsBinaria()
        {
            return false;
        }

        public override bool EsUnaria()
        {
            return true;
        }

        public override string ToJavaScriptString()
        {
            return "(" + this.Operador.Valor() + this.Termino.ToJavaScriptString() + ")";
        }
    }
}
