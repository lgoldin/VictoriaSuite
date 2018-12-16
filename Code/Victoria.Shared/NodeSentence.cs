using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Victoria.Shared
{
    public class NodeSentence : Node
    {
        public string Code { get; set; }
        
        public override Node Execute(IList<StageVariable> variables)
        {
            try
            {
                var cultureInfo = new CultureInfo("en-US");
                int indexEqual = this.Code.IndexOf("=");
                
                string sentence = this.Code.Substring(indexEqual + 1).Trim();
                sentence = this.GetSentenceToEvaluate(variables, cultureInfo, sentence);

                string variableStr = this.Code.Substring(0, indexEqual).Trim();
                StageVariable variable = this.GetVariable(variables, cultureInfo, variableStr);

                ExpressionResolver.Resolve(variable, sentence);
                
                return base.Execute(variables);
            }
            catch (Exception exception)
            {
                throw new Exception("Nodo sentencia", exception);
            }
        }

        private StageVariable GetVariable(IList<StageVariable> variables, CultureInfo cultureInfo, string variableStr)
        {
            StageVariable variable = null;

            if (variableStr.Contains("(") && variableStr.Contains(")"))
            {
                MatchCollection match = new Regex(@"(?<=\().+?(?=\))").Matches(this.Code);
                string vectorIndex = match[0].Value;
                var variableIndex = variables.FirstOrDefault(v => v.Name == vectorIndex);
                if (variableIndex != null)
                {
                    variableStr = variableStr.Remove(match[0].Index, match[0].Length).Insert(match[0].Index, variableIndex.ActualValue.ToString(cultureInfo));
                }

                foreach (StageVariableArray varArray in variables.Where(v => v is StageVariableArray).OrderByDescending(x => x.Name.Length))
                {
                    variable = varArray.Variables.FirstOrDefault(x => x.Name == variableStr);
                    if (variable != null)
                    {
                        break;
                    }
                }
            }
            else
            {
                variable = variables.First(x => x.Name == variableStr);
            }

            return variable;
        }

        private string GetSentenceToEvaluate(IList<StageVariable> variables, CultureInfo cultureInfo, string sentence)
        {
            sentence = this.ReplaceCommonVariablesInSentence(variables, cultureInfo, sentence);
            sentence = this.ReplaceArraysVariablesInSentence(variables, cultureInfo, sentence);

            if (sentence.Contains("R"))
            {
                sentence = sentence.Replace("R", new Random().NextDouble().ToString("F6", cultureInfo));
            }

            return sentence;
        }

        private string ReplaceCommonVariablesInSentence(IList<StageVariable> variables, CultureInfo cultureInfo, string sentence)
        {
            var regex = new Regex(@"[A-Za-z]+[A-Za-z0-9]*");
            MatchCollection matches = regex.Matches(sentence);
            int previousPosition = 0;
            foreach (Match match in matches)
            {
                StageVariable variableAux = variables.FirstOrDefault(x => x.Name == match.Value);
                if (variableAux != null)
                {
                    string actualValue = variableAux.ActualValue >= 0 ? variableAux.ActualValue.ToString("F6", cultureInfo).TrimEnd('0') : string.Format("({0})", variableAux.ActualValue.ToString("F6", cultureInfo).TrimEnd('0'));
                    int indexOfComma = actualValue.IndexOf('.');
                    string valueAfterComma = actualValue.Substring(indexOfComma);
                    if (valueAfterComma != null && valueAfterComma.Length == 1 && valueAfterComma == ".")
                    {
                        actualValue = actualValue.Remove(indexOfComma, 1);
                    }

                    sentence = sentence.Remove(match.Index - previousPosition, match.Length).Insert(match.Index - previousPosition, actualValue);
                    previousPosition += match.Length - actualValue.Length;
                }
            }

            return sentence;
        }

        private string ReplaceArraysVariablesInSentence(IList<StageVariable> variables, CultureInfo cultureInfo, string sentence)
        {
            var regex = new Regex(@"[A-Z0-9a-z]+[(][0-9]+[)]");
            MatchCollection matches = regex.Matches(sentence);
            int previousPosition = 0;
            foreach (Match match in matches)
            {
                foreach (StageVariableArray varArray in variables.Where(v => v is StageVariableArray))
                {
                    StageVariable variableAux = varArray.Variables.FirstOrDefault(x => x.Name == match.Value);
                    if (variableAux != null)
                    {
                        string actualValue = variableAux.ActualValue >= 0 ? variableAux.ActualValue.ToString(cultureInfo) : string.Format("({0})", variableAux.ActualValue.ToString(cultureInfo));
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

