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

        public override Node Execute(IList<StageVariable> variables)
        {
            if (this.variableIteradora == null) InicializarVariableIteradora(variables);
            if (this.variableIteradora.ActualValue < this.ValorFinal)
            {
                this.variableIteradora.ActualValue += this.Incremento;
                variables.First(v => v.Name == this.VariableName).ActualValue = this.variableIteradora.ActualValue;
                return this.IterationNode.Execute(variables);
            }
            else
            {
                this.variableIteradora.ActualValue = this.ValorInicial;
                variables.First(v => v.Name == this.VariableName).ActualValue = this.variableIteradora.ActualValue;
                return base.Execute(variables);
            }

        }

        private void InicializarVariableIteradora(IList<StageVariable> variables)
        {
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
        }
    }
}
