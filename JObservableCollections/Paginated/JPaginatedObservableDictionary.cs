// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


namespace JUtility.JObservableCollections.Paginated
{
    /// <summary>
    /// The paginated version of <see cref="JObservableCollections.JObservableDictionary{TKey, TValue}"/>.<para/>
    /// To be able to get notified when a property of a <see cref="TValue"/> in the <see cref="JObservableCollections.Paginated.JPaginatedObservableDictionary{TKey, TValue}"/> changes, the <see cref="TValue"/> must either
    /// implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> or inherit from <see cref="JObservableCollections.Helper.JObservableItem"/>.<para/>
    /// If the <see cref="TValue"/> implements <see cref="System.ComponentModel.INotifyPropertyChanged"/> then it should raise the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event when a property of <see cref="TValue"/> changes.
    /// If the <see cref="TValue"/> inherits from <see cref="JObservableCollections.Helper.JObservableItem"/> then it should call the <see cref="JObservableCollections.Helper.JObservableItem.OnPropertyChanged(string?)"/> method when a property of <see cref="TValue"/> changes.
    /// </summary>
    /// <remarks>
    /// In order to modify the dictionary, use <see cref="FullDictionary"/> property and in order to get the paginated version of the dictionary, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PaginatedCollection"/> property. Example XAML code:
    /// <code>
    /// &lt;ListView ItemsSource="{Binding MyDictionary.PaginatedCollection}"&gt;
    ///     &lt;ListView.View&gt;
    ///         &lt;GridView&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding Value.MyProperty1}"/&gt;
    ///             &lt;GridViewColumn DisplayMemberBinding="{Binding Value.MyProperty2}"/&gt;
    ///         &lt;/GridView&gt;
    ///     &lt;/ListView.View&gt;
    /// &lt;/ListView&gt;
    /// </code>
    /// <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/> and <see cref="JObservableCollections.Paginated.JPaginationBase{T}.NumofPages"/> properties are set automaticly when something changes in the dictionary. They both become 0 when
    /// there is no <see cref="System.Collections.Generic.KeyValuePair{TKey, TValue}"/> in the dictionary. In order to change the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.CurrentPage"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.ChangePage(int)"/> method and
    /// in order to set the <see cref="JObservableCollections.Paginated.JPaginationBase{T}.PageSize"/>, use <see cref="JObservableCollections.Paginated.JPaginationBase{T}.SetPageSize(int)"/> method.<para/> 
    /// </remarks>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class JPaginatedObservableDictionary<TKey, TValue> : JPaginationBase<KeyValuePair<TKey, TValue>> where TKey : notnull
    {
        /// <summary>
        /// The full dictionary without the pagination
        /// </summary>
        public JObservableDictionary<TKey, TValue> FullDictionary { get; private set; } = null!;


        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableDictionary(int pageSize) : base(pageSize)
        {
            FullDictionary = new JObservableDictionary<TKey, TValue>();
            FullDictionary.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullDictionary);
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(IDictionary{TKey, TValue})"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableDictionary(int pageSize, IDictionary<TKey, TValue> dictionary) : base(pageSize)
        {
            FullDictionary = new JObservableDictionary<TKey, TValue>(dictionary);
            FullDictionary.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullDictionary);
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(IEnumerable{KeyValuePair{TKey, TValue}})"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableDictionary(int pageSize, IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(pageSize)
        {
            FullDictionary = new JObservableDictionary<TKey, TValue>(collection);
            FullDictionary.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullDictionary);
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(IEqualityComparer{TKey}?)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableDictionary(int pageSize, IEqualityComparer<TKey>? comparer) : base(pageSize)
        {
            FullDictionary = new JObservableDictionary<TKey, TValue>(comparer);
            FullDictionary.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullDictionary);
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(int)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableDictionary(int pageSize, int capacity) : base(pageSize)
        {
            FullDictionary = new JObservableDictionary<TKey, TValue>(capacity);
            FullDictionary.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullDictionary);
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(IDictionary{TKey, TValue}, IEqualityComparer{TKey}?)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableDictionary(int pageSize, IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer) : base(pageSize)
        {
            FullDictionary = new JObservableDictionary<TKey, TValue>(dictionary, comparer);
            FullDictionary.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullDictionary);
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(IEnumerable{KeyValuePair{TKey, TValue}}, IEqualityComparer{TKey}?)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableDictionary(int pageSize, IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer) : base(pageSize)
        {
            FullDictionary = new JObservableDictionary<TKey, TValue>(collection, comparer);
            FullDictionary.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullDictionary);
        }

        /// <inheritdoc cref="System.Collections.Generic.Dictionary{TKey, TValue}.Dictionary(int, IEqualityComparer{TKey}?)"/>
        /// <param name="pageSize">The initial size of the pages in the dictionary.</param>
        /// <exception cref="System.ArgumentException">pageSize is 0 or negative.</exception>
        public JPaginatedObservableDictionary(int pageSize, int capacity, IEqualityComparer<TKey>? comparer) : base(pageSize)
        {
            FullDictionary = new JObservableDictionary<TKey, TValue>(capacity, comparer);
            FullDictionary.CollectionChanged += OnCollectionChanged;

            SetFullCollection(FullDictionary);
        }
    }
}
