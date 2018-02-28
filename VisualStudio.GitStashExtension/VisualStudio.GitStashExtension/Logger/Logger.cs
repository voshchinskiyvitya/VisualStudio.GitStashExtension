using System;
using System.IO;

namespace VisualStudio.GitStashExtension.Logger
{
    /// <summary>
    /// Represents simple file logger fro saving exception info.
    /// </summary>
    public static class Logger
    {
        private static readonly string LogFilePath;
        private static readonly string LogFileName;
        private static string LogFile => Path.Combine(LogFilePath, LogFileName);

        static Logger()
        {
            LogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VisualStudioGitStashExtension");
            LogFileName = "extension.log";

            var directoryInfo = new FileInfo(LogFile).Directory;
            directoryInfo?.Create();
        }

        /// <summary>
        /// Saves exception info to log file.
        /// </summary>
        /// <param name="e">Exception.</param>
        public static void LogException(Exception e)
        {
            var exceptionString = $"At {DateTime.Now:dd/MM/yyyy hh:mm:ss}" + Environment.NewLine +
                                  $"Exception:" + Environment.NewLine +
                                  $"Message: {e.Message}" + Environment.NewLine +
                                  $"Stack trace: {e.StackTrace}" + Environment.NewLine;

            File.WriteAllText(LogFile, exceptionString);
        }

        /// <summary>
        /// Gets log file location.
        /// </summary>
        /// <returns>Log file path string.</returns>
        public static string GetLogFilePath()
        {
            return LogFile;
        }
    }
}
