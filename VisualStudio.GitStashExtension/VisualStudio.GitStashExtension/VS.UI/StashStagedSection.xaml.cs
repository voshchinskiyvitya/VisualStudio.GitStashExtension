using Microsoft.TeamFoundation.Controls;
using System;
using System.Windows.Controls;
using VisualStudio.GitStashExtension.VS.ViewModels;

namespace VisualStudio.GitStashExtension.VS.UI
{
    public partial class StashStagedSection : UserControl
    {
        private readonly StashStagedSectionViewModel _viewModel;

        public StashStagedSection(IServiceProvider serviceProvider, ITeamExplorerSection currentSection)
        {
            InitializeComponent();

            DataContext = _viewModel = new StashStagedSectionViewModel(serviceProvider, currentSection);
        }
    }
}
