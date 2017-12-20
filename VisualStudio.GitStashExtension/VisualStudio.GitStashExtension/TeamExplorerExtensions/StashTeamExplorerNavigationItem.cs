using System.ComponentModel;
using Microsoft.TeamFoundation.Controls;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using VisualStudio.GitStashExtension.Annotations;

namespace VisualStudio.GitStashExtension.TeamExplorerExtensions
{
    [TeamExplorerNavigationItem("350FF356-5C4E-4861-B4C7-9CC97438F31F", 1000, TargetPageId = "1F9974CD-16C3-4AEF-AED2-0CE37988E2F1")]
    public class StashTeamExplorerNavigationItem: ITeamExplorerNavigationItem2
    {
        public void Dispose()
        {
        }

        public void Execute()
        {
            MessageBox.Show("Test message");
        }

        public void Invalidate()
        {
        }

        public string Text => "Stashes";
        public Image Image => null;
        public bool IsVisible => true;
        public bool IsEnabled => true;
        public int ArgbColor => 51212; // green
        public object Icon => null;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
