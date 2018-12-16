namespace Victoria.FormulaParser
{
    public abstract class Elemento
    {
        public abstract bool EsNumerico();

        public abstract bool EsOperador();
        
        public abstract bool EsInicioDeAgrupacion();
        
        public abstract bool EsFinDeAgrupacion();
        
        public abstract bool DeterminaSigno();
        
        public abstract string Valor();
    }
}
