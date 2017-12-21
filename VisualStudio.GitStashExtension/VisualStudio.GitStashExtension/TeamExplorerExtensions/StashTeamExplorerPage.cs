using System;
using System.ComponentModel;
using Microsoft.TeamFoundation.Controls;
using VisualStudio.GitStashExtension.VS.UI;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    [TeamExplorerPage(Constants.StashPageId)]
    public class StashTeamExplorerPage: ITeamExplorerPage
    {
        public StashTeamExplorerPage()
        {
            PageContent = new StashTeamExplorerPageUI();
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
            throw new NotImplementedException();
        }

        public string Title => Constants.StashesLabel;
        public object PageContent { get; }
        public bool IsBusy { get; }
    }
}
