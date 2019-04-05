using System;
using Microsoft.TeamFoundation.Controls;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.VS.UI;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    /// <summary>
    /// Stash information Team explorer page.
    /// </summary>
    [TeamExplorerPage(Constants.StashInfoPageId)]
    public class StashInfoTeamExplorerPage: TeamExplorerBase, ITeamExplorerPage
    {
        private Stash _stashInfo;

        #region Page properties
        public string Title => Constants.StashesInfoLabel;

        public object PageContent { get; private set; }

        public bool IsBusy { get; }
        #endregion

        #region Public methods
        public void Initialize(object sender, PageInitializeEventArgs e)
        {
            _stashInfo = e.Context as Stash;
            PageContent = new StashInfoPage(_stashInfo);

            var changesSection = this.GetSection(new Guid(Constants.StashInfoChangesSectionId));
            changesSection.SaveContext(this, new SectionSaveContextEventArgs
            {
                Context = _stashInfo
            });
        }

        public void Loaded(object sender, PageLoadedEventArgs e)
        {
        }

        public void SaveContext(object sender, PageSaveContextEventArgs e)
        {
            e.Context = _stashInfo;
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

        public void Dispose()
        {
        }
        #endregion
    }
}
