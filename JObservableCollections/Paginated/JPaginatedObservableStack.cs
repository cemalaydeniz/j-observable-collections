// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


namespace JUtility.JObservableCollections.Paginated
{
    /// <summary>
    /// The paginated version of <see cref="JObservableCollections.JObservableStack{T}"/>.<para/>
    /// To be able to get notified when a property of an element in the <see cref="JObservableCollections.Paginated.JPaginatedObservableStack{T}"/> changes, the element must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the elements implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of the elements changes.
    /// If the elements inherit from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of the elements changes.
    /// </summary>
    /// <remarks>
    /// In order to modify the stack, use <see cref="FullStack"/> property and in order to get the paginated version of the stack, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PaginatedCollection"/> property. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MyStack.PaginatedCollection}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/> and <see cref="JObservableCollections.Paginated.JPaginationBase{T}.NumofPages"/> properties are set automaticly when something changes in the stack.
    /// They both become 0 when there is no element in the stack. In order to change the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.ChangePage(int)"/> method and
    /// in order to set the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PageSize"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.SetPageSize(int)"/> method.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the stack.</typeparam>
    public class JPaginatedObservableStack<T> : JPaginationBase<T>
    {
        /// <summary>
        /// The full stack without the pagination
        /// </summary>
        public JObservableStack<T> FullStack { get; private set; } = null!;


        /// <inheritdoc cref="System.Collections.Generic.Stack{T}.Stack"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableStack(int pageSize) : base(pageSize)
        {
            FullStack = new JObservableStack<T>();
            FullStack.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullStack);
        }

        /// <inheritdoc cref="System.Collections.Generic.Stack{T}.Stack(IEnumerable{T})"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableStack(int pageSize, IEnumerable<T> collection) : base(pageSize)
        {
            FullStack = new JObservableStack<T>(collection);
            FullStack.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullStack);
        }

        /// <inheritdoc cref="System.Collections.Generic.Stack{T}.Stack(int)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableStack(int pageSize, int capacity) : base(pageSize)
        {
            FullStack = new JObservableStack<T>(capacity);
            FullStack.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullStack);
        }
    }
}
