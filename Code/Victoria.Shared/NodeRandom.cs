using System;
using System.Collections.Generic;
using System.Linq;

namespace Victoria.Shared
{
    public class NodeRandom : Node
    {
        public string Code { get; set; }

        public override Node Execute(IList<StageVariable> variables)
        {
            //logger.Info("Ejecutar");
            try
            {
                return base.Execute(variables);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }
    }
}
