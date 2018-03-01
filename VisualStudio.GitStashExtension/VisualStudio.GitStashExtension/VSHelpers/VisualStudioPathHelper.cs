using System.IO;
using Microsoft.Win32;

namespace VisualStudio.GitStashExtension.VSHelpers
{
    /// <summary>
    /// Helper class for getting paths visual studio or visual studio tools.
    /// </summary>
    public static class VisualStudioPathHelper
    {
        /// <summary>
        /// Gets VS install dir from registry value.
        /// </summary>
        public static string GetVisualStudioInstallPath()
        {
            var visualStudioInstalledPath = string.Empty;
            var visualStudioRegistryPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\VisualStudio\SxS\VS7");
            if (visualStudioRegistryPath != null)
            {
                visualStudioInstalledPath = visualStudioRegistryPath.GetValue("15.0", string.Empty) as string;
            }

            return visualStudioInstalledPath;
        }

        /// <summary>
        /// Gets vsDiffMerge.exe tool dir using VS install dir.
        /// </summary>
        public static string GetVsDiffMergeToolPath()
        {
            var toolPath = GetVisualStudioInstallPath() + @"Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\vsDiffMerge.exe";
            return File.Exists(toolPath) ? toolPath : string.Empty;
        }
    }
}
