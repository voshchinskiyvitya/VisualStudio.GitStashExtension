using System;
using System.ComponentModel;
using Microsoft.TeamFoundation.Controls;
using VisualStudio.GitStashExtension.VS.UI;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    [TeamExplorerSection(Constants.StashInfoChangesSectionId, Constants.StashInfoPageId, 100)]
    public class StashInfoChangesSection: ITeamExplorerSection
    {
        private object _sectionContent;

        public StashInfoChangesSection()
        {
            _sectionContent = new StashInfoChangesSectionUI();
        }

        public void Dispose()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void Initialize(object sender, SectionInitializeEventArgs e)
        {
        }

        public void Loaded(object sender, SectionLoadedEventArgs e)
        {
        }

        public void SaveContext(object sender, SectionSaveContextEventArgs e)
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
            return true;
        }

        public string Title => Constants.StashesInfoChangesSectionLabel;
        public object SectionContent => _sectionContent;

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
    }
}
