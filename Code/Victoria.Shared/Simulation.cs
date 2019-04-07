using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Victoria.Shared.EventArgs;
using Victoria.Shared.Interfaces;

namespace Victoria.Shared
{
    public class Simulation : ISimulation
    {
        private bool stopExecution { get; set; }

        private bool debugginMode = false;

        private List<Diagram> diagrams { get; set; }

        private List<Variable> variables { get; set; }
        
        public event SimulationStatusChangedDelegate SimulationStatusChanged;

        public delegate void SimulationStatusChangedDelegate(object sender, SimulationStatusChangedEventArgs e);

        public List<Stage> Stages { get; set; }

        public Simulation(IList<Diagram> diagrams, Dictionary<string, Variable> variables)
        {
            this.diagrams = diagrams.ToList();
            this.variables = variables.Values.ToList();
            if (!this.variables.Any(v => "T".Equals(v.Name)))
            {
                this.variables.Insert(0, new Variable() { ActualValue = 0, InitialValue = 0, Name = "T" });
            }
        }

		public Simulation(List<Diagram> diagramas, Dictionary<string, Variable> variables, List<Stage> stages) : this(diagramas, variables)
		{
			this.Stages = stages;
		}

        public bool HasStatusChanged()
        {
            return this.SimulationStatusChanged != null;
        }

        public void ChangeStatus(SimulationStatus status)
        {
            this.SimulationStatusChanged(this, new SimulationStatusChangedEventArgs(status));
        }

        // Activa y desactiva el modo debugs
        public bool DebugginMode()
        {
            return this.debugginMode;
        }

        public void SetDebugMode(bool value){
            this.debugginMode = value;
        }

        public void StopExecution(bool value)
        {
            this.stopExecution = value;
        }

        public bool CanContinue()
        {
            return this.stopExecution == false;
        }

        public void Update(IStageSimulation stageSimulation)
        {
            foreach (var variable in stageSimulation.GetVariables())
            {
                if (variable is StageVariableArray)
                {
                    var stageVariableArray = (StageVariableArray)variable;
                    var variableArray = (VariableArray)this.variables.First(v => v.Name == variable.Name);
                    foreach (var v in stageVariableArray.Variables)
                    {
                        variableArray.Variables.First(x => x.Name == v.Name).ActualValue = v.ActualValue;
                    }
                }
                else
                {
                    this.variables.First(v => v.Name == variable.Name).ActualValue = variable.ActualValue;
                }
            }

            this.stopExecution = stageSimulation.GetExecutionStatus();

            if (this.stopExecution)
            {
                this.ChangeStatus(SimulationStatus.Stoped);
            }
        }

        public List<Diagram> GetDiagrams()
        {
            return this.diagrams;
        }

        public List<Variable> GetVariables()
        {
            return this.variables;
        }

        public void SetVariables(List<Variable> variables)
        {
            this.variables = variables;
        }

        public double GetVariableValue(string name)
        {
            var regex = new Regex(@"[A-Z0-9a-z]+[(][0-9]+[)]");
            if (regex.IsMatch(name))
            {
                foreach (var variable in this.GetVariables().Where(x => x is VariableArray))
                {
                    var variableArray = (VariableArray)variable;
                    foreach (var v in variableArray.Variables)
                    {
                        if (v.Name == name)
                        {
                            return v.ActualValue;
                        }
                    }
                }
            }

            return this.GetVariables().First(x => x.Name == name).ActualValue;
        }
    }
}
