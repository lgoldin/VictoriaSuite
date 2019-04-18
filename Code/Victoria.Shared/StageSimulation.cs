using System.Collections.Generic;
using System.Linq;
using Victoria.Shared.Interfaces;

namespace Victoria.Shared
{
    public class StageSimulation : IStageSimulation
    {
        private ISimulation simulation { get; set; }
        private bool stopExecution { get; set; }
        private List<Diagram> diagrams { get; set; }
        private List<StageVariable> stageVariables { get; set; }
        
        public StageSimulation(ISimulation simulation)
        {
            this.simulation = simulation;
            this.diagrams = simulation.GetDiagrams().ToList();
            this.Initilize(simulation.GetVariables());
        }

        public bool DebugginMode()
        {
            return this.simulation.DebugginMode();
        }

        public bool GetExecutionStatus()
        {
            return this.stopExecution;
        }

        public void StopExecution(bool value)
        {
            this.stopExecution = value;
        }

        public bool CanContinue()
        {
            return this.stopExecution == false && this.simulation.CanContinue();
        }

        public List<StageVariable> GetVariables()
        {
            return this.stageVariables;
        }

        public Diagram GetMainDiagram()
        {
            return this.diagrams.First(x => x.Name == "Principal"); 
        }

        public bool MustNotifyUI()
        {
            //-- Notificar cada n vueltas
            int n = this.DebugginMode() ? 1 : 20;
            return (int)this.GetVariables().First(variable => variable.Name == "T").ActualValue % n == 0 || !this.CanContinue();
        }

        public ISimulation GetSimulation()
        {
            return this.simulation;
        }

        private void Initilize(List<Variable> variables)
        {
            this.stageVariables = new List<StageVariable>();

            foreach (var variable in variables)
            {
                if (variable is VariableArray)
                {
                    var variableArray = (VariableArray)variable;
                    
                    this.stageVariables.Add(new StageVariableArray
                    {
                        InitialValue = variableArray.InitialValue,
                        ActualValue = variableArray.ActualValue,
                        Name = variableArray.Name,
                        Variables = this.Map(variableArray.Variables)
                    });
                }
                else
                {
                    this.stageVariables.Add(new StageVariable
                    {
                        ActualValue = variable.ActualValue,
                        InitialValue = variable.InitialValue,
                        Name = variable.Name
                    });
                }
            }
        }

        private List<StageVariable> Map(List<Variable> variables)
        {
            return variables.Select(variable => new StageVariable
            {
                Name = variable.Name,
                ActualValue = variable.ActualValue,
                InitialValue = variable.InitialValue
            }).ToList();
        }
    }
}
