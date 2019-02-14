using System.Windows;

namespace VisualStudio.GitStashExtension.Extensions
{
    /// <summary>
    /// Class for the extensions of the <see cref="FrameworkElement"/>.
    /// </summary>
    public static class FrameworkElementExtension
    {
        /// <summary>
        /// Extension method for <see cref="FrameworkElement"/>. Returns needed parent of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the parent control.</typeparam>
        /// <param name="child">Element to find arent for.</param>
        public static T FindParentByType<T>(this FrameworkElement child) where T : FrameworkElement
        {
            var parentNode = child?.Parent;

            if (parentNode == null)
                return null;

            var parent = parentNode as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParentByType<T>(parentNode as FrameworkElement);
            }
        }
    }
}
