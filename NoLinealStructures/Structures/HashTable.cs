using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoLinealStructures.Structures
{
    public class HashTable<T> : Interfaces.IHashTableStructure<T>
    {
        private List<T[]> Dictionary = new List<T[]>(5);
        public Delegate Comparer;

        public void Inicializar()
        {
            for (int i = 0; i < Dictionary.Capacity; i++)
            {
                T[] currentArray = new T[10];
                Dictionary.Add(currentArray);
            }
        }
        public bool Add(int key, T value)
        {
            if (Dictionary.ElementAt(key) == null)
            {
                T[] currentArray = new T[10];
                currentArray[0] = value;
                Dictionary[key] = currentArray;
                return true;
            }
            else
            {
                for (int i = 0; i < Dictionary.ElementAt(key).Length; i++)
                {
                    if (Dictionary.ElementAt(key)[i] == null)
                    {
                        Dictionary.ElementAt(key)[i] = value;
                        return true;
                    }
                }                           
            }
            return false;
        }

        public int CountEmptys(int key)
        {
            int count = 0;
            for (int i = 0; i < Dictionary.ElementAt(key).Length; i++)
            {
                if (Dictionary.ElementAt(key)[i] == null)
                {
                    count++;
                }
            }
            return count;
        }

        public int Count(int key)
        {
            int count = 0;
            for (int i = 0; i < Dictionary.ElementAt(key).Length; i++)
            {
                if (Dictionary.ElementAt(key)[i] != null)
                {
                    count++;
                }
            }
            return count;
        }

        public bool IsFull(int key)
        {
            int count = 0;
            for (int i = 0; i < Dictionary.ElementAt(key).Length; i++)
            {
                if (Dictionary.ElementAt(key)[i] != null)
                {
                    count++;
                }
            }
            if (count == Dictionary.ElementAt(key).Length) return true;
            return false;
        }

        public string Positions(int key)
        {
            string result = "";
            for (int i = 0; i < Dictionary.ElementAt(key).Length; i++)
            {
                if (Dictionary.ElementAt(key)[i] == null)
                {
                    int position = i + 1;
                    result += " [" + Convert.ToString(position) + "] ";
                }
            }
            return result;
        }

        public T Find(int key, int position)
        {
            if (Dictionary.ElementAt(key)[position] != null)
            {
                return Dictionary.ElementAt(key)[position];
            }
            return default;
        }

        public void Remove(int key, string datakey)
        {
            for (int i = 0; i < Dictionary.ElementAt(key).Length; i++)
            {
                if (Dictionary.ElementAt(key)[i] != null)
                {
                    if ((int)Comparer.DynamicInvoke(Dictionary.ElementAt(key)[i], datakey) == 0)
                    {
                        Dictionary.ElementAt(key)[i] = default;
                        return;
                    }
                }
            }
        }

        public List<T> ToList(int key)
        {
            List<T> currentList = new List<T>();
            for (int i = 0; i < Dictionary.ElementAt(key).Length; i++)
            {
                if (Dictionary.ElementAt(key)[i] != null)
                {
                    currentList.Add(Dictionary.ElementAt(key)[i]);
                }
            }
            return currentList;
        }
    }
}
