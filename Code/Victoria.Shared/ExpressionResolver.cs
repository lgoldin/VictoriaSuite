using System;

namespace Victoria.Shared
{
    public static class ExpressionResolver
    {
        public static void Resolve(StageVariable variable, string expression)
        {
            var formulaParser = new FormulaParser.FormulaParser(expression);

            variable.ActualValue = formulaParser.GetValor();
        }

        public static bool ResolveBoolen(string expression)
        {
            try
            {
                var formulaParser = new FormulaParser.FormulaParser(expression);
                return formulaParser.GetValorAsBool();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
