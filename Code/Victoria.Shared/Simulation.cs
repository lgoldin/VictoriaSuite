﻿using System;
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

        private bool stopDebugExecution { get; set; }

        private bool debugginMode = false;

        private List<Diagram> diagrams { get; set; }

        private List<Variable> variables { get; set; }
        
        public event SimulationStatusChangedDelegate SimulationStatusChanged;

        public delegate void SimulationStatusChangedDelegate(object sender, SimulationStatusChangedEventArgs e);

        public List<Stage> Stages { get; set; }

        public Simulation(IList<Diagram> diagrams, Dictionary<string, Variable> variables)
        {
            this.stopDebugExecution = this.debugginMode ? true : false;

            logger.Info("Inicio Simulacion");
            this.diagrams = diagrams.ToList();
            this.variables = variables.Values.ToList();
            if (!this.variables.Any(v => "T".Equals(v.Name)))
            {
                this.variables.Insert(0, new Variable() { ActualValue = 0, InitialValue = 0, Name = "T" });
            }
            logger.Info("Fin Simulacion");
        }

		public Simulation(List<Diagram> diagramas, Dictionary<string, Variable> variables, List<Stage> stages) : this(diagramas, variables)
		{
            this.stopDebugExecution = this.debugginMode ? true : false;
            this.Stages = stages;
		}

        public bool HasStatusChanged()
        {
            logger.Info("Validacion de cambio de estado");
            return this.SimulationStatusChanged != null;
        }

        public void ChangeStatus(SimulationStatus status)
        {
            logger.Info("Inicio Cambiar de estado");
            this.SimulationStatusChanged(this, new SimulationStatusChangedEventArgs(status));
            logger.Info("Fin Cambiar de estado");
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
            logger.Info("Inicio Parar Execucion");
            this.stopExecution = value;
            logger.Info("Fin Parar Execucion");
        }

        public void StopDebugExecution(bool value)
        {
            this.stopDebugExecution = value;
        }

        public bool CanContinue()
        {
            //return this.stopExecution == false;
            return this.debugginMode ? !this.stopDebugExecution : !this.stopExecution;
        }

        public void Update(IStageSimulation stageSimulation)
        {
            logger.Info("Inicio Actualizar");
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
            logger.Info("Fin Actualizar");
        }

        public List<Diagram> GetDiagrams()
        {
            logger.Info("Obtener Diagramas");
            return this.diagrams;
        }

        public List<Variable> GetVariables()
        {
            logger.Info("Obtener Variables");
            return this.variables;
        }

        public void SetVariables(List<Variable> variables)
        {
            this.variables = variables;
        }

        public double GetVariableValue(string name)
        {
            logger.Info("Inicio obtener valor variable");
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
            logger.Info("Fin obtener valor variable");
            return this.GetVariables().First(x => x.Name == name).ActualValue;
        }
    }
}
