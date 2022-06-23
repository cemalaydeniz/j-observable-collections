using System.Collections.Generic;
using System.Collections.Specialized;


namespace JObservableCollections
{
    /// <summary>
    /// The observable version of <see cref="System.Collections.Generic.SortedDictionary{TKey, TValue}"/>.<para/>
    /// To be able to get notified when a property of a <see cref="TValue"/> in the <see cref="JObservableCollections.JObservableSortedDictionary{TKey, TValue}"/> changes, the <see cref="TValue"/> must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the <see cref="TValue"/> implements <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of <see cref="TValue"/> changes.
    /// If the <see cref="TValue"/> inherits from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of <see cref="TValue"/> changes.
    /// </summary>
    /// <remarks>
    /// Use the sorted dictionary and its key-value pairs to get notified. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MySortedDictionary}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding Value.MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding Value.MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// </remarks>
    /// <typeparam name="TKey">The type of the keys in the sorted dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the sorted dictionary.</typeparam>
    public class JObservableSortedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>, INotifyCollectionChanged where TKey : notnull
    {
        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;


        /// <inheritdoc cref="System.Collections.Generic.SortedDictionary{TKey, TValue}.SortedDictionary"/>
        public JObservableSortedDictionary() : base()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedDictionary{TKey, TValue}.SortedDictionary(IComparer{TKey}?)"/>
        public JObservableSortedDictionary(IComparer<TKey>? comparer) : base(comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedDictionary{TKey, TValue}.SortedDictionary(IDictionary{TKey, TValue})"/>
        public JObservableSortedDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedDictionary{TKey, TValue}.SortedDictionary(IDictionary{TKey, TValue}, IComparer{TKey}?)"/>
        public JObservableSortedDictionary(IDictionary<TKey, TValue> dictionary, IComparer<TKey>? comparer) : base(dictionary, comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key. If the specified key is not found,
        /// a get operation throws a System.Collections.Generic.KeyNotFoundException, and
        /// a set operation creates a new element with the specified key.</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">The property is retrieved and key does not exist in the collection.</exception>
        public new TValue this[TKey key]
        {
            get => this[key];
            set
            {
                bool exist = TryGetValue(key, out TValue? oldValue);
                int index = exist ? FindIndexOf(key) : 0;

                base[key] = value;

                if (exist)
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, oldValue), index));
                }
            }
        }


        /// <inheritdoc cref="System.Collections.Generic.SortedDictionary{TKey, TValue}.Add(TKey, TValue)"/>
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedDictionary{TKey, TValue}.Clear"/>
        public new void Clear()
        {
            base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedDictionary{TKey, TValue}.Remove(TKey)"/>
        public new bool Remove(TKey key)
        {
            bool exist = TryGetValue(key, out TValue? value);
            int index = exist ? FindIndexOf(key) : 0;

            bool result = base.Remove(key);

            if (result)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value), index));
            }

            return result;
        }


        /// <summary>
        /// Finds the index of the key in the sorted dictionary.
        /// </summary>
        /// <param name="key">The key in the sorted dictionary.</param>
        /// <returns>Returns the index of the key. If the key could not be found in the sorted dictionary, returns -1.</returns>
        private int FindIndexOf(TKey key)
        {
            if (Count == 0)
                return -1;

            bool found = false;
            int index = 0;
            foreach (var item in this)
            {
                if (item.Key.Equals(key))
                {
                    found = true;
                    break;
                }

                index++;
            }

            return found ? index : -1;
        }
    }
}
