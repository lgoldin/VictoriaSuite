using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static string GetSentenceToEvaluate(IList<StageVariable> variables, CultureInfo cultureInfo, string sentence)
        {
            sentence = ExpressionResolver.ReplaceCommonVariablesInSentence(variables, cultureInfo, sentence);
            sentence = ExpressionResolver.ReplaceArraysVariablesInSentence(variables, cultureInfo, sentence);

            if (sentence.Contains("R"))
            {
                sentence = sentence.Replace("R", new Random().NextDouble().ToString("F6", cultureInfo));
            }

            return sentence;
        }

        private static string ReplaceCommonVariablesInSentence(IList<StageVariable> variables, CultureInfo cultureInfo, string sentence)
        {
            var regex = new Regex(@"[A-Za-z]+[A-Za-z0-9]*f{0,1}");
            MatchCollection matches = regex.Matches(sentence);
            int previousPosition = 0;
            foreach (Match match in matches)
            {
                StageVariable variable = variables.FirstOrDefault(x => x.Name == match.Value);
                if (variable != null)
                {
                    string actualValue = variable.ActualValue.ToString(cultureInfo);
                    sentence = sentence.Remove(match.Index - previousPosition, match.Length).Insert(match.Index - previousPosition, actualValue);
                    previousPosition += match.Length - actualValue.Length;
                }
            }

            return sentence;
        }

        private static string ReplaceArraysVariablesInSentence(IList<StageVariable> variables, CultureInfo cultureInfo, string sentence)
        {
            var regex = new Regex(@"[A-Z0-9a-z]+[(][0-9]+[)]");
            MatchCollection matches = regex.Matches(sentence);
            int previousPosition = 0;
            foreach (Match match in matches)
            {
                foreach (StageVariableArray varArray in variables.Where(v => v is StageVariableArray))
                {
                    StageVariable variable = varArray.Variables.FirstOrDefault(x => x.Name == match.Value);
                    if (variable != null)
                    {
                        string actualValue = variable.ActualValue.ToString(cultureInfo);
                        sentence = sentence.Remove(match.Index - previousPosition, match.Length).Insert(match.Index - previousPosition, actualValue);
                        previousPosition += match.Length - actualValue.Length;
                        break;
                    }
                }
            }

            return sentence;
        }
    }
}
