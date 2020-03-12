using System;
using System.Collections.Generic;
using System.Linq;

namespace Victoria.Shared
{
    public class NodeRandom : Node
    {
        public string Code { get; set; }

        public override bool canBeDebugged
        {
            get { return true; }
        }

        public override Node Execute(IList<StageVariable> variables, Delegate NotifyUIMethod)
        {
            
            try
            {
                Debug.Debug.instance().execute(this, NotifyUIMethod, variables);
                return base.Execute(variables, NotifyUIMethod);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }
    }
}
