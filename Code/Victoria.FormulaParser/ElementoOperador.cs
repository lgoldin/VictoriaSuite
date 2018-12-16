using System.Collections.Generic;

namespace Victoria.FormulaParser
{
    public class ElementoOperador : Elemento
    {
        private static readonly Dictionary<string, bool> matrizDePrecedencia = 
            new Dictionary<string, bool>
            {
                { "++", true},
                { "+-", true},
                { "+*", false},
                { "+/", false},
                { "+^", false},

                { "-+", true},
                { "--", true},
                { "-*", false},
                { "-/", false},
                { "-^", false},

                { "*+", true},
                { "*-", true},
                { "**", true},
                { "*/", true},
                { "*^", false},

                { "/+", true},
                { "/-", true},
                { "/*", true},
                { "//", true},
                { "/^", false},

                { "^+", true},
                { "^-", true},
                { "^*", true},
                { "^/", true},
                { "^^", true},
            };

        private readonly string operador;

        public ElementoOperador(string operador)
        {
            this.operador = operador;
        }

        public bool TienePrecedencia(ElementoOperador operador)
        {
            string entrada = this.Valor() + operador.Valor();

            return matrizDePrecedencia[entrada];
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
            return (this.operador == "-");
        }

        public override string Valor()
        {
            return this.operador;
        }

        public bool EsPotencia()
        {
            return (this.operador == "^");
        }
    }
}
