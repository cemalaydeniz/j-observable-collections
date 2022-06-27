// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


namespace JUtility.JObservableCollections.Paginated
{
    /// <summary>
    /// The paginated version of <see cref="JObservableCollections.JObservableHashSet{T}"/>.<para/>
    /// To be able to get notified when a property of an element in the <see cref="JObservableCollections.Paginated.JPaginatedObservableHashSet{T}"/> changes, the element must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the elements implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of the elements changes.
    /// If the elements inherit from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of the elements changes.
    /// </summary>
    /// <remarks>
    /// In order to modify the hash set, use <see cref="FullHashSet"/> property and in order to get the paginated version of the hash set, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PaginatedCollection"/> property. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MyHashSet.PaginatedCollection}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/> and <see cref="JObservableCollections.Paginated.JPaginationBase{T}.NumofPages"/> properties are set automaticly when something changes in the hash set.
    /// They both become 0 when there is no element in the hash set. In order to change the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.ChangePage(int)"/> method and
    /// in order to set the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PageSize"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.SetPageSize(int)"/> method.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the hash set.</typeparam>
    public class JPaginatedObservableHashSet<T> : JPaginationBase<T>
    {
        /// <summary>
        /// The full hash set without the pagination
        /// </summary>
        public JObservableHashSet<T> FullHashSet { get; private set; } = null!;


        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.HashSet"/>
        /// <param name="pageSize">The initial size of the pages in the hash set.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableHashSet(int pageSize) : base(pageSize)
        {
            FullHashSet = new JObservableHashSet<T>();
            FullHashSet.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullHashSet);
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.HashSet(IEnumerable{T})"/>
        /// <param name="pageSize">The initial size of the pages in the hash set.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableHashSet(int pageSize, IEnumerable<T> collection) : base(pageSize)
        {
            FullHashSet = new JObservableHashSet<T>(collection);
            FullHashSet.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullHashSet);
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.HashSet(IEqualityComparer{T}?)"/>
        /// <param name="pageSize">The initial size of the pages in the hash set.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableHashSet(int pageSize, IEqualityComparer<T>? comparer) : base(pageSize)
        {
            FullHashSet = new JObservableHashSet<T>(comparer);
            FullHashSet.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullHashSet);
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.HashSet(int)"/>
        /// <param name="pageSize">The initial size of the pages in the hash set.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableHashSet(int pageSize, int capacity) : base(pageSize)
        {
            FullHashSet = new JObservableHashSet<T>(capacity);
            FullHashSet.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullHashSet);
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.HashSet(IEnumerable{T}, IEqualityComparer{T}?)"/>
        /// <param name="pageSize">The initial size of the pages in the hash set.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableHashSet(int pageSize, IEnumerable<T> collection, IEqualityComparer<T>? comparer) : base(pageSize)
        {
            FullHashSet = new JObservableHashSet<T>(collection, comparer);
            FullHashSet.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullHashSet);
        }

        /// <inheritdoc cref="System.Collections.Generic.HashSet{T}.HashSet(int, IEqualityComparer{T}?)"/>
        /// <param name="pageSize">The initial size of the pages in the hash set.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableHashSet(int pageSize, int capacity, IEqualityComparer<T>? comparer) : base(pageSize)
        {
            FullHashSet = new JObservableHashSet<T>(capacity, comparer);
            FullHashSet.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullHashSet);
        }
    }
}
