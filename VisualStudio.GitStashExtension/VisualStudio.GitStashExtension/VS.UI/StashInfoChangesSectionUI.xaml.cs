using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnvDTE;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using VisualStudio.GitStashExtension.GitHelpers;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.Services;
using VisualStudio.GitStashExtension.VS.ViewModels;

namespace VisualStudio.GitStashExtension.VS.UI
{
    /// <summary>
    /// Interaction logic for StashInfoChangesSection.xaml
    /// </summary>
    public partial class StashInfoChangesSectionUI : UserControl
    {
        private readonly StashInfoChangesSectionViewModel _viewModel;

        public StashInfoChangesSectionUI(Stash stash, IServiceProvider serviceProvider)
        {
            var vsImageService = serviceProvider.GetService(typeof(SVsImageService)) as IVsImageService2;
            var fileIconsService = new FileIconsService(vsImageService);
            var gitCommandExecuter = new GitCommandExecuter(serviceProvider);
            var teamExplorer = serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
            var vsDiffService = serviceProvider.GetService(typeof(SVsDifferenceService)) as IVsDifferenceService;
            var dte = serviceProvider.GetService(typeof(DTE)) as DTE;
            InitializeComponent();
            InitializeComponent();

            DataContext = _viewModel = new StashInfoChangesSectionViewModel(stash, fileIconsService, gitCommandExecuter, teamExplorer, vsDiffService, dte);
        }

        private void PreviewMouseWheelForTreeView(object sender, MouseWheelEventArgs e)
        {
            if (e.Handled)
                return;

            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent,
                Source = sender
            };
            var parent = (sender as Control)?.Parent as UIElement;
            parent?.RaiseEvent(eventArg);
        }

        private void TreeView_ExpandedOrCollapsed(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as TreeViewItem;
            if (item?.DataContext is FilesTreeViewItem itemWithIcon && 
                !itemWithIcon.IsFile)
            {
                //itemWithIcon.Source = _fileIconsService.GetFolderIcon(item.IsExpanded);
            }
        }

        private void CompareFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var treeItem = menuItem?.Tag as FilesTreeViewItem;
            var filePath = treeItem?.FullPath;
            var fileName = treeItem?.Text;
            var isNew = treeItem?.IsNew ?? false;
            var isStaged = treeItem?.IsStaged ?? false;

            _viewModel.RunDiff(filePath, fileName, isNew, isStaged);
        }
    }
}
