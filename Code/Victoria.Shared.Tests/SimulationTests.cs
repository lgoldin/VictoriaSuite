using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Victoria.Shared;
using System.Linq;
using System.Collections.ObjectModel;
using Victoria.Shared.EventArgs;
using Moq;
using Victoria.Shared.Interfaces;

namespace Victoria.Shared.Tests
{
    [TestClass]
	public class SimulationTests
	{
        [TestMethod]
		public void GetVariablesRetrievesAListWithVariableT()
		{
            var simulation = new Simulation(new List<Diagram>(), new Dictionary<string,Variable>());

            List<Variable> variables = simulation.GetVariables();

            Assert.AreEqual(1, variables.Count);
            Assert.AreEqual("T", variables.First().Name);
		}

        [TestMethod]
        public void GetVariablesRetrievesAListWithTFAndT()
        {
            var variables = new Dictionary<string, Variable>();
            variables.Add("TF", new Variable { Name = "TF", ActualValue = 0, InitialValue = 50000 });
            
            var simulation = new Simulation(new List<Diagram>(), variables);

            List<Variable> result = simulation.GetVariables();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("T", result.ElementAt(0).Name);
            Assert.AreEqual(50000, result.ElementAt(1).InitialValue);
            Assert.AreEqual("TF", result.ElementAt(1).Name);
        }

        [TestMethod]
        public void GetDiagramsRetrievesAnEmptyList()
        {
            var simulation = new Simulation(new List<Diagram>(), new Dictionary<string,Variable>());

            List<Diagram> result = simulation.GetDiagrams();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetDiagramsRetrievesAListWithOneElement()
        {
            var diagrams = new List<Diagram> 
            {
                new Diagram { Name = "Principal", Nodes = new ObservableCollection<Node>() }
            };

            var simulation = new Simulation(diagrams, new Dictionary<string, Variable>());

            List<Diagram> result = simulation.GetDiagrams();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Principal", result.First().Name);
        }

        [TestMethod]
        public void CanContinueReturnsTrue()
        {
            var diagrams = new List<Diagram> 
            {
                new Diagram { Name = "Principal", Nodes = new ObservableCollection<Node>() }
            };

            var variables = new Dictionary<string, Variable>();
            variables.Add("TF", new Variable { Name = "TF", ActualValue = 0, InitialValue = 50000 });

            var simulation = new Simulation(diagrams, variables);

            Assert.IsTrue(simulation.CanContinue());
        }

        [TestMethod]
        public void StopExecutionTrueCanContinueReturnsFalse()
        {
            var diagrams = new List<Diagram> 
            {
                new Diagram { Name = "Principal", Nodes = new ObservableCollection<Node>() }
            };

            var variables = new Dictionary<string, Variable>();
            variables.Add("TF", new Variable { Name = "TF", ActualValue = 0, InitialValue = 50000 });

            var simulation = new Simulation(diagrams, variables);
            
            simulation.StopExecution(true);

            Assert.IsFalse(simulation.CanContinue());
        }

        [TestMethod]
        public void StopExecutionFalseCanContinueReturnsTrue()
        {
            var diagrams = new List<Diagram> 
            {
                new Diagram { Name = "Principal", Nodes = new ObservableCollection<Node>() }
            };

            var variables = new Dictionary<string, Variable>();
            variables.Add("TF", new Variable { Name = "TF", ActualValue = 0, InitialValue = 50000 });

            var simulation = new Simulation(diagrams, variables);

            simulation.StopExecution(false);

            Assert.IsTrue(simulation.CanContinue());
        }

        [TestMethod]
        public void HasStatusChangedReturnsFalse()
        {
            var diagrams = new List<Diagram> 
            {
                new Diagram { Name = "Principal", Nodes = new ObservableCollection<Node>() }
            };

            var variables = new Dictionary<string, Variable>();
            variables.Add("TF", new Variable { Name = "TF", ActualValue = 0, InitialValue = 50000 });

            var simulation = new Simulation(diagrams, variables);

            Assert.IsFalse(simulation.HasStatusChanged());
        }

        [TestMethod]
        public void ChangeStatusHasStatusChangedReturnsTrue()
        {
            var diagrams = new List<Diagram> 
            {
                new Diagram { Name = "Principal", Nodes = new ObservableCollection<Node>() }
            };

            var variables = new Dictionary<string, Variable>();
            variables.Add("TF", new Variable { Name = "TF", ActualValue = 0, InitialValue = 50000 });

            var simulation = new Simulation(diagrams, variables);
            simulation.SimulationStatusChanged += SimulationOnSimulationStatusChanged;

            simulation.ChangeStatus(SimulationStatus.Stoped);

            Assert.IsTrue(simulation.HasStatusChanged());
        }

        [TestMethod]
        public void UpdateStopExecution()
        {
            var diagrams = new List<Diagram> 
            {
                new Diagram { Name = "Principal", Nodes = new ObservableCollection<Node>() }
            };

            var variables = new Dictionary<string, Variable>();
            variables.Add("TF", new Variable { Name = "TF", ActualValue = 0, InitialValue = 50000 });

            var simulation = new Simulation(diagrams, variables);
            simulation.SimulationStatusChanged += SimulationOnSimulationStatusChanged;

            var stageVariables = new List<StageVariable> 
            { 
                new StageVariable { Name = "TF", ActualValue = 0, InitialValue = 50000 },
                new StageVariable { Name = "T", ActualValue = 10, InitialValue = 0 }
            };

            var stageSimulation = new Mock<IStageSimulation>();
            stageSimulation.Setup(x => x.GetVariables()).Returns(stageVariables);
            stageSimulation.Setup(x => x.GetExecutionStatus()).Returns(true);

            simulation.Update(stageSimulation.Object);

            Assert.IsTrue(simulation.HasStatusChanged());
            Assert.IsFalse(simulation.CanContinue());
        }

        [TestMethod]
        public void UpdateStopExecutionAndUpdatesVariablesArray()
        {
            var diagrams = new List<Diagram> 
            {
                new Diagram { Name = "Principal", Nodes = new ObservableCollection<Node>() }
            };

            var variables = new Dictionary<string, Variable>();
            variables.Add("NS", new VariableArray { Name = "NS", Variables = new List<Variable> { new Variable { InitialValue = 0, Name = "NS", ActualValue = 0 } } });

            var simulation = new Simulation(diagrams, variables);
            simulation.SimulationStatusChanged += SimulationOnSimulationStatusChanged;

            var stageVariables = new List<StageVariable> 
            { 
                new StageVariableArray { Name = "NS", Variables = new List<StageVariable> { new StageVariable { InitialValue = 0, Name = "NS", ActualValue = 0 } } },
                new StageVariable { Name = "T", ActualValue = 10, InitialValue = 0 }
            };

            var stageSimulation = new Mock<IStageSimulation>();
            stageSimulation.Setup(x => x.GetVariables()).Returns(stageVariables);
            stageSimulation.Setup(x => x.GetExecutionStatus()).Returns(true);

            simulation.Update(stageSimulation.Object);

            Assert.IsTrue(simulation.HasStatusChanged());
            Assert.IsFalse(simulation.CanContinue());
        }

        private void SimulationOnSimulationStatusChanged(object sender, SimulationStatusChangedEventArgs simulationStatusChangedEventArgs)
        {
        }
	}
}

