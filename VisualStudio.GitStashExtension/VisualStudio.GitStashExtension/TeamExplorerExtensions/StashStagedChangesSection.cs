using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer.Framework;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using VisualStudio.GitStashExtension.Commands;
using VisualStudio.GitStashExtension.VS.UI;
using VisualStudio.GitStashExtension.Extensions;
using VisualStudio.GitStashExtension.UI.Commands;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    [TeamExplorerSection(Constants.StashStagedChangesSectionId, TeamExplorerPageIds.GitChanges, 1)]
    public class StashStagedChangesSection : ITeamExplorerSection
    {
        private readonly ITeamExplorer _teamExplorer;

        [ImportingConstructor]
        public StashStagedChangesSection([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            SectionContent = new StashStagedSection(serviceProvider, this);
            _teamExplorer = serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
        }

        public string Title => "";

        public object SectionContent { get; }

        private bool _isVisible = false;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsVisible)));
            }
        }

        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded)));
            }
        }

        public bool IsBusy => false;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Cancel()
        {
            IsVisible = false;
        }

        public void Dispose()
        {
        }

        public object GetExtensibilityService(Type serviceType)
        {
            return null;
        }

        public void Initialize(object sender, SectionInitializeEventArgs e)
        {
        }

        private bool StashStagedLinkInitialized = false;

        public void Loaded(object sender, SectionLoadedEventArgs e)
        {
            if (_teamExplorer.CurrentPage.GetId() != new Guid(TeamExplorerPageIds.GitChanges) && !StashStagedLinkInitialized)
            {
                return;
            }

            InitStashStagedChangesPageLink();
            HideStashStagedSectionExpanderLink();
        }

        public void Refresh()
        {
        }

        public void SaveContext(object sender, SectionSaveContextEventArgs e)
        {
        }

        /// <summary>
        /// The approach used below is not appropriate, because I've implemented direct WPF tree code modification.
        /// This code won't work if any changes to TeamExplorer page structure will be applied by Visual Studio development team.
        /// But there is no way to extend/modify current page/section layout, so for now and for better usability current code will be here.
        /// Also, I've implemented the second way to open stash staged section (see <see cref="StashStagedCommand"/>), so the same operation can be executed using another approach 
        /// (in the case if something is changed or error happened).
        /// </summary>
        #region Page modification methods
        private void InitStashStagedChangesPageLink()
        {
            var currentPage = _teamExplorer.CurrentPage.PageContent as UserControl;

            if (currentPage == null)
                return;

            var actionsLink = currentPage?.FindName("actionsLink") as TextBlock;
            var linksPanel = actionsLink?.Parent as Panel;

            if (linksPanel != null)
            {
                var stashStagedButton = new DropDownLink
                {
                    Text = "Stash staged",
                    Margin = new Thickness(10, 0, 0, 0),
                    DropDownMenuCommand = new ToggleStashStagedSectionVisibilityCommand(_teamExplorer)
                };

                linksPanel.Children.Add(stashStagedButton);
                StashStagedLinkInitialized = true;
            }
        }

        private void HideStashStagedSectionExpanderLink()
        {
            var section = (SectionContent as UserControl).FindParentByType<SectionControl>();
            if (section != null)
            {
                section.ExpanderButtonVisibility = Visibility.Collapsed;
            }
        }
        #endregion
    }
}
