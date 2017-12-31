using System;
using System.Windows;
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

        private void ApplyStashMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var stashId = menuItem.Tag as int?;

            if (stashId.HasValue)
            {
                _viewModel.ApplyStash(stashId.Value);
            }
        }

        private void ListItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var listItem = sender as TextBlock;
                var stashId = listItem.Tag as int?;

                if (stashId.HasValue)
                {
                    _viewModel.ApplyStash(stashId.Value);
                }
            }
        }
    }
}
