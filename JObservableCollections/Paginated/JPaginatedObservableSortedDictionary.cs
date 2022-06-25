namespace JObservableCollections.Paginated
{
    /// <summary>
    /// The paginated version of <see cref="JObservableCollections.JObservableSortedDictionary{TKey, TValue}"/>.<para/>
    /// To be able to get notified when a property of a <see cref="TValue"/> in the <see cref="JObservableCollections.Paginated.JPaginatedObservableSortedDictionary{TKey, TValue}"/> changes, the <see cref="TValue"/> must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the <see cref="TValue"/> implements <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of <see cref="TValue"/> changes.
    /// If the <see cref="TValue"/> inherits from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of <see cref="TValue"/> changes.
    /// </summary>
    /// <remarks>
    /// In order to modify the sorted dictionary, use <see cref="FullSortedDictionary"/> property and in order to get the paginated version of the sorted dictionary, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PaginatedCollection"/> property. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MySortedDictionary.PaginatedCollection}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding Value.MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding Value.MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/> and <see cref="JObservableCollections.Paginated.JPaginationBase{T}.NumofPages"/> properties are set automaticly when something changes in the sorted dictionary. They both become 0 when
    /// there is no <see cref="System.Collections.Generic.KeyValuePair{TKey, TValue}"/> in the sorted dictionary. In order to change the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.ChangePage(int)"/> method and
    /// in order to set the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PageSize"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.SetPageSize(int)"/> method.
    /// </remarks>
    /// <typeparam name="TKey">The type of the keys in the sorted dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the sorted dictionary.</typeparam>
    public class JPaginatedObservableSortedDictionary<TKey, TValue> : JPaginationBase<KeyValuePair<TKey, TValue>> where TKey : notnull
    {
        /// <summary>
        /// The full sorted dictionary without the pagination
        /// </summary>
        public JObservableSortedDictionary<TKey, TValue> FullSortedDictionary { get; private set; } = null!;


        /// <inheritdoc cref="System.Collections.Generic.SortedDictionary{TKey, TValue}.SortedDictionary"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedDictionary(int pageSize) : base(pageSize)
        {
            FullSortedDictionary = new JObservableSortedDictionary<TKey, TValue>();
            FullSortedDictionary.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullSortedDictionary);
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedDictionary{TKey, TValue}.SortedDictionary(IComparer{TKey}?)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedDictionary(int pageSize, IComparer<TKey>? comparer) : base(pageSize)
        {
            FullSortedDictionary = new JObservableSortedDictionary<TKey, TValue>(comparer);
            FullSortedDictionary.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullSortedDictionary);
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedDictionary{TKey, TValue}.SortedDictionary(IDictionary{TKey, TValue})"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedDictionary(int pageSize, IDictionary<TKey, TValue> dictionary) : base(pageSize)
        {
            FullSortedDictionary = new JObservableSortedDictionary<TKey, TValue>(dictionary);
            FullSortedDictionary.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullSortedDictionary);
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedDictionary{TKey, TValue}.SortedDictionary(IDictionary{TKey, TValue}, IComparer{TKey}?)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedDictionary(int pageSize, IDictionary<TKey, TValue> dictionary, IComparer<TKey>? comparer) : base(pageSize)
        {
            FullSortedDictionary = new JObservableSortedDictionary<TKey, TValue>(dictionary, comparer);
            FullSortedDictionary.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullSortedDictionary);
        }
    }
}
