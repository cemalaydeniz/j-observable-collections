// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


using System.Collections.Specialized;
using System.Runtime.Serialization;


namespace JUtility.JObservableCollections
{
    /// <summary>
    /// The observable version of <see cref="System.Collections.Generic.SortedSet{T}"/>.<para/>
    /// To be able to get notified when a property of an element in the <see cref="JObservableCollections.JObservableSortedSet{T}"/> changes, the element must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the elements implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of the elements changes.
    /// If the elements inherit from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of the elements changes.
    /// </summary>
    /// <remarks>
    /// Use the sorted set and its elements to get notified. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MySortedSet}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// </remarks>
    /// <typeparam name="T">The type of elements in the sorted set.</typeparam>
    public class JObservableSortedSet<T> : SortedSet<T>, INotifyCollectionChanged
    {
        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;


        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.SortedSet"/>
        public JObservableSortedSet() : base()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.SortedSet(IComparer{T}?)"/>
        public JObservableSortedSet(IComparer<T>? comparer) : base(comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.SortedSet(IEnumerable{T})"/>
        public JObservableSortedSet(IEnumerable<T> collection) : base(collection)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.SortedSet(IEnumerable{T}, IComparer{T}?)"/>
        public JObservableSortedSet(IEnumerable<T> collection, IComparer<T>? comparer) : base(collection, comparer)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.SortedSet(SerializationInfo, StreamingContext)"/>
        protected JObservableSortedSet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.Add(T)"/>
        public new bool Add(T item)
        {
            bool result = base.Add(item);

            if (result)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            return result;
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.Clear"/>
        public new virtual void Clear()
        {
            base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.ExceptWith(IEnumerable{T})"/>
        public new void ExceptWith(IEnumerable<T> other)
        {
            base.ExceptWith(other);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.IntersectWith(IEnumerable{T})"/>
        public new virtual void IntersectWith(IEnumerable<T> other)
        {
            base.IntersectWith(other);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.Remove(T)"/>
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

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.RemoveWhere(Predicate{T})"/>
        public new int RemoveWhere(Predicate<T> match)
        {
            int result = base.RemoveWhere(match);

            if (result > 0)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            return result;
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.SymmetricExceptWith(IEnumerable{T})"/>
        public new void SymmetricExceptWith(IEnumerable<T> other)
        {
            base.SymmetricExceptWith(other);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.UnionWith(IEnumerable{T})"/>
        public new void UnionWith(IEnumerable<T> other)
        {
            base.UnionWith(other);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <summary>
        /// Finds the index of the element in the sorted set.
        /// </summary>
        /// <param name="element">Element in the sorted set.</param>
        /// <returns>Returns the index of the element. If element could not be found in the sorted set, returns -1.</returns>
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
