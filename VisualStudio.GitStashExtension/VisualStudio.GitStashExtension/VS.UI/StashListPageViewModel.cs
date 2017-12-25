using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VisualStudio.GitStashExtension.Annotations;
using VisualStudio.GitStashExtension.Models;

namespace VisualStudio.GitStashExtension.VS.UI
{
    public class StashListPageViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<Stash> _stashList = new ObservableCollection<Stash>();
        public ObservableCollection<Stash> Stashes
        {
            get => _stashList;
            set
            {
                OnPropertyChanged();
                _stashList = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
