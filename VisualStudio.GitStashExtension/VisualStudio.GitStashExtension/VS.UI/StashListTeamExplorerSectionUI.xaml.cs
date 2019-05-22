using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.TeamFoundation.Controls;
using VisualStudio.GitStashExtension.VS.ViewModels;

namespace VisualStudio.GitStashExtension.VS.UI
{
    /// <summary>
    /// Interaction logic for StashTeamExplorerSectionUI.xaml
    /// </summary>
    public partial class StashListTeamExplorerSectionUI : UserControl
    {
        private readonly StashListSectionViewModel _viewModel;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITeamExplorer _teamExplorer;

        public StashListTeamExplorerSectionUI(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            _teamExplorer = _serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
            DataContext = _viewModel = new StashListSectionViewModel(serviceProvider);
        }

        public void Refresh()
        {
            _viewModel.SearchText = string.Empty;
        }

        private void PreviewMouseWheelForListView(object sender, MouseWheelEventArgs e)
        {
            if (e.Handled)
                return;

            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent,
                Source = sender
            };
            var parent = ((Control)sender).Parent as UIElement;
            parent?.RaiseEvent(eventArg);
        }
    }
}
