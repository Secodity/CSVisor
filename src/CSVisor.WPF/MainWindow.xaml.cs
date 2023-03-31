using ControlzEx.Theming;
using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace CSVisor.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainWindowViewModel _ViewModel => DataContext as MainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ThemeManager.Current.ChangeTheme(this, "Dark.Orange");
        }

        private void __SelectFile(object sender, RoutedEventArgs e)
        {
            _ViewModel.SelectFile();
        }
    }
}
