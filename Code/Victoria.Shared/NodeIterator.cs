using System;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Victoria.Shared
{
    public class NodeIterator : Node
    {
        private StageVariable variableIteradora;

        public int ValorInicial { get; set; }

        public int Incremento { get; set; }

        public int ValorFinal { get; set; }

        public string VariableName { get; set; }

        public Node IterationNode { get; set; }

        public override bool canBeDebugged
        {
            get { return false; }
        }

        public override Node Execute(IList<StageVariable> variables, Delegate NotifyUIMethod)
        {

            
            try
            {
                Debug.Debug.instance().execute(this, NotifyUIMethod, variables);
                //logger.Info("Inicio Ejecutar Nodo");
                if (this.variableIteradora == null) InicializarVariableIteradora(variables);
                if (this.variableIteradora.ActualValue < this.ValorFinal)
                {
                    this.variableIteradora.ActualValue += this.Incremento;
                    variables.First(v => v.Name == this.VariableName).ActualValue = this.variableIteradora.ActualValue;
                    //logger.Info("Fin Ejecutar Nodo");
                    return this.IterationNode.Execute(variables, NotifyUIMethod);
                }
                else
                {
                    this.variableIteradora.ActualValue = this.ValorInicial;
                    variables.First(v => v.Name == this.VariableName).ActualValue = this.variableIteradora.ActualValue;
                    //logger.Info("Fin Ejecutar Nodo");
                    return base.Execute(variables, NotifyUIMethod);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        private void InicializarVariableIteradora(IList<StageVariable> variables)
        {
            //logger.Info("Inicio Inicializar Variable Iteradora");
            var variable = variables.FirstOrDefault(v => v.Name == this.VariableName);
            if (!string.IsNullOrEmpty(this.VariableName) && variable != null)
            {
                this.variableIteradora = variable;
            }
            else
            {
                this.variableIteradora = new StageVariable();

            }

            this.variableIteradora.InitialValue = this.ValorInicial;
            this.variableIteradora.ActualValue = this.ValorInicial;
            //logger.Info("Fin Incializar Variable Iteradora");
        }
    }
}
