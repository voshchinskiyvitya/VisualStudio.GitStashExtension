using Microsoft.Win32;
using System;
using System.IO;

namespace VisualStudio.GitStashExtension.GitHelpers
{
    /// <summary>
    /// Helper class for finding git.exe file.
    /// </summary>
    public static class GitPathHelper
    {
        /// <summary>
        /// Returns git.exe path.
        /// </summary>
        /// <returns>String path representation.</returns>
        public static string GetGitPath()
        {
            var gitPath = GetGitPathFromProgramFiles();

            if (!string.IsNullOrEmpty(gitPath))
                return GetGitExeFilePath(gitPath);

            gitPath = GetGitPathFromRegistryValues();

            return GetGitExeFilePath(gitPath);
        }

        public static string GetGitPathFromProgramFiles()
        {
            string gitPath;
            if (Environment.Is64BitOperatingSystem)
            {
                var programFilesx64 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion", "ProgramW6432Dir", string.Empty).ToString();
                if (!string.IsNullOrEmpty(programFilesx64))
                {
                    gitPath = programFilesx64 + @"\git";
                    return Directory.Exists(gitPath) ? gitPath : string.Empty;
                }
            }

            gitPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\git";
            return Directory.Exists(gitPath) ? gitPath : string.Empty;
        }


        public static string GetGitPathFromRegistryValues()
        {
            var gitPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Git_is1", "InstallLocation", string.Empty).ToString();
            if(!string.IsNullOrEmpty(gitPath))
                return Directory.Exists(gitPath) ? gitPath : string.Empty;

            gitPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Git_is1", "InstallLocation", string.Empty).ToString();
            if (!string.IsNullOrEmpty(gitPath))
                return Directory.Exists(gitPath) ? gitPath : string.Empty;

            return string.Empty;
        }


        private static string GetGitExeFilePath(string gitInstallPath)
        {
            if (Directory.Exists(gitInstallPath))
            {
                return gitInstallPath + @"\bin\git.exe";
            }

            return null;
        }
    }
}
