using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victoria.Shared.Debug
{
    public class Debug
    {

        #region Properties

        private static Debug _instance = null;

        private bool isFirstNode = true;

        public String debugCommand { get;  set; }

        public bool debugModeOn { get; set; }

        public bool jumpToNextNode { get; set; }

        public Node executingNode { get; set; }

        #endregion


        public static Debug instance()
        {
            _instance =  _instance == null ? new Debug() : _instance;

            return _instance;
        }

        #region Methods

        public void execute(Node node, Delegate NotifyUIMethod)
        {
           
            if ( this.debugModeOn )
            {
                //Me posiciono en el primer nodo con breakpoint
                if (this.isFirstNode && node.HasBreakPoint)
                {
                    this.waitForCommand(node);
                    this.isFirstNode = false;
                }
                else
                {
                    if (!this.isFirstNode)
                    {
                        this.waitForCommand(node);
                    }
                }

                //Aviso a la vista que cambie el valor de las variables
                NotifyUIMethod.DynamicInvoke();
            }
        }

        private void waitForCommand(Node node)
        {
            this.executingNode = node;
            while (!this.jumpToNextNode){
                //Tengo que esperar hasta que se se tome una accion si estoy en debug(stepOver,StepInto,etc..)
            }

            if (this.debugCommand.Equals("Step Over"))
            {
                this.jumpToNextNode = false;
            }
            if (this.debugCommand.Equals("Continue"))
            {
                this.jumpToNextNode = this.executingNode.HasBreakPoint ? false : true;
            }
        }

        //public void Continue()
        //{
        //    this.jumpToNextNode = true;
        //    this.needToStopExecution = this.executingNode.HasBreakPoint ? true : false;
        //}

        #endregion
    }
}
