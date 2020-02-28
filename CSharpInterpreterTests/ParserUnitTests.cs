using CSharpInterpreter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace CSharpInterpreterTests
{
    [TestClass]
    public class ParserUnitTests
    {
        
        /*
        [TestMethod]
        public void TestMethod1()
        {
            string Expected = "5";

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                string[] args = null;
                CSharpInterpreter.MainProgram.Main(args);

                var result = sw.ToString().Trim();
                Assert.AreEqual(Expected, result);

            }
        }
        */
        /*
        [TestMethod]
        public void TestParser()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                string line = "";
                //line = "1 + 4;";
                //line = "return 5;";

                //line = "5;";
                //line = "true;";
                line = "if (x < y) { x } else { y }";
                Console.WriteLine(line);
                Lexer lexer = new Lexer(line);

                Parser prog = new Parser(lexer);
                Ast.Program astProgram = prog.ParseProgram();
                Console.WriteLine(astProgram.String());
                //Dump(astProgram);
                //DumpAsYaml(astProgram);
                astProgram.Dump();
                //WriteTokens(lexer);

                //string[] args = null;
                //CSharpInterpreter.MainProgram.Main(args);

                //var result = sw.ToString().Trim();
                //Assert.AreEqual(Expected, result);

            }
        }

        public static Dictionary<string, int> letstmts = new Dictionary<string, int>(10)
                                    {
                                        {"let a = 5; a;",     5 },
                                        {"let a = 5 * 5; a;",   25},
                                        {"let a = 5; let b = a; b;",   5},
                                        {"let a = 5; let b = a; let c = a + b + 5; c;",  15}
                                        
                                    };

        */
        //[TestMethod]
        //public void TestLetStatement()
        //{
        //    TestInt(letstmts);
        //}
        /*
        public static Dictionary<string, int> evalIntExps = new Dictionary<string, int>(20)
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
        */
        /*
        [TestMethod]
        public void TestEvalIntegerExpression()
        {
            foreach (var item in evalIntExps)
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

        public static Dictionary<string, bool> evalBoolExps = new Dictionary<string, bool>(20)
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
        */
        //[TestMethod]
        //public void TestEvalBooleanExpression()
        //{
        //    TestBool(evalBoolExps);
        //}
        /*
        public static Dictionary<string, bool> evalBangOper = new Dictionary<string, bool>(20)
                                    {
                                        {"!true;", false},
                                        {"!false;", true},
                                        {"!5;", false},
                                        {"!!true;", true},
                                        {"!!false;", false},
                                        {"!!5;", true},
                                        
                                    };
        */
        //[TestMethod]
        //public void TestBangOperator()
        //{
            //TestBool(evalBangOper);
        //}
        /*
        public void TestBool(Dictionary<string, bool> items)
        {
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


        public void TestInt(Dictionary<string, int> items)
        {
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


        public static Dictionary<string, int> evalIfElseExp = new Dictionary<string, int>(20)
                                    {
                                        {"if (true) { 10 }", 10},
                                        {"if (false) { 10 }", 0},
                                        //{"if (1) { 10 }", 10},
                                        {"if (1 < 2) { 10 }", 10},
                                        {"if (1 > 2) { 10 }", 0},
                                        {"if (1 > 2) { 10 } else { 20 }", 20},
                                        {"if (1 < 2) { 10 } else { 20 }", 10},


                                    };



        [TestMethod]
        public void TestIfElseExpressions()
        {
            TestInt(evalIfElseExp);
        }
        */
        /*
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
        */
    }
}
