﻿using System;

namespace Victoria.FormulaParser
{
    public class ElementoNumerico : Elemento
    {
        private readonly string numero;

        public ElementoNumerico(string numero)
        {
            //logger.Info("Inicio Elemento Numerico");
            this.numero = numero;
            //logger.Info("Fin Elemento Numerico");
        }

        public override bool EsNumerico()
        {
            return true;
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

        public override bool EsSeparador()
        {
            return false;
        }

        public override string Valor()
        {
            return this.numero;
        }

        public override bool EsFuncion()
        {
            return false;
        }
    }
}
