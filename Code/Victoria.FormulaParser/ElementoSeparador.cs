﻿using System;

namespace Victoria.FormulaParser
{
    public class ElementoSeparador : Elemento
    {
        private readonly string separador;

        public ElementoSeparador(string separador)
        {
            this.separador = separador;
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
        
        public override string Valor()
        {
            return this.separador;
        }

        public override bool EsFuncion()
        {
            return false;
        }

        public override bool EsSeparador()
        {
            return true;
        }
    }
}
