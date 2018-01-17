using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell.Interop;
using VisualStudio.GitStashExtension.Annotations;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.Services;

namespace VisualStudio.GitStashExtension.VS.UI
{
    public class StashInfoChangesSectionViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<TreeViewItem> _changeItems;
        private readonly IServiceProvider _serviceProvider;
        private readonly IVsImageService2 _vsImageService;
        private readonly FileIconsService _fileIconsService;
        public event PropertyChangedEventHandler PropertyChanged;

        public StashInfoChangesSectionViewModel(Stash stash, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _vsImageService = _serviceProvider.GetService(typeof(SVsImageService)) as IVsImageService2;
            _fileIconsService = new FileIconsService(_vsImageService);

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

        private TreeViewItem ToTreeViewItem(TreeNode node, bool isFile)
        {
            var fileParts = node.Text.Split('.');
            var fileExtension = fileParts.Last();
            var icon = isFile
                ? _fileIconsService.GetFileIcon("." + fileExtension)
                : _fileIconsService.GetFolderIcon();

            var panel = new StackPanel { Orientation = System.Windows.Controls.Orientation.Horizontal };
            panel.Children.Add(new Image
            {
                Height = 16,
                Width = 16,
                Source = icon
            });
            panel.Children.Add(new TextBlock
            {
                Text = node.Text
            });

            var treeViewItem = new TreeViewItem
            {
                Header = panel,
                IsExpanded = true,
                Focusable = true
            };
            foreach (var child in node.Nodes.Cast<TreeNode>().ToList())
            {
                treeViewItem.Items.Add(ToTreeViewItem(child, child.Nodes.Count == 0));
            }
            return treeViewItem;
        }
    }
}
