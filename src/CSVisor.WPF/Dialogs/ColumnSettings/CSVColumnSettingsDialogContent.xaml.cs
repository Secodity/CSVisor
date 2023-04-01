using ControlzEx.Theming;
using CSVisor.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CSVisor.WPF.Dialogs.ColumnSettings
{
    /// <summary>
    /// Interaction logic for CSVColumnSettingsDialogContent.xaml
    /// </summary>
    public partial class CSVColumnSettingsDialogContent
    {
        public CSVColumnSettingsDialogContent()
        {
            InitializeComponent();
            DataContextChanged += (object sender, DependencyPropertyChangedEventArgs e) =>
            {
                if (DataContext is CSVColumnSettingsDialogViewModel vm)
                    vm.PreviewGrid = previewDataGrid;
            };
            ThemeManager.Current.ChangeTheme(this, "Dark.Orange");
        }

        private void __Remove(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CSVColumnSortingViewModel vm)
                vm.Remove();
        }
    }
}
