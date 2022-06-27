// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


using System.Collections.Specialized;


namespace JUtility.JObservableCollections
{
    /// <summary>
    /// The observable version of <see cref="System.Collections.Generic.List{T}"/>.<para/>
    /// To be able to get notified when a property of an element in the <see cref="JObservableCollections.JObservableList{T}"/> changes, the element must either 
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the elements implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of the elements changes.
    /// If the elements inherit from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of the elements changes.
    /// </summary>
    /// <remarks>
    /// Use the list and its elements to get notified. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MyList}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// </remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class JObservableList<T> : List<T>, INotifyCollectionChanged
    {
        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;


        /// <inheritdoc cref="System.Collections.Generic.List{T}.List"/>
        public JObservableList() : base()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.List(IEnumerable{T})"/>
        public JObservableList(IEnumerable<T> collection) : base(collection)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.List(int)"/>
        public JObservableList(int capacity) : base(capacity)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0. -or- index is equal to or greater than <see cref="System.Collections.Generic.List{T}.Count"/>.</exception>
        public new T this[int index]
        {
            get => base[index];
            set
            {
                bool exist = GetElement(index, out T? element);

                base[index] = value;

                if (exist)
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, element));
                }
            }
        }


        /// <inheritdoc cref="System.Collections.Generic.List{T}.Add(T)"/>
        public new void Add(T item)
        {
            base.Add(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count - 1));
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.AddRange(IEnumerable{T})"/>
        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.Clear"/>
        public new void Clear()
        {
            base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.Insert(int, T)"/>
        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.InsertRange(int, IEnumerable{T})"/>
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.Remove(T)"/>
        public new bool Remove(T item)
        {
            int index = FindIndexOf(item);

            bool result = base.Remove(item);

            if (result)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            }

            return result;
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.RemoveAll(Predicate{T})"/>
        public new int RemoveAll(Predicate<T> match)
        {
            int result = base.RemoveAll(match);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            return result;
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.RemoveAt(int)"/>
        public new void RemoveAt(int index)
        {
            bool exist = GetElement(index, out T? element);

            base.RemoveAt(index);

            if (exist)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, element, index));
            }
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.RemoveRange(int, int)"/>
        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.Reverse"/>
        public new void Reverse()
        {
            base.Reverse();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.Reverse(int, int)"/>
        public new void Reverse(int index, int count)
        {
            base.Reverse(index, count);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.Sort(Comparison{T})"/>
        public new void Sort(Comparison<T> comparison)
        {
            base.Sort(comparison);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.Sort(int, int, IComparer{T}?)"/>
        public new void Sort(int index, int count, IComparer<T>? comparer)
        {
            base.Sort(index, count, comparer);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.Sort"/>
        public new void Sort()
        {
            base.Sort();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.Sort(IComparer{T}?)"/>
        public new void Sort(IComparer<T>? comparer)
        {
            base.Sort(comparer);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <summary>
        /// Finds the index of the element in the list.
        /// </summary>
        /// <param name="element">Element to find in the list.</param>
        /// <returns>Returns the index of the element. If element could not be found in the list, returns -1.</returns>
        private int FindIndexOf(T element)
        {
            if (Count == 0)
                return -1;

            bool found = false;
            int index = 0;
            foreach (var item in this)
            {
                if (item != null && item.Equals(element))
                {
                    found = true;
                    break;
                }

                index++;
            }

            return found ? index : -1;
        }

        /// <summary>
        /// Gets the element at the index.
        /// </summary>
        /// <param name="index">The index of the element</param>
        /// <param name="element">The result</param>
        /// <returns>Returns true if the index is within the range and returns false if the index is out of the range.</returns>
        private bool GetElement(int index, out T? element)
        {
            if ((uint)index >= Count)
            {
                element = default;
                return false;
            }

            element = this[index];
            return true;
        }
    }
}
