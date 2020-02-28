using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpInterpreter
{
    public class Evaluator
    {
        private IObject NULL = new NullObj();
        private IObject TRUE = new BooleanObj(true);
        private IObject FALSE = new BooleanObj(false);


        public Evaluator()
        {

        }
        
        public static class ASTTypes
        {
            public const string astProgram = nameof(Ast.Program);
        }
        
        public IObject Eval(Ast.Node node, Environment env)
        {

            // Statements
            if (node.GetType() == typeof(Ast.Program))
            {
                Ast.Program program = (Ast.Program)node;
                return evalProgram(program, env);
            }

            if (node.GetType() == typeof(Ast.BlockStatement))
            {
                Ast.BlockStatement blockStmt = new Ast.BlockStatement();
                blockStmt = (Ast.BlockStatement)node;
                return evalBlockStatement(blockStmt, env);
            }

            if (node.GetType() == typeof(Ast.ExpressionStatement))
            {
                Ast.ExpressionStatement expStmt = new Ast.ExpressionStatement();
                expStmt = (Ast.ExpressionStatement)node;
                return Eval(expStmt.expression, env);
            }

            if (node.GetType() == typeof(Ast.ReturnStatement))
            {
                Ast.ReturnStatement retStmt = new Ast.ReturnStatement();
                retStmt = (Ast.ReturnStatement)node;
                return Eval(retStmt.ReturnValue, env);
            }

            if (node.GetType() == typeof(Ast.LetStatement))
            {
                Ast.LetStatement letStmt = new Ast.LetStatement();
                letStmt = (Ast.LetStatement)node;
                var val = Eval(letStmt.Value, env);
                env.Set(letStmt.Name.Value, val);
                return val;
            }

            // Expressions
            if (node.GetType() == typeof(Ast.IntegerLiteral))
            {
                Ast.IntegerLiteral intLiteral = new Ast.IntegerLiteral();
                intLiteral = (Ast.IntegerLiteral)node;
                IntegerObj intObj = new IntegerObj(intLiteral.Value);
                return intObj;
            }

            if (node.GetType() == typeof(Ast.StringLiteral))
            {
                Ast.StringLiteral stringLiteral = new Ast.StringLiteral();
                stringLiteral = (Ast.StringLiteral)node;
                StringObj strObj = new StringObj(stringLiteral.Value);
                return strObj;
            }

            if (node.GetType() == typeof(Ast.Boolean))
            {
                Ast.Boolean astBoolean = new Ast.Boolean();
                astBoolean = (Ast.Boolean)node;
                BooleanObj booleanObj = new BooleanObj(astBoolean.Value);
                return booleanObj;
            }
            
            if (node.GetType() == typeof(Ast.PrefixExpression))
            {
                Ast.PrefixExpression astPrefixExp = new Ast.PrefixExpression();
                astPrefixExp = (Ast.PrefixExpression)node;

                var right = Eval(astPrefixExp.Right, env);

                return evalPrefixExpression(astPrefixExp.Operator, right);

            }

            if (node.GetType() == typeof(Ast.InfixExpression))
            {
                Ast.InfixExpression astInfixExp = new Ast.InfixExpression();
                astInfixExp = (Ast.InfixExpression)node;

                var left = Eval(astInfixExp.Left, env);

                var right = Eval(astInfixExp.Right, env);

                return evalInfixExpression(astInfixExp.Operator, left, right);

            }
            
            if (node.GetType() == typeof(Ast.IfExpression))
            {
                Ast.IfExpression astIfExp = new Ast.IfExpression();
                astIfExp = (Ast.IfExpression)node;
                return evalIfExpression(astIfExp, env);

            }

            if (node.GetType() == typeof(Ast.Identifier))
            {
                Ast.Identifier astIdentifier = new Ast.Identifier();
                return evalIdentifier(astIdentifier, env);
            }

            if (node.GetType() == typeof(Ast.FunctionLiteral))
            {
                Ast.FunctionLiteral funcLiteral = new Ast.FunctionLiteral();
                funcLiteral = (Ast.FunctionLiteral)node;

                var params1 = funcLiteral.parameters;
                var body = funcLiteral.body;

                FunctionObj funcObj = new FunctionObj();
                //funcObj.params = params1;
                //funcObj.env = env;
                //funcObj.body = body;
                return funcObj;
            }
            
            if (node.GetType() == typeof(Ast.CallExpression))
            {
                Ast.CallExpression callExp = new Ast.CallExpression();
                callExp = (Ast.CallExpression)node;

                var function = Eval(callExp.function, env);

                //var args = evalExpressions(callExp.Arguments, env);

                //return applyFunction(function, args);
            }
            
            if (node.GetType() == typeof(Ast.ArrayLiteral))
            {
                Ast.ArrayLiteral arrayLiteral = new Ast.ArrayLiteral();
                arrayLiteral = (Ast.ArrayLiteral)node;
                //var elements = evalExpressions(arrayLiteral.Elements, env);
                ArrayObj arrayObj = new ArrayObj();
                //arrayObj.elements = arrayLiteral.Elements;
                return arrayObj;
            }
            
            if (node.GetType() == typeof(Ast.IndexExpression))
            {
                Ast.IndexExpression indexExpression = new Ast.IndexExpression();
                indexExpression = (Ast.IndexExpression)node;
                var left = Eval(indexExpression.left, env);
                var index = Eval(indexExpression.index, env);
                //return evalIndexExpression(left.ToString(), index.ToString());
            }
            
            if (node.GetType() == typeof(Ast.HashLiteral))
            {
                Ast.HashLiteral hashLiteral = new Ast.HashLiteral();
                //return evalHashLiteral(hashLiteral, env);

            }
            
            return null;

        }

        public IObject evalProgram(Ast.Program program, Environment env)
        {
            IObject result = null;

            foreach (var statement in program.Statements)
            {
                result = Eval(statement, env);
                
                if (result.Type() == Object.RETURN_VALUE_OBJ)
                {
                    return (ReturnObj)result;
                }
                if (result.Type() == Object.ERROR_OBJ)
                {
                    return (ErrorObj)result;
                }
                
            }

            return result;
        }

        public IObject evalBlockStatement(Ast.BlockStatement block, Environment env)
        {
            IObject result = null;

            foreach (var statement in block.Statements)
            {
                result = Eval(statement, env);

                if (result != null)
                {
                    var rt = result.Type();
                    if (rt == Object.RETURN_VALUE_OBJ || rt == Object.ERROR_OBJ)
                    {
                        return result;
                    }
                }
            }

            return result;
        }

        public IObject nativeBoolToBooleanObject(bool input)
        {
            if (input == true)
            {
                return new BooleanObj(true);
            }
            return new BooleanObj(false);
        }

        public IObject evalPrefixExpression(string operatorValue, IObject right)
        {

            switch (operatorValue)
            {
                case "!":
                    return evalBangOperatorExpression(right);
                case "-":
                    return evalMinusPrefixOperatorExpression(right);
                default:
                    //return newError("unknown operator: %s%s", operatorValue, right.Type());
                    return null;
            }

        }


        public IObject evalInfixExpression(string operatorValue, IObject left, IObject right)
        {
            if (left.Type() == Object.INTEGER_OBJ && right.Type() == Object.INTEGER_OBJ)
            {
                return evalIntegerInfixExpression(operatorValue, (IntegerObj)left, (IntegerObj)right);
            }
            
            if (left.Type() == right.Type())
            {
                if (operatorValue == "==")
                    return nativeBoolToBooleanObject(left.Inspect() == right.Inspect());
                if (operatorValue == "!=")
                    return nativeBoolToBooleanObject(left.Inspect() != right.Inspect());
            }

            if (left.Type().ToString() != right.Type().ToString())
            {
                return newError("type mismatch: " + left.Type().ToString() + " " + operatorValue.ToString() + " " + right.Type().ToString());
            }

            return newError("unknown operator: " + left.Type().ToString() + " " + operatorValue.ToString() + " " + right.Type().ToString());
            
        }

        
        public IObject evalBangOperatorExpression(IObject right) {

            switch (right.Inspect()) {
                case "True":
                    return FALSE;
                case "False":
                    return TRUE;
                case "NULL":
                    return TRUE;
                default:
                    return FALSE;
            }

        }
        
        public IObject evalMinusPrefixOperatorExpression(IObject right)
        {
            
            if (right.Type() != Object.INTEGER_OBJ) {
                return newError("unknown operator: " +  right.GetType().ToString());
            }
            
            return new IntegerObj(-1 * ((IntegerObj)right).Value());
        }
        

        public IObject evalIntegerInfixExpression(string operatorValue, IntegerObj left, IntegerObj right)
        {

            switch (operatorValue)
            {
                case "+":
                    return new IntegerObj(left.Value() + right.Value());
                case "-":
                    return new IntegerObj(left.Value() - right.Value());
                case "*":
                    return new IntegerObj(left.Value() * right.Value());
                case "/":
                    return new IntegerObj(left.Value() / right.Value());
                case "<":
                    return nativeBoolToBooleanObject(left.Value() < right.Value());
                case ">":
                    return nativeBoolToBooleanObject(left.Value() > right.Value());
                case "==":
                    return nativeBoolToBooleanObject(left.Value() == right.Value());
                case "!=":
                    return nativeBoolToBooleanObject(left.Value() != right.Value());
                default:
                    //return newError("unknown operator: %s %s %s",
                    //    left.Type(), operator, right.Type())
                    return new IntegerObj(0);
            }

        }
        
        public IObject evalIfExpression(Ast.IfExpression ie, Environment env)
        {

            var condition = (BooleanObj)Eval(ie.Condition, env);

            if (isError(condition))
            {
                return condition;

            }

            if (isTruthy(condition))
            {
                return Eval(ie.Consequence, env);

            }
            else if (ie.Alternative != null)
            {
                return Eval(ie.Alternative, env);
            }
            else
            {
                IntegerObj obj = new IntegerObj(0);
                return obj;
            }

        }

        public IObject evalIdentifier(Ast.Identifier node, Environment env)
        {

            var val = env.Get(node.Value);

            return val;

        }

        public bool isTruthy(BooleanObj obj)
        {

            if (obj.Inspect() == "NULL")
            {
                return false;
            }
            if (obj.Inspect() == "True")
            {
                return true;
            }
            if (obj.Inspect() == "False")
            {
                return false;
            }
            return true;
        }

        public IObject newError(string format)
        {

            ErrorObj objError = new ErrorObj();
            objError.message = format;
            //return &object.Error{Message: fmt.Sprintf(format, a...)}
            return objError;
        }

        public bool isError(IObject obj)
        {

            if (obj != null)
            {
                return obj.Type() == Object.ERROR_OBJ;
            }
            return false;

        }

        public List<IObject> evalExpressions(Ast.Expression[] exps, Environment env)
        {
            List<IObject> results = new List<IObject>();
            foreach (Ast.Expression exp in exps) {
                var evaluated = Eval(exp, env);
                results.Add(evaluated);
            }
            return results;
        }

        public IObject applyFunction(IObject fn, IObject[] args)
        {

            var function = (FunctionObj)fn;

            var extendedEnv = extendFunctionEnv(function, args);

            var evaluated = Eval(function.Body, extendedEnv);

            return unwrapReturnValue(evaluated);
        }

        public Environment extendFunctionEnv(FunctionObj fn, IObject[] args)
        {

            var env = new Environment();
            
            foreach(Ast.Identifier param in fn.Parameters)
            {
                int idx = 0;
                env.Set(param.Value, args[idx++]);
            }

            return env;
            
        }

        public IObject unwrapReturnValue(IObject obj)
        {
            var returnValue = (ReturnObj)obj;

            return returnValue;

        }

    }
}


