using CSharpInterpreter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace CSharpInterpreterTests
{
    [TestClass]
    public class EvaluatorUnitTests
    {

        [TestMethod]
        public void TestEvalIntegerExpression()
        {
            Dictionary<string, int> items = new Dictionary<string, int>(20)
                                    {
                                        {"5;", 5},
                                        {"10;", 10},
                                        {"-5;", -5},
                                        {"-10;", -10},
                                        {"5 + 5 + 5 + 5 - 10;", 10},
                                        {"2 * 2 * 2 * 2 * 2;", 32},
                                        {"-50 + 100 + -50;", 0},
                                        {"5 * 2 + 10;", 20},
                                        {"5 + 2 * 10;", 25},
                                        {"20 + 2 * -10;", 0},
                                        {"50 / 2 * 2 + 10;", 60},
                                        {"2 * (5 + 10);", 30},
                                        {"3 * 3 * 3 + 10;", 37},
                                        {"3 * (3 * 3) + 10;", 37},
                                        {"(5 + 10 * 2 + 15 / 3) * 2 + -10;", 50}

                                    };

            foreach (var item in items)
            {
                Lexer lexer = new Lexer(item.Key);

                Parser prog = new Parser(lexer);
                Ast.Program astProgram = prog.ParseProgram();

                CSharpInterpreter.Environment env = new CSharpInterpreter.Environment();

                Evaluator eval = new Evaluator();

                IObject evaluated = eval.Eval(astProgram, env);

                var result = evaluated.Inspect();
                Assert.AreEqual(item.Value.ToString(), result);

            }
        }

        [TestMethod]
        public void TestEvalBooleanExpression()
        {
            Dictionary<string, bool> items = new Dictionary<string, bool>(20)
                                    {
                                        {"true;", true},
                                        {"false;", false},
                                        {"1 < 2;", true},
                                        {"1 > 2;", false},
                                        {"1 < 1;", false},
                                        {"1 > 1;", false},
                                        {"1 == 1;", true},
                                        {"1 != 1;", false},
                                        {"1 == 2;", false},
                                        {"1 != 2;", true},
                                        {"true == true;", true},
                                        {"false == false;", true},
                                        {"true == false;", false},
                                        {"true != false;", true},
                                        {"false != true;", true},
                                        {"(1 < 2) == true;", true},
                                        {"(1 < 2) == false;", false},
                                        {"(1 > 2) == true;", false},
                                        {"(1 > 2) == false;", true}

                                    };

            foreach (var item in items)
            {
                Lexer lexer = new Lexer(item.Key);

                Parser prog = new Parser(lexer);
                Ast.Program astProgram = prog.ParseProgram();

                CSharpInterpreter.Environment env = new CSharpInterpreter.Environment();

                Evaluator eval = new Evaluator();

                IObject evaluated = eval.Eval(astProgram, env);

                var result = evaluated.Inspect();
                Assert.AreEqual(item.Value.ToString(), result);

            }
        }

        [TestMethod]
        public void TestBangOperator()
        {
            Dictionary<string, bool> items = new Dictionary<string, bool>(20)
                                    {
                                        {"!true;", false},
                                        {"!false;", true},
                                        {"!5;", false},
                                        {"!!true;", true},
                                        {"!!false;", false},
                                        {"!!5;", true},

                                    };

            foreach (var item in items)
            {
                Lexer lexer = new Lexer(item.Key);

                Parser prog = new Parser(lexer);
                Ast.Program astProgram = prog.ParseProgram();

                CSharpInterpreter.Environment env = new CSharpInterpreter.Environment();

                Evaluator eval = new Evaluator();

                IObject evaluated = eval.Eval(astProgram, env);

                var result = evaluated.Inspect();
                Assert.AreEqual(item.Value.ToString(), result);

            }
        }

        [TestMethod]
        public void TestIfElseExpressions()
        {
            Dictionary<string, int> items = new Dictionary<string, int>(20)
                                    {
                                        {"if (true) { 10 }", 10},
                                        {"if (false) { 10 }", 0},
                                        //{"if (1) { 10 }", 10},
                                        {"if (1 < 2) { 10 }", 10},
                                        {"if (1 > 2) { 10 }", 0},
                                        {"if (1 > 2) { 10 } else { 20 }", 20},
                                        {"if (1 < 2) { 10 } else { 20 }", 10},

                                    };

            foreach (var item in items)
            {
                Lexer lexer = new Lexer(item.Key);

                Parser prog = new Parser(lexer);
                Ast.Program astProgram = prog.ParseProgram();

                CSharpInterpreter.Environment env = new CSharpInterpreter.Environment();

                Evaluator eval = new Evaluator();

                IObject evaluated = eval.Eval(astProgram, env);

                var result = evaluated.Inspect();
                Assert.AreEqual(item.Value.ToString(), result);

            }
        }

        [TestMethod]
        public void TestReturnStatements()
        {

            Dictionary<string, int> items = new Dictionary<string, int>(20)
            {
                                { "return 10;", 10},
                                { "return 10; 9;", 10},
                                { "return 2 * 5; 9;", 10},
                                { "9; return 2 * 5; 9;", 10},
                                { "if (10 > 1) { return 10; }", 10},
                                { "if (10 > 1) { if (10 > 1) { return 10; } return 1; }", 10},
                                { "let f = fn(x) { return x; x + 10; }; f(10);", 10},
                                { "let f = fn(x) { let result = x + 10; return result; return 10; fn(10);}", 20},

            };

            foreach (var item in items)
            {
                Lexer lexer = new Lexer(item.Key);

                Parser prog = new Parser(lexer);
                Ast.Program astProgram = prog.ParseProgram();

                CSharpInterpreter.Environment env = new CSharpInterpreter.Environment();

                Evaluator eval = new Evaluator();

                IObject evaluated = eval.Eval(astProgram, env);

                var result = evaluated.Inspect();
                Assert.AreEqual(item.Value.ToString(), result);

            }

        }

        [TestMethod]
        public void TestErrorHandling()
        {
            Dictionary<string, string> items = new Dictionary<string, string>(20)
            {

                { "5 + true;", "type mismatch: INTEGER + BOOLEAN" },
                { "5 + true; 5;", "type mismatch: INTEGER + BOOLEAN" },
                { "-true", "unknown operator: -BOOLEAN" },
                { "true + false;", "unknown operator: BOOLEAN + BOOLEAN" },
                { "true + false + true + false;", "unknown operator: BOOLEAN + BOOLEAN" },
                { "5; true + false; 5", "unknown operator: BOOLEAN + BOOLEAN" },
                { "if (10 > 1) { true + false; }", "unknown operator: BOOLEAN + BOOLEAN" },
                { "if (10 > 1) {if (10 > 1) { return true + false; } return 1;}", "unknown operator: BOOLEAN + BOOLEAN" },
                { "foobar", "identifier not found: foobar" },

            };

            foreach (var item in items)
            {
                Lexer lexer = new Lexer(item.Key);

                Parser prog = new Parser(lexer);
                Ast.Program astProgram = prog.ParseProgram();

                CSharpInterpreter.Environment env = new CSharpInterpreter.Environment();

                Evaluator eval = new Evaluator();

                IObject evaluated = eval.Eval(astProgram, env);

                var result = evaluated.Inspect();
                Assert.AreEqual(item.Value.ToString(), result);

            }

        }


        [TestMethod]
        public void TestLetStatements()
        {
            Dictionary<string, int> items = new Dictionary<string, int>(10)
                                    {
                                        {"let a = 5; a;",     5 },
                                        {"let a = 5 * 5; a;",   25},
                                        {"let a = 5; let b = a; b;",   5},
                                        {"let a = 5; let b = a; let c = a + b + 5; c;",  15}

                                    };

            foreach (var item in items)
            {
                Lexer lexer = new Lexer(item.Key);

                Parser prog = new Parser(lexer);
                Ast.Program astProgram = prog.ParseProgram();

                CSharpInterpreter.Environment env = new CSharpInterpreter.Environment();

                Evaluator eval = new Evaluator();

                IObject evaluated = eval.Eval(astProgram, env);

                var result = evaluated.Inspect();
                Assert.AreEqual(item.Value.ToString(), result);

            }
        }


        [TestMethod]
        public void TestFunctionObject()
        {
            
            string input = "fn(x) { x + 2; };";

            Lexer lexer = new Lexer(input);

            Parser prog = new Parser(lexer);
            Ast.Program astProgram = prog.ParseProgram();

            CSharpInterpreter.Environment env = new CSharpInterpreter.Environment();

            Evaluator eval = new Evaluator();

            IObject evaluated = eval.Eval(astProgram, env);

            var result = evaluated.Inspect();

            Assert.AreEqual(input, result);

        }


        [TestMethod]
        public void TestFunctionApplication()
        {

            Dictionary<string, int> items = new Dictionary<string, int>(10)
                                    {
                                        { "let identity = fn(x) { x; }; identity(5);", 5},
                                        { "let identity = fn(x) { return x; }; identity(5);", 5},
                                        { "let double = fn(x) { x * 2; }; double(5);", 10},
                                        { "let add = fn(x, y) { x + y; }; add(5, 5);", 10},
                                        { "let add = fn(x, y) { x + y; }; add(5 + 5, add(5, 5));", 20},
                                        { "fn(x) { x; }(5)", 5},


                                    };

            foreach (var item in items)
            {
                Lexer lexer = new Lexer(item.Key);

                Parser prog = new Parser(lexer);
                Ast.Program astProgram = prog.ParseProgram();

                CSharpInterpreter.Environment env = new CSharpInterpreter.Environment();

                Evaluator eval = new Evaluator();

                IObject evaluated = eval.Eval(astProgram, env);

                var result = evaluated.Inspect();
                Assert.AreEqual(item.Value.ToString(), result);

            }

        }
           

        [TestMethod]
        public void TestEnclosingEnvironments()
        {
            var input = @"
                    let first = 10;
                    let second = 10;
                    let third = 10;

                    let ourFunction = fn(first) {
                    let second = 20;

                    first + second + third;
                };

                ourFunction(20) + first + second;";

            Lexer lexer = new Lexer(input);

            Parser prog = new Parser(lexer);
            Ast.Program astProgram = prog.ParseProgram();

            CSharpInterpreter.Environment env = new CSharpInterpreter.Environment();

            Evaluator eval = new Evaluator();

            IObject evaluated = eval.Eval(astProgram, env);

            var result = evaluated.Inspect();
            Assert.AreEqual(input, result);





        }

    }
}
