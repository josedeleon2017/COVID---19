using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoLinealStructures.Interfaces
{
    interface IHashTableStructure<T>
    {
        bool Add(int key, T value);
        T Find(int key, int position);
        void Remove(int key, string datakey);
        int CountEmptys(int key);
        int Count(int key);
        bool IsFull(int key);
        string Positions(int key);
        List<T> ToList(int key);
    }
}
