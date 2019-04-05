using System;
using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.VS.UI;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    /// <summary>
    /// Section that contains information about Stash (name, id, branch, etc.).
    /// </summary>
    [TeamExplorerSection(Constants.StashInfoChangesSectionId, Constants.StashInfoPageId, 100)]
    public class StashInfoChangesSection: TeamExplorerBase, ITeamExplorerSection
    {
        private readonly IServiceProvider _serviceProvider;

        [ImportingConstructor]
        public StashInfoChangesSection([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #region Section properties
        public string Title => Constants.StashesInfoChangesSectionLabel;

        public object SectionContent { get; private set; }

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
        public void Initialize(object sender, SectionInitializeEventArgs e)
        {
        }

        public void Loaded(object sender, SectionLoadedEventArgs e)
        {
        }

        public void SaveContext(object sender, SectionSaveContextEventArgs e)
        {
            SectionContent = new StashInfoChangesSectionUI(e.Context as Stash, _serviceProvider);
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
