using System;
using System.Collections.Generic;
using System.Linq;

namespace Victoria.Shared
{
    public class NodeRandom : Node
    {
        public string Code { get; set; }

        public override Node Execute(IList<StageVariable> variables, Delegate NotifyUIMethod)
        {
            Debug.Debug.instance().execute(this, NotifyUIMethod);
            return base.Execute(variables, NotifyUIMethod);
        }
    }
}
