using System.Windows.Controls;
using VisualStudio.GitStashExtension.Models;

namespace VisualStudio.GitStashExtension.VS.UI
{
    /// <summary>
    /// Interaction logic for StashInfoPage.xaml
    /// </summary>
    public partial class StashInfoPage : UserControl
    {
        public StashInfoPage(Stash stash)
        {
            InitializeComponent();

            DataContext = new StashInfoPageViewModel(stash);
        }
    }
}
