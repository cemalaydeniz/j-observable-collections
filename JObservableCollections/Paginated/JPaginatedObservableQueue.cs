// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


namespace JObservableCollections.Paginated
{
    /// <summary>
    /// The paginated version of <see cref="JObservableCollections.JObservableQueue{T}"/>.<para/>
    /// To be able to get notified when a property of an element in the <see cref="JObservableCollections.Paginated.JPaginatedObservableQueue{T}"/> changes, the element must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the elements implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of the elements changes.
    /// If the elements inherit from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of the elements changes.
    /// </summary>
    /// <remarks>
    /// In order to modify the queue, use <see cref="FullQueue"/> property and in order to get the paginated version of the queue, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PaginatedCollection"/> property. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MyQueue.PaginatedCollection}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/> and <see cref="JObservableCollections.Paginated.JPaginationBase{T}.NumofPages"/> properties are set automaticly when something changes in the queue.
    /// They both become 0 when there is no element in the queue. In order to change the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.ChangePage(int)"/> method and
    /// in order to set the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PageSize"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.SetPageSize(int)"/> method.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    public class JPaginatedObservableQueue<T> : JPaginationBase<T>
    {
        /// <summary>
        /// The full queue without the pagination
        /// </summary>
        public JObservableQueue<T> FullQueue { get; private set; } = null!;


        /// <inheritdoc cref="System.Collections.Generic.Queue{T}.Queue"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableQueue(int pageSize) : base(pageSize)
        {
            FullQueue = new JObservableQueue<T>();
            FullQueue.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullQueue);
        }

        /// <inheritdoc cref="System.Collections.Generic.Queue{T}.Queue(IEnumerable{T})"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableQueue(int pageSize, IEnumerable<T> collection) : base(pageSize)
        {
            FullQueue = new JObservableQueue<T>(collection);
            FullQueue.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullQueue);
        }

        /// <inheritdoc cref="System.Collections.Generic.Queue{T}.Queue(int)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableQueue(int pageSize, int capacity) : base(pageSize)
        {
            FullQueue = new JObservableQueue<T>(capacity);
            FullQueue.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullQueue);
        }
    }
}
