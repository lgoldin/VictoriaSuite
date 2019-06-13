using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Victoria.Shared.Debug;

namespace Victoria.Shared
{
    public class Node
    {
        public string Name { get; set; }

        public bool HasBreakPoint { get; set; }

        public Node NextNode { get; set; }

        public virtual bool canBeDebugged
        {
            get { return false; } 
        }

        public virtual Node Execute(IList<StageVariable> variables, Delegate NotifyUIMethod)
        {
            
            if (this.NextNode != null)
            {
                return this.NextNode.Execute(variables, NotifyUIMethod);
            }

            Debug.Debug.instance().execute(this, NotifyUIMethod);
            return null;
        }
    }
}
