using Microsoft.TeamFoundation.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using VisualStudio.GitStashExtension.VS.ViewModels;

namespace VisualStudio.GitStashExtension.VS.UI
{
    public partial class StashStagedSection : UserControl
    {
        private readonly StashStagedSectionViewModel _viewModel;
        private readonly ITeamExplorerSection _section;

        public StashStagedSection(IServiceProvider serviceProvider, ITeamExplorerSection currentSection)
        {
            InitializeComponent();

            DataContext = _viewModel = new StashStagedSectionViewModel(serviceProvider);
            _section = currentSection;
        }

        private void CreateStashButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: add create staged stash 
            //_viewModel.CreateStash();
        }

        private void CloseSectionLink_Click(object sender, RoutedEventArgs e)
        {
            _section.Cancel();
        }
    }
}
