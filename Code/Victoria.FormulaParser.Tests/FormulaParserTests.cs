using System;
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

            string expression = formulaParser.ToString();
            Assert.AreEqual("((((2*3)*4)*5)+6)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(126, valor);
        }

        [TestMethod]
        public void Pow()
        {
            string formulaOriginal = "4 ^ 2";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(4^2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(16, valor);
        }

        [TestMethod]
        public void SumAndPow()
        {
            string formulaOriginal = "2 + 4 ^ 2";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(2+(4^2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(18, valor);
        }

        [TestMethod]
        public void PowAndSum()
        {
            string formulaOriginal = "4 ^ 2 + 2";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((4^2)+2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(18, valor);
        }

        [TestMethod]
        public void ComplexPowAndSum()
        {
            string formulaOriginal = "(5 * 4) ^ 2 + 2";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((5*4)^2)+2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(402, valor);
        }

        [TestMethod]
        public void PowAndPow()
        {
            string formulaOriginal = "((5 * 4) ^ (1 / 2)) ^ (1 / 2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((5*4)^(1/2))^(1/2))", expression);

            double valor = formulaParser.GetValor();
            double valorDeReferencia = Math.Sqrt(Math.Sqrt(20));

            Assert.AreEqual(valorDeReferencia, valor);
        }

        [TestMethod]
        public void LaPruebaDeVilma()
        {
            string formulaOriginal = "2*3-4/2+20";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((2*3)-(4/2))+20)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(24, valor);
        }

        [TestMethod]
        public void PruebaDelExcell1()
        {
            string formulaOriginal = "2*3-4/2+20";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((2*3)-(4/2))+20)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(24, valor);
        }

        [TestMethod]
        public void PruebaDelExcell2()
        {
            string formulaOriginal = "2*3+4";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((2*3)+4)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(10, valor);
        }

        [TestMethod]
        public void PruebaDelExcell3()
        {
            string formulaOriginal = "(2+3)*4";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((2+3)*4)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(20, valor);
        }

        [TestMethod]
        public void PruebaDelExcell4()
        {
            string formulaOriginal = "(-(2+3)*4)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((-(2+3))*4)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-20, valor);
        }

        [TestMethod]
        public void PruebaDelExcell5()
        {
            string formulaOriginal = "2*3-4/2+20";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((2*3)-(4/2))+20)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(24, valor);
        }

        [TestMethod]
        public void PruebaDelExcell6()
        {
            string formulaOriginal = "2*3-4/2-20";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((2*3)-(4/2))-20)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-16, valor);
        }

        [TestMethod]
        public void PruebaDelExcell7()
        {
            string formulaOriginal = "2 *3+6/5+7/5";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((2*3)+(6/5))+(7/5))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(8.6, valor);
        }

        [TestMethod]
        public void PruebaDelExcell8()
        {
            string formulaOriginal = "(-2)*1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((-2)*1)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-2, valor);
        }

        [TestMethod]
        public void PruebaDelExcell9()
        {
            string formulaOriginal = "2*(-1)*4";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((2*(-1))*4)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-8, valor);
        }

        [TestMethod]
        public void PruebaDelExcell10()
        {
            string formulaOriginal = "2*(-1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(2*(-1))", expression);


            double valor = formulaParser.GetValor();
            Assert.AreEqual(-2, valor);
        }

        [TestMethod]
        public void LaUltimaPrueba()
        {
            string formulaOriginal = "1/10+2/10*5";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((1/10)+((2/10)*5))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1.1, valor);
        }

        [TestMethod]
        public void Int1()
        {
            string formulaOriginal = "int(1.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int(1.2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void Int2()
        {
            string formulaOriginal = "int(1.2)+3";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(1.2)+3)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(4, valor);
        }

        [TestMethod]
        public void Int3()
        {
            string formulaOriginal = "int(1.2+3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int((1.2+3))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(4, valor);
        }

        [TestMethod]
        public void Int4()
        {
            string formulaOriginal = "int(int(1.2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int(int(1.2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void Int5()
        {
            string formulaOriginal = "int(-int(1.2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int((-int(1.2)))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-1, valor);
        }

        [TestMethod]
        public void Int6()
        {
            string formulaOriginal = "3+int(1.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(3+int(1.2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(4, valor);
        }

        [TestMethod]
        public void Log1()
        {
            string formulaOriginal = "log(8, 2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log(8,2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(3, valor);
        }

        [TestMethod]
        public void Random1()
        {
            string formulaOriginal = "random()";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("random()", expression);

            double valor = formulaParser.GetValor();
            Assert.IsTrue(valor > 0);
            Assert.IsTrue(valor < 1);
        }

        [TestMethod]
        public void Sumatoria1()
        {
            string formulaOriginal = "sumatoria(1, 2, 3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("sumatoria(1,2,3)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(6, valor);
        }

        [TestMethod]
        public void Sumatoria2()
        {
            string formulaOriginal = "sumatoria(2, 3, -4, 5, 6, 7)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("sumatoria(2,3,(-4),5,6,7)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(19, valor);
        }
    }
}
