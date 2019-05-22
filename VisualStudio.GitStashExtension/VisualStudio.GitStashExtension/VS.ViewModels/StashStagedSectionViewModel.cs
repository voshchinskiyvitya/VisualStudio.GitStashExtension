using Microsoft.TeamFoundation.Controls;
using System;
using System.Windows.Input;
using VisualStudio.GitStashExtension.Services;
using VisualStudio.GitStashExtension.VS.UI.Commands;

namespace VisualStudio.GitStashExtension.VS.ViewModels
{
    public class StashStagedSectionViewModel : NotifyPropertyChangeBase
    {
        private readonly ITeamExplorer _teamExplorer;
        private readonly IServiceProvider _serviceProvider;
        private readonly VisualStudioGitService _gitService;
        private readonly ITeamExplorerSection _section;

        public StashStagedSectionViewModel(IServiceProvider serviceProvider, ITeamExplorerSection section)
        {
            _serviceProvider = serviceProvider;
            _section = section;
            _teamExplorer = _serviceProvider.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
            _gitService = new VisualStudioGitService(serviceProvider);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetPropertyValue(value, ref _message);
        }

        /// <summary>
        /// Stash staged command.
        /// </summary>
        public ICommand StashStagedCommand => new DelegateCommand(o =>
        {
            if (_gitService.TryCreateStashStaged(Message))
            {
                Message = string.Empty;
            }
        });

        /// <summary>
        /// Cancel stash staged command.
        /// </summary>
        public ICommand CancelCommand => new DelegateCommand(o =>
        {
            Message = string.Empty;
            _section.Cancel();
        });
    }
}
