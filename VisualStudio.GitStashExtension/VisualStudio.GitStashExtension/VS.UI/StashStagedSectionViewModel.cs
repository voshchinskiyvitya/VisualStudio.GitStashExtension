using Microsoft.TeamFoundation.Controls;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VisualStudio.GitStashExtension.Annotations;
using VisualStudio.GitStashExtension.GitHelpers;

namespace VisualStudio.GitStashExtension.VS.UI
{
    public class StashStagedSectionViewModel : INotifyPropertyChanged
    {
        private readonly ITeamExplorer _teamExplorer;
        private readonly IServiceProvider _serviceProvider;
        private readonly GitCommandExecuter _gitCommandExecuter;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public StashStagedSectionViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _teamExplorer = _serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
            _gitCommandExecuter = new GitCommandExecuter(serviceProvider);
        }

        public void CreateStash()
        {
            //Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            //var result =_dte.ItemOperations.PromptToSave;
            //if (_gitCommandExecuter.TryCreateStash(_message, true, out var errorMessage))
            //{
            //    _teamExplorer.CurrentPage.RefreshPageAndSections();
            //    Message = string.Empty;
            //}
            //else
            //{
            //    _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
            //}
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
