using System.Collections.Generic;
using System.Linq;
using Victoria.Shared.Interfaces;

namespace Victoria.Shared
{

    public class StageSimulation : IStageSimulation
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(System.AppDomain));
        private ISimulation simulation { get; set; }
        private bool stopExecution { get; set; }
        private List<Diagram> diagrams { get; set; }
        private List<StageVariable> stageVariables { get; set; }

        public StageSimulation(ISimulation simulation)
        {
            logger.Info("Inicio Escenario Simulacion");
            this.simulation = simulation;
            this.diagrams = simulation.GetDiagrams().ToList();
            this.Initilize(simulation.GetVariables());
            logger.Info("Fin Escenario Simulacion");
        }

        public bool GetExecutionStatus()
        {
            logger.Info("Inicio Obtener estado Ejeciucion");
            return this.stopExecution;
        }

        public void StopExecution(bool value)
        {
            logger.Info("Inicio Parar Ejecucion");
            this.stopExecution = value;
            logger.Info("Fin Parar Ejecucion");
        }

        public bool CanContinue()
        {
            logger.Info("Inicio Validacion puede continuar");
            return this.stopExecution == false && this.simulation.CanContinue();
        }

        public List<StageVariable> GetVariables()
        {
            return this.stageVariables;
        }

        public Diagram GetMainDiagram()
        {
            logger.Info("Inicio obtener diagrama principal");
            return this.diagrams.First(x => x.Name == "Principal"); 
        }

        public bool MustNotifyUI()
        {
            //-- Notificar cada n vueltas
            int n = 20;
            return (int)this.GetVariables().First(variable => variable.Name == "T").ActualValue % n == 0 || !this.CanContinue();
        }

        public ISimulation GetSimulation()
        {
            return this.simulation;
        }

        private void Initilize(List<Variable> variables)
        {
            logger.Info("Inicio Inicializar");
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
            logger.Info("Fin Inicializar");
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
