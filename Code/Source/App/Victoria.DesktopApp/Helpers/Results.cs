﻿using System;
using System.Collections.Generic;
using System.Data;
using Victoria.Shared;
using Victoria.Shared.AnalisisPrevio;
using Victoria.ViewModelWPF;

namespace Victoria.DesktopApp.Helpers
{
    public class Results
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(App));
        public String simulationPath { get;  }
        public IList<StageViewModelBase> stages { get; }
        public TimeSpan simulationTotalTime { get;  }
        public String fileName { get;  }

        public Results(String _simulationPath, String _fileName, IList<StageViewModelBase> _stages, TimeSpan _simulationTotalTime)
        {
            //logger.Info("Inicio Resultados");
            this.simulationPath = _simulationPath;
            this.stages = _stages;
            this.simulationTotalTime = _simulationTotalTime;
            this.fileName = _fileName;
            //logger.Info("Fin Resultados");
        }        

        public List<DataTable> createResultsTables(IList<StageViewModelBase> stages)
        {
            //logger.Info("Inicio Crear Tablas de Resultados");
            List<DataTable> tablesList = new List<DataTable>();

            foreach (var stg in stages)
            {
                DataTable table = new DataTable();

                table.TableName = stg.Name;
                table.Columns.Add("Variable de Control");
                table.Columns.Add("Valor");

                DataTable table2 = new DataTable();

                table2.TableName = "";
                table2.Columns.Add("Variable de Resultado");
                table2.Columns.Add("Valor");

                foreach (var variable in stg.Simulation.GetVariables())
                {
                    if (variable.Type == VariableType.Control)
                    {
                        if (variable is VariableArray)
                        {
                            var variableArray = (VariableArray)variable;
                            foreach (var variableAux in variableArray.Variables)
                            {
                                table.Rows.Add(variableAux.Name, variableAux.ActualValue.ToString());
                            }
                        }
                        else
                        {
                            table.Rows.Add(variable.Name, variable.ActualValue.ToString());
                        }
                    }
                    if (variable.Type == VariableType.Result)
                    {
                        if (variable is VariableArray)
                        {
                            var variableArray = (VariableArray)variable;
                            foreach (var variableAux in variableArray.Variables)
                            {
                                table2.Rows.Add(variableAux.Name, variableAux.ActualValue.ToString());
                            }
                        }
                        else
                        {
                            table2.Rows.Add(variable.Name, variable.ActualValue.ToString());
                        }
                    }
                }

                tablesList.Add(table);
                tablesList.Add(table2);
            }
            //logger.Info("Fin Crear Tablas de Resultados");
            return tablesList;
        }
    }
}
