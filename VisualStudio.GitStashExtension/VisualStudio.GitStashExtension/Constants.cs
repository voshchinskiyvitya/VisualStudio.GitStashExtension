using System.Drawing;

namespace VisualStudio.GitStashExtension
{
    /// <summary>
    /// Represents container for constants.
    /// </summary>
    public static class Constants
    {
        public const string StashNavigationItemId = "350FF356-5C4E-4861-B4C7-9CC97438F31F";

        public const string StashPageId = "00FAA5EB-3EC5-4098-B550-D58C7035DDBE";

        public const string StashInfoPageId = "ABBF38A6-1272-4C92-AC7A-79FBCFC796A9";

        public const string StashListSectionId = "A94AE67A-AF52-42F2-B6E4-C433732AAEB3";

        public const string StashInfoChangesSectionId = "43CB2B08-DBEE-4096-B091-EBAE5E2E07D2";

        public const string StashesLabel = "Stashes";

        public const string StashesInfoLabel = "Stashe Info";

        public const string StashesListSectionLabel = "Stash list";

        public const string StashesInfoChangesSectionLabel = "Files";

        public static Color NavigationItemColorArgb = Color.FromArgb(0, 213, 66);  // green

        public static Color BlueThemeColor = Color.FromArgb(255, 236, 181);

        public static Color LightThemeColor = Color.FromArgb(238, 238, 242);  

        public static Color DarkThemeColor = Color.FromArgb(45, 45, 48);  

        public const string UnknownRepositoryErrorMessage = "Select repository to find stashes.";

        public const string UnexpectedErrorMessage = "Unexpected error.";

        public const string DiffToolErrorMessage = "Can't run vsDiffMerge.exe to compare files. \n" +
                                                   "Please check tool install path: \n" +
                                                   "%visual_studio_install-dir%Common7\\IDE\\CommonExtensions\\Microsoft\\TeamFoundation\\Team Explorer\\vsDiffMerge.exe";
    }
}
