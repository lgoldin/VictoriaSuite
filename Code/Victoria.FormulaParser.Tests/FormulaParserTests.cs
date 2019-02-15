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
        public void ProductoRestaDivisionSuma()
        {
            string formulaOriginal = "2*3-4/2+20";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((2*3)-(4/2))+20)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(24, valor);
        }

        [TestMethod]
        public void ProductoSuma()
        {
            string formulaOriginal = "2*3+4";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((2*3)+4)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(10, valor);
        }

        [TestMethod]
        public void SumaParentesisProducto()
        {
            string formulaOriginal = "(2+3)*4";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((2+3)*4)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(20, valor);
        }

        [TestMethod]
        public void SumaNegativaParentesisProducto()
        {
            string formulaOriginal = "(-(2+3)*4)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((-(2+3))*4)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-20, valor);
        }

        
        [TestMethod]
        public void ProductoRestaDivisionResta()
        {
            string formulaOriginal = "2*3-4/2-20";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((2*3)-(4/2))-20)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-16, valor);
        }

        [TestMethod]
        public void ProductoSumaDivisionSumaDivision()
        {
            string formulaOriginal = "2 *3+6/5+7/5";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((2*3)+(6/5))+(7/5))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(8.6, valor);
        }

        [TestMethod]
        public void ProductoNegativo()
        {
            string formulaOriginal = "(-2)*1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((-2)*1)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-2, valor);
        }

        [TestMethod]
        public void MultipleProducto()
        {
            string formulaOriginal = "2*(-1)*4";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((2*(-1))*4)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-8, valor);
        }

        [TestMethod]
        public void ProductoNegativo1()
        {
            string formulaOriginal = "2*(-1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(2*(-1))", expression);


            double valor = formulaParser.GetValor();
            Assert.AreEqual(-2, valor);
        }

        [TestMethod]
        public void DivisionSumaProducto()
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
        public void IntResta()
        {
            string formulaOriginal = "int(4.2)-3";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(4.2)-3)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void IntProducto()
        {
            string formulaOriginal = "int(4.2)*3";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(4.2)*3)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(12, valor);
        }

        [TestMethod]
        public void IntDivision()
        {
            string formulaOriginal = "int(4.2)/2";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(4.2)/2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void IntPotencia()
        {
            string formulaOriginal = "2^int(4.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(2^int(4.2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(16, valor);
        }

        [TestMethod]
        public void SumaInt()
        {
            string formulaOriginal = "int(4.2)+int(5.3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(4.2)+int(5.3))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(9, valor);
        }

        [TestMethod]
        public void RestaInt()
        {
            string formulaOriginal = "int(5.3)-int(4.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(5.3)-int(4.2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void ProductoInt()
        {
            string formulaOriginal = "int(5.3)*int(4.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(5.3)*int(4.2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(20, valor);
        }

        [TestMethod]
        public void DivisionInt()
        {
            string formulaOriginal = "int(4.2)/int(2.1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(4.2)/int(2.1))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void SumaIntNegativos()
        {
            string formulaOriginal = "int(-4.2)+int(-5.3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int((-4.2))+int((-5.3)))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-9, valor);
        }

        [TestMethod]
        public void RestaIntNegativos()
        {
            string formulaOriginal = "int(-5.3)-int(-4.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int((-5.3))-int((-4.2)))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-1, valor);
        }

        [TestMethod]
        public void ProductoIntNegativos()
        {
            string formulaOriginal = "int(-5.3)*int(-4.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int((-5.3))*int((-4.2)))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(20, valor);
        }

        [TestMethod]
        public void DivisionIntNegativos()
        {
            string formulaOriginal = "int(-8.3)/int(-4.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int((-8.3))/int((-4.2)))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void PotenciaInt()
        {
            string formulaOriginal = "int(4.2)^int(2.1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(4.2)^int(2.1))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(16, valor);
        }

        [TestMethod]
        public void PotenciaIntBegativos()
        {
            string formulaOriginal = "int(-4.2)^int(-2.1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int((-4.2))^int((-2.1)))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(0.0625, valor);
        }

        [TestMethod]
        public void PotenciaIntFraccion()
        {
            string formulaOriginal = "int(4.2)^(1/2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(4.2)^(1/2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }


        [TestMethod]
        public void PotenciaIntFraccionNegativa()
        {
            string formulaOriginal = "int(4.2)^(-1/2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(4.2)^((-1)/2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(0.5, valor);
        }

        [TestMethod]
        public void IntSuma()
        {
            string formulaOriginal = "int(4.2 + 3.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int((4.2+3.2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(7, valor);
        }

        [TestMethod]
        public void IntRestaDecimal()
        {
            string formulaOriginal = "int(4.2-3.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int((4.2-3.2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void IntProductoDecimal()
        {
            string formulaOriginal = "int(4.2*3.3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int((4.2*3.3))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(13, valor);
        }

        [TestMethod]
        public void IntDivisionDecimal()
        {
            string formulaOriginal = "int(4.2/2.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int((4.2/2.2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void IntLog10()
        {
            string formulaOriginal = "int(log(10000))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int(log(10000))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(4, valor);
        }



        [TestMethod]
        public void IntLn()
        {
            string formulaOriginal = "int(ln(16))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int(ln(16))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void IntExponencial()
        {
            string formulaOriginal = "int(e(5)))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int(e(5))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(148, valor);
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
        public void SumaLog()
        {
            string formulaOriginal = "log(8, 2) + log(9, 3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(8,2)+log(9,3))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(5, valor);
        }

        [TestMethod]
        public void RestaLog()
        {
            string formulaOriginal = "log(8, 2) - log(9, 3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(8,2)-log(9,3))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void ProductoLog()
        {
            string formulaOriginal = "log(8, 2) * log(9, 3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(8,2)*log(9,3))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(6, valor);
        }

        [TestMethod]
        public void DivisionLog()
        {
            string formulaOriginal = "log(16, 2)/log(9, 3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(16,2)/log(9,3))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void LogSuma()
        {
            string formulaOriginal = "log(12+4, 2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log((12+4),2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(4, valor);
        }

        [TestMethod]
        public void LogResta()
        {
            string formulaOriginal = "log(12-4, 2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log((12-4),2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(3, valor);
        }

        [TestMethod]
        public void LogProducto()
        {
            string formulaOriginal = "log(8*2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log((8*2),2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(4, valor);
        }

        [TestMethod]
        public void LogDivision()
        {
            string formulaOriginal = "log(8/2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log((8/2),2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void LogPotencia()
        {
            string formulaOriginal = "log(16,2)^2";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(16,2)^2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(16, valor);
        }

        [TestMethod]
        public void LogPotenciaFraccion()
        {
            string formulaOriginal = "log(16, 2)^(1/2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(16,2)^(1/2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void LogPotenciaFraccionNegativa()
        {
            string formulaOriginal = "log(16, 2)^(-1/2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(16,2)^((-1)/2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(0.5, valor);
        }

        [TestMethod]
        public void LogInt()
        {
            string formulaOriginal = "log(int(16.2),2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log(int(16.2),2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(4, valor);
        }

        [TestMethod]
        public void Log10()
        {
            string formulaOriginal = "log(8)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log(8)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.Log(8, 10), valor);
        }

       
        [TestMethod]
        public void SumaLog10()
        {
            string formulaOriginal = "log(100) + log(1000)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(100)+log(1000))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(5, valor);
        }

        [TestMethod]
        public void RestaLog10()
        {
            string formulaOriginal = "log(100) - log(1000)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(100)-log(1000))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.Log(100,10) - Math.Log(1000, 10), valor);
        }

        [TestMethod]
        public void ProductoLog10()
        {
            string formulaOriginal = "log(100) * log(1000)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(100)*log(1000))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.Log(100, 10) * Math.Log(1000,10), valor);
        }

        [TestMethod]
        public void DivisionLog10()
        {
            string formulaOriginal = "log(1000) / log(100)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(1000)/log(100))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.Log(1000, 10)/Math.Log(100, 10), valor);
        }

        [TestMethod]
        public void Log10Suma()
        {
            string formulaOriginal = "log(900 + 100)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log((900+100))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.Log(900+100, 10), valor);
        }

        [TestMethod]
        public void Log10Resta()
        {
            string formulaOriginal = "log(1100 - 100)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log((1100-100))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.Log(1100 - 100, 10), valor);
        }

        [TestMethod]
        public void Log10Producto()
        {
            string formulaOriginal = "log(1000 * 100)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log((1000*100))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(5, valor);
        }


        [TestMethod]
        public void Log10Division()
        {
            string formulaOriginal = "log(10000/100)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log((10000/100))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }



        [TestMethod]
        public void Log10Potencia()
        {
            string formulaOriginal = "log(100)^2";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(100)^2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(4, valor);
        }

        [TestMethod]
        public void Log10PotenciaFraccion()
        {
            string formulaOriginal = "log(10000)^(1/2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(10000)^(1/2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void Log10PotenciaFraccionNegativa()
        {
            string formulaOriginal = "log(10000)^(-1/2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(10000)^((-1)/2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(0.5, valor);
        }

        [TestMethod]
        public void Log10Int()
        {
            string formulaOriginal = "log(int(100.2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log(int(100.2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
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
        public void NotRandom()
        {
            string formulaOriginal = "not(random())";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("not(random())", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void IntRandom()
        {
            string formulaOriginal = "int(random())";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int(random())", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void IntRandomProducto()
        {
            string formulaOriginal = "int(3*random())";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int((3*random()))", expression);

            double valor = formulaParser.GetValor();

            Assert.IsTrue(valor >= 0);
            Assert.IsTrue(valor < 3);
        }

        [TestMethod]
        public void RandomProducto()
        {
            string formulaOriginal = "3*random()";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(3*random())", expression);

            double valor = formulaParser.GetValor();
            Assert.IsTrue(valor > 0);
            Assert.IsTrue(valor < 3);
        }

        [TestMethod]
        public void RandomRango()
        {
            string formulaOriginal = "random(10, 20)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("random(10,20)", expression);

            double valor = formulaParser.GetValor();
            Assert.IsTrue(valor > 10);
            Assert.IsTrue(valor < 20);
        }

        [TestMethod]
        public void IntRandomRango()
        {
            string formulaOriginal = "int(random(10, 20))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int(random(10,20))", expression);

            double valor = formulaParser.GetValor();
            Assert.IsTrue(valor >= 10);
            Assert.IsTrue(valor < 20);

        }

        [TestMethod]
        public void IntRandomRangoProducto()
        {
            string formulaOriginal = "int(3*random(10, 20))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int((3*random(10,20)))", expression);

            double valor = formulaParser.GetValor();
            Assert.IsTrue(valor >= 30);
            Assert.IsTrue(valor < 60);
        }

        [TestMethod]
        public void RandomRangoProducto()
        {
            string formulaOriginal = "3*random(10, 20)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(3*random(10,20))", expression);

            double valor = formulaParser.GetValor();
            Assert.IsTrue(valor >= 30);
            Assert.IsTrue(valor < 60);
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

        [TestMethod]
        public void SumatoriaDecimales()
        {
            string formulaOriginal = "sumatoria(0.1, 2.3, 4.4, 6.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("sumatoria(0.1,2.3,4.4,6.2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(13, valor);
        }

        [TestMethod]
        public void SumatoriaDecimalesNegativos()
        {
            string formulaOriginal = "sumatoria(-0.1, -2.3, -4.4, -6.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("sumatoria((-0.1),(-2.3),(-4.4),(-6.2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-13, valor);
        }

        [TestMethod]
        public void SumatoriaInt()
        {
            string formulaOriginal = "sumatoria(int(0.1), int(2.3), int(4.3), int(6.2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("sumatoria(int(0.1),int(2.3),int(4.3),int(6.2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(12, valor);
        }

        [TestMethod]
        public void SumatoriaIntNegativos()
        {
            string formulaOriginal = "sumatoria(int(-0.1),int( -2.3),int(-4.4),int(-6.2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("sumatoria(int((-0.1)),int((-2.3)),int((-4.4)),int((-6.2)))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-12, valor);
        }

        [TestMethod]
        public void SumatoriaLog()
        {
            string formulaOriginal = "sumatoria(log(8,2), log(9,3), log(1,2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("sumatoria(log(8,2),log(9,3),log(1,2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(5, valor);
        }

        [TestMethod]
        public void SumatoriaLog10()
        {
            string formulaOriginal = "sumatoria(log(100), log(1000), log(10000))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("sumatoria(log(100),log(1000),log(10000))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(9, valor);
        }

        [TestMethod]
        public void SumatoriaFactorial()
        {
            string formulaOriginal = "sumatoria(factorial(5), factorial(4), factorial(3))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("sumatoria(factorial(5),factorial(4),factorial(3))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(150, valor);
        }

        [TestMethod]
        public void Or1()
        {
            string formulaOriginal = "0||0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0||0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void Or2()
        {
            string formulaOriginal = "1||0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1||0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void Or3()
        {
            string formulaOriginal = "0||1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0||1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void Or4()
        {
            string formulaOriginal = "1||1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1||1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void OrTrue()
        {
            string formulaOriginal = "(2<5)||0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((2<5)||0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void OrFalse()
        {
            string formulaOriginal = "(3<2)||0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3<2)||0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void OrTrues()
        {
            string formulaOriginal = "(3>0)||(2>=1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3>0)||(2>=1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void OrFalses()
        {
            string formulaOriginal = "(4<2)||(0>1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((4<2)||(0>1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void OrTrueNegative()
        {
            string formulaOriginal = "((-2)>(-5)||0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((-2)>(-5))||0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void OrFalseDecimal()
        {
            string formulaOriginal = "(3.1<2.1)||0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3.1<2.1)||0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void OrInt()
        {
            string formulaOriginal = "(int(4.2)>int(2.1))||(int(2.1)<int(4.4))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((int(4.2)>int(2.1))||(int(2.1)<int(4.4)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void OrIntNegative()
        {
            string formulaOriginal = "(int(-4.2)<int(-2.1))||(int(-2.1)<int(-4.4))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((int((-4.2))<int((-2.1)))||(int((-2.1))<int((-4.4))))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void OrLog()
        {
            string formulaOriginal = "(log(8,2)>4)||log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((log(8,2)>4)||log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void OrLogNot()
        {
            string formulaOriginal = "(log(8,2)>4)||not(log(1,2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((log(8,2)>4)||not(log(1,2)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void OrSumaLog()
        {
            string formulaOriginal = "((3+ int(2.3))>log(8,2))||log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+int(2.3))>log(8,2))||log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void OrRestaLog()
        {
            string formulaOriginal = "((3- int(2.3))>log(8,2))||log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-int(2.3))>log(8,2))||log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void OrProductoLog()
        {
            string formulaOriginal = "((3*int(2.3))>log(8,2))||log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*int(2.3))>log(8,2))||log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void OrDivisionLog()
        {
            string formulaOriginal = "((4/int(2.3))>log(8,2))||log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((4/int(2.3))>log(8,2))||log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void OrSumaLog10()
        {
            string formulaOriginal = "((3+ factorial(2))>log(1000))||log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+factorial(2))>log(1000))||log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void OrRestaLog10()
        {
            string formulaOriginal = "((3- factorial(2))>log(1000))||log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-factorial(2))>log(1000))||log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }


        [TestMethod]
        public void OrProductoLog10()
        {
            string formulaOriginal = "((3* factorial(2))>log(1000))||log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*factorial(2))>log(1000))||log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void OrDivisionLog10()
        {
            string formulaOriginal = "((4/factorial(2))>log(1000))||log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((4/factorial(2))>log(1000))||log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void And1()
        {
            string formulaOriginal = "0&&0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0&&0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void And2()
        {
            string formulaOriginal = "1&&0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1&&0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void And3()
        {
            string formulaOriginal = "0&&1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0&&1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void And4()
        {
            string formulaOriginal = "1&&1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1&&1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void AndTrue()
        {
            string formulaOriginal = "(2<5)&&1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((2<5)&&1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void AndFalse()
        {
            string formulaOriginal = "(3<2)&&1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3<2)&&1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void AndTrues()
        {
            string formulaOriginal = "(3>0)&&(2>=1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3>0)&&(2>=1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void AndFalses()
        {
            string formulaOriginal = "(4<2)&&(0>1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((4<2)&&(0>1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void AndTrueNegative()
        {
            string formulaOriginal = "((-2)>(-5))&&1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((-2)>(-5))&&1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void AndFalseDecimal()
        {
            string formulaOriginal = "(3.4<2.4)&&1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3.4<2.4)&&1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void AndInt()
        {
            string formulaOriginal = "(int(3.4)<int(2.4))&&int(1.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((int(3.4)<int(2.4))&&int(1.2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void AndIntNegative()
        {
            string formulaOriginal = "(int(-3.4)<int(-2.4))&&int(1.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((int((-3.4))<int((-2.4)))&&int(1.2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void AndLog()
        {
            string formulaOriginal = "(log(8,2)<4)&&log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((log(8,2)<4)&&log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void AndLogNot()
        {
            string formulaOriginal = "(log(8,2)<4)&&not(log(2,2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((log(8,2)<4)&&not(log(2,2)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void AndSumaLog()
        {
            string formulaOriginal = "((3+ int(2.3))>log(8,2))&&log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+int(2.3))>log(8,2))&&log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void AndRestaLog()
        {
            string formulaOriginal = "((3- int(2.3))>log(8,2))&&log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-int(2.3))>log(8,2))&&log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void AndPoductoLog()
        {
            string formulaOriginal = "((3*int(2.3))>log(8,2))&&log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*int(2.3))>log(8,2))&&log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void AndDivisionLog()
        {
            string formulaOriginal = "((4/int(2.3))>log(8,2))&&log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((4/int(2.3))>log(8,2))&&log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }


        [TestMethod]
        public void AndSumaLog10()
        {
            string formulaOriginal = "((3+ factorial(2))>log(1000))&&log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+factorial(2))>log(1000))&&log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void AndRestaLog10()
        {
            string formulaOriginal = "((3- factorial(2))>log(1000))&&log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-factorial(2))>log(1000))&&log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void AndProductoLog10()
        {
            string formulaOriginal = "((3*factorial(2))>log(1000))&&log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*factorial(2))>log(1000))&&log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void AndDivisionLog10()
        {
            string formulaOriginal = "((4/factorial(2))>log(10))&&log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((4/factorial(2))>log(10))&&log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }



        [TestMethod]
        public void Equal()
        {
            string formulaOriginal = "0==0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0==0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void Equal2()
        {
            string formulaOriginal = "1==0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1==0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void Equal3()
        {
            string formulaOriginal = "0==1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0==1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void Equal4()
        {
            string formulaOriginal = "1==1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1==1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void EqualTrue()
        {
            string formulaOriginal = "(2<5)==1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((2<5)==1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void EqualFalse()
        {
            string formulaOriginal = "(3<2)==1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3<2)==1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void EqualTrues()
        {
            string formulaOriginal = "(3>0)==(2>=1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3>0)==(2>=1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void EqualFalses()
        {
            string formulaOriginal = "(4<2)==(0>1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((4<2)==(0>1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void EqualTrueNegative()
        {
            string formulaOriginal = "((-2)>(-5))==1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((-2)>(-5))==1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void EqualFalseDecimal()
        {
            string formulaOriginal = "(3.2<2.1)==1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3.2<2.1)==1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void EqualInt()
        {
            string formulaOriginal = "int(4.2)==int(4.5)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(4.2)==int(4.5))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void EqualIntNegative()
        {
            string formulaOriginal = "int(-4.2)==int(-4.5)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int((-4.2))==int((-4.5)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void EqualLog()
        {
            string formulaOriginal = "log(16,2)==log(9,3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(16,2)==log(9,3))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void EqualLogNot()
        {
            string formulaOriginal = "log(16,2)== not(log(2,2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(16,2)==not(log(2,2)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void EqualSumaLog()
        {
            string formulaOriginal = "((3+ int(2.3))>log(8,2))==log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+int(2.3))>log(8,2))==log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void EqualRestaLog()
        {
            string formulaOriginal = "((3- int(2.3))>log(8,2))==log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-int(2.3))>log(8,2))==log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void EqualProductoLog()
        {
            string formulaOriginal = "((3*int(2.3))>log(8,2))==log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*int(2.3))>log(8,2))==log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void EqualDivisionLog()
        {
            string formulaOriginal = "((4/int(2.3))>log(8,2))==log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((4/int(2.3))>log(8,2))==log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void EqualSumaLog10()
        {
            string formulaOriginal = "((3+ factorial(2))>log(1000))==log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+factorial(2))>log(1000))==log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void EqualRestaLog10()
        {
            string formulaOriginal = "((3- factorial(2))>log(1000))==log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-factorial(2))>log(1000))==log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void EqualProductoLog10()
        {
            string formulaOriginal = "((3*factorial(2))>log(1000))==log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*factorial(2))>log(1000))==log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void EqualDivisionLog10()
        {
            string formulaOriginal = "((4/factorial(2))>log(1000))==log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((4/factorial(2))>log(1000))==log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void NotEqual1()
        {
            string formulaOriginal = "0!=0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0!=0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void NotEqual2()
        {
            string formulaOriginal = "1!=0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1!=0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void NotEqual3()
        {
            string formulaOriginal = "0!=1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0!=1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void NotEqual4()
        {
            string formulaOriginal = "1!=1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1!=1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void NotEqualTrue()
        {
            string formulaOriginal = "(2<5)!=0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((2<5)!=0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void NotEqualFalse()
        {
            string formulaOriginal = "(3<2)!=0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3<2)!=0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void NotEqualTrues()
        {
            string formulaOriginal = "(3>0)!=(2>=1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3>0)!=(2>=1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void NotEqualFalses()
        {
            string formulaOriginal = "(4<2)!=(0>1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((4<2)!=(0>1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void NotEqualTrueNegative()
        {
            string formulaOriginal = "((-2)>(-5))!=0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((-2)>(-5))!=0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void NotEqualFalseDecimal()
        {
            string formulaOriginal = "(3.4<2.3)!=0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3.4<2.3)!=0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }


        [TestMethod]
        public void NotEqualInt()
        {
            string formulaOriginal = "int(4.3)!= int(5.3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(4.3)!=int(5.3))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void NotEqualIntNegative()
        {
            string formulaOriginal = "int(-4.3)!= int(-5.3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int((-4.3))!=int((-5.3)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void NotEqualLog()
        {
            string formulaOriginal = "log(4,2)!=log(9,3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(4,2)!=log(9,3))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void NotEqualLogNot()
        {
            string formulaOriginal = "log(4,2)!= not(log(2,2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(4,2)!=not(log(2,2)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }


        [TestMethod]
        public void NotEqualSumaLog()
        {
            string formulaOriginal = "((3+ int(2.3))>log(8,2))!=log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+int(2.3))>log(8,2))!=log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void NotEqualRestaLog()
        {
            string formulaOriginal = "((3- int(2.3))>log(8,2))!=log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-int(2.3))>log(8,2))!=log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void NotEqualProductoLog()
        {
            string formulaOriginal = "((3*int(2.3))>log(8,2))!=log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*int(2.3))>log(8,2))!=log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void NotEqualDivisionLog()
        {
            string formulaOriginal = "((4/int(2.3))>log(8,2))!=log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((4/int(2.3))>log(8,2))!=log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void NotEqualSumaLog10()
        {
            string formulaOriginal = "((3+ int(pi()))>log(1000))!=log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+int(pi()))>log(1000))!=log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void NotEqualRestaLog10()
        {
            string formulaOriginal = "((3- int(pi()))>log(1000))!=log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-int(pi()))>log(1000))!=log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void NotEqualProductoLog10()
        {
            string formulaOriginal = "((3*int(pi()))>log(1000))!=log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*int(pi()))>log(1000))!=log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void NotEqualDivisionLog10()
        {
            string formulaOriginal = "((6/int(pi()))>log(1000))!=log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((6/int(pi()))>log(1000))!=log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThan1()
        {
            string formulaOriginal = "0<0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0<0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThan2()
        {
            string formulaOriginal = "1<0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1<0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThan3()
        {
            string formulaOriginal = "0<1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0<1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThan4()
        {
            string formulaOriginal = "1<1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1<1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanTrue()
        {
            string formulaOriginal = "0<(2<5)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0<(2<5))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanFalse()
        {
            string formulaOriginal = "0<(3<2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0<(3<2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanTrues()
        {
            string formulaOriginal = "(3>0)<(2>=1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3>0)<(2>=1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanFalses()
        {
            string formulaOriginal = "(4<2)<(0>1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((4<2)<(0>1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanTrueNegative()
        {
            string formulaOriginal = "0<((-2)>(-5))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0<((-2)>(-5)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanFalseDecimal()
        {
            string formulaOriginal = "0<(3.3<2.3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0<(3.3<2.3))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanInt()
        {
            string formulaOriginal = "int(4.2)<int(2.1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(4.2)<int(2.1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanIntNegative()
        {
            string formulaOriginal = "int(-4.2)<int(-2.1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int((-4.2))<int((-2.1)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanLog()
        {
            string formulaOriginal = "log(8,2)<log(9,3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(8,2)<log(9,3))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanLogNot()
        {
            string formulaOriginal = "not(log(2,2))<log(9,3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(not(log(2,2))<log(9,3))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }


        [TestMethod]
        public void LessThanSumaLog()
        {
            string formulaOriginal = "((3+ int(2.3))>log(8,2))<log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+int(2.3))>log(8,2))<log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanRestaLog()
        {
            string formulaOriginal = "((3- int(2.3))>log(8,2))<log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-int(2.3))>log(8,2))<log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanProductoLog()
        {
            string formulaOriginal = "((3*int(2.3))>log(8,2))<log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*int(2.3))>log(8,2))<log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanDivisionLog()
        {
            string formulaOriginal = "((4/int(2.3))>log(8,2))<log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((4/int(2.3))>log(8,2))<log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanSumaLog10()
        {
            string formulaOriginal = "((3+ factorial(2))>log(1000))<log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+factorial(2))>log(1000))<log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanRestaLog10()
        {
            string formulaOriginal = "((3- factorial(2))>log(1000))<log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-factorial(2))>log(1000))<log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanProductoLog10()
        {
            string formulaOriginal = "((3*factorial(2))>log(1000))<log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*factorial(2))>log(1000))<log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanDivisionLog10()
        {
            string formulaOriginal = "((4/factorial(2))>log(1000))<log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((4/factorial(2))>log(1000))<log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }


        [TestMethod]
        public void GreaterThan1()
        {
            string formulaOriginal = "0>0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0>0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThan2()
        {
            string formulaOriginal = "1>0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1>0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThan3()
        {
            string formulaOriginal = "0>1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0>1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThan4()
        {
            string formulaOriginal = "1>1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1>1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanTrue()
        {
            string formulaOriginal = "(2<5)>0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((2<5)>0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanFalse()
        {
            string formulaOriginal = "0>(3<2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0>(3<2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanTrues()
        {
            string formulaOriginal = "(3>0)>(2>=1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3>0)>(2>=1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanFalses()
        {
            string formulaOriginal = "(4<2)>(0>1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((4<2)>(0>1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanTrueNegative()
        {
            string formulaOriginal = "((-2)>(-5))>0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((-2)>(-5))>0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanFalseDecimal()
        {
            string formulaOriginal = "0>(3.5<2.4)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0>(3.5<2.4))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanInt()
        {
            string formulaOriginal = "int(4.2)>int(2.1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(4.2)>int(2.1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanIntNegative()
        {
            string formulaOriginal = "int(-4.2)>int(-2.1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int((-4.2))>int((-2.1)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanLogNot()
        {
            string formulaOriginal = "log(8,2)> not(log(2,2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(8,2)>not(log(2,2)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanSumaLog()
        {
            string formulaOriginal = "((3+ int(2.3))>log(8,2))>log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+int(2.3))>log(8,2))>log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanRestaLog()
        {
            string formulaOriginal = "((3- int(2.3))>log(8,2))<log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-int(2.3))>log(8,2))<log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanProductoLog()
        {
            string formulaOriginal = "((3*int(2.3))>log(8,2))>log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*int(2.3))>log(8,2))>log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanDivisionLog()
        {
            string formulaOriginal = "((4/int(2.3))>log(8,2))>log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((4/int(2.3))>log(8,2))>log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanSumaLog10()
        {
            string formulaOriginal = "((3+ int(pi()))>log(1000))>log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+int(pi()))>log(1000))>log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanRestaLog10()
        {
            string formulaOriginal = "((3- int(pi()))>log(1000))>log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-int(pi()))>log(1000))>log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanProductoLog10()
        {
            string formulaOriginal = "((3*int(pi()))>log(1000))>log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*int(pi()))>log(1000))>log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanDivisionLog10()
        {
            string formulaOriginal = "((6/int(pi()))>log(1000))>log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((6/int(pi()))>log(1000))>log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }



        [TestMethod]
        public void LessThanOrEqualsTo1()
        {
            string formulaOriginal = "0<=0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0<=0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsTo2()
        {
            string formulaOriginal = "1<=0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1<=0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsTo3()
        {
            string formulaOriginal = "0<=1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0<=1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsTo4()
        {
            string formulaOriginal = "1<=1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1<=1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToTrue()
        {
            string formulaOriginal = "0<=(2<5)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0<=(2<5))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToFalse()
        {
            string formulaOriginal = "1<=(3<2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1<=(3<2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToTrues()
        {
            string formulaOriginal = "(3>0)<=(2>=1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3>0)<=(2>=1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }


        [TestMethod]
        public void LessThanOrEqualsToFalses()
        {
            string formulaOriginal = "(4<2)<=(0>1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((4<2)<=(0>1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToTrueNegative()
        {
            string formulaOriginal = "0<=((-2)<(-5))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0<=((-2)<(-5)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToFalseDecimal()
        {
            string formulaOriginal = "1<=(3.2<2.1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1<=(3.2<2.1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToInt()
        {
            string formulaOriginal = "int(3.2)<=int(4.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(3.2)<=int(4.2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToIntNegative()
        {
            string formulaOriginal = "int(-3.2)<=int(-4.2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int((-3.2))<=int((-4.2)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToLog()
        {
            string formulaOriginal = "log(8,2)<=log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(8,2)<=log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToLogNot()
        {
            string formulaOriginal = "log(8,2)<= not(log(2,2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(8,2)<=not(log(2,2)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToSumaLog()
        {
            string formulaOriginal = "((3+ int(2.3))>log(8,2))<=log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+int(2.3))>log(8,2))<=log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToRestaLog()
        {
            string formulaOriginal = "((3- int(2.3))>log(8,2))<=log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-int(2.3))>log(8,2))<=log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToProductoLog()
        {
            string formulaOriginal = "((3*int(2.3))>log(8,2))<=log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*int(2.3))>log(8,2))<=log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToDivisionLog()
        {
            string formulaOriginal = "((4/int(2.3))>log(8,2))<=log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((4/int(2.3))>log(8,2))<=log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToSumaLog10()
        {
            string formulaOriginal = "((3+ factorial(3))>log(1000))<=log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+factorial(3))>log(1000))<=log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToRestaLog10()
        {
            string formulaOriginal = "((3- factorial(3))>log(1000))<=log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-factorial(3))>log(1000))<=log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void LessThanOrEqualsToProductoLog10()
        {
            string formulaOriginal = "((3* factorial(3))>log(1000))<=log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*factorial(3))>log(1000))<=log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }


        [TestMethod]
        public void LessThanOrEqualsToDivisionLog10()
        {
            string formulaOriginal = "((factorial(3)/3)>log(1000))<=log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((factorial(3)/3)>log(1000))<=log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

       

        [TestMethod]
        public void GreaterThanOrEqualsTo1()
        {
            string formulaOriginal = "0>=0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0>=0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }
        
        [TestMethod]
        public void GreaterThanOrEqualsTo2()
        {
            string formulaOriginal = "1>=0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1>=0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsTo3()
        {
            string formulaOriginal = "0>=1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(0>=1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsTo4()
        {
            string formulaOriginal = "1>=1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1>=1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToTrue()
        {
            string formulaOriginal = "(2<5)>=0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((2<5)>=0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToFalse()
        {
            string formulaOriginal = "(3<2)>=1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3<2)>=1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToTrues()
        {
            string formulaOriginal = "(3>0)>=(2>=1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3>0)>=(2>=1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToFalses()
        {
            string formulaOriginal = "(4<2)>=(0>1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((4<2)>=(0>1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToTrueNegative()
        {
            string formulaOriginal = "((-2)>(-5))>=0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((-2)>(-5))>=0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToFalseDecimal()
        {
            string formulaOriginal = "(3.4<2.2)>=1";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3.4<2.2)>=1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }


        [TestMethod]
        public void GreaterThanOrEqualsToInt()
        {
            string formulaOriginal = "int(5.2)>=int(3.5)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(5.2)>=int(3.5))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToIntNegative()
        {
            string formulaOriginal = "int(-5.2)>=int(-3.5)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int((-5.2))>=int((-3.5)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToLog()
        {
            string formulaOriginal = "log(8,2)>=log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(8,2)>=log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToLogNot()
        {
            string formulaOriginal = "log(8,2)>= not(log(2,2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(8,2)>=not(log(2,2)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToSumaLog()
        {
            string formulaOriginal = "((3+ int(2.3))>log(8,2))>=log(1,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+int(2.3))>log(8,2))>=log(1,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToRestaLog()
        {
            string formulaOriginal = "((3- int(2.3))>log(8,2))>=log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-int(2.3))>log(8,2))>=log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToProductoLog()
        {
            string formulaOriginal = "((3*int(2.3))>log(8,2))>=log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*int(2.3))>log(8,2))>=log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToDivisionLog()
        {
            string formulaOriginal = "((4/int(2.3))>log(8,2))>=log(2,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((4/int(2.3))>log(8,2))>=log(2,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToSumaLog10()
        {
            string formulaOriginal = "((3+ factorial(3))>log(1000))>=log(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3+factorial(3))>log(1000))>=log(1))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToRestaLog10()
        {
            string formulaOriginal = "((3- factorial(3))>log(1000))>=log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3-factorial(3))>log(1000))>=log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToProductoLog10()
        {
            string formulaOriginal = "((3* factorial(3))>log(1000))>=log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((3*factorial(3))>log(1000))>=log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void GreaterThanOrEqualsToDivisionLog10()
        {
            string formulaOriginal = "((factorial(3)/3)>log(1000))>=log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((factorial(3)/3)>log(1000))>=log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }



        [TestMethod]
        public void Modulo()
        {
            string formulaOriginal = "4%2";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(4%2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(0, valor); 
        }

        [TestMethod]
        public void ModuloNegativos()
        {
            string formulaOriginal = "(-4)%(-2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((-4)%(-2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(0, valor);
        }

        [TestMethod]
        public void ModuloPositivoNegativo()
        {
            string formulaOriginal = "5%(-2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(5%(-2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void ModuloNegativoPositivo()
        {
            string formulaOriginal = "(-5)%2";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((-5)%2)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-1, valor);
        }

        [TestMethod]
        public void ModuloInt()
        {
            string formulaOriginal = "int(5.3)%int(2.1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(5.3)%int(2.1))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void ModuloIntNegativos()
        {
            string formulaOriginal = "int(-5.3)%int(-2.1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int((-5.3))%int((-2.1)))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-1, valor);
        }

        [TestMethod]
        public void ModuloIntPositivoNegativo()
        {
            string formulaOriginal = "int(5.3)%int(-2.1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(5.3)%int((-2.1)))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void ModuloIntNegativoPositivo()
        {
            string formulaOriginal = "int(-5.3)%int(2.1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int((-5.3))%int(2.1))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-1, valor);
        }

        [TestMethod]
        public void ModuloLog()
        {
            string formulaOriginal = "log(8,2)%log(4,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(log(8,2)%log(4,2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void ModuloSumaLog()
        {
            string formulaOriginal = "(3+ int(2.3))%log(8,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3+int(2.3))%log(8,2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void ModuloRestaLog()
        {
            string formulaOriginal = "(3- int(2.3))%log(8,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3-int(2.3))%log(8,2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void ModuloProductoLog()
        {
            string formulaOriginal = "(3*int(2.3))%log(8,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3*int(2.3))%log(8,2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(0, valor);
        }

        [TestMethod]
        public void ModuloDivisionLog()
        {
            string formulaOriginal = "(12/int(2.3))%log(8,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((12/int(2.3))%log(8,2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(0, valor);
        }

        [TestMethod]
        public void ModuloSumaLog10()
        {
            string formulaOriginal = "(3+ int(2.3))%log(100)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3+int(2.3))%log(100))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }


        [TestMethod]
        public void ModuloRestaLog10()
        {
            string formulaOriginal = "(3- int(2.3))%log(1000)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3-int(2.3))%log(1000))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void ModuloProductoLog10()
        {
            string formulaOriginal = "(3*int(2.3))%log(10000)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((3*int(2.3))%log(10000))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void ModuloDivisionLog10()
        {
            string formulaOriginal = "(12/int(2.3))%log(10000)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((12/int(2.3))%log(10000))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }


        [TestMethod]
        public void Precedencia1()
        {
            string formulaOriginal = "1*2/3%4+5-6<7<=8>9>=10==11!=12&&13||14";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((((((((((((1*2)/3)%4)+5)-6)<7)<=8)>9)>=10)==11)!=12)&&13)||14)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void Precedencia2()
        {
            string formulaOriginal = "1||2&&3!=4==5>=6>7<=8<9-10+11%12/13*14";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(1||(2&&((3!=4)==((((5>=6)>7)<=8)<((9-10)+(((11%12)/13)*14))))))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void Precedencia3()
        {
            string formulaOriginal = "int(2.3)*log(8,2)/log(100)%int(2.5)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((int(2.3)*log(8,2))/log(100))%int(2.5))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void Precedencia4()
        {
            string formulaOriginal = "int(5.2)-log(8,2)<log(100)<=int(2.5)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(((int(5.2)-log(8,2))<log(100))<=int(2.5))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void Precedencia5()
        {
            string formulaOriginal = "int(10.8)>log(9,3)>=log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((int(10.8)>log(9,3))>=log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void Precedencia6()
        {
            string formulaOriginal = "e(2) > e() >= ln(2.718281828459045235360)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((e(2)>e())>=ln(2.718281828459045235360))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void Precedencia7()
        {
            string formulaOriginal = "factorial(5) > factorial(6)>=log(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((factorial(5)>factorial(6))>=log(10))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void Precedencia8()
        {
            string formulaOriginal = "int(pi())*sumatoria(log(8,2),log(100),log(10000))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(int(pi())*sumatoria(log(8,2),log(100),log(10000)))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(27, valor);
         }

        [TestMethod]
        public void Precedencia9()
        {
            string formulaOriginal = "e(-2)>e(-1)>=log(16,2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((e((-2))>e((-1)))>=log(16,2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void Precedencia10()
        {
            string formulaOriginal = "sumatoria(log(8,2),log(100),log(10000))/int(pi())";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(sumatoria(log(8,2),log(100),log(10000))/int(pi()))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(3, valor);
            
        }

        [TestMethod]
        public void Precedencia11()
        {
            string formulaOriginal = "int(-3.2)*int(pi())/sumatoria(log(10), ln(2.718281828459045235360), log(2,2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("((int((-3.2))*int(pi()))/sumatoria(log(10),ln(2.718281828459045235360),log(2,2)))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(-3, valor);

        }



        [TestMethod]
        public void Not1()
        {
            string formulaOriginal = "not(1)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("not(1)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void Not2()
        {
            string formulaOriginal = "not(0)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("not(0)", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void NotTrue()
        {
            string formulaOriginal = "not(5>2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("not((5>2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void NotFalse()
        {
            string formulaOriginal = "not(5<=2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("not((5<=2))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void NotInt()
        {
            string formulaOriginal = "not(int(4.2) > int(3.3))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("not((int(4.2)>int(3.3)))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(0, valor);
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void NotIntBegative()
        {
            string formulaOriginal = "not(int(-4.2)>int(-3.3))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("not((int((-4.2))>int((-3.3))))", expression);

            double valor = formulaParser.GetValor();
            bool boolean = formulaParser.GetValorAsBool();
            Assert.AreEqual(1, valor);
            Assert.IsTrue(boolean);
        }



        [TestMethod]
        public void IndefinicionNSobreCero()
        {
            string formulaOriginal = "4/0";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(4/0)", expression);

            try
            {
                double valor = formulaParser.GetValor();
            }
            catch (IndefinicionMatematicaException ime)
            {
                Assert.AreEqual("El resultado es indefinido.", ime.Message);
                Assert.AreEqual("/", ime.Operador);
                Assert.AreEqual(double.PositiveInfinity, ime.ValorNoNumerico);
            }
        }

        [TestMethod]
        public void IndefinicionLogDeNegativo()
        {
            string formulaOriginal = "log(-8,10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log((-8),10)", expression);

            try
            {
                double valor = formulaParser.GetValor();
            }
            catch (IndefinicionMatematicaException ime)
            {
                Assert.AreEqual("El resultado es indefinido.", ime.Message);
                Assert.AreEqual("log", ime.Operador);
                Assert.AreEqual(double.NaN, ime.ValorNoNumerico);
            }
        }

        [TestMethod]
        public void IndefinicionLogDeCero()
        {
            string formulaOriginal = "log(0,10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("log(0,10)", expression);

            try
            {
                double valor = formulaParser.GetValor();
            }
            catch (IndefinicionMatematicaException ime)
            {
                Assert.AreEqual("El resultado es indefinido.", ime.Message);
                Assert.AreEqual("log", ime.Operador);
                Assert.AreEqual(double.NegativeInfinity, ime.ValorNoNumerico);
            }
        }

        [TestMethod]
        public void Ln1()
        {
            string formulaOriginal = "ln(10)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("ln(10)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.Log(10,Math.E), valor);
        }

        [TestMethod]
        public void Ln()
        {
            string formulaOriginal = "ln( 2.718281828459045235360)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("ln(2.718281828459045235360)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void LnExp()
        {
            string formulaOriginal = "ln(e())";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("ln(e())", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void LnExp1()
        {
            string formulaOriginal = "ln(e(2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("ln(e(2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void LnExpPotencia()
        {
            string formulaOriginal = "ln(e(1/2)^2)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("ln((e((1/2))^2))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }


        [TestMethod]
        public void LnExpPotenciaNegativo()
        {
            string formulaOriginal = "ln(e(-1/2)^(-2))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("ln((e(((-1)/2))^(-2)))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void LnSumatoria()
        {
            string formulaOriginal = "ln(sumatoria(int(4.3),int(2.3),int(1.5))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("ln(sumatoria(int(4.3),int(2.3),int(1.5)))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.Log(7, Math.E), valor);
        }

        [TestMethod]
        public void LnFactorial()
        {
            string formulaOriginal = "ln(factorial(5))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("ln(factorial(5))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.Log(120, Math.E), valor);
        }

        [TestMethod]
        public void LnPi()
        {
            string formulaOriginal = "ln(pi())";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("ln(pi())", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.Log(Math.PI, Math.E), valor);
        }

        [TestMethod]
        public void LnLog()
        {
            string formulaOriginal = "ln(log(16,4))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("ln(log(16,4))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.Log(Math.Log(16,4), Math.E), valor);
        }


        [TestMethod]
        public void E1()
        {
            string formulaOriginal = "e()";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("e()", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.E, valor);
        }

        [TestMethod]
        public void E2()
        {
            string formulaOriginal = "e(3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("e(3)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.Pow(Math.E, 3), valor);
        }

        [TestMethod]
        public void ExponencialLn()
        {
            string formulaOriginal = "e(ln( 2.718281828459045235360))";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("e(ln(2.718281828459045235360))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.E, valor);
        }

        [TestMethod]
        public void Factorial()
        {
            string formulaOriginal = "factorial(5)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("factorial(5)", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(120, valor);
        }

        [TestMethod]
        public void FactorialSuma()
        {
            string formulaOriginal = "factorial(5) + factorial(3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(factorial(5)+factorial(3))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(126, valor);
        }

        [TestMethod]
        public void FactorialResta()
        {
            string formulaOriginal = "factorial(5) - factorial(3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(factorial(5)-factorial(3))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(114, valor);
        }

        [TestMethod]
        public void FactorialProducto()
        {
            string formulaOriginal = "factorial(5) * factorial(3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(factorial(5)*factorial(3))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(720, valor);
        }

        [TestMethod]
        public void FactorialDivision()
        {
            string formulaOriginal = "factorial(5) / factorial(3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("(factorial(5)/factorial(3))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(20, valor);
        }

        [TestMethod]
        public void Pi()
        {
            string formulaOriginal = "pi()";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("pi()", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(Math.PI, valor);
        }

        [TestMethod]
        public void IntPi()
        {
            string formulaOriginal = "int(pi())";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int(pi())", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(3, valor);
        }

        [TestMethod]
        public void IntSumaPi()
        {
            string formulaOriginal = "int(3 + pi())";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int((3+pi()))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(6, valor);
        }

        [TestMethod]
        public void IntRestaPi()
        {
            string formulaOriginal = "int(6 - pi())";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int((6-pi()))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void IntProductoPi()
        {
            string formulaOriginal = "int(3 * pi())";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int((3*pi()))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(9, valor);
        }

        [TestMethod]
        public void IntDivisionPi()
        {
            string formulaOriginal = "int(pi()/3)";

            var formulaParser = new FormulaParser(formulaOriginal);

            string expression = formulaParser.ToString();
            Assert.AreEqual("int((pi()/3))", expression);

            double valor = formulaParser.GetValor();
            Assert.AreEqual(1, valor);
        }


    }
}
