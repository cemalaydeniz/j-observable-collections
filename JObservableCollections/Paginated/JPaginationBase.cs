using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;


namespace JObservableCollections.Paginated
{
    /// <summary>
    /// Adds the pagination feature to a collection. The reference of the full collection must be sent via <see cref="SetFullCollection(IEnumerable{T})"/> method before using the pagination feature.
    /// </summary>
    /// <remarks>
    /// <see cref="CurrentPage"/> and <see cref="NumofPages"/> properties become 0 when there is no element inside the collection.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the paginated collection. The type must be same as the type of the object that is sent via <see cref="INotifyCollectionChanged.CollectionChanged"/> event.
    /// See examples: <see cref="JObservableCollections.Paginated.JPaginatedObservableList{T}"/> and <see cref="JObservableCollections.Paginated.JPaginatedObservableDictionary{TKey, TValue}"/>.
    /// </typeparam>
    public abstract class JPaginationBase<T> : INotifyPropertyChanged
    {
        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;


        // The reference of paginated collection. It is used for necessary pagination processes, such as calculating the total number of pages
        private IEnumerable<T> fullCollection = null!;

        private IReadOnlyCollection<T> paginatedCollection = null!;
        /// <summary>
        /// The elements on the current page
        /// </summary>
        public IReadOnlyCollection<T> PaginatedCollection { get => paginatedCollection; }


        private int pageSize = 0;
        /// <summary>
        /// The number of elements that are on a single page. To change this value, use <see cref="SetPageSize(int)"/> method.
        /// </summary>
        public int PageSize { get => pageSize; }

        private int numofPages = 0;
        /// <summary>
        /// The total number of the pages that the current collection has. It is 0 when there is no element in the collection.
        /// </summary>
        public int NumofPages { get => numofPages; }

        private int currentPage = 0;
        /// <summary>
        /// The current page number. It is 0 when there is no element in the collection. To change this value, use <see cref="ChangePage(int)"/> method.
        /// </summary>
        public int CurrentPage { get => currentPage; }


        /// <summary>
        /// The <see cref="PageSize"/> must be set before the pagination feature is used.
        /// Make sure <see cref="SetFullCollection(IEnumerable{T})"/> method is called as well.
        /// </summary>
        /// <param name="pageSize">The size of the pages.</param>
        public JPaginationBase(int pageSize)
        {
            SetPageSize(pageSize);
        }

        /// <summary>
        /// The reference of the full collection from the child class must be set before the pagination feature is used.
        /// </summary>
        /// <param name="fullCollection">The reference of the full collection.</param>
        protected void SetFullCollection(IEnumerable<T> fullCollection)
        {
            this.fullCollection = fullCollection;
        }


        /// <summary>
        /// Add this method to the <see cref="System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged"/> event in order to get pagination work
        /// </summary>
        protected void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (!RefreshPageNumbers())
                {
                    RefreshCollection();
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // It will first check to see if the page numbers are affected by this action. If not, it will then check if item is on the current page or
                // or less than the current page. If so, the item affects the elements inside of the paginated collection
                int page = e.NewStartingIndex <= 0 ? (int)Math.Ceiling((double)(e.NewStartingIndex + 1) / pageSize) : 1;
                if (!RefreshPageNumbers() && page <= CurrentPage)
                {
                    RefreshCollection();
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                // It will first check to see if the page numbers are affected by this action. If not, it will then check if item is on the current page or
                // or less than the current page. If so, the item affects the elements inside of the paginated collection
                int page = e.OldStartingIndex <= 0 ? (int)Math.Ceiling((double)(e.OldStartingIndex + 1) / pageSize) : 1;
                if (!RefreshPageNumbers() && page <= CurrentPage)
                {
                    RefreshCollection();
                }
            }
            else
            {
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        if (item is not T)
                            return;

                        if (paginatedCollection.Contains((T)item))
                        {
                            RefreshCollection();
                            return;
                        }
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        if (item is not T)
                            return;

                        if (paginatedCollection.Contains((T)item))
                        {
                            RefreshCollection();
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates both <see cref="NumofPages"/> and <see cref="CurrentPage"/> properties again depending on the <see cref="PageSize"/>.
        /// Use it when the <see cref="numofPages"/> property must be adjusted again, such as when a new element is added
        /// </summary>
        /// <returns>Returns true if the collection needed to be refreshed.</returns>
        private bool RefreshPageNumbers()
        {
            if (fullCollection == null || fullCollection.Count() == 0)
            {
                numofPages = 0;
                currentPage = 0;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumofPages)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));

                RefreshCollection();

                return true;
            }

            numofPages = (int)Math.Ceiling((decimal)fullCollection.Count() / pageSize);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumofPages)));

            if (currentPage == 0)
            {
                currentPage = 1;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));

                RefreshCollection();

                return true;
            }
            else if (currentPage > numofPages)
            {
                currentPage = numofPages;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));

                RefreshCollection();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Refreshes the entire collection within the pagination limit.
        /// </summary>
        private void RefreshCollection()
        {
            if (fullCollection == null)
            {
                paginatedCollection = null!;
            }
            else
            {
                paginatedCollection = fullCollection.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaginatedCollection)));
        }


        /// <summary>
        /// Sets the <see cref="PageSize"/> property. It also changes the <see cref="CurrentPage"/> and <see cref="NumofPages"/> properties depending on the new page size.
        /// </summary>
        /// <param name="newPageSize">The new number of elements on a page.</param>
        /// <exception cref="ArgumentOutOfRangeException">newPageSize is 0 or negative.</exception>
        public void SetPageSize(int newPageSize)
        {
            if (newPageSize <= 0)
                throw new ArgumentOutOfRangeException("newPageSize must be greater than 0");

            if (newPageSize == pageSize)
                return;

            pageSize = newPageSize;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageSize)));

            if (!RefreshPageNumbers())
            {
                RefreshCollection();
            }
        }

        /// <summary>
        /// Changes the <see cref="CurrentPage"/> property.
        /// </summary>
        /// <param name="pageNumber">The new page number.</param>
        /// <exception cref="ArgumentOutOfRangeException">pageNumber is 0 or negative.</exception>
        /// <exception cref="ArgumentException">pageNumber exceeds the total number of pages in the collection.</exception>
        public void ChangePage(int pageNumber)
        {
            if (pageNumber <= 0) throw new ArgumentOutOfRangeException("pageNumber must be between 1 and the number of pages, inclusivly");
            if (pageNumber > numofPages) throw new ArgumentException("pageNumber exceeds the total number of pages in the collection.");

            currentPage = pageNumber;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));

            RefreshCollection();
        }

        /// <summary>
        /// Calculates the absolute index depending on the <see cref="CurrentPage"/>.
        /// </summary>
        /// <param name="relativeIndex">The relative index of an element on the current page.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">relativeIndex is negative.</exception>
        /// <exception cref="IndexOutOfRangeException">The absolute index is out of the range, or the collection is empty.</exception>
        public int GetAbsoluteIndex(int relativeIndex)
        {
            if (relativeIndex < 0) throw new ArgumentOutOfRangeException("relativeIndex is negative.");
            if (currentPage <= 0) throw new IndexOutOfRangeException("The absolute index is out of the range because the collection is empty.");

            int index = ((currentPage - 1) * pageSize) + relativeIndex;
            if (index >= fullCollection.Count())
                throw new IndexOutOfRangeException("The absolute index is out of the range.");

            return index;
        }
    }
}
