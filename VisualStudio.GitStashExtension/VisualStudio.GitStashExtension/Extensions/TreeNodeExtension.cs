using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VisualStudio.GitStashExtension.Models;

namespace VisualStudio.GitStashExtension.Extensions
{
    public static class TreeNodeExtension
    {
        public static FilesTreeViewItem ToTreeViewItemStructure(this IList<ChangedFile> changedFiles)
        {
            return changedFiles.ToTreeNodeStructure().ToTreeViewItem(false);
        }

        private static FilesTreeViewItem ToTreeViewItem(this TreeNode node, bool isFile)
        {
            var attributes = node.Tag as FileAttributes;

            var treeViewItem = new FilesTreeViewItem
            {
                Text = node.Text,
                FullPath = node.GetTreeViewNodeFullPath(),
                IsFile = isFile,
                IsNew = attributes?.IsNew,
                IsStaged = attributes?.IsStaged,
                Items = node.Nodes?.OfType<TreeNode>()?.Select(n => ToTreeViewItem(n, n.Nodes.Count == 0))?.ToList() ?? new List<FilesTreeViewItem>()
            };

            return treeViewItem;
        }

        private static TreeNode ToTreeNodeStructure(this IList<ChangedFile> changedFiles)
        {
            var separator = '/';
            var rootNode = new TreeNode();

            foreach (var file in changedFiles)
            {
                if (string.IsNullOrEmpty(file.Path.Trim()))
                {
                    continue;
                }

                var currentNode = rootNode;
                var pathNodes = file.Path.Split(separator);
                foreach (var item in pathNodes)
                {
                    var foundedNode = currentNode.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text == item);
                    if (foundedNode != null)
                    {
                        currentNode = foundedNode;
                    }
                    else
                    {
                        currentNode = currentNode.Nodes.Add(item);
                        // Last node in the path -> file.
                        if (item == pathNodes.LastOrDefault())
                        {
                            // Additional file info
                            currentNode.Tag = new FileAttributes
                            {
                                IsNew = file.IsNew,
                                IsStaged = file.IsStaged
                            };
                        }
                    }
                }
            }

            return rootNode;
        }

        private static string GetTreeViewNodeFullPath(this TreeNode node)
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
