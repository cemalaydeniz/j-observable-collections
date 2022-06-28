# J Observable Collections
This is a utility class containing observable and paginated observable versions of every generic collection under the `System.Collections.Generic` namespace. Each J Observable Collection gets inheritance from its related generic collection. So that they can be used as if they are normal generic collections.

# What does it include?
It includes observable and paginated observable collections:
```csharp
JObservableDictionary<TKey, TValue>
JPaginatedObservableDictionary<TKey, TValue>

JObservableHashSet<T>
JPaginatedObservableHashSet<T>

JObservableLinkedList<T>
JPaginatedObservableLinkedList<T>

JObservableQueue<T>
JPaginatedObservableQueue<T>

JObservableSortedDictionary<TKey, TValue>
JPaginatedObservableSortedDictionary<TKey, TValue>

JObservableSortedList<TKey, TValue>
JPaginatedObservableSortedList<TKey, TValue>

JObservableSortedSet<T>
JPaginatedObservableSortedSet<T>

JObservableStack<T>
JPaginatedObservableStack<T>
```

It also includes `JPaginationBase` class in order to create your own custom paginated collection, and `JObservableItem` as an extra in order to raise the nofitication event for your properties inside of the elements.

# How to use it?
The observable collections are under the `JUtility.JObservableCollections` namespace while the paginated observable collections are under the `JUtility.JObservableCollections.Paginated` namespace. You can check out the examples down below to see how to use observables and paginated observables. Each collection class also has detailed documentation about how to use them in the code.

## Example code for `JObservableQueue<T>`
`Customer.cs`: The type of the elements in the queue. 
```csharp
public class Customer
{
    public int OrderNumber { get; set; }
    public string Name { get; set; }
}
```
The class above does not raise the notification event when a property in `Customer` object changes. In order to get notified, the `Customer` class must implement `INotifyPropertyChanged` or get inheritance from `JObservableItem` class.

`INotifyPropertyChanged` implementation:
```csharp
public class Customer : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private int orderNumber;
    public int OrderNumber
    {
        get => orderNumber;
        set
        {
            orderNumber = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderNumber)));
        }
    }

    private string name;
    public string Name
    {
        get => name;
        set
        {
            name = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        }
    }
}
```
`JObservableItem` implementation:
```csharp
using JUtility.JObservableCollections.Helper;

public class Customer : JObservableItem
{
    private int orderNumber;
    public int OrderNumber
    {
        get => orderNumber;
        set
        {
            orderNumber = value;
            OnPropertyChanged(nameof(OrderNumber));
        }
    }

    private string name;
    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged(nameof(Name));
        }
    }
}
```

`MainWindowViewModel.cs`: VM of the MainWindow. The `customers` variable or `Customers` property can be used directly to modify the queue.
```csharp
using JUtility.JObservableCollections;

public class MainWindowViewModel
{
    private JObservableQueue<Customer> customers = new JObservableQueue<Customer>();
    public JObservableQueue<Customer> Customers { get => customers; }
}
```

`MainWindow.xaml`: The window where the queue is shown
```xaml
...
<ListView ItemsSource="{Binding Customers}">
    <ListView.View>
        <GridView>
            <GridViewColumn DisplayMemberBinding="{Binding OrderNumber}"/>
            <GridViewColumn DisplayMemberBinding="{Binding Name}"/>
        </GridView>
    </ListView.View>
</ListView>
...
```

## Example code for `JPaginatedObservableList<T>`
In this example, we assume, the same `Customer` class is used.

`MainWindowViewModel.cs`: VM of the MainWindow. The `customers.FullList` or `Customers.FullList` property can be used directly to modify the paginated list.
```csharp
using JUtility.JObservableCollections.Paginated;

public class MainWindowViewModel
{
    private JPaginatedObservableList<Customer> customers = new JPaginatedObservableList<Customer>(20);  // Each page size is 20
    public JPaginatedObservableList<Customer> Customers { get => customers; }
}
```

`MainWindow.xaml`: The window where the paginated list is shown
```xaml
...
<ListView ItemsSource="{Binding Customers.PaginatedCollection}">
    <ListView.View>
        <GridView>
            <GridViewColumn DisplayMemberBinding="{Binding OrderNumber}"/>
            <GridViewColumn DisplayMemberBinding="{Binding Name}"/>
        </GridView>
    </ListView.View>
</ListView>
...
```

To be able to set the page size or change the current page, you can use `SetPageSize(int)` or `ChangePage(int)` methods:
```csharp
Customers.FullList.SetPageSize(50);
Customers.FullList.ChangePage(2);
```

# License
This project is licensed under the MIT License - see the LICENSE.md file for details