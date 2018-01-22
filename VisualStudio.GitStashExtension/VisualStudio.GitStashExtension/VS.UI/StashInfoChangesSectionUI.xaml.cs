using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisualStudio.GitStashExtension.Models;

namespace VisualStudio.GitStashExtension.VS.UI
{
    /// <summary>
    /// Interaction logic for StashInfoChangesSection.xaml
    /// </summary>
    public partial class StashInfoChangesSectionUI : UserControl
    {
        private readonly IServiceProvider _serviceProvider;

        public StashInfoChangesSectionUI(Stash stash, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();

            DataContext = new StashInfoChangesSectionViewModel(stash, serviceProvider);
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
            var parent = ((Control) sender).Parent as UIElement;
            parent?.RaiseEvent(eventArg);
        }
    }
}
