using System;
using System.Globalization;

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

        public override bool EsFuncion()
        {
            return false;
        }

        public override string ToString()
        {
            return this.numero;
        }

        public override double GetValor()
        {
            double valor;
            if (double.TryParse(this.numero, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out valor))
            {
                return valor;
            }
            else
            {
                //logger.Error("No se pudo convertir a Double el string: "+ this.numero );
                throw new InvalidOperationException("No se pudo convertir a Double el string");
            }
        }
    }
}
