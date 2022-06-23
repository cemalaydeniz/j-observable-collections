using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;


namespace JObservableCollections
{
    /// <summary>
    /// The observable version of <see cref="System.Collections.Generic.Queue{T}"/>.<para/>
    /// To be able to get notified when a property of an element in the <see cref="JObservableCollections.JObservableQueue{T}"/> changes, the element must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the elements implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of the elements changes.
    /// If the elements inherit from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of the elements changes.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    public class JObservableQueue<T> : Queue<T>, INotifyCollectionChanged
    {
        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;


        /// <inheritdoc cref="System.Collections.Generic.Queue{T}.Queue"/>
        public JObservableQueue() : base()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Queue{T}.Queue(IEnumerable{T})"/>
        public JObservableQueue(IEnumerable<T> collection) : base(collection)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Queue{T}.Queue(int)"/>
        public JObservableQueue(int capacity) : base(capacity)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <inheritdoc cref="System.Collections.Generic.Queue{T}.Clear"/>
        public new void Clear()
        {
            base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Queue{T}.Dequeue"/>
        public new T Dequeue()
        {
            T item = base.Dequeue();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, 0));

            return item;
        }

        /// <inheritdoc cref="System.Collections.Generic.Queue{T}.Enqueue(T)"/>
        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        /// <inheritdoc cref="System.Collections.Generic.Queue{T}.TryDequeue(out T)"/>
        public new bool TryDequeue([MaybeNullWhen(false)] out T result)
        {
            bool boolResult = base.TryDequeue(out result);

            if (boolResult)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, result, 0));
            }

            return boolResult;
        }
    }
}
