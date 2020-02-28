using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpInterpreter
{
    public class Lexer
    {

        public string input = "";
        int position = 0;
        int readPosition = 0;
        char ch;
        char[] inpArray;

        public Lexer(string input)
        {
            inpArray = input.ToCharArray();
            ReadChar();
        }

        public Token NextToken()
        {
            Token token;// = new Token("EOF", "0");
         
            SkipWhitespace();

            switch (ch)
            {
                case '=':
                    if (PeekChar() == '=')
                    {
                        char currCh = ch;
                        ReadChar();
                        string literal = currCh.ToString() + ch.ToString();
                        //string tokenType = TokenTypes.LookupIdent(literal);
                        token = new Token(TokenTypes.EQ, literal);
                    }
                    else
                    {
                        token = new Token(TokenTypes.ASSIGN, ch.ToString());
                    }
                    break;
                case '+':
                    token = new Token(TokenTypes.PLUS, ch.ToString());
                    break;

                case '-':
                    token = new Token(TokenTypes.MINUS, ch.ToString());
                    break;
                case '!':
                    if (PeekChar() == '=')
                    {
                        char currCh = ch;
                        ReadChar();
                        string literal = currCh.ToString() + ch.ToString();
                        token = new Token(TokenTypes.NOT_EQ, literal);
                    }
                    else
                    {
                        token = new Token(TokenTypes.BANG, ch.ToString());

                    }
                    break;
                case '/':
                    token = new Token(TokenTypes.SLASH, ch.ToString());
                    break;
                case '*':
                    token = new Token(TokenTypes.ASTERISK, ch.ToString());
                    break;
                case '<':
                    token = new Token(TokenTypes.LT, ch.ToString());
                    break;
                case '>':
                    token = new Token(TokenTypes.GT, ch.ToString());
                    break;
                case ';':
                    token = new Token(TokenTypes.SEMICOLON, ch.ToString());
                    break;
                case ',':
                    token = new Token(TokenTypes.COMMA, ch.ToString());
                    break;
                case '{':
                    token = new Token(TokenTypes.LBRACE, ch.ToString());
                    break;
                case '}':
                    token = new Token(TokenTypes.RBRACE, ch.ToString());
                    break;
                case '(':
                    token = new Token(TokenTypes.LPAREN, ch.ToString());
                    break;
                case ')':
                    token = new Token(TokenTypes.RPAREN, ch.ToString());
                    break;
                case '0':
                    token = new Token(TokenTypes.EOF, "");
                    break;
                default:
                    if (IsLetter(ch))
                    {
                        token = new Token();
                        token.Literal = ReadIdentifier();
                        token.TokenType = TokenTypes.LookupIdent(token.Literal);
                        return token;
                    }
                    else if (IsDigit(ch))
                    {
                        token = new Token();
                        token.Literal = ReadNumber();
                        token.TokenType = TokenTypes.INT;
                        return token;
                    }
                    else
                    {
                        token = new Token(TokenTypes.LookupIdent(ch.ToString()), ch.ToString());
                    }
                    break;
            }

            ReadChar();

            return token;
        }

        public void SkipWhitespace()
        {
            if (ch == ' ' || ch == '\t' || ch == '\n' || ch == '\r')
            {
                ReadChar();
            }
        }

        public void ReadChar()
        {

            if (readPosition >= inpArray.Length)
            {
                ch = '0';
            }
            else
            {
                ch = inpArray[readPosition];
            }
            position = readPosition;

            readPosition += 1;
        }

        public char PeekChar()
        {
            if (readPosition >= inpArray.Length)
            {
                return '0';
            }
            else
            {
                return inpArray[readPosition];
            }
        }

        public string ReadIdentifier()
        {
            string ret = string.Empty;

            while(IsLetter(ch)){
                ret += ch.ToString();
                ReadChar();
            }
            return ret;
        }

        
        public string ReadNumber()
        {
            string ret = string.Empty;
            while (IsDigit(ch))
            {
                ret += ch.ToString();
                ReadChar();
            }
            return ret;
        }

        public bool IsLetter(char ch)
        {
            return 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z' || ch == '_';
        }

        public bool IsDigit(char ch)
        {
            return '0' <= ch && ch <= '9';
        }

    }
}
