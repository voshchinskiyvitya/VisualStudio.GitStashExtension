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

namespace VisualStudio.GitStashExtension.VS.UI
{
    /// <summary>
    /// Interaction logic for StashInfoChangesSection.xaml
    /// </summary>
    public partial class StashInfoChangesSectionUI : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IVsImageService2 _vsImageService;
        private readonly FileIconsService _fileIconsService;
        private readonly GitCommandExecuter _gitCommandExecuter;
        private readonly ITeamExplorer _teamExplorer;
        private readonly IVsDifferenceService _vsDiffService;
        private readonly DTE _dte;
        private readonly StashInfoChangesSectionViewModel _viewModel;

        public StashInfoChangesSectionUI(Stash stash, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _vsImageService = _serviceProvider.GetService(typeof(SVsImageService)) as IVsImageService2;
            _fileIconsService = new FileIconsService(_vsImageService);
            _gitCommandExecuter = new GitCommandExecuter(_serviceProvider);
            _teamExplorer = _serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
            _vsDiffService = _serviceProvider.GetService(typeof(SVsDifferenceService)) as IVsDifferenceService;
            _dte = _serviceProvider.GetService(typeof(DTE)) as DTE;
            InitializeComponent();

            DataContext = _viewModel = new StashInfoChangesSectionViewModel(stash, _fileIconsService, _gitCommandExecuter, _teamExplorer, _vsDiffService, _dte);
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
            var parent = ((Control) sender).Parent as UIElement;
            parent?.RaiseEvent(eventArg);
        }

        private void TreeView_ExpandedOrCollapsed(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as TreeViewItem;
            if (item?.DataContext is TreeViewItemWithIcon itemWithIcon && 
                !itemWithIcon.IsFile)
            {
                itemWithIcon.Source = _fileIconsService.GetFolderIcon(item.IsExpanded);
            }
        }

        private void CompareFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var treeItem = menuItem?.Tag as TreeViewItemWithIcon;
            var filePath = treeItem?.FullPath;
            var fileName = treeItem?.Text;
            var isNew = treeItem?.IsNew ?? false;

            _viewModel.RunDiff(filePath, fileName, isNew);
        }
    }
}
