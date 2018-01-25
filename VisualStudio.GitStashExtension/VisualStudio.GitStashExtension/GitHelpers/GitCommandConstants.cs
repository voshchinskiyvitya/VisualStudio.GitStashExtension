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

        public const string StashDeleteFormatted = "stash drop stash@{{{0}}}";

        public const string StashInfoFormatted = "stash show stash@{{{0}}} --name-only";

        public const string StashFileDiffFormatted = "difftool -y -R stash@{{{0}}} -- {1}";
    }
}
