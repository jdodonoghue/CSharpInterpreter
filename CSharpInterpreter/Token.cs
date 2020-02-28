using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpInterpreter
{
    public class Token
    {
        public string TokenType = "";
        public string Literal = "";

        public Token() {

        }

        public Token(string tokenType, string literal)
        {
            TokenType = tokenType;
            Literal = literal;
        }

        public override string ToString()
        {
            //{ Type: LET Literal:let}
            return "{Type:" + TokenType + " " + "Literal:" + Literal + "}";
        }
    }

    public static class TokenTypes
    {

        public static string ILLEGAL = "ILLEGAL";
        public static string EOF = "EOF";

        // Identifiers + literals
        public static string IDENT = "IDENT"; // add, foobar, x, y, ...
        public static string INT = "INT";   // 1343456
        public static string STRING = "STRING";

        // Operators
        public static string ASSIGN = "=";
        public static string PLUS = "+";
        public static string MINUS = "-";
        public static string BANG = "!";
        public static string ASTERISK = "*";
        public static string SLASH = "/";

        public static string LT = "<";
        public static string GT = ">";

        public static string EQ = "==";
        public static string NOT_EQ = "!=";

        // Delimiters
        public static string COMMA = ",";
        public static string SEMICOLON = ";";
        public static string COLON = ":";

        public static string LPAREN = "(";
        public static string RPAREN = ")";
        public static string LBRACE = "{";
        public static string RBRACE = "}";
        public static string LBRACKET = "[";
        public static string RBRACKET = "]";

        // Keywords
        public static string FUNCTION = "FUNCTION";
        public static string LET = "LET";
        public static string TRUE = "TRUE";
        public static string FALSE = "FALSE";
        public static string IF = "IF";
        public static string ELSE = "ELSE";
        public static string RETURN = "RETURN";

        public static Dictionary<string, string> keywords = new Dictionary<string, string>(10)
                                    {
                                        {"fn",     "FUNCTION" },
                                        {"let",    "LET"},
                                        {"true",   "TRUE"},
                                        {"false",  "FALSE"},
                                        {"if",     "IF"},
                                        {"else",   "ELSE"},
                                        {"return", "RETURN"}
                                    };


        public static string LookupIdent(string ident)
        {
            if (keywords.ContainsKey(ident))
            {
                string value = keywords[ident];
                return value;
            }
            return IDENT;
        }

    }
}
