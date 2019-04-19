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

        private static bool firstStop = true;

        private void waitForDebugCommand()
        {
            XMLParser.setExecutingNode(this.Name); //Aviso que nodo esta ejecutando
            while (!XMLParser.getJumpToNextNode())
            {
                //Tengo que esperar hasta que se se tome una accion si estoy en debug(stepOver,StepInto,etc..)
            }
            XMLParser.setJumpToNextNode(false);
        }
      
        public virtual Node Execute(IList<StageVariable> variables)
        {
            if (this.NextNode != null)
            {
                if (XMLParser.getdebuggingNode())
                {

                    //Me posiciono en el primer nodo con breakpoing
                    if (Node.firstStop && this.HasBreakPoint)
                    {
                        this.waitForDebugCommand();
                        Node.firstStop = false;
                    }
                    else
                    {
                        if (!Node.firstStop)
                        {
                            this.waitForDebugCommand();
                        }
                    }

                    
                   
                }

                return this.NextNode.Execute(variables);
            }

            return null;
        }
    }
}
