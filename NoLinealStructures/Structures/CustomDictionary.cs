using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoLinealStructures.Structures
{
    public class CustomDictionary<T> : Interfaces.ICustomDictionaryDataStructure<T>
    {
        private Dictionary<string, T[]> Dictionary = new Dictionary<string, T[]>();
        public Delegate Comparer;
        public bool Add(string key, T value)
        {
            if (Dictionary.ContainsKey(key))
            {
                for (int i = 0; i < Dictionary[key].Length; i++)
                {
                    if (Dictionary[key][i] == null)
                    {
                        Dictionary[key][i] = value;
                        return true;
                    }
                }
            }
            else
            {
                T[] currentArray = new T[10];
                Dictionary.Add(key, currentArray);
                Dictionary[key][0] = value;
                return true;
            }
            return false;
        }

        public int CountEmptys(string key)
        {
            if (!Dictionary.ContainsKey(key))
            {
                return 10;
            }
            int count = 0;
            for (int i = 0; i < Dictionary[key].Count(); i++)
            {
                if (Dictionary[key][i] == null)
                {
                    count++;
                }
            }
            return count;
        }

        public T Find(string key)
        {
            string index = key.Substring(0, 2);
            if (Dictionary.ContainsKey(index))
            {
                if (key.Contains("10"))
                {
                    int position = Convert.ToInt32(key.Substring(3, 2));
                    return Dictionary[index][position - 1];
                }
                else
                {
                    int position = Convert.ToInt32(key.Substring(3, 1));
                    return Dictionary[index][position - 1];
                }
            }
            else
            {
                return default;
            }
        }

        public string Positions(string key)
        {
            string index = key.Substring(0, 2);
            if (Dictionary.ContainsKey(index))
            {
                string result = "";
                int count = 0;
                for (int i = 0; i < 10; i++)
                {
                    if (Dictionary[index][i] == null)
                    {
                        int position = i + 1;
                        count++;
                        result += " [" + Convert.ToString(position) + "] ";
                    }
                    if (count == 10)
                    {
                        return " [1 - 10] ";
                    }
                }
                return result;
            }
            else
            {
                return " [1 - 10] ";
            }
        }

        public void Remove(string key, string datakey)
        {
            string index = key.Substring(0, 2);
            if (Dictionary.ContainsKey(index))
            {
                for (int i = 0; i < 10; i++)
                {
                    if (Dictionary[index][i] != null)
                    {
                        if ((int)Comparer.DynamicInvoke(Dictionary[index][i], datakey) == 0)
                        {
                            Dictionary[index][i] = default;
                            return;
                        }
                    }
                }
            }
        }

        public bool Contains(string key)
        {
            return Dictionary.ContainsKey(key);
        }

        public List<T> ToList(string key)
        {
            List<T> currentList = new List<T>();
            if (!Dictionary.ContainsKey(key))
            {
                return currentList;
            }
            for (int i = 0; i < Dictionary[key].Length; i++)
            {
                if (Dictionary[key][i] != null)
                {
                    currentList.Add(Dictionary[key][i]);
                }
            }
            return currentList;
        }

        public bool isFull(string key)
        {
            int count = 0;
            if (!Dictionary.ContainsKey(key))
            {
                return false;
            }
            for (int i = 0; i < Dictionary[key].Length; i++)
            {
                if (Dictionary[key][i] != null)
                {
                    count++;
                }
            }
            if (count == 10)
            {
                return true;
            }
            return false;
        }
    }
}
