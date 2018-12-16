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

            string formulaJavaScript = formulaParser.ToJavaScriptString();

            variable.ActualValue = engine.Execute(formulaJavaScript).GetCompletionValue().AsNumber();
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
