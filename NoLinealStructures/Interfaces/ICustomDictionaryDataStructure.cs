using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoLinealStructures.Structures;

namespace NoLinealStructures.Interfaces
{
    interface ICustomDictionaryDataStructure<T>
    {
        bool Add(string key, T value);
        T Find(string key);
        int CountEmptys(string key);
        string Positions(string key);
        void Remove(string key, string datakey);
        bool Contains(string key);
        List<T> ToList(string key);
        bool isFull(string key);
    }
}
