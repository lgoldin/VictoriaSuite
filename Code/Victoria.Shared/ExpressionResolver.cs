using System;
using Jint;

namespace Victoria.Shared
{
    public static class ExpressionResolver
    {
        public static void Resolve(StageVariable variable, string expression)
        {
            var engine = new Engine();

            var formulaParser = new FormulaParser.FormulaParser(expression);

            variable.ActualValue = formulaParser.GetValor();
        }

        public static bool ResolveBoolen(string expression)
        {
            try
            {
                Jint.Engine engine = new Engine();
                return engine.Execute(expression).GetCompletionValue().AsBoolean();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
