﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Commands;
using Victoria.Shared;
using Victoria.Shared.Prism;
using Variable = Victoria.ModelWPF.Variable;
using Victoria.Shared.Interfaces;

namespace Victoria.ViewModelWPF
{
    public abstract class StageViewModelBase : NotificationObject
    {
        private string name;
        private Simulation simulation;
        private ObservableCollection<Variable> variables = new ObservableCollection<Variable>();
        private IList<ChartViewModelBase> charts = new ObservableCollection<ChartViewModelBase>();
        private ChartViewModelBase selectedChart;
        private bool executing;
        private string _filterText;

        protected DelegateCommand addChartCommand;
        protected DelegateCommand deleteChartCommand;
        protected DelegateCommand executeStageCommand;
        protected DelegateCommand stopExecutionStageCommand;
        protected DelegateCommand exportStageCommand;
        protected DelegateCommand addAnimationToCanvasCommand;
        protected DelegateCommand executeAnimationsCommand;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        public Simulation Simulation
        {
            get { return simulation; }
            set
            {
                simulation = value;
                this.RaisePropertyChanged("Simulation");
            }
        }

        public ObservableCollection<Variable> Variables
        {
            get 
            { 
                return variables;
            }

            set
            {
                variables = value;
                this.RaisePropertyChanged("Variables");
            }
        }

        public List<Variable> FilteredVariables
        {
            get
            {
                return string.IsNullOrEmpty(FilterText) ? this.Variables.ToList() : variables.Where(v => v.Name.ToUpper().Contains(FilterText.ToUpper())).ToList();
            }
        }

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                this.RaisePropertyChanged("FilterText");
                this.RaisePropertyChanged("FilteredVariables");
            }
        }

        public IList<ChartViewModelBase> Charts
        {
            get 
            {
                return charts; 
            }

            set
            {
                charts = value;
                this.RaisePropertyChanged("Charts");
            }
        }

        public ChartViewModelBase SelectedChart
        {
            get 
            { 
                return selectedChart; 
            }

            set
            {
                selectedChart = value;
                this.RaisePropertyChanged("SelectedCharts");
            }
        }

        public bool Executing
        {
            get 
            { 
                return executing; 
            }

            set
            {
                executing = value;
                this.RaisePropertyChanged("Executing");
            }
        }

        #region Commands

        /// <summary>
        /// Gets OpenSimulationCommand.
        /// </summary>
        public ICommand AddChartCommand
        {
            get
            {
                return this.addChartCommand;
            }
        }

        /// <summary>
        /// Gets SaveSimulationCommand.
        /// </summary>
        public ICommand DeleteChartCommand
        {
            get
            {
                return this.deleteChartCommand;
            }
        }

        /// <summary>
        /// Gets ExecuteSimulationCommand.
        /// </summary>
        public ICommand ExecuteStageCommand
        {
            get
            {
                return this.executeStageCommand;
            }
        }

        /// <summary>
        /// Gets StopExecutionSimulationCommand.
        /// </summary>
        public ICommand StopExecutionStageCommand
        {
            get
            {
                return this.stopExecutionStageCommand;
            }
        }

        /// <summary>
        /// Gets ExportStageCommand.
        /// </summary>
        public ICommand ExportStageCommand
        {
            get
            {
                return this.exportStageCommand;
            }
        }

        public ICommand AddAnimationToCanvasCommand
        {
            get
            {
                return this.addAnimationToCanvasCommand;
            }
        }

        public ICommand ExecuteAnimationsCommand
        {
            get
            {
                return this.executeAnimationsCommand;
            }
        }

        #endregion
    }
}
