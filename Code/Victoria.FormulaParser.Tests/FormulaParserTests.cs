using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Victoria.FormulaParser.Tests
{
    [TestClass]
    public class FormulaParserTests
    {
        [TestMethod]
        public void MultipleAndSum()
        {
            string formulaOriginal = "2 * 3 * 4 * 5 + 6";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToJavaScriptString();

            Assert.AreEqual("((((2*3)*4)*5)+6)", expression);
        }

        [TestMethod]
        public void Pow()
        {
            string formulaOriginal = "4 ^ 2";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToJavaScriptString();

            Assert.AreEqual("Math.pow(4, 2)", expression);
        }

        [TestMethod]
        public void SumAndPow()
        {
            string formulaOriginal = "2 + 4 ^ 2";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToJavaScriptString();

            Assert.AreEqual("(2+Math.pow(4, 2))", expression);
        }

        [TestMethod]
        public void PowAndSum()
        {
            string formulaOriginal = "4 ^ 2 + 2";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToJavaScriptString();

            Assert.AreEqual("(Math.pow(4, 2)+2)", expression);
        }

        [TestMethod]
        public void ComplexPowAndSum()
        {
            string formulaOriginal = "(5 * 4) ^ 2 + 2";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToJavaScriptString();

            Assert.AreEqual("(Math.pow((5*4), 2)+2)", expression);
        }

        [TestMethod]
        public void PowAndPow()
        {
            string formulaOriginal = "((5 * 4) ^ (1 / 2)) ^ (1 / 2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToJavaScriptString();

            Assert.AreEqual("Math.pow(Math.pow((5*4), (1/2)), (1/2))", expression);
        }

        [TestMethod]
        public void DivisionMasProducto()
        {
            string formulaOriginal = "1/10+2/10*5";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToJavaScriptString();

            Assert.AreEqual("((1/10)+((2/10)*5))", expression);
        }

        [TestMethod]
        public void SumaYProductoCombinadoConNegativo()
        {
            string formulaOriginal = "0 + (0 - 0) * (-1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToJavaScriptString();

            Assert.AreEqual("(0+((0-0)*(-1)))", expression);
        }
    }
}
