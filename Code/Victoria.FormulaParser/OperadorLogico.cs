using System;
using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public abstract class OperadorLogico : ElementoOperador
    {
        public const double FALSE = 0;
        public const double TRUE = 1;

        protected double AsDouble(bool valor)
        {
            return valor ? TRUE : FALSE;
        }

        protected bool AsBool(double valor)
        {
            return valor == 0 ? false : true;
        }

        public override bool EsNumerico()
        {
            return false;
        }

        public override bool EsOperador()
        {
            return true;
        }

        public override bool EsInicioDeAgrupacion()
        {
            return false;
        }

        public override bool EsFinDeAgrupacion()
        {
            return false;
        }

        public override bool DeterminaSigno()
        {
            return false;
        }

        public override bool EsFuncion()
        {
            return false;
        }

        public override bool EsSeparador()
        {
            return false;
        }
    }
}
