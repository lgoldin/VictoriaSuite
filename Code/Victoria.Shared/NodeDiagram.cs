using System;
using System.Collections.Generic;

namespace Victoria.Shared
{
    public class NodeDiagram : Node
    {
        public string DiagramName { get; set; }

        public Diagram Diagram { get; set; }

        public bool IsInitializer { get; set; }

        public override bool canBeDebugged
        {
            get { return true; }
        } 

        public override Node Execute(IList<StageVariable> variables, Delegate NotifyUIMethod)
        {
            if (!this.IsInitializer)
            {
                Debug.Debug.instance().execute(this, NotifyUIMethod);
                this.Diagram.Execute(variables, NotifyUIMethod);
            }
             
            return this.NextNode.Execute(variables,  NotifyUIMethod);
        }
    }
}
