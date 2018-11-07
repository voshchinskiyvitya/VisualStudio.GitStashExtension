namespace VisualStudio.GitStashExtension.GitHelpers
{
    /// <summary>
    /// Represents container for git commmands.
    /// </summary>
    public class GitCommandConstants
    {
        public const string StashList = "stash list";

        public const string StashApplyFormatted = "stash apply stash@{{{0}}}";

        public const string StashPopFormatted = "stash pop stash@{{{0}}}";

        public const string Stash = "stash";

        public const string StashIncludeUntracked = "stash --include-untracked";

        public const string StashSaveFormatted = "stash save {0}";

        public const string StashSaveFormattedIncludeUntracked = "stash save --include-untracked {0}";

        public const string StashDeleteFormatted = "stash drop stash@{{{0}}}";

        /// <remarks>
        /// Stash with new and staged files contains files in the different parts:
        ///   1. Regular files in stash@{n}
        ///   2. New files in stash@{n}^3
        ///   3. Staged files in stash@{n}^2
        /// </remarks>

        #region Stash info (files list)
        /// Regular files.
        public const string StashInfoFormatted = "show stash@{{{0}}} --name-only --pretty=\"\"";

        /// Staged only new files.
        public const string StashUntrackedAndStagedInfoFormatted = "show stash@{{{0}}}^^2 --name-only --pretty=\"\" --diff-filter=A";

        /// Staged regular and new files.
        public const string StashStagedInfoFormatted = "show stash@{{{0}}}^^2 --name-only --pretty=\"\"";

        /// New files.
        public const string StashUntrackedInfoFormatted = "show stash@{{{0}}}^^3 --name-only --pretty=\"\"";
        #endregion

        #region Stash parts existing validation (new files, staged, etc)
        /// Staged files.
        public const string CatFileStashCheckUntrackedStagedFilesExist = "cat-file -t stash@{{{0}}}^^2";

        /// New untracked files.
        public const string CatFileStashCheckUntrackedFilesExist = "cat-file -t stash@{{{0}}}^^3";
        #endregion

        #region Stash files diff
        /// Regular or Staged regular files after stash file version
        public const string AfterStashFileVersionSaveTempFormatted = "show stash@{{{0}}}:\"{1}\" > {2}";

        /// Regular or Staged regular files before stash file version
        public const string BeforeStashFileVersionSaveTempFormatted = "show stash@{{{0}}}^^:\"{1}\" > {2}";

        /// Staged new file version
        public const string UntrackedStagedStashFileVersionSaveTempFormatted = "show stash@{{{0}}}^^2:\"{1}\" > {2}";

        /// Regular new file version
        public const string UntrackedStashFileVersionSaveTempFormatted = "show stash@{{{0}}}^^3:\"{1}\" > {2}";
        #endregion

        public const string StashFileDiffFormatted = "difftool --trust-exit-code -y -x \"'{0}' //t\" stash@{{{1}}}^^ stash@{{{1}}} -- {2}";
    }
}
