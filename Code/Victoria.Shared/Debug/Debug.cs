using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Victoria.Shared.Debug
{
    public class Debug
    {

        #region Properties

        private static Debug _instance = null;

        private bool isFirstNode = true;

        private bool subDiagramHasStarted = false;

        //DesignerItem usa esta lista para saber a quien les puede dibujar los contornos rojo o azul para el Debug
        private List<String> nodesToBreakpoint = new List<string> { "nodo_sentencia", "nodo_condicion","nodo_diagrama","nodo_random" };

        public bool conditionResult { get; set; } = false;

        public string conditionExpresion { get; set; } = null;

        public bool debugModeOn { get; set; }

        public bool jumpToNextNode { get; set; }

        public Node executingNode { get; set; }

        public ManualResetEvent colorSignalEvent { get; set; } = null;

        public enum Mode
        {
            None,
            Stop,
            StepInto,
            StepOver,
            Continue,
            ConditionedContinue
        }

        public Mode debugCommand { get; set; } = Mode.None;

        #endregion

        public static Debug instance()
        {
            _instance = _instance == null ? new Debug() : _instance;

            return _instance;
        }

        public void initilize()
        {
            this.debugModeOn = true;
            this.isFirstNode = true;
            this.jumpToNextNode = false;
        }

        #region Methods

        public void execute(Node node, Delegate NotifyUIMethod, IList<StageVariable> executionVariables)
        {

            if (this.debugModeOn)
            {
                //Me posiciono en el primer nodo con breakpoint
                if (this.isFirstNode && node.HasBreakPoint)
                {
                    this.waitForCommand(node, executionVariables);
                    this.isFirstNode = false;
                }
                else
                {
                    if (!this.isFirstNode)
                    {
                        this.waitForCommand(node,executionVariables);
                    }
                }

                //Aviso a la vista que cambie el valor de las variables
                NotifyUIMethod.DynamicInvoke();
            }
        }

        private void waitForCommand(Node node, IList<StageVariable> executionVariables)
        {    
            this.executingNode = node;

            if (!this.executingNode.canBeDebugged && !this.subDiagramHasStarted)
            {
                this.jumpToNextNode = true;
            }
            else
            {
                if (this.debugCommand.Equals(Mode.Continue))
                {
                    this.jumpToNextNode = this.executingNode.HasBreakPoint ? false : true;
                }
                if (this.debugCommand.Equals(Mode.ConditionedContinue))
                {
                    Boolean exerciseHasFinished = ExpressionResolver.ResolveBoolen(
                        ExpressionResolver.GetSentenceToEvaluate(executionVariables, new CultureInfo("en-US"), "T > TF")
                        );

                    if (exerciseHasFinished)
                    {
                        this.conditionResult = false;
                        this.jumpToNextNode = false;
                    }
                    else
                    { 
                        string replacedCondition = ExpressionResolver.GetSentenceToEvaluate(executionVariables , new CultureInfo("en-US"), this.conditionExpresion.ToUpper());
                    
                        if (!this.conditionResult)
                        {
                            this.conditionResult = ExpressionResolver.ResolveBoolen(replacedCondition);
                            this.jumpToNextNode = true;
                        }
                        else
                        {
                            if (this.executingNode.HasBreakPoint)
                            {
                                this.conditionResult = false;
                                this.jumpToNextNode = false;
                            }                    
                        }
                    }
                }
            }

            //Tengo que esperar hasta que se se tome una accion si estoy en debug (stepOver,StepInto,etc)
            while (!this.jumpToNextNode)
            {
                this.colorSignalEvent.Set();
                this.colorSignalEvent.Reset();
            }
            

            if (this.debugCommand.Equals(Mode.StepOver))
            {
                if (this.executingNode.GetType().ToString() == "Victoria.Shared.NodeDiagram")
                {
                    this.subDiagramHasStarted = true;
                    this.jumpToNextNode = true;
                }
                else
                {
                    if (!this.subDiagramHasStarted)
                    {
                        this.jumpToNextNode = false;
                    }
                    else
                    {
                        //if (this.executingNode.NextNode == null) 
                       if(this.executingNode.GetType().ToString() == "Victoria.Shared.Node")
                        {
                            this.subDiagramHasStarted = false;
                            this.jumpToNextNode = false;
                        }
                    }
                }
            }
            if (this.debugCommand.Equals(Mode.StepInto))
            {
                this.jumpToNextNode = false;
            }
        }

        public List<String>getNodesToBreakpoint(){
            return this.nodesToBreakpoint;
        }

        #endregion
    }
}
