using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using VisualStudio.GitStashExtension.VS.UI;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    /// <summary>
    /// Stash list Team explorer page.
    /// </summary>
    [TeamExplorerPage(Constants.StashPageId, Undockable = true, MultiInstances = false)]
    public class StashListTeamExplorerPage : TeamExplorerBase, ITeamExplorerPage
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CreateStashSection _pageContent;

        [ImportingConstructor]
        public StashListTeamExplorerPage([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            PageContent = _pageContent = new CreateStashSection(_serviceProvider);            
        }

        #region Page properties
        public string Title => Constants.StashesLabel;

        public object PageContent { get; }

        public bool IsBusy { get; }
        #endregion

        #region Page public methods
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

        public void Dispose()
        {
        }
        #endregion
    }
}
