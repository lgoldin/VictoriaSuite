using System;

namespace Victoria.Shared
{
    public static class ExpressionResolver
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));
        public static void Resolve(StageVariable variable, string expression)
        {
            try { 
            //logger.Info("Inicio Resolver");
            var formulaParser = new FormulaParser.FormulaParser(expression);
            
            variable.ActualValue = formulaParser.GetValor();
                //logger.Info("Fin Resolver");
            } catch(Exception ex)
            {
                logger.Error("Error Resolver:" + ex.Message);
                throw ex;
            }
        }

        public static bool ResolveBoolen(string expression)
        {
            try
            {
                //logger.Info("Inicio Resolver Boolean");
                var formulaParser = new FormulaParser.FormulaParser(expression);
                //logger.Info("Fin Resolver Boolean");
                return formulaParser.GetValorAsBool();
            }
            catch (Exception exception)
            {   
                logger.Error("Error Resolver Boolean:" + exception.Message);
                throw exception;
            }
        }
    }
}
