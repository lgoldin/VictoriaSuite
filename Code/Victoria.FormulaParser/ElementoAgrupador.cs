namespace Victoria.FormulaParser
{
    public class ElementoAgrupador : Elemento
    {
        private readonly string agrupador;

        public ElementoAgrupador(string agrupador)
        {
            this.agrupador = agrupador;
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
        
        public override string Valor()
        {
            return this.agrupador;
        }
    }
}
