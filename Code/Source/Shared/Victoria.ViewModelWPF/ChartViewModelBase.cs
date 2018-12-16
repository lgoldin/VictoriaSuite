using System.Collections.Generic;
using Victoria.Shared;
using Victoria.Shared.Prism;

namespace Victoria.ViewModelWPF
{
    public abstract class ChartViewModelBase : NotificationObject
    {
        private bool isVisible;
        protected Variable independentVariable;
        protected List<Variable> dependentVariables;
        private string name;

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                this.RaisePropertyChanged("IsVisible");
            }
        }

        public Variable IndependentVariable
        {
            get { return independentVariable; }
            set
            {
                independentVariable = value;
                this.RaisePropertyChanged("IndependentVariable");
            }
        }

        public List<Variable> DependentVariables
        {
            get { return dependentVariables; }
            set
            {
                dependentVariables = value;
                this.RaisePropertyChanged("DependentVariable");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.RaisePropertyChanged("");
            }
        }

        public ChartViewModelBase(string name, Variable independentVariable, List<Variable> dependentVariables)
        {
            this.Name = name;
            this.DependentVariables = dependentVariables;
            this.IndependentVariable = independentVariable;
            independentVariable.ValueChanged += this.IndependentVariableValueChanged;
        }

        protected abstract void IndependentVariableValueChanged(object sender, Shared.EventArgs.VariableValueChangeEventArgs e);

        public abstract void Reset();
    }
}
