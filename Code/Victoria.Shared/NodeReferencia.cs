using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victoria.Shared
{
    public class NodeReferencia : Node
    {
        public string Code { get; set; }

        public override bool canBeDebugged
        {
            get { return false; }
        }

        public override Node Execute(IList<StageVariable> variables, Delegate NotifyUIMethod)
        {

            //logger.Info("Ejecutar");
            try
            {
                Debug.Debug.instance().execute(this, NotifyUIMethod, variables);
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
