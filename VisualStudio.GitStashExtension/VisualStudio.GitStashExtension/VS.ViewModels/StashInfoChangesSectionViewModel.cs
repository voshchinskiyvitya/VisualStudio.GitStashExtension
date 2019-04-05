using System;
using System.Collections.ObjectModel;
using System.IO;
using EnvDTE;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using VisualStudio.GitStashExtension.Extensions;
using VisualStudio.GitStashExtension.GitHelpers;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.Services;
using Log = VisualStudio.GitStashExtension.Logger.Logger;

namespace VisualStudio.GitStashExtension.VS.ViewModels
{
    public class StashInfoChangesSectionViewModel: NotifyPropertyChangeBase
    {
        private readonly FileIconsService _fileIconsService;
        private readonly Stash _stash;
        private readonly GitCommandExecuter _gitCommandExecuter;
        private readonly ITeamExplorer _teamExplorer;
        private readonly IVsDifferenceService _vsDiffService;
        private readonly DTE _dte;

        public StashInfoChangesSectionViewModel(Stash stash, 
            FileIconsService fileIconsService, 
            GitCommandExecuter gitCommandExecuter, 
            ITeamExplorer teamExplorer, 
            IVsDifferenceService vsDiffService, 
            DTE dte)
        {
            _fileIconsService = fileIconsService;
            _gitCommandExecuter = gitCommandExecuter;
            _teamExplorer = teamExplorer;
            _vsDiffService = vsDiffService;
            _dte = dte;
            _stash = stash;

            if (stash?.ChangedFiles == null)
                return;

            var rootTreeViewItem = stash.ChangedFiles.ToTreeViewItemStructure();
            var rootViewModel = new TreeViewItemWithIconViewModel(rootTreeViewItem, _fileIconsService);
            ChangeItems = rootViewModel.Items;
        }

        /// <summary>
        /// Stash change items collection.
        /// </summary>
        public ObservableCollection<TreeViewItemWithIconViewModel> ChangeItems { get; }

        /// <summary>
        /// Run file diff.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="isNew">Indicates that file is new and doesn't have previous version.</param>
        /// <param name="isStaged">Indicates that file was staged before the stash.</param>
        public void RunDiff(string filePath, string fileName, bool isNew, bool isStaged)
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
                        if (!_gitCommandExecuter.TrySaveFileUntrackedStashVersion(_stash.Id, filePath, untrackedTempPath, true, out var error))
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
                        if (!_gitCommandExecuter.TrySaveFileUntrackedStashVersion(_stash.Id, filePath, untrackedTempPath, false, out var error))
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

                if (!_gitCommandExecuter.TrySaveFileBeforeStashVersion(_stash.Id, filePath, beforeTempPath, out var errorMessage))
                {
                    _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
                    return;
                }

                if (!_gitCommandExecuter.TrySaveFileAfterStashVersion(_stash.Id, filePath, afterTempPath, out errorMessage))
                {
                    _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
                    return;
                }

                _vsDiffService.OpenComparisonWindow2(beforeTempPath, afterTempPath, fileName + " stash diff", "Stash diff", fileName + " before stash", fileName + " after stash", "Stash file content", "", 0);

            }
            catch(Exception e)
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
    }
}
