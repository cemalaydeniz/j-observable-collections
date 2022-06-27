// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


namespace JUtility.JObservableCollections.Paginated
{
    /// <summary>
    /// The paginated version of <see cref="JObservableCollections.JObservableList{T}"/>.<para/>
    /// To be able to get notified when a property of an element in the <see cref="JObservableCollections.Paginated.JPaginatedObservableList{T}"/> changes, the element must either 
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the elements implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of the elements changes.
    /// If the elements inherit from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of the elements changes.
    /// </summary>
    /// <remarks>
    /// In order to modify the list, use <see cref="FullList"/> property and in order to get the paginated version of the list, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PaginatedCollection"/> property. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MyList.PaginatedCollection}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/> and <see cref="JObservableCollections.Paginated.JPaginationBase{T}.NumofPages"/> properties are set automaticly when something changes in the list.
    /// They both become 0 when there is no element in the list. In order to change the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.ChangePage(int)"/> method and
    /// in order to set the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PageSize"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.SetPageSize(int)"/> method.<para/> 
    /// If an index is needed to access or change an element, the absolute version of the index must be used. E.g. using 5th index on the 2nd page results in changing the 5th element from the beginning in the <see cref="FullList"/> property. In order to use the
    /// absolute index, calculate the absolute index manually or use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.GetAbsoluteIndex(int)"/> method.
    /// The method takes the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/> property into account.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class JPaginatedObservableList<T> : JPaginationBase<T>
    {
        /// <summary>
        /// The full list without the pagination
        /// </summary>
        public JObservableList<T> FullList { get; private set; } = null!;


        /// <inheritdoc cref="System.Collections.Generic.List{T}.List"/>
        /// <param name="pageSize">The initial size of the pages in the list.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableList(int pageSize) : base(pageSize)
        {
            FullList = new JObservableList<T>();
            FullList.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullList);
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.List(IEnumerable{T})"/>
        /// <param name="pageSize">The initial size of the pages in the list.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableList(int pageSize, IEnumerable<T> collection) : base(pageSize)
        {
            FullList = new JObservableList<T>(collection);
            FullList.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullList);
        }

        /// <inheritdoc cref="System.Collections.Generic.List{T}.List(int)"/>
        /// <param name="pageSize">The initial size of the pages in the list.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableList(int pageSize, int capacity) : base(pageSize)
        {
            FullList = new JObservableList<T>(capacity);
            FullList.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullList);
        }
    }
}
