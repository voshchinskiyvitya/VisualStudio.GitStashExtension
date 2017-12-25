namespace VisualStudio.GitStashExtension.Models
{
    /// <summary>
    /// Represents model for git command execution result info.
    /// </summary>
    public class GitCommandResult
    {
        /// <summary>
        /// Message after git command execution.
        /// </summary>
        public string OutputMessage { get; set; }

        /// <summary>
        /// Error message after git command execution.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Flag indicates whether or not git command was finished with errors.
        /// </summary>
        public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
    }
}
