using System.ComponentModel;
using System.Runtime.CompilerServices;
using VisualStudio.GitStashExtension.Annotations;
using VisualStudio.GitStashExtension.Models;

namespace VisualStudio.GitStashExtension.VS.ViewModels
{
    public class StashInfoPageViewModel: INotifyPropertyChanged
    {
        private Stash _stash;
        public event PropertyChangedEventHandler PropertyChanged;

        public StashInfoPageViewModel(Stash stash)
        {
            Stash = stash;
        }

        public Stash Stash
        {
            get => _stash;
            set
            {
                _stash = value;
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
