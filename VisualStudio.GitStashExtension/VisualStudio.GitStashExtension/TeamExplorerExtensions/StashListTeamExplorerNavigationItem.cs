using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Controls;
using System.Drawing;
using System.Runtime.CompilerServices;
using VisualStudio.GitStashExtension.Annotations;
using Microsoft.VisualStudio.Shell;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    [TeamExplorerNavigationItem(Constants.StashNavigationItemId, 1000, TargetPageId = Constants.StashPageId)]
    public class StashListTeamExplorerNavigationItem : ITeamExplorerNavigationItem2
    {
        private readonly ITeamExplorer _teamExplorer;
        private readonly IServiceProvider _serviceProvider;

        [ImportingConstructor]
        public StashListTeamExplorerNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _teamExplorer = _serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
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
        public Image Image => null;
        public bool IsVisible => true;
        public bool IsEnabled => true;
        public int ArgbColor => Constants.NavigationItemColorArgb;
        public object Icon => null;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
