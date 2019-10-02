using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Victoria.Shared.EventArgs;
using Victoria.Shared.Interfaces;

namespace Victoria.Shared
{
    public class Simulation : ISimulation
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));
        private bool stopExecution { get; set; }

        private List<Diagram> diagrams { get; set; }

        private List<Variable> variables { get; set; }
        
        public event SimulationStatusChangedDelegate SimulationStatusChanged;

        public delegate void SimulationStatusChangedDelegate(object sender, SimulationStatusChangedEventArgs e);

        public List<Stage> Stages { get; set; }

        public Simulation(IList<Diagram> diagrams, Dictionary<string, Variable> variables)
        {
            try
            {
                //logger.Info("Inicio Simulacion");
                this.diagrams = diagrams.ToList();
                this.variables = variables.Values.ToList();
                if (!this.variables.Any(v => "T".Equals(v.Name)))
                {
                    this.variables.Insert(0, new Variable() { ActualValue = 0, InitialValue = 0, Name = "T" });
                }
                //logger.Info("Fin Simulacion");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

		public Simulation(List<Diagram> diagramas, Dictionary<string, Variable> variables, List<Stage> stages) : this(diagramas, variables)
		{
			this.Stages = stages;
		}

        public bool HasStatusChanged()
        {
            try
            {
                //logger.Info("Validacion de cambio de estado");
                return this.SimulationStatusChanged != null;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        public void ChangeStatus(SimulationStatus status)
        {
            try
            {
                //logger.Info("Inicio Cambiar de estado");
                this.SimulationStatusChanged(this, new SimulationStatusChangedEventArgs(status));
                //logger.Info("Fin Cambiar de estado");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        public void StopExecution(bool value)
        {
            //logger.Info("Inicio Parar Execucion");
            this.stopExecution = value;
            //logger.Info("Fin Parar Execucion");
        }

        public bool CanContinue()
        {
            return this.stopExecution == false;
        }

        public void Update(IStageSimulation stageSimulation)
        {
            try { 
                //logger.Info("Inicio Actualizar");
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
                                      
                    logger.Info("Simulación Detenida (Listado de Variables): " + VariablesToString());                     
                }
                
                //logger.Info("Fin Actualizar");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        public String VariablesToString()
        {
            String cadena = String.Empty;
            foreach(Variable v in this.variables)
            {
                cadena += v.Name + ": " + v.ActualValue.ToString() + " | ";// System.Environment.NewLine;
            }
            return cadena.Remove(cadena.Length-3);
        }

        public List<Diagram> GetDiagrams()
        {
            //logger.Info("Obtener Diagramas");
            return this.diagrams;
        }

        public List<Variable> GetVariables()
        {
            //logger.Info("Obtener Variables");
            return this.variables;
        }

        public void SetVariables(List<Variable> variables)
        {
            this.variables = variables;
        }

        public double GetVariableValue(string name)
        {
            //logger.Info("Inicio obtener valor variable");
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
            //logger.Info("Fin obtener valor variable");
            return this.GetVariables().First(x => x.Name == name).ActualValue;
        }
    }
}
