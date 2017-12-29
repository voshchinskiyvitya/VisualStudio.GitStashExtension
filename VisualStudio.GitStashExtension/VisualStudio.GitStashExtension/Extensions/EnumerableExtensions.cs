using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualStudio.GitStashExtension.Extensions
{
    /// <summary>
    /// Extension class for IEnumerable.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Converts Emumerable to ObservableCollection.
        /// </summary>
        /// <typeparam name="T">Type of Enumerable elements.</typeparam>
        /// <param name="enumerable">Enumerable to convert.</param>
        /// <returns>ObservableCollection.</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }
    }
}
