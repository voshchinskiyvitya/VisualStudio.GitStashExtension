using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Controls;
using System.Drawing;
using System.Runtime.CompilerServices;
using VisualStudio.GitStashExtension.Annotations;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using VisualStudio.GitStashExtension.Extensions;
using System.Windows.Threading;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    [TeamExplorerNavigationItem(Constants.StashNavigationItemId, 1000, TargetPageId = Constants.StashPageId)]
    public class StashListTeamExplorerNavigationItem : ITeamExplorerNavigationItem2
    {
        private readonly ITeamExplorer _teamExplorer;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGitExt _gitService;

        [ImportingConstructor]
        public StashListTeamExplorerNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _teamExplorer = _serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
            _image = Resources.TeamExplorerIcon;
            _gitService = (IGitExt)_serviceProvider.GetService(typeof(IGitExt));

            IsVisible = _gitService.AnyActiveRepository();
            _gitService.PropertyChanged += GitServicePropertyChanged;
        }

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

        public string Text => Constants.StashesLabel;

        private Image _image;
        private bool _isVisible;
        public Image Image => _image;

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled => true;
        public int ArgbColor => BitConverter.ToInt32(
            new[] {
                Constants.NavigationItemColorArgb.B,
                Constants.NavigationItemColorArgb.G,
                Constants.NavigationItemColorArgb.R,
                Constants.NavigationItemColorArgb.A,
            }, 0);

        public object Icon => null;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
    }
}
