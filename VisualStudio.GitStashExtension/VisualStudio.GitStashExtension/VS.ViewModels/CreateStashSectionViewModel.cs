using Microsoft.TeamFoundation.Controls;
using System;
using EnvDTE;
using VisualStudio.GitStashExtension.GitHelpers;
using System.Windows.Input;
using VisualStudio.GitStashExtension.VS.UI.Commands;

namespace VisualStudio.GitStashExtension.VS.ViewModels
{
    public class CreateStashSectionViewModel : NotifyPropertyChangeBase
    {
        private readonly ITeamExplorer _teamExplorer;
        private readonly IServiceProvider _serviceProvider;
        private readonly GitCommandExecuter _gitCommandExecuter;
        private readonly DTE _dte;

        public CreateStashSectionViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _teamExplorer = _serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
            _dte = _serviceProvider.GetService(typeof(DTE)) as DTE;
            _gitCommandExecuter = new GitCommandExecuter(serviceProvider);

            CreateStashCommand = new DelegateCommand(o => CreateStash());
        }

        #region View Model properties
        /// <summary>
        /// The message that should be assigned to the stash.
        /// </summary>
        private string _message;
        public string Message
        {
            get => _message;
            set => SetPropertyValue(value, ref _message);
        }

        /// <summary>
        /// Flag indicates whether or not stash should include untracked files.
        /// </summary>
        private bool _includeUntrackedFiles;
        public bool IncludeUntrackedFiles
        {
            get => _includeUntrackedFiles;
            set => SetPropertyValue(value, ref _includeUntrackedFiles);
        }

        /// <summary>
        /// Command that executes Create stash operation.
        /// </summary>
        public ICommand CreateStashCommand { get; }
        #endregion

        #region Private methods
        private void CreateStash()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            var result = _dte.ItemOperations.PromptToSave;
            if (_gitCommandExecuter.TryCreateStash(_message, _includeUntrackedFiles, out var errorMessage))
            {
                _teamExplorer.CurrentPage.RefreshPageAndSections();
                Message = string.Empty;
                IncludeUntrackedFiles = false;
            }
            else
            {
                _teamExplorer?.ShowNotification(errorMessage, NotificationType.Error, NotificationFlags.None, null, Guid.NewGuid());
            }
        }
        #endregion
    }
}
