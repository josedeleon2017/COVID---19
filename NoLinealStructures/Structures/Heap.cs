using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoLinealStructures.Interfaces;
using NoLinealStructures.Structures;


namespace NoLinealStructures.Structures
{
    public class Heap<T> : Interfaces.IHeapStructure<T>
    {
        public Node<T> Root { get; set; }
        public int Count = 0;
        public Delegate GetPriorityValue;
        public Delegate DateComparison;
        public Delegate Comparer;
        public void Add(T value)
        {
            Node<T> node = new Node<T>(value);
            if (Root == null && Count == 0)
            {
                Root = node;
                Count++;
            }
            else
            {
                if (Count == 1)
                {
                    Root.Left = node;
                    Count++;
                    if ((int)GetPriorityValue.DynamicInvoke(node.Value) > (int)GetPriorityValue.DynamicInvoke(Root.Value))
                    {
                        SwapNodeValue(Root, node);
                    }
                    else if ((int)GetPriorityValue.DynamicInvoke(node.Value) == (int)GetPriorityValue.DynamicInvoke(Root.Value) && (int)DateComparison.DynamicInvoke(node.Value, Root.Value) == -1)
                    {
                        SwapNodeValue(Root, node);
                    }
                }
                else if (Count == 2)
                {
                    Root.Right = node;
                    Count++;
                    if ((int)GetPriorityValue.DynamicInvoke(node.Value) > (int)GetPriorityValue.DynamicInvoke(Root.Value))
                    {

                        SwapNodeValue(Root, node);
                    }
                    else if ((int)GetPriorityValue.DynamicInvoke(node.Value) == (int)GetPriorityValue.DynamicInvoke(Root.Value) && (int)DateComparison.DynamicInvoke(node.Value, Root.Value) == -1)
                    {
                        SwapNodeValue(Root, node);
                    }
                }
                else
                {
                    string address = GetAddress(Count + 1);
                    Node<T> NodeTemp = Root;
                    for (int i = 0; i < address.Length - 1; i++)
                    {
                        if (address[i] == '0')
                        {
                            NodeTemp = NodeTemp.Left;
                        }
                        else if (address[i] == '1')
                        {
                            NodeTemp = NodeTemp.Right;
                        }
                    }
                    if (address[address.Length - 1] == '0')
                    {
                        NodeTemp.Left = node;
                        Count++;
                        Node<T> Parent = GetParent(NodeTemp.Left);
                        Node<T> Next = Parent.Left;
                        while (Parent != null)
                        {
                            if ((int)GetPriorityValue.DynamicInvoke(Next.Value) > (int)GetPriorityValue.DynamicInvoke(Parent.Value))
                            {
                                SwapNodeValue(Parent, Next);
                                Next = Parent;
                                Parent = GetParent(Parent);
                            }
                            else if ((int)GetPriorityValue.DynamicInvoke(Next.Value) == (int)GetPriorityValue.DynamicInvoke(Parent.Value) && (int)DateComparison.DynamicInvoke(Next.Value, Parent.Value) == -1)
                            {
                                SwapNodeValue(Parent, Next);
                                Next = Parent;
                                Parent = GetParent(Parent);
                            }
                            else
                            {
                                Parent = null;
                            }
                        }
                    }
                    else if (address[address.Length - 1] == '1')
                    {
                        NodeTemp.Right = node;
                        Count++;
                        Node<T> Parent = GetParent(NodeTemp.Right);
                        Node<T> Next = Parent.Right;
                        while (Parent != null)
                        {
                            if ((int)GetPriorityValue.DynamicInvoke(Next.Value) > (int)GetPriorityValue.DynamicInvoke(Parent.Value))
                            {
                                SwapNodeValue(Parent, Next);
                            }
                            else if ((int)GetPriorityValue.DynamicInvoke(Next.Value) == (int)GetPriorityValue.DynamicInvoke(Parent.Value) && (int)DateComparison.DynamicInvoke(Next.Value, Parent.Value) == -1)
                            {
                                SwapNodeValue(Parent, Next);
                            }
                            Next = Parent;
                            Parent = GetParent(Parent);
                        }
                    }

                }
            }
        }

        public int Find(T value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            Root = null;
            Count = 0;
        }

        public T RemoveRoot()
        {
            if (Count == 0)
            {
                return default;
            }
            else if (Count == 1)
            {
                T result = Root.Value;
                Clear();
                return result;
            }
            else
            {
                T result = Root.Value;
                Root.Value = GetLastAddedNode().Value;
                Node<T> ParentOfLast = GetParent(GetLastAddedNode());
                if (ParentOfLast.Right != null)
                {
                    ParentOfLast.Right = null;
                }
                else if (ParentOfLast.Right == null)
                {
                    ParentOfLast.Left = null;
                }
                Count--;
                ReSort();
                return result;
            }
        }

        private void ReSort()
        {
            Node<T> Temp = Root;
            while (Temp.Left != null && Temp.Right != null)
            {
                if (Temp.Right == null)
                {
                    if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) < (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value))
                    {
                        SwapNodeValue(Temp, Temp.Left);
                    }
                    else if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) == (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value) && (int)DateComparison.DynamicInvoke(Temp.Value, Temp.Left.Value) == 1)
                    {
                        SwapNodeValue(Temp, Temp.Left);
                    }
                    else
                    {
                        //
                        break;
                    }
                }
                else
                {
                    if ((int)GetPriorityValue.DynamicInvoke(Temp.Left.Value) > (int)GetPriorityValue.DynamicInvoke(Temp.Right.Value))
                    {
                        if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) < (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value))
                        {
                            SwapNodeValue(Temp, Temp.Left);
                            Temp = Temp.Left;
                        }
                        else if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) == (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value) && (int)DateComparison.DynamicInvoke(Temp.Value, Temp.Left.Value) == 1)
                        {
                            SwapNodeValue(Temp, Temp.Left);
                            Temp = Temp.Left;
                        }
                        else
                        {
                            //
                            break;
                        }
                    }
                    //TEST
                    else if ((int)GetPriorityValue.DynamicInvoke(Temp.Left.Value) == (int)GetPriorityValue.DynamicInvoke(Temp.Right.Value))
                    {
                        if ((int)DateComparison.DynamicInvoke(Temp.Left.Value, Temp.Right.Value) == -1 || (int)DateComparison.DynamicInvoke(Temp.Left.Value, Temp.Right.Value) == 0)
                        {
                            if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) < (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value))
                            {
                                SwapNodeValue(Temp, Temp.Left);
                                Temp = Temp.Left;
                            }
                            else if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) == (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value) && (int)DateComparison.DynamicInvoke(Temp.Value, Temp.Left.Value) == 1)
                            {
                                SwapNodeValue(Temp, Temp.Left);
                                Temp = Temp.Left;
                            }
                            else
                            {
                                //
                                break;
                            }
                        }
                        else if ((int)DateComparison.DynamicInvoke(Temp.Left.Value, Temp.Right.Value) == 1)
                        {
                            if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) < (int)GetPriorityValue.DynamicInvoke(Temp.Right.Value))
                            {
                                SwapNodeValue(Temp, Temp.Right);
                                Temp = Temp.Right;
                            }
                            else if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) == (int)GetPriorityValue.DynamicInvoke(Temp.Right.Value) && (int)DateComparison.DynamicInvoke(Temp.Value, Temp.Right.Value) == 1)
                            {
                                SwapNodeValue(Temp, Temp.Right);
                                Temp = Temp.Right;
                            }
                            else
                            {
                                //
                                break;
                            }
                        }


                    }
                    //
                    else if ((int)GetPriorityValue.DynamicInvoke(Temp.Right.Value) > (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value))
                    {
                        if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) < (int)GetPriorityValue.DynamicInvoke(Temp.Right.Value))
                        {
                            SwapNodeValue(Temp, Temp.Right);
                            Temp = Temp.Right;
                        }
                        else if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) == (int)GetPriorityValue.DynamicInvoke(Temp.Right.Value) && (int)DateComparison.DynamicInvoke(Temp.Value, Temp.Right.Value) == 1)
                        {
                            SwapNodeValue(Temp, Temp.Right);
                            Temp = Temp.Right;
                        }
                        else
                        {
                            //
                            break;
                        }
                    }
                }
            }
        }
        private string GetAddress(int node)
        {
            string address = Convert.ToString(node, 2);
            return address.Substring(1, address.Length - 1);
        }
        private void SwapNodeValue(Node<T> Parent, Node<T> Next)
        {
            Node<T> temp = new Node<T>(Parent.Value);
            Parent.Value = Next.Value;
            Next.Value = temp.Value;
        }

        private Node<T> GetLastAddedNode()
        {
            Node<T> Last = Root;
            if (Count == 0)
            {
                return null;
            }
            else if (Count == 1)
            {
                return Last;
            }
            else if (Count == 2)
            {
                return Root.Left;
            }
            else if (Count == 3)
            {
                return Root.Right;
            }
            else
            {
                string address = GetAddress(Count);
                Node<T> Node = Root;
                for (int i = 0; i < address.Length; i++)
                {
                    if (address[i] == '0')
                    {
                        Node = Node.Left;
                    }
                    else if (address[i] == '1')
                    {
                        Node = Node.Right;
                    }
                }
                return Node;
            }
        }
        private Node<T> GetParent(Node<T> Next)
        {
            if (Count == 0 || Count == 1)
            {
                return null;
            }
            else if (Count == 2 || Count == 3)
            {
                return Root;
            }
            else
            {
                for (int i = 3; i <= Count; i++)
                {
                    string address = GetAddress(i);
                    Node<T> Node = Root;
                    for (int j = 0; j < address.Length - 1; j++)
                    {
                        /* 
                         if ((int)Comparer.DynamicInvoke(Next.Value, Node.Left.Value) == 0 || (int)Comparer.DynamicInvoke(Next.Value, Node.Right.Value) == 0)
                         {
                             return Node;
                         }*/
                        if (address[j] == '0')
                        {
                            Node = Node.Left;
                        }
                        else if (address[j] == '1')
                        {
                            Node = Node.Right;
                        }
                    }
                    if (Node.Left != null)
                    {
                        if ((int)Comparer.DynamicInvoke(Next.Value, Node.Left.Value) == 0)
                        {
                            return Node;
                        }
                    }
                    if (Node.Right != null)
                    {
                        if ((int)Comparer.DynamicInvoke(Next.Value, Node.Right.Value) == 0)
                        {
                            return Node;
                        }
                    }
                }
            }
            return null;
        }

    }
}
