using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using VisualStudio.GitStashExtension.Annotations;

namespace VisualStudio.GitStashExtension.VS.UI
{
    public class StashInfoChangesSectionViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<TreeViewItem> _changeItems;
        public event PropertyChangedEventHandler PropertyChanged;

        public StashInfoChangesSectionViewModel()
        {
            
        }

        public ObservableCollection<TreeViewItem> ChangeItems
        {
            get => _changeItems;
            set
            {
                _changeItems = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
