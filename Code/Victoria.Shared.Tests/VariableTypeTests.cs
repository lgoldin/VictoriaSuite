using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Victoria.Shared;
using System.Linq;
using System.Collections.ObjectModel;
using Victoria.Shared.EventArgs;
using Moq;
using Victoria.Shared.Interfaces;
using Victoria.Shared.AnalisisPrevio;

namespace Victoria.Shared.Tests
{
    [TestClass]
	public class VariableTypeTests
	{
        [TestMethod]
		public void GetDataVariableType()
		{
            Assert.AreEqual(1, (int)VariableType.Data);
		}

        [TestMethod]
        public void GetControlVariableType()
        {
            Assert.AreEqual(2, (int)VariableType.Control);
        }

        [TestMethod]
        public void GetStateVariableType()
        {
            Assert.AreEqual(3, (int)VariableType.State);
        }

        [TestMethod]
        public void GetResultVariableType()
        {
            Assert.AreEqual(4, (int)VariableType.Result);
        }

        [TestMethod]
        public void GetOtherVariableType()
        {
            Assert.AreEqual(5, (int)VariableType.Other);
        }

        [TestMethod]
        public void GetVariableTypeFromValue()
        {
            Assert.AreEqual(VariableType.Result, (VariableType)4);
        }
	}
}

