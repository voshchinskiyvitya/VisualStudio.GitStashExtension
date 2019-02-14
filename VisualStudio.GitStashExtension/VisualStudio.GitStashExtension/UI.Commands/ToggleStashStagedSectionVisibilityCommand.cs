using Microsoft.TeamFoundation.Controls;
using System;
using System.Windows.Input;

namespace VisualStudio.GitStashExtension.UI.Commands
{
    /// <summary>
    /// Command fot toggling stash staged section visibility.
    /// </summary>
    public class ToggleStashStagedSectionVisibilityCommand : ICommand
    {
        private readonly ITeamExplorer _teamExplorer;

        public ToggleStashStagedSectionVisibilityCommand(ITeamExplorer teamExplorer)
        {
            _teamExplorer = teamExplorer;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var stashStagedSection = _teamExplorer?.CurrentPage?.GetSection(new Guid(Constants.StashStagedChangesSectionId));
            if (stashStagedSection != null)
            {
                stashStagedSection.IsVisible = !stashStagedSection.IsVisible;
            }
        }
    }
}
