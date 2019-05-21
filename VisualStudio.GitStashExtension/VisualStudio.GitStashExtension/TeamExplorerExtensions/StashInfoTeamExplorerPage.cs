using System;
using Microsoft.TeamFoundation.Controls;
using VisualStudio.GitStashExtension.Helpers;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.VS.UI;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    /// <summary>
    /// Stash information Team explorer page.
    /// </summary>
    [TeamExplorerPage(Constants.StashInfoPageId, Undockable = true, MultiInstances = false)]
    public class StashInfoTeamExplorerPage : TeamExplorerBase, ITeamExplorerPage
    {
        private Stash _stashInfo;
        private object _pageContent;

        #region Page properties
        public string Title => Constants.StashesInfoLabel;


        public object PageContent
        {
            get => _pageContent;
            private set => SetPropertyValue(value, ref _pageContent);
        }

        public bool IsBusy { get; }

        /// <summary>
        /// This property can be used to track that page properly initialized.
        /// </summary>
        /// <remarks>
        /// There is a inicialization issue for undocked page, when new Stash Ingo data is not loaded correctly, so we should manually trigger <see cref="Initialize"/> method.
        /// </remarks>
        public int? StashId => _stashInfo?.Id;
        #endregion

        #region Public methods
        public void Initialize(object sender, PageInitializeEventArgs e)
        {
            var context = e.Context as StashNavigationContext;
            _stashInfo = !context.NavigatedDirectly && context.Stash != null && RemovedStashesContainer.Contains(context.Stash.Id) ? null : context.Stash;
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
            e.Context = new StashNavigationContext { Stash = _stashInfo };
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
