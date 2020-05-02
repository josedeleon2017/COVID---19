using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoLinealStructures.Structures
{
    public class Tree<T> : Interfaces.ITreeDataStructure<T>
    {
        public Node<T> Root { get; set; }
        public static int Count;
        public Delegate Comparer;

        /// <summary>
        /// Inserción en Árbol AVL
        /// </summary>
        /// <param name="nodeF"></param>
        /// <param name="value"></param>
        /// <returns>La raíz con todos los sub árboles balanceados</returns>
        public Node<T> InsertAVL(Node<T> nodeF, T value)
        {
            if (nodeF == null)
            {
                Count++;
                return (new Node<T>(value));
            }

            if ((int)Comparer.DynamicInvoke(nodeF.Value, value) == 1)
            {
                nodeF.Left = InsertAVL(nodeF.Left, value);
            }
            if ((int)Comparer.DynamicInvoke(nodeF.Value, value) == -1)
            {
                nodeF.Right = InsertAVL(nodeF.Right, value);
            }

            nodeF.Factor = 1 + maxFactor(getFactor(nodeF.Left), getFactor(nodeF.Right));

            int balance = getBalance(nodeF);

            if (balance > 1 && (int)Comparer.DynamicInvoke(value, nodeF.Left.Value) == -1)
            {
                return s_Right(nodeF);
            }

            if (balance < -1 && (int)Comparer.DynamicInvoke(value, nodeF.Right.Value) == 1)
            {
                return s_Left(nodeF);
            }

            if (balance > 1 && (int)Comparer.DynamicInvoke(value, nodeF.Left.Value) == 1)
            {
                nodeF.Left = s_Left(nodeF.Left);
                return s_Right(nodeF);
            }

            if (balance < -1 && (int)Comparer.DynamicInvoke(value, nodeF.Right.Value) == -1)
            {
                nodeF.Right = s_Right(nodeF.Right);
                return s_Left(nodeF);
            }

            return nodeF;
        }

        Node<T> s_Right(Node<T> nodeF)
        {
            Node<T> currentLeft = nodeF.Left;
            Node<T> treeRight = currentLeft.Right;

            currentLeft.Right = nodeF;
            nodeF.Left = treeRight;

            nodeF.Factor = maxFactor(getFactor(nodeF.Left), getFactor(nodeF.Right)) + 1;
            currentLeft.Factor = maxFactor(getFactor(currentLeft.Left), getFactor(currentLeft.Right)) + 1;

            return currentLeft;
        }

        Node<T> s_Left(Node<T> nodeF)
        {
            Node<T> currentRight = nodeF.Right;
            Node<T> treeLeft = currentRight.Left;

            currentRight.Left = nodeF;
            nodeF.Right = treeLeft;

            nodeF.Factor = maxFactor(getFactor(nodeF.Left), getFactor(nodeF.Right)) + 1;
            currentRight.Factor = maxFactor(getFactor(currentRight.Left), getFactor(currentRight.Right)) + 1;

            return currentRight;
        }

        int getFactor(Node<T> node)
        {
            if (node == null)
            {
                return 0;
            }
            return node.Factor;
        }

        int getBalance(Node<T> node)
        {
            if (node == null)
            {
                return 0;
            }
            return getFactor(node.Left) - getFactor(node.Right);
        }

        int maxFactor(int leftFactor, int rightFactor)
        {
            if (leftFactor > rightFactor)
            {
                return leftFactor;
            }
            else
            {
                return rightFactor;
            }
        }

        public Node<T> DeleteAVL(Node<T> node, T value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Busqueda en Árbol Binario 
        /// </summary>
        /// <param name="value"></param>
        /// <returns>El nodo que contenga la llave específicada</returns>
        public T Find(T value)
        {
            return Find(Root, value);
        }

        private T Find(Node<T> nodeF, T value)
        {
            Node<T> node = new Node<T>(value);

            if (nodeF == null)
            {
                return default;
            }
            else if ((int)Comparer.DynamicInvoke(nodeF.Value, node.Value) == 0)
            {
                node.Value = nodeF.Value;
                return node.Value;
            }
            else if ((int)Comparer.DynamicInvoke(nodeF.Value, node.Value) == 1)
            {
                return Find(nodeF.Left, value);
            }
            else
            {
                return Find(nodeF.Right, value);
            }
        }

        /// <summary>
        /// Recorre el árbol en In Orden
        /// </summary>
        /// <returns>Una lista con el recorrido completo</returns>
        public List<T> ToInOrden()
        {
            List<T> currentList = new List<T>();
            try
            {
                if (Root.Value != null)
                {
                    InOrden(Root, currentList);
                }
                else
                {
                    return null;
                }
                return currentList;
            }
            catch
            {
                return null;
            }
        }

        private void InOrden(Node<T> node, List<T> currentList)
        {
            if (node.Left != null)
            {
                InOrden(node.Left, currentList);
            }
            currentList.Add(node.Value);
            if (node.Right != null)
            {
                InOrden(node.Right, currentList);
            }
        }

        /// <summary>
        /// Recorre el árbol en Post Orden
        /// </summary>
        /// <returns>Una lista con el recorrido completo</returns>
        public List<T> ToPostOrden()
        {
            List<T> currentList = new List<T>();
            try
            {
                if (Root.Value != null)
                {
                    PostOrden(Root, currentList);
                }
                else
                {
                    return null;
                }
                return currentList;
            }
            catch
            {
                return null;
            }
        }

        private void PostOrden(Node<T> node, List<T> currentList)
        {
            if (node.Left != null)
            {
                PostOrden(node.Left, currentList);
            }
            if (node.Right != null)
            {
                PostOrden(node.Right, currentList);
            }
            currentList.Add(node.Value);
        }

        /// <summary>
        /// Recorre el árbol en Pre Orden
        /// </summary>
        /// <returns>Una lista con el recorrido completo</returns>
        public List<T> ToPreOrden()
        {
            List<T> currentList = new List<T>();
            try
            {
                if (Root.Value != null)
                {
                    PreOrden(Root, currentList);
                }
                else
                {
                    return null;
                }
                return currentList;
            }
            catch
            {
                return null;
            }
        }

        private void PreOrden(Node<T> node, List<T> currentList)
        {
            currentList.Add(node.Value);
            if (node.Left != null)
            {
                PreOrden(node.Left, currentList);
            }
            if (node.Right != null)
            {
                PreOrden(node.Right, currentList);
            }
        }

        public List<T> Filter(bool position)
        {
            List<T> currentList = new List<T>();
            try
            {
                if (Root.Value != null)
                {
                    currentList.Add(Root.Value);
                    if(position)
                    {
                        InOrdenFilter(Root.Right, currentList);
                    }
                    else
                    {
                        InOrdenFilter(Root.Left, currentList);
                    }
                }
                else
                {
                    return null;
                }
                return currentList;
            }
            catch
            {
                return null;
            }
        }

        private void InOrdenFilter(Node<T> node, List<T> currentList)
        {
            if (node.Left != null)
            {
                InOrdenFilter(node.Left, currentList);
            }
            currentList.Add(node.Value);
            if (node.Right != null)
            {
                InOrdenFilter(node.Right, currentList);
            }
        }

    }
}
