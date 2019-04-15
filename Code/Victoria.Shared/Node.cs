using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

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
                
                if (this.HasBreakPoint)
                {
                    
                    XMLParser.setExecutingNode(this.Name); //Aviso que nodo esta ejecutando
                    while (XMLParser.getdebuggingNode() && !XMLParser.getJumpToNextNode() ) {
                        //Tengo que esperar hasta que se se tome una accion si estoy en debug(stepOver,StepInt,etc..)
                        //getJumpToNextNode se pone en true para romper el while
                    }
                    XMLParser.setJumpToNextNode(false);
                }

                return this.NextNode.Execute(variables);
            }

            return null;
        }
    }
}
