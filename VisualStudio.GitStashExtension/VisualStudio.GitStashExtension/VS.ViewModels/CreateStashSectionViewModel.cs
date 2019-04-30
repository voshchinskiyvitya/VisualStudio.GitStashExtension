using System;
using System.Windows.Input;
using VisualStudio.GitStashExtension.VS.UI.Commands;
using VisualStudio.GitStashExtension.Services;

namespace VisualStudio.GitStashExtension.VS.ViewModels
{
    public class CreateStashSectionViewModel : NotifyPropertyChangeBase
    {
        private readonly VisualStudioGitService _gitService;

        public CreateStashSectionViewModel(IServiceProvider serviceProvider)
        {           
            _gitService = new VisualStudioGitService(serviceProvider);

            CreateStashCommand = new DelegateCommand(o => {
                Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

                if (_gitService.TryCreateStash(Message, IncludeUntrackedFiles))
                {
                    Message = string.Empty;
                    IncludeUntrackedFiles = false;
                }
            });
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
    }
}
