using VisualStudio.GitStashExtension.Models;

namespace VisualStudio.GitStashExtension.VS.ViewModels
{
    public class StashInfoPageViewModel: NotifyPropertyChangeBase
    {
        public StashInfoPageViewModel(Stash stash)
        {
            Stash = stash;
        }

        private Stash _stash;
        public Stash Stash
        {
            get => _stash;
            set => SetPropertyValue(value, ref _stash);
        }
    }
}
