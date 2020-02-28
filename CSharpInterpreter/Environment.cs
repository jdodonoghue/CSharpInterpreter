using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpInterpreter
{
    public class Environment
    {

        Dictionary<string, IObject> store = new Dictionary<string, IObject>(100);
        //Environment outer;

        public Environment()
        {
            
        }

        //public Environment NewEnclosedEnvironment(Environment outer)
        //{
        //    var env = NewEnvironment();

        //env.outer = outer;
        //    return env;
        //}



        public void Set(string name, IObject val)
        {
            store[name] = val;
        }

        public IObject Get(string name)
        {
            var obj = store[name];
            //if (obj == null && outer != null) {
            //    obj = outer.Get(name);
            //
            //}

            return obj;
        }


    }
}



