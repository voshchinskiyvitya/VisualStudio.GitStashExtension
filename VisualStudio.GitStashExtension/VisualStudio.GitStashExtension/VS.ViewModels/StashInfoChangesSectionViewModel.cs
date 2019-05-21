using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.VisualStudio.Shell.Interop;
using VisualStudio.GitStashExtension.Extensions;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.Services;
using VisualStudio.GitStashExtension.VS.UI.Commands;

namespace VisualStudio.GitStashExtension.VS.ViewModels
{
    public class StashInfoChangesSectionViewModel: NotifyPropertyChangeBase
    {
        private readonly Stash _stash;
        private readonly VisualStudioGitService _gitService;

        public StashInfoChangesSectionViewModel(Stash stash, IServiceProvider serviceProvider)
        {
            _stash = stash;
            _gitService = new VisualStudioGitService(serviceProvider);

            if (stash?.ChangedFiles == null)
                return;

            var vsImageService = serviceProvider.GetService(typeof(SVsImageService)) as IVsImageService2;
            var fileIconsService = new FileIconsService(vsImageService);

            var rootTreeViewItem = stash.ChangedFiles.ToTreeViewItemStructure();
            var rootViewModel = new TreeViewItemWithIconViewModel(rootTreeViewItem, stash.Id, fileIconsService, _gitService);

            ChangeItems = rootViewModel.Items;
        }

        /// <summary>
        /// Stash change items collection.
        /// </summary>
        public ObservableCollection<TreeViewItemWithIconViewModel> ChangeItems { get; }
    }
}
