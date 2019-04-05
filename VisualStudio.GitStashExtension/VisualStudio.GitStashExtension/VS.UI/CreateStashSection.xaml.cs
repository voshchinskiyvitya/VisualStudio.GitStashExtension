using System;
using System.Windows.Controls;
using VisualStudio.GitStashExtension.VS.ViewModels;

namespace VisualStudio.GitStashExtension.VS.UI
{
    /// <summary>
    /// Interaction logic for CreateStashSection.xaml
    /// </summary>
    public partial class CreateStashSection : UserControl
    {
        private readonly CreateStashSectionViewModel _viewModel;

        public CreateStashSection(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            DataContext = _viewModel = new CreateStashSectionViewModel(serviceProvider);
        }
    }
}
