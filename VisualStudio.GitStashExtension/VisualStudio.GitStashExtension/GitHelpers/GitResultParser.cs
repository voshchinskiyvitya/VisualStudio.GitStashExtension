using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VisualStudio.GitStashExtension.Models;

namespace VisualStudio.GitStashExtension.GitHelpers
{
    /// <summary>
    /// Represents parser for git commands result.
    /// </summary>
    public static class GitResultParser
    {
        /// <summary>
        /// Converst git stash list command string result to list of Stash models.
        /// </summary>
        /// <param name="data">String data.</param>
        /// <returns>List of Stash models.</returns>
        public static IList<Stash> ParseStashListResult(string data)
        {
            var stashesInfo = data.Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            var stashes = new List<Stash>();

            foreach (var stashData in stashesInfo)
            {
                var info = stashData.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                var firstSectionMatch = Regex.Match(info[0], @"stash@{(.*)}");
                var thirdSectionMatch = Regex.Match(info[1], @"On (.*)");

                var stash = new Stash();

                if (firstSectionMatch.Success)
                {
                    if (int.TryParse(firstSectionMatch.Groups[1].Value, out var stashId))
                    {
                        stash.Id = stashId;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (thirdSectionMatch.Success)
                {
                    stash.BranchName = thirdSectionMatch.Groups[1].Value;
                }

                stash.Message = info[2];

                stashes.Add(stash);
            }

            return stashes;
        }

        /// <summary>
        /// Converst git stash info command string result to Stash model.
        /// </summary>
        /// <param name="data">String data.</param>
        /// <returns>Stash model.</returns>
        public static Stash ParseStashInfoResult(string data)
        {
            var filePaths = data.Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            return new Stash
            {
                ChangedFiles = filePaths.Select(p => new ChangedFile { Path = p }).ToList()
            };
        }

        /// <summary>
        /// Converst git stash untrackedinfo command string result to Stash model.
        /// </summary>
        /// <param name="data">String data.</param>
        /// <returns>Stash model.</returns>
        public static Stash ParseStashUntrackedInfoResult(string data)
        {
            var stash = ParseStashInfoResult(data);
            foreach (var file in stash.ChangedFiles)
            {
                file.IsNew = true;
            }

            return stash;
        }
    }
}
