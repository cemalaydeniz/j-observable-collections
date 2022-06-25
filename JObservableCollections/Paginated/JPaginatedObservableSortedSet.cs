namespace JObservableCollections.Paginated
{
    /// <summary>
    /// The paginated version of <see cref="JObservableCollections.JObservableSortedSet{T}"/>.<para/>
    /// To be able to get notified when a property of an element in the <see cref="JObservableCollections.Paginated.JPaginatedObservableSortedSet{T}"/> changes, the element must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the elements implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of the elements changes.
    /// If the elements inherit from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of the elements changes.
    /// </summary>
    /// <remarks>
    /// In order to modify the sorted set, use <see cref="FullSortedSet"/> property and in order to get the paginated version of the sorted set, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PaginatedCollection"/> property. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MySortedSet.PaginatedCollection}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/> and <see cref="JObservableCollections.Paginated.JPaginationBase{T}.NumofPages"/> properties are set automaticly when something changes in the sorted set.
    /// They both become 0 when there is no element in the hash set. In order to change the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.ChangePage(int)"/> method and
    /// in order to set the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PageSize"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.SetPageSize(int)"/> method.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the sorted set.</typeparam>
    public class JPaginatedObservableSortedSet<T> : JPaginationBase<T>
    {
        /// <summary>
        /// The full sorted set without the pagination
        /// </summary>
        public JObservableSortedSet<T> FullSortedSet { get; private set; } = null!;


        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.SortedSet"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedSet(int pageSize) : base(pageSize)
        {
            FullSortedSet = new JObservableSortedSet<T>();
            FullSortedSet.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullSortedSet);
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.SortedSet(IComparer{T}?)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedSet(int pageSize, IComparer<T>? comparer) : base(pageSize)
        {
            FullSortedSet = new JObservableSortedSet<T>(comparer);
            FullSortedSet.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullSortedSet);
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.SortedSet(IEnumerable{T})"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedSet(int pageSize, IEnumerable<T> collection) : base(pageSize)
        {
            FullSortedSet = new JObservableSortedSet<T>(collection);
            FullSortedSet.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullSortedSet);
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedSet{T}.SortedSet(IEnumerable{T}, IComparer{T}?)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedSet(int pageSize, IEnumerable<T> collection, IComparer<T>? comparer) : base(pageSize)
        {
            FullSortedSet = new JObservableSortedSet<T>(collection, comparer);
            FullSortedSet.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullSortedSet);
        }
    }
}
