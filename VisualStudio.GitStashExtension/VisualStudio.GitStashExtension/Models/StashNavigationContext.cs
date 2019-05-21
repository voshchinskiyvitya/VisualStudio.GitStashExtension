namespace VisualStudio.GitStashExtension.Models
{
    /// <summary>
    /// Represents simple model for storing Stash navigation data.
    /// </summary>
    public class StashNavigationContext
    {
        /// <summary>
        /// Stash data.
        /// </summary>
        public Stash Stash { get; set; }

        /// <summary>
        /// Flag indicates that user navigated directly to the Stash Info page, using Stash info link.
        /// </summary>
        /// <remarks>
        /// If this flag is false, this means that user navigated using Team Explorer history panel.
        /// </remarks>
        public bool NavigatedDirectly { get; set; }
    }
}
