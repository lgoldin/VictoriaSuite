using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Victoria.Shared
{
    public class NodeCondition : Node
    {
        
        public string Code { get; set; }
        
        public Node ChildNodeFalse { get; set; }

        public Node ChilNodeTrue { get; set; }

        public override bool canBeDebugged
        {
            get { return true; }
        }

        public override Node Execute(IList<StageVariable> variables, Delegate NotifyUIMethod)
        { 
            try
            {
                Debug.Debug.instance().execute(this, NotifyUIMethod, variables);

                logger.Info("Inicio Nodo Condicion");

                var cultureInfo = new CultureInfo("en-US");
                
                string sentence = ExpressionResolver.GetSentenceToEvaluate(variables, cultureInfo, this.Code);

                var result = ExpressionResolver.ResolveBoolen(sentence);

                return result ? this.ChildNodeFalse.Execute(variables, NotifyUIMethod) : this.ChilNodeTrue.Execute(variables, NotifyUIMethod);

                logger.Info("Fin Nodo Condicion");
            }
            catch (Exception exception)
            {

                logger.Error("Error Nodo Condicon:" + exception.Message);
                throw new Exception("Nodo Condicion", exception);
            }
        }
      

        private string GetSentenceToEvaluate(IList<StageVariable> variables, CultureInfo cultureInfo, string sentence)
        {

            logger.Info("Inicio Obtener Sentencia para evaluar");
            sentence = this.ReplaceCommonVariablesInSentence(variables, cultureInfo, sentence);
            sentence = this.ReplaceArraysVariablesInSentence(variables, cultureInfo, sentence);

            if (sentence.Contains("R"))
            {
                sentence = sentence.Replace("R", new Random().NextDouble().ToString("F6", cultureInfo));
            }

            logger.Info("Fin Obtener Sentencia para evaluar");
            return sentence;
        }

        private string ReplaceCommonVariablesInSentence(IList<StageVariable> variables, CultureInfo cultureInfo, string sentence)
        {

            logger.Info("Inicio Reemplazar Variables Comunes");
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
            logger.Info("Fin Reemplazar Variables Comunes");
            return sentence;
        }

        private string ReplaceArraysVariablesInSentence(IList<StageVariable> variables, CultureInfo cultureInfo, string sentence)
        {
            logger.Info("Inicio Reemplazar variables en array");

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
            logger.Info("Fin Reemplazar variables en array");
            return sentence;
        }
    }
}

