using System;
using System.Windows.Controls;
using Microsoft.TeamFoundation.Controls;
using VisualStudio.GitStashExtension.GitHelpers;

namespace VisualStudio.GitStashExtension.VS.UI
{
    /// <summary>
    /// Interaction logic for StashTeamExplorerPageUI.xaml
    /// </summary>
    public partial class StashListTeamExplorerPageUI : UserControl
    {
        private readonly StashListPageViewModel _viewModel;

        public StashListTeamExplorerPageUI(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            DataContext = _viewModel = new StashListPageViewModel(serviceProvider);
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.UpdateStashList(SearchText.Text);
        }
    }
}
