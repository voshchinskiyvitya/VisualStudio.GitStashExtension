using Microsoft.TeamFoundation.Controls;
using System;
using System.Windows;
using System.Windows.Input;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.Services;
using VisualStudio.GitStashExtension.TeamExplorerExtensions;
using VisualStudio.GitStashExtension.VS.UI.Commands;

namespace VisualStudio.GitStashExtension.VS.ViewModels
{
    /// <summary>
    /// Stash list item view model.
    /// </summary>
    public class StashListItemViewModel : NotifyPropertyChangeBase
    {
        private readonly Stash _stash;
        private readonly VisualStudioGitService _gitService;
        private readonly ITeamExplorer _teamExplorer;
        private readonly IServiceProvider _serviceProvider;

        public StashListItemViewModel(Stash stash, IServiceProvider serviceProvider)
        {
            _stash = stash;
            _serviceProvider = serviceProvider;
            _gitService = new VisualStudioGitService(serviceProvider);
            _teamExplorer = serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
        }

        /// <summary>
        /// Stash ID.
        /// </summary>
        public int Id => _stash.Id;

        /// <summary>
        /// Stash message.
        /// </summary>
        public string Message => _stash.Message;

        #region Commands

        /// <summary>
        /// Applies stash to current repository state.
        /// </summary>
        public ICommand ApplyStashCommand => new DelegateCommand(o => _gitService.ApplyStash(Id));

        /// <summary>
        /// Pops (applies and removes) stash.
        /// </summary>
        public ICommand PopStashCommand => new DelegateCommand(o =>
        {
            var popStashPromptResult = MessageBox.Show($"Are you sure you want to pop the stash? {Environment.NewLine}All stashed changes will be applied and then deleted!",
                        "Pop stash", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (popStashPromptResult == MessageBoxResult.Yes)
            {
                _gitService.PopStash(Id);
            }
        });

        /// <summary>
        /// Removes stash.
        /// </summary>
        public ICommand DeleteStashCommand => new DelegateCommand(o =>
        {
            var deleteStashPromptResult = MessageBox.Show($"Are you sure you want to delete the stash? {Environment.NewLine}All stashed changes also will be deleted!",
                        "Delete stash", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (deleteStashPromptResult == MessageBoxResult.Yes)
            {
                _gitService.DeleteStash(Id);
            }
        });

        /// <summary>
        /// Open stash info.
        /// </summary>
        public ICommand OpenStashInfoCommand => new DelegateCommand(o =>
        {
            var stashInfo = _gitService.GetStashInfo(Id);

            if (stashInfo != null)
            {
                _stash.ChangedFiles = stashInfo.ChangedFiles;
                var stashContext = new StashNavigationContext { Stash = _stash, NavigatedDirectly = true };
                var page = _teamExplorer.NavigateToPage(new Guid(Constants.StashInfoPageId), stashContext) as StashInfoTeamExplorerPage;
                if (page != null && page.StashId != _stash.Id)
                {
                    page.Initialize(this, new PageInitializeEventArgs(_serviceProvider, stashContext));
                }
            }
        });

        #endregion
    }
}
