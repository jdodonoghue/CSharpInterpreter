using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpInterpreter
{

    public class Parser
    {

        private Token currToken = null;
        private Token peekToken = null;

        private Lexer lexer;

        private Dictionary<string, int> CallOrder = new Dictionary<string, int>(10)
        {
            {"LOWEST",      1},
            {"EQUALS",      2},
            {"LESSGREATER", 3},
            {"SUM",         4},
            {"PRODUCT",     5},
            {"PREFIX",      6},
            {"CALL",        7},
            {"INDEX",       8}
        };

        private Dictionary<string, int> Precedences = new Dictionary<string, int>(10)
        {
            {"==",  2},
            {"!=",  2},
            {"<",   3},
            {">",   3},
            {"+",   4},
            {"-",   4},
            {"/",   5},
            {"*",   5},
            {"{",   7},
            {"[",   8}
        };

        public Ast.Expression PrefixParseFn(Token token)
        {
            Ast.Expression exp = new Ast.Expression();

            return exp;
        }

        public Ast.Expression InfixParseFns(Token token)
        {
            Ast.Expression exp = new Ast.Expression();

            return exp;
        }

        public Parser(Lexer lex)
        {
            lexer = lex;
            NextToken();
            NextToken();
            
        }

        public void NextToken()
        {
            currToken = peekToken;
            peekToken = lexer.NextToken();
        }

        public bool CurrTokenIs(string tokenType) {
            return (currToken.TokenType == tokenType);
        }

        public bool PeekTokenIs(string tokenType) {

            return (peekToken.TokenType == tokenType);
        }

        public Ast.Program ParseProgram() {

            Ast.Program program = new Ast.Program();
            Ast.Statement statement = new Ast.Statement();
            List<Ast.Statement> statements = new List<Ast.Statement>();

            program.Statements = statements;

            while (!CurrTokenIs(TokenTypes.EOF))
            {
                statement = ParseStatement();
                program.Statements.Add(statement);
                
                NextToken();
            }
            return program;
        }

        public Ast.Statement ParseStatement() {
            switch (currToken.TokenType)
            {
                case "LET":
                    return ParseLetStatement();
                case "RETURN":
                    return ParseReturnStatement();
                default:
                    return ParseExpressionStatement();
            }
        }
        
        public void PeekError(string tokenType)
        {
            var msg = String.Format("expected next token to be {0}, got {1} instead",
                tokenType, peekToken.TokenType);

            //p.errors = append(p.errors, msg)
        }

        public bool ExpectPeek(string tokenType) {
            if (PeekTokenIs(tokenType)) {
                NextToken();
                return true;
	        } else {
                PeekError(tokenType);
                return false;
	        }
        }

        public Ast.Statement ParseLetStatement() { 
            var stmt = new Ast.LetStatement();
            stmt.token = currToken;

            if (!ExpectPeek(TokenTypes.IDENT)) {
                return new Ast.Statement();
            }

            stmt.Name = new Ast.Identifier();
            stmt.Name.token = currToken;
            stmt.Name.Value = currToken.Literal;

            if (!ExpectPeek(TokenTypes.ASSIGN)) {
                return new Ast.Statement();
            }

            NextToken();

            stmt.Value = ParseExpression(CallOrder["LOWEST"]);
                 
            if (peekToken.TokenType == TokenTypes.SEMICOLON)
            {
                NextToken();
            }

            Ast.Statement astStatement = new Ast.Statement();
            astStatement = stmt;
            return astStatement;

        }

        public Ast.ExpressionStatement ParseExpressionStatement()
        {
            Ast.ExpressionStatement statement = new Ast.ExpressionStatement();

            statement.token = currToken;

            statement.expression = ParseExpression(CallOrder["LOWEST"]);

            if (peekToken.TokenType == TokenTypes.SEMICOLON)
            {
                NextToken();
            }

            return statement;
        }

        public Ast.Expression ParseExpression(int precedence)
        {
            Ast.Expression leftExp = null;
            
            switch (currToken.TokenType)
            {
                case "IDENT":
                    leftExp = ParseIdentifier();
                    break;
                case "INT":
                    leftExp = ParseIntegerLiteral();
                    break;
                case "!":
                    leftExp = ParsePrefixExpression();
                    break;
                case "-":
                    leftExp = ParsePrefixExpression();
                    break;
                case "TRUE":
                    leftExp = ParseBoolean();
                    break;
                case "FALSE":
                    leftExp = ParseBoolean();
                    break;
                case "(":
                    leftExp = ParseGroupedExpression();
                    break;
                case "IF":
                    leftExp = ParseIfExpression();
                    break;
                case "FUNCTION":
                    leftExp = ParseFunctionLiteral();
                    break;
                default:
                    break;
            }

            while (!PeekTokenIs(TokenTypes.SEMICOLON) && (int)precedence < PeekPrecedence())
            {
                Ast.Expression infixExp = null;

                NextToken();

                switch (currToken.TokenType)
                {
                    case "+":
                        infixExp = ParseInfixExpression(leftExp);
                        break;
                    case "-":
                        infixExp = ParseInfixExpression(leftExp);
                        break;
                    case "/":
                        infixExp = ParseInfixExpression(leftExp);
                        break;
                    case "*":
                        infixExp = ParseInfixExpression(leftExp);
                        break;
                    case "==":
                        infixExp = ParseInfixExpression(leftExp);
                        break;
                    case "!=":
                        infixExp = ParseInfixExpression(leftExp);
                        break;
                    case "<":
                        infixExp = ParseInfixExpression(leftExp);
                        break;
                    case ">":
                        infixExp = ParseInfixExpression(leftExp);
                        break;
                    case "(":
                        infixExp = ParseCallExpression(leftExp);
                        break;
                    default:
                        break;
                }
                
                if (infixExp == null) {
                    return leftExp;
                }
                
                leftExp = infixExp;
            }

            return leftExp;

        }

        public Ast.PrefixExpression ParsePrefixExpression()
        {
            Ast.PrefixExpression expression = new Ast.PrefixExpression();
            expression.token = currToken;
            expression.Operator = currToken.Literal;

            NextToken();

            expression.Right = ParseExpression(CallOrder["PREFIX"]);

            return expression;
        }

        public Ast.InfixExpression ParseInfixExpression(Ast.Expression left)
        {
            Ast.InfixExpression expression = new Ast.InfixExpression();
            expression.token = currToken;
            expression.Operator = currToken.Literal;
            expression.Left = left;

            var precedence = CurrPrecedence();

            NextToken();

            expression.Right = ParseExpression(precedence);

            return expression;
        }

        public Ast.Expression ParseCallExpression(Ast.Expression function) {
            Ast.CallExpression exp = new Ast.CallExpression();
            exp.token = currToken;
            exp.function = function;

            exp.Arguments = ParseExpressionList(TokenTypes.RPAREN);
            return exp;
        }

        public List<Ast.Expression> ParseExpressionList(string end) {
            List<Ast.Expression> list = new List<Ast.Expression>();

	        if (PeekTokenIs(end)) {
                NextToken();
                return list;
	        }

            NextToken();
            list.Add(ParseExpression(CallOrder["LOWEST"]));

	        while(PeekTokenIs(TokenTypes.COMMA)) {
                NextToken();
                NextToken();

                list.Add(ParseExpression(CallOrder["LOWEST"]));
	        }

	        if (!ExpectPeek(end)) {
                return null;
	        }

            return list;
        }


        public Ast.Expression ParseIndexExpression(Ast.Expression left) {
            Ast.IndexExpression exp = new Ast.IndexExpression();
            exp.token = currToken;
            exp.left = left;
            NextToken();

            exp.index = ParseExpression(CallOrder["LOWEST"]);

	        if (!ExpectPeek(TokenTypes.RBRACKET)) {
                return null;
            }

            return exp;
        }

        public Ast.Expression ParseArrayLiteral() {
            Ast.ArrayLiteral array = new Ast.ArrayLiteral();
            array.token = currToken;

            array.Elements = ParseExpressionList(TokenTypes.RBRACKET);

            return array;
        }

        public Ast.Expression ParseFunctionLiteral() {
            Ast.FunctionLiteral lit = new Ast.FunctionLiteral();
            lit.token = currToken;
       
	        if (!ExpectPeek(TokenTypes.LPAREN)) {
                return null;
            }

            lit.parameters = ParseFunctionParameters();

	        if (!ExpectPeek(TokenTypes.LBRACE)) {
                return null;
	        }

            lit.body = ParseBlockStatement();

            return lit;
        }

        public List<Ast.Identifier> ParseFunctionParameters() {
            List<Ast.Identifier> identifiers = new List<Ast.Identifier>();
            
	        if (PeekTokenIs(TokenTypes.RPAREN)) {
                NextToken();

                return identifiers;

            }

            NextToken();

            Ast.Identifier ident = new Ast.Identifier();
            ident.token = currToken;
            ident.Value = currToken.Literal;
            identifiers.Add(ident);
        
	        while(PeekTokenIs(TokenTypes.COMMA)) {
                NextToken();

                NextToken();
                Ast.Identifier ident1 = new Ast.Identifier();
                ident1.token = currToken;
                ident1.Value = currToken.Literal;
                identifiers.Add(ident1);

            }

	        if (!ExpectPeek(TokenTypes.RPAREN)) {
                return null;

            }

            return identifiers;
        }

        public Ast.IfExpression ParseIfExpression() {
            //Ast.Expression expression = new Ast.Expression();
            Ast.IfExpression ifExpression = new Ast.IfExpression();
            ifExpression.token = currToken;

	        if (!ExpectPeek(TokenTypes.LPAREN)) {
                return ifExpression;
            }

            NextToken();

            ifExpression.Condition = ParseExpression(CallOrder["LOWEST"]);
            

            if (!ExpectPeek(TokenTypes.RPAREN)) {
                return ifExpression;
            }

            if (!ExpectPeek(TokenTypes.LBRACE)) {
                return ifExpression;
            }

            ifExpression.Consequence = ParseBlockStatement();

            if (PeekTokenIs(TokenTypes.ELSE)) {
                NextToken();
            
                if (!ExpectPeek(TokenTypes.LBRACE)) {
                    return ifExpression;
		        }

                ifExpression.Alternative = ParseBlockStatement();
	        }

            return ifExpression;
        }

        public Ast.BlockStatement ParseBlockStatement() {
        
            Ast.BlockStatement block = new Ast.BlockStatement();
            block.token = currToken;
            List<Ast.Statement> statements = new List<Ast.Statement>();

            Ast.Statement statement = new Ast.Statement();

            block.Statements = statements;
            

            NextToken();

            while (!CurrTokenIs(TokenTypes.RBRACE) && !CurrTokenIs(TokenTypes.EOF)) { 
                
                statement = ParseStatement();
		        if (statement != null) {
                    block.Statements.Add(statement);
		        }
                NextToken();
	        }

            return block;
        }


        public Ast.Expression ParseGroupedExpression()
        {
            Ast.Expression exp = new Ast.Expression();
            NextToken();

            exp = ParseExpression(CallOrder["LOWEST"]);

	        if (!ExpectPeek(TokenTypes.RPAREN)) {
                return exp;
            }

            return exp;
        }


        public Ast.Expression ParseBoolean() {
            Ast.Boolean astBoolean = new Ast.Boolean();
            astBoolean.token = currToken;
            astBoolean.Value = CurrTokenIs(TokenTypes.TRUE);
            return astBoolean;
        }

        public Ast.Expression ParseIntegerLiteral() {
            Ast.Expression expression = new Ast.Expression();

            Ast.IntegerLiteral lit = new Ast.IntegerLiteral();
            lit.token = currToken;
            /*            
	        if err != nil {
		        msg := fmt.Sprintf("could not parse %q as integer", p.curToken.Literal)
		        p.errors = append(p.errors, msg)
		        return nil
            }
            */
            lit.Value = Convert.ToInt32(currToken.Literal);
            expression = lit;
            return expression;
        }

        public Ast.Expression ParseIdentifier() {

            Ast.Identifier identifier = new Ast.Identifier();
                        
            identifier.token = currToken;
            identifier.Value = currToken.Literal;

            return identifier;
        }

        
       

        public int CurrPrecedence() {

            if (Precedences.ContainsKey(currToken.TokenType))
            {
                return Precedences[currToken.TokenType];
            }
            return CallOrder["LOWEST"];
        }


        public Ast.Statement ParseReturnStatement()
        {
            Ast.ReturnStatement statement = new Ast.ReturnStatement();
            statement.token = currToken;

            NextToken();

            statement.ReturnValue = ParseExpression(CallOrder["LOWEST"]);

            if (peekToken.TokenType == TokenTypes.SEMICOLON)
            {
                NextToken();
            }

            return statement;
        }

        public Ast.Expression ParseStringLiteral() {
            Ast.StringLiteral stringLiteral = new Ast.StringLiteral();
            stringLiteral.token = currToken;
            stringLiteral.Value = currToken.Literal;
            return stringLiteral;
        }

        

        public Ast.Expression ParseHashLiteral()  {
          
            Ast.HashLiteral hash = new Ast.HashLiteral();
            hash.token = currToken;

            //hash.Pairs = Ast.Expression;

            while (!PeekTokenIs(TokenTypes.RBRACE))
            {
                NextToken();
                Ast.Expression key = ParseExpression(CallOrder["LOWEST"]);

                if (!ExpectPeek(TokenTypes.COLON)) {
                    return hash;
                }
                NextToken();
                var value = ParseExpression(CallOrder["LOWEST"]);

                while (!PeekTokenIs(TokenTypes.RBRACE)) {
                    NextToken();
                    key = ParseExpression(CallOrder["LOWEST"]);

                    if (!ExpectPeek(TokenTypes.COLON)) {
                        return hash;
                    }

                    NextToken();
                    value = ParseExpression(CallOrder["LOWEST"]);

                    //hash.Pairs[key] = value;


                    if (!PeekTokenIs(TokenTypes.RBRACE) && !ExpectPeek(TokenTypes.COMMA)) {
                        return null;
                    }
                }

                if (!ExpectPeek(TokenTypes.RBRACE)) {
                    return hash;
                }
            }
                return hash;
        }

        public int PeekPrecedence() {

            if (Precedences.ContainsKey(peekToken.TokenType)) {
                //int ret = Precedences[peekToken.TokenType];
                //return ret;
                return Precedences[peekToken.TokenType];
            }
            return CallOrder["LOWEST"];
        }

        

    }

}
