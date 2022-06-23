using System.Collections.Generic;
using System.Collections.Specialized;


namespace JObservableCollections
{
    /// <summary>
    /// The observable version of <see cref="System.Collections.Generic.SortedList{TKey, TValue}"/>.<para/>
    /// To be able to get notified when a property of a <see cref="TValue"/> in the <see cref="JObservableCollections.JObservableSortedList{TKey, TValue}"/> changes, the <see cref="TValue"/> must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the <see cref="TValue"/> implements <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of <see cref="TValue"/> changes.
    /// If the <see cref="TValue"/> inherits from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of <see cref="TValue"/> changes.
    /// </summary>
    /// <remarks>
    /// Use the sorted list and its key-value pairs to get notified. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MySortedList}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding Value.MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding Value.MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// </remarks>
    /// <typeparam name="TKey">The type of the keys in the sorted list.</typeparam>
    /// <typeparam name="TValue">The type of the values in the sorted list.</typeparam>
    public class JObservableSortedList<TKey, TValue> : SortedList<TKey, TValue>, INotifyCollectionChanged where TKey : notnull
    {
        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;


        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.SortedList"/>
        public JObservableSortedList() : base()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.SortedList(IComparer{TKey}?)"/>
        public JObservableSortedList(IComparer<TKey>? comparer) : base(comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.SortedList(IDictionary{TKey, TValue})"/>
        public JObservableSortedList(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.SortedList(int)"/>
        public JObservableSortedList(int capacity) : base(capacity)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.SortedList(IDictionary{TKey, TValue}, IComparer{TKey}?)"/>
        public JObservableSortedList(IDictionary<TKey, TValue> dictionary, IComparer<TKey>? comparer) : base(dictionary, comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.SortedList(int, IComparer{TKey}?)"/>
        public JObservableSortedList(int capacity, IComparer<TKey>? comparer) : base(capacity, comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get or set.</param>
        /// <returns>The value associated with the specified key. If the specified key is not found,
        /// a get operation throws a System.Collections.Generic.KeyNotFoundException and
        /// a set operation creates a new element using the specified key.</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">The property is retrieved and key does not exist in the collection.</exception>
        public new TValue this[TKey key]
        {
            get => base[key];
            set
            {
                bool exist = TryGetValue(key, out TValue? oldValue);
                int index = exist ? IndexOfKey(key) : 0;

                base[key] = value;

                if (exist)
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, oldValue), index));
                }
            }
        }


        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.Add(TKey, TValue)"/>
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.Clear"/>
        public new void Clear()
        {
            base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.Remove(TKey)"/>
        public new bool Remove(TKey key)
        {
            bool exist = ContainsKey(key);
            int index = 0;
            TValue? value = default;
            if (exist)
            {
                index = IndexOfKey(key);
                value = this[key];
            }

            bool result = base.Remove(key);

            if (result)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value), index));
            }

            return result;
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.RemoveAt(int)"/>
        public new void RemoveAt(int index)
        {
            GetKeyValuePair(out KeyValuePair<TKey, TValue>? result, index);

            base.RemoveAt(index);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, result, index));
        }


        /// <summary>
        /// Gets the copy of the <see cref="KeyValuePair{TKey, TValue}"/> at the index.
        /// </summary>
        /// <param name="result">The copy of the <see cref="KeyValuePair{TKey, TValue}"/> at the index.</param>
        /// <param name="index">The index of the <see cref="KeyValuePair{TKey, TValue}"/>.</param>
        private void GetKeyValuePair(out KeyValuePair<TKey, TValue>? result, int index)
        {
            int currentIndex = 0;
            foreach (var item in this)
            {
                if (index == currentIndex)
                {
                    result = new KeyValuePair<TKey, TValue>(item.Key, item.Value);
                    return;
                }

                currentIndex++;
            }

            result = null;
        }
    }
}
