using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoLinealStructures.Structures;

namespace NoLinealStructures.Interfaces
{
    interface ITreeDataStructure<T>
    {
        Node<T> InsertAVL(Node<T> node, T value);
        Node<T> DeleteAVL(Node<T> node, T value);
        T Find(T value);
        List<T> ToPreOrden();
        List<T> ToInOrden();
        List<T> ToPostOrden();

    }
}
