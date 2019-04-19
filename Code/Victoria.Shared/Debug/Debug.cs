using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victoria.Shared.Debug
{
    class Debug
    {

        #region Properties

        private static Debug _instance = null;

        private bool isFirstNode = true;

        private bool debugModeOn = XMLParser.getdebuggingNode();

        #endregion


        public static Debug instance()
        {
            _instance =  _instance == null ? new Debug() : _instance;

            return _instance;
        }

        #region Methods

        public void execute(Node node)
        {
            if ( this.debugModeOn )
            {
                //Me posiciono en el primer nodo con breakpoint
                if (this.isFirstNode && node.HasBreakPoint)
                {
                    this.waitForCommand(node.Name);
                    this.isFirstNode = false;
                }
                else
                {
                    if (!this.isFirstNode)
                    {
                        this.waitForCommand(node.Name);
                    }
                }               
            }
        }

        private void waitForCommand(String node_id)
        {
            XMLParser.setExecutingNode(node_id); //Aviso que nodo esta ejecutando
            while (!XMLParser.getJumpToNextNode())
            {
                //Tengo que esperar hasta que se se tome una accion si estoy en debug(stepOver,StepInto,etc..)
            }
            XMLParser.setJumpToNextNode(false);            
        }

        #endregion
    }
}
