using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victoria.Shared
{
    public class NodeReferencia : Node
    {
        public string Code { get; set; }

        public override Node Execute(IList<StageVariable> variables)
        {
            //logger.Info("Ejecutar");
            try
            {
                return this.NextNode;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }
    }
}
