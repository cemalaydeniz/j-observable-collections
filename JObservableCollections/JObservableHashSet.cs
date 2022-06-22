using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;


namespace JObservableCollections
{
    /// <summary>
    /// The observable version of <see cref="System.Collections.Generic.HashSet{T}"/>.
    /// To be able to get notified when a property of an element in the <see cref="JObservableCollections.JObservableHashSet{T}"/> changes, the element must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.
    /// If the elements implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of the elements changes.
    /// If the elements inherit from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of the elements changes.
    /// </summary>
    /// <typeparam name="T">The type of elements in the hash set.</typeparam>
    public class JObservableHashSet<T> : HashSet<T>, INotifyCollectionChanged
    {
        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;


        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.HashSet"/>
        public JObservableHashSet() : base()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.HashSet(IEnumerable{T})"/>
        public JObservableHashSet(IEnumerable<T> collection) : base(collection)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.HashSet(IEqualityComparer{T}?)"/>
        public JObservableHashSet(IEqualityComparer<T>? comparer) : base(comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.HashSet(int)"/>
        public JObservableHashSet(int capacity) : base(capacity)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.HashSet(IEnumerable{T}, IEqualityComparer{T}?)"/>
        public JObservableHashSet(IEnumerable<T> collection, IEqualityComparer<T>? comparer) : base(collection, comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.HashSet(int, IEqualityComparer{T}?)"/>
        public JObservableHashSet(int capacity, IEqualityComparer<T>? comparer) : base(capacity, comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.HashSet(SerializationInfo, StreamingContext)"/>
        protected JObservableHashSet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.Add(T)"/>
        public new bool Add(T item)
        {
            bool result = base.Add(item);

            if (result)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            return result;
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.Clear"/>
        public new void Clear()
        {
            base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.ExceptWith(IEnumerable{T})"/>
        public new void ExceptWith(IEnumerable<T> other)
        {
            base.ExceptWith(other);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.IntersectWith(IEnumerable{T})"/>
        public new void IntersectWith(IEnumerable<T> other)
        {
            base.IntersectWith(other);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.Remove(T)"/>
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

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.RemoveWhere(Predicate{T})"/>
        public new int RemoveWhere(Predicate<T> match)
        {
            int result = base.RemoveWhere(match);

            if (result > 0)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            return result;
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.SymmetricExceptWith(IEnumerable{T})"/>
        public new void SymmetricExceptWith(IEnumerable<T> other)
        {
            base.SymmetricExceptWith(other);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.UnionWith(IEnumerable{T})"/>
        public new void UnionWith(IEnumerable<T> other)
        {
            base.UnionWith(other);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <summary>
        /// Finds the index of the element in the hash set.
        /// </summary>
        /// <param name="element">Element in the hash set.</param>
        /// <returns>Returns the index of the element. If element could not be found in the hast set, returns -1.</returns>
        private int FindIndexOf(T element)
        {
            if (Count == 0)
                return -1;

            bool found = false;
            int index = 0;
            foreach (var setItem in (IEnumerable<T>)this)
            {
                if (setItem != null && setItem.Equals(element))
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
