using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpInterpreter
{
    enum NodeType {
        EQUAL,
        PLUS,
        MINUS
    }

    class Node
    {
        public string nodeValue = "";
        public int nodeType = 0;

        public Node() {
            
        }
    }
}
