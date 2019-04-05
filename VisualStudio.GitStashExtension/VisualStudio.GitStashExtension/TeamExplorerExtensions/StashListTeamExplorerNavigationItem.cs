using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Controls;
using System.Drawing;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using VisualStudio.GitStashExtension.Extensions;
using System.Windows.Threading;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    /// <summary>
    /// Git stash navigation item (redirects user to the Git stash page <see cref="StashListTeamExplorerPage"/>).
    /// </summary>
    [TeamExplorerNavigationItem(Constants.StashNavigationItemId, 1000, TargetPageId = Constants.StashPageId)]
    public class StashListTeamExplorerNavigationItem : TeamExplorerBase, ITeamExplorerNavigationItem2
    {
        private readonly ITeamExplorer _teamExplorer;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGitExt _gitService;

        [ImportingConstructor]
        public StashListTeamExplorerNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _teamExplorer = _serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
            _gitService = _serviceProvider.GetService(typeof(IGitExt)) as IGitExt;
            Image = Resources.TeamExplorerIcon;

            IsVisible = _gitService.AnyActiveRepository();
            _gitService.PropertyChanged += GitServicePropertyChanged;
        }

        #region Navigation item properties
        public string Text => Constants.StashesLabel;

        public Image Image { get; }

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set => SetPropertyValue(value, ref _isVisible);
        }

        public bool IsEnabled => true;

        public int ArgbColor => Constants.NavigationItemColorArgbBit;

        public object Icon => null;
        #endregion

        #region Public methods
        public void Dispose()
        {
        }

        public void Execute()
        {
            _teamExplorer.NavigateToPage(new Guid(Constants.StashPageId), null);
        }

        public void Invalidate()
        {
        }
        #endregion

        #region Private methods
        private void GitServicePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var previousValue = IsVisible;
            IsVisible = propertyChangedEventArgs.PropertyName == nameof(_gitService.ActiveRepositories) &&
                        _gitService.AnyActiveRepository();

            // Refresh page only if Stash navigation become visible.
            if (!previousValue && IsVisible)
            {
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    if (_teamExplorer.CurrentPage.GetId() == new Guid(Constants.HomePageId))
                        _teamExplorer.CurrentPage.Refresh();
                });
            }
        }
        #endregion
    }
}
