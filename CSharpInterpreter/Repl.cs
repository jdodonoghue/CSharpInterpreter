using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpInterpreter
{
    public class Repl
    {
        const string PROMPT = ">> ";

        public void StartEvaluator()
        {

            while (true) // Loop indefinitely
            {
                Console.Write(PROMPT); // Prompt
                string line = Console.ReadLine(); // Get string from user
                if (line == "exit") // Check string
                {
                    //Environment.Exit(0);
                    break;
                }
                Console.WriteLine(line);
                Lexer lexer = new Lexer(line);
                //WriteTokens(lexer);

                Parser parser = new Parser(lexer);
                Ast.Program program = parser.ParseProgram();

                //Dump(program);
                //Console.WriteLine(program.String());
                //WriteTokens(lexer);

                //Console.Write("You typed "); // Report output
                //Console.Write(line.Length);
                //Console.WriteLine(" character(s)");
                //break;
                
                Environment env = new Environment();
                Evaluator eval = new Evaluator();
                IObject evaluated = eval.Eval(program, env);

                if (evaluated != null) {
                    Console.WriteLine(evaluated.Inspect());
                   
                }

            }

        }

        private static void Dump(object o)
        {
            string json = JsonConvert.SerializeObject(o, Formatting.Indented);
            Console.WriteLine(json);
        }

        public void StartParser()
        {

            while (true) // Loop indefinitely
            {
                Console.Write(PROMPT); // Prompt
                string line = Console.ReadLine(); // Get string from user
                if (line == "exit") // Check string
                {
                    //Environment.Exit(0);
                    break;
                }
                Console.WriteLine(line);
                Lexer lexer = new Lexer(line);
                WriteTokens(lexer);

                Parser prog = new Parser(lexer);
                prog.ParseProgram();

                WriteTokens(lexer);

                Console.Write("You typed "); // Report output
                Console.Write(line.Length);
                Console.WriteLine(" character(s)");
                //break;
            }

        }

        public void StartLexer()
        {

            while (true) // Loop indefinitely
            {
                Console.Write(PROMPT); // Prompt
                string line = Console.ReadLine(); // Get string from user
                if (line == "exit") // Check string
                {
                    //Environment.Exit(0);
                    break;
                }
                Console.WriteLine(line);
                Lexer lexer = new Lexer(line);

                WriteTokens(lexer);

                Console.Write("You typed "); // Report output
                Console.Write(line.Length);
                Console.WriteLine(" character(s)");
                //break;
            }

        }

        private void WriteTokens(Lexer lexer)
        {
            Token token;

            for (token = lexer.NextToken(); token.TokenType != TokenTypes.EOF; token = lexer.NextToken())
            {
                Console.WriteLine(token.ToString());
            }
        }
    }
}
