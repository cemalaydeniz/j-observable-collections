// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


using System.ComponentModel;


namespace JObservableCollections.Helper
{
    /// <summary>
    /// A helper class to send notification when a property changes.
    /// Alternatively, <see cref="INotifyPropertyChanged"/> can be used when inheritance is not possible
    /// </summary>
    public class JObservableItem : INotifyPropertyChanged
    {
        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;


        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        protected void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
