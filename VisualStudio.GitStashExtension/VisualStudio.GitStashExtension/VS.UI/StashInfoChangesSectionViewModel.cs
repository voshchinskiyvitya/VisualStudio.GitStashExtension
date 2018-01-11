using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Forms;
using VisualStudio.GitStashExtension.Annotations;
using VisualStudio.GitStashExtension.Models;

namespace VisualStudio.GitStashExtension.VS.UI
{
    public class StashInfoChangesSectionViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<TreeViewItem> _changeItems;
        public event PropertyChangedEventHandler PropertyChanged;

        public StashInfoChangesSectionViewModel(Stash stash)
        {
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

            var rootTreeViewItem = ConvertToTreeViewItem(rootNode);
            ChangeItems = new ObservableCollection<TreeViewItem>(rootTreeViewItem.Items.Cast<TreeViewItem>().ToList());
        }

        public ObservableCollection<TreeViewItem> ChangeItems
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

        private TreeViewItem ConvertToTreeViewItem(TreeNode node)
        {
            var treeViewItem = new TreeViewItem
            {
                Header = node.Text,
                IsExpanded = true,
                Focusable = true
            };
            foreach (var child in node.Nodes.Cast<TreeNode>().ToList())
            {
                treeViewItem.Items.Add(ConvertToTreeViewItem(child));
            }
            return treeViewItem;
        }
    }
}
