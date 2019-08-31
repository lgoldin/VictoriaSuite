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
            Debug.Debug.instance().execute(this, NotifyUIMethod,variables);
            logger.Info("Ejecutar");

            return this.NextNode;
        }
    }
}
