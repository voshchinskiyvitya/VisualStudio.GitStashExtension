using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VisualStudio.GitStashExtension
{
    /// <summary>
    /// Base implementation of <see cref="INotifyPropertyChanged"/> interface.
    /// Contains general methods for Notifiable properties change.
    /// </summary>
    public class NotifyPropertyChangeBase: INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets property value and notifies all subscribers if the value was changed.
        /// </summary>
        /// <typeparam name="T">Type of the changed property.</typeparam>
        /// <param name="newValue">New property value.</param>
        /// <param name="property">Propery reference.</param>
        /// <param name="changeCallback">Action that will be executed after property change.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>True if the value was changed, otherwise false.</returns>
        protected bool SetPropertyValue<T>(T newValue, ref T property, Action changeCallback, [CallerMemberName] string propertyName = null)
        {
            var valueWasChanged = SetPropertyValue(newValue, ref property, propertyName);

            if (valueWasChanged)
            {
                changeCallback?.Invoke();
            }

            return valueWasChanged;
        }

        /// <summary>
        /// Sets property value and notifies all subscribers if the value was changed.
        /// </summary>
        /// <typeparam name="T">Type of the changed property.</typeparam>
        /// <param name="newValue">New property value.</param>
        /// <param name="property">Propery reference.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>True if the value was changed, otherwise false.</returns>
        protected bool SetPropertyValue<T>(T newValue, ref T property, [CallerMemberName] string propertyName = null)
        {
            // Notify subscribers only if the value was changed.
            if ((Equals(property, default(T)) && Equals(newValue, default(T))) ||
                Equals(property, newValue))
            {
                return false;
            }

            property = newValue;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }
    }
}
