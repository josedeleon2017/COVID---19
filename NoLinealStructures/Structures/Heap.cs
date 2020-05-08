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
        //Declaración de variables.
        public Node<T> Root { get; set; }
        public int Count = 0;
        public Delegate GetPriorityValue;
        public Delegate DateComparison;
        public Delegate Comparer;

        /// <summary>
        /// Función Add utilizada para agregar un valor al Heap.
        /// </summary>
        public void Add(T value)
        {
            Node<T> node = new Node<T>(value);
            //Caso de añadido en caso la estructura se encuentre vacía.
            if (Root == null && Count == 0)
            {
                Root = node;
                Count++;
            }
            else
            {
                //Casos secundarios de añadido para los primeros elementos.
                if (Count == 1)
                {
                    Root.Left = node;
                    Count++;
                    if ((int)GetPriorityValue.DynamicInvoke(node.Value) < (int)GetPriorityValue.DynamicInvoke(Root.Value))
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
                    if ((int)GetPriorityValue.DynamicInvoke(node.Value) < (int)GetPriorityValue.DynamicInvoke(Root.Value))
                    {

                        SwapNodeValue(Root, node);
                    }
                    else if ((int)GetPriorityValue.DynamicInvoke(node.Value) == (int)GetPriorityValue.DynamicInvoke(Root.Value) && (int)DateComparison.DynamicInvoke(node.Value, Root.Value) == -1)
                    {
                        SwapNodeValue(Root, node);
                    }
                }
                //Caso en el que la estructura tenga más de dos nodos.
                else
                {
                    //Se obtiene la dirección en la que se debe colocar el siguiente nodo para forma invariante.
                    string address = GetAddress(Count + 1);
                    Node<T> NodeTemp = Root;
                    //Recorrido hasta la posición anterior de donde se insertará el nodo.
                    //Si se encuentra un cero se sigue a la izquierda, si se encuentra un uno ses sigue a la derecha.
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
                    //Terminado el recorrido se evalua caso en el que se inserta a la izquierda.
                    if (address[address.Length - 1] == '0')
                    {
                        //Se inserta y se añade al conteo de nodos.
                        NodeTemp.Left = node;
                        Count++;
                        //Se obtiene padre del nodo insertado y el nodo insertado para evaluar.
                        Node<T> Parent = GetParent(NodeTemp.Left);
                        Node<T> Next = Parent.Left;
                        //Se evalua la condición de orden invariante, recorriendo hacia arriba, hasta llegar a raíz si es necesario.
                        while (Parent != null)
                        {
                            //Caso en el que se debe hacer cambio debido a prioridad
                            if ((int)GetPriorityValue.DynamicInvoke(Next.Value) < (int)GetPriorityValue.DynamicInvoke(Parent.Value))
                            {
                                SwapNodeValue(Parent, Next);
                                Next = Parent;
                                Parent = GetParent(Parent);
                            }
                            //Caso en el que la prioridad es igual pero debe hacerse cambio debido a fecha.
                            else if ((int)GetPriorityValue.DynamicInvoke(Next.Value) == (int)GetPriorityValue.DynamicInvoke(Parent.Value) && (int)DateComparison.DynamicInvoke(Next.Value, Parent.Value) == -1)
                            {
                                SwapNodeValue(Parent, Next);
                                Next = Parent;
                                Parent = GetParent(Parent);
                            }
                            //Caso que no hay cambio, ciclo se cierra.
                            else
                            {
                                Parent = null;
                            }
                        }
                    }
                    //Caso en el que debe insertarse a la derecha.
                    else if (address[address.Length - 1] == '1')
                    {
                        //Se inserta y se añade al conteo de nodos.
                        NodeTemp.Right = node;
                        Count++;
                        //Se obtiene a padre del nodo insertado y el nodo insertado para evaluar.
                        Node<T> Parent = GetParent(NodeTemp.Right);
                        Node<T> Next = Parent.Right;
                        //Se evalua la condición de orden invariante, recorriendo hacia arriba, hasta llegar a raíz si es necesario.
                        while (Parent != null)
                        {
                            //Caso en el que se debe hacer cambio debido a prioridad
                            if ((int)GetPriorityValue.DynamicInvoke(Next.Value) < (int)GetPriorityValue.DynamicInvoke(Parent.Value))
                            {
                                SwapNodeValue(Parent, Next);
                            }
                            //Caso en el que la prioridad es igual pero debe hacerse cambio debido a fecha.
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

        /// <summary>
        /// Función que elimina la estructura completa y la restablece a sus valores por defecto.
        /// </summary>
        public void Clear()
        {
            Root = null;
            Count = 0;
        }

        /// <summary>
        /// Función que devuelve el elemento más prioritario, encontrado en la raíz.
        /// </summary>
        public T RemoveRoot()
        {
            //Caso en el que la estructura se encuentre vacía.
            if (Count == 0)
            {
                return default;
            }
            //Caso en el que la estructura cuente con un elemento.
            else if (Count == 1)
            {
                T result = Root.Value;
                Clear();
                return result;
            }
            //Caso en el que la estructura tiene más de un elemento.
            else
            {
                //Se guarda el valor de la raìz para retornarlo.
                T result = Root.Value;
                //Se sustituye el valor de la raíz por el del último nodo agregado.
                Root.Value = GetLastAddedNode().Value;
                //Se obtiene al padre del ultimo nodo para eliminar el ultimo agregado.
                Node<T> ParentOfLast = GetParent(GetLastAddedNode());
                //caso de eliminación derecho
                if (ParentOfLast.Right != null)
                {
                    ParentOfLast.Right = null;
                }
                //Caso de eliminación izquierdo
                else if (ParentOfLast.Right == null)
                {
                    ParentOfLast.Left = null;
                }
                //Se elimina nodo eliminado del conteo de nodos de la estructura
                Count--;
                //Se llama a la función para arreglar el orden invariante.
                ReSort();
                //Se retorna el valor de la raíz original.
                return result;
            }
        }
        /// <summary>
        /// Función que se utiliza en el caso de eliminación para arreglar la condición del orden invariante, va desde la raíz hacia abajo.
        /// </summary>
        private void ReSort()
        {
            //Se obtiene la raíz
            Node<T> Temp = Root;
            //Ciclo que se asegura de que la raíz tenga al menos un subárbol.
            while (Temp.Left != null && Temp.Right != null)
            {
                //Caso en el que subarbol derecho sea nulo, por tanto unicamente hay subarbol izquierdo, esto debido a forma invariante.
                if (Temp.Right == null)
                {
                    //Comparación por prioridad, si entra se hace cambio de valores.
                    if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) > (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value))
                    {
                        SwapNodeValue(Temp, Temp.Left);
                    }
                    //Caso en el que la prioridad es igual pero debe hacerse cambio debido a fecha.
                    else if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) == (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value) && (int)DateComparison.DynamicInvoke(Temp.Value, Temp.Left.Value) == 1)
                    {
                        SwapNodeValue(Temp, Temp.Left);
                    }
                    //Cierre del ciclo para evitar el recorrido completo del árbol si no es necesario.
                    else
                    {
                        break;
                    }
                }
                //Caso en el cual el nodo tiene dos subárboles,se comprueba cual de los dos hijos es más prioritario.
                else
                {
                    //Caso en el que subárbol izquierdo es más prioritario.
                    if ((int)GetPriorityValue.DynamicInvoke(Temp.Left.Value) < (int)GetPriorityValue.DynamicInvoke(Temp.Right.Value))
                    {
                        //Comprobación por prioridad, se hace cambio de valores.
                        if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) > (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value))
                        {
                            SwapNodeValue(Temp, Temp.Left);
                            Temp = Temp.Left;
                        }
                        //caso de prioridad es igual entre padre e hijo, se comprueba fecha y se hace cambio
                        else if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) == (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value) && (int)DateComparison.DynamicInvoke(Temp.Value, Temp.Left.Value) == 1)
                        {
                            SwapNodeValue(Temp, Temp.Left);
                            Temp = Temp.Left;
                        }
                        //Cierre del ciclo para evitar el recorrido completo del árbol si no es necesario.
                        else
                        {
                            break;
                        }
                    }
                    //Caso en el que los subárboles tienen misma prioridad.
                    else if ((int)GetPriorityValue.DynamicInvoke(Temp.Left.Value) == (int)GetPriorityValue.DynamicInvoke(Temp.Right.Value))
                    {
                        //Caso en el que el subárbol izquierdo es más prioritario debido a fecha o tienen la misma prioridad.
                        if ((int)DateComparison.DynamicInvoke(Temp.Left.Value, Temp.Right.Value) == -1 || (int)DateComparison.DynamicInvoke(Temp.Left.Value, Temp.Right.Value) == 0)
                        {
                            //Comprobación por prioridad, se hace cambio de valores.
                            if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) > (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value))
                            {
                                SwapNodeValue(Temp, Temp.Left);
                                Temp = Temp.Left;
                            }
                            //caso de prioridad es igual entre padre e hijo, se comprueba fecha y se hace cambio
                            else if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) == (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value) && (int)DateComparison.DynamicInvoke(Temp.Value, Temp.Left.Value) == 1)
                            {
                                SwapNodeValue(Temp, Temp.Left);
                                Temp = Temp.Left;
                            }
                            //Cierre del ciclo para evitar el recorrido completo del árbol si no es necesario.
                            else
                            {
                                break;
                            }
                        }
                        //caso en el que el subárbol derecho es más prioritario debido a fecha.
                        else if ((int)DateComparison.DynamicInvoke(Temp.Left.Value, Temp.Right.Value) == 1)
                        {
                            //Comprobación por prioridad, se hace cambio de valores.
                            if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) > (int)GetPriorityValue.DynamicInvoke(Temp.Right.Value))
                            {
                                SwapNodeValue(Temp, Temp.Right);
                                Temp = Temp.Right;
                            }
                            //caso de prioridad es igual entre padre e hijo, se comprueba fecha y se hace cambio
                            else if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) == (int)GetPriorityValue.DynamicInvoke(Temp.Right.Value) && (int)DateComparison.DynamicInvoke(Temp.Value, Temp.Right.Value) == 1)
                            {
                                SwapNodeValue(Temp, Temp.Right);
                                Temp = Temp.Right;
                            }
                            //Cierre del ciclo para evitar el recorrido completo del árbol si no es necesario.
                            else
                            {
                                break;
                            }
                        }


                    }
                    //Caso en el que el subárbol derecho es más prioritario.
                    else if ((int)GetPriorityValue.DynamicInvoke(Temp.Right.Value) < (int)GetPriorityValue.DynamicInvoke(Temp.Left.Value))
                    {
                        //Comprobación por prioridad, se hace cambio de valores.
                        if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) > (int)GetPriorityValue.DynamicInvoke(Temp.Right.Value))
                        {
                            SwapNodeValue(Temp, Temp.Right);
                            Temp = Temp.Right;
                        }
                        //caso de prioridad es igual entre padre e hijo, se comprueba fecha y se hace cambio
                        else if ((int)GetPriorityValue.DynamicInvoke(Temp.Value) == (int)GetPriorityValue.DynamicInvoke(Temp.Right.Value) && (int)DateComparison.DynamicInvoke(Temp.Value, Temp.Right.Value) == 1)
                        {
                            SwapNodeValue(Temp, Temp.Right);
                            Temp = Temp.Right;
                        }
                        //Cierre del ciclo para evitar el recorrido completo del árbol si no es necesario.
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Función utilizada para obtener la "dirección" o indiciaciones para llegar a un nodo.
        /// </summary>
        private string GetAddress(int node)
        {
            //Convierte el número de nodo a número binario.
            string address = Convert.ToString(node, 2);
            //Regresa un string con el número en binario, excepto el primer digito el cual no es necesario.
            return address.Substring(1, address.Length - 1);
        }

        /// <summary>
        /// Función que permite hacer el  cambio de valores entre dos nodos.
        /// </summary>
        private void SwapNodeValue(Node<T> Parent, Node<T> Next)
        {
            Node<T> temp = new Node<T>(Parent.Value);
            Parent.Value = Next.Value;
            Next.Value = temp.Value;
        }

        /// <summary>
        /// Función que regresa el último nodo agregado.
        /// </summary>
        private Node<T> GetLastAddedNode()
        {
            //Primeros casos
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
            //Casos donde el número de nodos es mayor a 3
            else
            {
                //Se obtiene dirección del último nodo
                string address = GetAddress(Count);
                Node<T> Node = Root;
                //Se recorre desde raíz.
                for (int i = 0; i < address.Length; i++)
                {
                    //Caso donde aparece cero, se sigue a la izquierda.
                    if (address[i] == '0')
                    {
                        Node = Node.Left;
                    }
                    //Caso donde aparece uno, se sigue a la derecha.
                    else if (address[i] == '1')
                    {
                        Node = Node.Right;
                    }
                }
                //Se regresa el valor del último nodo.
                return Node;
            }
        }

        /// <summary>
        /// Función que regresa el padre del nodo enviado como parámetro.
        /// </summary>
        private Node<T> GetParent(Node<T> Next)
        {
            //Casos especiales.
            if (Count == 0 || Count == 1)
            {
                return null;
            }
            //Casos en el que el padre no puede ser otro que la raìz.
            else if (Count == 2 || Count == 3)
            {
                return Root;
            }
            //casos con cantidad de nodos mayor a 3.
            else
            {
                //Se entra en un ciclo for recorriendo nodo por nodo.
                for (int i = 3; i <= Count; i++)
                {
                    //Se obtiene la dirección del nodo i.
                    string address = GetAddress(i);
                    Node<T> Node = Root;
                    //Se llega hasta el nodo padre del nodo i.
                    for (int j = 0; j < address.Length - 1; j++)
                    {
                        if (address[j] == '0')
                        {
                            Node = Node.Left;
                        }
                        else if (address[j] == '1')
                        {
                            Node = Node.Right;
                        }
                    }
                    //Si el nodo izquierdo no  es nulo se compara.
                    if (Node.Left != null)
                    {
                        //Se compara usando un delegado, si es igual se regresa al nodo i.
                        if ((int)Comparer.DynamicInvoke(Next.Value, Node.Left.Value) == 0)
                        {
                            return Node;
                        }
                    }
                    //Si el nodo derecho no  es nulo se compara.
                    if (Node.Right != null)
                    {
                        //Se compara usando un delegado, si es igual se regresa al nodo i.
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
