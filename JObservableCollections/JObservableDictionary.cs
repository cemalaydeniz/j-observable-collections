using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;


namespace JObservableCollections
{
    /// <summary>
    /// The observable version of <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>.<para/>
    /// To be able to get notified when a property of a <see cref="TValue"/> in the <see cref="JObservableCollections.JObservableDictionary{TKey, TValue}"/> changes, the <see cref="TValue"/> must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the <see cref="TValue"/> implements <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of <see cref="TValue"/> changes.
    /// If the <see cref="TValue"/> inherits from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of <see cref="TValue"/> changes.
    /// </summary>
    /// <remarks>
    /// Use the dictionary and its key-value pairs to get notified. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MyDictionary}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding Value.MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding Value.MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// </remarks>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class JObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged where TKey : notnull
    {
        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;


        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary"/>
        public JObservableDictionary() : base()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(IDictionary{TKey, TValue})"/>
        public JObservableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(IEnumerable{KeyValuePair{TKey, TValue}})"/>
        public JObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(IEqualityComparer{TKey}?)"/>
        public JObservableDictionary(IEqualityComparer<TKey>? comparer) : base(comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(int)"/>
        public JObservableDictionary(int capacity) : base(capacity)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(IDictionary{TKey, TValue}, IEqualityComparer{TKey}?)"/>
        public JObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer) : base(dictionary, comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(IEnumerable{KeyValuePair{TKey, TValue}}, IEqualityComparer{TKey}?)"/>
        public JObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer) : base(collection, comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(int, IEqualityComparer{TKey}?)"/>
        public JObservableDictionary(int capacity, IEqualityComparer<TKey>? comparer) : base(capacity, comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(SerializationInfo, StreamingContext)"/>
        protected JObservableDictionary(SerializationInfo info, StreamingContext context)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a <see cref="System.Collections.Generic.KeyNotFoundException"/>,
        /// and a set operation creates a new element with the specified key.</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">The property is retrieved and key does not exist in the collection.</exception>
        public new TValue this[TKey key]
        {
            get => base[key];
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


        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Add(TKey, TValue)"/>
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Clear"/>
        public new void Clear()
        {
            base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Remove(TKey, out TValue)"/>
        public new bool Remove(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            int index = FindIndexOf(key);
            
            bool result = base.Remove(key, out value);

            if (result)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value), index));
            }

            return result;
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Remove(TKey)"/>
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

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.TryAdd(TKey, TValue)"/>
        public new bool TryAdd(TKey key, TValue value)
        {
            bool result = base.TryAdd(key, value);

            if (result)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            return result;
        }


        /// <summary>
        /// Finds the index of the key in the dictionary.
        /// </summary>
        /// <param name="key">The key in the dictionary.</param>
        /// <returns>Returns the index of the key. If the key could not be found in the dictionary, returns -1.</returns>
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
