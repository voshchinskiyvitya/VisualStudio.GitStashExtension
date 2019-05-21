using Microsoft.TeamFoundation.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VisualStudio.GitStashExtension.Annotations;
using VisualStudio.GitStashExtension.Extensions;
using VisualStudio.GitStashExtension.GitHelpers;
using VisualStudio.GitStashExtension.Helpers;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.VS.UI.Commands;
using VisualStudio.GitStashExtension.VSHelpers;

namespace VisualStudio.GitStashExtension.VS.ViewModels
{
    public class StashListSectionViewModel : NotifyPropertyChangeBase
    {
        private readonly ITeamExplorer _teamExplorer;
        private readonly IServiceProvider _serviceProvider;
        private readonly GitCommandExecuter _gitCommandExecuter;

        public StashListSectionViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _teamExplorer = _serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
            _gitCommandExecuter = new GitCommandExecuter(serviceProvider);

            UpdateStashList(string.Empty);
            RemovedStashesContainer.ResetContainer();

            PropertyChanged += (e, s) =>
            {
                if (s.PropertyName == nameof(SearchText))
                {
                    UpdateStashList(SearchText);
                }
            };
        }

        #region View Model properties
        private ObservableCollection<Stash> _stashList = new ObservableCollection<Stash>();
        public ObservableCollection<Stash> Stashes
        {
            get => _stashList;
            set => SetPropertyValue(value, ref _stashList);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetPropertyValue(value, ref _searchText);
        }

        public ImageSource SearchIconSource
        {
            get
            {
                var theme = VisualStudioThemeHelper.GetCurrentTheme();

                var path = theme == VsColorTheme.Dark ?
                        @"pack://application:,,,/VisualStudio.GitStashExtension;component/Resources/SearchIcon_white.png" :
                        @"pack://application:,,,/VisualStudio.GitStashExtension;component/Resources/SearchIcon.png";

                var uriSource = new Uri(path, UriKind.Absolute);
                return new BitmapImage(uriSource);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Applies stash to current repository state.
        /// </summary>
        public ICommand ApplyStashCommand => new DelegateCommand(o => ApplyStash((int)o));

        /// <summary>
        /// Pops (applies and removes) stash.
        /// </summary>
        public ICommand PopStashCommand => new DelegateCommand(o => PopStash((int)o));

        /// <summary>
        /// Removes stash.
        /// </summary>
        public ICommand DeleteStashCommand => new DelegateCommand(o => DeleteStash((int)o));

        #endregion

        #region Private methods
        /// <summary>
        /// Updates stash list content based on search text.
        /// </summary>
        /// <param name="searchText">Search text.</param>
        private void UpdateStashList(string searchText)
        {
            Task.Run(() =>
            {
                if (_gitCommandExecuter.TryGetAllStashes(out var stashes, out var error))
                {
                    Stashes = stashes.Where(s => string.IsNullOrEmpty(searchText) || s.Message.Contains(searchText)).ToObservableCollection();
                }
                else
                {
                    _teamExplorer?.ShowNotification(error, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
                }
            });
        }

        /// <summary>
        /// Applies stash to current repository state.
        /// </summary>
        /// <param name="stashId">Stash Id.</param>
        private void ApplyStash(int stashId)
        {
            if (_gitCommandExecuter.TryApplyStash(stashId, out var errorMessage))
            {
                _teamExplorer.NavigateToPage(new Guid(TeamExplorerPageIds.GitChanges), null);
            }
            else
            {
                _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
            }
        }

        /// <summary>
        /// Pops (applies and removes) stash by id.
        /// </summary>
        /// <param name="stashId">Stash Id.</param>
        private void PopStash(int stashId)
        {
            if (_gitCommandExecuter.TryPopStash(stashId, out var errorMessage))
            {
                _teamExplorer.NavigateToPage(new Guid(TeamExplorerPageIds.GitChanges), null);
                RemovedStashesContainer.AddDeletedStash(stashId);
            }
            else
            {
                _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
            }
        }

        /// <summary>
        /// Removes stash by id.
        /// </summary>
        /// <param name="stashId">Stash Id.</param>
        private void DeleteStash(int stashId)
        {
            if (_gitCommandExecuter.TryDeleteStash(stashId, out var errorMessage))
            {
                _teamExplorer.CurrentPage.RefreshPageAndSections();
                RemovedStashesContainer.AddDeletedStash(stashId);
            }
            else
            {
                _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
            }
        }

        /// <summary>
        /// Gets stash info by id.
        /// </summary>
        /// <param name="stashId">Stash Id.</param>
        public Stash GetStashInfo(int stashId)
        {
            if (!_gitCommandExecuter.TryGetStashInfo(stashId, out var stash, out var errorMessage))
            {
                _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
            }

            return stash;
        }
        #endregion
    }
}
