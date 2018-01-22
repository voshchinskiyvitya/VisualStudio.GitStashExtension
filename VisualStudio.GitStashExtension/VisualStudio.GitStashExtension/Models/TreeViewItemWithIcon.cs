using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace VisualStudio.GitStashExtension.Models
{
    /// <summary>
    /// Represents treeview item viewmodel. Contains file text and icon.
    /// </summary>
    public class TreeViewItemWithIcon
    {
        /// <summary>
        /// File or folder name for treeview.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Icon source for file or folder.
        /// </summary>
        public BitmapSource Source { get; set; }

        /// <summary>
        /// Indicates whether folder is expanded or not.
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Child items.
        /// </summary>
        public IList<TreeViewItemWithIcon> Items { get; set; }
    }
}
