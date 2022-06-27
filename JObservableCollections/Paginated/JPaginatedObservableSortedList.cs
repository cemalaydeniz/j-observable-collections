// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


namespace JUtility.JObservableCollections.Paginated
{
    /// <summary>
    /// The paginated version of <see cref="JObservableCollections.JObservableSortedList{TKey, TValue}"/>.<para/>
    /// To be able to get notified when a property of a <see cref="TValue"/> in the <see cref="JObservableCollections.Paginated.JPaginatedObservableSortedList{TKey, TValue}"/> changes, the <see cref="TValue"/> must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the <see cref="TValue"/> implements <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of <see cref="TValue"/> changes.
    /// If the <see cref="TValue"/> inherits from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of <see cref="TValue"/> changes.
    /// </summary>
    /// <remarks>
    /// In order to modify the sorted list, use <see cref="FullSortedList"/> property and in order to get the paginated version of the sorted list, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PaginatedCollection"/> property. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MySortedList.PaginatedCollection}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding Value.MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding Value.MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/> and <see cref="JObservableCollections.Paginated.JPaginationBase{T}.NumofPages"/> properties are set automaticly when something changes in the sorted list.
    /// They both become 0 when there is no element in the sorted list. In order to change the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.ChangePage(int)"/> method and
    /// in order to set the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PageSize"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.SetPageSize(int)"/> method.
    /// </remarks>
    /// <typeparam name="TKey">The type of the keys in the sorted list.</typeparam>
    /// <typeparam name="TValue">The type of the values in the sorted list.</typeparam>
    public class JPaginatedObservableSortedList<TKey, TValue> : JPaginationBase<KeyValuePair<TKey, TValue>> where TKey : notnull
    {
        /// <summary>
        /// The full sorted list without the pagination
        /// </summary>
        public JObservableSortedList<TKey, TValue> FullSortedList { get; private set; } = null!;


        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.SortedList"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedList(int pageSize) : base(pageSize)
        {
            FullSortedList = new JObservableSortedList<TKey, TValue>();
            FullSortedList.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullSortedList);
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.SortedList(IComparer{TKey}?)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedList(int pageSize, IComparer<TKey>? comparer) : base(pageSize)
        {
            FullSortedList = new JObservableSortedList<TKey, TValue>(comparer);
            FullSortedList.CollectionChanged += OnCollectionChanged;
            
            SetFullCollection(FullSortedList);
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.SortedList(IDictionary{TKey, TValue})"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedList(int pageSize, IDictionary<TKey, TValue> dictionary) : base(pageSize)
        {
            FullSortedList = new JObservableSortedList<TKey, TValue>(dictionary);
            FullSortedList.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullSortedList);
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.SortedList(int)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedList(int pageSize, int capacity) : base(pageSize)
        {
            FullSortedList = new JObservableSortedList<TKey, TValue>(capacity);
            FullSortedList.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullSortedList);
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.SortedList(IDictionary{TKey, TValue}, IComparer{TKey}?)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedList(int pageSize, IDictionary<TKey, TValue> dictionary, IComparer<TKey>? comparer) : base(pageSize)
        {
            FullSortedList = new JObservableSortedList<TKey, TValue>(dictionary, comparer);
            FullSortedList.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullSortedList);
        }

        /// <inheritdoc cref="System.Collections.Generic.SortedList{TKey, TValue}.SortedList(int, IComparer{TKey}?)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableSortedList(int pageSize, int capacity, IComparer<TKey>? comparer) : base(pageSize)
        {
            FullSortedList = new JObservableSortedList<TKey, TValue>(capacity, comparer);
            FullSortedList.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullSortedList);
        }
    }
}
