// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


namespace JObservableCollections.Paginated
{
    /// <summary>
    /// The paginated version of <see cref="JObservableCollections.JObservableLinkedList{T}"/>.<para/>
    /// To be able to get notified when a property of <see cref="System.Collections.Generic.LinkedListNode{T}.Value"/> in the <see cref="JObservableCollections.Paginated.JPaginatedObservableLinkedList{T}"/> changes, the <see cref="System.Collections.Generic.LinkedListNode{T}.Value"/> must either 
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the <see cref="System.Collections.Generic.LinkedListNode{T}.Value"/> implements <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of <see cref="System.Collections.Generic.LinkedListNode{T}.Value"/> changes.
    /// If the <see cref="System.Collections.Generic.LinkedListNode{T}.Value"/> inherits from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of <see cref="System.Collections.Generic.LinkedListNode{T}.Value"/> changes.
    /// </summary>
    /// <remarks>
    /// This data structure also includes accessing nodes by an index, whose O(n) = n.<para/>
    /// When you need to replace a value inside of a node, use <see cref="JObservableCollections.JObservableLinkedList{T}.Replace(LinkedListNode{T}, T)"/>, whose O(n) is 1, or <see cref="JObservableCollections.JObservableLinkedList{T}.Replace(int, T)"/>, whose O(n) is n, in order to get notified.<para/>
    /// In order to modify the linked list, use <see cref="FullLinkedList"/> property and in order to get the paginated version of the linked list, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PaginatedCollection"/> property. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MyLinkedList.PaginatedCollection}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/> and <see cref="JObservableCollections.Paginated.JPaginationBase{T}.NumofPages"/> properties are set automaticly when something changes in the linked list. They both become 0 when there is no <see cref="System.Collections.Generic.LinkedListNode{T}"/> in the linked list.
    /// In order to change the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.ChangePage(int)"/> method and in order to set the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PageSize"/>,
    /// use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.SetPageSize(int)"/> method.<para/>
    /// If an index is needed to access or change a node, the absolute version of the index must be used. E.g. using 5th index on the 2nd page results in changing the 5th node from the beginning in the <see cref="FullLinkedList"/> property. In order to use the
    /// absolute index, calculate the absolute index manually or use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.GetAbsoluteIndex(int)"/> method.
    /// The method takes the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/> property into account.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the linked list.</typeparam>
    public class JPaginatedObservableLinkedList<T> : JPaginationBase<T>
    {
        /// <summary>
        /// The full linked list without the pagination
        /// </summary>
        public JObservableLinkedList<T> FullLinkedList { get; private set; } = null!;


        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.LinkedList"/>
        /// <param name="pageSize">The initial size of the pages in the linked list.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableLinkedList(int pageSize) : base(pageSize)
        {
            FullLinkedList = new JObservableLinkedList<T>();
            FullLinkedList.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullLinkedList);
        }

        /// <inheritdoc cref="System.Collections.Generic.LinkedList{T}.LinkedList(IEnumerable{T})"/>
        /// <param name="pageSize">The initial size of the pages in the linked list.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableLinkedList(int pageSize, IEnumerable<T> collection) : base(pageSize)
        {
            FullLinkedList = new JObservableLinkedList<T>(collection);
            FullLinkedList.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullLinkedList);
        }
    }
}
