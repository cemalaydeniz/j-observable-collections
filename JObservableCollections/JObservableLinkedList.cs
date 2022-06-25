// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;


namespace JObservableCollections
{
    /// <summary>
    /// The observable version of <see cref="System.Collections.Generic.LinkedList{T}"/>.<para/>
    /// To be able to get notified when a property of <see cref="System.Collections.Generic.LinkedListNode{T}.Value"/> in the <see cref="JObservableCollections.JObservableLinkedList{T}"/> changes, the <see cref="System.Collections.Generic.LinkedListNode{T}.Value"/> must either 
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the <see cref="System.Collections.Generic.LinkedListNode{T}.Value"/> implements <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of <see cref="System.Collections.Generic.LinkedListNode{T}.Value"/> changes.
    /// If the <see cref="System.Collections.Generic.LinkedListNode{T}.Value"/> inherits from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of <see cref="System.Collections.Generic.LinkedListNode{T}.Value"/> changes.
    /// </summary>
    /// <remarks>
    /// This data structure also includes accessing nodes by an index, whose O(n) = n.<para/>
    /// When you need to replace a value inside of a node, use <see cref="Replace(LinkedListNode{T}, T)"/>, whose O(n) is 1, or <see cref="Replace(int, T)"/>, whose O(n) is n, in order to get notified.<para/>
    /// Use the linked list and the values directly to get notified. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MyLinkedList}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// </remarks>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    public class JObservableLinkedList<T> : LinkedList<T>, INotifyCollectionChanged
    {
        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;


        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.LinkedList"/>
        public JObservableLinkedList() : base()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.LinkedList(IEnumerable{T})"/>
        public JObservableLinkedList(IEnumerable<T> collection) : base(collection)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.LinkedList(SerializationInfo, StreamingContext)"/>
        protected JObservableLinkedList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <summary>
        /// Gets the node at the index.
        /// </summary>
        /// <param name="index">The index of the node.</param>
        /// <returns>Returns the node that is at the index if exists</returns>
        /// <exception cref="IndexOutOfRangeException">index is out of the range.</exception>
        public LinkedListNode<T> this[int index]
        {
            get
            {
                if ((uint)index >= Count)
                    throw new IndexOutOfRangeException("index is out of the range.");

                LinkedListNode<T> result = First;
                int currentIndex = 0;
                while (currentIndex != index)
                {
                    result = result.Next;
                    currentIndex++;
                }

                return result;
            }
        }


        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.AddAfter(LinkedListNode{T}, LinkedListNode{T})"/>
        public new void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            int index = FindIndexOf(node);

            base.AddAfter(node, newNode);

            if (index >= 0)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newNode.Value, index + 1));
            }
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.AddAfter(LinkedListNode{T}, T)"/>
        public new LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            int index = FindIndexOf(node);

            LinkedListNode<T> result = base.AddAfter(node, value);

            if (index >= 0)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, index + 1));
            }

            return result;
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.AddBefore(LinkedListNode{T}, LinkedListNode{T})"/>
        public new void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            int index = FindIndexOf(node);

            base.AddBefore(node, newNode);

            if (index >= 0)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newNode.Value, index));
            }
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.AddBefore(LinkedListNode{T}, T)"/>
        public new LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            int index = FindIndexOf(node);

            LinkedListNode<T> result = base.AddBefore(node, value);

            if (index >= 0)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, index));
            }

            return result;
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.AddFirst(LinkedListNode{T})"/>
        public new void AddFirst(LinkedListNode<T> node)
        {
            base.AddFirst(node);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, node.Value, 0));
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.AddFirst(T)"/>
        public new LinkedListNode<T> AddFirst(T value)
        {
            LinkedListNode<T> result = base.AddFirst(value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, 0));

            return result;
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.AddLast(LinkedListNode{T})"/>
        public new void AddLast(LinkedListNode<T> node)
        {
            base.AddLast(node);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, node.Value, Count - 1));
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.AddLast(T)"/>
        public new LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> result = base.AddLast(value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, Count - 1));

            return result;
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.Clear"/>
        public new void Clear()
        {
            base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.Remove(LinkedListNode{T})"/>
        public new void Remove(LinkedListNode<T> node)
        {
            int index = FindIndexOf(node);
            T value = node.Value;

            base.Remove(node);

            if (index >= 0)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, index));
            }
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.Remove(T)"/>
        public new bool Remove(T value)
        {
            int index = FindIndexOf(value);

            bool result = base.Remove(value);

            if (result)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, index));
            }

            return result;
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.RemoveFirst"/>
        public new void RemoveFirst()
        {
            bool exist = false;
            T? value = default(T);
            if (First != null)
            {
                exist = true;
                value = First.Value;
            }

            base.RemoveFirst();

            if (exist)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, 0));
            }
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.RemoveLast"/>
        public new void RemoveLast()
        {
            int index = -1;
            T? value = default(T);
            if (Last != null)
            {
                index = Count - 1;
                value = Last.Value;
            }

            base.RemoveLast();

            if (index >= 0)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, index));
            }
        }


        /// <summary>
        /// Use this method to get notified when replacing the value of a node.
        /// </summary>
        /// <param name="node">Node to change its value.</param>
        /// <param name="value">The new value.</param>
        /// <exception cref="ArgumentNullException">node is null.</exception>
        /// <exception cref="InvalidOperationException">node does not belong to the current <see cref="LinkedList{T}"/>.</exception>
        /// <exception cref="NodeNotFoundException">node does not exist current <see cref="LinkedList{T}"/>.</exception>
        public void Replace(LinkedListNode<T> node, T value)
        {
            if (node == null) throw new ArgumentNullException("node is null.");
            if (node.List != this) throw new InvalidOperationException("node does not belong to the current linked list.");

            int index = FindIndexOf(node);
            if (index < 0)
                throw new NodeNotFoundException("node does not exist in the current linked list.");

            T oldValue = node.Value;
            node.Value = value;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldValue, index));
        }

        /// <summary>
        /// Use this method to get notified when replacing the value of a node.
        /// </summary>
        /// <param name="index">The index of the node whose value changes.</param>
        /// <param name="value">The new value.</param>
        /// <exception cref="NodeNotFoundException">node does not exist current <see cref="LinkedList{T}"/>.</exception>
        public void Replace(int index, T value)
        {
            if ((uint)index > Count)
                throw new IndexOutOfRangeException("index is out of the range.");

            T oldValue = this[index].Value;
            this[index].Value = value;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldValue, index));
        }


        /// <summary>
        /// Finds the index of the node in the linked list.
        /// </summary>
        /// <param name="node">Node to find in the linked list.</param>
        /// <returns>Returns the index of the node. If the node could not be found in the linked list, returns -1.</returns>
        private int FindIndexOf(LinkedListNode<T> node)
        {
            if (node == null || node.List != this || Count == 0)
                return -1;

            bool found = false;
            int index;
            for (index = 0; index < Count; index++)
            {
                if (this[index] == node)
                {
                    found = true;
                    break;
                }
            }

            return found ? index : -1;
        }

        /// <summary>
        /// Finds the index of the node that has the value in the linked list.
        /// </summary>
        /// <param name="value">Value of the node to find in the linked list.</param>
        /// <returns>Returns the index of the node that has the value. If node could not be found in the linked list, returns -1.</returns>
        private int FindIndexOf(T value)
        {
            if (Count == 0)
                return -1;

            bool found = false;
            int index = 0;
            foreach (var item in this)
            {
                if (item != null && item.Equals(value))
                {
                    found = true;
                    break;
                }

                index++;
            }

            return found ? index : -1;
        }



        [Serializable]
        private class NodeNotFoundException : Exception
        {
            public NodeNotFoundException() { }
            public NodeNotFoundException(string message) : base(message) { }
        }
    }
}
