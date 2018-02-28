using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using VisualStudio.GitStashExtension.Annotations;
using VisualStudio.GitStashExtension.GitHelpers;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.Services;
using Log = VisualStudio.GitStashExtension.Logger.Logger;

namespace VisualStudio.GitStashExtension.VS.UI
{
    public class StashInfoChangesSectionViewModel: INotifyPropertyChanged
    {
        private readonly FileIconsService _fileIconsService;
        private readonly Stash _stash;
        private readonly GitCommandExecuter _gitCommandExecuter;
        private readonly ITeamExplorer _teamExplorer;
        private readonly IVsDifferenceService _vsDiffService;
        private ObservableCollection<TreeViewItemWithIcon> _changeItems;


        public event PropertyChangedEventHandler PropertyChanged;

        public StashInfoChangesSectionViewModel(Stash stash, FileIconsService fileIconsService, GitCommandExecuter gitCommandExecuter, ITeamExplorer teamExplorer, IVsDifferenceService vsDiffService)
        {
            _fileIconsService = fileIconsService;
            _gitCommandExecuter = gitCommandExecuter;
            _teamExplorer = teamExplorer;
            _vsDiffService = vsDiffService;
            _stash = stash;

            if (stash == null)
                return;

            var separator = '/';
            var rootNode = new TreeNode();
            var paths = stash.ChangedFiles
                .Select(f => f.Path)
                .Where(x => !string.IsNullOrEmpty(x.Trim()))
                .ToList();

            foreach (var path in paths)
            {
                var currentNode = rootNode;
                var pathNodes = path.Split(separator);
                foreach (var item in pathNodes)
                {
                    var foundedNode = currentNode.Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Text == item);
                    currentNode = foundedNode ?? currentNode.Nodes.Add(item);
                }
            }

            var rootTreeViewItem = ToTreeViewItem(rootNode, false);
            ChangeItems = new ObservableCollection<TreeViewItemWithIcon>(rootTreeViewItem.Items.ToList());
        }

        public ObservableCollection<TreeViewItemWithIcon> ChangeItems
        {
            get => _changeItems;
            set
            {
                _changeItems = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Run file diff.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="fileName">File name.</param>
        public void RunDiff(string filePath, string fileName)
        {
            var beforeTempPath = Path.GetTempFileName();
            var afterTempPath = Path.GetTempFileName();

            try
            {

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private TreeViewItemWithIcon ToTreeViewItem(TreeNode node, bool isFile)
        {
            var fileParts = node.Text.Split('.');
            var fileExtension = fileParts.Last();
            var icon = isFile
                ? _fileIconsService.GetFileIcon("." + fileExtension)
                : _fileIconsService.GetFolderIcon(true);

            var treeViewItem = new TreeViewItemWithIcon
            {
                Text = node.Text,
                FullPath = GetTreeViewNodeFullPath(node),
                Source = icon,
                IsExpanded = !isFile,
                IsFile = isFile
            };

            foreach (var child in node.Nodes.Cast<TreeNode>().ToList())
            {
                if (treeViewItem.Items == null)
                {
                    treeViewItem.Items = new List<TreeViewItemWithIcon>();
                }

                treeViewItem.Items.Add(ToTreeViewItem(child, child.Nodes.Count == 0));
            }
            return treeViewItem;
        }

        private string GetTreeViewNodeFullPath(TreeNode node)
        {
            var fullPath = string.Empty;
            if (!string.IsNullOrEmpty(node.Parent?.Text))
            {
                fullPath += GetTreeViewNodeFullPath(node.Parent) + "/";
            }

            fullPath += node.Text;

            return fullPath;
        }
    }
}
