namespace VisualStudio.GitStashExtension.GitHelpers
{
    /// <summary>
    /// Represents container for git commmands.
    /// </summary>
    public class GitCommandConstants
    {
        public const string StashList = "stash list";

        public const string StashApplyFormatted = "stash apply stash@{{{0}}}";

        public const string Stash = "stash";

        public const string StashSaveFormatted = "stash save {0}";
    }
}
