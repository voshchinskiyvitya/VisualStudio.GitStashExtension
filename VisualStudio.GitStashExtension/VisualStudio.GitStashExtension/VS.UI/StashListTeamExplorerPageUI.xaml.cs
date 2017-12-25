using System;
using System.Windows.Controls;
using Microsoft.TeamFoundation.Controls;
using VisualStudio.GitStashExtension.GitHelpers;

namespace VisualStudio.GitStashExtension.VS.UI
{
    /// <summary>
    /// Interaction logic for StashTeamExplorerPageUI.xaml
    /// </summary>
    public partial class StashListTeamExplorerPageUI : UserControl
    {
        private readonly StashListPageViewModel _viewModel;
        private readonly ITeamExplorer _teamExplorer;
        private readonly IServiceProvider _serviceProvider;

        public StashListTeamExplorerPageUI(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            DataContext = _viewModel = new StashListPageViewModel();

            _serviceProvider = serviceProvider;
            _teamExplorer = _serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;

            var git = new GitCommandExecuter(_serviceProvider);

            if (git.TryGetAllStashes(out var stashes, out var error))
            {
                foreach (var stash in stashes)
                {
                    _viewModel.Stashes.Add(stash);
                }
            }
            else
            {
                _teamExplorer?.ShowNotification(error, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
            }
        }
    }
}
