using System;
using System.Collections.Generic;
using System.Linq;

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
            Debug.Debug.instance().execute(this, NotifyUIMethod,variables);
            
            //logger.Info("Inicio Ejecutar Nodo");
            if (this.variableIteradora == null)
                    InicializarVariableIteradora(variables);

            if (this.variableIteradora.ActualValue < this.ValorFinal)
            {
                this.variableIteradora.ActualValue += this.Incremento;
                variables.First(v => v.Name == this.VariableName).ActualValue = this.variableIteradora.ActualValue;

                return this.IterationNode.Execute(variables, NotifyUIMethod);
                //logger.Info("Fin Ejecutar Nodo");
            }
            else
            {
                this.variableIteradora.ActualValue = this.ValorInicial;
                variables.First(v => v.Name == this.VariableName).ActualValue = this.variableIteradora.ActualValue;

                return base.Execute(variables, NotifyUIMethod);
                //logger.Info("Fin Ejecutar Nodo");
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
