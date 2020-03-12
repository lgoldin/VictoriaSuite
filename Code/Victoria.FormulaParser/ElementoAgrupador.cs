using System;

namespace Victoria.FormulaParser
{
    public class ElementoAgrupador : Elemento
    {
        private readonly string agrupador;

        public ElementoAgrupador(string agrupador)
        {
            //logger.Info("Inicio Elemento Agrupador");
            this.agrupador = agrupador;
            //logger.Info("Fin Elemento Agrupador");
        }

        public override bool EsNumerico()
        {
            return false;
        }

        public override bool EsOperador()
        {
            return false;
        }

        public override bool EsInicioDeAgrupacion()
        {
            if (this.agrupador == "(")
            {
                return true;
            }

            return false;
        }

        public override bool EsFinDeAgrupacion()
        {
            if (this.agrupador == ")")
            {
                return true;
            }

            return false;
        }

        public override bool DeterminaSigno()
        {
            return false;
        }

        public override bool EsSeparador()
        {
            return false;
        }

        public override string Valor()
        {
            return this.agrupador;
        }

        public override bool EsFuncion()
        {
            return false;
        }
    }
}
