﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using VisualStudio.GitStashExtension.Models;
using VisualStudio.GitStashExtension.Services;

namespace VisualStudio.GitStashExtension.VS.ViewModels
{
    public class TreeViewItemWithIconViewModel: NotifyPropertyChangeBase
    {
        private readonly FilesTreeViewItem _internalModel;
        private readonly FileIconsService _fileIconService;

        public TreeViewItemWithIconViewModel(FilesTreeViewItem model, FileIconsService fileIconService)
        {
            _internalModel = model;
            _fileIconService = fileIconService;

            Source = IsFile ?
                     _fileIconService.GetFileIcon("." + FileExtension) :
                     _fileIconService.GetFolderIcon(IsExpanded);

            var childNodes = model.Items.Select(m => new TreeViewItemWithIconViewModel(m, fileIconService)).ToList();
            Items = new ObservableCollection<TreeViewItemWithIconViewModel>(childNodes);
        }

        #region View Model properties
        /// <summary>
        /// Icon source for file or folder.
        /// </summary>
        private BitmapSource _source;
        public BitmapSource Source
        {
            get => _source;
            set => SetPropertyValue(value, ref _source);
        }

        /// <summary>
        /// Flag indicates whether or not tree item is expanded.
        /// </summary>
        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetPropertyValue(value, ref _isExpanded, () => 
            {
                if (!IsFile)
                {
                    Source = _fileIconService.GetFolderIcon(IsExpanded);
                }
            });
        }

        /// <summary>
        /// Child items.
        /// </summary>
        public ObservableCollection<TreeViewItemWithIconViewModel> Items { get; }
        #endregion

        #region Properties
        /// <summary>
        /// File or folder name for treeview.
        /// </summary>
        public string Text => _internalModel.Text;

        /// <summary>
        /// Full path for current file or folder.
        /// </summary>
        public string FullPath => _internalModel.FullPath;

        /// <summary>
        /// Indicates whether current item is file.
        /// </summary>
        public bool IsFile => _internalModel.IsFile;

        /// <summary>
        /// Flag indicates whether this file is new (untracked) or not.
        /// </summary>
        public bool? IsNew => _internalModel.IsNew;

        /// <summary>
        /// Flag indicates whether this file is new (untracked) and staged or not.
        /// </summary>
        public bool? IsStaged => _internalModel.IsStaged;

        /// <summary>
        /// File extension (.cs, .txt, etc.).
        /// </summary>
        public string FileExtension => _internalModel.FileExtension;

        /// <summary>
        /// Context menu header text for file comparing/opening.
        /// </summary>
        public string ContextMenuText => IsNew ?? false ? "Open" : "Compare with previous";
        #endregion
    }
}
