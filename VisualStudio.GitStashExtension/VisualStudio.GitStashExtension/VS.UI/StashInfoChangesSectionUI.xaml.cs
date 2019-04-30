using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.VS.ViewModels;

namespace VisualStudio.GitStashExtension.VS.UI
{
    /// <summary>
    /// Interaction logic for StashInfoChangesSection.xaml
    /// </summary>
    public partial class StashInfoChangesSectionUI : UserControl
    {
        private readonly StashInfoChangesSectionViewModel _viewModel;

        public StashInfoChangesSectionUI(Stash stash, IServiceProvider serviceProvider)
        {
            InitializeComponent();

            DataContext = _viewModel = new StashInfoChangesSectionViewModel(stash, serviceProvider);
        }

        private void PreviewMouseWheelForTreeView(object sender, MouseWheelEventArgs e)
        {
            if (e.Handled)
                return;

            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent,
                Source = sender
            };
            var parent = (sender as Control)?.Parent as UIElement;
            parent?.RaiseEvent(eventArg);
        }
    }
}
