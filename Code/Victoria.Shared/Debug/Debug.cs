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

        private bool subDiagramHasStarted = false;

        private List<String> nodesToBreakpoint = new List<string> { "nodo_sentencia", "nodo_condicion","nodo_diagrama","nodo_random" };

        public bool conditionResult { get; set; }

        public String debugCommand { get; set; } = "";

        public bool debugModeOn { get; set; }

        public bool canSetDebugColor { get; set; }  = false;

        public bool jumpToNextNode { get; set; }

        public Node executingNode { get; set; }



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

            //En este if deberia poner todos los nodos que no deberian poder debuguearse como nodoInicio
            if (this.executingNode.GetType().ToString() == "Victoria.Shared.Node" ||
                this.executingNode.GetType().ToString() == "Victoria.Shared.NodeReferencia")
                this.jumpToNextNode = true; 

                while (!this.jumpToNextNode)
                {
                    this.canSetDebugColor = true; //Tengo que esperar hasta que se se tome una accion si estoy en debug (stepOver,StepInto,etc..)
                }
                this.canSetDebugColor = false;

                if (this.debugCommand.Equals("Step Over"))
                {

                    if (this.executingNode.GetType().ToString() == "Victoria.Shared.NodeDiagram")
                    {
                        this.subDiagramHasStarted = true;
                        this.jumpToNextNode = true;
                    }
                    else
                    {
                        if (this.subDiagramHasStarted)
                        {
                            if (this.executingNode.NextNode == null)
                            {
                                this.subDiagramHasStarted = false;
                                this.jumpToNextNode = false;
                            }
                        }
                        else
                        {
                            this.jumpToNextNode = false;
                        }
                    }
                }
                if (this.debugCommand.Equals("Step Into"))
                {
                    if (this.executingNode.NextNode == null)
                    {
                        this.jumpToNextNode = true;
                    }
                    else
                    {
                        this.jumpToNextNode = false;
                    }
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
        }

        public List<String>getNodesToBreakpoint(){
            return this.nodesToBreakpoint;
        }

        #endregion
    }
}
