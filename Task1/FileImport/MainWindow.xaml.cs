using FileImport.ViewModels;
using FileImport.Services;
using System.Windows;


namespace FileImport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new FileDataVM(new FileDialogService());
        }
    }
}