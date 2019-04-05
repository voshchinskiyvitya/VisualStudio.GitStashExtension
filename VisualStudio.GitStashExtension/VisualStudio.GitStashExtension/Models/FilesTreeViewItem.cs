using System.Collections.Generic;
using System.Linq;

namespace VisualStudio.GitStashExtension.Models
{
    /// <summary>
    /// Represents file tree view item model.
    /// </summary>
    public class FilesTreeViewItem
    {
        #region Properties
        /// <summary>
        /// File or folder name for treeview.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Full path for current file or folder.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Indicates whether current item is file.
        /// </summary>
        public bool IsFile { get; set; }

        /// <summary>
        /// Flag indicates whether this file is new (untracked) or not.
        /// </summary>
        public bool? IsNew { get; set; }

        /// <summary>
        /// Flag indicates whether this file is new (untracked) and staged or not.
        /// </summary>
        public bool? IsStaged { get; set; }

        /// <summary>
        /// File extension (.cs, .txt, etc.).
        /// </summary>
        public string FileExtension => FullPath?.Split('.')?.LastOrDefault();

        /// <summary>
        /// Child items.
        /// </summary>
        /// <remarks>
        /// Only folder items (IsFile == False) can contain Child items.
        /// </remarks>
        public IList<FilesTreeViewItem> Items { get; set; }
        #endregion
    }
}
