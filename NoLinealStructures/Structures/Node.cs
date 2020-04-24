using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoLinealStructures.Structures
{
    public class Node<T>
    {
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public T Value { get; set; }
        public int Factor { get; set; }

        public Node(T value)
        {
            Value = value;
            Left = null;
            Right = null;
            Factor = 1;
        }
    }
}
