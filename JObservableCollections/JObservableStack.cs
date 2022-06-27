// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;


namespace JUtility.JObservableCollections
{
    /// <summary>
    /// The observable version of <see cref="System.Collections.Generic.Stack{T}"/>.<para/>
    /// To be able to get notified when a property of an element in the <see cref="JObservableCollections.JObservableStack{T}"/> changes, the element must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the elements implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of the elements changes.
    /// If the elements inherit from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of the elements changes.
    /// </summary>
    /// <remarks>
    /// Use the stack and its elements to get notified. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MyStack}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// </remarks>
    /// <typeparam name="T">The type of elements in the stack.</typeparam>
    public class JObservableStack<T> : Stack<T>, INotifyCollectionChanged
    {
        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;


        /// <inheritdoc cref="System.Collections.Generic.Stack{T}.Stack"/>
        public JObservableStack() : base()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Stack{T}.Stack(IEnumerable{T})"/>
        public JObservableStack(IEnumerable<T> collection) : base(collection)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Stack{T}.Stack(int)"/>
        public JObservableStack(int capacity) : base(capacity)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        /// <inheritdoc cref="System.Collections.Generic.Stack{T}.Clear"/>
        public new void Clear()
        {
            base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc cref="System.Collections.Generic.Stack{T}.Pop"/>
        public new T Pop()
        {
            T item = base.Pop();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, 0));

            return item;
        }

        /// <inheritdoc cref="System.Collections.Generic.Stack{T}.Push(T)"/>
        public new void Push(T item)
        {
            base.Push(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, 0));
        }

        /// <inheritdoc cref="System.Collections.Generic.Stack{T}.TryPop(out T)"/>
        public new bool TryPop([MaybeNullWhen(false)] out T result)
        {
            bool boolResult = base.TryPop(out result);

            if (boolResult)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, result, 0));
            }

            return boolResult;
        }
    }
}
