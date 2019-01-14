﻿using System;
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
        public void Equal1()
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
    }
}
