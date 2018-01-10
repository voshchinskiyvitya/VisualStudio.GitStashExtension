using System;
using System.ComponentModel;
using Microsoft.TeamFoundation.Controls;
using VisualStudio.GitStashExtension.VS.UI;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    [TeamExplorerPage(Constants.StashInfoPageId)]
    public class StashInfoTeamExplorerPage: ITeamExplorerPage
    {
        private readonly object _pageContent;

        public StashInfoTeamExplorerPage()
        {
            _pageContent = new StashInfoPage();
        }

        public void Dispose()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void Initialize(object sender, PageInitializeEventArgs e)
        {
        }

        public void Loaded(object sender, PageLoadedEventArgs e)
        {
        }

        public void SaveContext(object sender, PageSaveContextEventArgs e)
        {
        }

        public void Refresh()
        {
        }

        public void Cancel()
        {
        }

        public object GetExtensibilityService(Type serviceType)
        {
            return null;
        }

        public string Title => Constants.StashesInfoLabel;
        public object PageContent => _pageContent;
        public bool IsBusy { get; }
    }
}
