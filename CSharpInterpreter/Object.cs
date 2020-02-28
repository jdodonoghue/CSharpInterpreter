using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpInterpreter
{
    public class Object
    {

        //private static string ObjectType = "";

        public static string NULL_OBJ = "NULL";
        public static string ERROR_OBJ = "ERROR";
        public static string INTEGER_OBJ = "INTEGER";
        public static string BOOLEAN_OBJ = "BOOLEAN";
        public static string STRING_OBJ = "STRING";
        public static string RETURN_VALUE_OBJ = "RETURN_VALUE";
        public static string FUNCTION_OBJ = "FUNCTION";
        public static string BUILTIN_OBJ = "BUILTIN";
        public static string ARRAY_OBJ = "ARRAY";
        public static string HASH_OBJ = "HASH";
    }


        public class HashKey
        {
            public string Type;
            public int Value;
        }

        public interface IHashable
        {
            //public HashKey HashKey();
        }

        public interface IObject
        {
            string Type();
            string Inspect();
        }


        public class IntegerObj : IObject
        {
            private int value = 0;

            public IntegerObj(int valueIn)
            {
                value = valueIn;
            }
            public int Value() {
                return value;
            }
            public string Type()
            {
                return Object.INTEGER_OBJ;
            }

            public string Inspect()
            {
                return value.ToString();
            }

            public HashKey HashKey()
            {
                HashKey hashKey = new HashKey();
                hashKey.Type = Type();
                hashKey.Value = value;
                return hashKey;
            }
        }

        public class BooleanObj : IObject
        {

            private bool value = false;

            public BooleanObj(bool valueIn)
            {
                value = valueIn;
            }

            public string Type()
            {
                return Object.BOOLEAN_OBJ;
            }

            public string Inspect()
            {
                return value.ToString();
            }

            public HashKey HashKey()
            {
                HashKey hashKey = new HashKey();

                int value1 = 0;

                if (value1 == 0)
                {
                    value = false;
                }
                else
                {
                    value = true;
                }

                //hashKey.Type = Type();
                hashKey.Value = value1;
                return hashKey;
            }
        }

        public class NullObj : IObject
        {

            public string Type()
            {
                return Object.NULL_OBJ;
            }

            public string Inspect()
            {
                return "null";
            }

        }

        public class ReturnObj : IObject
        {
            public string value = "";
            
            public ReturnObj(string valueIn)
            {
                value = valueIn;
            }
            
            public string Type()
            {
                return Object.RETURN_VALUE_OBJ;
            }
            public string Inspect()
            {
                //return value.Inspect();
                return value.ToString();
            }

        }

        public class ErrorObj : IObject
        {
            public string message = "";

            public string Type()
            {
                return Object.ERROR_OBJ;
            }

            public string Inspect()
            {
                return "ERROR: " + message;
            }
        }

        public class FunctionObj : IObject
        {
            public List<Ast.Identifier> Parameters = new List<Ast.Identifier>();

            public Ast.BlockStatement Body;
            public Environment Env;

            public string Type()
            {
                return Object.FUNCTION_OBJ;
            }

            public string Inspect()
            {
                string output = "";

                List<string> paramsA = new List<string>();

                foreach (var p in Parameters)
                {
                    paramsA.Add(p.ToString());
                }

                output = "fn";
                output += "(";
                output += String.Join(", ", paramsA);
                output += ") {\n";
                output += Body.String();
                output += "\n}";

                return output.ToString();
            }

        }

        public class StringObj : IObject {

            string value = "";

            public StringObj(string valueIn)
            {   
                value = valueIn;
            }

            public string Type()
            {
                return Object.STRING_OBJ;
            }
            public string Inspect()
            {
                return value;
            }

            public HashKey HashKey() {

                HashKey hashKey = new HashKey();

                //var h = fnv.New64a();
                //h.Write([]byte(s.Value));

                //hashKey.Type = s.Type();
                //hashKey.Value = (int)h.Sum64();
                return hashKey;

            }
        }

        public class BuiltinObj : IObject {

            //public Object Fn(Object[] args)
            //{
            //    return new Object();
            //}

            //public Object BuiltinFunction(Object[] args)
            //{
            //    return new Object();
            //}

            public string Type()
            {
                return Object.BUILTIN_OBJ;
            }
            public string Inspect()
            {
                return "builtin function";
            }

        }

        public class ArrayObj : IObject
        {

            public List<string> elements = new List<string>();

            public string Type()
            {
                return Object.ARRAY_OBJ;
            }
            public string Inspect()
            {
                string output = "";
                
                List<string> elements = new List<string>();

                //foreach (var e in Elements)
                //{
                //    elements.Add(e.ToString());
                //}

                output += "[";
                output += String.Join(", ", elements);
                output += "]";

                return output.ToString();

            }

        }
        /*
        public class HashPair
        {
            string Key;
            string Value;
        }
        */
        /*
        public class HashObj : IObject
        {

            Dictionary<HashKey, HashPair> Pairs = new Dictionary<HashKey, HashPair>(100);

            public string Type()
            {
                return Object.HASH_OBJ;
            }

            public string Inspect()
            {

                string output = "";

                List<string> pairs = new List<string>();

                foreach (var e in Pairs)
                {
                    pairs.Add(e.ToString());
                }

                output += "{";
                output += String.Join(", ", pairs);
                output += "}";

                return output.ToString();

            }
        }
    //}
    */
}  