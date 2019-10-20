using System;
using System.Collections.Generic;

namespace Victoria.Shared
{
    public class NodeResult : Node
    {
        public IEnumerable<string> Variables { get; set; }

        public override bool canBeDebugged
        {
            get { return false; }
        }

        public override Node Execute(IList<StageVariable> variables, Delegate NotifyUIMethod)
        {
            return null;
        }
    }
}
