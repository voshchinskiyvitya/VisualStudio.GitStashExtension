using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Composition;
using VisualStudio.GitStashExtension.VS.UI;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    /// <summary>
    /// Section that contains list of the created stashes.
    /// </summary>
    [TeamExplorerSection(Constants.StashListSectionId, Constants.StashPageId, 100)]
    public class GitStashListStashesSection : TeamExplorerBase, ITeamExplorerSection
    {
        private readonly StashListTeamExplorerSectionUI _sectionContent;

        [ImportingConstructor]
        public GitStashListStashesSection([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            SectionContent = _sectionContent = new StashListTeamExplorerSectionUI(serviceProvider);
        }

        #region Section properties
        public string Title => Constants.StashesListSectionLabel;

        public object SectionContent { get; }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible;
            set => SetPropertyValue(value, ref _isVisible);
        }

        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetPropertyValue(value, ref _isExpanded);
        }

        public bool IsBusy => false;
        #endregion

        #region Public methods
        public void Cancel()
        {
        }

        public object GetExtensibilityService(Type serviceType)
        {
            return null;
        }

        public void Initialize(object sender, SectionInitializeEventArgs e)
        {
        }

        public void Loaded(object sender, SectionLoadedEventArgs e)
        {
        }

        public void Refresh()
        {
            _sectionContent.Refresh();
        }

        public void SaveContext(object sender, SectionSaveContextEventArgs e)
        {
        }

        public void Dispose()
        {
        }
        #endregion
    }
}
