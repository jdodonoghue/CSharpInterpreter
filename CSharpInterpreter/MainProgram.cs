using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
//using System.IdentityModel.Tokens.Jwt;
using System.Text;
using YamlDotNet.Serialization;

namespace CSharpInterpreter
{
    public class MainProgram
    {
        /*
        >> let x = 5;
        {Type:LET Literal:let}
        {Type:IDENT Literal:x}
        {Type:ASSIGN Literal:=}
        {Type:INT Literal:5}
        {Type:SEMICOLON Literal:;}

        >> let x = 3 + 2;
        {Type:LET Literal:let}
        {Type:IDENT Literal:x}
        {Type:ASSIGN Literal:=}
        {Type:INT Literal:3}
        {Type:PLUS Literal:+}
        {Type:INT Literal:2}
        {Type:SEMICOLON Literal:;}
        */

        public static void Main(string[] args)
        {

            //RunLexer();
            //RunParser();
            RunEvaluator();

            //TestParser();

            //TestIntegerArithmetic();

            //TestEvalIntegerExpression();

            Console.ReadLine();

        }

        public static void TestIntegerArithmetic()
        {
            var input = new { inputIn = "1;", expected = 1 };
            //var input = new { inputIn = "2;", expected = 2 };
            //var input = new { inputIn = "1 + 2;", expected = 3 };

            Lexer lexer = new Lexer(input.inputIn);
            Parser prog = new Parser(lexer);
            
            Ast.Program astProg = prog.ParseProgram();

        }

        public static void RunEvaluator()
        {
            Repl repl = new Repl();
            repl.StartEvaluator();
        }

        public static void RunLexer() {
            Repl repl = new Repl();
            repl.StartLexer();
        }

        public static void RunParser()
        {
            Repl repl = new Repl();
            repl.StartParser();
        }

        public static void TestParser()
        {
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
            //Console.WriteLine(astProgram.String());
            //Dump(astProgram);
            //DumpAsYaml(astProgram);
            astProgram.Dump();
            //WriteTokens(lexer);

        }

        private static void Dump(object o)
        {
            string json = JsonConvert.SerializeObject(o, Formatting.Indented);
            Console.WriteLine(json);
        }

        private static void DumpAsYaml(object o)
        {
            var stringBuilder = new StringBuilder();
            var serializer = new Serializer();
            serializer.Serialize(new IndentedTextWriter(new StringWriter(stringBuilder)), o);
            Console.WriteLine(stringBuilder);
        }

    }

    public static class ObjectHelper
    {
        public static void Dump<T>(this T x)
        {
            string json = JsonConvert.SerializeObject(x, Formatting.Indented);
            Console.WriteLine(json);
        }
    }

    //static List<Token> BuildTokenList1()
    //{

    //    List<Token> listOfTokenObj = new List<Token>();
    //    Token token1 = new Token { TokenType = "LET", Literal = "let" };
    //    Token token2 = new Token { TokenType = "IDENT", Literal = "x" };
    //    Token token3 = new Token { TokenType = "=", Literal = "=" };
    //    Token token4 = new Token { TokenType = "INT", Literal = "5" };
    //    Token token5 = new Token { TokenType = ";", Literal = ";" };

    //    listOfTokenObj.Add(token1);
    //    listOfTokenObj.Add(token2);
    //    listOfTokenObj.Add(token3);
    //    listOfTokenObj.Add(token4);
    //    listOfTokenObj.Add(token5);

    //    return listOfTokenObj;
    //}

    //static List<Token> BuildTokenList2()
    //{

    //    List<Token> listOfTokenObj = new List<Token>();
    //    Token token1 = new Token { TokenType = "LET", Literal = "let" };
    //    Token token2 = new Token { TokenType = "IDENT", Literal = "x" };
    //    Token token3 = new Token { TokenType = "=", Literal = "=" };
    //    Token token4 = new Token { TokenType = "INT", Literal = "3" };
    //    Token token5 = new Token { TokenType = "+", Literal = "+" };
    //    Token token6 = new Token { TokenType = "INT", Literal = "2" };
    //    Token token7 = new Token { TokenType = ";", Literal = ";" };

    //    listOfTokenObj.Add(token1);
    //    listOfTokenObj.Add(token2);
    //    listOfTokenObj.Add(token3);
    //    listOfTokenObj.Add(token4);
    //    listOfTokenObj.Add(token5);
    //    listOfTokenObj.Add(token6);
    //    listOfTokenObj.Add(token7);

    //    return listOfTokenObj;
    //}
}

