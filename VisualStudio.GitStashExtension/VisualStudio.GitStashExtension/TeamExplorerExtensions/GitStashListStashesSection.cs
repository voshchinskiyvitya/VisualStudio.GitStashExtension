using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualStudio.GitStashExtension.VS.UI;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    [TeamExplorerSection(Constants.StashListSectionId, Constants.StashPageId, 100)]
    public class GitStashListStashesSection : ITeamExplorerSection
    {
        private readonly StashListTeamExplorerSectionUI _sectionContent;

        [ImportingConstructor]
        public GitStashListStashesSection([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            SectionContent = _sectionContent = new StashListTeamExplorerSectionUI(serviceProvider);
        }

        public string Title => Constants.StashesListSectionLabel;

        public object SectionContent { get; }

        private bool _isVisible = true;
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
    }
}
