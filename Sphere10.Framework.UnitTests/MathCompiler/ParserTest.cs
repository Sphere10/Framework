//-----------------------------------------------------------------------
// <copyright file="ParserTest.cs" company="Sphere 10 Software">
//
// Copyright (c) Sphere 10 Software. All rights reserved. (http://www.sphere10.com)
//
// Distributed under the MIT software license, see the accompanying file
// LICENSE or visit http://www.opensource.org/licenses/mit-license.php.
//
// <author>Herman Schoenfeld</author>
// <date>2018</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using Sphere10.Framework.Maths.Compiler;

namespace Sphere10.Framework.UnitTests {

    [TestFixture]
    public class ParserTests {

        /// <summary>
        /// -x-y-z = gives tree:
        ///                     -
        ///                 -       z
        ///             -x      y
        /// </summary>
        [Test]
        public void TestTreeStructure() {
            string exp = "-x-y-z";
            Parser parser = new Parser(new Scanner(new StringReader(exp)));
            SyntaxTree tree = parser.ParseExpression();
            // (-x) - y - z
            Assert.IsInstanceOfType(typeof(BinaryOperatorTree), tree);
            Assert.AreEqual(Operator.Subtraction, ((BinaryOperatorTree)tree).Operator);
            Assert.IsInstanceOfType(typeof(BinaryOperatorTree),
                ((BinaryOperatorTree)tree).LeftHandSide);
            Assert.AreEqual(Operator.Subtraction,
                ((BinaryOperatorTree)((BinaryOperatorTree)tree).LeftHandSide).Operator);
            Assert.IsInstanceOfType(typeof(UnaryOperatorTree),
                ((BinaryOperatorTree)((BinaryOperatorTree)tree).LeftHandSide).LeftHandSide);
            Assert.IsInstanceOfType(typeof(FactorTree),
                ((BinaryOperatorTree)((BinaryOperatorTree)tree).LeftHandSide).RightHandSide);
            Assert.AreEqual(Operator.UnaryMinus,
                ((UnaryOperatorTree)((BinaryOperatorTree)((BinaryOperatorTree)tree).LeftHandSide).LeftHandSide).Operator); ;
            Assert.IsInstanceOfType(typeof(FactorTree),
                ((UnaryOperatorTree)((BinaryOperatorTree)((BinaryOperatorTree)tree).LeftHandSide).LeftHandSide).Operand); ;
            Assert.IsInstanceOfType(typeof(FactorTree),
                ((BinaryOperatorTree)tree).RightHandSide);
        }

        [Test]
        public void TestUnaryAndPower() {
            string exp = "-x^2.5E-1";
            Parser parser = new Parser(new Scanner(new StringReader(exp)));
            SyntaxTree tree = parser.ParseExpression();
            Assert.AreEqual("UnaryMinus(Power(Identifier(x),Scalar(2.5E-1)))", tree.ToString());
        }

        [Test]
        public void TestUnaryAndMultiplication() {
            string exp = "-x*-y";
            Parser parser = new Parser(new Scanner(new StringReader(exp)));
            SyntaxTree tree = parser.ParseExpression();
            Assert.AreEqual("Multiplication(UnaryMinus(Identifier(x)),UnaryMinus(Identifier(y)))", tree.ToString());
        }

        [Test]
        public void TestUnaryAndNegativePower() {
            string exp = "-x^-y";
            Parser parser = new Parser(new Scanner(new StringReader(exp)));
            SyntaxTree tree = parser.ParseExpression();
            Assert.AreEqual("UnaryMinus(Power(Identifier(x),UnaryMinus(Identifier(y))))", tree.ToString());
        }

        [Test]
        public void TestUnaryAndNegativePowerTower() {
            string exp = "-x^-y^-z";
            Parser parser = new Parser(new Scanner(new StringReader(exp)));
            SyntaxTree tree = parser.ParseExpression();
            Assert.AreEqual("UnaryMinus(Power(Identifier(x),UnaryMinus(Power(Identifier(y),UnaryMinus(Identifier(z))))))", tree.ToString());
        }
 
        [Test]
        public void TestMultiplicationDivision() {
            string exp = "x*y/z";
            Parser parser = new Parser(new Scanner(new StringReader(exp)));
            SyntaxTree tree = parser.ParseExpression();
            Assert.AreEqual("Multiplication(Identifier(x),Division(Identifier(y),Identifier(z)))", tree.ToString());
        }

        [Test]
        public void TestDivisionDivision() {
            string exp = "x/y/z";
            Parser parser = new Parser(new Scanner(new StringReader(exp)));
            SyntaxTree tree = parser.ParseExpression();
            Assert.AreEqual("Division(Division(Identifier(x),Identifier(y)),Identifier(z))", tree.ToString());
        }

        [Test]
        public void TestDivisionMultiplication() {
            string exp = "x/y*z";
            Parser parser = new Parser(new Scanner(new StringReader(exp)));
            SyntaxTree tree = parser.ParseExpression();
            Assert.AreEqual("Multiplication(Division(Identifier(x),Identifier(y)),Identifier(z))", tree.ToString());
        }

        [Test]
        public void TestAdditionSub() {
            string exp = "x+y-z";
            Parser parser = new Parser(new Scanner(new StringReader(exp)));
            SyntaxTree tree = parser.ParseExpression();
            Assert.AreEqual("Subtraction(Addition(Identifier(x),Identifier(y)),Identifier(z))", tree.ToString());
        }
    
    }

}
