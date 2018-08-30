using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisualStudio.GitStashExtension.VS.UI
{
    /// <summary>
    /// Логика взаимодействия для CreateStashSection.xaml
    /// </summary>
    public partial class CreateStashSection : UserControl
    {
        private readonly CreateStashSetionViewModel _viewModel;

        public CreateStashSection(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            DataContext = _viewModel = new CreateStashSetionViewModel(serviceProvider);
        }

        private void CreateStashButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CreateStash();
        }
    }
}
