using System;

namespace Victoria.FormulaParser
{
    public class ExpresionUnaria : Expresion
    {
        public ExpresionUnaria(
            Expresion expresionMadre,
            Expresion termino,
            ElementoOperador operador) : base(expresionMadre)
        {
            //logger.Info("Inicio Expresion Unaria");
            this.Termino = termino;
            this.Operador = operador;
            //logger.Info("Fin Expresion Unaria");

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

        public override double GetValor()
        {
            double termino = this.Termino.GetValor();

            double valor = this.Operador.Operar(termino);

            return valor;
        }

        public override bool EsFuncion()
        {
            return false;
        }

        public override string ToString()
        {
            return "(" + this.Operador.Valor() + this.Termino.ToString() + ")";
        }
    }
}
