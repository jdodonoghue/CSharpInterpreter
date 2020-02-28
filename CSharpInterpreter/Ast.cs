using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpInterpreter
{
    public class Ast
    {
        
        // The base Node interface
        public interface Node
        {
            string TokenLiteral();
            string ToString();

        }
       
        // All statement nodes implement this
        public class Statement : Node
        {
            public void StatementNode() {
                throw new NotImplementedException();
            }

            public string TokenLiteral()
            {
                return "";
            }
        }
        
        // All expression nodes implement this
        public class Expression : Node
        {
            public void ExpressionNode() { }

            public string String()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return "";
            }

            public string TokenLiteral()
            {
                return "";
                
            }

        }

        public class Identifier : Expression
        {
            public Token token;  // the token.IDENT token
            public string Value;

            public override string ToString()
            {
                string ret = "";
                ret = token.ToString();

                return ret;
            }
        }

        // Statements
        public class LetStatement : Statement
        {
            public Token token;
            public Identifier Name;
            public Expression Value;

            public override string ToString()
            {
                string ret = "";
                ret = token.ToString();
                ret += " Name: " + Name.Value;
                ret += " Value: " + Value.TokenLiteral();
                return ret;
            }

        }

        public class Program : Statement
        {
            public List<Ast.Statement> Statements;
 
            public string String() {
                string ret = "";
                foreach (var stmt in Statements) {
                    ret += stmt.ToString();
                }
                return ret;
            }
        }

        public class IntegerLiteral : Expression
        {
            public Token token;
            public int Value;

            public override string ToString()
            {
                string ret = "";
                ret = token.ToString();
                ret += " Value: " + Value.ToString();
                return ret;
            }

        }

    
        public class ReturnStatement : Statement
        {
            public Token token;
            public Expression ReturnValue;

            public override string ToString()
            {
                string ret = "";
                ret = token.ToString();
                ret += " ReturnValue: " + ReturnValue.TokenLiteral();
                return ret;
            }

        }

        public class ExpressionStatement : Statement
        {
            public Token token;
            public Expression expression;
            public override string ToString()
            {
                string ret = "";
                ret += token.ToString() + " " + expression.ToString();
                
                return ret;
            }

        }

        public class HashLiteral : Expression
        {
            public Token token;
            public List<Expression> Pairs;
        }

        public class IndexExpression : Expression
        {
            public Token token;
            public Expression left;
            public Expression index;
        }

        public class BlockStatement : Expression
        {
            public Token token;
            public List<Statement> Statements;

            public new string String()
            {
                string ret = "";
                foreach (var stmt in Statements)
                {
                    ret += stmt.TokenLiteral();
                }
                return ret;
            }
        }

        public class PrefixExpression : Expression
        {
            public Token token;  // The prefix token, e.g. !
            public string Operator;
            public Expression Right;

            public override string ToString()
            {
                string ret = "";
                ret = token.ToString();
                ret += " Operator: " + Operator;
                ret += " Right: " + Right.TokenLiteral();
                return ret;
            }


        }

        public class InfixExpression : Expression
        {
            public Token token;  // The operator token, e.g. +
            public Expression Left;

            public string Operator;
            public Expression Right;

            public override string ToString()
            {
                string ret = "";
                ret = token.ToString();
                ret += " Left: " + Left.TokenLiteral();
                ret += " Operator: " + Operator;
                ret += " Right: " + Right.TokenLiteral();
                return ret;
            }
        }

        public class IfExpression : Expression
        {

            public Token token;

            public Expression Condition;

            public BlockStatement Consequence;

            public BlockStatement Alternative;
        }


        public class FunctionLiteral : Expression
        {
            public Token token;
            public List<Identifier> parameters;
            public BlockStatement body;
        }

        public class CallExpression : Expression
        {
            public Token token;
            public Expression function;
            public List<Ast.Expression> Arguments;
        }

        public class StringLiteral : Expression
        {
            public Token token;
            public string Value;
        }

        public class Boolean : Expression
        {
            public Token token;
            public bool Value;
        }

        public class ArrayLiteral : Expression
        {
            public Token token;
            public List<Ast.Expression> Elements;
        }

        public Ast()
        {

        }


    }
}
