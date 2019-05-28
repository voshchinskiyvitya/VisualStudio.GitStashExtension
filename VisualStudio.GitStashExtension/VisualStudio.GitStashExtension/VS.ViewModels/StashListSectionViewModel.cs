using Microsoft.TeamFoundation.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VisualStudio.GitStashExtension.Extensions;
using VisualStudio.GitStashExtension.GitHelpers;
using VisualStudio.GitStashExtension.Helpers;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.Services;
using VisualStudio.GitStashExtension.VSHelpers;

namespace VisualStudio.GitStashExtension.VS.ViewModels
{
    public class StashListSectionViewModel : NotifyPropertyChangeBase
    {
        private readonly ITeamExplorer _teamExplorer;
        private readonly IServiceProvider _serviceProvider;
        private readonly GitCommandExecuter _gitCommandExecuter;
        private readonly VisualStudioGitService _gitService;

        public StashListSectionViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _teamExplorer = _serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
            _gitCommandExecuter = new GitCommandExecuter(serviceProvider);
            _gitService = new VisualStudioGitService(_serviceProvider);

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
        private ObservableCollection<StashListItemViewModel> _stashList = new ObservableCollection<StashListItemViewModel>();
        public ObservableCollection<StashListItemViewModel> Stashes
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

        #region Private methods
        /// <summary>
        /// Updates stash list content based on search text.
        /// </summary>
        /// <param name="searchText">Search text.</param>
        private void UpdateStashList(string searchText)
        {
            Task.Run(() =>
            {
                Stashes = _gitService
                    .GetAllStashes(searchText)
                    .Select(s => new StashListItemViewModel(s, _serviceProvider))
                    .ToObservableCollection();             
            });
        }        
        #endregion
    }
}
