using EnvDTE;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VisualStudio.GitStashExtension.GitHelpers;
using VisualStudio.GitStashExtension.Helpers;
using VisualStudio.GitStashExtension.Models;
using Log = VisualStudio.GitStashExtension.Logger.Logger;

namespace VisualStudio.GitStashExtension.Services
{
    /// <summary>
    /// Represents simple service for git commands execution.
    /// Actually, it's just a middle layer between <see cref="GitCommandExecuter"/> and team explorer page/section view models.
    /// </summary>
    public class VisualStudioGitService
    {
        private readonly GitCommandExecuter _gitCommandExecuter;
        private readonly ITeamExplorer _teamExplorer;
        private readonly DTE _dte;
        private readonly IVsDifferenceService _vsDiffService;

        public VisualStudioGitService(IServiceProvider serviceProvider)
        {
            _gitCommandExecuter = new GitCommandExecuter(serviceProvider);
            _teamExplorer = serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
            _dte = serviceProvider.GetService(typeof(DTE)) as DTE;
            _vsDiffService = serviceProvider.GetService(typeof(SVsDifferenceService)) as IVsDifferenceService;
        }

        /// <summary>
        /// Tries to create stash (if operatiopn wasn't successful - shows Team Explorer notification).
        /// </summary>
        /// <param name="message">message that should be assigned to the Stash.</param>
        /// <param name="includeUntrackedFiles">Flag indicates that untracked files should be included.</param>
        /// <returns>True if operation was successful, otherwise - false.</returns>
        public bool TryCreateStash(string message, bool includeUntrackedFiles)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            var result = _dte.ItemOperations.PromptToSave;
            if (_gitCommandExecuter.TryCreateStash(message, includeUntrackedFiles, out var errorMessage))
            {
                _teamExplorer.CurrentPage.RefreshPageAndSections();
                return true;
            }
            else
            {
                _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
                return false;
            }
        }

        /// <summary>
        /// Run file diff for the specified file in the specified stash.
        /// </summary>
        /// <param name="stashId">Id of the stash.</param>
        /// <param name="filePath">File path.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="isNew">Indicates that file is new and doesn't have previous version.</param>
        /// <param name="isStaged">Indicates that file was staged before the stash.</param>
        public void RunDiff(int stashId, string filePath, string fileName, bool isNew, bool isStaged)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            var beforeTempPath = Path.GetTempFileName();
            var afterTempPath = Path.GetTempFileName();
            var untrackedTempPath = Path.GetTempFileName();

            try
            {
                if (isNew)
                {
                    if (isStaged)
                    {
                        if (!_gitCommandExecuter.TrySaveFileUntrackedStashVersion(stashId, filePath, untrackedTempPath, true, out var error))
                        {
                            _teamExplorer?.ShowNotification(error, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
                            return;
                        }
                        else
                        {
                            _dte.ItemOperations.OpenFile(untrackedTempPath);
                            return;
                        }
                    }
                    else
                    {
                        if (!_gitCommandExecuter.TrySaveFileUntrackedStashVersion(stashId, filePath, untrackedTempPath, false, out var error))
                        {
                            _teamExplorer?.ShowNotification(error, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
                            return;
                        }
                        else
                        {
                            _dte.ItemOperations.OpenFile(untrackedTempPath);
                            return;
                        }
                    }
                }

                if (!_gitCommandExecuter.TrySaveFileBeforeStashVersion(stashId, filePath, beforeTempPath, out var errorMessage))
                {
                    _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
                    return;
                }

                if (!_gitCommandExecuter.TrySaveFileAfterStashVersion(stashId, filePath, afterTempPath, out errorMessage))
                {
                    _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
                    return;
                }

                _vsDiffService.OpenComparisonWindow2(beforeTempPath, afterTempPath, fileName + " stash diff", "Stash diff", fileName + " before stash", fileName + " after stash", "Stash file content", "", 0);

            }
            catch (Exception e)
            {
                Log.LogException(e);
                _teamExplorer?.ShowNotification(Constants.UnexpectedErrorMessage + Environment.NewLine + $"Find error info in {Log.GetLogFilePath()}", NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
            }
            finally
            {
                File.Delete(beforeTempPath);
                File.Delete(afterTempPath);
            }
        }

        /// <summary>
        /// Get stash list content based on search text.
        /// </summary>
        /// <param name="searchText">Search text.</param>
        public IList<Stash> GetAllStashes(string searchText)
        {
            if (_gitCommandExecuter.TryGetAllStashes(out var stashes, out var error))
            {
                return stashes.Where(s => string.IsNullOrEmpty(searchText) || s.Message.Contains(searchText)).ToList();
            }

            _teamExplorer?.ShowNotification(error, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
            return new List<Stash>();
        }

        /// <summary>
        /// Applies stash to current repository state.
        /// </summary>
        /// <param name="stashId">Stash Id.</param>
        public void ApplyStash(int stashId)
        {
            if (_gitCommandExecuter.TryApplyStash(stashId, out var errorMessage))
            {
                _teamExplorer.NavigateToPage(new Guid(TeamExplorerPageIds.GitChanges), null);
            }
            else
            {
                _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
            }
        }

        /// <summary>
        /// Pops (applies and removes) stash by id.
        /// </summary>
        /// <param name="stashId">Stash Id.</param>
        public void PopStash(int stashId)
        {
            if (_gitCommandExecuter.TryPopStash(stashId, out var errorMessage))
            {
                _teamExplorer.NavigateToPage(new Guid(TeamExplorerPageIds.GitChanges), null);
                RemovedStashesContainer.AddDeletedStash(stashId);
            }
            else
            {
                _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
            }
        }

        /// <summary>
        /// Removes stash by id.
        /// </summary>
        /// <param name="stashId">Stash Id.</param>
        public void DeleteStash(int stashId)
        {
            if (_gitCommandExecuter.TryDeleteStash(stashId, out var errorMessage))
            {
                _teamExplorer.CurrentPage.RefreshPageAndSections();
                RemovedStashesContainer.AddDeletedStash(stashId);
            }
            else
            {
                _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
            }
        }

        /// <summary>
        /// Gets stash info by id.
        /// </summary>
        /// <param name="stashId">Stash Id.</param>
        public Stash GetStashInfo(int stashId)
        {
            if (!_gitCommandExecuter.TryGetStashInfo(stashId, out var stash, out var errorMessage))
            {
                _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
            }

            return stash;
        }
    }
}
