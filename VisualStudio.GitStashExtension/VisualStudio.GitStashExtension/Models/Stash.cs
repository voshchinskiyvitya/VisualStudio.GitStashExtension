namespace VisualStudio.GitStashExtension.Models
{
    /// <summary>
    /// Represents simple model for stash info.
    /// </summary>
    public class Stash
    {
        /// <summary>
        /// Stash ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Stashe message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Branch name on which stash was created.
        /// </summary>
        public string BranchName { get; set; }
    }
}
