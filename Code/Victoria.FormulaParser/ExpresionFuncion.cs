using System.Collections.Generic;
using System.Linq;

namespace Victoria.FormulaParser
{
    public class ExpresionFuncion : Expresion
    {
        private List<Expresion> argumentos;

        public ExpresionFuncion(
            Expresion expresionMadre,
            ElementoFuncion funcion) : base(expresionMadre)
        {
            logger.Info("Inicio Expresion Funcion");
            this.Funcion = funcion;
            this.argumentos = new List<Expresion>();
            logger.Info("Fin Expresion Funcion");
    }

    public ElementoFuncion Funcion { get; set; }
        
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
            return true;
        }

        public void AgregarArgumento(Expresion argumento)
        {
            this.argumentos.Add(argumento);
        }

        public override double GetValor()
        {
            List<double> agumentos = this.argumentos.Select(a => a.GetValor()).ToList();

            double valor = this.Funcion.Operar(agumentos);

            return valor;
        }

        public override string ToString()
        {
            string valor = this.Funcion.Valor() + "(";

            foreach (Expresion argumento in this.argumentos)
            {
                valor += argumento.ToString() + ",";
            }

            if (argumentos.Count > 0)
            {
                valor = valor.Remove(valor.Length - 1);
            }

            valor += ")";
            return valor;
        }
    }
}
