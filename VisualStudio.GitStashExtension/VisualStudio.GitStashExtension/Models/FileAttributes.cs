namespace VisualStudio.GitStashExtension.Models
{
    /// <summary>
    /// Simple container for Stash file attributes (isNew, isStaged, etc.).
    /// </summary>
    public class FileAttributes
    {
        /// <summary>
        /// Flag indicates whether this file is new (untracked) or not.
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// Flag indicates whether file was staged before the stash or not.
        /// </summary>
        public bool IsStaged { get; set; }
    }
}
