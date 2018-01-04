using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (Environment.Is64BitOperatingSystem)
            {
                var programFilesx64 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion", "ProgramW6432Dir", null);
                if (programFilesx64 != null)
                {
                    return GetGitExeFilePath(programFilesx64 + @"\git");
                }
            }

            var gitPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\git";
            return GetGitExeFilePath(gitPath);
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
