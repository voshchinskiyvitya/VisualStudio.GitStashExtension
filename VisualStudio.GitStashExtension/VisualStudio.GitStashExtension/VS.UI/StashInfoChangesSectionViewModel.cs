using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using VisualStudio.GitStashExtension.Annotations;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.Services;

namespace VisualStudio.GitStashExtension.VS.UI
{
    public class StashInfoChangesSectionViewModel: INotifyPropertyChanged
    {
        private readonly FileIconsService _fileIconsService;
        private ObservableCollection<TreeViewItemWithIcon> _changeItems;
        public event PropertyChangedEventHandler PropertyChanged;

        public StashInfoChangesSectionViewModel(Stash stash, FileIconsService fileIconsService)
        {
            _fileIconsService = fileIconsService;

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
                Source = icon,
                IsExpanded = !isFile
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
    }
}
