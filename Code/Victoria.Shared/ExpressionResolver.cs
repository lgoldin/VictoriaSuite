using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Victoria.Shared
{
    public static class ExpressionResolver
    {
        public static ObservableCollection<commonFDP.ResultadoAjuste> listFdpPreviusAnalisis { get; set; } = null;


        //el parametro R se utiliza unicamente en casos en los que se  haya definido una FDP para el dato que se va a ejecutar ya que se calcula a partir del objeto y no por el parser
        public static void Resolve(StageVariable variable, string expression, double R=0)
        {
            commonFDP.ResultadoAjuste associatedInverse = listFdpPreviusAnalisis.FirstOrDefault(x => x.DatoAsociado == variable.Name);
            if (associatedInverse != null)
            {
                variable.ActualValue = associatedInverse.FDP.getYfroX(R);
            }
            else
            {
                var formulaParser = new FormulaParser.FormulaParser(expression);

                variable.ActualValue = formulaParser.GetValor();
            }

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
