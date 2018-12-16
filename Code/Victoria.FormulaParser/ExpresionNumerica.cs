namespace Victoria.FormulaParser
{
    public class ExpresionNumerica : Expresion
    {
        private readonly string numero;

        public ExpresionNumerica(
            Expresion expresionMadre,
            string numero) : base(expresionMadre)
        {
            this.numero = numero;
        }

        public override bool EsBinaria()
        {
            return false;
        }

        public override bool EsUnaria()
        {
            return false;
        }

        public override string ToJavaScriptString()
        {
            return this.numero;
        }
    }
}
