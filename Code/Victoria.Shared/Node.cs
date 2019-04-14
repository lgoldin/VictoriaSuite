using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Victoria.Shared
{
    public class Node
    {
        public string Name { get; set; }

        public bool HasBreakPoint { get; set; }

        public Node NextNode { get; set; }

      
        public virtual Node Execute(IList<StageVariable> variables)
        {
            if (this.NextNode != null)
            {
                // Tengo que esperar hasta que se se tome una accion si estoy en debug(stepOver,StepInt,etc..)
                if (this.NextNode.HasBreakPoint)
                {
                    XMLParser.setExecutingNode(this.NextNode.Name);
                    while (XMLParser.getdebuggingNode()) {
                        // Waiting for action
                    }
                    
                }

                return this.NextNode.Execute(variables);
            }

            return null;
        }
    }
}
