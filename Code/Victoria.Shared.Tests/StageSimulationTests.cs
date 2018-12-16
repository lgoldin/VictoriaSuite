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
	public class StageSimulationTests
	{
        [TestMethod]
		public void GetVariablesRetrievesAListWithVariableT()
		{
            var simulation = new Mock<ISimulation>();
            simulation.Setup(x => x.GetDiagrams()).Returns(new List<Diagram>());
            simulation.Setup(x => x.GetVariables()).Returns(new List<Variable> { new Variable { ActualValue = 0, InitialValue = 0, Name = "T" } });

            var stageSimulation = new StageSimulation(simulation.Object);

            List<StageVariable> result = stageSimulation.GetVariables();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("T", result.First().Name);
		}

        [TestMethod]
        public void GetVariablesRetrievesAListWithTFStageVariableArrayAndT()
        {
            var variables = new List<Variable> 
            { 
                new Variable { ActualValue = 0, InitialValue = 0, Name = "T" },
                new Variable { ActualValue = 0, InitialValue = 50000, Name = "TF" },
                new VariableArray { Name = "NS", Variables = new List<Variable> { new Variable { Name = "NS", InitialValue = 0, ActualValue = 0 } } }
            };

            var simulation = new Mock<ISimulation>();
            simulation.Setup(x => x.GetDiagrams()).Returns(new List<Diagram>());
            simulation.Setup(x => x.GetVariables()).Returns(variables);

            var stageSimulation = new StageSimulation(simulation.Object);

            List<StageVariable> result = stageSimulation.GetVariables();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("T", result.ElementAt(0).Name);
            Assert.AreEqual("TF", result.ElementAt(1).Name);
            Assert.AreEqual("NS", result.ElementAt(2).Name);
        }

        [TestMethod]
        public void CanContinueReturnsTrue()
        {
            var variables = new List<Variable> 
            { 
                new Variable { ActualValue = 0, InitialValue = 0, Name = "T" },
                new Variable { ActualValue = 0, InitialValue = 50000, Name = "TF" },
                new VariableArray { Name = "NS", Variables = new List<Variable> { new Variable { Name = "NS", InitialValue = 0, ActualValue = 0 } } }
            };

            var simulation = new Mock<ISimulation>();
            simulation.Setup(x => x.GetDiagrams()).Returns(new List<Diagram>());
            simulation.Setup(x => x.GetVariables()).Returns(variables);
            simulation.Setup(x => x.CanContinue()).Returns(true);

            var stageSimulation = new StageSimulation(simulation.Object);

            Assert.IsTrue(stageSimulation.CanContinue());
        }

        [TestMethod]
        public void CanContinueReturnsFalseBecauseSimulationRetrievesFalse()
        {
            var variables = new List<Variable> 
            { 
                new Variable { ActualValue = 0, InitialValue = 0, Name = "T" },
                new Variable { ActualValue = 0, InitialValue = 50000, Name = "TF" },
                new VariableArray { Name = "NS", Variables = new List<Variable> { new Variable { Name = "NS", InitialValue = 0, ActualValue = 0 } } }
            };

            var simulation = new Mock<ISimulation>();
            simulation.Setup(x => x.GetDiagrams()).Returns(new List<Diagram>());
            simulation.Setup(x => x.GetVariables()).Returns(variables);
            simulation.Setup(x => x.CanContinue()).Returns(false);

            var stageSimulation = new StageSimulation(simulation.Object);

            Assert.IsFalse(stageSimulation.CanContinue());
        }

        [TestMethod]
        public void StopExecutionTrueCanContinueReturnsFalse()
        {
            var variables = new List<Variable> 
            { 
                new Variable { ActualValue = 0, InitialValue = 0, Name = "T" },
                new Variable { ActualValue = 0, InitialValue = 50000, Name = "TF" },
                new VariableArray { Name = "NS", Variables = new List<Variable> { new Variable { Name = "NS", InitialValue = 0, ActualValue = 0 } } }
            };

            var simulation = new Mock<ISimulation>();
            simulation.Setup(x => x.GetDiagrams()).Returns(new List<Diagram>());
            simulation.Setup(x => x.GetVariables()).Returns(variables);
            simulation.Setup(x => x.CanContinue()).Returns(false);

            var stageSimulation = new StageSimulation(simulation.Object);

            stageSimulation.StopExecution(true);

            Assert.IsFalse(stageSimulation.CanContinue());
        }

        [TestMethod]
        public void StopExecutionFalseCanContinueReturnsTrue()
        {
            var variables = new List<Variable> 
            { 
                new Variable { ActualValue = 0, InitialValue = 0, Name = "T" },
                new Variable { ActualValue = 0, InitialValue = 50000, Name = "TF" },
                new VariableArray { Name = "NS", Variables = new List<Variable> { new Variable { Name = "NS", InitialValue = 0, ActualValue = 0 } } }
            };

            var simulation = new Mock<ISimulation>();
            simulation.Setup(x => x.GetDiagrams()).Returns(new List<Diagram>());
            simulation.Setup(x => x.GetVariables()).Returns(variables);
            simulation.Setup(x => x.CanContinue()).Returns(true);

            var stageSimulation = new StageSimulation(simulation.Object);

            stageSimulation.StopExecution(false);

            Assert.IsTrue(stageSimulation.CanContinue());
        }

        [TestMethod]
        public void GetSimulationOk()
        {
            var variables = new List<Variable> 
            { 
                new Variable { ActualValue = 0, InitialValue = 0, Name = "T" },
                new Variable { ActualValue = 0, InitialValue = 50000, Name = "TF" },
                new VariableArray { Name = "NS", Variables = new List<Variable> { new Variable { Name = "NS", InitialValue = 0, ActualValue = 0 } } }
            };

            var simulation = new Mock<ISimulation>();
            simulation.Setup(x => x.GetDiagrams()).Returns(new List<Diagram>());
            simulation.Setup(x => x.GetVariables()).Returns(variables);
 
            var stageSimulation = new StageSimulation(simulation.Object);

            var result = stageSimulation.GetSimulation();

            Assert.AreEqual(simulation.Object, result);
        }

        [TestMethod]
        public void MustNotifyUITrueBecauseActualValueIs20()
        {
            var variables = new List<Variable> 
            { 
                new Variable { ActualValue = 20, InitialValue = 0, Name = "T" },
                new Variable { ActualValue = 0, InitialValue = 50000, Name = "TF" },
                new VariableArray { Name = "NS", Variables = new List<Variable> { new Variable { Name = "NS", InitialValue = 0, ActualValue = 0 } } }
            };

            var simulation = new Mock<ISimulation>();
            simulation.Setup(x => x.GetDiagrams()).Returns(new List<Diagram>());
            simulation.Setup(x => x.GetVariables()).Returns(variables);

            var stageSimulation = new StageSimulation(simulation.Object);

            Assert.IsTrue(stageSimulation.MustNotifyUI());
        }

        [TestMethod]
        public void MustNotifyUIFalseBecauseActualValueIs17()
        {
            var variables = new List<Variable> 
            { 
                new Variable { ActualValue = 17, InitialValue = 0, Name = "T" },
                new Variable { ActualValue = 0, InitialValue = 50000, Name = "TF" },
                new VariableArray { Name = "NS", Variables = new List<Variable> { new Variable { Name = "NS", InitialValue = 0, ActualValue = 0 } } }
            };

            var simulation = new Mock<ISimulation>();
            simulation.Setup(x => x.GetDiagrams()).Returns(new List<Diagram>());
            simulation.Setup(x => x.GetVariables()).Returns(variables);
            simulation.Setup(x => x.CanContinue()).Returns(true);

            var stageSimulation = new StageSimulation(simulation.Object);

            Assert.IsFalse(stageSimulation.MustNotifyUI());
        }

        [TestMethod]
        public void MustNotifyUITrueBecauseCanContinueIsFalse()
        {
            var variables = new List<Variable> 
            { 
                new Variable { ActualValue = 20, InitialValue = 0, Name = "T" },
                new Variable { ActualValue = 0, InitialValue = 50000, Name = "TF" },
                new VariableArray { Name = "NS", Variables = new List<Variable> { new Variable { Name = "NS", InitialValue = 0, ActualValue = 0 } } }
            };

            var simulation = new Mock<ISimulation>();
            simulation.Setup(x => x.GetDiagrams()).Returns(new List<Diagram>());
            simulation.Setup(x => x.GetVariables()).Returns(variables);

            var stageSimulation = new StageSimulation(simulation.Object);

            stageSimulation.StopExecution(true);

            Assert.IsTrue(stageSimulation.MustNotifyUI());
        }

        [TestMethod]
        public void GetExecutionStatusOk()
        {
            var variables = new List<Variable> 
            { 
                new Variable { ActualValue = 0, InitialValue = 0, Name = "T" },
                new Variable { ActualValue = 0, InitialValue = 50000, Name = "TF" },
                new VariableArray { Name = "NS", Variables = new List<Variable> { new Variable { Name = "NS", InitialValue = 0, ActualValue = 0 } } }
            };

            var simulation = new Mock<ISimulation>();
            simulation.Setup(x => x.GetDiagrams()).Returns(new List<Diagram>());
            simulation.Setup(x => x.GetVariables()).Returns(variables);

            var stageSimulation = new StageSimulation(simulation.Object);

            var result = stageSimulation.GetExecutionStatus();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void GetMainDiagramOk()
        {
            var variables = new List<Variable> 
            { 
                new Variable { ActualValue = 0, InitialValue = 0, Name = "T" },
                new Variable { ActualValue = 0, InitialValue = 50000, Name = "TF" },
                new VariableArray { Name = "NS", Variables = new List<Variable> { new Variable { Name = "NS", InitialValue = 0, ActualValue = 0 } } }
            };

            var diagrams = new List<Diagram> 
            {
                new Diagram { Name = "Principal", Nodes = new ObservableCollection<Node>() }
            };

            var simulation = new Mock<ISimulation>();
            simulation.Setup(x => x.GetDiagrams()).Returns(diagrams);
            simulation.Setup(x => x.GetVariables()).Returns(variables);

            var stageSimulation = new StageSimulation(simulation.Object);

            var result = stageSimulation.GetMainDiagram();

            Assert.AreEqual(diagrams.First(), result);
        }
	}
}

