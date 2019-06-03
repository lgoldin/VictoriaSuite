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

        private bool canStopStepOver = false;

        private List<String> nodesToBreakpoint = new List<string> { "nodo_sentencia", "nodo_condicion","nodo_diagrama" };

        public bool conditionResult { get; set; }

        public String debugCommand { get; set; } = "";

        public bool debugModeOn { get; set; }

        public bool jumpToNextNode { get; set; }

        public Node executingNode { get; set; }

        public bool setDebugColor = false;


        #endregion


        public static Debug instance()
        {
            _instance = _instance == null ? new Debug() : _instance;

            return _instance;
        }

        #region Methods

        public void execute(Node node, Delegate NotifyUIMethod)
        {

            if (this.debugModeOn)
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

            if (this.debugCommand.Equals("Step Over") )
            {
                bool isEndNode = false;
                this.jumpToNextNode = this.executingNode.GetType().ToString() == "Diagram" ? true : false;
                isEndNode = this.jumpToNextNode && this.executingNode.NextNode == null ? true : false;
                if (isEndNode)
                {
                    if (!this.canStopStepOver)
                    {
                        this.canStopStepOver = true;
                    }
                    else
                    {
                        this.jumpToNextNode = false;
                        this.canStopStepOver = false;
                        isEndNode = false;
                    }               
                }
             }
            if (this.debugCommand.Equals("Step Into"))
            {
                this.jumpToNextNode = false;
            }
            if (this.debugCommand.Equals("Continue"))
            {
                this.jumpToNextNode = this.executingNode.HasBreakPoint ? false : true;
            }
            if (this.debugCommand.Equals("Conditioned Continue"))
            {
                this.conditionResult = !this.conditionResult && ExpressionResolver.ResolveBoolen("NS > 1") ? true : false;
                this.jumpToNextNode = this.executingNode.HasBreakPoint && this.conditionResult ? false : true;
            }

            while (!this.jumpToNextNode)
            {
                //Tengo que esperar hasta que se se tome una accion si estoy en debug (stepOver,StepInto,etc..)
            }
        }

        public List<String>getNodesToBreakpoint(){
            return this.nodesToBreakpoint;
        }

        #endregion
    }
}
