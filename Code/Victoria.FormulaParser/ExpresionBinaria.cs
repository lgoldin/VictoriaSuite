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
            //logger.Info("Inicio Expresion Binaria");
            this.TerminoIzquierdo = terminoIzquierdo;
            this.Operador = operador;
            this.TerminoDerecho = terminoDerecho;
            //logger.Info("Fin Expresion Binaria");
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

        public override bool EsFuncion()
        {
            return false;
        }

        public override double GetValor()
        {
            double terminoIzquierdo = this.TerminoIzquierdo.GetValor();
            double terminoDerecho = this.TerminoDerecho.GetValor();

            double valor = this.Operador.Operar(terminoIzquierdo, terminoDerecho);

            return valor;
        }

        public override string ToString()
        {
            return "(" + this.TerminoIzquierdo.ToString() + this.Operador.Valor() + this.TerminoDerecho.ToString() + ")";
        }
    }
}
