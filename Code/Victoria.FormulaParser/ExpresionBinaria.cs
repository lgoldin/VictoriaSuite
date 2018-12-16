namespace Victoria.FormulaParser
{
    public class ExpresionBinaria : Expresion
    {
        public ExpresionBinaria(
            Expresion expresionMadre,
            Expresion terminoIzquierdo,
            ElementoOperador operador,
            Expresion terminoDerecho) : base(expresionMadre)
        {
            this.TerminoIzquierdo = terminoIzquierdo;
            this.Operador = operador;
            this.TerminoDerecho = terminoDerecho;
        }

        public Expresion TerminoIzquierdo { get; set; }

        public ElementoOperador Operador { get; set; }
        
        public Expresion TerminoDerecho { get; set; }
    
        public override bool EsBinaria()
        {
            return true;
        }

        public override bool EsUnaria()
        {
            return false;
        }

        public override string ToJavaScriptString()
        {
            if (this.Operador.EsPotencia())
            {
                return "Math.pow(" + this.TerminoIzquierdo.ToJavaScriptString() + ", " + this.TerminoDerecho.ToJavaScriptString() + ")";
            }
            else
            {
                return "(" + this.TerminoIzquierdo.ToJavaScriptString() + this.Operador.Valor() + this.TerminoDerecho.ToJavaScriptString() + ")";
            }
        }
    }
}
